using System;
using Event.Classes;
using TomskNipi.Event.MobileClasses;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Event.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewUserSmall
    {
        public static readonly BindableProperty MobUserProperty = BindableProperty.Create(nameof(MobUser), typeof(MobUser), typeof(ViewUserSmall), null, propertyChanged: (bindable, oldVal, newVal) =>
        {
            var view = (ViewUserSmall) bindable;
            view.OnPropertyChanged(nameof(HasUser));
            view.OnPropertyChanged(nameof(UserPhoto));
            view.OnPropertyChanged(nameof(UserName));
        });

        public ViewUserSmall()
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

        public string UserPhoto => Service.GetUserPhoto(HasUser ? MobUser.Id : Guid.Empty);

        public string UserName => HasUser ? MobUser.ViewNameShort : string.Empty;

        #endregion

        #region События

        private void MobUser_OnTapped(object sender, EventArgs e)
        {
            if (HasUser)
            {
                MobUserClicked?.Invoke(this, new MobUserClickedEventArgs(MobUser));
            }
        }

        #endregion
    }
}