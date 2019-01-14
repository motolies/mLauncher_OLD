using EventHook;
using MLauncher.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace MLauncher
{
    public partial class LauncherForm : Form
    {
        static string prevClick;
        static System.Threading.Timer listClearTimer;
        public static LiteDB DB = new LiteDB(Path.Combine(Application.StartupPath, "Settings.db"));
        public static LauncherStatus Status = new LauncherStatus();

        private bool _isDragging;
        private Point _clickPoint;


        public LauncherForm()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            DoubleBuffered = true;
            this.TopMost = Status.IsTopMost;
            InitializeComponent();
        }

        private void MouseHook()
        {
            MouseWatcher.Start();
            MouseWatcher.OnMouseInput += (s, e) =>
            {
                //Console.WriteLine(string.Format("Mouse event {0} at point {1},{2}", e.Message.ToString(), e.Point.x, e.Point.y));
                if (e.Message.ToString().Contains("BUTTONDOWN"))
                {
                    if (!string.IsNullOrWhiteSpace(prevClick) && prevClick != e.Message.ToString())
                    {
                        this.Invoke(new MethodInvoker(delegate
                        {
                            this.Location = new Point(e.Point.x, e.Point.y);
                            this.ShowLauncher();
                        }));
                    }
                    prevClick = e.Message.ToString();
                }
            };


        }


        private void LauncherForm_Load(object sender, EventArgs e)
        {
            //DB를 읽어서 셋팅 파일을 가져온다 
            //기본 셋팅값은 Status.IconSize와 같이 가지고 온다
            TabControl tabControl = new TabControl();
            // tabControl.Dock = DockStyle.Fill;
            tabControl.Name = "tabControl";
            this.Controls.Add(tabControl);

            foreach (DataRow tab in DB.ExecuteReader("SELECT * FROM TABS WHERE Enable = 1 ORDER BY ID").Rows)
            {
                //tabpage 를 만들어 넣고
                TabPage tb = new TabPage();
                tb.Name = tab["Name"].ToString();
                tb.Text = tab["Name"].ToString();
                tb.UseVisualStyleBackColor = true;
                tabControl.Controls.Add(tb);

                //tabpage에 button을 만들어 넣는다.
                for (int i = 0; i < Status.HorizonCount; i++) //가로 갯수
                {
                    for (int j = 0; j < Status.VerticalCount; j++) // 세로 갯수
                    {
                        string btnID = string.Format("btn_{0}_{1}_{2}", tab["ID"], i, j);
                        mButton b = new mButton();
                        b.Name = btnID;
                        b.Size = new Size(Status.IconSize, Status.IconSize);
                        b.Location = new Point(0 + (Status.IconSize - 1) * i, 0 + (Status.IconSize - 1) * j);
                        b.UseVisualStyleBackColor = true;
                        b.AllowDrop = true;
                        string btnPath = DB.ExecuteValue<string>(string.Format("SELECT Path FROM Buttons WHERE ID = '{0}'", btnID));
                        b.Title = Path.GetFileName(btnPath);
                        b.Image = IconManager.ToBitmap(btnPath);
                        b.Click += new EventHandler(Button_Click);
                        b.LabelClick += new EventHandler(Button_Click);
                        b.MouseHover += new EventHandler(Button_MouseHover);
                        b.DragEnter += new DragEventHandler(Button_DragEnter);
                        b.DragDrop += new DragEventHandler(Button_DragDrop);

                        b.MouseDown += new MouseEventHandler(Button_MouseDown);
                        b.MouseUp += new MouseEventHandler(Button_MouseUp);
                        b.MouseMove += new MouseEventHandler(Button_MouseMove);

                        b.ContextMenuStrip = this.contextMenuStrip;
                        tb.Controls.Add(b);
                    }
                }
            }
            int BorderWidth = SystemInformation.FrameBorderSize.Width;
            int TitlebarHeight = SystemInformation.CaptionHeight;

            if (tabControl.TabPages.Count > 0)
            {

                tabControl.TabPages[0].ClientSize = new Size(Status.IconSize * Status.HorizonCount, Status.IconSize * Status.VerticalCount);
                tabControl.ClientSize = new Size(Status.IconSize * Status.HorizonCount, Status.IconSize * Status.VerticalCount + TitlebarHeight);
                this.ClientSize = new Size(Status.IconSize * Status.HorizonCount, Status.IconSize * Status.VerticalCount + TitlebarHeight + statusStrip.Height);
            }

            //this.ClientSize = new Size(Status.IconSize * Status.HorizonCount + BorderWidth / 2, Status.IconSize * Status.VerticalCount + statusStrip.Height + TitlebarHeight - BorderWidth / 2);

            //스레드 타이머
            TimerCallback tc = new TimerCallback((o) =>
            {
                prevClick = string.Empty;
                this.Invoke(new MethodInvoker(delegate
                {
                    txtDatetime.Text = DateTime.Now.ToString("yyyy-MM-dd(ddd) tt hh:mm:ss");
                }));
            });
            listClearTimer = new System.Threading.Timer(tc, null, 0, 500);

            //마우스 후킹
            //MouseHook();
            KeyboardHook();


        }
        private void KeyboardHook()
        {
            KeyboardWatcher.Start();
            KeyboardWatcher.OnKeyInput += (s, e) =>
            {
                Console.WriteLine(string.Format("Key {0} event of key {1}", e.KeyData.EventType, e.KeyData.Keyname));
                if (e.KeyData.EventType == KeyEvent.down && e.KeyData.Keyname == "Delete")
                {
                    var btn = GetChildAtPoint(Cursor.Position);
                    if (btn != null)
                        Console.WriteLine(btn.Name);

                }
            };
        }

   


        private bool DraggingFromGrid = false;
        private System.Drawing.Point DraggingStartPoint = new System.Drawing.Point();
        private void Button_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Left && e.Clicks == 1)
            //    DoDragDrop(((mButton)sender), DragDropEffects.Copy);
            if (DraggingFromGrid)
            {
                if (System.Math.Abs(e.X - DraggingStartPoint.X) > 10 || System.Math.Abs(e.Y - DraggingStartPoint.Y) > 10)
                {
                    DraggingFromGrid = false;
                    DoDragDrop(((mButton)sender), DragDropEffects.Copy);
                }
            }
        }

        private void Button_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (DraggingFromGrid)
            {
                DraggingFromGrid = false;
            }
        }

        private void Button_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {

            //if (e.Button == MouseButtons.Left && e.Clicks == 1)
            //    DoDragDrop(((mButton)sender), DragDropEffects.Copy);
            if (e.Button == MouseButtons.Left)
            {
                DraggingFromGrid = true;
                DraggingStartPoint = new System.Drawing.Point(e.X, e.Y);
            }

        }

        private void Button_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(mButton)))
            {
                var origin = e.Data.GetData(typeof(mButton)) as mButton;
                var btn = sender as mButton;
                if (origin.Name != btn.Name)
                {
                    string oriPath = DB.ExecuteValue<string>(string.Format("SELECT Path FROM Buttons WHERE ID = '{0}'", origin.Name));

                    string path = oriPath;
                    if (!File.Exists(path) && !Directory.Exists(path))
                        return;

                    if (PathManager.GetType(oriPath) == PathManager.PathType.Shotcut)
                        path = PathManager.AbsolutePath(oriPath);

                    InsertButton(btn, path);
                    DeleteButton(origin);
                }
            }
            else
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    string path = file;
                    if (!File.Exists(path) && !Directory.Exists(path))
                        return;

                    if (PathManager.GetType(file) == PathManager.PathType.Shotcut)
                        path = PathManager.AbsolutePath(file);

                    var btn = sender as mButton;

                    InsertButton(btn, path);
                }
            }
        }
        private void Button_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
            else if (e.Data.GetDataPresent(typeof(mButton))) e.Effect = DragDropEffects.Copy;

        }




        private void Button_MouseHover(object sender, EventArgs e)
        {
            try
            {
                string btnPath = DB.ExecuteValue<string>(string.Format("SELECT Path FROM Buttons WHERE ID = '{0}'", ((mButton)sender).Name));
                //txtPath.Text = Path.GetFileName(btnPath);
                txtPath.Text = btnPath;
                toolTip.SetToolTip((Control)sender, Path.GetFileName(btnPath));
            }
            catch { }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            try
            {
                string btnPath = DB.ExecuteValue<string>(string.Format("SELECT Path FROM Buttons WHERE ID = '{0}'", ((mButton)sender).Name));
                Process.Start(btnPath);
            }
            catch { }
        }

        private void LauncherForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }


        private void ContextNotify_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("종료하시겠습니까?", "종료", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //Process.GetCurrentProcess().Kill();
                Application.ExitThread();
                Environment.Exit(0);
            }
        }

        private bool InsertButton(mButton btn, string path)
        {

            //if (string.IsNullOrWhiteSpace(DB.ExecuteValue<string>(string.Format("select ID from Buttons where ID = '{0}'", btn.Name))))
            //{
            //    //insert
            //    if (DB.ExecuteNonQuery(string.Format("insert into Buttons Values('{0}', '{1}')", btn.Name, path)) < 1)
            //        return false;
            //}
            //else
            //{
            //    //update
            //    if (DB.ExecuteNonQuery(string.Format("update Buttons set Path = '{0}' Where ID = '{1}'", path, btn.Name)) < 1)
            //        return false;
            //}

            if (DB.ExecuteNonQuery(string.Format("insert or replace into Buttons Values('{0}', '{1}')", btn.Name, path)) < 1)
                return false;

            btn.Image = IconManager.ToBitmap(path);
            btn.Title = Path.GetFileName(path);
            return true;
        }
        private bool DeleteButton(mButton btn)
        {
            if (DB.ExecuteNonQuery(string.Format("Delete From Buttons Where ID = '{0}'", btn.Name)) < 1)
                return false;

            btn.Image = null;
            btn.Title = string.Empty;
            return true;
        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            this.ShowLauncher();
        }

        private void tMenu_Click(object sender, EventArgs e)
        {
            var obj = sender as ToolStripMenuItem;
            switch (obj.Name)
            {
                case "tMenu_Setting":
                    SettingForm f = new SettingForm();
                    f.StartPosition = FormStartPosition.CenterParent;
                    f.ShowDialog();
                    break;
                case "tMenu_Delete":
                    DeleteButton((mButton)contextMenuStrip.SourceControl);
                    break;
                case "tMenu_OpenPath":
                    string btnPath = DB.ExecuteValue<string>(string.Format("SELECT Path FROM Buttons WHERE ID = '{0}'", contextMenuStrip.SourceControl.Name));
                    if (!string.IsNullOrWhiteSpace(btnPath))
                        Process.Start("explorer.exe", Path.GetDirectoryName(btnPath));
                    break;
            }
        }



        private void LauncherForm_Activated(object sender, EventArgs e)
        {
            Console.WriteLine("LauncherForm_Activated");

        }
    }
}
