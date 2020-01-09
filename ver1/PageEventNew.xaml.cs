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
                var result = await Service.GetNewList(Config.MobEvent.Id);
                valid = result.Validate(this);
                if (valid)
                {
                    NewList.AddRange(result.Data);
                }
            }
            else
                NewList.Clear();

            return valid;
        }

        #endregion

        #region События

        private void NewListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(e.Item is MobNew item))
                return;
            Navigation.PushAsync(new PageEventNewCard(item));
        }

        #endregion
    }
}