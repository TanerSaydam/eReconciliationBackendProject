using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IBaBsReconciliationDetailService
    {
        IResult Add(BaBsReconciliationDetail babsReconciliationDetail);
        IResult AddToExcel(string filePath, int companyId);
        IResult Update(BaBsReconciliationDetail babsReconciliationDetail);
        IResult Delete(BaBsReconciliationDetail babsReconciliationDetail);
        IDataResult<BaBsReconciliationDetail> GetById(int id);
        IDataResult<List<BaBsReconciliationDetail>> GetList(int babsReconciliationId);
    }

}
