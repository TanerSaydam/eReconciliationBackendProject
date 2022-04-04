using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class BaBsReconciliationDto : BaBsReconciliation
    {
        public string CompanyName { get; set; }
        public string CompanyTaxDepartment { get; set; }
        public string CompanyTaxIdNumber { get; set; }
        public string CompanyIdentityNumber { get; set; }
        public string AccountName { get; set; }
        public string AccountTaxDepartment { get; set; }
        public string AccountTaxIdNumber { get; set; }
        public string AccountIdentityNumber { get; set; }
        public string AccountEmail { get; set; }
        public string CurrencyCode { get; set; }

    }
}
