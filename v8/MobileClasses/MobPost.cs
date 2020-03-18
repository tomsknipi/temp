using System;

namespace TomskNipi.Event.MobileClasses
{
    /// <summary>
    /// Должность
    /// </summary>
    public class MobPost : MobAbstractEntity
    {
        public MobPost(Guid id, string name)
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