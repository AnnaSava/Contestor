using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.Service.Contract
{
   public class CompletingTaskViewModel
    {
        /// <summary>
        /// Идентификатор бизнес-объекта
        /// </summary>
        public long ContestId { get; set; }

        /// <summary>
        /// Идентификатор задачи бизнес-процесса
        /// </summary>
        public string TaskId { get; set; }

        /// <summary>
        /// Идентификатор выбранного действия
        /// </summary>
        public int ActionId { get; set; }

        /// <summary>
        /// Наименование выбранного действия
        /// </summary>
        public string ActionName { get; set; }
    }
}
