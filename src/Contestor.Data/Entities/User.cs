using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.Proto.Data.Entities
{
    public class User : IdentityUser<long>
    {
        public virtual ICollection<Participant> Participants { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }
    }
}
