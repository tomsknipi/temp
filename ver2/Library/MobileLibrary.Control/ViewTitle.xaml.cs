using MobileLibrary.Extension;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileLibrary.Control
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewTitle
    {
        private static Color _title1Color = Color.Default;
        private static Color _title2Color = Color.Default;

        public static readonly BindableProperty Title1ColorProperty = BindableProperty.Create(nameof(Title1Color), typeof(Color), typeof(ViewTitle), Color.Default, propertyChanged: (bindable, oldVal, newVal) =>
        {
            var view = (ViewTitle) bindable;
            if (view.Label1Title == null)
                _title1Color = (Color) newVal;
            else
                view.Label1Title.TextColor = (Color) newVal;
        });

        private const int Title1FontSize = 19;

        public static readonly BindableProperty Title1Property = BindableProperty.Create(nameof(Title1), typeof(string), typeof(ViewTitle), string.Empty, propertyChanged: (bindable, oldVal, newVal) =>
        {
            var view = (ViewTitle) bindable;
            var val = (string) newVal;
            val = val?.Replace("\r", "").Replace("\n", " ").Trim();
            view.Label1Title.Text = val;
            view.Label1Title.IsVisible = !val.IsNullOrEmpty();
        });

        public static readonly BindableProperty Title2ColorProperty = BindableProperty.Create(nameof(Title2Color), typeof(Color), typeof(ViewTitle), Color.Default, propertyChanged: (bindable, oldVal, newVal) =>
        {
            var view = (ViewTitle) bindable;
            if (view.Label2Title == null)
                _title2Color = (Color) newVal;
            else
                view.Label2Title.TextColor = (Color) newVal;
        });

        private const int Title2FontSize = 16;

        public static readonly BindableProperty Title2Property = BindableProperty.Create(nameof(Title2), typeof(string), typeof(ViewTitle), string.Empty, propertyChanged: (bindable, oldVal, newVal) =>
        {
            var view = (ViewTitle) bindable;
            var val = (string) newVal;
            val = val?.Replace("\r", "").Replace("\n", " ").Trim();
            view.Label2Title.Text = val;
            view.Label2Title.IsVisible = !val.IsNullOrEmpty();
        });

        public ViewTitle()
        {
            InitializeComponent();

            Label1Title.IsVisible = !Label1Title.Text.IsNullOrEmpty();
            Label2Title.IsVisible = !Label2Title.Text.IsNullOrEmpty();

            Label1Title.FontSize = NativeInfo.GetNativeSize(Title1FontSize);
            Label2Title.FontSize = NativeInfo.GetNativeSize(Title2FontSize);

            Label1Title.TextColor = _title1Color;
            Label2Title.TextColor = _title2Color;

            LayoutChanged += (sender, args) =>
            {
                var size = NativeInfo.GetNativeSize(Title1FontSize);
                if (!Label1Title.FontSize.Equals(size))
                    Label1Title.FontSize = size;

                size = NativeInfo.GetNativeSize(Title2FontSize);
                if (!Label2Title.FontSize.Equals(size))
                    Label2Title.FontSize = size;
            };
        }

        #region Свойства

        public string Title1
        {
            get => (string) GetValue(Title1Property);
            set => SetValue(Title1Property, value);
        }

        public Color Title1Color
        {
            get => (Color) GetValue(Title1ColorProperty);
            set => SetValue(Title1ColorProperty, value);
        }

        public string Title2
        {
            get => (string) GetValue(Title2Property);
            set => SetValue(Title2Property, value);
        }

        public Color Title2Color
        {
            get => (Color) GetValue(Title2ColorProperty);
            set => SetValue(Title2ColorProperty, value);
        }

        #endregion
    }
}