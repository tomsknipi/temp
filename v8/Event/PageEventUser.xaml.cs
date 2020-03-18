using System.Collections.ObjectModel;
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
    public partial class PageEventUser
    {
        public PageEventUser()
        {
            InitializeComponent();
        }

        #region Поля

        private bool _isInitialized;

        #endregion

        #region Свойства

        public ObservableCollection<MobUser> UserList { get; } = new ObservableCollection<MobUser>();

        private string UserLastName => UserList.Count > 0 ? UserList[UserList.Count - 1].LastName : string.Empty;

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
                    await Task.Delay(100);
                    _isInitialized = await RefreshData(false);
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        private async Task<bool> RefreshData(bool nextPage)
        {
            var valid = false;
            if (Config.User != null)
            {
                var result = await Service.GetUserList(Config.MobEvent.Id, nextPage, UserLastName);
                valid = result.Validate(this);
                if (valid)
                {
                    if (!nextPage)
                        UserList.Clear();
                    UserList.AddRange(result.Data);
                }
            }
            else
                UserList.Clear();

            return valid;
        }

        #endregion

        #region События

        private async void UserListView_OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (IsBusy || e.Item == null || UserList.Count == 0)
                return;
            if (e.Item == UserList[UserList.Count - 1])
            {
                IsBusy = true;
                try
                {
                    await Task.Delay(100);
                    await RefreshData(true);
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        private async void UserListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                await Task.Delay(100);
                if (e.Item is MobUser item)
                {
                    await Navigation.PushAsync(new PageEventUserCard(item));
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion
    }
}