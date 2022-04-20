using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IMailParameterService
    {
        IResult Update(MailParameter mailParameter);
        IDataResult<MailParameter> Get(int companyId);
        IResult ConnectionTest(int companyId);
    }
}
