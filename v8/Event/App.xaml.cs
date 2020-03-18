using Xamarin.Forms;

// ReSharper disable UnusedMember.Global

namespace Event
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new MainPage();
        }

        #region Методы

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        #endregion

        #region Font

        public static double FontMicro => (double) Current.Resources["FontMicro"];

        public static double FontSmall => (double) Current.Resources["FontSmall"];

        public static double FontMedium => (double) Current.Resources["FontMedium"];

        public static double FontLarge => (double) Current.Resources["FontLarge"];

        #endregion

        #region Color

        public static Color ColorPrimary => (Color) Current.Resources["Primary"];

        public static Color ColorPrimaryDark => (Color) Current.Resources["PrimaryDark"];

        public static Color ColorPrimaryLight => (Color) Current.Resources["PrimaryLight"];

        public static Color ColorAccent => (Color) Current.Resources["Accent"];

        public static Color ColorPrimaryText => (Color) Current.Resources["PrimaryText"];

        public static Color ColorSecondaryText => (Color) Current.Resources["SecondaryText"];

        public static Color ColorPlaceholderTextMicro => (Color) Current.Resources["PlaceholderTextMicro"];

        public static Color ColorPlaceholderText => (Color) Current.Resources["PlaceholderText"];

        public static Color ColorIcons => (Color) Current.Resources["Icons"];

        public static Color ColorDivider => (Color) Current.Resources["Divider"];

        public static Color ColorWindow => (Color) Current.Resources["Window"];

        public static Color ColorSelected => (Color) Current.Resources["Selected"];

        public static Color ColorDisabled => (Color) Current.Resources["Disabled"];

        #endregion
    }
}