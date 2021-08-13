using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.Service.Contract
{
    public class WorkForVoteViewModel
    {
        public long Id { get; set; }

        public long ParticipantId { get; set; }

        public long ContestId { get; set; }

        public int RoundNumber { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public bool VisitorHasVoted { get; set; }

        public bool VisitorIsAuthor { get; set; }
    }
}
