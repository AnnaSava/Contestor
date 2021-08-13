using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.Data.Contract.Models
{
    public class VoteModel
    {
        public long VoterId { get; set; }

        public int Points { get; set; }
    }
}
