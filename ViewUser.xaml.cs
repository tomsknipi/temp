using System;
using System.IO;
using TomskNipi.Event.MobileClasses;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Event.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewUser
    {
        public static readonly BindableProperty MobUserProperty = BindableProperty.Create(nameof(MobUser), typeof(MobUser), typeof(ViewUser), null, propertyChanged: (bindable, oldVal, newVal) =>
        {
            var view = (ViewUser) bindable;
            view.OnPropertyChanged(nameof(HasUser));
            view.OnPropertyChanged(nameof(UserPhoto));
            view.OnPropertyChanged(nameof(UserPhotoTintColor));
            view.OnPropertyChanged(nameof(UserName));
            view.OnPropertyChanged(nameof(ViewMobOrganization));
            view.OnPropertyChanged(nameof(HasViewMobOrganization));
            view.OnPropertyChanged(nameof(ViewMobPost));
            view.OnPropertyChanged(nameof(HasViewMobPost));
        });

        public ViewUser()
        {
            InitializeComponent();
        }

        #region Делегаты

        public event EventHandler<MobUserClickedEventArgs> MobUserClicked;

        #endregion

        #region Свойства

        public MobUser MobUser
        {
            get => (MobUser) GetValue(MobUserProperty);
            set => SetValue(MobUserProperty, value);
        }

        public bool HasUser => MobUser != null;

        public ImageSource UserPhoto => UserPhotoExist ? ImageSource.FromStream(() => new MemoryStream(MobUser.Photo)) : ImageSource.FromFile("user_profile_medium.png");

        private bool UserPhotoExist => HasUser && MobUser.Photo != null && MobUser.Photo.Length > 0;

        public Color UserPhotoTintColor => UserPhotoExist ? Color.Transparent : App.ColorDisabled;

        public string UserName => HasUser ? MobUser.ViewNameShort : string.Empty;

        public string ViewMobOrganization => HasUser ? MobUser.ViewMobOrganization : string.Empty;

        public bool HasViewMobOrganization => !string.IsNullOrEmpty(ViewMobOrganization);

        public string ViewMobPost => HasUser ? MobUser.ViewMobPost : string.Empty;

        public bool HasViewMobPost => !string.IsNullOrEmpty(ViewMobPost);

        #endregion

        #region События

        private void MobUser_OnTapped(object sender, EventArgs e)
        {
            if (HasUser)
                MobUserClicked?.Invoke(this, new MobUserClickedEventArgs(MobUser));
        }

        #endregion
    }

    public class MobUserClickedEventArgs : EventArgs
    {
        public MobUserClickedEventArgs(MobUser mobUser)
        {
            MobUser = mobUser;
        }

        #region Свойства

        public MobUser MobUser { get; }

        #endregion
    }
}