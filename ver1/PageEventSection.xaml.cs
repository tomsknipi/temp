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
    public partial class PageEventSection
    {
        public PageEventSection()
        {
            InitializeComponent();
        }

        #region Поля

        private bool _isInitialized;

        #endregion

        #region Свойства

        public ObservableCollection<MobSection> SectionList { get; } = new ObservableCollection<MobSection>();

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
                var result = await Service.GetSectionList(Config.MobEvent.Id);
                valid = result.Validate(this);
                if (valid)
                {
                    SectionList.AddRange(result.Data);
                }
            }
            else
                SectionList.Clear();

            return valid;
        }

        #endregion

        #region События

        private void SectionListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(e.Item is MobSection item) || !item.IsSectionReports)
                return;
            Navigation.PushAsync(new PageEventSectionReport(item));
        }

        #endregion
    }
}