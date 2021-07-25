using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.Data.Contract.Models
{
    public class ContestModel : BaseModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string ProcessName { get; set; }

        public string ProcessKey { get; set; }

        public int RoundNumber { get; set; }

        public string Status { get; set; }
    }
}
