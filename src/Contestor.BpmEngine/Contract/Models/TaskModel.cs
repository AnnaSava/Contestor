using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.BpmEngine.Contract
{
   public class TaskModel
    {
        public TaskModel()
        {
            AvailableActions = new Dictionary<int, string>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string ProcessDefinitionId { get; set; }

        public string TaskDefinitionKey { get; set; }

        public string ProcessInstanceId { get; set; }

        public DateTime? DueDate { get; set; }

        public Dictionary<int, string> AvailableActions { get; set; }
    }
}
