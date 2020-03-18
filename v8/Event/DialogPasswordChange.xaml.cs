using System;
using System.Threading.Tasks;
using Event.Classes;
using Event.Extension;
using MobileLibrary.Extension;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms.Xaml;

namespace Event
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DialogPasswordChange
    {
        public DialogPasswordChange()
        {
            InitializeComponent();
        }

        #region Методы

        public bool Validate()
        {
            if (EntryPasswordNew.Text.IsNullOrEmpty())
            {
                DisplayAlert(Title, "Новый пароль не может быть пустым!", "OK");

                return false;
            }

            if (EntryPasswordNew.Text.Contains(' '))
            {
                DisplayAlert(Title, "Пароль не может содержать пробелы!", "OK");

                return false;
            }

            if (EntryPasswordNew.Text.Length < 3 || EntryPasswordNew.Text.Length > 104)
            {
                DisplayAlert(Title, "Длина пароля должна быть не менее 3 и не более 104 символов!", "OK");

                return false;
            }

            if (EntryPasswordNew.Text != EntryPasswordConfirm.Text)
            {
                DisplayAlert(Title, "Новый пароль и подтверждение не совпадают!", "OK");

                return false;
            }

            return true;
        }

        #endregion

        #region События

        private async void ButtonCancel_OnClicked(object sender, EventArgs e)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                await Task.Delay(100);
                PageResult = false;
                await Navigation.PopPopupAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void ButtonOk_OnClicked(object sender, EventArgs e)
        {
            if (IsBusy)
                return;

            if (!Validate())
                return;
            IsBusy = true;
            try
            {
                await Task.Delay(100);
                var result = await Service.PostUserPassword(Config.User.Id, Save.ConnectionPassword, EntryPasswordNew.Text);
                if (result.Validate(this))
                {
                    Save.ConnectionPassword = EntryPasswordNew.Text;
                    PageResult = true;
                    await Navigation.PopPopupAsync();
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion
    }
}