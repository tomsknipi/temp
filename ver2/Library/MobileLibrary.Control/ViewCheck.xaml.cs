using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileLibrary.Control
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewCheck
    {
        public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(ViewCheck), false, propertyChanged: (bindable, oldVal, newVal) =>
        {
            var view = (ViewCheck) bindable;
            view.Button.Source = (bool) newVal ? "check.png" : "empty.png";
        });

        public new static readonly BindableProperty IsEnabledProperty = BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(ViewCheck), true, propertyChanged: (bindable, oldVal, newVal) =>
        {
            var view = (ViewCheck) bindable;
            view.Button.IsEnabled = (bool) newVal;
        });

        public ViewCheck()
        {
            InitializeComponent();
        }

        #region Делегаты

        public event EventHandler<EventArgs> OnCheckedChanged;

        #endregion

        #region Свойства

        public bool IsChecked
        {
            get => (bool) GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }

        public new bool IsEnabled
        {
            get => (bool) GetValue(IsEnabledProperty);
            set => SetValue(IsEnabledProperty, value);
        }

        #endregion

        #region События

        private void Button_OnClicked(object sender, EventArgs e)
        {
            IsChecked = !IsChecked;
            OnCheckedChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}