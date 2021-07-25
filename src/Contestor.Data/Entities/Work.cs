using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.Data.Entities
{
    public class Work : BaseEntity
    {
        public long ParticipantId { get; set; }

        public virtual Participant Participant { get; set; }

        public long ContestId { get; set; }

        public virtual Contest Contest { get; set; }

        public int RoundNumber { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
    }
}
