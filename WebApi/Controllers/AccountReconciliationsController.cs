using Business.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountReconciliationsController : ControllerBase
    {
        private readonly IAccountReconciliationService _accountReconciliationService;

        public AccountReconciliationsController(IAccountReconciliationService accountReconciliationService)
        {
            _accountReconciliationService = accountReconciliationService;
        }

        [HttpPost("addFromExcel")]
        public IActionResult AddFromExcel(IFormFile file, int companyId)
        {
            if (file.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + ".xlsx";
                var filePath = $"{Directory.GetCurrentDirectory()}/Content/{fileName}";
                using (FileStream stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                    stream.Flush();
                }

                var result = _accountReconciliationService.AddToExcel(filePath, companyId);
                if (result.Success)
                {
                    return Ok(result);
                }
                return BadRequest(result.Message);
            }
            return BadRequest("Dosya seçimi yapmadınız");
        }

        [HttpPost("add")]
        public IActionResult Add(AccountReconciliation accountReconciliation)
        {
            var result = _accountReconciliationService.Add(accountReconciliation);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("update")]
        public IActionResult Update(AccountReconciliation accountReconciliation)
        {
            var result = _accountReconciliationService.Update(accountReconciliation);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("updateResult")]
        public IActionResult UpdateResult(AccountReconciliation accountReconciliation)
        {            
            var result = _accountReconciliationService.Update(accountReconciliation);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("delete")]
        public IActionResult Delete(int id)
        {
            var result = _accountReconciliationService.Delete(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("getById")]
        public IActionResult GetById(int id)
        {
            var result = _accountReconciliationService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("getList")]
        public IActionResult GetList(int companyId)
        {
            var result = _accountReconciliationService.GetListDto(companyId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("getCount")]
        public IActionResult GetCount(int companyId)
        {
            var result = _accountReconciliationService.GetCountDto(companyId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        //SendReconciliationMail(AccountReconciliationDto accountReconciliationDto)
        [HttpGet("sendReconciliationMail")]
        public IActionResult SendReconciliationMail(int id)
        {
            var result = _accountReconciliationService.SendReconciliationMail(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("getByCode")]
        public IActionResult GetByCode(string code)
        {
            var result = _accountReconciliationService.GetByCode(code);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("getByCodeDto")]
        public IActionResult GetByCodeDto(string code)
        {
            var result = _accountReconciliationService.GetByCodeDto(code);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("sendResult")]
        public IActionResult sendResult(ReconciliationResultDto reconciliationResultDto)
        {
            var result = _accountReconciliationService.Result(reconciliationResultDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
}
