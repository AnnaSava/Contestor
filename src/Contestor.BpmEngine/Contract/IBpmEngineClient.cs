using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.Proto.BpmEngine
{
    public interface IBpmEngineClient
    {
        Task<IEnumerable<ProcessModel>> GetProcessesLatestVersions();

        Task<ProcessModel> GetProcessById(string processDefinitionId);

        Task<string> StartProcess(StartingProcessModel startingProcessModel);

        Task<IEnumerable<TaskModel>> GetCurrentTasks(string businessKey);

        Task<TaskModel> GetTask(string taskId);

        Task<bool> CompleteTask(CompletingTaskModel completingTask);

        Task<IEnumerable<JobModel>> GetTimers();
    }
}
