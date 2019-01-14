using IWshRuntimeLibrary;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MLauncher
{
    public partial class SettingForm : Form
    {
        public SettingForm()
        {
            InitializeComponent();
        }
        protected List<Tab> list = new List<Tab>();
        private void SettingForm_Load(object sender, EventArgs e)
        {
            txtPwd.Text = LauncherForm.Status.Pwd;
            txtHorizonCount.Value = LauncherForm.Status.HorizonCount;
            txtVerticalCount.Value = LauncherForm.Status.VerticalCount;
            txtIsTopMost.Text = LauncherForm.Status.IsTopMost.ToString();
            txtStartMenu.Text = Startup.ExistsProgram().ToString();

            foreach (DataRow r in LauncherForm.DB.ExecuteReader("select * from Tabs order by ID").Rows)
            {
                list.Add(new Tab { ID = (int)r["ID"], Name = r["Name"].ToString(), Enable = r["Enable"].ToString() == "1" ? true : false });
            }
            DataGridViewInit();
        }



        private void DataGridViewInit()
        {
           

            dataGridView.DataSource = typeof(List<Tab>);
            dataGridView.DataSource = list;
            //init
            dataGridView.Columns[0].ReadOnly = true;
            dataGridView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView.Columns[0].Width = 30;

            dataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView.Columns[1].DefaultCellStyle.Padding = new Padding(5, 0, 0, 0); 

            dataGridView.Columns[2].Width = 60;


            //init
            dataGridView.DefaultCellStyle.SelectionBackColor = Color.Red; // 선택색상
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // row 로 선택하기
            dataGridView.RowHeadersVisible = false; // 왼쪽 화살표 안보이게
            dataGridView.AllowUserToAddRows = false; //맨 아래 로그 가리기
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None; //row size 막기
            dataGridView.AllowUserToResizeRows = false; //row size 막기
            dataGridView.AllowUserToResizeColumns = false; //column size 막기
            dataGridView.MultiSelect = false;
            dataGridView.Columns.Cast<DataGridViewColumn>().ToList().ForEach(f =>
            {
                f.SortMode = DataGridViewColumnSortMode.NotSortable; // sort 막기
                f.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter; //헤더텍스트 센터 정렬
                f.HeaderCell.Style.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Pixel); // 폰트 사이트 및 크기 조정
            });
            //even Row에 대하서 색상 칠하기
            dataGridView.Rows.Cast<DataGridViewRow>().Where((x, i) => i % 2 != 0).ToList().ForEach(r => r.DefaultCellStyle.BackColor = Color.AliceBlue);


        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("저장 하시겠습니까?", "저장", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                LauncherForm.Status.Pwd = txtPwd.Text.Trim();
                LauncherForm.Status.HorizonCount = (int)txtHorizonCount.Value;
                LauncherForm.Status.VerticalCount = (int)txtVerticalCount.Value;
                LauncherForm.Status.IsTopMost = Convert.ToBoolean(txtIsTopMost.Text);

                if (Convert.ToBoolean(txtStartMenu.Text))
                    Startup.AddProgram();
                else
                    Startup.DelProgram();

                //탭에 대한 내용 저장
                LauncherForm.DB.ExecuteNonQuery("delete from Tabs");
                foreach (Tab t in list)
                {
                    string q = string.Format("insert into Tabs(ID, Name, Enable) values({0}, '{1}', {2})", t.ID, t.Name, t.Enable ? 1 : 0);
                    LauncherForm.DB.ExecuteNonQuery(q);
                }

                this.Hide();
                Application.Restart();
                //Process.GetCurrentProcess().Kill();
                Application.ExitThread();
                Environment.Exit(0);
            }
        }

        private void btnADD_Click(object sender, EventArgs e)
        {
            //var obj = list.Max(o => o.ID);
            list.Add(new Tab { ID = list.Max(o => o.ID) + 1, Name = "", Enable = false });
            DataGridViewInit();
        }

        private void btnDELETE_Click(object sender, EventArgs e)
        {
            /*
             * buttons 테이블에 값이 있는지 보고 있으면 물어보고,
             * 없으면 그냥 삭제한다
             */

            int tabID = (int)dataGridView.SelectedRows[0].Cells[0].Value;

            if (LauncherForm.DB.ExecuteValue<int>(string.Format("select count(*) from Buttons where ID like 'btn_{0}%'", tabID)) > 0)
            {
                if (MessageBox.Show("해당 탭에는 내용이 있습니다.\r\n정말 삭제하시겠습니까?", "삭제", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return;
                }
            }

            list.RemoveAll(o => o.ID == tabID);
            DataGridViewInit();

        }
    }
    public class Tab
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool Enable { get; set; }
    }

    public static class Startup
    {
        static string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        static string shortcutAddress = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), string.Format("{0}.lnk", appName));
        static string exeAddress = Path.Combine(Application.StartupPath, string.Format("{0}.exe", appName));
        internal static void AddProgram()
        {
            //바로가기 등록
            object shDesktop = (object)"Desktop";
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
            shortcut.TargetPath = exeAddress;
            shortcut.Save();
        }
        internal static void DelProgram()
        {
            if (ExistsProgram())
            {
                System.IO.File.Delete(shortcutAddress);
            }
        }
        internal static bool ExistsProgram()
        {
            return System.IO.File.Exists(shortcutAddress);
        }
    }
}
