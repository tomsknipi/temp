using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TomskNipi.Event.MobileClasses.Annotations;
using TomskNipi.Event.MobileClasses.Enum;

namespace TomskNipi.Event.MobileClasses
{
    public class MobPresentation : MobAbstractEntity, INotifyPropertyChanged
    {
        public MobPresentation(Guid id, string name, string description, PresentationType valueType, string valueText, Guid eventId)
        {
            Id = id;
            Name = name;
            Description = description;
            ValueType = valueType;
            ValueText = valueText;
            EventId = eventId;
        }

        #region Поля

        private double _viewProgress;

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
        /// Тип презентационного материала
        /// </summary>
        public PresentationType ValueType { get; set; }

        /// <summary>
        /// Link: содержание ссылки
        /// Document: Имя файла с расширением
        /// </summary>
        public string ValueText { get; set; }

        /// <summary>
        /// Идентификатор мероприятия
        /// </summary>
        public Guid EventId { get; set; }

        public string ViewName => string.IsNullOrEmpty(Name) ? string.Empty : Name.Trim();

        public string ViewDescription => string.IsNullOrEmpty(Description) ? string.Empty : Description.Trim();

        public bool HasViewDescription => !string.IsNullOrEmpty(ViewDescription);

        /// <summary>
        /// Для отображения прогресса загрузки
        /// </summary>
        public double ViewProgress
        {
            get => _viewProgress;
            set
            {
                if (!_viewProgress.Equals(value))
                {
                    _viewProgress = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Для отображения картинки
        /// </summary>
        public string ViewImage => ValueType == PresentationType.Link ? "open_in_browser.png" : "cloud_download.png";

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