using System;
using System.IO;
using System.Threading.Tasks;
using Event.Classes;
using Event.Extension;
using Event.View;
using MobileLibrary.Extension;
using TomskNipi.Event.MobileClasses;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Event
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageEventNewCard
    {
        public PageEventNewCard(MobNew mobNew)
        {
            MobNew = mobNew;
            InitializeComponent();
        }

        #region Поля

        private bool _isInitialized;

        private MobLike _mobLike;

        #endregion

        #region Свойства

        public MobLike MobLike
        {
            get => _mobLike;
            set
            {
                _mobLike = value;
                OnPropertyChanged(nameof(MobLike));
            }
        }

        public MobNew MobNew { get; }

        public bool HasNew => MobNew != null;

        public string Title2 => HasNew ? MobNew.Name : string.Empty;

        public bool HasMobUser => HasNew && MobNew.MobUser != null;

        public MobUser MobUser => HasMobUser ? MobNew.MobUser : null;

        public string SourcePhoto => HasNew && MobNew.IsPhoto ? Service.GetNewPhoto(MobNew.Id) : null;

        #endregion

        #region Методы

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_isInitialized)
                return;
            if (Config.User != null && Config.MobEvent != null)
            {
                IsBusy = true;
                try
                {
                    _isInitialized = await RefreshData();
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        private async Task<bool> RefreshData()
        {
            var valid = false;
            if (Config.User != null && Config.MobEvent != null && MobNew != null)
            {
                var result = await Service.GetLike(Config.User.Id, Config.MobEvent.Id, "News", MobNew.Id);
                valid = result.Validate(this);
                if (valid)
                {
                    MobLike = result.Data;
                }
            }
            else
                MobLike = null;

            return valid;
        }

        #endregion

        #region События

        private async void SourcePhoto_OnTapped(object sender, EventArgs e)
        {
            if (HasNew && MobNew.IsPhoto)
            {
                var image = await ImagePhoto.GetImageAsJpgAsync();
                if (image != null && image.Length > 0)
                    await Navigation.GetPushModalAsync(new DialogPhotoView(ImageSource.FromStream(() => new MemoryStream(image))));
            }
        }

        private void ViewUser_OnMobUserClicked(object sender, MobUserClickedEventArgs e)
        {
            Navigation.PushAsync(new PageEventUserCard(e.MobUser));
        }

        #endregion
    }
}