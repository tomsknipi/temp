using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileLibrary.Control
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewActivityIndicator
    {
        public static readonly BindableProperty IsBusyProperty = BindableProperty.Create(nameof(IsBusy), typeof(bool), typeof(ViewActivityIndicator), false);

        public ViewActivityIndicator()
        {
            InitializeComponent();
        }

        #region Свойства

        public bool IsBusy
        {
            get => (bool) GetValue(IsBusyProperty);
            set => SetValue(IsBusyProperty, value);
        }

        #endregion
    }
}