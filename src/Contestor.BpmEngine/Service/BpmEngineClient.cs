using Contestor.BpmEngine.Contract;
using Contestor.BpmEngine.JsonObjects;
using Contestor.BpmEngine.Mapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.BpmEngine.Service
{
    public class BpmEngineClient : IBpmEngineClient
    {
        private const string SelectedActionVariableName = "SelectedAction";

        private readonly HttpClient _httpClient;

        public BpmEngineClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ProcessModel>> GetProcessesLatestVersions()
        {
            var processes = await SendGetRequest<IEnumerable<ProcessModel>>("process-definition?latestVersion=true");
            return processes;
        }

        public async Task<ProcessModel> GetProcessById(string processDefinitionId)
        {
            if (string.IsNullOrWhiteSpace(processDefinitionId))
            {
                return null;
            }

            var processes = await SendGetRequest<ProcessModel[]>($"process-definition?processDefinitionId={processDefinitionId}");

            return processes.Any() ? processes[0] : null;
        }

        public async Task<string> StartProcess(StartingProcessModel startingProcessModel)
        {
            var processInstances = await SendGetRequest<ProcessInstanceModel[]>($"process-instance?businessKey={startingProcessModel.BusinessKey}");

            if (processInstances.Any())
                return processInstances.FirstOrDefault()?.Id;

            var startingProcess = new StartingProcessObject(startingProcessModel.BusinessKey);

            var startingProcessResult = await SendPostRequest<StartingProcessObject, StartingProcessResultObject>(
                    $"process-definition/{startingProcessModel.ProcessId}/start",
                    startingProcess);

            return startingProcessResult.Id;
        }

        public async Task<IEnumerable<TaskModel>> GetCurrentTasks(string businessKey)
        {
            var tasks = await SendGetRequest<HistoryTaskObject[]>($"history/task?processInstanceBusinessKey={businessKey}&unfinished=true");

            if (tasks == null)
            {
                return new List<TaskModel>();
            }

            var taskModels = new List<TaskModel>();

            foreach (var task in tasks)
            {
                var formVarsObject = await GetTaskFormVariables<TaskFormVariablesObject>(task.Id);
                taskModels.Add(MapTask(task, formVarsObject));
            }

            return taskModels;
        }

        public async Task<TaskModel> GetTask(string taskId)
        {
            var tasks = await SendGetRequest<HistoryTaskObject[]>($"history/task?taskId={taskId}&unfinished=true");

            if (!tasks.Any())
            {
                return new TaskModel();
            }

            var formVarsObject = await GetTaskFormVariables<TaskFormVariablesObject>(taskId);

            return MapTask(tasks[0], formVarsObject);
        }

        private TaskModel MapTask(HistoryTaskObject historyTask, TaskFormVariablesObject formVarsObject)
        {
            var task = new TaskModel
            {
                Id = historyTask.Id,
                Name = historyTask.Name,
                ProcessDefinitionId = historyTask.ProcessDefinitionId,
                TaskDefinitionKey = historyTask.TaskDefinitionKey,
                DueDate = historyTask.Due,
                AvailableActions = formVarsObject.AvailableActions.ToStringDictionary()
            };

            return task;
        }

        private async Task<T> GetTaskFormVariables<T>(string taskId)
        {
            return await SendGetRequest<T>($"task/{taskId}/form-variables/");
        }

        public async Task<bool> CompleteTask(CompletingTaskModel completingTask)
        {
            var completeTaskObj = FillCompletingTask(completingTask);

            await SendPostRequest<CompletingTaskObject, bool>(
                $"task/{completingTask.TaskId}/complete",
                completeTaskObj);

            return true;
        }

        private static CompletingTaskObject FillCompletingTask(CompletingTaskModel completingTask)
        {
            var completeTaskObj = new CompletingTaskObject
            {
                Variables = new Dictionary<string, VariableValueObject>
                {
                    {
                        SelectedActionVariableName,
                        new VariableValueObject { Value = completingTask.ActionId.ToString() }
                    }
                }
            };

            return completeTaskObj;
        }

        private async Task<TResult> SendGetRequest<TResult>(string url)
        {
            var response = await _httpClient.GetStringAsync(url);
            var deserialized = JsonConvert.DeserializeObject<TResult>(response);

            return deserialized;
        }

        private async Task<TResult> SendPostRequest<T, TResult>(string url, T data)
        {
            var content = new StringContent(SerializeObject(data), Encoding.UTF8, "application/json");

            using var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error {response.StatusCode} Url {url}: {response.ReasonPhrase}");
            }

            string responseContent = await response.Content.ReadAsStringAsync();
            var deserialized = string.IsNullOrWhiteSpace(responseContent)
                ? default
                : JsonConvert.DeserializeObject<TResult>(responseContent);

            return deserialized;
        }

        public static string SerializeObject<T>(T obj)
        {
            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var serialized = JsonConvert.SerializeObject(
                obj,
                new JsonSerializerSettings
                {
                    ContractResolver = contractResolver,
                    Formatting = Formatting.Indented
                });

            return serialized;
        }
    }
}
