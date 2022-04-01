using Business.Abstract;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AccountReconciliationManager : IAccountReconciliationService
    {
        private readonly IAccountReconciliationDal _accountReconciliationDal;

        public AccountReconciliationManager(IAccountReconciliationDal accountReconciliationDal)
        {
            _accountReconciliationDal = accountReconciliationDal;
        }
    }
}
