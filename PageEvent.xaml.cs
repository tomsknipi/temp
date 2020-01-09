using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Event.Classes;
using Event.Extension;
using Event.View;
using MobileLibrary.Extension;
using TomskNipi.Event.MobileClasses;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Event
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageEvent
    {
        public PageEvent(MobEvent mobEvent)
        {
            _mobEvent = mobEvent;
            InitializeComponent();
            if (mobEvent != null && mobEvent != Config.MobEvent)
            {
                Save.EventId = _mobEvent.Id;
                Config.MobEvent = _mobEvent;
            }

            CommentList.CollectionChanged += (sender, args) => { OnPropertyChanged(nameof(HasCommentList)); };
        }

        #region Поля

        private bool _isInitialized;

        private readonly MobEvent _mobEvent;

        private MobLike _mobLike;

        #endregion

        #region Свойства

        public MobLike MobLike
        {
            get => _mobLike;
            set
            {
                _mobLike = value;
                OnPropertyChanged(nameof(MobLike));
            }
        }

        public bool HasEvent => _mobEvent != null;

        public bool HasMobUser => HasEvent && _mobEvent.MobUser != null;

        public MobUser MobUser => HasMobUser ? _mobEvent.MobUser : null;

        public string Title2 => HasEvent ? _mobEvent.Name : string.Empty;

        public string ViewDateFromTo => HasEvent ? _mobEvent.ViewDateFromTo : string.Empty;

        public string ViewLocation => HasEvent ? _mobEvent.Location.AsString().Trim() : string.Empty;

        public bool HasViewLocation => !ViewLocation.IsNullOrEmpty();

        public string ViewName => HasEvent ? _mobEvent.Name.AsString().Trim() : string.Empty;

        public string ViewDescription => HasEvent ? _mobEvent.Description.AsString().Trim() : string.Empty;

        public bool HasViewDescription => !ViewDescription.IsNullOrEmpty();

        public string EditorSendPlaceholder => EditorSend.IsFocused || EditorSend.Text.AsString().Length > 0 ? $"Комментировать   {500 - EditorSend.Text.AsString().Length}" : "Комментировать";

        public bool ButtonSendIsEnabled => HasEvent && !string.IsNullOrWhiteSpace(EditorSend.Text.AsString());

        public ObservableCollection<MobComment> CommentList { get; } = new ObservableCollection<MobComment>();

        public bool HasCommentList => CommentList.Count > 0;

        #endregion

        #region Методы

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_isInitialized)
                return;
            if (Config.User != null && Config.MobEvent != null)
            {
                IsBusy = true;
                try
                {
                    _isInitialized = await RefreshData();
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        private async Task<bool> RefreshData()
        {
            var valid = false;
            if (Config.User != null && Config.MobEvent != null)
            {
                var result = await Service.GetLike(Config.User.Id, Config.MobEvent.Id, "Events", Config.MobEvent.Id);
                valid = result.Validate(this);
                if (valid)
                {
                    MobLike = result.Data;
                    var comments = await Service.GetCommentList(Config.MobEvent.Id, "Events", Config.MobEvent.Id);
                    valid = comments.Validate(this);
                    if (valid)
                    {
                        CommentList.AddRange(comments.Data);
                    }
                }
            }
            else
                MobLike = null;

            return valid;
        }

        #endregion

        #region События

        private async void ButtonSend_OnClicked(object sender, EventArgs e)
        {
            IsBusy = true;
            try
            {
                var result = await Service.PostComment(Config.User.Id, Config.MobEvent.Id, "Events", Config.MobEvent.Id, EditorSend.Text.Trim());
                if (result.Validate(this))
                {
                    EditorSend.Text = string.Empty;
                    CommentList.Add(result.Data);
                    await ScrollViewMain.ScrollToAsync(ViewAccordionComment, ScrollToPosition.End, true);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void EditorSend_OnFocused(object sender, FocusEventArgs e)
        {
            Task.Run(() =>
            {
                Thread.Sleep(500);
                OnPropertyChanged(nameof(EditorSendPlaceholder));
                OnPropertyChanged(nameof(ButtonSendIsEnabled));
            });
        }

        private void EditorSend_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            OnPropertyChanged(nameof(EditorSendPlaceholder));
            OnPropertyChanged(nameof(ButtonSendIsEnabled));
        }

        private void EditorSend_OnUnfocused(object sender, FocusEventArgs e)
        {
            OnPropertyChanged(nameof(EditorSendPlaceholder));
            OnPropertyChanged(nameof(ButtonSendIsEnabled));
        }

        private void ViewUser_OnMobUserClicked(object sender, MobUserClickedEventArgs e)
        {
            Navigation.PushAsync(new PageEventUserCard(e.MobUser));
        }

        #endregion
    }
}