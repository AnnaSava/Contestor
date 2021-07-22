using Contestor.Data.Contract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.Service.Contract
{
    public interface IContestService
    {
        Task<ContestModel> Create(ContestModel model);

        Task<Dictionary<string, string>> GetProcessesDictionary();
    }
}
