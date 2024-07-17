using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CustomerList
{
    public class DB_Conn
    {
        //커넥션 객체

        private static SqlConnection conn = null;

        public static string DBConnString { get; private set; }
        public static bool bDBConnCheck = false;
        private static int errorBoxCount = 0;

        /// <summary>
        /// 생성자
        /// </summary>
        public DB_Conn()
        { }

        public static SqlConnection DBConn
        {
            get
            {
                if (!ConnectToDB())
                {
                    return null;
                }
                return conn;
            }
        }

        public static bool ConnectToDB()
        {
            if (conn == null)
            {
                //서버명, 초기 DB명, 인증 방법
                conn = new SqlConnection();           
                conn.ConnectionString = @"Server=222.231.58.71; database=SMS; uid=eshinan; pwd=!eshinan4600";
                //DBConnString = String.Format("Data Source=({0});Initial Catalog={1};Integrated Security={2}; Timeout=3", "local", "BOEMBOEMJOJO", "SSPI");
            }
            try
            {
                if (!IsDBConnected)
                {
                    conn.ConnectionString = @"Server=222.231.58.71; database=SMS; uid=eshinan; pwd=!eshinan4600";
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        bDBConnCheck = true;
                    }
                    else
                    {
                        bDBConnCheck = false;
                    }                   
                }
            }
            catch (SqlException e)
            {
                errorBoxCount++;
                if (errorBoxCount == 1)
                {
                    MessageBox.Show(e.Message, "DB_Conn - ConnectToDB()");
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// Database Open 여부 확인
        /// </summary>
        public static bool IsDBConnected
        {
            get
            {
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Database 해제
        /// </summary>
        public static void Close()
        {
            if (IsDBConnected)
                DBConn.Close();
        }
    }
}