using Business.Abstract;
using Core.Utilities.Hashing;
using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IForgotPasswordService _forgotPasswordService;

        public AuthController(IAuthService authService, IForgotPasswordService forgotPasswordService)
        {
            _authService = authService;
            _forgotPasswordService = forgotPasswordService;
        }

        [HttpPost("register")]
        public IActionResult Register(UserAndCompanyRegisterDto userAndCompanyRegister)
        {
            //var userExists = _authService.UserExists(userAndCompanyRegister.UserForRegister.Email);
            //if (!userExists.Success)
            //{
            //    return BadRequest(userExists.Message);
            //}

            //var companyExists = _authService.CompanyExists(userAndCompanyRegister.Company);
            //if (!companyExists.Success)
            //{
            //    return BadRequest(userExists.Message);
            //}

            var registerResult = _authService.Register(userAndCompanyRegister.UserForRegister, userAndCompanyRegister.UserForRegister.Password, userAndCompanyRegister.Company);

            var result = _authService.CreateAccessToken(registerResult.Data, registerResult.Data.CompanyId);

            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(registerResult.Message);
        }

        [HttpPost("registerSecondAccount")]
        public IActionResult RegisterSecondAccount(UserForRegisterToSecondAccountDto userForRegister)
        {
            var userExists = _authService.UserExists(userForRegister.Email);
            if (!userExists.Success)
            {
                return BadRequest(userExists.Message);
            }

            var registerResult = _authService.RegisterSecondAccount(userForRegister, userForRegister.Password, userForRegister.CompanyId, userForRegister.AdminUserId);
            if (registerResult.Success)
            {
                return Ok(registerResult);
            }

            return BadRequest(registerResult.Message);
        }

        [HttpPost("login")]
        public IActionResult Login(UserForLogin userForLogin)
        {
            var userToLogin = _authService.Login(userForLogin);
            if (!userToLogin.Success)
            {
                return BadRequest(userToLogin.Message);
            }

            if (userToLogin.Data.IsActive)
            {
                if (userToLogin.Data.MailConfirm)
                {
                    var userCompany = _authService.GetCompany(userToLogin.Data.Id).Data;
                    var result = _authService.CreateAccessToken(userToLogin.Data, userCompany.CompanyId);
                    if (result.Success)
                    {
                        return Ok(result);
                    }
                    return BadRequest(result);
                }
                return BadRequest("Gelen onay mailini cevaplamalısınız. Mail adresinizi onaylamadan sisteme giriş yapamazsınız!");

            }
            return BadRequest("Kullanıcı pasif durumda. Aktif etmek için yöneticinize danışın");


        }

        [HttpGet("changeCompany")]
        public IActionResult ChangeCompany(int userId, int companyId)
        {
            var user = _authService.GetById(userId).Data;
            var result = _authService.CreateAccessToken(user, companyId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("confirmuser")]
        public IActionResult ConfirmUser(string value)
        {
            var user = _authService.GetByMailConfirmValue(value).Data;
            if (user.MailConfirm)
            {
                return BadRequest("Kullanıcı maili zaten onaylı. Aynı maili tekrar onaylayamazsınız!");
            }

            user.MailConfirm = true;
            user.MailConfirmDate = DateTime.Now;
            var result = _authService.Update(user);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("sendConfirmEmail")]
        public IActionResult SendConfirmEmail(string email)
        {
            var user = _authService.GetByEmail(email).Data;

            if (user == null)
            {
                return BadRequest("Kullanıcı bulunamadı!");
            }

            if (user.MailConfirm)
            {
                return BadRequest("Kullanıcının maili onaylı!");
            }

            var result = _authService.SendConfirmEmailAgain(user);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("forgotPassword")]
        public IActionResult ForgotPassword(string email)
        {
            var user = _authService.GetByEmail(email).Data;

            if (user == null)
            {
                return BadRequest("Kullanıcı bulunamadı!");
            }

            var lists = _forgotPasswordService.GetListByUserId(user.Id).Data;
            foreach (var item in lists)
            {
                item.IsActive = false;
                _forgotPasswordService.Update(item);
            }

            var forgotPassword = _forgotPasswordService.CreateForgotPassword(user).Data;

            var result = _authService.SendForgotPasswordEmail(user, forgotPassword.Value);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("forgotPasswordLinkCheck")]
        public IActionResult ForgotPasswordLinkCheck(string value)
        {
            var result = _forgotPasswordService.GetForgotPassword(value);
            if (result == null)
            {
                return BadRequest("Tıkladığınız link geçersiz!");
            }

            if (result.IsActive == true)
            {
                DateTime date1 = DateTime.Now.AddHours(-1);
                DateTime date2 = DateTime.Now;
                if (result.SendDate >= date1 && result.SendDate <= date2)
                {
                    return Ok(true);
                }
                else
                {
                    return BadRequest("Tıkladığınız link geçersiz!");
                }
            }
            else
            {
                return BadRequest("Tıkladığınız link geçersiz!");
            }
        }

        [HttpPost("changePasswordToForgotPassword")]
        public IActionResult ChangePasswordToForgotPassword(ForgotPasswordDto passwordDto)
        {
            var forgotPasswordResult = _forgotPasswordService.GetForgotPassword(passwordDto.Value);
            forgotPasswordResult.IsActive = false;
            _forgotPasswordService.Update(forgotPasswordResult);
            var userResult = _authService.GetById(forgotPasswordResult.UserId).Data;
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(passwordDto.Password, out passwordHash, out passwordSalt);
            userResult.PasswordHash = passwordHash;
            userResult.PasswordSalt = passwordSalt;
            var result = _authService.ChangePassword(userResult);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
}
