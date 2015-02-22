using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WPS_Editor
{
    public class SuperForm : Form
    { 
        private const int MF_BYPOSITION = 0x400;

        public void DisableCloseButton()
        {
            IntPtr hMenu = GetSystemMenu(this.Handle, false);
            int menuItemCount = GetMenuItemCount(hMenu);
            RemoveMenu(hMenu, menuItemCount - 1, MF_BYPOSITION);
            DrawMenuBar((int)this.Handle);
        }
        public void EnableCloseButton()
        {
            GetSystemMenu(this.Handle, true);
            DrawMenuBar((int)this.Handle);
        }

        // Win32 API declarations
        [DllImport("User32")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("User32")]
        private static extern int GetMenuItemCount(IntPtr hWnd);
        [DllImport("User32")]
        private static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("User32")]
        private static extern IntPtr DrawMenuBar(int hwnd);
    }
}
