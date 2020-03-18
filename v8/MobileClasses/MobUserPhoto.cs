using System;

namespace TomskNipi.Event.MobileClasses
{
    [Serializable]
    public class MobUserPhoto : MobAbstractEntity
    {
        public MobUserPhoto(Guid id, byte[] photo)
        {
            Id = id;
            Photo = photo;
        }

        #region Свойства

        public byte[] Photo { get; set; }

        #endregion
    }
}