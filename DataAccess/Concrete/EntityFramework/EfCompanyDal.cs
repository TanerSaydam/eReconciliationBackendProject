using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCompanyDal : EfEntityRepositoryBase<Company, ContextDb>, ICompanyDal
    {
        public UserCompany GetCompany(int userId)
        {
            using (var context = new ContextDb())
            {
                var result = context.UserCompanies.Where(p => p.UserId == userId).FirstOrDefault();
                return result;
                
            }
        }

        public List<Company> GetListByUserId(int userId)
        {
            using (var context = new ContextDb())
            {                
                var result = from userCompany in context.UserCompanies.Where(p => p.UserId == userId)
                             join company in context.Companies on userCompany.CompanyId equals company.Id
                             select new Company
                             {
                                 Id = company.Id,
                                 AddedAt = company.AddedAt,
                                 Address = company.Address,
                                 IdentityNumber = company.IdentityNumber,
                                 IsActive = company.IsActive,
                                 Name = company.Name,
                                 TaxDepartment = company.TaxDepartment,
                                 TaxIdNumber = company.TaxIdNumber
                             };
                return result.OrderBy(p => p.Name).ToList();
            }
        }

        public void UserCompanyAdd(int userId, int companyId)
        {
            using (var context = new ContextDb())
            {
                UserCompany userCompany = new UserCompany()
                {
                    UserId = userId,
                    CompanyId = companyId,
                    AddedAt = DateTime.Now,
                    IsActive = true
                };
                context.UserCompanies.Add(userCompany);
                context.SaveChanges();
            }
        }
    }
}
