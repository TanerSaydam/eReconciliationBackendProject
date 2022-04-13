using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IUserCompanyService
    {
        void Delete(UserCompany userCompany);
        UserCompany GetByUserIdAndCompanyId(int userId, int companyId);
        List<UserCompany> GetListByUserId(int userId);
    }
}
