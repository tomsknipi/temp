using System.Threading.Tasks;
using Event.Extension;
using MobileLibrary.Common;
using MobileLibrary.Control;
using MobileLibrary.Extension;
using Plugin.FirebasePushNotification;
using TomskNipi.Event.MobileClasses;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Event.Classes
{
    /// <summary>
    /// Конфигуратор приложения
    /// </summary>
    public static class Config
    {
        private static MobEvent _mobEventInternal;
        private static MobUser _mobUserInternal;

        static Config()
        {
            CrossFirebasePushNotification.Current.OnTokenRefresh += (source, args) => { Device.BeginInvokeOnMainThread(RefreshToken); };
            CrossFirebasePushNotification.Current.OnNotificationReceived += (source, args) => { Device.BeginInvokeOnMainThread(() => { NotifySender.Send(Notify.CrossFirebasePushNotificationReceive, args.Data); }); };
        }

        #region Свойства

        /// <summary>
        /// Активный пользователь
        /// </summary>
        public static MobUser User
        {
            get => _mobUserInternal;
            set
            {
                _mobUserInternal = value;
                RefreshToken();
                NotifySender.Send(Notify.UserChanged, null);
            }
        }

        /// <summary>
        /// Активное мероприятие
        /// </summary>
        public static MobEvent MobEvent
        {
            get => _mobEventInternal;
            set
            {
                _mobEventInternal = value;
                NotifySender.Send(Notify.EventChanged, null);
            }
        }

        /// <summary>
        /// Версия приложения
        /// </summary>
        public static string AppVersion
        {
            get
            {
                var mas = AppInfo.VersionString.Split('.');
                return $"{(mas.Length >= 1 && mas[0].Length > 0 ? mas[0] : "0")}.{(mas.Length >= 2 && mas[1].Length > 0 ? mas[1] : "0")}";
            }
        }

        /// <summary>
        /// Билд приложения
        /// </summary>
        public static string AppBuild => AppInfo.BuildString;

        /// <summary>
        /// Идентификатор приложения
        /// </summary>
        public static string AppId => NativeInfo.DeviceId;

        #endregion

        #region Методы

        public static async Task ConnectAndLoginAsync()
        {
            if (User == null && Save.ConnectionValid)
            {
                var result = await Service.GetAuthentication(Save.ConnectionLogin, Save.ConnectionPassword);
                if (result.Validate(null))
                {
                    User = result.Data;
                }
            }
        }

        private static async void RefreshToken()
        {
            var token = CrossFirebasePushNotification.Current.Token;
            if (User == null || token.IsNullOrEmpty())
                return;
            await Service.PostToken(User.Id, token, AppId, DeviceInfo.Model, $"{DeviceInfo.Platform.ToString()} {DeviceInfo.VersionString}", $"{AppVersion}.{AppBuild}");
        }

        #endregion
    }
}