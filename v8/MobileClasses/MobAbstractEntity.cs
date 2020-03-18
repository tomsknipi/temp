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

        public static string GetSizeString(long length)
        {
            const long Б = 0, КВ = 1024, МБ = КВ * 1024, ГБ = МБ * 1024, ТБ = ГБ * 1024;
            double size = length;
            var suffix = nameof(Б);

            if (length >= ТБ)
            {
                size = Math.Round((double) length / ТБ, 2);
                suffix = nameof(ТБ);
            }
            else if (length >= ГБ)
            {
                size = Math.Round((double) length / ГБ, 2);
                suffix = nameof(ГБ);
            }
            else if (length >= МБ)
            {
                size = Math.Round((double) length / МБ, 2);
                suffix = nameof(МБ);
            }
            else if (length >= КВ)
            {
                size = Math.Round((double) length / КВ, 2);
                suffix = nameof(КВ);
            }

            return $"{size} {suffix}";
        }

        public static DateTime StringToDate(string value, string format = "yyyy-MM-dd HH:mm")
        {
            return DateTime.ParseExact(value, format, new CultureInfo("ru-RU"), DateTimeStyles.AllowWhiteSpaces);
        }

        #endregion
    }
}