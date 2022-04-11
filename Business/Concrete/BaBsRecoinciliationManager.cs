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
using Entities.Dtos;
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
        private readonly IMailService _mailService;
        private readonly IMailTemplateService _mailTemplateService;
        private readonly IMailParameterService _mailParameterService;

        public BaBsRecoinciliationManager(IBaBsRecoinciliationDal baBsRecoinciliationDal, ICurrencyAccountService currencyAccountService, IMailService mailService, IMailTemplateService mailTemplateService, IMailParameterService mailParameterService)
        {
            _baBsRecoinciliationDal = baBsRecoinciliationDal;
            _currencyAccountService = currencyAccountService;
            _mailService = mailService;
            _mailTemplateService = mailTemplateService;
            _mailParameterService = mailParameterService;
        }

        [PerformanceAspect(3)]
        //[SecuredOperation("BaBsReconciliation.Add,Admin")]
        [CacheRemoveAspect("IBaBsReconciliationService.Get")]
        public IResult Add(BaBsReconciliation babsReconciliation)
        {
            string guid = Guid.NewGuid().ToString();
            babsReconciliation.Guid = guid;
            _baBsRecoinciliationDal.Add(babsReconciliation);
            return new SuccessResult(Messages.AddedBaBsReconciliation);
        }

        [PerformanceAspect(3)]
        [SecuredOperation("BaBsReconciliation.Add,Admin")]
        [CacheRemoveAspect("IBaBsReconciliationService.Get")]
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
                            string guid = Guid.NewGuid().ToString();                            

                            int currencyAccountId = _currencyAccountService.GetByCode(code, companyId).Data.Id;

                            BaBsReconciliation baBsReconciliation = new BaBsReconciliation()
                            {
                                CompanyId = companyId,
                                CurrencyAccountId = currencyAccountId,
                                Type = type,
                                Mounth = Convert.ToInt16(mounth),
                                Year = Convert.ToInt16(year),
                                Quantity = Convert.ToInt16(quantit),
                                Total = Convert.ToDecimal(total),
                                Guid = guid
                            };

                            _baBsRecoinciliationDal.Add(baBsReconciliation);
                        }
                    }
                }
            }

            File.Delete(filePath);

            return new SuccessResult(Messages.AddedBaBsReconciliation);
        }

        [PerformanceAspect(3)]
        [SecuredOperation("BaBsReconciliation.Delete,Admin")]
        [CacheRemoveAspect("IBaBsReconciliationService.Get")]
        public IResult Delete(BaBsReconciliation babsReconciliation)
        {
            _baBsRecoinciliationDal.Delete(babsReconciliation);
            return new SuccessResult(Messages.DeletedBaBsReconciliation);
        }

        [PerformanceAspect(3)]
        //[SecuredOperation("BaBsReconciliation.Get,Admin")]
        [CacheAspect(60)]
        public IDataResult<BaBsReconciliation> GetByCode(string code)
        {
            return new SuccesDataResult<BaBsReconciliation>(_baBsRecoinciliationDal.Get(p => p.Guid == code));
        }

        [PerformanceAspect(3)]
        [SecuredOperation("BaBsReconciliation.Get,Admin")]
        [CacheAspect(60)]
        public IDataResult<BaBsReconciliation> GetById(int id)
        {
            return new SuccesDataResult<BaBsReconciliation>(_baBsRecoinciliationDal.Get(p => p.Id == id));
        }

        [PerformanceAspect(3)]
        [SecuredOperation("BaBsReconciliation.GetList,Admin")]
        [CacheAspect(60)]
        public IDataResult<List<BaBsReconciliation>> GetList(int companyId)
        {
            return new SuccesDataResult<List<BaBsReconciliation>>(_baBsRecoinciliationDal.GetList(p => p.CompanyId == companyId));
        }

        [PerformanceAspect(3)]
        [SecuredOperation("BaBsReconciliation.GetList,Admin")]
        [CacheAspect(60)]
        public IDataResult<List<BaBsReconciliation>> GetListByCurrencyAccountId(int currencyAccount)
        {            
            return new SuccesDataResult<List<BaBsReconciliation>>(_baBsRecoinciliationDal.GetList(p => p.CurrencyAccountId == currencyAccount));
        }

        [PerformanceAspect(3)]
        [SecuredOperation("BaBsReconciliation.GetList,Admin")]
        [CacheAspect(60)]
        public IDataResult<List<BaBsReconciliationDto>> GetListDto(int companyId)
        {
            return new SuccesDataResult<List<BaBsReconciliationDto>>(_baBsRecoinciliationDal.GetAllDto(companyId));
        }

        [PerformanceAspect(3)]
        [SecuredOperation("BaBsReconciliation.SendMail,Admin")]
        public IResult SendReconciliationMail(BaBsReconciliationDto babsReconciliationDto)
        {
            string subject = "Mutabakat Maili";
            string body = $"Şirket Adımız: {babsReconciliationDto.CompanyName} <br /> " +
                $"Şirket Vergi Dairesi: {babsReconciliationDto.CompanyTaxDepartment} <br />" +
                $"Şirket Vergi Numarası: {babsReconciliationDto.CompanyTaxIdNumber} - {babsReconciliationDto.CompanyIdentityNumber} <br /><hr>" +
                $"Sizin Şirket: {babsReconciliationDto.AccountName} <br />" +
                $"Sizin Şirket Vergi Dairesi: {babsReconciliationDto.AccountTaxDepartment} <br />" +
                $"Sizin Şirket Vergi Numarası: {babsReconciliationDto.AccountTaxIdNumber} - {babsReconciliationDto.AccountIdentityNumber} <br /><hr>" +
                $"Ay / Yıl: {babsReconciliationDto.Mounth} / {babsReconciliationDto.Year} <br />" +                
                $"Adet: {babsReconciliationDto.Quantity} <br />" +
                $"Tutar: {babsReconciliationDto.Total} {babsReconciliationDto.CurrencyCode} <br />";
            string link = "https://localhost:7220/api/BaBsReconciliations/GetByCode?code=" + babsReconciliationDto.Guid;
            string linkDescription = "Mutabakatı Cevaplamak için Tıklayın";

            var mailTemplate = _mailTemplateService.GetByTemplateName("Kayıt", 4);
            string templateBody = mailTemplate.Data.Value;
            templateBody = templateBody.Replace("{{title}}", subject);
            templateBody = templateBody.Replace("{{message}}", body);
            templateBody = templateBody.Replace("{{link}}", link);
            templateBody = templateBody.Replace("{{linkDescription}}", linkDescription);


            var mailParameter = _mailParameterService.Get(4);
            Entities.Dtos.SendMailDto sendMailDto = new Entities.Dtos.SendMailDto()
            {
                mailParameter = mailParameter.Data,
                email = babsReconciliationDto.AccountEmail,
                subject = subject,
                body = templateBody
            };

            _mailService.SendMail(sendMailDto);

            return new SuccessResult(Messages.MailSendSucessful);
        }

        [PerformanceAspect(3)]
        [SecuredOperation("BaBsReconciliation.Update,Admin")]
        [CacheRemoveAspect("IBaBsReconciliationService.Get")]
        public IResult Update(BaBsReconciliation babsReconciliation)
        {
            _baBsRecoinciliationDal.Update(babsReconciliation);
            return new SuccessResult(Messages.UpdatedBaBsReconciliation);
        }
    }
}
