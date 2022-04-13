using Core.Entities.Concrete;
using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IUserService
    {
        List<OperationClaim> GetClaims(User user, int companyId);
        void Add(User user);
        void Update(User user);        
        IResult UpdateResult(UserForRegisterToSecondAccountDto userForRegister);        
        IResult UpdateOperationClaim(OperationClaimForUserListDto operationClaim);        
        User GetById(int id);
        IDataResult<User> GetByIdToResult(int id);
        User GetByMail(string email);
        User GetByMailConfirmValue(string value);
        IDataResult<List<UserCompanyDtoForList>> GetUserList(int companyId);
        IDataResult<List<OperationClaimForUserListDto>> GetOperationClaimListForUser(string value, int companyId);
        IResult UserCompanyDelete(int userId, int companyId);
        IDataResult<List<AdminCompaniesForUserDto>> GetAdminCompaniesForUser(int adminUserId, int userUserId);
        IResult UserCompanyAdd(int userId, int companyId);
        IResult UserDelete(int userId);
        IDataResult<List<Company>> GetUserCompanyList(string value);
    }
}
