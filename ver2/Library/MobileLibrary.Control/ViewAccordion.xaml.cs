using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileLibrary.Control
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewAccordion
    {
        public static readonly BindableProperty TitleBackgroundColorProperty = BindableProperty.Create(nameof(TitleBackgroundColor), typeof(Color), typeof(ViewAccordion), Color.Default,
            propertyChanged: (bindable, oldVal, newVal) =>
            {
                var view = (ViewAccordion) bindable;
                if (view.LayoutTitle != null)
                    view.LayoutTitle.BackgroundColor = (Color) newVal;
            });

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(ViewAccordion), string.Empty, propertyChanged: (bindable, oldVal, newVal) =>
        {
            var view = (ViewAccordion) bindable;
            if (view.LabelTitle != null)
                view.LabelTitle.Text = (string) newVal;
        });

        public static readonly BindableProperty TitleTextColorProperty = BindableProperty.Create(nameof(TitleTextColor), typeof(Color), typeof(ViewAccordion), Color.Black, propertyChanged: (bindable, oldVal, newVal) =>
        {
            var view = (ViewAccordion) bindable;
            if (view.LabelTitle != null)
                view.LabelTitle.TextColor = (Color) newVal;
            if (view.ImageArrow != null)
                view.ImageArrow.TintColor = (Color) newVal;
        });

        public static readonly BindableProperty TitleTextLeftOffsetProperty = BindableProperty.Create(nameof(TitleTextLeftOffset), typeof(int), typeof(ViewAccordion), 0, propertyChanged: (bindable, oldVal, newVal) =>
        {
            var view = (ViewAccordion) bindable;
            if (view.LabelTitle != null)
            {
                var margin = view.LabelTitle.Margin;
                margin.Left = (int) newVal;
                view.LabelTitle.Margin = margin;
            }
        });

        public ViewAccordion()
        {
            InitializeComponent();
            LabelTitle.Text = Title;
            LayoutTitle.BackgroundColor = TitleBackgroundColor;
            LabelTitle.TextColor = ImageArrow.TintColor = TitleTextColor;
            UpdateButton(new StatusChangedEventArgs(ExpandableView.IsExpanded ? ExpandStatus.Expanded : ExpandStatus.Collapsed));
            ExpandableView.StatusChanged += (sender, args) => { UpdateButton(args); };
        }

        #region Свойства

        public string Title
        {
            get => (string) GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public Color TitleBackgroundColor
        {
            get => (Color) GetValue(TitleBackgroundColorProperty);
            set => SetValue(TitleBackgroundColorProperty, value);
        }

        public Color TitleTextColor
        {
            get => (Color) GetValue(TitleTextColorProperty);
            set => SetValue(TitleTextColorProperty, value);
        }

        public int TitleTextLeftOffset
        {
            get => (int) GetValue(TitleTextLeftOffsetProperty);
            set => SetValue(TitleTextLeftOffsetProperty, value);
        }

        public View View
        {
            get => ExpandableView.SecondaryView;
            set => ExpandableView.SecondaryView = value;
        }

        public double Spacing
        {
            get => ExpandableView.Spacing;
            set => ExpandableView.Spacing = value;
        }

        #endregion

        #region Методы

        private async void UpdateButton(StatusChangedEventArgs e)
        {
            if (e.Status == ExpandStatus.Expanding || e.Status == ExpandStatus.Expanded)
                await ImageArrow.RotateTo(0);
            else if (e.Status == ExpandStatus.Collapsing || e.Status == ExpandStatus.Collapsed)
                await ImageArrow.RotateTo(180);
        }

        #endregion
    }
}