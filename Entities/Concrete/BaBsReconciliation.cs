using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class BaBsReconciliation : IEntity
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int CurrencyAccountId { get; set; }
        public string Type { get; set; }
        public int Mounth { get; set; }
        public int Year { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public bool IsSendEmail { get; set; }
        public DateTime? SendEmailDate { get; set; }
        public bool? IsEmailRead { get; set; }
        public DateTime? EmailReadDate { get; set; }
        public bool? IsResultSucceed { get; set; }
        public DateTime? ResultDate { get; set; }
        public string? ResultNote { get; set; }
        public string? Guid { get; set; }
    }
}
