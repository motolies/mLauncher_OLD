using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace MLauncher
{
    static class Program
    {
     
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new LauncherForm());

            bool bnew;
            Mutex mutex = new Mutex(true, "MLauncher", out bnew);
            if (bnew)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new LauncherForm());
                mutex.ReleaseMutex();
            }
            else
            {
                //MessageBox.Show("프로그램이 실행중입니다.");
                Process[] processes = Process.GetProcesses();
                foreach (Process proc in processes)
                {
                    if (proc.ProcessName.Equals("MLauncher"))
                    {
                        // 윈도우 핸들러
                        // 창의 상태에 관계없이 프로세스 네임을 가지고 오려면 아래와 같은 방식으로 핸들을 찾는다.
                        //IntPtr procHandler = FindWindow(null, Process.GetProcessById(proc.Id).ProcessName);
                        IntPtr procHandler = FindWindow(null, "MLauncher");

                        //윈도우 크기 구하기
                        RECT r = new RECT();
                        GetWindowRect(procHandler, out r);

                        //화면 센터위치 구하기
                        int pWidth = Screen.PrimaryScreen.Bounds.Width;
                        int pHeight = Screen.PrimaryScreen.Bounds.Height;
                        MoveWindow(procHandler, pWidth/2 - r.Width /2, pHeight / 2 - r.Height / 2, r.Width, r.Height, true);

                        // 활성화
                        ShowWindow(procHandler, SW_SHOWNORMAL);
                        SetForegroundWindow(procHandler);

                        return;
                    }
                }


                Application.ExitThread();
                Environment.Exit(0);
            }
        }

        #region  프로세스 찾아서 띄우기
        //http://rea1man.tistory.com/entry/C-Process%EC%9D%98-WindowState-%EC%A0%9C%EC%96%B4%ED%95%98%EA%B8%B0
        [DllImport("User32", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern void SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("User32.dll")]
        public static extern bool MoveWindow(IntPtr handle, int x, int y, int width, int height, bool redraw);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        private const int SW_SHOWNORMAL = 1;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            //http://pinvoke.net/default.aspx/Structures/RECT.html
            public int Left, Top, Right, Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public RECT(System.Drawing.Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) { }

            public int X
            {
                get { return Left; }
                set { Right -= (Left - value); Left = value; }
            }

            public int Y
            {
                get { return Top; }
                set { Bottom -= (Top - value); Top = value; }
            }

            public int Height
            {
                get { return Bottom - Top; }
                set { Bottom = value + Top; }
            }

            public int Width
            {
                get { return Right - Left; }
                set { Right = value + Left; }
            }

            public System.Drawing.Point Location
            {
                get { return new System.Drawing.Point(Left, Top); }
                set { X = value.X; Y = value.Y; }
            }

            public System.Drawing.Size Size
            {
                get { return new System.Drawing.Size(Width, Height); }
                set { Width = value.Width; Height = value.Height; }
            }

            public static implicit operator System.Drawing.Rectangle(RECT r)
            {
                return new System.Drawing.Rectangle(r.Left, r.Top, r.Width, r.Height);
            }

            public static implicit operator RECT(System.Drawing.Rectangle r)
            {
                return new RECT(r);
            }

            public static bool operator ==(RECT r1, RECT r2)
            {
                return r1.Equals(r2);
            }

            public static bool operator !=(RECT r1, RECT r2)
            {
                return !r1.Equals(r2);
            }

            public bool Equals(RECT r)
            {
                return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
            }

            public override bool Equals(object obj)
            {
                if (obj is RECT)
                    return Equals((RECT)obj);
                else if (obj is System.Drawing.Rectangle)
                    return Equals(new RECT((System.Drawing.Rectangle)obj));
                return false;
            }

            public override int GetHashCode()
            {
                return ((System.Drawing.Rectangle)this).GetHashCode();
            }

            public override string ToString()
            {
                return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
            }
        }

        #endregion

    }
}
