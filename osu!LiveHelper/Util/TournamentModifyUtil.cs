﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace osu_LiveHelper.Util
{
    public class TournamentModifyUtil
    {

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        private static extern int SetParent(IntPtr hWndChild, IntPtr hWndParent);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);


        public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        public const uint SWP_NOMOVE = 0x0002;
        public const uint SWP_NOSIZE = 0x0001;


        public static void CancelGameWindowsTopMost()
        {
            List<IntPtr> manageWindowHandle = FindWindowsWithText("osu!tourney Tournament Manager");

            if (manageWindowHandle.Count > 0)
            {
                List<IntPtr> littleOsuHandle = FindWindowsWithText("osu!tourney Tournament Client");
                littleOsuHandle.ForEach(handle =>
                {
                    SetWindowPos(handle, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
                });
            }
            else
            {
                Console.WriteLine("找不到直播端，请确保已启动直播端且尚未执行“将直播端合并为单窗口”操作");
            }
        }


        public static void MergeLiveWindows()
        {
            List<IntPtr> manageWindowHandle = FindWindowsWithText("osu!tourney Tournament Manager");

            if (manageWindowHandle.Count > 0)
            {
                IntPtr OsuManagerHandle = manageWindowHandle[0];
                List<IntPtr> littleOsuHandle = FindWindowsWithText("osu!tourney Tournament Client");
                littleOsuHandle.ForEach(handle =>
                {
                    SetParent(handle, OsuManagerHandle);
                });
            }
            else
            {
                Console.WriteLine("找不到直播端，请确保已启动直播端");
            }
        }


        // Delegate to filter which windows to include 
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        /// <summary> Get the text for the window pointed to by hWnd </summary>
        public static string GetWindowText(IntPtr hWnd)
        {
            int size = GetWindowTextLength(hWnd);
            if (size > 0)
            {
                var builder = new StringBuilder(size + 1);
                GetWindowText(hWnd, builder, builder.Capacity);
                return builder.ToString();
            }

            return String.Empty;
        }

        /// <summary> Find all windows that match the given filter </summary>
        /// <param name="filter"> A delegate that returns true for windows
        ///    that should be returned and false for windows that should
        ///    not be returned </param>
        public static List<IntPtr> FindWindows(EnumWindowsProc filter)
        {
            IntPtr found = IntPtr.Zero;
            List<IntPtr> windows = new List<IntPtr>();

            EnumWindows(delegate (IntPtr wnd, IntPtr param)
            {
                if (filter(wnd, param))
                {
                    // only add the windows that pass the filter
                    windows.Add(wnd);
                }

                // but return true here so that we iterate all windows
                return true;
            }, IntPtr.Zero);

            return windows;
        }

        /// <summary> Find all windows that contain the given title text </summary>
        /// <param name="titleText"> The text that the window title must contain. </param>
        public static List<IntPtr> FindWindowsWithText(string titleText)
        {
            return FindWindows(delegate (IntPtr wnd, IntPtr param)
            {
                return GetWindowText(wnd).Contains(titleText);
            });
        }

    }
}
