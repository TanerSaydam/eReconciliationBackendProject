using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IUserThemeOptionService
    {        
        IResult Update(UserThemeOption userThemeOption);          
        void Delete(UserThemeOption userThemeOption);
        IDataResult<UserThemeOption> GetByUserId(int userId);
    }
}
