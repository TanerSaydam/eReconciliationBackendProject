using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ForgotPasswordManager : IForgotPasswordService
    {
        private readonly IForgotPasswordDal _forgotPasswordDal;

        public ForgotPasswordManager(IForgotPasswordDal forgotPasswordDal)
        {
            _forgotPasswordDal = forgotPasswordDal;
        }

        public IDataResult<ForgotPassword> CreateForgotPassword(User user)
        {
            ForgotPassword forgotPassword = new ForgotPassword()
            {
                IsActive = true,
                SendDate = DateTime.Now,
                UserId = user.Id,
                Value = Guid.NewGuid().ToString()
            };
            _forgotPasswordDal.Add(forgotPassword);
            return new SuccesDataResult<ForgotPassword>(forgotPassword);
        }

        public ForgotPassword GetForgotPassword(string value)
        {
            return _forgotPasswordDal.Get(p => p.Value == value);
        }

        public IDataResult<List<ForgotPassword>> GetListByUserId(int userId)
        {
            return new SuccesDataResult<List<ForgotPassword>>(_forgotPasswordDal.GetList(p=> p.UserId == userId && p.IsActive == true));
        }

        public void Update(ForgotPassword forgotPassword)
        {
            _forgotPasswordDal.Update(forgotPassword);
        }
    }
}
