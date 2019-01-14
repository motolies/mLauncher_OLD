using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MLauncher.Class
{
    public class IconManager
    {

        public static Bitmap ToBitmap(string path)
        {
            switch (PathManager.GetType(path))
            {
                case PathManager.PathType.Shotcut:
                    using (Icon i = Icon.ExtractAssociatedIcon(PathManager.AbsolutePath(path)))
                    {
                        return new Bitmap(i.ToBitmap());
                    }
                case PathManager.PathType.File:
                    using (Icon i = Icon.ExtractAssociatedIcon(path))
                    {
                        return new Bitmap(i.ToBitmap());
                    }
                case PathManager.PathType.NotExists:
                    return new Bitmap(Properties.Resources.xicon, LauncherForm.Status.IconSize, LauncherForm.Status.IconSize);
                case PathManager.PathType.Directory:
                    return GetIcon(path, false, false).ToBitmap();
                case PathManager.PathType.Null:
                default:
                    return null;
            }
        }

     

        /// <summary>
        /// Get the associated Icon for a file or application, this method always returns
        /// an icon. If the strPath is invalid or there is no idonc the default icon is returned
        /// </summary>

        /// <param name="strPath">full path to the file or directory</param>
        /// <param name="bSmall">if true, the 16x16 icon is returned otherwise the 32x32</param>
        /// <param name="bOpen">if true, and strPath is a folder, returns the 'open' icon rather than the 'closed'</param>
        /// <returns></returns>
        private static Icon GetIcon(string strPath, bool bSmall, bool bOpen)
        {
            SHFILEINFO info = new SHFILEINFO(true);
            int cbFileInfo = Marshal.SizeOf(info);
            SHGFI flags;
            if (bSmall)
                flags = SHGFI.Icon | SHGFI.SmallIcon;
            else
                flags = SHGFI.Icon | SHGFI.LargeIcon;
            if (bOpen) flags = flags | SHGFI.OpenIcon;

            SHGetFileInfo(strPath, 256, out info, (uint)cbFileInfo, flags);

            return Icon.FromHandle(info.hIcon);
        }

        [DllImport("Shell32.dll")]
        private static extern int SHGetFileInfo(
           string pszPath, uint dwFileAttributes,
           out SHFILEINFO psfi, uint cbfileInfo,
           SHGFI uFlags);

        private enum SHGFI
        {
            SmallIcon = 0x00000001,
            OpenIcon = 0x00000002,
            LargeIcon = 0x00000000,
            Icon = 0x00000100,
            DisplayName = 0x00000200,
            Typename = 0x00000400,
            SysIconIndex = 0x00004000,
            LinkOverlay = 0x00008000,
            UseFileAttributes = 0x00000010
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SHFILEINFO
        {
            public SHFILEINFO(bool b)
            {
                hIcon = IntPtr.Zero; iIcon = 0; dwAttributes = 0; szDisplayName = ""; szTypeName = "";
            }
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.LPStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.LPStr, SizeConst = 80)]
            public string szTypeName;
        };
    }
}
