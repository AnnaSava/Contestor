using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.Data.Entities
{
    public class Contest : BaseEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string ProcessName { get; set; }

        public string ProcessKey { get; set; }

        public string Status { get; set; }

        public virtual ICollection<Participant> Participants { get; set; }
    }
}
