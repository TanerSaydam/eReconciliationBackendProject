using Core.DataAccess;
using Core.Entities.Concrete;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IUserDal : IEntityRepository<User>
    {
        List<OperationClaim> GetClaims(User user, int companyId);
        List<UserCompanyDtoForList> GetUserList(int companyId);
        List<OperationClaimForUserListDto> GetOperationClaimListForUser(string value, int companyId);
        List<AdminCompaniesForUserDto> GetAdminCompaniesForUser(int adminUserId, int userUserId);
        List<Company> GetUserCompanyList(string value);
    }
}
