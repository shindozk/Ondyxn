using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Ondyxn.UI.Helpers;

public static class NativeWindowHelper
{
    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    private static extern bool ReleaseCapture();

    [DllImport("user32.dll")]
    private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

    private const int GWL_STYLE = -16;
    private const int WS_MAXIMIZEBOX = 0x00010000;
    private const int WS_MINIMIZEBOX = 0x00020000;

    public static void EnableMinimizeAndMaximize(nint hwnd)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return;

        var style = GetWindowLong((IntPtr)hwnd, GWL_STYLE);
        SetWindowLong((IntPtr)hwnd, GWL_STYLE, style | WS_MAXIMIZEBOX | WS_MINIMIZEBOX);
    }

    public static void StartDragging(nint hwnd)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return;
        ReleaseCapture();
        SendMessage((IntPtr)hwnd, 0x00A1, 0x2, 0);
    }
}
