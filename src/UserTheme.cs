using System;
using System.Collections.Generic;
using SwinGameSDK;
using System.IO;
using System.Security.Permissions;

namespace Battleship
{
    public static class UserTheme
    {
        //get path of Directory
        private static string path = Directory.GetCurrentDirectory();
        private static string newPath2 = path.Substring(0, (path.Length - 10)) + @"\bin\Debug\Resources\Themes"; //"\\Resources\\images";
        
        //check whether is the themefolder empty or not
        public static bool IsThemeFolderEmpty()
        {
            var info = new DirectoryInfo(newPath2);

            if (info.Exists)
            {
                return info.GetFileSystemInfos().Length == 0;
            }

            return true;
        }

        //check whether this file exist in newPath2
        public static bool IsFileExist(string filenameEx)
        {
            return File.Exists(newPath2 + "\\" + filenameEx);
        }
    }
}
