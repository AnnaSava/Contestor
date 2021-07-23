using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.BpmEngine.JsonObjects
{
    public class StartProcessObject
    {
        public StartProcessObject(string businessKey)
        {
            BusinessKey = businessKey;
            Variables = new Dictionary<string, VariableObject>();
        }

        public string BusinessKey { get; set; }

        public Dictionary<string, VariableObject> Variables { get; set; }
    }
}
