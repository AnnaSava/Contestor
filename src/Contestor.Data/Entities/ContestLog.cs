using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.Data.Entities
{
    public class ContestLog
    {
        public long Id { get; set; }

        public long ContestId { get; set; }

        public DateTime DateTime { get; set; }

        public string Action { get; set; }

        public string Value { get; set; }

        public string Message { get; set; }
    }
}
