using Core.DataAccess;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IAccountReconciliationDal : IEntityRepository<AccountReconciliation>
    {
        List<AccountReconciliationDto> GetAllDto(int companyId);
        AccountReconciliationDto GetByIdDto(int id);
        AccountReconciliationDto GetByCodeDto(string code);
        AccountReconciliationsCountDto GetCountDto(int companyId);
    }
}
