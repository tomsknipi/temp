using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TomskNipi.Event.MobileClasses
{
    public class MobEvent : MobAbstractEntity, INotifyPropertyChanged
    {
        public MobEvent(Guid id, string name, string description, DateTime dateFrom, DateTime dateTo, string location, MobUser mobUser, bool isPresentations, bool isEventUsers, bool isSubEvents, bool isNews, bool isSections)
        {
            Id = id;
            Name = name;
            Description = description;
            DateFrom = dateFrom;
            DateTo = dateTo;
            Location = location;
            MobUser = mobUser;
            IsPresentations = isPresentations;
            IsEventUsers = isEventUsers;
            IsSubEvents = isSubEvents;
            IsNews = isNews;
            IsSections = isSections;
        }

        #region Поля

        private int _messageEventUserCount;

        private int _messageNewCount;

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
        /// Организатор
        /// </summary>
        public MobUser MobUser { get; set; }

        /// <summary>
        /// Наличие презентационных материалов
        /// </summary>
        public bool IsPresentations { get; set; }

        /// <summary>
        /// Наличие пользователей мероприятия
        /// </summary>
        public bool IsEventUsers { get; set; }

        /// <summary>
        /// Наличие расписаний
        /// </summary>
        public bool IsSubEvents { get; set; }

        /// <summary>
        /// Наличие новостей
        /// </summary>
        public bool IsNews { get; set; }

        /// <summary>
        /// Наличие секций
        /// </summary>
        public bool IsSections { get; set; }

        /// <summary>
        /// Количество уведомлений новостей
        /// </summary>
        public int MessageNewCount
        {
            get => _messageNewCount;
            set
            {
                if (!_messageNewCount.Equals(value))
                {
                    _messageNewCount = value;
                    OnPropertyChanged();
                }
            }
        }

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

        public string DateFromJava => DateToString(DateFrom);

        public string DateToJava => DateToString(DateTo);

        #endregion

        #region Методы

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