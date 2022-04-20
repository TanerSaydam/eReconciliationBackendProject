using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailTemplatesController : ControllerBase
    {
        private readonly IMailTemplateService _mailTemplateService;

        public MailTemplatesController(IMailTemplateService mailTemplateService)
        {
            _mailTemplateService = mailTemplateService;
        }

        [HttpPost("add")]
        public IActionResult Add(MailTemplate mailTemplate)
        {
            var result = _mailTemplateService.Add(mailTemplate);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("update")]
        public IActionResult Update(MailTemplate mailTemplate)
        {
            var result = _mailTemplateService.Update(mailTemplate);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("getByCompanyId")]
        public IActionResult GetByCompanyId(int companyId)
        {
            var result = _mailTemplateService.GetByCompanyId(companyId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
}
