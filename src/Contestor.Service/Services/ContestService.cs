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

        public ContestService(IContestDalService contestDalService, IBpmEngineClient bpmEngineService)
        {
            _contestDalService = contestDalService;
            _bpmEngineService = bpmEngineService;
        }

        public async Task<ContestModel> Create(ContestModel model)
        {
            var process = await _bpmEngineService.GetProcessById(model.ProcessKey);
            model.ProcessName = process?.Name;

            return await _contestDalService.Create(model);
        }

        public async Task Open(long contestId)
        {
            await _contestDalService.SetStatus(contestId, ContestStatus.Open);
        }

        public async Task OpenRegistration(long contestId)
        {
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
            return await _contestDalService.GetOne(id);
        }

        public async Task<IEnumerable<ContestModel>> GetAll(int page, int count)
        {
            return await _contestDalService.GetAll(page, count);
        }

        public async Task<IEnumerable<ContestModel>> GetAllForNewParticipants(long visitorId, int page, int count)
        {
            return await _contestDalService.GetAllForNewParticipants(visitorId, page, count);
        }

        public async Task<Dictionary<string, string>> GetProcessesDictionary()
        {
            var businessProcesses = (await _bpmEngineService.GetLatestVersionProcesses()).ToList();
            return businessProcesses.ToDictionary(x => x.Id, x => x.Name);
        }

        public async Task<string> StartContestProcess(long contestId)
        {
            var contest = await _contestDalService.GetOne(contestId);

            var startProcessModel = new StartProcessModel { ProcessId = contest.ProcessKey, BusinessKey = contest.Id.ToString() };

            var process = await _bpmEngineService.StartProcess(startProcessModel);
            await _contestDalService.SetStatus(contestId, ContestStatus.Started);
            return process;
        }

        public async Task RegisterParticipant(long contestId, long userId)
        {
            await _contestDalService.RegisterParticipant(contestId, userId);
        }

        public async Task<ParticipantModel> GetParticipant(long contestId, long userId)
        {
            return await _contestDalService.GetParticipant(contestId, userId);
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
    }
}
