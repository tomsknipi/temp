using System.Threading.Tasks;
using Event.Classes;
using Event.Extension;
using TomskNipi.Event.MobileClasses;
using Xamarin.Forms.Xaml;

namespace Event
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageEventSheduleCard
    {
        public PageEventSheduleCard(MobShedule mobShedule)
        {
            MobShedule = mobShedule;
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

        public MobShedule MobShedule { get; }

        public bool HasShedule => MobShedule != null;

        public string Title2 => HasShedule ? MobShedule.Name : string.Empty;

        #endregion

        #region Методы

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_isInitialized)
            {
                return;
            }

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
            if (Config.User != null && Config.MobEvent != null && MobShedule != null)
            {
                var result = await Service.GetLike(Config.User.Id, Config.MobEvent.Id, "SubEvents", MobShedule.Id);
                valid = result.Validate(this);
                if (valid)
                {
                    MobLike = result.Data;
                }
            }
            else
            {
                MobLike = null;
            }

            return valid;
        }

        #endregion
    }
}