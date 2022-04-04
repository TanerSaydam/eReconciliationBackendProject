using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAccountReconciliationDetailService
    {
        IResult Add(AccountReconciliationDetail accountReconciliationDetail);
        IResult AddToExcel(string filePath, int companyId);
        IResult Update(AccountReconciliationDetail accountReconciliationDetail);
        IResult Delete(AccountReconciliationDetail accountReconciliationDetail);
        IDataResult<AccountReconciliationDetail> GetById(int id);
        IDataResult<List<AccountReconciliationDetail>> GetList(int accountReconciliationId);
    }
}
