using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.Data.Entities
{
    public class Participant 
    {
        [Key]
        public long UserId { get; set; }

        public virtual User User { get; set; }

        [Key]
        public long ContestId { get; set; }

        public virtual Contest Contest { get; set; }

        public string DisplayName { get; set; }

        public int WorksCount { get; set; }

        public ICollection<Work> Works { get; set; }
    }
}
