using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Event.Classes;
using Event.Extension;
using MobileLibrary.Common;
using MobileLibrary.Extension;
using TomskNipi.Event.MobileClasses;
using TomskNipi.Event.MobileClasses.Enum;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Event
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageEventNew
    {
        public PageEventNew()
        {
            InitializeComponent();
        }

        #region Поля

        private bool _isInitialized;

        #endregion

        #region Свойства

        public ObservableCollection<MobNew> NewList { get; } = new ObservableCollection<MobNew>();

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
            if (Config.User != null && Config.MobEvent != null)
            {
                var result = await Service.GetNewList(Config.User.Id, Config.MobEvent.Id);
                valid = result.Validate(this);
                if (valid)
                {
                    NewList.AddRange(result.Data);
                    if (Config.MobEvent.MessageNewCount > 0)
                    {
                        var read = await Service.PostMessageDateRead(Config.User.Id, Config.MobEvent.Id, MessageType.News);
                        if (read.Validate(null) && read.Data)
                        {
                            Config.MobEvent.MessageNewCount = 0;
                            NotifySender.Send(Notify.EventPropertyChanged, null);
                        }
                    }
                }
            }
            else
                NewList.Clear();

            return valid;
        }

        #endregion

        #region События

        private async void NewListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                await Task.Delay(100);
                if (e.Item is MobNew item)
                    await Navigation.PushAsync(new PageEventNewCard(item));
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion
    }
}