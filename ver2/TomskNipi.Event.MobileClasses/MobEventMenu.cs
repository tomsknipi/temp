using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TomskNipi.Event.MobileClasses.Annotations;

namespace TomskNipi.Event.MobileClasses
{
    public class MobEventMenu : MobAbstractEntity, INotifyPropertyChanged
    {
        public MobEventMenu(Guid id, string name, string description, DateTime dateFrom, DateTime dateTo, string location)
        {
            Id = id;
            Name = name;
            Description = description;
            DateFrom = dateFrom;
            DateTo = dateTo;
            Location = location;
        }

        #region Поля

        private int _messageEventUserCount;

        #endregion

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
        /// Количество уведомлений пользователя мероприятия
        /// </summary>
        public int MessageEventUserCount
        {
            get => _messageEventUserCount;
            set
            {
                if (!_messageEventUserCount.Equals(value))
                {
                    _messageEventUserCount = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ViewDateFromTo => $"{DateFrom:dd.MM.yyyy HH:mm} - {(DateFrom.Date == DateTo.Date ? DateTo.ToString("HH:mm") : DateTo.ToString("dd.MM.yyyy HH:mm"))}";

        public string ViewLocation => string.IsNullOrEmpty(Location) ? string.Empty : Location.Trim();

        public bool HasViewLocation => !string.IsNullOrEmpty(ViewLocation);

        public string DateFromJava => DateToString(DateFrom);

        public string DateToJava => DateToString(DateTo);

        #endregion

        #region Методы

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region INotifyPropertyChanged Реализаторы интерфейса

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}