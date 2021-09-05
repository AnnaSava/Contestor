using AutoMapper;
using Contestor.BpmEngine.Contract;
using Contestor.Data.Contract;
using Contestor.Data.Contract.Interfaces;
using Contestor.Data.Contract.Models;
using Contestor.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.Service.Services
{
    public class ContestService : IContestService
    {
        private readonly IContestDalService _contestDalService;
        private readonly IBpmEngineClient _bpmEngineService;
        private readonly IMapper _mapper;

        public ContestService(IContestDalService contestDalService, IBpmEngineClient bpmEngineService, IMapper mapper)
        {
            _contestDalService = contestDalService;
            _bpmEngineService = bpmEngineService;
            _mapper = mapper;
        }

        public async Task<ContestModel> Create(ContestModel model)
        {
            var process = await _bpmEngineService.GetProcessById(model.ProcessKey);
            model.ProcessName = process?.Name;
            model.AutoRegEnabled = true;

            return await _contestDalService.Create(model);
        }

        public async Task Open(long contestId)
        {            
            await _contestDalService.SetStatus(contestId, ContestStatus.Open);
        }

        public async Task OpenRegistration(long contestId)
        {
            //TODO может сделать UoW, чтобы один раз вызывать dbContext.SaveChanges? или один метод на уровне DAL?
            await _contestDalService.SetAutoRegEnabled(contestId, false);
            await _contestDalService.SetStatus(contestId, ContestStatus.RegistrationOpen);
        }

        public async Task StartVoting(long contestId)
        {
            await _contestDalService.SetStatus(contestId, ContestStatus.Voting);
        }

        public async Task WaitWinner(long contestId)
        {
            await _contestDalService.SetStatus(contestId, ContestStatus.WaitingWinner);
        }

        public async Task SetFinishedStatus(long contestId)
        {
            await _contestDalService.SetStatus(contestId, ContestStatus.Finished);
        }

        public async Task<ContestModel> GetOne(long id)
        {
            var model = await _contestDalService.GetOne(id);
            return model;
        }

        public async Task<ContestModel> GetOne(long id, long visitorId)
        {
            var model = await _contestDalService.GetOne(id, visitorId);
            return model;
        }

        public async Task<ContestManageViewModel> GetOneForManage(long id)
        {
            var model = await _contestDalService.GetOne(id);
            var vm = _mapper.Map<ContestManageViewModel>(model);

            var currentTasks = await _bpmEngineService.GetCurrentTasks(model.Id.ToString());
            vm.Tasks = currentTasks.ToList();
            return vm;
        }

        public async Task<IEnumerable<ContestModel>> GetAll(int page, int count)
        {
            return await _contestDalService.GetAll(page, count);
        }

        public async Task<IEnumerable<ContestModel>> GetAllForNewParticipants(long visitorId, int page, int count)
        {
            return await _contestDalService.GetAllForNewParticipants(visitorId, page, count);
        }

        public async Task<IEnumerable<ContestModel>> GetAllForVoting()
        {
            return await _contestDalService.GetAllForVoting();
        }

        public async Task<IEnumerable<ContestModel>> GetTopForVoting(long visitorId)
        {
            return await _contestDalService.GetTopForVoting(visitorId);
        }

        public async Task<Dictionary<string, string>> GetProcessesDictionary()
        {
            var businessProcesses = (await _bpmEngineService.GetProcessesLatestVersions()).ToList();
            return businessProcesses.ToDictionary(x => x.Id, x => x.Name);
        }

        public async Task<string> StartContestProcess(long contestId)
        {
            var contest = await _contestDalService.GetOne(contestId);

            var startProcessModel = new StartingProcessModel { ProcessId = contest.ProcessKey, BusinessKey = contest.Id.ToString() };

            await _contestDalService.SetStatus(contestId, ContestStatus.Started);

            try
            {
                var process = await _bpmEngineService.StartProcess(startProcessModel);

                return process;
            }
            catch(Exception ex)
            {
                await _contestDalService.SetStatus(contestId, ContestStatus.Draft);
            }

            return null;
        }

        public async Task RegisterParticipant(long contestId, long userId)
        {
            await _contestDalService.RegisterParticipant(contestId, userId);
        }

        public async Task<ParticipantModel> GetParticipant(long contestId, long userId)
        {
            return await _contestDalService.GetParticipant(contestId, userId);
        }

        public async Task<int> GetParticipantsCount(long contestId)
        {
            return await _contestDalService.GetParticipantsCount(contestId);
        }

        public async Task<int> GetParticipantsHavingWorkCount(long contestId)
        {
            return await _contestDalService.GetParticipantsHavingWorkCount(contestId);
        }

        public async Task SendWork(WorkModel model)
        {
            var contest = await _contestDalService.GetOne(model.ContestId);
            var participant = await _contestDalService.GetParticipant(model.ContestId, model.ParticipantId);
            if (participant == null && contest.AutoRegEnabled == false) throw new Exception($"Участник {model.ParticipantId} не зарегистрирован на конкурс {model.ContestId}");

            if (participant == null) await _contestDalService.RegisterParticipant(contest.Id, model.ParticipantId);

            model.RoundNumber = contest.RoundNumber;
            model.ParticipantId = model.ParticipantId;

            await _contestDalService.SendWork(model);
        }

        public async Task<bool> CompleteTask(CompletingTaskViewModel model)
        {
            var currentTask = await _bpmEngineService.GetTask(model.TaskId);

            var completingTask = new CompletingTaskModel
            {
                TaskId = currentTask.Id,
                ActionId = model.ActionId
            };

            try
            {
                await _bpmEngineService.CompleteTask(completingTask);
                return true;
                    }
            catch
            {
                return false;            }

        }

        public async Task<IEnumerable<WorkForVoteViewModel>> GetAllWorks(long contestId, long visitorId)
        {
            var works = await _contestDalService.GetAllWorksWithVotes(contestId);

            var vmWorks = new List<WorkForVoteViewModel>();

           foreach(var work in works)
            {
                var vmWork = _mapper.Map<WorkForVoteViewModel>(work);
                vmWork.VisitorHasVoted = work.Votes.Any(m => m.VoterId == visitorId);
                vmWork.VisitorIsAuthor = work.ParticipantId == visitorId;
                vmWorks.Add(vmWork);
            }
            return vmWorks;
        }

        public async Task Vote(long voterId, long workId)
        {
            await _contestDalService.Vote(voterId, workId);
        }

        public async Task<IEnumerable<WorkWithScoreModel>> GetTopVotedWorks(long contestId)
        {
            //TODO мапить во вью-модели
            return await _contestDalService.GetTopVotedWorks(contestId);
        }
    }
}
