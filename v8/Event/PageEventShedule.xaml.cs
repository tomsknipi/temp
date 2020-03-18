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
    public partial class PageEventShedule
    {
        public PageEventShedule()
        {
            InitializeComponent();
        }

        #region Поля

        private bool _isInitialized;

        #endregion

        #region Свойства

        public ObservableCollection<MobShedule> SheduleList { get; } = new ObservableCollection<MobShedule>();

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
                var result = await Service.GetSheduleList(Config.MobEvent.Id);
                valid = result.Validate(this);
                if (valid)
                {
                    SheduleList.AddRange(result.Data);
                }
            }
            else
                SheduleList.Clear();

            return valid;
        }

        #endregion

        #region События

        private async void SheduleListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                await Task.Delay(100);
                if (e.Item is MobShedule item)
                {
                    await Navigation.PushAsync(new PageEventSheduleCard(item));
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