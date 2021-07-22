using Contestor.BpmEngine.Contract;
using Newtonsoft.Json;
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

        public async Task<IEnumerable<BusinessProcessModel>> GetLatestVersionProcesses()
        {
            var allProcesses = await SendGetRequest<IEnumerable<BusinessProcessModel>>("process-definition?latestVersion=true");
            return allProcesses;
        }

        public async Task<BusinessProcessModel> GetProcessById(string processId)
        {
            if (string.IsNullOrWhiteSpace(processId))
            {
                return null;
            }

            try
            {
                var allProcesses = await SendGetRequest<BusinessProcessModel[]>($"process-definition?processDefinitionId={processId}");

                return allProcesses.Any() ? allProcesses[0] : null;
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        private async Task<TResult> SendGetRequest<TResult>(string url)
        {
            var response = await _httpClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<TResult>(response);
        }
    }
}
