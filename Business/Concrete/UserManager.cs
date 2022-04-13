using Business.Abstract;
using Business.BusinessAcpects;
using Business.Constans;
using Business.ValidaitonRules.FluentValidation;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Core.Entities.Concrete;
using Core.Utilities.Hashing;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        private IUserDal _userDal;
        private IOperationClaimService _operationClaimService;
        private IUserOperationClaimService _userOperationClaimService;
        private IUserCompanyService _userCompanyService;
        private ICompanyService _companyService;
        private IUserReletionShipService _userReletionShipService;
        private IUserThemeOptionService _userThemeOptionService;

        public UserManager(IUserDal userDal, IUserOperationClaimService userOperationClaimService, IUserCompanyService userCompanyService, IOperationClaimService operationClaimService, ICompanyService companyService, IUserReletionShipService userReletionShipService, IUserThemeOptionService userThemeOptionService)
        {
            _userDal = userDal;
            _userOperationClaimService = userOperationClaimService;
            _userCompanyService = userCompanyService;   
            _operationClaimService = operationClaimService;
            _companyService = companyService;
            _userReletionShipService = userReletionShipService; 
            _userThemeOptionService = userThemeOptionService;   
        }

        [CacheRemoveAspect("IUserService.Get")]
        [ValidationAspect(typeof(UserValidator))]
        public void Add(User user)
        {
            _userDal.Add(user);
        }

        public IDataResult<List<AdminCompaniesForUserDto>> GetAdminCompaniesForUser(int adminUserId, int userUserId)
        {
            return new SuccesDataResult<List<AdminCompaniesForUserDto>>(_userDal.GetAdminCompaniesForUser(adminUserId, userUserId));
        }

        [CacheAspect(60)]
        public User GetById(int id)
        {
            return _userDal.Get(u=> u.Id == id);
        }
        
        [CacheAspect(60)]
        public IDataResult<User> GetByIdToResult(int id)
        {
            return new SuccesDataResult<User>(_userDal.Get(u => u.Id == id));
        }

        [CacheAspect(60)]
        public User GetByMail(string email)
        {
            return _userDal.Get(p=> p.Email == email);
        }

        [CacheAspect(60)]
        public User GetByMailConfirmValue(string value)
        {
            return _userDal.Get(p=> p.MailConfirmValue == value); 
        }
        
        public List<OperationClaim> GetClaims(User user, int companyId)
        {
            return _userDal.GetClaims(user, companyId);
        }

        [SecuredOperation("User.Update,Admin")]
        public IDataResult<List<OperationClaimForUserListDto>> GetOperationClaimListForUser(string value, int companyId)
        {
            return new SuccesDataResult<List<OperationClaimForUserListDto>>(_userDal.GetOperationClaimListForUser(value, companyId));
        }

        public IDataResult<List<Company>> GetUserCompanyList(string value)
        {
            return new SuccesDataResult<List<Company>>(_userDal.GetUserCompanyList(value));
        }

        [PerformanceAspect(3)]
        [SecuredOperation("User.GetList,Admin")]
        public IDataResult<List<UserCompanyDtoForList>> GetUserList(int companyId)
        {
            return new SuccesDataResult<List<UserCompanyDtoForList>>(_userDal.GetUserList(companyId));
        }

        [PerformanceAspect(3)]
        //[SecuredOperation("User.Update,Admin")]
        [CacheRemoveAspect("IUserService.Get")]
        public void Update(User user)
        {
            _userDal.Update(user);
        }

        [SecuredOperation("User.Update,Admin")]
        public IResult UpdateOperationClaim(OperationClaimForUserListDto operationClaim)
        {
            if (operationClaim.Status == true)
            {
                var result = _userOperationClaimService.GetList(operationClaim.UserId,operationClaim.CompanyId).Data.Where(p=> p.OperationClaimId == operationClaim.Id).FirstOrDefault();
                _userOperationClaimService.Delete(result);
            }
            else
            {
                UserOperationClaim userOperationClaim = new UserOperationClaim()
                {
                    CompanyId = operationClaim.CompanyId,
                    AddedAt = DateTime.Now,
                    IsActive = true,
                    OperationClaimId = operationClaim.Id,
                    UserId = operationClaim.UserId
                };
                _userOperationClaimService.Add(userOperationClaim);
            }

            return new SuccessResult(Messages.UpdatedUserOperationClaim);
        }

        [CacheRemoveAspect("IUserService.Get")]
        public IResult UpdateResult(UserForRegisterToSecondAccountDto userForRegister)
        {
            var findUser = _userDal.Get(i => i.Id == userForRegister.Id);
            findUser.Name = userForRegister.Name;
            findUser.Email = userForRegister.Email;

            if (userForRegister.Password != "")
            {
                byte[] passwordHash, passwordSalt;
                HashingHelper.CreatePasswordHash(userForRegister.Password, out passwordHash, out passwordSalt);
                findUser.PasswordHash = passwordHash;
                findUser.PasswordSalt = passwordSalt;
            }

            _userDal.Update(findUser);
            return new SuccessResult(Messages.UpdatedUser);
        }

        [TransactionScopeAspect]
        public IResult UserCompanyAdd(int userId, int companyId)
        {
            _companyService.UserCompanyAdd(userId, companyId);

            var operationClaims = _operationClaimService.GetList().Data;
            foreach (var operationClaim in operationClaims)
            {
                if (operationClaim.Name != "Admin" && operationClaim.Name != "MailParameter" && operationClaim.Name != "MailTemplete")
                {
                    UserOperationClaim userOperation = new UserOperationClaim()
                    {
                        CompanyId = companyId,
                        AddedAt = DateTime.Now,
                        IsActive = true,
                        OperationClaimId = operationClaim.Id,
                        UserId = userId
                    };
                    _userOperationClaimService.Add(userOperation);
                }
            }
            return new SuccessResult(Messages.AddedUserCompanyReletionShip);
        }

        [TransactionScopeAspect]
        public IResult UserCompanyDelete(int userId, int companyId)
        {
            var userOperationClaims = _userOperationClaimService.GetList(userId, companyId).Data;
            foreach (var userOperationCliam in userOperationClaims)
            {
                _userOperationClaimService.Delete(userOperationCliam);
            }

            var result = _userCompanyService.GetByUserIdAndCompanyId(userId, companyId);
            _userCompanyService.Delete(result);


            return new SuccessResult(Messages.DeletedUserCompanyReletionShip);
        }

        [TransactionScopeAspect]
        public IResult UserDelete(int userId)
        {
            var userCompanies = _userCompanyService.GetListByUserId(userId);
            foreach (var company in userCompanies)
            {
                var userOperationClaims = _userOperationClaimService.GetList(userId, company.CompanyId).Data;
                foreach (var userOperationCliam in userOperationClaims)
                {
                    _userOperationClaimService.Delete(userOperationCliam);
                }

                var result = _userCompanyService.GetByUserIdAndCompanyId(userId, company.CompanyId);
                _userCompanyService.Delete(result);
            }

            var userReletionShips = _userReletionShipService.GetList(userId);
            foreach (var userReletionShip in userReletionShips)
            {
                _userReletionShipService.Delete(userReletionShip);
            }

            var theme = _userThemeOptionService.GetByUserId(userId).Data;
            _userThemeOptionService.Delete(theme);

            var user = _userDal.Get(p => p.Id == userId);
            _userDal.Delete(user);
            return new SuccessResult(Messages.DeletedUser);
            
        }
    }
}
