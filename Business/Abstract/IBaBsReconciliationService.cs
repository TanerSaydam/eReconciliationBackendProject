using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IBaBsReconciliationService
    {
        IResult Add(BaBsReconciliation babsReconciliation);
        IResult AddToExcel(string filePath, int companyId);
        IResult Update(BaBsReconciliation babsReconciliationn);
        IResult Delete(BaBsReconciliation babsReconciliation);
        IDataResult<BaBsReconciliation> GetById(int id);
        IDataResult<List<BaBsReconciliation>> GetList(int companyId);
    }
}
