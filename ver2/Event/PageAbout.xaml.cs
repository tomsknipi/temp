using System.Threading.Tasks;
using System.Windows.Input;
using Event.Classes;
using MobileLibrary.Extension;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Event
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageAbout
    {
        public PageAbout()
        {
            InitializeComponent();
            ImageLogo.Scale = 0;
            ImageLogo.Opacity = 0;
        }

        #region Свойства

        public string VersionText => Config.AppVersion;

        public string BuildText => Config.AppBuild;

        public ICommand CommandPhone { get; } = new Command<string>(arg =>
        {
            if (arg.IsNullOrEmpty())
                return;
            MobileLibrary.Common.Utils.PhoneCall(arg);
        });

        public ICommand CommandMail { get; } = new Command<string>(arg =>
        {
            if (arg.IsNullOrEmpty())
                return;
            MobileLibrary.Common.Utils.SendEmail(arg);
        });

        public ICommand CommandUrl { get; } = new Command<string>(arg =>
        {
            if (arg.IsNullOrEmpty())
                return;
            MobileLibrary.Common.Utils.OpenUrl(arg);
        });

        #endregion

        #region Методы

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Task.WhenAll(ImageLogo.ScaleTo(1, 1000), ImageLogo.FadeTo(1, 1000));
        }

        #endregion
    }
}