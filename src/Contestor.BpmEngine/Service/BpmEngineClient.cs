using Contestor.BpmEngine.Contract;
using Contestor.BpmEngine.JsonObjects;
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
        private readonly HttpClient _httpClient;
        public BpmEngineClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ProcessModel>> GetLatestVersionProcesses()
        {
            var allProcesses = await SendGetRequest<IEnumerable<ProcessModel>>("process-definition?latestVersion=true");
            return allProcesses;
        }

        public async Task<ProcessModel> GetProcessById(string processId)
        {
            if (string.IsNullOrWhiteSpace(processId))
            {
                return null;
            }

            try
            {
                var allProcesses = await SendGetRequest<ProcessModel[]>($"process-definition?processDefinitionId={processId}");

                return allProcesses.Any() ? allProcesses[0] : null;
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<string> StartProcess(StartProcessModel startProcessModel)
        {
            var existingInstances =
                await SendGetRequest<ProcessInstanceModel[]>($"process-instance?businessKey={startProcessModel.BusinessKey}");

            if (existingInstances.Any())
                return existingInstances.FirstOrDefault()?.Id;

            var startProcessObject = new StartProcessObject(startProcessModel.BusinessKey);

            var startProcessResult =
                await SendPostRequest<StartProcessObject, StartProcessResultObject>(
                    $"process-definition/{startProcessModel.ProcessId}/start",
                    startProcessObject);

            return startProcessResult.Id;
        }

        private async Task<TResult> SendGetRequest<TResult>(string url)
        {
            var response = await _httpClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<TResult>(response);
        }

        private async Task<TRes> SendPostRequest<T, TRes>(string url, T data)
        {
            using (HttpResponseMessage response = await _httpClient.PostAsync(
                url,
                new StringContent(SerializeObject(data), Encoding.UTF8, "application/json")))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error {response.StatusCode} Url {url}: {response.ReasonPhrase}");
                }

                string responseContent = await response.Content.ReadAsStringAsync();

                return string.IsNullOrWhiteSpace(responseContent)
                    ? default(TRes)
                    : JsonConvert.DeserializeObject<TRes>(responseContent);
            }
        }

        public static string SerializeObject<T>(T obj)
        {
            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            return JsonConvert.SerializeObject(
                obj,
                new JsonSerializerSettings
                {
                    ContractResolver = contractResolver,
                    Formatting = Formatting.Indented
                });
        }
    }
}
