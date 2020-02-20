using System;
using System.IO;
using System.Threading.Tasks;
using Event.Classes;
using Event.Extension;
using MobileLibrary.Common;
using MobileLibrary.Extension;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using TomskNipi.Event.MobileClasses;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Event
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageAccount : INotifyReceiver
    {
        public PageAccount()
        {
            InitializeComponent();
            UpdateMenu();
        }

        #region Поля

        private bool _isInitialized;

        #endregion

        #region Свойства

        public MobUser MobUser => Config.User;

        public bool HasUser => MobUser != null;

        public ImageSource UserPhoto => Service.GetUserPhoto(HasUser ? MobUser.Id : Guid.Empty);

        public string ViewMobOrganization => HasUser ? MobUser.ViewMobOrganization : string.Empty;

        public bool HasViewMobOrganization => !string.IsNullOrEmpty(ViewMobOrganization);

        public string ViewMobPost => HasUser ? MobUser.ViewMobPost : string.Empty;

        public bool HasViewMobPost => !string.IsNullOrEmpty(ViewMobPost);

        #endregion

        #region Методы

        private void Init()
        {
            if (_isInitialized)
                return;

            EntryLogin.Text = Save.ConnectionLogin;
            EntryPassword.Text = Save.ConnectionPassword;

            _isInitialized = true;
            UpdateMenu();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            IsBusy = true;
            try
            {
                await Task.Delay(100);
                await Config.ConnectAndLoginAsync();
                Init();
            }
            finally
            {
                UpdateMenu();
                IsBusy = false;
            }
        }

        private async void SavePhoto(byte[] buffer)
        {
            IsBusy = true;
            try
            {
                await Task.Delay(100);
                var result = await Service.PostUserPhoto(Config.User.Id, buffer);
                if (result.Validate(this))
                {
                    var source = ImagePhoto.Source;
                    ImagePhoto.Source = null;
                    ImagePhoto.Source = source;
                    NotifySender.Send(Notify.UserPhotoChanged, null);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void UpdateMenu()
        {
            ButtonConnect.IsEnabled = !EntryLogin.Text.IsNullOrEmpty() && !EntryPassword.Text.IsNullOrEmpty();
            ButtonPasswordChange.IsEnabled = Config.User != null;
        }

        #endregion

        #region События

        private async void ButtonConnect_OnClicked(object sender, EventArgs e)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                await Task.Delay(100);
                var result = await Service.GetAuthentication(EntryLogin.Text, EntryPassword.Text);
                if (result.Validate(this))
                {
                    Save.ConnectionLogin = EntryLogin.Text;
                    Save.ConnectionPassword = EntryPassword.Text;
                    Config.User = result.Data;
                    await DisplayAlert(Title, "Подключение к сервису выполнено успешно.", "OK");

                    var result2 = await Service.GetEvent(Config.User.Id, Save.EventId, true);
                    ((MainPage) Application.Current.MainPage).Detail = result2.Validate(null)
                        ? new NavigationPage(new PageEvent(result2.Data))
                        : new NavigationPage(new PageEvent(null));
                }
            }
            finally
            {
                UpdateMenu();
                IsBusy = false;
            }
        }

        private async void ButtonPasswordChange_OnClicked(object sender, EventArgs e)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                await Task.Delay(100);
                if (Config.User == null)
                    return;
                var result = await Navigation.GetPushModalAsync(new DialogPasswordChange());
                if (result)
                    EntryPassword.Text = Save.ConnectionPassword;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void EntryLogin_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateMenu();
        }

        private void EntryPassword_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateMenu();
        }

        private async void UserPhoto_OnTapped(object sender, EventArgs e)
        {
            if (Config.User == null)
                return;
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                await Task.Delay(100);
                await CrossMedia.Current.Initialize();
                var image = await ImagePhoto.GetImageAsJpgAsync();
                var canCamera = CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported;
                var canGallery = CrossMedia.Current.IsPickPhotoSupported;
                var canView = image != null && image.Length > 0;
                var canDelete = image != null && image.Length > 0;
                if (!canCamera && !canGallery && !canView && !canDelete)
                    return;
                PhotoAction result;
                if (canCamera && !canGallery && !canView && !canDelete)
                    result = PhotoAction.Camera;
                else if (canGallery && !canCamera && !canView && !canDelete)
                    result = PhotoAction.Gallery;
                else
                {
                    var dialogResult = await Navigation.GetPushModalAsync(new DialogPhotoAction(canCamera, canGallery, canView, canDelete));
                    if (dialogResult == null)
                        return;
                    result = dialogResult.Action;
                }

                switch (result)
                {
                    case PhotoAction.Camera:
                    {
                        var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
                        if (storageStatus != PermissionStatus.Granted)
                            storageStatus = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                        if (storageStatus != PermissionStatus.Granted)
                            return;

                        var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>();
                        if (cameraStatus != PermissionStatus.Granted)
                            cameraStatus = await CrossPermissions.Current.RequestPermissionAsync<CameraPermission>();
                        if (cameraStatus != PermissionStatus.Granted)
                            return;

                        using var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                        {
                            Directory = "Мероприятия",
                            SaveToAlbum = true,
                            CompressionQuality = 97,
                            PhotoSize = PhotoSize.MaxWidthHeight,
                            MaxWidthHeight = 255,
                            DefaultCamera = CameraDevice.Front,
                            SaveMetaData = false
                        });
                        if (file == null)
                            return;
                        await using var stream = file.GetStream();
                        var buffer = new byte[stream.Length];
                        var bufferRead = stream.Read(buffer, 0, buffer.Length);
                        if (bufferRead != buffer.Length)
                            return;
                        SavePhoto(buffer);
                        break;
                    }

                    case PhotoAction.Gallery:
                    {
                        var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
                        if (storageStatus != PermissionStatus.Granted)
                            storageStatus = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                        if (storageStatus != PermissionStatus.Granted)
                            return;

                        using var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                        {
                            CompressionQuality = 97,
                            PhotoSize = PhotoSize.MaxWidthHeight,
                            MaxWidthHeight = 256,
                            SaveMetaData = false
                        });
                        if (file == null)
                            return;
                        await using var stream = file.GetStream();
                        var buffer = new byte[stream.Length];
                        var bufferRead = stream.Read(buffer, 0, buffer.Length);
                        if (bufferRead != buffer.Length)
                            return;
                        SavePhoto(buffer);
                        break;
                    }

                    case PhotoAction.View:
                    {
                        if (image != null && image.Length > 0)
                            await Navigation.GetPushModalAsync(new DialogPhotoView(ImageSource.FromStream(() => new MemoryStream(image))));
                        break;
                    }

                    case PhotoAction.Delete:
                    {
                        SavePhoto(null);
                        break;
                    }
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion

        #region INotifyReceiver Реализаторы интерфейса

        public void NotifyReceive(string message, object data)
        {
            if (message == Notify.UserChanged || message == Notify.UserPhotoChanged)
            {
                OnPropertyChanged(nameof(MobUser));
                OnPropertyChanged(nameof(HasUser));
                OnPropertyChanged(nameof(UserPhoto));
                OnPropertyChanged(nameof(ViewMobOrganization));
                OnPropertyChanged(nameof(HasViewMobOrganization));
                OnPropertyChanged(nameof(ViewMobPost));
                OnPropertyChanged(nameof(HasViewMobPost));
                UpdateMenu();
            }
        }

        #endregion
    }
}