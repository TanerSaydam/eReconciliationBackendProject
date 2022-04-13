using Business.Abstract;
using Business.Constans;
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
    public class UserThemeOptionManager : IUserThemeOptionService
    {
        private readonly IUserThemeOptionDal _userThemeOptionDal;

        public UserThemeOptionManager(IUserThemeOptionDal userThemeOptionDal)
        {
            _userThemeOptionDal = userThemeOptionDal;
        }

        public void Delete(UserThemeOption userThemeOption)
        {
            _userThemeOptionDal.Delete(userThemeOption);
        }

        public IDataResult<UserThemeOption> GetByUserId(int userId)
        {
            return new SuccesDataResult<UserThemeOption>(_userThemeOptionDal.Get(p => p.UserId == userId));
        }

        public IResult Update(UserThemeOption userThemeOption)
        {
            var result = _userThemeOptionDal.Get(p => p.UserId == userThemeOption.UserId);
            if (result == null)
            {
                userThemeOption.Id = 0;
                _userThemeOptionDal.Add(userThemeOption);
            }
            else
            {
                _userThemeOptionDal.Update(userThemeOption);
            }
            return new SuccessResult(Messages.UpdatedUserTheme);
        }
    }
}
