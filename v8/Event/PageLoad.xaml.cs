using System.Threading.Tasks;
using Event.Classes;
using Event.Extension;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Event
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageLoad
    {
        public PageLoad()
        {
            InitializeComponent();
        }

        #region Поля

        private bool _isInitialized;

        #endregion

        #region Методы

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_isInitialized)
                return;
            IsBusy = true;
            try
            {
                await Task.Delay(100);
                CacheDirectory.Clear();
                await Config.ConnectAndLoginAsync();

                if (Config.User == null)
                {
                    ((MainPage) Application.Current.MainPage).Detail = new NavigationPage(new PageAccount());
                }
                else
                {
                    var result = await Service.GetEvent(Config.User.Id, Save.EventId, true);
                    ((MainPage) Application.Current.MainPage).Detail = result.Validate(null)
                        ? new NavigationPage(new PageEvent(result.Data))
                        : new NavigationPage(new PageEvent(null));
                }
            }
            finally
            {
                IsBusy = false;
            }

            _isInitialized = true;
        }

        #endregion
    }
}