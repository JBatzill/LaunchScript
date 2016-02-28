using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Launch_Script.Tools
{
    public static class WindowsOperations
    {

        public enum SnappingDirection
        {
            LEFT, RIGHT, TOP, BOTTOM
        }
        private const int SNAPPING_DELAY = 100;
        public static async Task SnapCurrentWindowClean(SnappingDirection dir, int delay = SNAPPING_DELAY)
        {
            VirtualKeyCode key = VirtualKeyCode.VK_LEFT;
            switch (dir)
            {
                case SnappingDirection.LEFT:
                    key = VirtualKeyCode.VK_LEFT;
                    break;
                case SnappingDirection.RIGHT:
                    key = VirtualKeyCode.VK_RIGHT;
                    break;
                case SnappingDirection.TOP:
                    key = VirtualKeyCode.VK_UP;
                    break;
                case SnappingDirection.BOTTOM:
                    key = VirtualKeyCode.VK_DOWN;
                    break;
            }

            VirtualKeyboard.KeyDown(VirtualKeyCode.VK_LWIN);
            VirtualKeyboard.KeyDown(key);
            VirtualKeyboard.KeyUp(key);
            VirtualKeyboard.KeyUp(VirtualKeyCode.VK_LWIN);
            await Task.Delay(delay);

            //if left or right, "select window for other side"-tool pops up
            if (dir == SnappingDirection.LEFT || dir == SnappingDirection.RIGHT)
            {
                VirtualKeyboard.KeyDown(VirtualKeyCode.VK_ESCAPE);
                VirtualKeyboard.KeyUp(VirtualKeyCode.VK_ESCAPE);
                await Task.Delay(delay);
            }
        }

        public static async Task SnapCurrentWindowClean(List<SnappingDirection> dirs, int delay = SNAPPING_DELAY)
        {
            //press windows key
            VirtualKeyboard.KeyDown(VirtualKeyCode.VK_LWIN);

            //press all other keys
            VirtualKeyCode key = VirtualKeyCode.VK_UP;
            foreach (SnappingDirection dir in dirs)
            {
                //get key for direction
                switch (dir)
                {
                    case SnappingDirection.LEFT:
                        key = VirtualKeyCode.VK_LEFT;
                        break;
                    case SnappingDirection.RIGHT:
                        key = VirtualKeyCode.VK_RIGHT;
                        break;
                    case SnappingDirection.TOP:
                        key = VirtualKeyCode.VK_UP;
                        break;
                    case SnappingDirection.BOTTOM:
                        key = VirtualKeyCode.VK_DOWN;
                        break;
                }

                VirtualKeyboard.KeyDown(key);
                VirtualKeyboard.KeyUp(key);
                await Task.Delay(delay);
            }

            VirtualKeyboard.KeyUp(VirtualKeyCode.VK_LWIN);
            await Task.Delay(delay);

            //if left or right was last operation, "select window for other side"-tool pops up
            if (key == VirtualKeyCode.VK_LEFT || key == VirtualKeyCode.VK_RIGHT)
            {
                VirtualKeyboard.KeyDown(VirtualKeyCode.VK_ESCAPE);
                VirtualKeyboard.KeyUp(VirtualKeyCode.VK_ESCAPE);
                await Task.Delay(delay);
            }
        }

        public static string GetSystemDefaultBrowser()
        {
            string browserName = "iexplore.exe";
            using (RegistryKey userChoiceKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice"))
            {
                if (userChoiceKey != null)
                {
                    object progIdValue = userChoiceKey.GetValue("Progid");
                    if (progIdValue != null)
                    {
                        if (progIdValue.ToString().ToLower().Contains("chrome"))
                            browserName = "chrome.exe";
                        else if (progIdValue.ToString().ToLower().Contains("firefox"))
                            browserName = "firefox.exe";
                        else if (progIdValue.ToString().ToLower().Contains("safari"))
                            browserName = "safari.exe";
                        else if (progIdValue.ToString().ToLower().Contains("opera"))
                            browserName = "opera.exe";
                    }
                }
            }

            return browserName;
        }

        #region FILE_ASSOCIATION
        //https://msdn.microsoft.com/en-us/library/windows/desktop/hh127451(v=vs.85).aspx


        private static string _getFinalKey(string id, string ext)
        {
            return id + ext;
        }
        private static string _getExtKey(string ext)
        {
            return ext + "\\OpenWithProgIds\\";
        }
        private static string _getAppKey(string finalID)
        {
            return finalID + "\\shell\\Open\\command\\";
        }

        private static bool IsFileTypeAssociatedApp(string finalID)
        {
            RegistryKey key = Registry.ClassesRoot.OpenSubKey(_getAppKey(finalID));
            if (key == null) return false;

            return key.GetValueNames().Contains("");
        }
        private static bool ContainsFileTypeAssociatedTraceApp(string finalID)
        {
            RegistryKey key = Registry.ClassesRoot.OpenSubKey(finalID);
            return key != null;
        }
        private static bool IsFileTypeAssociatedExt(string ext, string finalID)
        {
            RegistryKey key = Registry.ClassesRoot.OpenSubKey(_getExtKey(ext));
            if (key == null) return false;

            return key.GetValueNames().Contains(finalID);
        }


        /// <summary>
        /// Adds the current program as std program for files with .ext extension
        /// if not added yet.
        /// </summary>
        /// <param name="ext">new extension</param>
        /// <param name="id">uniqueId</param>
        public static void AssociateFileTypeIfNeeded(string ext, string id)
        {
            if (String.IsNullOrEmpty(ext) || !ext.StartsWith(".")) throw new ArgumentException("Invalid extension!");
            if (String.IsNullOrEmpty(id)) throw new ArgumentException("invalid id");

            string finalID = _getFinalKey(id, ext);

            if (!IsFileTypeAssociatedExt(ext, finalID))
            {
                RegistryKey keyExt = Registry.ClassesRoot.CreateSubKey(_getExtKey(ext), RegistryKeyPermissionCheck.ReadWriteSubTree);
                keyExt.SetValue(finalID, "");
            }
                
            if (!IsFileTypeAssociatedApp(finalID))
            {
                RegistryKey keyApp = Registry.ClassesRoot.CreateSubKey(_getAppKey(finalID), RegistryKeyPermissionCheck.ReadWriteSubTree);
                //"" => (Default)
                keyApp.SetValue("", System.Reflection.Assembly.GetEntryAssembly().Location + " %1 ");
            }
        }

        public static void RemoveFileTypeAssociation(string ext, string id)
        {
            if (String.IsNullOrEmpty(ext) || !ext.StartsWith(".")) throw new ArgumentException("Invalid extension!");
            if (String.IsNullOrEmpty(id)) throw new ArgumentException("invalid id");

            string finalID = _getFinalKey(id, ext);

            if (!ContainsFileTypeAssociatedTraceApp(finalID)) return;

            Registry.ClassesRoot.DeleteSubKeyTree(finalID + "\\");
        }

        #endregion
    }

}
