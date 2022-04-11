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
    public class EfUserOperationClaimDal : EfEntityRepositoryBase<UserOperationClaim, ContextDb>, IUserOperationClaimDal
    {
        public List<UserOperationClaimDto> GetListDto(int userId, int companyId)
        {
            using (var context = new ContextDb())
            {
                var result = from userOperationClam in context.UserOperationClaims.Where(x => x.UserId == userId && x.CompanyId == companyId)
                             join operationClaim in context.OperationClaims on userOperationClam.OperationClaimId equals operationClaim.Id
                             select new UserOperationClaimDto
                             {
                                 UserId = userId,
                                 Id = operationClaim.Id,
                                 CompanyId = companyId,
                                 OperationClaimId = operationClaim.Id,
                                 OperationClaimDescription = operationClaim.Description,
                                 OperationClaimName = operationClaim.Name
                             };
                return result.ToList();
                    


            }
        }
    }
}
