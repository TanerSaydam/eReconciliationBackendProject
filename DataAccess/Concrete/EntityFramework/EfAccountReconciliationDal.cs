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
    public class EfAccountReconciliationDal : EfEntityRepositoryBase<AccountReconciliation, ContextDb>, IAccountReconciliationDal
    {
        public List<AccountReconciliationDto> GetAllDto(int companyId)
        {
            using (var context = new ContextDb())
            {
                var result = from recoinciliation in context.AccountReconciliations.Where(p=> p.CompanyId == companyId)
                             join company in context.Companies on recoinciliation.CompanyId equals company.Id
                             join account in context.CurrencyAccounts on recoinciliation.CurrencyAccountId equals account.Id
                             join currency in context.Currencies on recoinciliation.CurrencyId equals currency.Id
                             select new AccountReconciliationDto
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
                                 CurrencyCredit = recoinciliation.CurrencyCredit,
                                 CurrencyDebit = recoinciliation.CurrencyDebit,
                                 CurrencyId = recoinciliation.CurrencyId,
                                 EmailReadDate = recoinciliation.EmailReadDate,
                                 EndingDate = recoinciliation.EndingDate,
                                 Guid = recoinciliation.Guid,
                                 Id = recoinciliation.Id,
                                 IsEmailRead = recoinciliation.IsEmailRead,
                                 IsResultSucceed = recoinciliation.IsResultSucceed,
                                 IsSendEmail = recoinciliation.IsSendEmail,
                                 ResultDate = recoinciliation.ResultDate,
                                 ResultNote = recoinciliation.ResultNote,
                                 SendEmailDate = recoinciliation.SendEmailDate,
                                 StartingDate = recoinciliation.StartingDate,
                                 CurrencyCode = currency.Code,
                                 AccountEmail = account.Email,
                                 AccountCode = account.Code
                             };
                return result.ToList();
            }
        }

        public AccountReconciliationDto GetByCodeDto(string code)
        {
            using (var context = new ContextDb())
            {
                var result = from recoinciliation in context.AccountReconciliations.Where(p=> p.Guid == code)
                             join company in context.Companies on recoinciliation.CompanyId equals company.Id
                             join account in context.CurrencyAccounts on recoinciliation.CurrencyAccountId equals account.Id
                             join currency in context.Currencies on recoinciliation.CurrencyId equals currency.Id
                             select new AccountReconciliationDto
                             {
                                 CompanyId = recoinciliation.CompanyId,
                                 CurrencyAccountId = account.Id,
                                 AccountIdentityNumber = account.IdentityNumber,
                                 AccountName = account.Name,
                                 AccountTaxDepartment = account.TaxDepartment,
                                 AccountTaxIdNumber = account.TaxIdNumber,
                                 CompanyIdentityNumber = company.IdentityNumber,
                                 CompanyName = company.Name,
                                 CompanyTaxDepartment = company.TaxDepartment,
                                 CompanyTaxIdNumber = company.TaxIdNumber,
                                 CurrencyCredit = recoinciliation.CurrencyCredit,
                                 CurrencyDebit = recoinciliation.CurrencyDebit,
                                 CurrencyId = recoinciliation.CurrencyId,
                                 EmailReadDate = recoinciliation.EmailReadDate,
                                 EndingDate = recoinciliation.EndingDate,
                                 Guid = recoinciliation.Guid,
                                 Id = recoinciliation.Id,
                                 IsEmailRead = recoinciliation.IsEmailRead,
                                 IsResultSucceed = recoinciliation.IsResultSucceed,
                                 IsSendEmail = recoinciliation.IsSendEmail,
                                 ResultDate = recoinciliation.ResultDate,
                                 ResultNote = recoinciliation.ResultNote,
                                 SendEmailDate = recoinciliation.SendEmailDate,
                                 StartingDate = recoinciliation.StartingDate,
                                 CurrencyCode = currency.Code,
                                 AccountEmail = account.Email,
                                 AccountCode = account.Code
                             };
                return result.FirstOrDefault();
            }
        }

        public AccountReconciliationDto GetByIdDto(int id)
        {
            using (var context = new ContextDb())
            {
                var result = from recoinciliation in context.AccountReconciliations.Where(p => p.Id == id)
                             join company in context.Companies on recoinciliation.CompanyId equals company.Id
                             join account in context.CurrencyAccounts on recoinciliation.CurrencyAccountId equals account.Id
                             join currency in context.Currencies on recoinciliation.CurrencyId equals currency.Id
                             select new AccountReconciliationDto
                             {
                                 CompanyId = recoinciliation.CompanyId,
                                 CurrencyAccountId = account.Id,
                                 AccountIdentityNumber = account.IdentityNumber,
                                 AccountName = account.Name,
                                 AccountTaxDepartment = account.TaxDepartment,
                                 AccountTaxIdNumber = account.TaxIdNumber,
                                 CompanyIdentityNumber = company.IdentityNumber,
                                 CompanyName = company.Name,
                                 CompanyTaxDepartment = company.TaxDepartment,
                                 CompanyTaxIdNumber = company.TaxIdNumber,
                                 CurrencyCredit = recoinciliation.CurrencyCredit,
                                 CurrencyDebit = recoinciliation.CurrencyDebit,
                                 CurrencyId = recoinciliation.CurrencyId,
                                 EmailReadDate = recoinciliation.EmailReadDate,
                                 EndingDate = recoinciliation.EndingDate,
                                 Guid = recoinciliation.Guid,
                                 Id = recoinciliation.Id,
                                 IsEmailRead = recoinciliation.IsEmailRead,
                                 IsResultSucceed = recoinciliation.IsResultSucceed,
                                 IsSendEmail = recoinciliation.IsSendEmail,
                                 ResultDate = recoinciliation.ResultDate,
                                 ResultNote = recoinciliation.ResultNote,
                                 SendEmailDate = recoinciliation.SendEmailDate,
                                 StartingDate = recoinciliation.StartingDate,
                                 CurrencyCode = currency.Code,
                                 AccountEmail = account.Email,
                                 AccountCode = account.Code
                             };
                return result.FirstOrDefault();
            }
        }

        public AccountReconciliationsCountDto GetCountDto(int companyId)
        {
            using (var context = new ContextDb())
            {
                var result = context.AccountReconciliations.Where(p => p.CompanyId == companyId).ToList();
                AccountReconciliationsCountDto accountReconciliationsCountDto = new AccountReconciliationsCountDto
                {
                    AllReconciliation = result.Count(),
                    SucceedReconciliation = result.Where(p => p.IsResultSucceed == true).Count(),
                    NotResponseReconciliation = result.Where(p => p.IsResultSucceed == null).Count(),
                    FailReconciliation = result.Where(p => p.IsResultSucceed == false).Count()
                };
                return accountReconciliationsCountDto;
            }
        }
    }
}
