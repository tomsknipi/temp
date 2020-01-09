﻿using System;
using System.IO;
using System.Threading.Tasks;
using Event.Classes;
using Event.Extension;
using MobileLibrary.Extension;
using TomskNipi.Event.MobileClasses;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Event
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageEventUserCard
    {
        public PageEventUserCard(MobUser mobUser)
        {
            MobUser = mobUser;
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

        public MobUser MobUser { get; }

        public bool HasUser => MobUser != null;

        public ImageSource UserPhoto => UserPhotoExist ? ImageSource.FromStream(() => new MemoryStream(MobUser.Photo)) : ImageSource.FromFile("user_profile_large.png");

        private bool UserPhotoExist => HasUser && MobUser.Photo != null && MobUser.Photo.Length > 0;

        public Color UserPhotoTintColor => UserPhotoExist ? Color.Transparent : App.ColorSelected;

        public string UserName => HasUser ? MobUser.ViewName : string.Empty;

        public string ViewMobOrganization => HasUser ? MobUser.ViewMobOrganization : string.Empty;

        public bool HasViewMobOrganization => !string.IsNullOrEmpty(ViewMobOrganization);

        public string ViewMobPost => HasUser ? MobUser.ViewMobPost : string.Empty;

        public bool HasViewMobPost => !string.IsNullOrEmpty(ViewMobPost);

        public string Title2 => HasUser ? MobUser.ViewNameShort : string.Empty;

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
            if (Config.User != null && Config.MobEvent != null && MobUser != null)
            {
                var result = await Service.GetLike(Config.User.Id, Config.MobEvent.Id, "Users", MobUser.Id);
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

        private async void UserPhoto_OnTapped(object sender, EventArgs e)
        {
            if (UserPhotoExist)
                await Navigation.GetPushModalAsync(new DialogPhotoView(UserPhoto));
        }

        #endregion
    }
}