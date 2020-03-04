using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Event.Classes;
using Event.Extension;
using MobileLibrary.Common;
using MobileLibrary.Extension;
using Rg.Plugins.Popup.Extensions;
using TomskNipi.Event.MobileClasses;
using TomskNipi.Event.MobileClasses.Enum;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Event
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DialogEventSelect
    {
        public DialogEventSelect()
        {
            InitializeComponent();
        }

        #region Поля

        private bool _isInitialized;

        #endregion

        #region Свойства

        public ObservableCollection<MobEventMenu> EventList { get; } = new ObservableCollection<MobEventMenu>();

        private DateTime EventLastDateIn => EventList.Count > 0 ? EventList[EventList.Count - 1].DateFrom : DateTime.MaxValue;

        #endregion

        #region Методы

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_isInitialized)
                return;
            if (Config.User != null)
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
                var result = await Service.GetEventMenuList(Config.User.Id, nextPage, EventLastDateIn);
                valid = result.Validate(this);
                if (valid)
                {
                    if (!nextPage)
                        EventList.Clear();
                    EventList.AddRange(result.Data);
                    if (Config.MobEvent != null && Config.MobEvent.MessageEventUserCount > 0)
                    {
                        var read = await Service.PostMessageDateRead(Config.User.Id, Guid.Empty, MessageType.EventUsers);
                        if (read.Validate(null) && read.Data)
                        {
                            Config.MobEvent.MessageEventUserCount = 0;
                            NotifySender.Send(Notify.EventPropertyChanged, null);
                        }
                    }
                }
            }
            else
                EventList.Clear();

            return valid;
        }

        #endregion

        #region События

        private async void ButtonCancel_OnClicked(object sender, EventArgs e)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                await Task.Delay(100);
                PageResult = null;
                await Navigation.PopPopupAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void EventListView_OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (IsBusy || e.Item == null || EventList.Count == 0)
                return;
            if (e.Item == EventList[EventList.Count - 1])
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

        private async void EventListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                await Task.Delay(100);
                if (e.Item is MobEventMenu mobEventMenu)
                {
                    PageResult = mobEventMenu;
                    await Navigation.PopPopupAsync();
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