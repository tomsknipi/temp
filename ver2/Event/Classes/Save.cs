using System;
using MobileLibrary.Extension;
using Xamarin.Essentials;

namespace Event.Classes
{
    /// <summary>
    /// Хранение данных приложения на устройстве
    /// </summary>
    public static class Save
    {
        #region Свойства

        /// <summary>
        /// Логин
        /// </summary>
        public static string ConnectionLogin
        {
            get => Preferences.Get(nameof(ConnectionLogin), string.Empty);
            set => Preferences.Set(nameof(ConnectionLogin), value);
        }

        /// <summary>
        /// Пароль
        /// </summary>
        public static string ConnectionPassword
        {
            get => Preferences.Get(nameof(ConnectionPassword), string.Empty);
            set => Preferences.Set(nameof(ConnectionPassword), value);
        }

        /// <summary>
        /// Идентификатор мероприятия
        /// </summary>
        public static Guid EventId
        {
            get => Guid.TryParse(Preferences.Get($"{nameof(EventId)}v1", string.Empty), out var result) ? result : Guid.Empty;
            set => Preferences.Set($"{nameof(EventId)}v1", value.AsString());
        }

        /// <summary>
        /// True - подключение готово к связи с сервером
        /// </summary>
        public static bool ConnectionValid => !ConnectionLogin.IsNullOrEmpty() && !ConnectionPassword.IsNullOrEmpty();

        #endregion
    }
}