using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.BpmEngine.JsonObjects
{
    public class VariableObject
    {
        public string Type { get; set; }

        public object Value { get; set; }

        public object ValueInfo { get; set; }
    }
}
