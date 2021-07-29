using Contestor.BpmEngine.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.Service.Contract
{
   public class ContestManageViewModel
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ProcessName { get; set; }

        public string ProcessKey { get; set; }

        public int RoundNumber { get; set; }

        public string Status { get; set; }

        public bool AutoRegEnabled { get; set; }

        public int MaxWorksCount { get; set; }

        public List<TaskModel> Tasks { get; set; }
    }
}
