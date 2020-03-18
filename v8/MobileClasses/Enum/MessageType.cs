namespace TomskNipi.Event.MobileClasses.Enum
{
    /// <summary>
    /// Тип сообщения мобильному пользователю
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// Нетипизированное сообщение
        /// </summary>
        Empty,

        /// <summary>
        /// Пользователя добавили в (удалили из) список учистников мероприятия
        /// </summary>
        EventUsers,

        /// <summary>
        /// Добавили (Изменили) новость
        /// </summary>
        News
    }
}