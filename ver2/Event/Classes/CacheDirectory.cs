using System.IO;
using Xamarin.Essentials;

namespace Event.Classes
{
    public static class CacheDirectory
    {
        #region Методы

        public static void Clear()
        {
            try
            {
                var dir = Get();
                Directory.Delete(dir, true);
            }
            catch
            {
                // ignored
            }
        }

        public static string Get()
        {
            var dir = Path.Combine(FileSystem.CacheDirectory, "myCache");
            Directory.CreateDirectory(dir);

            return dir;
        }

        #endregion
    }
}