using System;
using System.Threading.Tasks;
using Event.Classes;
using Event.Extension;
using MobileLibrary.Extension;
using TomskNipi.Event.MobileClasses;
using TomskNipi.Event.MobileClasses.Enum;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Event.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewLike
    {
        public static readonly BindableProperty MobLikeProperty = BindableProperty.Create(nameof(MobLike), typeof(MobLike), typeof(ViewLike), null, propertyChanged: (bindable, oldVal, newVal) =>
        {
            var view = (ViewLike) bindable;
            RaisePropertyChangedMobLike(view);
        });

        public static readonly BindableProperty PageProperty = BindableProperty.Create(nameof(Page), typeof(Page), typeof(Page));

        public ViewLike()
        {
            InitializeComponent();
        }

        #region Свойства

        public MobLike MobLike
        {
            get => (MobLike) GetValue(MobLikeProperty);
            set => SetValue(MobLikeProperty, value);
        }

        public Page Page
        {
            get => (Page) GetValue(PageProperty);
            set => SetValue(PageProperty, value);
        }

        public bool HasLike => MobLike != null;

        public Color DownColorImage => HasLike ? MobLike.LikeValue == LikeType.Down ? App.ColorPlaceholderText : App.ColorAccent : App.ColorDisabled;

        public Color DownColorText => HasLike ? App.ColorAccent : App.ColorDisabled;

        public string DownText => HasLike ? MobLike.LikeDownCount.AsString() : "0";

        public Color UpColorImage => HasLike ? MobLike.LikeValue == LikeType.Up ? App.ColorPlaceholderText : App.ColorAccent : App.ColorDisabled;

        public Color UpColorText => HasLike ? App.ColorAccent : App.ColorDisabled;

        public string UpText => HasLike ? MobLike.LikeUpCount.AsString() : "0";

        #endregion

        #region Методы

        private static void RaisePropertyChangedMobLike(ViewLike view)
        {
            view.OnPropertyChanged(nameof(HasLike));
            view.OnPropertyChanged(nameof(DownColorImage));
            view.OnPropertyChanged(nameof(DownColorText));
            view.OnPropertyChanged(nameof(DownText));
            view.OnPropertyChanged(nameof(UpColorImage));
            view.OnPropertyChanged(nameof(UpColorText));
            view.OnPropertyChanged(nameof(UpText));
        }

        #endregion

        #region События

        private async void Down_OnTapped(object sender, EventArgs e)
        {
            if (Page != null && Page.IsBusy)
                return;
            if (!HasLike || string.IsNullOrWhiteSpace(MobLike.TypeName) || MobLike.TypeId == Guid.Empty)
                return;
            var isBusy = false;
            if (Page != null && !Page.IsBusy)
                Page.IsBusy = isBusy = true;
            try
            {
                await Task.Delay(100);
                var likeValue = MobLike.LikeValue switch
                {
                    LikeType.None => LikeType.Down,
                    LikeType.Up => LikeType.Down,
                    _ => LikeType.None
                };

                var result = await Service.PostLike(Config.User.Id, Config.MobEvent.Id, MobLike.TypeName, MobLike.TypeId, likeValue);
                var valid = result.Validate(Page);
                if (valid)
                {
                    MobLike.Assign(result.Data);
                    RaisePropertyChangedMobLike(this);
                }
            }
            finally
            {
                if (Page != null && isBusy)
                    Page.IsBusy = false;
            }
        }

        private async void Up_OnTapped(object sender, EventArgs e)
        {
            if (Page != null && Page.IsBusy)
                return;
            if (!HasLike || string.IsNullOrWhiteSpace(MobLike.TypeName) || MobLike.TypeId == Guid.Empty)
                return;
            var isBusy = false;
            if (Page != null && !Page.IsBusy)
                Page.IsBusy = isBusy = true;
            try
            {
                await Task.Delay(100);
                var likeValue = MobLike.LikeValue switch
                {
                    LikeType.None => LikeType.Up,
                    LikeType.Down => LikeType.Up,
                    _ => LikeType.None
                };

                var result = await Service.PostLike(Config.User.Id, Config.MobEvent.Id, MobLike.TypeName, MobLike.TypeId, likeValue);
                var valid = result.Validate(Page);
                if (valid)
                {
                    MobLike.Assign(result.Data);
                    RaisePropertyChangedMobLike(this);
                }
            }
            finally
            {
                if (Page != null && isBusy)
                    Page.IsBusy = false;
            }
        }

        #endregion
    }
}