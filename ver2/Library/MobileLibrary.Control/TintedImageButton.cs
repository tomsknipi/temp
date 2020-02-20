using Xamarin.Forms;

namespace MobileLibrary.Control
{
    public class TintedImageButton : ImageButton
    {
        public static readonly BindableProperty TintColorProperty = BindableProperty.Create(nameof(TintColor), typeof(Color), typeof(TintedImageButton), Color.Black);

        #region Свойства

        public Color TintColor
        {
            get => (Color) GetValue(TintColorProperty);
            set => SetValue(TintColorProperty, value);
        }

        #endregion
    }
}