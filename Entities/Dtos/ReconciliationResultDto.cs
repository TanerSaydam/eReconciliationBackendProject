using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class ReconciliationResultDto : IDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public bool Result { get; set; }
    }
}
