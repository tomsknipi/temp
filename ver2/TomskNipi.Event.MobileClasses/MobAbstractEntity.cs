using System;
using System.Globalization;

namespace TomskNipi.Event.MobileClasses
{
    public abstract class MobAbstractEntity
    {
        #region Свойства

        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        #endregion

        #region Методы

        public static string DateToString(DateTime value, string format = "yyyy-MM-dd HH:mm")
        {
            return value.ToString(format);
        }

        public static DateTime StringToDate(string value, string format = "yyyy-MM-dd HH:mm")
        {
            return DateTime.ParseExact(value, format, new CultureInfo("ru-RU"), DateTimeStyles.AllowWhiteSpaces);
        }

        #endregion
    }
}