using Business.Abstract;
using Business.BusinessAcpects;
using Business.Constans;
using Core.Entities.Concrete;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserOperationClaimManager : IUserOperationClaimService
    {
        private readonly IUserOperationClaimDal _userOperationClaimDal;

        public UserOperationClaimManager(IUserOperationClaimDal userOperationClaimDal)
        {
            _userOperationClaimDal = userOperationClaimDal;
        }

        //[SecuredOperation("Admin,UserOperationClaim.Add")]
        public IResult Add(UserOperationClaim userOperationClaim)
        {
            _userOperationClaimDal.Add(userOperationClaim);
            return new SuccessResult(Messages.AddedUserOperationClaim);
        }

        [SecuredOperation("Admin,UserOperationClaim.Delete")]
        public IResult Delete(UserOperationClaim userOperationClaim)
        {
            _userOperationClaimDal.Delete(userOperationClaim);
            return new SuccessResult(Messages.DeletedUserOperationClaim);
        }

        [SecuredOperation("Admin,UserOperationClaim.Get")]
        public IDataResult<UserOperationClaim> GetById(int id)
        {
            return new SuccesDataResult<UserOperationClaim>(_userOperationClaimDal.Get(i => i.Id == id));
        }

        [SecuredOperation("Admin,UserOperationClaim.GetList")]
        public IDataResult<List<UserOperationClaim>> GetList(int userId, int companyId)
        {
            return new SuccesDataResult<List<UserOperationClaim>>(_userOperationClaimDal.GetList(p=> p.UserId == userId && p.CompanyId == companyId));
        }

        public IDataResult<List<UserOperationClaimDto>> GetListDto(int userId, int companyId)
        {
            return new SuccesDataResult<List<UserOperationClaimDto>>(_userOperationClaimDal.GetListDto(userId,companyId));
        }

        [SecuredOperation("Admin,UserOperationClaim.Update")]
        public IResult Update(UserOperationClaim userOperationClaim)
        {
            _userOperationClaimDal.Update(userOperationClaim);
            return new SuccessResult(Messages.UpdatedUserOperationClaim);
        }
    }
}
