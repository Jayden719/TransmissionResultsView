
using CustomerList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace TransmissionResultsView
{
    internal class Sqls
    {
       
        // n = 1, 71 sms
        // n = 2, 71 mms
        // n = 3, 75 sms
        // n = 4, 75 mms
        // n = 5, 75 fax
        // n = 6, 75 aspfax
        // n = 7, 75 alarm
        //List<string> task_tables = new List<string> { };
        //List<string> task_Split_Tables = new List<string> { };
        //List<string> sqls_returned = new List<string> { };
        

        public List<string> Get_tableNames(int n)
        {
            
            string query_71sms = "SELECT '[106].Data_BAK_Old_71.dbo.'+TABLE_NAME FROM [106].Data_BAK_Old_71.INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME LIKE 'SM_SC%'\r\nunion all\r\nSELECT 'Data_BAK.dbo.'+TABLE_NAME FROM Data_BAK.INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME LIKE 'SM_SC%'\r\nunion all\r\nselect * from\r\n(SELECT top 2 TABLE_NAME as tablename FROM SMS.INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME LIKE 'SM_SC%' and RIGHT(TABLE_NAME, 3) != 'bak' ORDER BY TABLE_NAME DESC)a";
            string query_71mms = "SELECT '[106].Data_BAK_Old_71.dbo.'+TABLE_NAME FROM [106].Data_BAK_Old_71.INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME LIKE 'SM_MMS%'\r\nunion all\r\nSELECT 'Data_BAK.dbo.'+TABLE_NAME FROM Data_BAK.INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME LIKE 'SM_MMS%'\r\nunion all\r\nselect * from\r\n(SELECT top 2 TABLE_NAME as tablename FROM SMS.INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME LIKE 'SM_MMS%' and RIGHT(TABLE_NAME, 3) != 'bak' ORDER BY TABLE_NAME DESC)a";
            string query_75sms = "SELECT '[106].Data_BAK_Old_75.dbo.'+TABLE_NAME FROM [106].Data_BAK_Old_75.INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME LIKE 'SC%'\r\nunion all\r\nSELECT '[75].Data_BAK.dbo.'+TABLE_NAME FROM [75].Data_BAK.INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME LIKE 'SC%'\r\nunion all\r\nselect * from\r\n(select top 2 '[75].ASP.dbo.'+TABLE_NAME as tablename FROM [75].ASP.INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME LIKE 'SC%' ORDER BY TABLE_NAME DESC)a\r\n";
            string query_75mms = "SELECT '[106].Data_BAK_Old_75.dbo.'+TABLE_NAME FROM [106].Data_BAK_Old_75.INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME LIKE 'MMS%'\r\nunion all\r\nSELECT '[75].Data_BAK.dbo.'+TABLE_NAME FROM [75].Data_BAK.INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME LIKE 'MMS%'\r\nunion all\r\nselect * from\r\n(select top 2 '[75].ASP.dbo.'+TABLE_NAME as tablename FROM [75].ASP.INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME LIKE 'MMS%' ORDER BY TABLE_NAME DESC)a";

            string query_moa_Fax = "SELECT '[106].Data_BAK_Old_75.dbo.'+TABLE_NAME FROM [106].Data_BAK_Old_75.INFORMATION_SCHEMA.TABLES WHERE LEN(TABLE_NAME) = 25 and TABLE_NAME Like 'Task%'\r\nunion all\r\nselect * from\r\n(SELECT top 10 '[75].Data_BAK.dbo.'+TABLE_NAME as tablename FROM [75].Data_BAK.INFORMATION_SCHEMA.TABLES WHERE (LEN(TABLE_NAME) = 25 and TABLE_NAME Like 'Task%')  order by TABLE_NAME)a\r\nunion all\r\nselect * from\r\n(select  '[75].FAX.dbo.'+TABLE_NAME as tablename FROM [75].FAX.INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME Like 'Task%' and (LEN(TABLE_NAME) in (18, 25) or LEN(TABLE_NAME)=10))a";
            string query_asp_Fax = "SELECT '[106].Data_BAK_Old_75.dbo.'+TABLE_NAME FROM [106].Data_BAK_Old_75.INFORMATION_SCHEMA.TABLES WHERE LEN(TABLE_NAME) = 28 and TABLE_NAME Like 'Task%'\r\nunion all\r\nselect * from\r\n(SELECT top 10 '[75].Data_BAK.dbo.'+TABLE_NAME as tablename FROM [75].Data_BAK.INFORMATION_SCHEMA.TABLES WHERE (LEN(TABLE_NAME) = 28 and TABLE_NAME Like 'Task%')  order by TABLE_NAME)a\r\nunion all\r\nselect * from\r\n(select  '[75].ASP.dbo.'+TABLE_NAME as tablename FROM [75].ASP.INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME LIKE 'Task%' and (LEN(TABLE_NAME) in (18, 28) or LEN(TABLE_NAME)=10))a";
            string query_alarmtalk = "SELECT '[106].Data_BAK_Old_75.dbo.'+TABLE_NAME FROM [106].Data_BAK_Old_75.INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME Like 'Biz%'\r\nunion all\r\nselect * from\r\n(SELECT top 10 '[75].Data_BAK.dbo.'+TABLE_NAME as tablename FROM [75].Data_BAK.INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME Like 'Biz%' order by TABLE_NAME)a\r\nunion all\r\nselect * from\r\n(select top 2 '[75].ASP.dbo.'+TABLE_NAME as tablename FROM [75].ASP.INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME Like 'Biz%' ORDER BY TABLE_NAME DESC)a";
            if (n == 1)
            {
                Console.WriteLine("query_71sms : " + query_71sms);
                return  SqlToStringList(query_71sms);
            }
            else if(n == 2)
            {
                Console.WriteLine("query_71mms : " + query_71mms);
                return  SqlToStringList(query_71mms);
            }
            else if (n == 3)
            {
                return  SqlToStringList(query_75sms);
            }
            else if (n == 4)
            {
                return  SqlToStringList(query_75mms);
            }
            else if (n == 5)
            {
            
                return  SqlToStringList(query_moa_Fax);
            }
            else if (n == 6)
            {
                return  SqlToStringList(query_asp_Fax);
            }
            else if (n == 7)
            {
                return  SqlToStringList(query_alarmtalk);
            }
            else { return null;  }

        }

        //public List<DateTime> Get_tableDateTime(List<string> tableNames)
        //{

        //    DateTime curr = DateTime.Now;
        //    List<DateTime> dateTimes = new List<DateTime> { };
        //    if (tableNames.Count == 1)
        //    {
        //        // 빌링 주간이 아니고 현재 테이블이 현재달을 조회할 수 있을 때
        //        // 근데 극 월초일때는 전달부터 현재달초까지 조회
                
        //        DateTime time = new DateTime(curr.Year, curr.Month, 01);
        //        dateTimes.Add(time);

        //    }
        //    else if (tableNames.Count == 2)
        //    {
        //        // 빌링 주간에 이전 달 데이터가 아직 데이터백으로 넘어가지 않았을 때 요기
        //        // 현재달 전달 데이터
        //    }
        //    else
        //    {
        //        // 데이터백, 데이터백 올드는 여기로 들어옴
        //    }

     
            
        //    foreach (string table_name in tableNames)
        //    {
        //        //202301
        //        string sub_right6 = table_name.Substring(table_name.Length - 6);
        //        if (sub_right6 == "istory")
        //        {

        //        }
        //        else
        //        {
        //            DateTime date = new DateTime(int.Parse(sub_right6.Substring(0, 4)), int.Parse(sub_right6.Substring(4)), 01);
        //            dateTimes.Add(date);
        //        }
        //    }
        //    return null;
        //}

        public List<string> SqlToStringList(string sql)
        {   
            DataTable dt =  SqlToDt(sql);
            if (dt != null)
            {
                List<string> strList;
                strList = dt.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToList();
                return strList;
            }
            else
            {
                return null;
            }                
        }
      
        public DataTable SqlToDt(string sql)
        {       
                lock (DB_Conn.DBConn)
                {
                    if (!DB_Conn.IsDBConnected)
                    {
                        MessageBox.Show("Database 연결을 확인하세요.");
                        return null;
                    }
                    //DB 연결이 되고 난 후
                    SqlDataAdapter adapter = new SqlDataAdapter();
                
                    try
                    {
                        DataTable dt = new DataTable();
                        adapter = new SqlDataAdapter(sql, DB_Conn.DBConn);                  
                        adapter.Fill(dt);
                        return dt;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return null;
                    }
                }
            //DB_Conn.Close();
        

            //for (int i = 0; i < dt.Columns.Count; i++)
            //{
            //    dt.Columns[i].ColumnName = column_dummy.Columns[i].ColumnName;
            //}
            //return null;
        }
        public async Task<DataTable> SqlsToMergedDt(string[] sqls, DataTable column_dummy = null)
        {
/*            Console.WriteLine(sqls.Length);
            foreach(string s in sqls)
            {
                Console.WriteLine("sqls : " + s);

            }*/
            string union = String.Join("; ", sqls);
          
            DataTable ds = await SqlsToDs(union);           
           
            return ds;
           /* for (int i = 0; i < ds.Tables.Count; i++)
            {             
                Console.WriteLine("Task_05");
                dt.Merge(ds.Tables[i]);
                Console.WriteLine("Task_06");                
            }     */       

            //lock (DB_Conn.DBConn)
            //{
            //    if (!DB_Conn.IsDBConnected)
            //    {
            //        MessageBox.Show("Database 연결을 확인하세요.");
            //        return dt;
            //    }
            //    //DB 연결이 되고 난 후..
            //    SqlDataAdapter[] adapter = new SqlDataAdapter[sqls.Length];
            //    try
            //    {
                    
            //        for (int i = 0; i < sqls.Length; i++)
            //        {
            //            DataTable base_dt = new DataTable();
            //            adapter[i].SelectCommand.CommandTimeout = 300;
            //            adapter[i] = new SqlDataAdapter(sqls[i], DB_Conn.DBConn);
            //            adapter[i].Fill(base_dt);
            //            dt.Merge(base_dt);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }
            //    DB_Conn.Close();
            //}
            //if (column_dummy != null)
            //{
            //    for (int i = 0; i < dt.Columns.Count; i++)
            //    {
            //        dt.Columns[i].ColumnName = column_dummy.Columns[i].ColumnName;
            //    }
            //}
            //return dt;
        }
      

        public async Task<DataTable> SqlsToDs(string sqls_SplitedBySemiColone)
        {          
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
       
            await Task.Run(() =>
            {
                using (DB_Conn.DBConn)
                {
                    if (!DB_Conn.IsDBConnected)
                    {
                        MessageBox.Show("Database 연결을 확인하세요.");
                        return;
                        //return ds;
                    }
                //DB 연결이 되고 난 후..
                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(sqls_SplitedBySemiColone, DB_Conn.DBConn);
                    adapter.SelectCommand.CommandTimeout = 300;

                    adapter.Fill(ds);
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        dt.Merge(ds.Tables[i]);
                    }
                 
  /*                  frm.dgv_message.DataSource = dt;
                    frm.dgv_message.RowHeadersWidth = 70;

                    frm.setRowNumber(frm.dgv_message);
                    frm.dgv_message.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
       
                    int cnt_dgv = frm.dgv_message.Rows.Count;
                    if (cnt_dgv == 0)
                    {
                        frm.lbl_cnt.Text = cnt_dgv.ToString() + "건이 조회되었습니다.";
                    }
                    else
                    {
                        frm.lbl_cnt.Text = cnt_dgv.ToString() + "건이 조회되었습니다.";
       
                    }
                    Cursor.Current = Cursors.Default;*/
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                }
            });               
             DB_Conn.Close();
             return dt;
        }


        public string[] SQLs_MessageResult(string userid, List<string> tableNames, DateTime start_month, DateTime end_month, bool isSMSorAlarmtalk, bool isFaxTask, bool? isDotcom = null)
        {          
            List<DateTime> tableDatetimes = new List<DateTime> { };
            
            DateTime curr = DateTime.Now;
            DateTime datetime;
            foreach (string tableName in tableNames)
            {
                string year_month = tableName.Substring(tableName.Length - 6); // 202303
                if (year_month != "istory" && year_month != "FaxLog" && year_month != "JobLog" && year_month != "C_TRAN" && year_month != "MS_MSG")
                {
                    datetime = new DateTime(int.Parse(year_month.Substring(0, 4)), int.Parse(year_month.Substring(4, 2)), 01);
                }
                else
                {
                    datetime = new DateTime(curr.Year, curr.Month, 01);
                    // 월 극초반일때(3일이전 - 빌링 전)                
                 /*   if (tableDatetimes.Count > 0 && tableDatetimes[tableDatetimes.Count - 1] == datetime.AddMonths(-2))
                    {
                        datetime = datetime.AddMonths(-1);
                    }*/
                }
                tableDatetimes.Add(datetime);
            }
            
            List<string> sqls_returned = new List<string> { };
            List<string> sqls_job_returned = new List<string> { };
           
          
            for (int i =0; i< tableDatetimes.Count; i++)
            {
                /*
                var st = new DateTime(start_month.Year, start_month.Month, 01);
                if (tableDatetimes[i] < st || tableDatetimes[i] > end_month)
                {           
                    continue;                    
                }    
                */          
                string sql_sms;                
                string sql_mms;                
                string sql_faxtask;
                string sql_alarmtalk;
                string sql_smsJob;
                string sql_mmsJob;
                string sql_alarmJob;

                string tablename = tableNames[i];
                string start_date = tableDatetimes[i].ToString("yyyy-MM-01");
                string start_date_alarm = tableDatetimes[i].ToString("yyyyMM");
                string end_date;
                TimeSpan dateDiff = DateTime.Today - end_month;

                if (!tablename.EndsWith("tory"))
                {
                    end_date = tableDatetimes[i].AddMonths(1).ToString("yyyy-MM-01");
                }
                else
                {
                    end_date = DateTime.Now.AddMonths(1).ToString("yyyy-MM-01");      
                }


                if(!isFaxTask) //문자임
                {
                    if (isSMSorAlarmtalk) // 문자, sms
                    {
                        
                        // 아래 where sJobID in ~~~ 어쩌구 주석을 주석해제하고 바로 위 where 문을 주석처리하면
                        //  -> 검색속도가 더 빨라지지만 joblog에서 지워진 데이터는 검색이 안됨 (2023.05 기준 2023.01 이전 데이터는 잡로그에 없음.)

                        if ((bool)isDotcom) // 문자, sms, 닷컴임
                        {
                            sql_sms = String.Format(
                            "select 구분, JobID, UserID, 수신번호, 발신번호, 전송시간, 완료시간, 전송결과, 제목, 내용 from ( " +
                            "select distinct tr_num, tr_modified, 'SMS' as 구분, TR_ETC1 as JobID, sUserID AS UserID,TR_PHONE as 수신번호,TR_CALLBACK as '발신번호',CONVERT(CHAR(19), TR_SENDDATE, 120) as '전송시간',CONVERT(CHAR(19), TR_RSLTDATE, 120) as 완료시간,\r\n\t\t" +
                            "(select sname from icomfax.dbo.ccode with (nolock) where sgroup='LG' AND SCODE=TR_RSLTSTAT) as 전송결과,'' as 제목,replace(replace(TR_MSG, char(13), ''), char(10), '') as 내용 \r\n\t\t" +
                            "from {0} with(nolock) " +
                            "where sUserID = '{2}' and TR_SENDDATE >= '{3}' and TR_SENDDATE < '{4}')a  \r\n\t\t" ,
                            //+
                            //"where TR_ETC1 in (select sJobID FROM JobLog with (nolock) where sUserID = '{2}' and dtStartTime >= '{3}' and dtStartTime < '{4}' and nSvcType = 3)\r\n\t\t" +
                            //"union all\r\n\t\t" +
                            //"select 'SMS' as 구분, sJobID as JobID, sUserID AS UserID, fdestine AS 수신번호, fcallback AS 발신번호,CONVERT(CHAR(19), fsenddate, 120) AS 전송시간,CONVERT(CHAR(19), frsltdate, 120) AS 완료시간,\r\n\t\t" +
                            //"(select top 1 sName from iComFax.dbo.CCode(nolock) where sCode = frsltstat and sGroup = 'LGHV') as 전송결과, \r\n\t\t" +
                            //"fsubject as 제목,replace(replace(fmessage, char(13), ''), char(10), '') as 내용  from {1} with (nolock)\r\n\t\t" +
                            //"where sUserID = '{2}' and fsenddate >= '{3}' and fsenddate < '{4}'",
                            //"where sJobID in (select sJobID FROM JobLog with (nolock) where sUserID = '{2}' and dtStartTime >= '{3}' and dtStartTime < '{4}' and nSvcType = 3)",
                            tablename, tablename.Replace("SM", "TBL"), userid, start_month.ToString().Substring(0,10), end_month.AddDays(1).ToString().Substring(0, 10));
                            
                           if(dateDiff.Days < 5)
                            {
                                sql_smsJob = String.Format(
                                      "select distinct(sSplitSrv) from JobLog with(nolock) where suserid = '{0}' and dtRecvTime >= '{1}' and dtRecvTime < '{2}' and left(sjobid,2)='JS' \r\n\t\t" +
                                      "union \r\n\t\t" +
                                      "select distinct(sSplitSrv) from JobsmsLog with(nolock) where suserid = '{0}' and dtRecvTime >= '{1}' and dtRecvTime < '{2}' and left(sjobid,2)='JS'"
                                      ,
                                      userid, end_month.AddDays(-3).ToString().Substring(0, 10), end_month.AddDays(1).ToString().Substring(0, 10));

                                sqls_job_returned.Add(sql_smsJob);
                            }                        
                        }
                        else // 문자, sms, 비즈임
                        {
                            sql_sms = String.Format(
                            "select 구분, JobID, UserID, 수신번호, 발신번호, 전송시간, 완료시간, 전송결과, 제목, 내용 from ( " +
                            "select distinct tr_num, tr_modified, 'SMS' as 구분, TR_ETC1 as JobID, sUserID AS UserID,TR_PHONE as 수신번호,TR_CALLBACK as '발신번호',CONVERT(CHAR(19), TR_SENDDATE, 120) as '전송시간',CONVERT(CHAR(19), TR_RSLTDATE, 120) as 완료시간,\r\n\t\t" +
                            "(select sname from icomfax.dbo.ccode with (nolock) where sgroup='LG' AND SCODE=TR_RSLTSTAT) as 전송결과,'' as 제목,replace(replace(TR_MSG, char(13), ''), char(10), '') as 내용 \r\n\t\t" +
                            "from {0} with(nolock) " +
                            "where sUserID = '{1}' and TR_SENDDATE >= '{2}' and TR_SENDDATE < '{3}')a",
                            //"where TR_ETC1 in (select sJobID FROM [75].ASP.dbo.JobLog with (nolock) where sUserID = '{1}' and dtStartTime >= '{2}' and dtStartTime < '{3}' and nSvcType = 3) \r\n\t\t",
                            tablename, userid, start_month.ToString().Substring(0, 10), end_month.AddDays(1).ToString().Substring(0, 10)
                            );

                            if (dateDiff.Days < 5)
                            {
                                sql_smsJob = String.Format(
                                 "select distinct(sSrvName) as sSplitSrv from [75].ASP.dbo.JobLog with(nolock) where suserid = '{0}' and left(sjobid,2)='JS' and dtRecvTime >= '{1}' and dtRecvTime < '{2}' \r\n\t\t" +
                                 "union \r\n\t\t" +
                                 "select distinct(sSrvName) as sSplitSrv from [75].ASP.dbo.JobsmsLog with(nolock) where suserid = '{0}' and left(sjobid,2)='JB' and dtRecvTime >= '{1}' and dtRecvTime < '{2}'"
                                 ,
                                 userid, end_month.AddDays(-3).ToString().Substring(0, 10), end_month.AddDays(1).ToString().Substring(0, 10));
                                sqls_job_returned.Add(sql_smsJob);

                            }
                        }
                        
                            sqls_returned.Add(sql_sms);
                            
                            //SQL_JobSearch(sqls_job_returned.ToArray(), userid, start_month, end_month, (bool)isDotcom);


                    }
                    else // 문자, mms
                    {
                        if ((bool)isDotcom) // 문자, mms, 닷컴
                        {
                            sql_mms = String.Format(
                            "select 구분, JobID, UserID, 수신번호, 발신번호, 전송시간, 완료시간, 전송결과, 제목, 내용 from ( " +
                            "select distinct  msgkey, reportdate, 'MMS' AS 구분, ETC1 as JobID, sUserID AS UserID, PHONE AS 수신번호, CALLBACK AS 발신번호,CONVERT(CHAR(19), REQDATE, 120) AS 전송시간, CONVERT(CHAR(19), RSLTDATE, 120) AS 완료시간,\r\n\t\t" +
                            "case when rslt='1000' and id='-1' then '전송시간 초과' else (select top 1 sName from iComFax.dbo.CCode(nolock) where sCode = RSLT and sGroup = 'LG') end as 전송결과, \r\n\t\t" +
                            "replace(replace(SUBJECT, char(13), ''), char(10), '') as 제목, replace(replace(MSG, char(13), ''), char(10), '') as 내용 from {0} with (nolock)\r\n\t\t" +
                            "where sUserID = '{2}' and REQDATE >= '{3}' and REQDATE < '{4}')a \r\n\t\t", 
                            //+
                            //"where etc1 in (select sJobID FROM JobLog where sUserID = '{2}' and dtStartTime >= '{3}' and dtStartTime < '{4}' and nSvcType in (5,6))\r\n\t\t" +
                            //"union all\r\n\t\t" +
                            //"select 'MMS' AS 구분, sJobID as JobID, sUserID AS UserID, fdestine AS 수신번호, fcallback AS 발신번호,CONVERT(CHAR(19), fsenddate, 120) AS 전송시간, CONVERT(CHAR(19), frsltdate, 120) AS 완료시간,\r\n\t\t" +
                            //"(select top 1 sName from iComFax.dbo.CCode(nolock) where sCode = frsltstat and sGroup = 'LGHV') as 전송결과, \r\n\t\t" +
                            //"replace(replace(fsubject, char(13), ''), char(10), '') as 제목,replace(replace(fmessage, char(13), ''), char(10), '') as 내용  from {1} with (nolock)\r\n\t\t" +
                            //"where sUserID = '{2}' and fsenddate >= '{3}' and fsenddate < '{4}'",
                            //"where sJobID in (select sJobID FROM JobLog where sUserID = '{2}' and dtStartTime >= '{3}' and dtStartTime < '{4}' and nSvcType in (5,6))",
                            tablename, tablename.Replace("SM", "TBL"), userid, start_month.ToString().Substring(0, 10), end_month.AddDays(1).ToString().Substring(0, 10)
                            );

                            if (dateDiff.Days < 5)
                            {
                                sql_mmsJob = String.Format(
                                "select distinct(sSplitSrv) from JobLog with(nolock) where suserid = '{0}' and dtRecvTime >= '{1}' and dtRecvTime < '{2}' and left(sjobid,2) in ('JL','JM') \r\n\t\t" +
                                "union \r\n\t\t" +
                                "select distinct(sSplitSrv) from JobmmsLog with(nolock) where suserid = '{0}' and dtRecvTime >= '{1}' and dtRecvTime < '{2}' and left(sjobid,2) in ('JL','JM')"
                                ,
                                userid, end_month.AddDays(-3).ToString().Substring(0, 10), end_month.AddDays(1).ToString().Substring(0, 10));
                                sqls_job_returned.Add(sql_mmsJob);

                            }
                        }
                        else // 문자, mms, 비즈.
                        {
                            sql_mms = String.Format(
                            "select 구분, JobID, UserID, 수신번호, 발신번호, 전송시간, 완료시간, 전송결과, 제목, 내용 from ( " +
                            "select distinct msgkey, reportdate, 'MMS' AS 구분, ETC1 as JobID, sUserID AS UserID, PHONE AS 수신번호, CALLBACK AS 발신번호,CONVERT(CHAR(19), REQDATE, 120) AS 전송시간, CONVERT(CHAR(19), RSLTDATE, 120) AS 완료시간,\r\n\t\t" +
                            "case when rslt='1000' and id='-1' then '전송시간 초과' else (select top 1 sName from iComFax.dbo.CCode(nolock) where sCode = RSLT and sGroup = 'LG') end as 전송결과, \r\n\t\t" +
                            "replace(replace(SUBJECT, char(13), ''), char(10), '') as 제목, replace(replace(MSG, char(13), ''), char(10), '') as 내용  from {0} with(nolock)\r\n\t\t" +
                            "where sUserID = '{1}' and REQDATE >= '{2}' and REQDATE < '{3}')a",
                            //"where etc1 in (select sJobID FROM [75].ASP.dbo.JobLog with (nolock) where sUserID = '{1}' and dtStartTime >= '{2}' and dtStartTime < '{3}' and nSvcType in (5,6))\r\n\t\t",
                            tablename, userid, start_month.ToString().Substring(0, 10), end_month.AddDays(1).ToString().Substring(0, 10)
                            );

                            if(dateDiff.Days < 5)
                            {
                                sql_mmsJob = String.Format(
                                "select distinct(sSrvName) as sSplitSrv from [75].ASP.dbo.JobLog with(nolock) where suserid = '{0}' and dtRecvTime >= '{1}' and dtRecvTime < '{2}' and left(sjobid,2)='JB' \r\n\t\t" +
                                "union \r\n\t\t" +
                                "select distinct(sSrvName) as sSplitSrv from [75].ASP.dbo.JobmmsLog with(nolock) where suserid = '{0}' and dtRecvTime >= '{1}' and dtRecvTime < '{2}' and left(sjobid,2)='JB'"
                                ,
                                userid, end_month.AddDays(-3).ToString().Substring(0, 10), end_month.AddDays(1).ToString().Substring(0, 10));

                                sqls_job_returned.Add(sql_mmsJob);
                            }
                        }                        
                        sqls_returned.Add(sql_mms);
                        //SQL_JobSearch(sqls_job_returned.ToArray(), userid, start_month, end_month, (bool)isDotcom);
                    }
                }
                else // 팩스(태스크) or 알림톡
                {
                    if (!isSMSorAlarmtalk) // 팩스임
                    {                
                        string faxnum;
                        if ((bool)isDotcom)
                        {
                            faxnum = "sFaxNum";
                        }
                        else
                        {
                            faxnum = "sFaxNo";
                        }
                        sql_faxtask = String.Format(
                         "select distinct sJobID as 접수번호, sUserID as UserID,{4} as 수신번호,sFromInfo as 발신번호,CONVERT(CHAR(19), dtStartTime, 120) as 전송시간,CONVERT(CHAR(19), dtEndTime, 120) as 완료시간," +
                         "(select top 1 sName from iComFax.dbo.CCode with(nolock) where sCode = nResult and sGroup = 'FAX') as 전송결과" +
                         "\r\nfrom {0} with(nolock) where suserid = '{1}' and dtStartTime >= '{2}' and dtStartTime < '{3}'",
                         tablename, userid, start_month.ToString().Substring(0, 10), end_month.AddDays(1).ToString().Substring(0, 10), faxnum);
                        sqls_returned.Add(sql_faxtask);
                    }
                    else // 알림톡임
                    {
                        isDotcom = true;
                        sql_alarmtalk = String.Format(
                         "select distinct sJobID as 접수번호,sUserID as UserID,CALLPHONE as 수신번호,REQPHONE as 발신번호, SENTTIME as 전송시간,RECVTIME as 완료시간, " +
                         "(select top 1 sname from icomfax.dbo.ccode with(nolock) where sgroup='AT' AND SCODE=ERRCODE ) as 전송결과,SUBJECT as 제목, ID AS 비용, replace(replace(MSG, char(13), ''), char(10), '') AS 내용 \r\n" +
                         "from {0} with (nolock) where sUserID='{1}' and RECVTIME >= '{2}' and RECVTIME < '{3}'",
                         tablename, userid, start_month.ToString().Substring(0, 10), end_month.AddDays(1).ToString().Substring(0, 10));
                       
                        if(dateDiff.Days < 5)
                        {
                            sql_alarmJob = String.Format(
                             "select distinct(sSrvName) as sSplitSrv from [75].ASP.dbo.JobBizLog with(nolock) where suserid = '{0}' and dtRecvTime >= '{1}' and dtRecvTime < '{2}'" 
                             ,userid, end_month.AddDays(-3).ToString().Substring(0, 10), end_month.AddDays(1).ToString().Substring(0, 10));

                            sqls_job_returned.Add(sql_alarmJob);
                        }
                        sqls_returned.Add(sql_alarmtalk);                   
                    }                   
                }
                Console.WriteLine("sqls_returned.Count :" + sqls_returned.Count);
            }    

            string[] sql = SQL_JobSearch(sqls_job_returned.ToArray(), userid, start_month, end_month, (bool)isDotcom, (bool)isSMSorAlarmtalk, (bool)isFaxTask);
            foreach(string s in sql)
            {
                sqls_returned.Add(s);
            }
            foreach(string s in sqls_returned)
            {
                Console.WriteLine("sqls_returned : " + s);
            }
            return sqls_returned.ToArray();            
        }

        private string[] SQL_JobSearch(string[] search, string userid, DateTime startMonth, DateTime endMonth, bool isdotcom, bool issmsoralarmtalk, bool isfaxTask)
        {
            List<string> task_tables = new List<string> { };
            string strConn = @"Server=222.231.58.71; database=SMS; uid=eshinan; pwd=!eshinan4600";
            foreach(string s in search)
            {
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = s;
                    SqlDataReader rdr = cmd.ExecuteReader();
               
                    while (rdr.Read())
                    {
                        if(rdr["sSplitSrv"].ToString() == null || rdr["sSplitSrv"].ToString() == "")
                        {

                        }
                        else
                        {                          
                            task_tables.Add(rdr["sSplitSrv"].ToString());                            
                        }
                    }
                   
                    rdr.Close();
                }
            }
            string[] task_tables_toString = task_tables.ToArray();
            string[] Task_SQLs_message = SQL_TaskSearch(task_tables_toString, userid, startMonth, endMonth, isdotcom, issmsoralarmtalk, isfaxTask);
            return Task_SQLs_message;
        }

        private string[] SQL_TaskSearch(string[] task_tables, string userid, DateTime startMonth, DateTime endMonth, bool isdotCom, bool isSmsoralarmtalk, bool isfaxtask)
        {
            List<string> task_Split_Tables = new List<string> { };
            List<string> task_sql = new List<string> { };
        
            foreach (string spliter in task_tables)
            {
                if (spliter.Length > 6)
                {
                    var DongBoSplit = spliter.Split(';');
                    foreach(var split in DongBoSplit)
                    {                    
                        task_Split_Tables.Add(split);                      
                    }
                }
                else
                {
                    task_Split_Tables.Add(spliter);
                }
            }
            List<string> task_Split_Tables_Dis = task_Split_Tables.Distinct().ToList();
            foreach(string taskTable in task_Split_Tables_Dis)
            {
                if ((bool)!isfaxtask)
                {
                    if ((bool)isdotCom)
                    {
                        if ((bool)isSmsoralarmtalk)
                        {
                            //SMS 스플릿 조회
                            string table = "[" + taskTable + "]" + "." + "dbo" + "." + "SC_TRAN";
                            string sql = String.Format(
                                        "select 구분, JobID, UserID, 수신번호, 발신번호, 전송시간, 완료시간, 전송결과, 제목, 내용 from ( " +
                                        "select distinct tr_num, tr_modified, 'SMS' AS 구분, TR_ETC1 as JobID, sUserID AS UserID, TR_PHONE AS 수신번호, TR_CALLBACK AS 발신번호,CONVERT(CHAR(19), TR_SENDDATE, 120) AS 전송시간, CONVERT(CHAR(19), TR_RSLTDATE, 120) AS 완료시간,\r\n\t\t" +
                                        "case when TR_RSLTSTAT='1000' and TR_id='-1' then '전송시간 초과' else (select top 1 sName from iComFax.dbo.CCode(nolock) where sCode = TR_RSLTSTAT and sGroup = 'LG') end as 전송결과, \r\n\t\t" +
                                        "'' as 제목, replace(replace(TR_MSG, char(13), ''), char(10), '') as 내용  from {0} with (nolock)\r\n\t\t" +
                                        "where sUserID = '{1}' and TR_SENDDATE >= '{2}' and TR_SENDDATE < '{3}')a", table, userid, startMonth.ToString().Substring(0, 10), endMonth.AddDays(1).ToString().Substring(0, 10));
                            task_sql.Add(sql);
                        }
                        else
                        {
                            //LMS, MMS 스플릿 조회
                            string table = "[" + taskTable + "]" + "." + "dbo" + "." + "MMS_MSG";
                            string sql = String.Format(
                                        "select 구분, JobID, UserID, 수신번호, 발신번호, 전송시간, 완료시간, 전송결과, 제목, 내용 from ( " +
                                        "select distinct msgkey, reportdate, 'MMS' AS 구분, ETC1 as JobID, sUserID AS UserID, PHONE AS 수신번호, CALLBACK AS 발신번호,CONVERT(CHAR(19), REQDATE, 120) AS 전송시간, CONVERT(CHAR(19), RSLTDATE, 120) AS 완료시간,\r\n\t\t" +
                                        "case when rslt='1000' and id='-1' then '전송시간 초과' else (select top 1 sName from iComFax.dbo.CCode(nolock) where sCode = RSLT and sGroup = 'LG') end as 전송결과, \r\n\t\t" +
                                        "replace(replace(SUBJECT, char(13), ''), char(10), '') as 제목, replace(replace(MSG, char(13), ''), char(10), '') as 내용  from {0} with (nolock)\r\n\t\t" +
                                        "where sUserID = '{1}' and REQDATE >= '{2}' and REQDATE < '{3}')a ", table, userid, startMonth.ToString().Substring(0, 10), endMonth.AddDays(1).ToString().Substring(0, 10));
                            task_sql.Add(sql);
                            //sqls_returned.Add(sql);

                        }
                    }
                    else
                    {
                        if ((bool)isSmsoralarmtalk)
                        {
                            string table = "[75]" + "." + taskTable + "." + "dbo" + "." + "SC_TRAN";
                            string sql = String.Format(
                                        "select 구분, JobID, UserID, 수신번호, 발신번호, 전송시간, 완료시간, 전송결과, 제목, 내용 from ( " +
                                        "select distinct tr_num, tr_modified, 'SMS' AS 구분, TR_ETC1 as JobID, sUserID AS UserID, TR_PHONE AS 수신번호, TR_CALLBACK AS 발신번호,CONVERT(CHAR(19), TR_SENDDATE, 120) AS 전송시간, CONVERT(CHAR(19), TR_RSLTDATE, 120) AS 완료시간,\r\n\t\t" +
                                        "case when TR_RSLTSTAT='1000' and TR_id='-1' then '전송시간 초과' else (select top 1 sName from iComFax.dbo.CCode(nolock) where sCode = TR_RSLTSTAT and sGroup = 'LG') end as 전송결과, \r\n\t\t" +
                                        "'' as 제목, replace(replace(TR_MSG, char(13), ''), char(10), '') as 내용  from {0} with (nolock)\r\n\t\t" +
                                        "where sUserID = '{1}' and TR_SENDDATE >= '{2}' and TR_SENDDATE < '{3}')a ", table, userid, startMonth.ToString().Substring(0, 10), endMonth.AddDays(1).ToString().Substring(0, 10));
                            task_sql.Add(sql);
                            //sqls_returned.Add(sql);
                        }
                        else
                        {
                            string table ="[75]" + "." + taskTable + "." + "dbo" + "." + "MMS_MSG";
                            string sql = String.Format(
                                        "select 구분, JobID, UserID, 수신번호, 발신번호, 전송시간, 완료시간, 전송결과, 제목, 내용 from ( " +
                                        "select distinct msgkey, reportdate, 'MMS' AS 구분, ETC1 as JobID, sUserID AS UserID, PHONE AS 수신번호, CALLBACK AS 발신번호,CONVERT(CHAR(19), REQDATE, 120) AS 전송시간, CONVERT(CHAR(19), RSLTDATE, 120) AS 완료시간,\r\n\t\t" +
                                        "case when rslt='1000' and id='-1' then '전송시간 초과' else (select top 1 sName from iComFax.dbo.CCode(nolock) where sCode = RSLT and sGroup = 'LG') end as 전송결과, \r\n\t\t" +
                                        "replace(replace(SUBJECT, char(13), ''), char(10), '') as 제목, replace(replace(MSG, char(13), ''), char(10), '') as 내용  from {0} with (nolock)\r\n\t\t" +
                                        "where sUserID = '{1}' and REQDATE >= '{2}' and REQDATE < '{3}')a ", table, userid, startMonth.ToString().Substring(0, 10), endMonth.AddDays(1).ToString().Substring(0, 10));
                            task_sql.Add(sql);
                            //sqls_returned.Add(sql);

                        }
                    }
                }
                else
                {
                    if ((bool)!isSmsoralarmtalk)
                    {

                    }
                    else
                    {
                        string table = "[75]" + "." + taskTable + "." + "dbo" + "." + "SUREData_Log";
                        string sql =  String.Format(
                         "select distinct sJobID as 접수번호,sUserID as UserID,CALLPHONE as 수신번호,REQPHONE as 발신번호, SENTTIME as 전송시간,RECVTIME as 완료시간, " +
                         "(select top 1 sname from icomfax.dbo.ccode with(nolock) where sgroup='AT' AND SCODE=ERRCODE ) as 전송결과,SUBJECT as 제목, ID AS 비용, MSG AS 내용 \r\n" +
                         "from {0} with (nolock) where sUserID='{1}' and RECVTIME >= '{2}' and RECVTIME < '{3}'",
                         table, userid, startMonth.ToString().Substring(0, 10), endMonth.AddDays(1).ToString().Substring(0, 10));
                        task_sql.Add(sql);
                    }
                }          
            }
            return task_sql.ToArray();
            
        }

        public string[] SQL_FaxJobResult(string userid, DateTime start_month, DateTime end_month, bool isdotcom)
        {
            string dotcom_biz;
            string page;
            if (isdotcom)
            {
                dotcom_biz = "[75].FAX.dbo.";
                page = "nTotalPage";
            }
            else
            {
                dotcom_biz = "[75].ASP.dbo.";
                page = "nPage";
            }
            string jobfax = string.Format(
                "select distinct sJobID as 접수번호,CONVERT(CHAR(19), dtStartTime, 120) as 전송시간,(select top 1 sname from icomfax.dbo.ccode with(nolock) where sgroup='FAX' AND SCODE=nResult) as 전송결과,nTotalCnt as 전체건수,{0} as 페이지,nSuccessCnt as 성공건수,nCharge as 요금\r\n" +
                "from {1}JobLog with (nolock) where sUserID='{2}' and (dtStartTime >= '{3}' and dtStartTime < '{4}') and nSvcType in (1,2)", page,dotcom_biz, userid, start_month.ToString().Substring(0, 10), end_month.AddDays(1).ToString().Substring(0, 10));

            string[] strings = new string[1];
            strings[0] = jobfax;
            return strings;
        }


    }
}
