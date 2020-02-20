using MobileLibrary.Extension;
using TomskNipi.Event.MobileClasses;
using Xamarin.Forms;

namespace Event.Extension
{
    /// <summary>
    /// Расширение для MobResult
    /// </summary>
    public static class MobResultExtension
    {
        #region Методы

        /// <summary>
        /// Проверка на наличие ошибок. Возвращает True - если ошибки отсутствуют
        /// </summary>
        /// <param name="mobResult">Результат запроса</param>
        /// <param name="page">Страница для отображения сообщения об ошибке. Если null - то сообщение об ошибке не отображается</param>
        /// <returns></returns>
        public static bool Validate<T>(this MobResult<T> mobResult, Page page)
        {
            if (mobResult != null && !mobResult.Error)
                return true;
            page?.DisplayAlert("Ошибка", mobResult == null || mobResult.Text.IsNullOrEmpty() ? "Внутренняя ошибка" : mobResult.Text, "OK");

            return false;
        }

        #endregion
    }
}