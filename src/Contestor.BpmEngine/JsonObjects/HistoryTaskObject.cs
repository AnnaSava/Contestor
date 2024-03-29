﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.Proto.BpmEngine.JsonObjects
{
   public class HistoryTaskObject
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ProcessDefinitionId { get; set; }

        public string TaskDefinitionKey { get; set; }

        public DateTime? Due { get; set; }
    }
}
