using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TermsAndConditionsController : ControllerBase
    {
        private readonly ITermsandConditionService _termsandConditionService;

        public TermsAndConditionsController(ITermsandConditionService termsandConditionService)
        {
            _termsandConditionService = termsandConditionService;
        }

        [HttpGet("get")]
        public IActionResult Get()
        {
            var result = _termsandConditionService.Get();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("update")]
        public IActionResult Update(TermsandCondition termsandCondition)
        {
            var result = _termsandConditionService.Update(termsandCondition);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
}
