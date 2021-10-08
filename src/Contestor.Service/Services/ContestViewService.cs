using AutoMapper;
using Contestor.Proto.BpmEngine;
using Contestor.Proto.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contestor.Proto.Services
{
    public class ContestViewService : IContestViewService
    {
        private readonly IContestService _contestService;
        private readonly IBpmEngineClient _bpmEngineService;
        private readonly IMapper _mapper;

        public ContestViewService(IContestService contestDalService, IBpmEngineClient bpmEngineService, IMapper mapper)
        {
            _contestService = contestDalService;
            _bpmEngineService = bpmEngineService;
            _mapper = mapper;
        }

        public async Task<ContestModel> Create(ContestModel model)
        {
            var process = await _bpmEngineService.GetProcessById(model.ProcessKey);
            model.ProcessName = process?.Name;
            model.AutoRegEnabled = true;

            return await _contestService.Create(model);
        }

        public async Task<ContestModel> Update(ContestModel model)
        {
            var process = await _bpmEngineService.GetProcessById(model.ProcessKey);
            model.ProcessName = process?.Name;

            return await _contestService.Update(model);
        }

        public async Task SetDueDate(long contestId, string date)
        {
            if (DateTime.TryParse(date, out DateTime parsedDate))
                await _contestService.SetDueDate(contestId, parsedDate);
        }

        public async Task Open(long contestId)
        {            
            await _contestService.SetStatus(contestId, ContestStatus.Open);
        }

        public async Task Close (long contestId)
        {
            await _contestService.SetStatus(contestId, ContestStatus.Closed);
        }

        public async Task OpenRegistration(long contestId)
        {
            //TODO может сделать UoW, чтобы один раз вызывать dbContext.SaveChanges? или один метод на уровне DAL?
            await _contestService.SetAutoRegEnabled(contestId, false);
            await _contestService.SetStatus(contestId, ContestStatus.RegistrationOpen);

            //TODO врзможно, работа с таймерами понадобится. Пусть пока будет закомментировано
            //var timer = (await _bpmEngineService.GetTimers()).SingleOrDefault();
            //if (timer != null)
            //{
            //    await _contestDalService.SetDueDate(contestId, DateTime.Parse(timer.DueDate));
            //}
        }

        public async Task CloseRegistration(long contestId)
        {
            await _contestService.SetStatus(contestId, ContestStatus.RegistrationClosed);
        }

        public async Task StartVoting(long contestId)
        {
            await _contestService.SetStatus(contestId, ContestStatus.Voting);
        }

        public async Task FinishVoting(long contestId)
        {
            await _contestService.SetStatus(contestId, ContestStatus.VotingFinished);
        }

        public async Task PublishWinners(long contestId)
        {
            await _contestService.SetWinnerPlaces(contestId);
        }

        public async Task SetFinishedStatus(long contestId)
        {
            await _contestService.SetStatus(contestId, ContestStatus.Finished);
            await _contestService.SetDueDate(contestId, null);
        }

        public async Task<ContestModel> GetOne(long id)
        {
            var model = await _contestService.GetOne(id);
            return model;
        }

        public async Task<ContestModel> GetOne(long id, long visitorId)
        {
            var model = await _contestService.GetOne(id, visitorId);
            return model;
        }

        public async Task<ContestManageViewModel> GetOneForManage(long id)
        {
            var model = await _contestService.GetOne(id);
            var vm = _mapper.Map<ContestManageViewModel>(model);

            var currentTasks = await _bpmEngineService.GetCurrentTasks(model.Id.ToString());
            vm.Tasks = currentTasks.ToList();

            return vm;
        }

        public async Task<IEnumerable<ContestModel>> GetAll(int page, int count)
        {
            return await _contestService.GetAll(page, count);
        }

        public async Task<IEnumerable<ContestModel>> GetAllByUser(long userId, int page, int count)
        {
            return await _contestService.GetAllByUser(userId, page, count);
        }

        public async Task<IEnumerable<ContestModel>> GetAllForNewParticipants(long visitorId, int page, int count)
        {
            return await _contestService.GetAllForNewParticipants(visitorId, page, count);
        }

        public async Task<IEnumerable<ContestModel>> GetAllForVoting()
        {
            return await _contestService.GetAllForVoting();
        }

        public async Task<IEnumerable<ContestModel>> GetTopForVoting(long visitorId)
        {
            return await _contestService.GetTopForVoting(visitorId);
        }

        public async Task<Dictionary<string, string>> GetProcessesDictionary()
        {
            var businessProcesses = (await _bpmEngineService.GetProcessesLatestVersions()).ToList();
            return businessProcesses.ToDictionary(x => x.Id, x => x.Name);
        }

        public async Task<string> StartContestProcess(long contestId, string apiUrl)
        {
            var contest = await _contestService.GetOne(contestId);

            var startProcessModel = new StartingProcessModel { ProcessId = contest.ProcessKey, BusinessKey = contest.Id.ToString(), ApiUrl = apiUrl };

            await _contestService.SetStatus(contestId, ContestStatus.Started);

            try
            {
                var process = await _bpmEngineService.StartProcess(startProcessModel);

                return process;
            }
            catch(Exception ex)
            {
                await _contestService.SetStatus(contestId, ContestStatus.Draft);
            }

            return null;
        }

        public async Task RegisterParticipant(long contestId, long userId)
        {
            await _contestService.RegisterParticipant(contestId, userId);
        }

        public async Task<ParticipantModel> GetParticipant(long contestId, long userId)
        {
            return await _contestService.GetParticipant(contestId, userId);
        }

        public async Task<int> GetParticipantsCount(long contestId)
        {
            return await _contestService.GetParticipantsCount(contestId);
        }

        public async Task<IEnumerable<ParticipantModel>> GetAllParticipants(long contestId)
        {
            return await _contestService.GetAllParticipants(contestId, 1, 100);
        }

        public async Task<int> GetParticipantsHavingWorkCount(long contestId)
        {
            return await _contestService.GetParticipantsHavingWorkCount(contestId);
        }

        public async Task<int> GetWorksHavingVotesCount(long contestId)
        {
            return await _contestService.GetWorksHavingVotesCount(contestId);
        }

        public async Task SendWork(WorkModel model)
        {
            var contest = await _contestService.GetOne(model.ContestId);
            var participant = await _contestService.GetParticipant(model.ContestId, model.ParticipantId);
            if (participant == null && contest.AutoRegEnabled == false) throw new Exception($"Участник {model.ParticipantId} не зарегистрирован на конкурс {model.ContestId}");

            if (participant == null) await _contestService.RegisterParticipant(contest.Id, model.ParticipantId);

            model.RoundNumber = contest.RoundNumber;
            model.ParticipantId = model.ParticipantId;

            await _contestService.SendWork(model);
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
                return false;
            }

        }

        public async Task<IEnumerable<WorkModel>> GetAllWorks(long contestId)
        {
            return await _contestService.GetAllWorks(contestId);
        }

        public async Task<IEnumerable<WorkForVoteViewModel>> GetAllWorks(long contestId, long visitorId)
        {
            var works = await _contestService.GetAllWorksWithVotes(contestId);

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
        public async Task<IEnumerable<WorkModel>> GetWorksByUser(long userId, int page, int count)
        {
            return await _contestService.GetWorksByUser(userId, page, count);
        }

        public async Task Vote(long voterId, long workId)
        {
            await _contestService.Vote(voterId, workId);
        }

        public async Task<IEnumerable<WorkWithScoreModel>> GetTopVotedWorks(long contestId)
        {
            //TODO мапить во вью-модели
            return await _contestService.GetTopVotedWorks(contestId);
        }
    }
}
