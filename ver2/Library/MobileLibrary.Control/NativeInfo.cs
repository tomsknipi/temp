using Xamarin.Forms;

namespace MobileLibrary.Control
{
    public static class NativeInfo
    {
        #region Свойства

        public static string DeviceId => DependencyService.Get<INativeInfo>().DeviceId;

        #endregion

        #region Методы

        public static double GetNativeSize(double size)
        {
            return DependencyService.Get<INativeInfo>().GetNativeSize(size);
        }

        #endregion
    }

    public interface INativeInfo
    {
        #region Свойства

        string DeviceId { get; }

        #endregion

        #region Методы

        double GetNativeSize(double size);

        #endregion
    }
}