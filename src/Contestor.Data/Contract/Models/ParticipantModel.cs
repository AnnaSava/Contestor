using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.Proto.Data
{
    public class ParticipantModel
    {
        public long UserId { get; set; }

        public long ContestId { get; set; }

        public string DisplayName { get; set; }

        public int WorksCount { get; set; }

    }
}
