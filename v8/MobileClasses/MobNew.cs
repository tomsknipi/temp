using System;

namespace TomskNipi.Event.MobileClasses
{
    public class MobNew : MobAbstractEntity
    {
        public MobNew(Guid id, string name, string description, DateTime dateInput, Guid userId, MobUser mobUser, Guid eventId, string eventName, DateTime eventDateFrom, DateTime eventDateTo, string eventLocation)
        {
            Id = id;
            Name = name;
            Description = description;
            DateInput = dateInput;
            UserId = userId;
            MobUser = mobUser;
            EventId = eventId;
            EventName = eventName;
            EventDateFrom = eventDateFrom;
            EventDateTo = eventDateTo;
            EventLocation = eventLocation;
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
        /// Дата публикации
        /// </summary>
        public DateTime DateInput { get; set; }

        /// <summary>
        /// Идентификатор автора
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Автор
        /// </summary>
        public MobUser MobUser { get; set; }

        /// <summary>
        /// Идентификатор мероприятия
        /// </summary>
        public Guid EventId { get; set; }

        public string EventName { get; set; }

        public DateTime EventDateFrom { get; set; }

        public string EventDateFromJava => DateToString(EventDateFrom);

        public DateTime EventDateTo { get; set; }

        public string EventDateToJava => DateToString(EventDateTo);

        public string EventLocation { get; set; }

        public string ViewName => string.IsNullOrEmpty(Name) ? string.Empty : Name.Trim();

        public string ViewDescription => string.IsNullOrEmpty(Description) ? string.Empty : Description.Trim();

        public bool HasViewDescription => !string.IsNullOrEmpty(ViewDescription);

        public string DateInputJava => DateToString(DateInput);

        public int MessageCount { get; set; }

        #endregion
    }
}