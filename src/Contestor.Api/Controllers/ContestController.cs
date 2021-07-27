using Contestor.Service.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contestor.BpmEngine.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContestController : ControllerBase
    {
        IContestService _contestService;

        public ContestController(IContestService contestService)
        {
            _contestService = contestService;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task Open(string contestId)
        {
            await _contestService.Open(long.Parse(contestId));
        }

        [HttpPost]
        [Route("[action]")]
        public async Task OpenRegistration(string contestId)
        {
            await _contestService.OpenRegistration(long.Parse(contestId));
        }

        [HttpPost]
        [Route("[action]")]
        public async Task StartVoting(string contestId)
        {
            await _contestService.StartVoting(long.Parse(contestId));
        }

        [HttpPost]
        [Route("[action]")]
        public async Task WaitWinner(string contestId)
        {
            await _contestService.WaitWinner(long.Parse(contestId));
        }

        [HttpPost]
        [Route("[action]")]
        public async Task SetFinishedStatus(string contestId)
        {
            await _contestService.SetFinishedStatus(long.Parse(contestId));
        }
    }
}
