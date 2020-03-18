using System;

namespace TomskNipi.Event.MobileClasses
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class MobUser : MobAbstractEntity
    {
        public MobUser(Guid id, string login, string lastName, string firstName, string middleName, MobOrganization mobOrganization, MobPost mobPost, bool isAdmin, bool isSuperAdmin)
        {
            Id = id;
            Login = login;
            LastName = lastName;
            FirstName = firstName;
            MiddleName = middleName;
            MobOrganization = mobOrganization;
            MobPost = mobPost;
            IsAdmin = isAdmin;
            IsSuperAdmin = isSuperAdmin;
        }

        #region Свойства

        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Организация
        /// </summary>
        public MobOrganization MobOrganization { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        public MobPost MobPost { get; set; }

        /// <summary>
        /// Администратор
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// True - Супер Администратор :)
        /// </summary>
        public bool IsSuperAdmin { get; set; }

        public string ViewMobOrganization => MobOrganization == null || string.IsNullOrEmpty(MobOrganization.Name) ? string.Empty : MobOrganization.Name.Trim();

        public bool HasViewMobOrganization => !string.IsNullOrEmpty(ViewMobOrganization);

        public string ViewMobPost => MobPost == null || string.IsNullOrEmpty(MobPost.Name) ? string.Empty : MobPost.Name.Trim();

        public bool HasViewMobPost => !string.IsNullOrEmpty(ViewMobPost);

        public string ViewName => $"{LastName} {FirstName} {MiddleName}".Trim();

        public string ViewNameShort
        {
            get
            {
                var result = LastName == null ? string.Empty : LastName.Trim() + ' ';
                var temp = FirstName == null ? string.Empty : FirstName.Trim();
                if (string.IsNullOrEmpty(temp))
                    return result;
                result += char.ToUpper(temp[0]) + ".";
                temp = MiddleName == null ? string.Empty : MiddleName.Trim();
                if (string.IsNullOrEmpty(temp))
                    return result;
                result += char.ToUpper(temp[0]) + ".";

                return result;
            }
        }

        #endregion
    }
}