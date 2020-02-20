using System;
using System.Collections.Generic;

namespace TomskNipi.Event.MobileClasses
{
    public class MobSectionReport : MobAbstractEntity
    {
        public MobSectionReport(Guid id, string name, string description, DateTime dateFrom, DateTime dateTo, string location, Guid sectionId)
        {
            Id = id;
            Name = name;
            Description = description;
            DateFrom = dateFrom;
            DateTo = dateTo;
            Location = location;
            SectionId = sectionId;
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

        /// <summary>
        /// Идентификатор секции
        /// </summary>
        public Guid SectionId { get; set; }

        /// <summary>
        /// Список докладчиков
        /// </summary>
        public List<MobUser> MobUserList { get; set; } = new List<MobUser>();

        public string ViewDateFromTo => $"{DateFrom:dd.MM.yyyy HH:mm} - {(DateFrom.Date == DateTo.Date ? DateTo.ToString("HH:mm") : DateTo.ToString("dd.MM.yyyy HH:mm"))}";

        public string ViewLocation => string.IsNullOrEmpty(Location) ? string.Empty : Location.Trim();

        public bool HasViewLocation => !string.IsNullOrEmpty(ViewLocation);

        public string ViewName => string.IsNullOrEmpty(Name) ? string.Empty : Name.Trim();

        public string ViewDescription => string.IsNullOrEmpty(Description) ? string.Empty : Description.Trim();

        public bool HasViewDescription => !string.IsNullOrEmpty(ViewDescription);

        public bool HasMobUserList => MobUserList.Count > 0;

        public string DateFromJava => DateToString(DateFrom);

        public string DateToJava => DateToString(DateTo);

        #endregion
    }
}