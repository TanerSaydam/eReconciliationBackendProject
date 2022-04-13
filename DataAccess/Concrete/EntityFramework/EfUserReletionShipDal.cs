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
    public class EfUserReletionShipDal : EfEntityRepositoryBase<UserReletionship, ContextDb>, IUserReletionshipDal
    {
        public UserReletionshipDto GetById(int userUserId)
        {
            using (var context = new ContextDb())
            {
                var result = from userReletionShip in context.UserReletionships.Where(p => p.UserUserId == userUserId)
                             join adminUser in context.Users on userReletionShip.AdminUserId equals adminUser.Id
                             join userUser in context.Users on userReletionShip.UserUserId equals userUser.Id
                             select new UserReletionshipDto
                             {
                                 AdminUserId = adminUser.Id,
                                 AdminAddedAt = adminUser.AddedAt,
                                 AdminMail = adminUser.Email,
                                 AdminIsActive = adminUser.IsActive,
                                 AdminUserName = adminUser.Name,
                                 Companies = (from userCompany in context.UserCompanies.Where(p => p.UserId == userUser.Id)
                                              join user in context.Users on userCompany.UserId equals user.Id
                                              join company in context.Companies on userCompany.CompanyId equals company.Id
                                              select new Company
                                              {
                                                  Id = company.Id,
                                                  Name = company.Name,
                                                  TaxDepartment = company.TaxDepartment,
                                                  TaxIdNumber = company.TaxIdNumber,
                                                  IdentityNumber = company.IdentityNumber,
                                                  Address = company.Address,    
                                                  AddedAt = company.AddedAt,
                                                  IsActive = company.IsActive
                                              }).ToList(),
                                 Id = userReletionShip.Id,
                                 UserAddedAt = userUser.AddedAt,
                                 UserIsActive = userUser.IsActive,
                                 UserMail = userUser.Email,
                                 UserUserId = userUser.Id,
                                 UserUserName = userUser.Name,
                                 UserMailValue = userUser.MailConfirmValue
                             };
                return result.FirstOrDefault();
            }
        }

        public List<UserReletionshipDto> GetListDto(int adminUserId)
        {
            using (var context = new ContextDb())
            {
                var result = from userReletionShip in context.UserReletionships.Where(p => p.AdminUserId == adminUserId)
                             join adminUser in context.Users on userReletionShip.AdminUserId equals adminUser.Id
                             join userUser in context.Users on userReletionShip.UserUserId equals userUser.Id
                             select new UserReletionshipDto
                             {
                                 AdminUserId = adminUserId,
                                 AdminAddedAt = adminUser.AddedAt,
                                 AdminMail = adminUser.Email,
                                 AdminIsActive = adminUser.IsActive,
                                 AdminUserName = adminUser.Name,
                                 Companies = (from userCompany in context.UserCompanies.Where(p => p.UserId == userUser.Id)
                                              join user in context.Users on userCompany.UserId equals user.Id
                                              join company in context.Companies on userCompany.CompanyId equals company.Id
                                              select new Company
                                              {
                                                  Id = company.Id,
                                                  Name = company.Name,
                                                  TaxDepartment = company.TaxDepartment,
                                                  TaxIdNumber = company.TaxIdNumber,
                                                  IdentityNumber = company.IdentityNumber,
                                                  Address = company.Address,
                                                  AddedAt = company.AddedAt,
                                                  IsActive = company.IsActive
                                              }).ToList(),
                                 Id = userReletionShip.Id,
                                 UserAddedAt = userUser.AddedAt,
                                 UserIsActive = userUser.IsActive,
                                 UserMail = userUser.Email,
                                 UserUserId = userUser.Id,
                                 UserUserName = userUser.Name,
                                 UserMailValue = userUser.MailConfirmValue
                             };
                return result.OrderBy(p => p.UserUserName).ToList();
            }
        }
    }
}
