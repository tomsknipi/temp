namespace Event.Classes
{
    /// <summary>
    /// Нотификатор приложения
    /// </summary>
    public static class Notify
    {
        /// <summary>
        /// Уведомление от Firebase
        /// </summary>
        public const string CrossFirebasePushNotificationReceive = "CrossFirebasePushNotificationReceive";

        /// <summary>
        /// Уведомление о смене активного мероприятия
        /// </summary>
        public const string EventChanged = "EventChanged";

        /// <summary>
        /// Уведомление об изменении свойств активного мероприятия
        /// </summary>
        public const string EventPropertyChanged = "EventPropertyChanged";

        /// <summary>
        /// Уведомление о смене активного пользователя
        /// </summary>
        public const string UserChanged = "UserChanged";

        /// <summary>
        /// Уведомление о смене фото активного пользователя
        /// </summary>
        public const string UserPhotoChanged = "UserPhotoChanged";
    }
}