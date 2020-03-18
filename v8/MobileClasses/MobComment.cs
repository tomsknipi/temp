using System;

namespace TomskNipi.Event.MobileClasses
{
    public class MobComment : MobAbstractEntity
    {
        public MobComment(Guid id, string commentValue, string typeName, Guid typeId, DateTime dateInput, MobUser mobUser)
        {
            Id = id;
            CommentValue = commentValue;
            TypeName = typeName;
            TypeId = typeId;
            DateInput = dateInput;
            MobUser = mobUser;
        }

        #region Свойства

        /// <summary>
        /// Значение комментария
        /// </summary>
        public string CommentValue { get; set; }

        /// <summary>
        /// Тип комментария
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Идентификатор типа комментария
        /// </summary>
        public Guid TypeId { get; set; }

        /// <summary>
        /// Дата комментария
        /// </summary>
        public DateTime DateInput { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public MobUser MobUser { get; set; }

        public string DateInputJava => DateToString(DateInput);

        #endregion
    }
}