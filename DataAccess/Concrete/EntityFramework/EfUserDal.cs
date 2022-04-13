using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
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
    public class EfUserDal : EfEntityRepositoryBase<User, ContextDb>, IUserDal
    {
        public List<AdminCompaniesForUserDto> GetAdminCompaniesForUser(int adminUserId, int userUserId)
        {
            using (var context = new ContextDb())
            {
                var result = from userCompany in context.UserCompanies.Where(p => p.UserId == adminUserId)
                             join company in context.Companies on userCompany.CompanyId equals company.Id
                             select new AdminCompaniesForUserDto
                             {
                                 Id = company.Id,
                                 AddedAt = company.AddedAt,
                                 Address = company.Address,
                                 IdentityNumber = company.IdentityNumber,
                                 IsActive = company.IsActive,
                                 Name = company.Name,
                                 TaxDepartment = company.TaxDepartment,
                                 TaxIdNumber = company.TaxIdNumber,
                                 IsTrue = (context.UserCompanies.Where(p => p.UserId == userUserId && p.CompanyId == company.Id).Count() > 0 ? true : false)
                             };
                return result.Where(p=>p.IsTrue == false).OrderBy(p => p.Name).ToList();
            }
        }

        public List<OperationClaim> GetClaims(User user, int companyId)
        {
            using (var context = new ContextDb())
            {
                var result = from operationClaim in context.OperationClaims
                             join userOperationClaim in context.UserOperationClaims
                             on operationClaim.Id equals userOperationClaim.OperationClaimId
                             where userOperationClaim.CompanyId == companyId && userOperationClaim.UserId == user.Id
                             select new OperationClaim
                             {
                                 Id = operationClaim.Id,
                                 Name = operationClaim.Name,
                             };

                return result.ToList();
            }
        }

        public List<OperationClaimForUserListDto> GetOperationClaimListForUser(string value, int companyId)
        {
            using (var context = new ContextDb())
            {
                var user = context.Users.Where(p=> p.MailConfirmValue == value).FirstOrDefault();

                var result = from operationClaim in context.OperationClaims
                             where operationClaim.Name != "Admin" && !operationClaim.Name.Contains("UserOperationClaim")
                             select new OperationClaimForUserListDto
                             {
                                 Id = operationClaim.Id,
                                 Name = operationClaim.Name,
                                 Description = operationClaim.Description,
                                 Status = (context.UserOperationClaims.Where(p => p.UserId == user.Id && p.OperationClaimId == operationClaim.Id && p.CompanyId == companyId).Count() > 0 ? true : false),
                                 UserName = user.Name,
                                 UserId = user.Id,
                                 CompanyId = companyId
                             };
                return result.OrderBy(p => p.Name).ToList();
            }
        }

        public List<Company> GetUserCompanyList(string value)
        {
            using (var context = new ContextDb())
            {
                var user = context.Users.Where(p=> p.MailConfirmValue == value).FirstOrDefault();
                var result = from userCompany in context.UserCompanies.Where(p => p.UserId == user.Id)
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

        public List<UserCompanyDtoForList> GetUserList(int companyId)
        {
            using (var context = new ContextDb())
            {
                var result = from userCompany in context.UserCompanies.Where(p => p.CompanyId == companyId && p.IsActive == true)
                             join user in context.Users on userCompany.UserId equals user.Id
                             join company in context.Companies on userCompany.CompanyId equals company.Id
                             select new UserCompanyDtoForList
                             {
                                 Id = userCompany.Id,
                                 UserId = userCompany.UserId,
                                 CompanyId = companyId,
                                 CompanyName = company.Name,
                                 Email = user.Email,
                                 Name = user.Name,
                                 UserAddedAt = user.AddedAt,
                                 UserIsActive = user.IsActive,
                                 UserMailValue = user.MailConfirmValue
                             }; 
                return result.OrderBy(o=> o.Name).ToList();
            }
        }
    }
}
