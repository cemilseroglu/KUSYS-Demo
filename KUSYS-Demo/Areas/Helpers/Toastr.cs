using NToastNotify;

namespace KUSYS_Demo.Areas.Helper
{
    public class Toastr
    {
        private readonly IToastNotification _toastNotification;
        public Toastr(IToastNotification toastNotification)
        {
            _toastNotification = toastNotification;
        }
        //successes
        public void Success()
        {
            _toastNotification.AddSuccessToastMessage("İşleminiz başarıyla tamamlandı", new ToastrOptions()
            {
                Title = "Başarılı"
            });
        }
        //success override
        public void Success(string message)
        {
            _toastNotification.AddSuccessToastMessage(message, new ToastrOptions()
            {
                Title = "Başarılı"
            });
        }

        //Errors
        public void Error()
        {
            _toastNotification.AddErrorToastMessage("İşleminiz başarısız oldu, tekrar deneyin", new ToastrOptions()
            {
                Title = "Hata !"
            });
        }

        public void Error(string message)
        {
            _toastNotification.AddErrorToastMessage($"{message}", new ToastrOptions()
            {
                Title = "Hata !"
            });
        }

        public void ExistingError()
        {
            _toastNotification.AddErrorToastMessage("Aynı isimde bir kayıt mevcuttur", new ToastrOptions()
            {
                Title = "Hata !"
            });
        }
        public void NullError()
        {
            _toastNotification.AddErrorToastMessage("Bazı alanlarda eksik veriler var. Yönetici ile iletişime geçin.", new ToastrOptions()
            {
                Title = "KRİTİK HATA !"
            });
        }


        public void AccessDenied()
        {
            _toastNotification.AddErrorToastMessage("Lütfen giriş yaparak tekrar deneyiniz.", new ToastrOptions()
            {
                Title = "Hata !"
            });
        }

    }
}
