using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.BpmEngine.Contract
{
    public class ProcessInstanceModel
    {
        public string Id { get; set; }

        public string DefinitionId { get; set; }

        public string BusinessKey { get; set; }
    }
}
