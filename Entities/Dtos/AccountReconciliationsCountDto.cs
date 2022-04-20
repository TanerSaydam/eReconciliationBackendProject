using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class AccountReconciliationsCountDto : IDto
    {
        public int AllReconciliation { get; set; }
        public int SucceedReconciliation { get; set; }
        public int NotResponseReconciliation { get; set; }
        public int FailReconciliation { get; set; }
    }
}
