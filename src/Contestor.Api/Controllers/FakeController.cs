using Contestor.Proto;
using Contestor.Proto.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Contestor.Proto.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakeController : ControllerBase
    {
        IContestViewService _contestService;

        public FakeController(IContestViewService contestService)
        {
            _contestService = contestService;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task RegisterParticipants(long contestId)
        {
            for (int i = 1; i <= 5; i++)
            {
                await _contestService.RegisterParticipant(contestId, i);
            }
        }

        [HttpPost]
        [Route("[action]")]
        public async Task SendWorks(long contestId)
        {
            for (int i = 1; i <= 5; i++)
            {
                var work = new WorkModel
                {
                    ContestId = contestId,
                    ParticipantId = i,
                    Title = $"Work {i} {DateTime.Now}"
                };

                await _contestService.SendWork(work);
            }
        }
    }
}
