using Business.Abstract;
using Business.Constans;
using Business.ValidaitonRules.FluentValidation;
using Core.Aspects.Autofac.Transaction;
using Core.CrossCuttingConcerns.Validation;
using Core.Entities.Concrete;
using Core.Utilities.Hashing;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using Core.Utilities.Security.JWT;
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
    public class AuthManager : IAuthService
    {
        private readonly IUserService _userService;
        private readonly ITokenHelper _tokenHelper;
        private readonly ICompanyService _companyService;
        private readonly IMailParameterService _mailParameterService;
        private readonly IMailService _mailService;
        private readonly IMailTemplateService _mailTemplateService;
        private readonly IUserOperationClaimService _userOperarionClaimService;
        private readonly IOperationClaimService _OperarionClaimService;
   
        public AuthManager(IUserService userService, ITokenHelper tokenHelper, ICompanyService companyService, IMailParameterService mailParameterService, IMailService mailService, IMailTemplateService mailTemplateService, IUserOperationClaimService userOperarionClaimService, IOperationClaimService OperarionClaimService)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
            _companyService = companyService;
            _mailParameterService = mailParameterService;
            _mailService = mailService;
            _mailTemplateService = mailTemplateService;
            _userOperarionClaimService = userOperarionClaimService;
            _OperarionClaimService = OperarionClaimService;
        }

        public IResult CompanyExists(Company company)
        {
            var result = _companyService.CompanyExists(company);
            if (result.Success == false)
            {
                return new ErrorResult(Messages.CompanyAlreadyExists);
            }
            return new SuccessResult();
        }

        public IDataResult<AccessToken> CreateAccessToken(User user, int companyId)
        {
            var claims = _userService.GetClaims(user, companyId);
            var company = _companyService.GetById(companyId).Data;
            var accessToken = _tokenHelper.CreateToken(user, claims, companyId, company.Name);
            return new SuccesDataResult<AccessToken>(accessToken, Messages.SuccessfulLogin);
        }

        public IDataResult<User> GetById(int id)
        {
            return new SuccesDataResult<User>(_userService.GetById(id));
        }

        public IDataResult<User> GetByMailConfirmValue(string value)
        {
            return new SuccesDataResult<User>(_userService.GetByMailConfirmValue(value));
        }

        public IDataResult<User> Login(UserForLogin userForLogin)
        {
            var userToCheck = _userService.GetByMail(userForLogin.Email);
            if (userToCheck == null)
            {
                return new ErrorDataResult<User>(Messages.UserNotFound);
            }

            if (!HashingHelper.VerifyPasswordHash(userForLogin.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
            {
                return new ErrorDataResult<User>(Messages.PasswordError);
            }

            return new SuccesDataResult<User>(userToCheck, Messages.SuccessfulLogin);

        }
                    
        [TransactionScopeAspect]
        public IDataResult<UserCompanyDto> Register(UserForRegister userForRegister, string password, Company company)
        {            

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            var user = new User()
            {
                Email = userForRegister.Email,
                AddedAt = DateTime.Now,
                IsActive = true,
                MailConfirm = false,
                MailConfirmDate = DateTime.Now,
                MailConfirmValue = Guid.NewGuid().ToString(),
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Name = userForRegister.Name
            };            

            _userService.Add(user);
            _companyService.Add(company);

            _companyService.UserCompanyAdd(user.Id, company.Id);

            UserCompanyDto userCompanyDto = new UserCompanyDto()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                AddedAt = user.AddedAt,
                CompanyId = company.Id,
                IsActive = true,
                MailConfirm = user.MailConfirm,
                MailConfirmDate = user.MailConfirmDate,
                MailConfirmValue = user.MailConfirmValue,
                PasswordHash = user.PasswordHash,
                PasswordSalt = user.PasswordSalt
            };

            SendConfirmEmail(user);

            return new SuccesDataResult<UserCompanyDto>(userCompanyDto, Messages.UserRegistered);
        }

        void SendConfirmEmail(User user)
        {
            string subject = "Kullanıcı Kayıt Onay Maili";
            string body = "Kullanıcınız sisteme kayıt oldu. Kaydınızı tamamlamak için aşağıdaki linke tıklamanız gerekmektedir.";
            string link = "http://localhost:4200/registerConfirm/" + user.MailConfirmValue;
            string linkDescription = "Kaydı Onaylamak için Tıklayın";

            var mailTemplate = _mailTemplateService.GetByTemplateName("Kayıt", 4);
            string templateBody = mailTemplate.Data.Value;
            templateBody = templateBody.Replace("{{title}}", subject);
            templateBody = templateBody.Replace("{{message}}", body);
            templateBody = templateBody.Replace("{{link}}", link);
            templateBody = templateBody.Replace("{{linkDescription}}", linkDescription);


            var mailParameter = _mailParameterService.Get(4);
            SendMailDto sendMailDto = new SendMailDto()
            {
                mailParameter = mailParameter.Data,
                email = user.Email,
                subject = "Kullanıcı Kayıt Onay Maili",
                body = templateBody
            };

            _mailService.SendMail(sendMailDto);

            user.MailConfirmDate = DateTime.Now;
            _userService.Update(user);
        }

        public IDataResult<User> RegisterSecondAccount(UserForRegister userForRegister, string password, int companyId)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            var user = new User()
            {
                Email = userForRegister.Email,
                AddedAt = DateTime.Now,
                IsActive = true,
                MailConfirm = false,
                MailConfirmDate = DateTime.Now,
                MailConfirmValue = Guid.NewGuid().ToString(),
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Name = userForRegister.Name
            };

            _userService.Add(user);

            _companyService.UserCompanyAdd(user.Id, companyId);

            SendConfirmEmail(user);

            return new SuccesDataResult<User>(user, Messages.UserRegistered);
        }

        public IResult Update(User user)
        {
            _userService.Update(user);
            return new SuccessResult(Messages.UpdatedUser);
        }

        public IResult UserExists(string email)
        {
            if (_userService.GetByMail(email)!= null)
            {
                return new ErrorResult(Messages.UserAlreadyExists);
            }
            return new SuccessResult();
        }

        public IResult SendConfirmEmailAgain(User user)
        {
            if (user.MailConfirm == true)
            {
                return new ErrorResult(Messages.MailAlreadyConfirm);
            }

            DateTime confirmMailDate = user.MailConfirmDate;
            DateTime now = DateTime.Now;
            if (confirmMailDate.ToShortDateString() == now.ToShortDateString())
            {
                if (confirmMailDate.Hour == now.Hour && confirmMailDate.AddMinutes(5).Minute <= now.Minute)
                {
                    SendConfirmEmail(user);
                    return new SuccessResult(Messages.MailConfirmSendSuccessful);
                }
                else
                {
                    return new ErrorResult(Messages.MailConfirmTimeHasNotExpired);
                }
            }
            SendConfirmEmail(user);
            return new SuccessResult(Messages.MailConfirmSendSuccessful);

        }

        public IDataResult<UserCompany> GetCompany(int userId)
        {
            return new SuccesDataResult<UserCompany>(_companyService.GetCompany(userId).Data);
        }

        public IDataResult<User> GetByEmail(string email)
        {
            return new SuccesDataResult<User>(_userService.GetByMail(email));
        }

        public IResult SendForgotPasswordEmail(User user, string value)
        {
            string subject = "Şifremi Unuttum";
            string body = "e-Mutabakat sitemizden şifrenizi unuttuğunuzu belirttiniz. Aşağıdaki linkte tıklayarak şifrenizi yeniden belirleyebilirsiniz. Linkin 1 saat süresi vardır. Süre sonunda kullanılamaz. İyi günler dileriz.";
            string link = "http://localhost:4200/forgot-password/" + value;
            string linkDescription = "Şifrenizi Tekrar Belirlemek için Tıklayın";

            var mailTemplate = _mailTemplateService.GetByTemplateName("Kayıt", 4);
            string templateBody = mailTemplate.Data.Value;
            templateBody = templateBody.Replace("{{title}}", subject);
            templateBody = templateBody.Replace("{{message}}", body);
            templateBody = templateBody.Replace("{{link}}", link);
            templateBody = templateBody.Replace("{{linkDescription}}", linkDescription);


            var mailParameter = _mailParameterService.Get(4);
            SendMailDto sendMailDto = new SendMailDto()
            {
                mailParameter = mailParameter.Data,
                email = user.Email,
                subject = subject,
                body = templateBody
            };

            _mailService.SendMail(sendMailDto);            

            return new SuccessResult(Messages.MailSendSucessful);
        }

        public IResult ChangePassword(User user)
        {
            _userService.Update(user);
            return new SuccessResult(Messages.ChangedPassword);
        }
    }
}
