using System;
using TomskNipi.Event.MobileClasses.Enum;

namespace TomskNipi.Event.MobileClasses
{
    public class MobLike : MobAbstractEntity
    {
        public MobLike(LikeType likeValue, string typeName, Guid typeId, int likeUpCount, int likeDownCount)
        {
            LikeValue = likeValue;
            TypeName = typeName;
            TypeId = typeId;
            LikeUpCount = likeUpCount;
            LikeDownCount = likeDownCount;
        }

        #region Свойства

        /// <summary>
        /// Значение лайка
        /// </summary>
        public LikeType LikeValue { get; set; }

        /// <summary>
        /// Тип лайка
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Идентификатор типа лайка
        /// </summary>
        public Guid TypeId { get; set; }

        /// <summary>
        /// Количество "Нравится"
        /// </summary>
        public int LikeUpCount { get; set; }

        /// <summary>
        /// Количество "Не нравится"
        /// </summary>
        public int LikeDownCount { get; set; }

        #endregion

        #region Методы

        public void Assign(MobLike value)
        {
            LikeValue = value.LikeValue;
            TypeName = value.TypeName;
            TypeId = value.TypeId;
            LikeUpCount = value.LikeUpCount;
            LikeDownCount = value.LikeDownCount;
        }

        #endregion
    }
}