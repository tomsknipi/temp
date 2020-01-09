using System;
using System.IO;
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

        public ImageSource UserPhoto => HasUserPhoto ? ImageSource.FromStream(() => new MemoryStream(MobUser.Photo)) : ImageSource.FromFile("user_profile_large.png");

        public bool HasUserPhoto => HasUser && MobUser.Photo != null && MobUser.Photo.Length > 0;

        public Color UserPhotoTintColor => HasUserPhoto ? Color.Transparent : App.ColorSelected;

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
                var result = await Service.PostUserPhoto(Config.User.Id, buffer);
                if (result.Validate(this))
                {
                    Config.User.Photo = buffer ?? new byte[0];
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
            IsBusy = true;
            try
            {
                var result = await Service.GetAuthentication(EntryLogin.Text, EntryPassword.Text);
                if (result.Validate(this))
                {
                    Save.ConnectionLogin = EntryLogin.Text;
                    Save.ConnectionPassword = EntryPassword.Text;
                    Config.User = result.Data;
                    await DisplayAlert(Title, "Подключение к сервису выполнено успешно.", "OK");

                    var result2 = await Service.GetEvent(Config.User.Id, Save.EventId);
                    if (result2.Validate(null))
                        ((MainPage) Application.Current.MainPage).Detail = new NavigationPage(new PageEvent(result2.Data));
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
            if (Config.User == null)
                return;
            var result = await Navigation.GetPushModalAsync(new DialogPasswordChange());
            if (result)
                EntryPassword.Text = Save.ConnectionPassword;
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
            await CrossMedia.Current.Initialize();
            var canCamera = CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported;
            var canGallery = CrossMedia.Current.IsPickPhotoSupported;
            var canView = HasUserPhoto;
            var canDelete = HasUserPhoto;
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
                    var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>();
                    if (cameraStatus != PermissionStatus.Granted)
                        cameraStatus = await CrossPermissions.Current.RequestPermissionAsync<CameraPermission>();
                    if (cameraStatus != PermissionStatus.Granted)
                        return;

                    var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
                    if (storageStatus != PermissionStatus.Granted)
                        storageStatus = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                    if (storageStatus != PermissionStatus.Granted)
                        return;

                    Device.BeginInvokeOnMainThread(async () =>
                    {
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
                    });

                    break;
                }

                case PhotoAction.Gallery:
                {
                    var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
                    if (storageStatus != PermissionStatus.Granted)
                        storageStatus = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                    if (storageStatus != PermissionStatus.Granted)
                        return;

                    Device.BeginInvokeOnMainThread(async () =>
                    {
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
                    });

                    break;
                }

                case PhotoAction.View:
                {
                    await Navigation.GetPushModalAsync(new DialogPhotoView(UserPhoto));
                    break;
                }

                case PhotoAction.Delete:
                {
                    SavePhoto(null);
                    break;
                }
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
                OnPropertyChanged(nameof(HasUserPhoto));
                OnPropertyChanged(nameof(UserPhotoTintColor));
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