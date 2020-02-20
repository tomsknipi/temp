using System;

namespace TomskNipi.Event.MobileClasses
{
    public class MobSection : MobAbstractEntity
    {
        public MobSection(Guid id, string name, string description, Guid eventId, DateTime dateFrom, DateTime dateTo, int sectionReportsCount)
        {
            Id = id;
            Name = name;
            Description = description;
            EventId = eventId;
            DateFrom = dateFrom;
            DateTo = dateTo;
            SectionReportsCount = sectionReportsCount;
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
        /// Идентификатор мероприятия
        /// </summary>
        public Guid EventId { get; set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        public DateTime DateFrom { get; set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        public DateTime DateTo { get; set; }

        /// <summary>
        /// Наличие докладов
        /// </summary>
        public bool IsSectionReports => SectionReportsCount > 0;

        /// <summary>
        /// Количество докладов
        /// </summary>
        public int SectionReportsCount { get; set; }

        public string ViewDateFromTo
        {
            get
            {
                if (DateFrom == DateTime.MinValue || DateTo == DateTime.MinValue)
                    return string.Empty;

                return $"{DateFrom:dd.MM.yyyy HH:mm} - {(DateFrom.Date == DateTo.Date ? DateTo.ToString("HH:mm") : DateTo.ToString("dd.MM.yyyy HH:mm"))}";
            }
        }

        public string ViewDescription => string.IsNullOrEmpty(Description) ? string.Empty : Description.Trim();

        public bool HasViewDescription => !string.IsNullOrEmpty(ViewDescription);

        public string DateFromJava => DateToString(DateFrom);

        public string DateToJava => DateToString(DateTo);

        #endregion
    }
}