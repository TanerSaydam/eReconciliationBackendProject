using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCurrencyAccountDal : EfEntityRepositoryBase<CurrencyAccount, ContextDb>, ICurrencyAccountDal
    {
        public bool CheckCurrencyAccountReconciliations(int currencyAccountId)
        {
            using (var context = new ContextDb())
            {
                var reconciliations = context.AccountReconciliations.Where(p => p.CurrencyAccountId == currencyAccountId).ToList();
                if (reconciliations.Count() > 0)
                {
                    return false;
                }

                var babsReconciliations = context.BaBsReconciliations.Where(p => p.CurrencyAccountId == currencyAccountId).ToList();
                if (reconciliations.Count() > 0)
                {
                    return false;
                }
                return true;
            }
        }
    }
}
