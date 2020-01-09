using System;
using System.Collections.ObjectModel;
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
    public partial class PageEventSectionReport
    {
        public PageEventSectionReport(MobSection mobSection)
        {
            _mobSection = mobSection;
            InitializeComponent();
        }

        #region Поля

        private bool _isInitialized;

        private readonly MobSection _mobSection;

        #endregion

        #region Свойства

        public string Title2 => HasSection ? _mobSection.Name : string.Empty;

        public bool HasSection => _mobSection != null;

        public ObservableCollection<MobSectionReport> SectionReportList { get; } = new ObservableCollection<MobSectionReport>();

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
            if (Config.User != null && HasSection)
            {
                var result = await Service.GetSectionReportList(_mobSection.Id);
                valid = result.Validate(this);
                if (valid)
                {
                    SectionReportList.AddRange(result.Data);
                }
            }
            else
                SectionReportList.Clear();

            return valid;
        }

        #endregion

        #region События

        private void Report_OnTapped(object sender, EventArgs e)
        {
            if (e is TappedEventArgs eventArgs && eventArgs.Parameter is MobSectionReport sectionReport)
            {
                Navigation.PushAsync(new PageEventSectionReportCard(sectionReport));
            }
        }

        private void ViewUser_OnMobUserClicked(object sender, MobUserClickedEventArgs e)
        {
            Navigation.PushAsync(new PageEventUserCard(e.MobUser));
        }

        #endregion
    }
}