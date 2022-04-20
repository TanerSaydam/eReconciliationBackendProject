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
    public class AccountReconciliationManager : IAccountReconciliationService
    {
        private readonly IAccountReconciliationDal _accountReconciliationDal;
        private readonly IAccountReconciliationDetailService _accountReconciliationDetailService;
        private readonly ICurrencyAccountService _currencyAccountService;
        private readonly IMailService _mailService;
        private readonly IMailTemplateService _mailTemplateService;
        private readonly IMailParameterService _mailParameterService;

        public AccountReconciliationManager(IAccountReconciliationDal accountReconciliationDal, ICurrencyAccountService currencyAccountService, IMailService mailService, IMailTemplateService mailTemplateService, IMailParameterService mailParameterService, IAccountReconciliationDetailService accountReconciliationDetailService)
        {
            _accountReconciliationDal = accountReconciliationDal;
            _currencyAccountService = currencyAccountService;
            _mailService = mailService;
            _mailTemplateService = mailTemplateService;
            _mailParameterService = mailParameterService;
            _accountReconciliationDetailService = accountReconciliationDetailService;
        }

        [PerformanceAspect(3)]        
        [SecuredOperation("AccountReconciliation.Add,Admin")]
        [CacheRemoveAspect("IAccountReconciliationService.Get")]
        public IResult Add(AccountReconciliation accountReconciliation)
        {
            string guid = Guid.NewGuid().ToString();
            accountReconciliation.Guid = guid;
            _accountReconciliationDal.Add(accountReconciliation);
            return new SuccessResult(Messages.AddedAccountReconciliation);
        }

        [PerformanceAspect(3)]
        [SecuredOperation("AccountReconciliation.Add,Admin")]
        [CacheRemoveAspect("IAccountReconciliationService.Get")]
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
                            DateTime startingDate = reader.GetDateTime(1);
                            DateTime endingDate = reader.GetDateTime(2);
                            double currencyId = reader.GetDouble(3);
                            double debit = reader.GetDouble(4);
                            double credit = reader.GetDouble(5);

                            int currencyAccountId = _currencyAccountService.GetByCode(code, companyId).Data.Id;
                            string guid = Guid.NewGuid().ToString();
                            
                            AccountReconciliation accountReconciliation = new AccountReconciliation()
                            {
                                CompanyId = companyId,
                                CurrencyAccountId = currencyAccountId,
                                CurrencyCredit = Convert.ToDecimal(credit),
                                CurrencyDebit = Convert.ToDecimal(debit),
                                CurrencyId =Convert.ToInt16(currencyId),
                                StartingDate = startingDate,
                                EndingDate =endingDate,
                                Guid = guid
                            };

                            _accountReconciliationDal.Add(accountReconciliation);
                        }
                    }                   
                }
            }

            File.Delete(filePath);

            return new SuccessResult(Messages.AddedAccountReconciliation);
        }

        [PerformanceAspect(3)]
        [SecuredOperation("AccountReconciliation.Delete,Admin")]
        [CacheRemoveAspect("IAccountReconciliationService.Get")]
        [TransactionScopeAspect]
        public IResult Delete(int id)
        {
            var result = _accountReconciliationDal.Get(p => p.Id == id);
            var resultDetails = _accountReconciliationDetailService.GetList(id).Data;
            foreach (var detail in resultDetails)
            {
                _accountReconciliationDetailService.Delete(detail);
            }

            _accountReconciliationDal.Delete(result);
            return new SuccessResult(Messages.DeletedAccountReconciliation);
        }

        [PerformanceAspect(3)]        
        public IDataResult<AccountReconciliation> GetByCode(string code)
        {
            return new SuccesDataResult<AccountReconciliation>(_accountReconciliationDal.Get(p => p.Guid == code));
        }

        [PerformanceAspect(3)]
        public IDataResult<AccountReconciliationDto> GetByCodeDto(string code)
        {
            return new SuccesDataResult<AccountReconciliationDto>(_accountReconciliationDal.GetByCodeDto(code));
        }

        [PerformanceAspect(3)]
        [SecuredOperation("AccountReconciliation.Get,Admin")]
        [CacheAspect(60)]
        public IDataResult<AccountReconciliation> GetById(int id)
        {
            return new SuccesDataResult<AccountReconciliation>(_accountReconciliationDal.Get(p => p.Id == id));
        }

        [PerformanceAspect(3)]
        [SecuredOperation("AccountReconciliation.GetList,Admin")]
        [CacheAspect(60)]
        public IDataResult<AccountReconciliationsCountDto> GetCountDto(int companyId)
        {
            return new SuccesDataResult<AccountReconciliationsCountDto>(_accountReconciliationDal.GetCountDto(companyId));
        }

        [PerformanceAspect(3)]
        [SecuredOperation("AccountReconciliation.GetList,Admin")]
        [CacheAspect(60)]
        public IDataResult<List<AccountReconciliation>> GetList(int companyId)
        {
            return new SuccesDataResult<List<AccountReconciliation>>(_accountReconciliationDal.GetList(p => p.CompanyId == companyId));
        }

        [PerformanceAspect(3)]
        [SecuredOperation("AccountReconciliation.GetList,Admin")]
        [CacheAspect(60)]
        public IDataResult<List<AccountReconciliation>> GetListByCurrencyAccountId(int currencyAccountId)
        {
            return new SuccesDataResult<List<AccountReconciliation>>(_accountReconciliationDal.GetList(p => p.CurrencyAccountId == currencyAccountId));
        }

        [PerformanceAspect(3)]
        [SecuredOperation("AccountReconciliation.GetList,Admin")]
        [CacheAspect(60)]
        public IDataResult<List<Entities.Dtos.AccountReconciliationDto>> GetListDto(int companyId)
        {
            return new SuccesDataResult<List<AccountReconciliationDto>>(_accountReconciliationDal.GetAllDto(companyId));
        }

        [CacheRemoveAspect("IAccountReconciliationService.Get")]
        public IResult Result(ReconciliationResultDto reconciliationResultDto)
        {
            var result = _accountReconciliationDal.Get(p => p.Id == reconciliationResultDto.Id);
            result.IsResultSucceed = reconciliationResultDto.Result;
            result.ResultDate = DateTime.Now;
            result.ResultNote = "Cevaplayan: " + reconciliationResultDto.Name + " Not: " + reconciliationResultDto.Note;
            _accountReconciliationDal.Update(result);
            return new SuccessResult(Messages.ReconciliationResultSucceed);
        }

        [PerformanceAspect(3)]
        [SecuredOperation("AccountReconciliation.SendMail,Admin")]
        public IResult SendReconciliationMail(int id)
        {
            var accountReconciliationDto = _accountReconciliationDal.GetByIdDto(id);

            if (accountReconciliationDto.IsResultSucceed == true)
            {
                return new ErrorResult(Messages.IsReconciliationAlreadySucceed);
            }
            string subject = "Mutabakat Maili";
            string body = $"Şirket Adımız: {accountReconciliationDto.CompanyName} <br /> " +
                $"Şirket Vergi Dairesi: {accountReconciliationDto.CompanyTaxDepartment} <br />" +
                $"Şirket Vergi Numarası: {accountReconciliationDto.CompanyTaxIdNumber} - {accountReconciliationDto.CompanyIdentityNumber} <br /><hr>" +
                $"Sizin Şirket: {accountReconciliationDto.AccountName} <br />" +
                $"Sizin Şirket Vergi Dairesi: {accountReconciliationDto.AccountTaxDepartment} <br />" +
                $"Sizin Şirket Vergi Numarası: {accountReconciliationDto.AccountTaxIdNumber} - {accountReconciliationDto.AccountIdentityNumber} <br /><hr>" +
                $"Borç: {accountReconciliationDto.CurrencyDebit} {accountReconciliationDto.CurrencyCode} <br />" +
                $"Alacak: {accountReconciliationDto.CurrencyCredit} {accountReconciliationDto.CurrencyCode} <br />";
            string link = "http://localhost:4200/account-reconciliation-result/" + accountReconciliationDto.Guid;
            string linkDescription = "Mutabakatı Cevaplamak için Tıklayın";

            var mailTemplate = _mailTemplateService.GetByTemplateName("Mutabakat", accountReconciliationDto.CompanyId);
            string templateBody = mailTemplate.Data.Value;
            templateBody = templateBody.Replace("{{title}}", subject);
            templateBody = templateBody.Replace("{{message}}", body);
            templateBody = templateBody.Replace("{{link}}", link);
            templateBody = templateBody.Replace("{{linkDescription}}", linkDescription);


            var mailParameter = _mailParameterService.Get(4);
            Entities.Dtos.SendMailDto sendMailDto = new Entities.Dtos.SendMailDto()
            {
                mailParameter = mailParameter.Data,
                email = accountReconciliationDto.AccountEmail,
                subject = subject,
                body = templateBody
            };

            _mailService.SendMail(sendMailDto);

            return new SuccessResult(Messages.MailSendSucessful);
        }

        [PerformanceAspect(3)]
        [SecuredOperation("AccountReconciliation.Update,Admin")]
        [CacheRemoveAspect("IAccountReconciliationService.Get")]
        public IResult Update(AccountReconciliation accountReconciliation)
        {
            _accountReconciliationDal.Update(accountReconciliation);
            return new SuccessResult(Messages.UpdatedAccountReconciliation);
        }

        [PerformanceAspect(3)]
        [SecuredOperation("AccountReconciliation.Update,Admin")]
        [CacheRemoveAspect("IAccountReconciliationService.Get")]
        public IResult UpdateResult(AccountReconciliation accountReconciliation)
        {
            _accountReconciliationDal.Update(accountReconciliation);
            return new SuccessResult(Messages.UpdatedAccountReconciliationResult);
        }
    }
}
