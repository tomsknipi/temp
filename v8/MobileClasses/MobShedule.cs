using System;

namespace TomskNipi.Event.MobileClasses
{
    public class MobShedule : MobAbstractEntity
    {
        public MobShedule(Guid id, string name, string description, DateTime dateFrom, DateTime dateTo, string location)
        {
            Id = id;
            Name = name;
            Description = description;
            DateFrom = dateFrom;
            DateTo = dateTo;
            Location = location;
        }

        #region Свойства

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        public DateTime DateFrom { get; set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        public DateTime DateTo { get; set; }

        /// <summary>
        /// Место проведения
        /// </summary>
        public string Location { get; set; }

        public string ViewDateFromTo => $"{DateFrom:dd.MM.yyyy HH:mm} - {(DateFrom.Date == DateTo.Date ? DateTo.ToString("HH:mm") : DateTo.ToString("dd.MM.yyyy HH:mm"))}";

        public string ViewLocation => string.IsNullOrEmpty(Location) ? string.Empty : Location.Trim();

        public bool HasViewLocation => !string.IsNullOrEmpty(ViewLocation);

        public string ViewName => string.IsNullOrEmpty(Name) ? string.Empty : Name.Trim();

        public string ViewDescription => string.IsNullOrEmpty(Description) ? string.Empty : Description.Trim();

        public bool HasViewDescription => !string.IsNullOrEmpty(ViewDescription);

        public string DateFromJava => DateToString(DateFrom);

        public string DateToJava => DateToString(DateTo);

        #endregion
    }
}