using Business.Abstract;
using Business.Constans;
using Core.Aspects.Autofac.Transaction;
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
    public class BaBsRecoinciliationManager : IBaBsReconciliationService
    {
        private readonly IBaBsRecoinciliationDal _baBsRecoinciliationDal;
        private readonly ICurrencyAccountService _currencyAccountService;

        public BaBsRecoinciliationManager(IBaBsRecoinciliationDal baBsRecoinciliationDal, ICurrencyAccountService currencyAccountService)
        {
            _baBsRecoinciliationDal = baBsRecoinciliationDal;
            _currencyAccountService = currencyAccountService;
        }       

        public IResult Add(BaBsReconciliation babsReconciliation)
        {
            _baBsRecoinciliationDal.Add(babsReconciliation);
            return new SuccessResult(Messages.AddedBaBsReconciliation);
        }

        [TransactionScopeAspect]
        public IResult AddToExcel(string filePath, int companyId)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        string code = reader.GetString(0);

                        if (code != "Cari Kodu" && code != null)
                        {
                            string type = reader.GetString(1);
                            double mounth = reader.GetDouble(2);
                            double year = reader.GetDouble(3);
                            double quantit = reader.GetDouble(4);
                            double total = reader.GetDouble(5);

                            int currencyAccountId = _currencyAccountService.GetByCode(code, companyId).Data.Id;

                            BaBsReconciliation baBsReconciliation = new BaBsReconciliation()
                            {
                                CompanyId = companyId,
                                CurrencyAccountId = currencyAccountId,
                                Type = type,
                                Mounth = Convert.ToInt16(mounth),
                                Year = Convert.ToInt16(year),
                                Quantity = Convert.ToInt16(quantit),
                                Total = Convert.ToDecimal(total)
                            };

                            _baBsRecoinciliationDal.Add(baBsReconciliation);
                        }
                    }
                }
            }

            return new SuccessResult(Messages.AddedBaBsReconciliation);
        }

        public IResult Delete(BaBsReconciliation babsReconciliation)
        {
            _baBsRecoinciliationDal.Delete(babsReconciliation);
            return new SuccessResult(Messages.DeletedBaBsReconciliation);
        }

        public IDataResult<BaBsReconciliation> GetById(int id)
        {
            return new SuccesDataResult<BaBsReconciliation>(_baBsRecoinciliationDal.Get(p => p.Id == id));
        }

        public IDataResult<List<BaBsReconciliation>> GetList(int companyId)
        {
            return new SuccesDataResult<List<BaBsReconciliation>>(_baBsRecoinciliationDal.GetList(p => p.CompanyId == companyId));
        }

        public IResult Update(BaBsReconciliation babsReconciliation)
        {
            _baBsRecoinciliationDal.Update(babsReconciliation);
            return new SuccessResult(Messages.UpdatedBaBsReconciliation);
        }
    }
}
