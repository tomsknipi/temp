namespace TomskNipi.Event.MobileClasses
{
    /// <summary>
    /// Результат запроса мобильного приложения
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MobResult<T>
    {
        #region Свойства

        /// <summary>
        /// Возвращаемые данные
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// True - ошибка
        /// </summary>
        public bool Error { get; set; }

        /// <summary>
        /// Стек ошибки
        /// </summary>
        public string ErrorStack { get; set; }

        /// <summary>
        /// Сообщение сервера
        /// </summary>
        public string Text { get; set; }

        #endregion
    }
}