using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Event.Classes;
using Event.Extension;
using MobileLibrary.Extension;
using TomskNipi.Event.MobileClasses;
using TomskNipi.Event.MobileClasses.Enum;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Event
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageEventPresentation
    {
        public PageEventPresentation()
        {
            InitializeComponent();
        }

        #region Поля

        private bool _isInitialized;

        #endregion

        #region Свойства

        public ObservableCollection<MobPresentation> PresentationList { get; } = new ObservableCollection<MobPresentation>();

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
                var result = await Service.GetPresentationList(Config.MobEvent.Id);
                valid = result.Validate(this);
                if (valid)
                {
                    PresentationList.AddRange(result.Data);
                }
            }
            else
                PresentationList.Clear();

            return valid;
        }

        #endregion

        #region События

        private async void PresentationListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(e.Item is MobPresentation item))
                return;
            switch (item.ValueType)
            {
                case PresentationType.Link:
                    await Launcher.TryOpenAsync(item.ValueText);
                    break;
                case PresentationType.Document:
                    await Launcher.TryOpenAsync(Service.GetPresentationFile(item.Id, item.ValueText));
                    break;
            }
        }

        #endregion
    }
}