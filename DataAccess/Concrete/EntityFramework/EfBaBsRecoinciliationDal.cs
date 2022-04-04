using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfBaBsRecoinciliationDal : EfEntityRepositoryBase<BaBsReconciliation, ContextDb>, IBaBsRecoinciliationDal
    {
        public List<BaBsReconciliationDto> GetAllDto(int companyId)
        {
            using (var context = new ContextDb())
            {
                var result = from recoinciliation in context.BaBsReconciliations.Where(p => p.CompanyId == companyId)
                             join company in context.Companies on recoinciliation.CompanyId equals company.Id
                             join account in context.CurrencyAccounts on recoinciliation.CurrencyAccountId equals account.Id
                             select new BaBsReconciliationDto
                             {
                                 CompanyId = companyId,
                                 CurrencyAccountId = account.Id,
                                 AccountIdentityNumber = account.IdentityNumber,
                                 AccountName = account.Name,
                                 AccountTaxDepartment = account.TaxDepartment,
                                 AccountTaxIdNumber = account.TaxIdNumber,
                                 CompanyIdentityNumber = company.IdentityNumber,
                                 CompanyName = company.Name,
                                 CompanyTaxDepartment = company.TaxDepartment,
                                 CompanyTaxIdNumber = company.TaxIdNumber,
                                 Total = recoinciliation.Total,                            
                                 EmailReadDate = recoinciliation.EmailReadDate,                         
                                 Guid = recoinciliation.Guid,
                                 Id = recoinciliation.Id,
                                 IsEmailRead = recoinciliation.IsEmailRead,
                                 IsResultSucceed = recoinciliation.IsResultSucceed,
                                 IsSendEmail = recoinciliation.IsSendEmail,
                                 ResultDate = recoinciliation.ResultDate,
                                 ResultNote = recoinciliation.ResultNote,
                                 SendEmailDate = recoinciliation.SendEmailDate,                                 
                                 CurrencyCode = "TL",
                                 AccountEmail = account.Email,
                                 Mounth = recoinciliation.Mounth,
                                 Type = recoinciliation.Type,
                                 Quantity = recoinciliation.Quantity,
                                 Year = recoinciliation.Year 
                             };

                return result.ToList();
            }
        }
    }
}
