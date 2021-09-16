using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.BpmEngine.Contract
{
    public class StartingProcessModel
    {
        public string ProcessId { get; set; }

        public string BusinessKey { get; set; }

        public string ApiUrl { get; set; }
    }
}
