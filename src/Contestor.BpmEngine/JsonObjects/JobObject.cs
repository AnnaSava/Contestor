using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.BpmEngine.JsonObjects
{
   public class JobObject
    {
        public string Id { get; set; }

        public string JobDefinitionId { get; set; }

        public string DueDate { get; set; }

        public string ProcessInstanceId { get; set; }

        public string ExecutionId { get; set; }

        public string ProcessDefinitionId { get; set; }

        public string ProcessDefinitionKey { get; set; }

        public int Retries { get; set; }

        public string ExceptionMessage { get; set; }

        public string failedActivityId { get; set; }

        public bool Suspended { get; set; }

        public int Priority { get; set; }

        public string TenantId { get; set; }

        public string CreateTime { get; set; }
    }
}
