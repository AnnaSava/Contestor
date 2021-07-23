using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.BpmEngine.Contract
{
    public interface IBpmEngineClient
    {
        Task<IEnumerable<ProcessModel>> GetLatestVersionProcesses();

        Task<ProcessModel> GetProcessById(string processId);

        Task<string> StartProcess(StartProcessModel startProcessModel);
    }
}
