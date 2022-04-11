using Business.Abstract;
using Business.BusinessAcpects;
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
    public class TermsandConditionManager : ITermsandConditionService
    {
        private readonly ITermsandConditionDal _termsandConditionDal;

        public TermsandConditionManager(ITermsandConditionDal termsandConditionDal)
        {
            _termsandConditionDal = termsandConditionDal;
        }

        //[SecuredOperation("Admin")]
        public IDataResult<TermsandCondition> Get()
        {
            return new SuccesDataResult<TermsandCondition>(_termsandConditionDal.GetList().FirstOrDefault());
        }

        [SecuredOperation("Admin")]
        public IResult Update(TermsandCondition termsandCondition)
        {
            var result = _termsandConditionDal.GetList().FirstOrDefault();
            if (result != null)
            {
                result.Description = termsandCondition.Description;
                _termsandConditionDal.Update(result);
            }
            else
            {
                _termsandConditionDal.Add(termsandCondition);
            }
            return new SuccessResult(Messages.UpdateTermsAndConditions);
        }
    }
}
