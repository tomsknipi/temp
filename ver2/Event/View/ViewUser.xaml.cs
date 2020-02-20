using System;
using Event.Classes;
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
            view.OnPropertyChanged(nameof(UserName));
            view.OnPropertyChanged(nameof(ViewMobOrganization));
            view.OnPropertyChanged(nameof(HasViewMobOrganization));
            view.OnPropertyChanged(nameof(ViewMobPost));
            view.OnPropertyChanged(nameof(HasViewMobPost));
        });

        public ViewUser()
        {
            InitializeComponent();
            _recognizer.Tapped += MobUser_OnTapped;
        }

        #region Делегаты

        public event EventHandler<MobUserClickedEventArgs> MobUserClicked
        {
            add
            {
                lock (_objectLock)
                {
                    MobUserClickedInternal += value;
                    if (StackMain.GestureRecognizers.IndexOf(_recognizer) <= 0)
                    {
                        StackMain.GestureRecognizers.Add(_recognizer);
                    }
                }
            }
            remove
            {
                lock (_objectLock)
                {
                    MobUserClickedInternal -= value;
                    if (MobUserClickedInternal == null)
                    {
                        StackMain.GestureRecognizers.Remove(_recognizer);
                    }
                }
            }
        }

        private event EventHandler<MobUserClickedEventArgs> MobUserClickedInternal;

        #endregion

        #region Поля

        private readonly object _objectLock = new object();

        private readonly TapGestureRecognizer _recognizer = new TapGestureRecognizer {NumberOfTapsRequired = 1};

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

        public string ViewMobOrganization => HasUser ? MobUser.ViewMobOrganization : string.Empty;

        public bool HasViewMobOrganization => !string.IsNullOrEmpty(ViewMobOrganization);

        public string ViewMobPost => HasUser ? MobUser.ViewMobPost : string.Empty;

        public bool HasViewMobPost => !string.IsNullOrEmpty(ViewMobPost);

        #endregion

        #region События

        private void MobUser_OnTapped(object sender, EventArgs e)
        {
            if (HasUser)
            {
                MobUserClickedInternal?.Invoke(this, new MobUserClickedEventArgs(MobUser));
            }
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