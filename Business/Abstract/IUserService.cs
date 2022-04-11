using Core.Entities.Concrete;
using Core.Utilities.Results.Abstract;
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
        User GetById(int id);
        IDataResult<User> GetByIdToResult(int id);
        User GetByMail(string email);
        User GetByMailConfirmValue(string value);
        IDataResult<List<UserCompanyDtoForList>> GetUserList(int companyId);
    }
}
