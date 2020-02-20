using System;
using MobileLibrary.Extension;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileLibrary.Control
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewBadge
    {
        private static Color _badgeBorderColor = Color.Default;
        private static Color _badgeColor = Color.Blue;
        private static Color _textColor = Color.White;

        public static BindableProperty BadgeBorderColorProperty = BindableProperty.Create(nameof(BadgeBorderColor), typeof(Color), typeof(ViewBadge), Color.Default, propertyChanged: (bindable, oldVal, newVal) =>
        {
            var view = (ViewBadge) bindable;
            if (view.FrameCircle == null)
                _badgeBorderColor = (Color) newVal;
            else
                view.FrameCircle.BorderColor = (Color) newVal;
        });

        public static BindableProperty BadgeColorProperty = BindableProperty.Create(nameof(BadgeColor), typeof(Color), typeof(ViewBadge), Color.Blue, propertyChanged: (bindable, oldVal, newVal) =>
        {
            var view = (ViewBadge) bindable;
            if (view.FrameCircle == null)
                _badgeColor = (Color) newVal;
            else
                view.FrameCircle.BackgroundColor = (Color) newVal;
        });

        public static BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ViewBadge), Color.White, propertyChanged: (bindable, oldVal, newVal) =>
        {
            var view = (ViewBadge) bindable;
            if (view.LabelBadge == null)
                _textColor = (Color) newVal;
            else
                view.LabelBadge.TextColor = (Color) newVal;
        });

        private const int TextFontSize = 10;

        public static BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(ViewBadge), "", propertyChanged: (bindable, oldVal, newVal) =>
        {
            var view = (ViewBadge) bindable;
            var val = view.GetDisplayText();
            view.LabelBadge.Text = val;
            view.IsVisible = !val.IsNullOrEmpty() && val != "0";
        });

        public static BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(int), typeof(ViewBadge), 0, propertyChanged: (bindable, oldVal, newVal) =>
        {
            var view = (ViewBadge) bindable;
            var val = view.GetDisplayText();
            view.LabelBadge.Text = val;
            view.IsVisible = !val.IsNullOrEmpty() && val != "0";
        });

        public ViewBadge()
        {
            InitializeComponent();

            LabelBadge.TextColor = _textColor;
            FrameCircle.BackgroundColor = _badgeColor;
            FrameCircle.BorderColor = _badgeBorderColor;

            LabelBadge.FontSize = NativeInfo.GetNativeSize(TextFontSize);

            LayoutChanged += (sender, args) =>
            {
                var size = NativeInfo.GetNativeSize(TextFontSize);
                if (!LabelBadge.FontSize.Equals(size))
                    LabelBadge.FontSize = size;
            };
        }

        #region Свойства

        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public int Value
        {
            get => (int) GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public Color TextColor
        {
            get => (Color) GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public Color BadgeColor
        {
            get => (Color) GetValue(BadgeColorProperty);
            set => SetValue(BadgeColorProperty, value);
        }

        public Color BadgeBorderColor
        {
            get => (Color) GetValue(BadgeBorderColorProperty);
            set => SetValue(BadgeBorderColorProperty, value);
        }

        #endregion

        #region Методы

        private string GetDisplayText()
        {
            if (!Text.IsNullOrEmpty())
                return Text;

            if (Value >= 0)
                return Math.Min(Value, 99).ToString();

            return string.Empty;
        }

        #endregion
    }
}