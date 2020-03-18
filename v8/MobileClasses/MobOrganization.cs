using System;

namespace TomskNipi.Event.MobileClasses
{
    /// <summary>
    /// Организация
    /// </summary>
    public class MobOrganization : MobAbstractEntity
    {
        public MobOrganization(Guid id, string name, string abbreviation)
        {
            Id = id;
            Name = name;
            Abbreviation = abbreviation;
        }

        #region Свойства

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Сокращенное наименование
        /// </summary>
        public string Abbreviation { get; set; }

        #endregion
    }
}