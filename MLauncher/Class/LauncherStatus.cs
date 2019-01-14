using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MLauncher.Class
{
    public class LauncherStatus
    {
        public string Pwd
        {
            get { return LauncherForm.DB.ExecuteValue<string>("SELECT Pwd FROM Settings"); }
            set { LauncherForm.DB.ExecuteNonQuery(string.Format("UPDATE Settings set Pwd='{0}'", value)); }
        }
        public bool IsTopMost
        {
            get { return LauncherForm.DB.ExecuteValue<int>("SELECT IsTopMost FROM Settings") == 0 ? false : true; }
            set { LauncherForm.DB.ExecuteNonQuery(string.Format("UPDATE Settings SET IsTopMost={0}", value ? 1 : 0)); }
        }
        public int IconSize
        {
            get { return 50; }
        }
        public int HorizonCount
        {
            get { return LauncherForm.DB.ExecuteValue<int>("SELECT HorizonCount FROM Settings"); }
            set { LauncherForm.DB.ExecuteNonQuery(string.Format("UPDATE Settings SET HorizonCount={0}", value)); }
        }
        public int VerticalCount
        {
            get { return LauncherForm.DB.ExecuteValue<int>("SELECT VerticalCount FROM Settings"); }
            set { LauncherForm.DB.ExecuteNonQuery(string.Format("UPDATE Settings SET VerticalCount={0}", value)); }
        }
        //public bool StartMenu
        //{
        //    get { return LauncherForm.DB.ExecuteValue<Int64>("SELECT StartMenu FROM Settings") == 0 ? false : true; }
        //    set { LauncherForm.DB.ExecuteNonQuery(string.Format("UPDATE Settings SET StartMenu={0}", value ? 1 : 0)); }
        //}
    }

}
