using Contestor.Proto.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Contestor.Proto.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContestController : ControllerBase
    {
        IContestViewService _contestService;

        public ContestController(IContestViewService contestService)
        {
            _contestService = contestService;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task SetDueDate(string contestId, [FromBody]DateInput input)
        {
            await _contestService.SetDueDate(long.Parse(contestId), input?.Date);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task OpenRegistration(string contestId)
        {
            await _contestService.OpenRegistration(long.Parse(contestId));
        }

        [HttpPost]
        [Route("[action]")]
        public async Task CloseRegistration(string contestId)
        {
            await _contestService.CloseRegistration(long.Parse(contestId));
        }

        [HttpPost]
        [Route("[action]")]
        public async Task Open(string contestId)
        {
            await _contestService.Open(long.Parse(contestId));
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
        public async Task Finish(string contestId)
        {
            await _contestService.SetFinishedStatus(long.Parse(contestId));
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<int> GetParticipantsCount(long contestId)
        {
            return await _contestService.GetParticipantsCount(contestId);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<int> GetParticipantsHavingWorkCount(long contestId)
        {
            return await _contestService.GetParticipantsHavingWorkCount(contestId);
        }
    }
}
