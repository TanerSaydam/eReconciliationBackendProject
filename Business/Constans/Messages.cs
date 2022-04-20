using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constans
{
    public class Messages
    {
        public static string AddedCompany = "Şirket kaydı başarıyla tamamlandı";
        public static string UpdatedCompany = "Şirket kaydı başarıyla güncellendi";
        public static string CompanyAlreadyExists = "Bu şirket daha önce kaydedilmiş";        

        public static string UserNotFound = "Kullanıcı bulunamadı";
        public static string PasswordError = "Şifre yanlış";
        public static string SuccessfulLogin = "Giriş başarılı";
        public static string UserRegistered = "Kullanıcı kaydı başarılı";
        public static string UserAlreadyExists = "Bu kullanıcı mevcut";
        public static string UserMailConfirmSuccessful = "Mailiniz başarıyla onaylandı";
        public static string MailAlreadyConfirm = "Mailiniz zaten onaylı. Tekrar gönderim yapılmadı";
        public static string MailConfirmTimeHasNotExpired = "Mail onayını 5 dakikada bir gönderebilirsiniz";
        public static string UpdatedUser = "Kullanıcı başarıyla güncellendi";
        public static string DeletedUser = "Kullanıcı başarıyla silindi";
        public static string ChangedPassword = "Şifre başarıyla değiştirildi";
        public static string DeletedUserCompanyReletionShip = "Kullanıcının şirket ile bağlantısı kesilmiştir";
        public static string AddedUserCompanyReletionShip = "Kullanıcı ile şirket arasında bağlantı kuruldu";
        public static string UpdatedUserTheme = "Tema başarıyla güncellendi";

        public static string MailParameterUpdated = "Mail parameterleri başarıyla güncellendi";
        public static string MailSendSucessful = "Mail başarıyla gönderildi";
        public static string MailConfirmSendSuccessful = "Onay maili tekrar gönderildi";

        public static string MailTemplateAdded = "Mail şablonu başarıyla kaydedildi";
        public static string MailTemplateUpdated = "Mail şablonu başarıyla güncellendi";
        public static string MailTemplateDeleted = "Mail şablonu başarıyla silindi";

        public static string AddedCurrencyAccount = "Cari kaydı başarıyla eklendi";
        public static string UpdatedCurrencyAccount = "Cari kaydı başarıyla güncellendi";
        public static string DeletedCurrencyAccount = "Cari kaydı başarıyla silindi";
        public static string AccountHaveRecontiliations = "Mutabakat işlemi olan cari kaydı silemezsiniz. İsterseniz cariyi pasife çekebilirsiniz.";

        public static string AddedAccountReconciliation = "Cari mutabakat kaydı başarıyla eklendi";
        public static string UpdatedAccountReconciliation = "Cari mutabakat kaydı başarıyla güncellendi";
        public static string DeletedAccountReconciliation = "Cari mutabakat başarıyla silindi";

        public static string AddedAccountReconciliationDetail = "Cari mutabakat detay kaydı başarıyla eklendi";
        public static string UpdatedAccountReconciliationDetail = "Cari mutabakat detay kaydı başarıyla güncellendi";
        public static string UpdatedAccountReconciliationResult = "Cari mutabakat için sozlü mutabakat başarıyla işlendi";
        public static string IsReconciliationAlreadySucceed = "Onaylı mutabakat için tekrar mail gonderemezsiniz";
        public static string DeletedAccountReconciliationDetail = "Cari mutabakat detay başarıyla silindi";

        public static string ReconciliationResultSucceed = "Mutabakat cevabı başarıyla kaydedildi";

        public static string AddedBaBsReconciliation = "BaBs kaydı başarıyla eklendi";
        public static string UpdatedBaBsReconciliation = "BaBs kaydı başarıyla güncellendi";
        public static string DeletedBaBsReconciliation = "BaBs başarıyla silindi";

        public static string AddedBaBsReconciliationDetail = "BaBs detay kaydı başarıyla eklendi";
        public static string UpdatedBaBsReconciliationDetail = "BaBs detay kaydı başarıyla güncellendi";
        public static string DeletedBaBsReconciliationDetail = "BaBs detay başarıyla silindi";

        public static string AddedOperationClaim = "Yetki başarıyla eklendi";
        public static string UpdatedOperationClaim = "Yetki başarıyla güncellendi";
        public static string DeletedOperationClaim = "Yetki başarıyla silindi";

        public static string AddedUserOperationClaim = "Kullanıcıya yetki başarıyla eklendi";
        public static string UpdatedUserOperationClaim = "Kullanıcı yetkisi başarıyla güncellendi";
        public static string DeletedUserOperationClaim = "Kullanıcı yetkisi başarıyla silindi";

        public static string UpdateTermsAndConditions = "Sözleşme başarıyla güncellendi";
    }
}
