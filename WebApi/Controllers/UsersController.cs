using Business.Abstract;
using Core.Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public UsersController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpGet("getUserList")]
        public IActionResult GetUserList(int companyId)
        {
            var result = _userService.GetUserList(companyId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("getById")]
        public IActionResult GetById(int id)
        {
            var result = _userService.GetByIdToResult(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("update")]
        public IActionResult Update(UserForRegisterToSecondAccountDto userForRegister)
        {
            var findUser = _userService.GetById(userForRegister.Id);
            if (findUser.Email != userForRegister.Email)
            {
                var userExists = _authService.UserExists(userForRegister.Email);
                if (!userExists.Success)
                {
                    return BadRequest(userExists.Message);
                }
            }            

            var result = _userService.UpdateResult(userForRegister);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("changeStatus")]
        public IActionResult ChangeStatus(int id)
        {
            var findUser = _userService.GetById(id);
            if (findUser.IsActive)
            {
                findUser.IsActive = false;
            }
            else
            {
                findUser.IsActive = true;
            }

            var result = _authService.Update(findUser);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
}
