using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Event.Classes;
using Event.Extension;
using MobileLibrary.Extension;
using TomskNipi.Event.MobileClasses;
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
    }
}