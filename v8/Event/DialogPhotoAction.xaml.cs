using System;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Event
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DialogPhotoAction
    {
        public DialogPhotoAction(bool canCamera, bool canGallery, bool canView, bool canDelete)
        {
            ItemCamera = new DialogPhotoActionResult("camera.png", "Камера", PhotoAction.Camera, App.ColorAccent, false, canCamera);
            ItemGallery = new DialogPhotoActionResult("image.png", "Галерея", PhotoAction.Gallery, App.ColorAccent, false, canGallery);
            ItemView = new DialogPhotoActionResult("visibility.png", "Просмотреть", PhotoAction.View, App.ColorAccent, canCamera || canGallery, canView);
            ItemDelete = new DialogPhotoActionResult("delete.png", "Удалить", PhotoAction.Delete, Color.Brown, (canCamera || canGallery) && !canView, canDelete);
            InitializeComponent();
        }

        #region Свойства

        public DialogPhotoActionResult ItemCamera { get; set; }

        public DialogPhotoActionResult ItemGallery { get; set; }

        public DialogPhotoActionResult ItemView { get; set; }

        public DialogPhotoActionResult ItemDelete { get; set; }

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
                PageResult = null;
                await Navigation.PopPopupAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void ImageButtonCamera_OnClicked(object sender, EventArgs e)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                await Task.Delay(100);
                PageResult = ItemCamera;
                await Navigation.PopPopupAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void ImageButtonDelete_OnClicked(object sender, EventArgs e)
        {
            if (IsBusy)
                return;

            var ansver = await DisplayAlert(null, "Вы действительно собираетесь удалить изображение?", "ДА", "НЕТ");
            if (ansver)
            {
                IsBusy = true;
                try
                {
                    await Task.Delay(100);
                    PageResult = ItemDelete;
                    await Navigation.PopPopupAsync();
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        private async void ImageButtonGallery_OnClicked(object sender, EventArgs e)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                await Task.Delay(100);
                PageResult = ItemGallery;
                await Navigation.PopPopupAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void ImageButtonView_OnClicked(object sender, EventArgs e)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                await Task.Delay(100);
                PageResult = ItemView;
                await Navigation.PopPopupAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion
    }

    public enum PhotoAction
    {
        Camera,
        Gallery,
        View,
        Delete,
        Cancel
    }

    public class DialogPhotoActionResult
    {
        public DialogPhotoActionResult(string imageSource, string text, PhotoAction action, Color tintColor, bool isSplitter, bool isVisible)
        {
            ImageSource = imageSource;
            Text = text;
            Action = action;
            TintColor = tintColor;
            IsSplitter = isSplitter;
            IsVisible = isVisible;
        }

        #region Свойства

        public string ImageSource { get; set; }

        public string Text { get; set; }

        public PhotoAction Action { get; set; }

        public Color TintColor { get; set; }

        public bool IsSplitter { get; set; }

        public bool IsVisible { get; set; }

        #endregion
    }
}