using Core.Entities.Concrete;
using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ICompanyService
    {
        //CRUD
        IResult Add(Company company);
        IResult Update(Company company);
        IDataResult<Company> GetById(int id);
        IResult AddCompanyAndUserCompany(CompanyDto companyDto);
        IDataResult<List<Company>> GetList();
        IDataResult<List<Company>> GetListByUserId(int userId);
        IDataResult<UserCompany> GetCompany(int userId);
        IResult CompanyExists(Company company);
        IResult UserCompanyAdd(int userId, int companyId);        
    }
}
