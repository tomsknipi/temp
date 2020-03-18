using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Event.Classes;
using Event.Extension;
using MobileLibrary.Common;
using MobileLibrary.Extension;
using TomskNipi.Event.MobileClasses.Enum;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Event
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [DesignTimeVisible(false)]
    public partial class MainPage : INotifyReceiver
    {
        #region MenuType Перечисление

        public enum MenuType
        {
            PageLoad,
            PageEvent,
            PageEventDialogSelect,
            PageEventSection,
            PageEventUser,
            PageEventNew,
            PageEventPresentation,
            PageEventShedule,
            PageAccount,
            PageAbout
        }

        #endregion

        public MainPage()
        {
            MenuItems.Add(new MenuItem("home.png", "Выбор мероприятия", MenuType.PageEventDialogSelect, false));
            MenuItems.Add(new MenuItem("filter_none.png", "Секции", MenuType.PageEventSection, false));
            MenuItems.Add(new MenuItem("dvr.png", "Новости", MenuType.PageEventNew, false));
            MenuItems.Add(new MenuItem("people.png", "Участники", MenuType.PageEventUser, false));
            MenuItems.Add(new MenuItem("description.png", "Материалы", MenuType.PageEventPresentation, false));
            MenuItems.Add(new MenuItem("access_time.png", "Расписание", MenuType.PageEventShedule, false));

            MenuItems.Add(new MenuItem("account_box.png", "Аккаунт", MenuType.PageAccount, true));
            MenuItems.Add(new MenuItem("info.png", "О Программе", MenuType.PageAbout, false));

            InitializeComponent();
            Detail = new NavigationPage(new PageLoad());
        }

        #region Поля

        private Color _eventBackgroundColor = App.ColorWindow;

        #endregion

        #region Свойства

        public List<MenuItem> MenuItems { get; } = new List<MenuItem>();

        public List<MenuItem> MenuItemsVisible => MenuItems.Where(n => n.IsVisible).ToList();

        private static bool HasUser => Config.User != null;

        private static bool HasEvent => HasUser && Config.MobEvent != null;

        public string EventViewDateFromTo => HasEvent ? Config.MobEvent.ViewDateFromTo : string.Empty;

        public string EventLocation => HasEvent ? Config.MobEvent.Location : string.Empty;

        public bool HasEventLocation => !string.IsNullOrEmpty(EventLocation);

        public string EventName => HasEvent ? Config.MobEvent.Name : string.Empty;

        public Color EventBackgroundColor
        {
            get => _eventBackgroundColor;
            set
            {
                if (_eventBackgroundColor != value)
                {
                    _eventBackgroundColor = value;
                    OnPropertyChanged(nameof(EventBackgroundColor));
                }
            }
        }

        #endregion

        #region Методы

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (nameof(Detail) == propertyName)
                UpdateSelectedMenu();
        }

        private void UpdateSelectedMenu()
        {
            var menuType = Detail.Navigation.NavigationStack[0].GetType() switch
            {
                { } page when page == typeof(PageEvent) => MenuType.PageEvent,
                { } page when page == typeof(PageEventSection) => MenuType.PageEventSection,
                { } page when page == typeof(PageEventUser) => MenuType.PageEventUser,
                { } page when page == typeof(PageEventNew) => MenuType.PageEventNew,
                { } page when page == typeof(PageEventPresentation) => MenuType.PageEventPresentation,
                { } page when page == typeof(PageEventShedule) => MenuType.PageEventShedule,
                { } page when page == typeof(PageAccount) => MenuType.PageAccount,
                { } page when page == typeof(PageAbout) => MenuType.PageAbout,
                { } page when page == typeof(PageLoad) => MenuType.PageLoad,
                _ => throw new Exception($"Неизвестный тип окна: {Detail.Navigation.NavigationStack[0].GetType().Name}")
            };

            var items = MenuItems.Where(n => n.MenuType != menuType && n.IsSelected);
            foreach (var item in items)
            {
                item.IsSelected = false;
            }

            items = MenuItems.Where(n => n.MenuType == menuType && !n.IsSelected);
            foreach (var item in items)
            {
                item.IsSelected = true;
            }

            EventBackgroundColor = menuType == MenuType.PageEvent ? App.ColorSelected : App.ColorWindow;
        }

        #endregion

        #region События

        private async void Event_OnTapped(object sender, EventArgs e)
        {
            if (!HasEvent)
                return;

            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                await Task.Delay(100);
                IsPresented = false;
                await Task.Delay(225);

                Detail = new NavigationPage(new PageEvent(Config.MobEvent));
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void ListViewMenu_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (IsBusy)
                return;

            if (!(e.Item is MenuItem item))
                return;

            if (Config.User == null && Save.ConnectionValid && item.MenuType != MenuType.PageAbout)
            {
                IsBusy = true;
                try
                {
                    await Task.Delay(100);
                    var result = await Service.GetAuthentication(Save.ConnectionLogin, Save.ConnectionPassword);
                    if (result.Validate(this))
                    {
                        Config.User = result.Data;
                    }
                    else
                    {
                        UpdateSelectedMenu();
                        if (item.MenuType != MenuType.PageAccount)
                            return;
                    }
                }
                finally
                {
                    IsBusy = false;
                }
            }

            IsPresented = false;
            await Task.Delay(225);

            switch (item.MenuType)
            {
                case MenuType.PageEventDialogSelect:
                    if (Config.User == null)
                        return;
                    var result = await Navigation.GetPushModalAsync(new DialogEventSelect());
                    if (result != null)
                    {
                        var resultEvent = await Service.GetEvent(Config.User.Id, result.Id, false);
                        if (resultEvent.Validate(this))
                            Detail = new NavigationPage(new PageEvent(resultEvent.Data));
                    }

                    break;
                case MenuType.PageEventSection:
                    Detail = new NavigationPage(new PageEventSection());
                    break;
                case MenuType.PageEventUser:
                    Detail = new NavigationPage(new PageEventUser());
                    break;
                case MenuType.PageEventNew:
                    Detail = new NavigationPage(new PageEventNew());
                    break;
                case MenuType.PageEventPresentation:
                    Detail = new NavigationPage(new PageEventPresentation());
                    break;
                case MenuType.PageEventShedule:
                    Detail = new NavigationPage(new PageEventShedule());
                    break;
                case MenuType.PageAccount:
                    Detail = new NavigationPage(new PageAccount());
                    break;
                case MenuType.PageAbout:
                    await Detail.Navigation.PushAsync(new PageAbout());
                    break;
                default:
                    throw new Exception($"Неизвестный тип меню: {item.MenuType}");
            }
        }

        #endregion

        #region INotifyReceiver Реализаторы интерфейса

        public async void NotifyReceive(string message, object data)
        {
            if (message == Notify.CrossFirebasePushNotificationReceive && Config.User != null && Config.MobEvent != null)
            {
                if (data != null && data is IDictionary<string, object> dictionary
                                 && dictionary.TryGetValue("TypeName", out var typeName) && typeName.AsString() == MessageType.News.AsString()
                                 && dictionary.TryGetValue("EventId", out var eventId) && eventId.AsString() == Config.MobEvent.Id.AsString())
                {
                    var result = await Service.GetEvent(Config.User.Id, Config.MobEvent.Id, false);
                    var valid = result.Validate(null);
                    if (valid && result.Data.Id == Config.MobEvent.Id && result.Data.MessageNewCount != Config.MobEvent.MessageNewCount)
                    {
                        Config.MobEvent.MessageNewCount = result.Data.MessageNewCount;
                        NotifySender.Send(Notify.EventPropertyChanged, null);
                    }
                }

                return;
            }

            if (message == Notify.UserChanged || message == Notify.EventChanged || message == Notify.EventPropertyChanged)
            {
                OnPropertyChanged(nameof(EventViewDateFromTo));
                OnPropertyChanged(nameof(EventLocation));
                OnPropertyChanged(nameof(HasEventLocation));
                OnPropertyChanged(nameof(EventName));

                foreach (var item in MenuItems)
                {
                    item.UpdateProperty();
                }

                OnPropertyChanged(nameof(MenuItemsVisible));
            }
        }

        #endregion

        public class MenuItem : INotifyPropertyChanged
        {
            public MenuItem(string imageSource, string text, MenuType menuType, bool isSplitter)
            {
                ImageSource = imageSource;
                Text = text;
                MenuType = menuType;
                IsSplitter = isSplitter;
            }

            #region Поля

            private bool _isSelected;

            #endregion

            #region Свойства

            public string ImageSource { get; }

            public Color ImageColor => IsEnabled ? App.ColorPlaceholderText : App.ColorDisabled;

            public string Text { get; }

            public MenuType MenuType { get; }

            public bool IsSplitter { get; }

            public bool IsSelected
            {
                get => _isSelected;
                set
                {
                    if (_isSelected != value)
                    {
                        _isSelected = value;
                        OnPropertyChanged(nameof(IsSelected));
                        OnPropertyChanged(nameof(TextColor));
                        OnPropertyChanged(nameof(BackgroundColor));
                    }
                }
            }

            public bool IsEnabled => MenuType == MenuType.PageAbout || MenuType == MenuType.PageAccount || HasUser;

            public bool IsVisible
            {
                get
                {
                    if (MenuType == MenuType.PageAbout || MenuType == MenuType.PageAccount || MenuType == MenuType.PageEventDialogSelect)
                        return true;
                    if (!HasUser || !HasEvent)
                        return false;

                    return MenuType switch
                    {
                        MenuType.PageEventSection => Config.MobEvent.IsSections,
                        MenuType.PageEventUser => Config.MobEvent.IsEventUsers,
                        MenuType.PageEventNew => Config.MobEvent.IsNews,
                        MenuType.PageEventPresentation => Config.MobEvent.IsPresentations,
                        MenuType.PageEventShedule => Config.MobEvent.IsSubEvents,
                        _ => false
                    };
                }
            }

            public int MessageCount
            {
                get
                {
                    if (!HasUser || !HasEvent)
                        return 0;

                    return MenuType switch
                    {
                        MenuType.PageEventDialogSelect => Config.MobEvent == null ? 0 : Config.MobEvent.MessageEventUserCount,
                        MenuType.PageEventNew => Config.MobEvent == null ? 0 : Config.MobEvent.MessageNewCount,
                        _ => 0
                    };
                }
            }

            public Color TextColor => IsEnabled ? App.ColorAccent : App.ColorDisabled;

            public Color BackgroundColor => IsSelected ? App.ColorSelected : Color.Transparent;

            #endregion

            #region Методы

            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            public void UpdateProperty()
            {
                OnPropertyChanged(nameof(IsEnabled));
                OnPropertyChanged(nameof(IsVisible));
                OnPropertyChanged(nameof(ImageColor));
                OnPropertyChanged(nameof(TextColor));
                OnPropertyChanged(nameof(MessageCount));
            }

            #endregion

            #region INotifyPropertyChanged Реализаторы интерфейса

            public event PropertyChangedEventHandler PropertyChanged;

            #endregion
        }
    }
}