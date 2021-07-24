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
        public async Task SetFinishedStatus(long contestId)
        {
            await _contestService.SetFinishedStatus(contestId);
        }
    }
}
