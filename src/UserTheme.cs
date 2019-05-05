using System;
using System.Collections.Generic;
using SwinGameSDK;
using System.IO;
using System.Security.Permissions;

namespace Battleship
{
    public static class UserTheme
    {
        private static string path = Directory.GetCurrentDirectory();
        private static string newPath2 = path.Substring(0, (path.Length - 10)) + @"\bin\Debug\Resources\Themes"; //"\\Resources\\images";


        public static void ChangeResourcePath()
        {
            var info = new DirectoryInfo(newPath2);
            if (info.Exists)
            {
                SwinGame.SetAppPath(newPath2);
            }
        }


        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static bool IsThemeFileEmpty()
        {
            var info = new DirectoryInfo(newPath2);

            if (info.Exists)
            {
                return info.GetFileSystemInfos().Length == 0;
            }

            return true;
        }
    }
}
