using System;
using System.ComponentModel.DataAnnotations;
using TomskNipi.Event.Core.Enum;

namespace TomskNipi.Event.Core.Model
{
    /// <summary>
    /// Презентационный материал
    /// </summary>
    public class Presentation : AbstractEntity
    {
        #region Свойства

        /// <summary>
        /// Наименование
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [MaxLength(500)]
        public string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        [MaxLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// Тип презентационного материала
        /// </summary>
        [Required]
        public PresentationType ValueType { get; set; }

        /// <summary>
        /// Link: содержание ссылки
        /// Document: Имя файла с расширением
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string ValueText { get; set; }

        /// <summary>
        /// MimeType, если это файл
        /// </summary>
        [MaxLength(500)]
        public string ValueMime { get; set; }

        /// <summary>
        /// Размер в байтах, если это файл
        /// </summary>
        public int ValueSize { get; set; }

        /// <summary>
        /// Идентификатор мероприятия
        /// </summary>
        public Guid EventId { get; set; }

        /// <summary>
        /// Мероприятие
        /// </summary>
        public virtual Event Event { get; set; }

        /// <summary>
        /// Файл документа
        /// </summary>
        public virtual PresentationFile PresentationFile { get; set; }

        #endregion
    }
}