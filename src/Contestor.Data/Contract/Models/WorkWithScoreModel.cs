using Contestor.Data.Contract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.Data.Contract
{
  public  class WorkWithScoreModel
    {
        public WorkModel Work { get; set; }

        public int Score { get; set; }
    }
}
