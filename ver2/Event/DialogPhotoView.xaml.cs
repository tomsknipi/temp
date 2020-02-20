using System;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Event
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DialogPhotoView
    {
        public DialogPhotoView(ImageSource photo)
        {
            Photo = photo;
            InitializeComponent();
        }

        #region Свойства

        public ImageSource Photo { get; }

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

        #endregion
    }
}