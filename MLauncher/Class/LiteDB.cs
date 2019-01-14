using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MLauncher.Class
{
    //일단은 상주형 프로그램이기 때문에 커넥션은 닫지 않을 생각임
    public class LiteDB
    {
        static SQLiteConnection mConn;
        static string DBPath = "Settings.db";
        public LiteDB(string dbpath = null)
        {
            if (!string.IsNullOrWhiteSpace(dbpath))
            {
                DBPath = dbpath;
            }

            // db 파일이 있는지 검사
            if (!System.IO.File.Exists(DBPath))
            {
                SQLiteConnection.CreateFile(DBPath);  // SQLite DB 생성
            }

            string ConnectionString = string.Format("Data Source={0};Version=3;", DBPath);

            mConn = new SQLiteConnection(ConnectionString);
            mConn.Open();

            // 테이블 조회 후 테이블이 없으면 테이블을 만들고 기본값을 넣는다
            DataTable dt = ExecuteReader("SELECT name FROM sqlite_master WHERE type = 'table';");
            if (dt.Rows.Count < 1)
                CreateTable();
        }


        public DataTable ExecuteReader(string sql)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, mConn))
                {
                    using (SQLiteDataReader rdr = cmd.ExecuteReader())
                    {
                        dt.Load(rdr);
                        return dt;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public int ExecuteNonQuery(string sql)
        {
            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(sql, mConn))
                {
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
        }
        public T ExecuteValue<T>(string sql)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, mConn))
                {
                    using (SQLiteDataReader rdr = cmd.ExecuteReader())
                    {
                        dt.Load(rdr);
                        if (dt.Rows.Count == 1)
                            return (T)dt.Rows[0][0];
                        else
                            throw new Exception("리턴받은 로우의 갯수가 하나가 아닙니다. Where 조건을 확인해주세요.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return default(T);
            }
        }
        private void CreateTable()
        {
            string CreateQuery =
                @"
                CREATE TABLE IF NOT EXISTS Buttons 
                (
                    ID	TEXT PRIMARY KEY NOT NULL,
                    Path	TEXT
                );

                CREATE TABLE IF NOT EXISTS Tabs 
                (
                    ID	INT primary key,
                    Name TEXT, 
                    Enable	INT Default 1
                );
                INSERT INTO Tabs(ID, Name)
                VALUES(0, 'Program');
                INSERT INTO Tabs(ID, Name)
                VALUES(1, 'Folder');

                CREATE TABLE IF NOT EXISTS Settings 
                (
                    Pwd TEXT, 
                    IsTopMost	INT,
                    IconSize	INT,
                    HorizonCount	INT,
                    VerticalCount	INT,
                    StartMenu	INT
                );
                INSERT INTO Settings
                VALUES('1234', 1, 50, 10, 4, 0);
                ";

            ExecuteNonQuery(CreateQuery);

        }

    }
}
