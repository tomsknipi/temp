using System.ComponentModel;

namespace TomskNipi.Event.MobileClasses.Enum
{
    /// <summary>
    /// Тип лайка
    /// </summary>
    public enum LikeType
    {
        [Description("Нет")] None = 0,
        [Description("Нравится")] Up = 1,
        [Description("Не нравится")] Down = -1
    }
}