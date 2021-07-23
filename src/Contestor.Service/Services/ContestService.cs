using Contestor.BpmEngine.Contract;
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
            return await _contestDalService.Create(model);
        }

        public async Task<ContestModel> GetOne(long id)
        {
            return await _contestDalService.GetOne(id);
        }

        public async Task<IEnumerable<ContestModel>> GetAll(int page, int count)
        {
            return await _contestDalService.GetAll(page, count);
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

            return await _bpmEngineService.StartProcess(startProcessModel);
        }
    }
}
