using System.Threading.Tasks;
using Event.Classes;
using Event.Extension;
using Event.View;
using TomskNipi.Event.MobileClasses;
using Xamarin.Forms.Xaml;

namespace Event
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageEventSectionReportCard
    {
        public PageEventSectionReportCard(MobSectionReport mobSectionReport)
        {
            _mobSectionReport = mobSectionReport;
            InitializeComponent();
        }

        #region Поля

        private bool _isInitialized;

        private MobLike _mobLike;

        private readonly MobSectionReport _mobSectionReport;

        #endregion

        #region Свойства

        public string Title2 => HasReport ? _mobSectionReport.Name : string.Empty;

        public MobSectionReport Report => HasReport ? _mobSectionReport : null;

        public bool HasReport => _mobSectionReport != null;

        public MobLike MobLike
        {
            get => _mobLike;
            set
            {
                _mobLike = value;
                OnPropertyChanged(nameof(MobLike));
            }
        }

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
            if (Config.User != null && Config.MobEvent != null && _mobSectionReport != null)
            {
                var result = await Service.GetLike(Config.User.Id, Config.MobEvent.Id, "Sectionreports", _mobSectionReport.Id);
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

        private async void ViewUser_OnMobUserClicked(object sender, MobUserClickedEventArgs e)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                await Task.Delay(100);
                await Navigation.PushAsync(new PageEventUserCard(e.MobUser));
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion
    }
}