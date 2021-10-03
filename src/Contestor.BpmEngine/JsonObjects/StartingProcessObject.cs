using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.Proto.BpmEngine.JsonObjects
{
    public class StartingProcessObject
    {
        public StartingProcessObject(string businessKey)
        {
            BusinessKey = businessKey;
            Variables = new Dictionary<string, VariableObject>();
        }

        public string BusinessKey { get; set; }

        public Dictionary<string, VariableObject> Variables { get; set; }
    }
}
