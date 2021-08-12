using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.Data.Entities
{
    public class Vote
    {
        public long VoterId { get; set; }

        public User Voter { get; set; }

        public long WorkId { get; set; }

        public Work Work { get; set; }

        public int Points { get; set; }
    }
}
