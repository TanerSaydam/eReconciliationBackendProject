using Business.Abstract;
using Business.BusinessAcpects;
using Business.Constans;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AccountReconciliationDetailManager : IAccountReconciliationDetailService
    {
       private readonly IAccountReconciliationDetailDal _accountReconciliationDetailDal;

        public AccountReconciliationDetailManager(IAccountReconciliationDetailDal accountReconciliationDetailDal)
        {
            _accountReconciliationDetailDal = accountReconciliationDetailDal;
        }

        [PerformanceAspect(3)]
        [SecuredOperation("AccountReconciliationDetail.Add,Admin")]
        [CacheRemoveAspect("AccountReconciliationDetail.Get")]
        public IResult Add(AccountReconciliationDetail accountReconciliationDetail)
        {
            _accountReconciliationDetailDal.Add(accountReconciliationDetail);
            return new SuccessResult(Messages.AddedAccountReconciliationDetail);
        }

        [PerformanceAspect(3)]
        [SecuredOperation("AccountReconciliationDetail.Add,Admin")]
        [CacheRemoveAspect("AccountReconciliationDetail.Get")]
        [TransactionScopeAspect]
        public IResult AddToExcel(string filePath, int accountReconciliationId)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        string description = reader.GetString(1);

                        if (description != "Açıklama" && description != null)
                        {
                            DateTime date = reader.GetDateTime(0);                           
                            double currencyId = reader.GetDouble(2);
                            double debit = reader.GetDouble(3);
                            double credit = reader.GetDouble(4);

                            AccountReconciliationDetail accountReconciliationDetail = new AccountReconciliationDetail()
                            {
                                AccountReconciliationId = accountReconciliationId,
                                Description = description,
                                Date = date,
                                CurrencyCredit = Convert.ToDecimal(credit),
                                CurrencyDebit = Convert.ToDecimal(debit),
                                CurrencyId = Convert.ToInt16(currencyId)
                            };

                            _accountReconciliationDetailDal.Add(accountReconciliationDetail);
                        }
                    }
                }
            }

            File.Delete(filePath);

            return new SuccessResult(Messages.AddedAccountReconciliation);
        }

        [PerformanceAspect(3)]
        [SecuredOperation("AccountReconciliationDetail.Delete,Admin")]
        [CacheRemoveAspect("AccountReconciliationDetail.Get")]
        public IResult Delete(AccountReconciliationDetail accountReconciliationDetail)
        {
            _accountReconciliationDetailDal.Delete(accountReconciliationDetail);
            return new SuccessResult(Messages.DeletedAccountReconciliationDetail);
        }

        [PerformanceAspect(3)]
        [SecuredOperation("AccountReconciliationDetail.Get,Admin")]
        [CacheAspect(60)]
        public IDataResult<AccountReconciliationDetail> GetById(int id)
        {
            return new SuccesDataResult<AccountReconciliationDetail>(_accountReconciliationDetailDal.Get(p => p.Id == id));
        }

        [PerformanceAspect(3)]
        [SecuredOperation("AccountReconciliationDetail.GetList,Admin")]
        [CacheAspect(60)]
        public IDataResult<List<AccountReconciliationDetail>> GetList(int accountReconciliationId)
        {
            return new SuccesDataResult<List<AccountReconciliationDetail>>(_accountReconciliationDetailDal.GetList(p => p.AccountReconciliationId == accountReconciliationId));
        }

        [PerformanceAspect(3)]
        [SecuredOperation("AccountReconciliationDetail.Update,Admin")]
        [CacheRemoveAspect("AccountReconciliationDetail.Get")]
        public IResult Update(AccountReconciliationDetail accountReconciliationDetail)
        {
            _accountReconciliationDetailDal.Update(accountReconciliationDetail);
            return new SuccessResult(Messages.UpdatedAccountReconciliationDetail);
        }
    }
}
