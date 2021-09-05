using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.Data.Contract
{
    public class ContestModel : BaseModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string ProcessName { get; set; }

        public string ProcessKey { get; set; }

        public int RoundNumber { get; set; }

        public string Status { get; set; }

        public bool AutoRegEnabled { get; set; }

        public int MaxWorksCount { get; set; }

        #region ViewModel additions

        public bool VisitorIsParticipant { get; set; }

        public int VisitorWorksCount { get; set; }

        #endregion
    }
}
