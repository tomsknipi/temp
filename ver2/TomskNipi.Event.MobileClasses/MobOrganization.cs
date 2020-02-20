using System;

namespace TomskNipi.Event.MobileClasses
{
    /// <summary>
    /// Организация
    /// </summary>
    public class MobOrganization : MobAbstractEntity
    {
        public MobOrganization(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        #region Свойства

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        #endregion
    }
}