using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.Data.Contract.Models
{
  public  class WorkWithVotesModel : BaseModel
    {
        public long ParticipantId { get; set; }

        public long ContestId { get; set; }

        public int RoundNumber { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public List<VoteModel> Votes { get; set; }
    }
}
