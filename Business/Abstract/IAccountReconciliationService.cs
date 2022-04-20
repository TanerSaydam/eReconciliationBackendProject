using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAccountReconciliationService
    {
        IResult Add(AccountReconciliation accountReconciliation);
        IResult AddToExcel(string filePath, int companyId);
        IResult Update(AccountReconciliation accountReconciliation);
        IResult UpdateResult(AccountReconciliation accountReconciliation);
        IResult Delete(int id);
        IResult Result(ReconciliationResultDto reconciliationResultDto);
        IDataResult<AccountReconciliation> GetById(int id);
        IDataResult<AccountReconciliation> GetByCode(string code);
        IDataResult<List<AccountReconciliation>> GetList(int companyId);
        IDataResult<List<AccountReconciliation>> GetListByCurrencyAccountId(int currencyAccountId);
        IDataResult<List<AccountReconciliationDto>> GetListDto(int companyId);
        IResult SendReconciliationMail(int id);
        IDataResult<AccountReconciliationsCountDto> GetCountDto(int companyId);
        IDataResult<AccountReconciliationDto> GetByCodeDto(string code);

    }
}
