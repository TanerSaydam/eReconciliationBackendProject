using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context;
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

        public List<UserCompanyDtoForList> GetUserList(int companyId)
        {
            using (var context = new ContextDb())
            {
                var result = from userCompany in context.UserCompanies.Where(p => p.CompanyId == companyId && p.IsActive == true)
                             join user in context.Users on userCompany.UserId equals user.Id
                             select new UserCompanyDtoForList
                             {
                                 Id = userCompany.Id,
                                 UserId = userCompany.UserId,
                                 CompanyId = companyId,
                                 Email = user.Email,
                                 Name = user.Name,
                                 UserAddedAt = user.AddedAt,
                                 UserIsActive = user.IsActive
                             };
                return result.OrderBy(o=> o.Name).ToList();
            }
        }
    }
}
