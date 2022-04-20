using Business.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailParameterController : ControllerBase
    {
        private readonly IMailParameterService _mailParameterService;
        private readonly IMailService _mailService;
        public MailParameterController(IMailParameterService mailParameterService, IMailService mailService)
        {
            _mailParameterService = mailParameterService;
            _mailService = mailService;
        }

        [HttpPost("update")]
        public IActionResult Update(MailParameter mailParameter)
        {
            var result = _mailParameterService.Update(mailParameter);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("getById")]
        public IActionResult GetById(int id)
        {
            var result = _mailParameterService.Get(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("connectionTest")]
        public IActionResult ConnectionTest(int companyId)
        {
            var result = _mailParameterService.ConnectionTest(companyId);            
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

    }
}
