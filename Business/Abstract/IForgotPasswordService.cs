using Core.Entities.Concrete;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IForgotPasswordService
    {
        IDataResult<ForgotPassword> CreateForgotPassword(User user);
        IDataResult<List<ForgotPassword>> GetListByUserId(int userId);
        ForgotPassword GetForgotPassword(string value);
        void Update(ForgotPassword forgotPassword);
    }
}
