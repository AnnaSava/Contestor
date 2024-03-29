﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.Proto.Data
{
    public static class ContestStatus
    {
        public const string Draft = "draft";

        public const string Started = "started";

        public const string RegistrationOpen = "regopen";

        public const string RegistrationClosed = "regclosed";

        public const string Open = "open";

        public const string Closed = "closed";

        public const string Voting = "voting";

        public const string VotingFinished = "votingfin";

        public const string WaitingWinner = "waitwinner";

        public const string Finished = "finished";
    }
}
