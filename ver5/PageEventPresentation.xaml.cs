using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Event.Classes;
using Event.Extension;
using MobileLibrary.Extension;
using TomskNipi.Event.MobileClasses;
using TomskNipi.Event.MobileClasses.Enum;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Event
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageEventPresentation
    {
        public PageEventPresentation()
        {
            InitializeComponent();
        }

        #region Поля

        private bool _isInitialized;

        #endregion

        #region Свойства

        public ObservableCollection<MobPresentation> PresentationList { get; } = new ObservableCollection<MobPresentation>();

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
                    await Task.Delay(100);
                    _isInitialized = await RefreshData();
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        public async Task OpenDocument(MobPresentation item)
        {
            try
            {
                var url = Service.GetPresentationFile(item.Id, item.ValueText);
                using var webClient = new WebClient();
                webClient.DownloadProgressChanged += async (sender, args) =>
                {
                    item.ViewProgress = args.ProgressPercentage / 100.0f;
                    await Task.Delay(100);
                };
                var file = Path.Combine(CacheDirectory.Get(), item.ValueText);
                await webClient.DownloadFileTaskAsync(url, file);

                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(file)
                });
            }
            finally
            {
                item.ViewProgress = 0;
            }
        }

        private async Task<bool> RefreshData()
        {
            var valid = false;
            if (Config.User != null && Config.MobEvent != null)
            {
                var result = await Service.GetPresentationList(Config.MobEvent.Id);
                valid = result.Validate(this);
                if (valid)
                {
                    PresentationList.AddRange(result.Data);
                }
            }
            else
                PresentationList.Clear();

            return valid;
        }

        #endregion

        #region События

        private async void PresentationListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                await Task.Delay(100);
                if (e.Item is MobPresentation item)
                {
                    switch (item.ValueType)
                    {
                        case PresentationType.Link:
                            await Launcher.TryOpenAsync(item.ValueText);
                            break;
                        case PresentationType.Document:
                            await OpenDocument(item);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion
    }
}