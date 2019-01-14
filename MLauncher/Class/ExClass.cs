using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MLauncher.Class
{
    public static class ExClass
    {
        public static void ShowLauncher(this Form frm)
        {
            frm.TopMost = true;
            frm.Show();
            if (!LauncherForm.Status.IsTopMost)
            {
                frm.TopMost = false;
            }
        }
    }
}
