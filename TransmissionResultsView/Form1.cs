
//using Microsoft.Office.Interop.Excel;
//using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using Action = System.Action;
using DataTable = System.Data.DataTable;

namespace TransmissionResultsView
{
    delegate void SqlMessageProcessDel(string[] SQLs_message, int width);
    public partial class Form1 : Form
    {
        // 전역변수 선언
        ExcelSave es = new ExcelSave();
        Sqls sql = new Sqls();

        //Thread th = null;
        

        public Form1()
        {
            InitializeComponent();
            this.Text = string.Format("{0} ver. {1}", "전송결과조회 프로그램", System.Windows.Forms.Application.ProductVersion);

            Init_DataGridView();
            dgv_message.ReadOnly = true;
            dgv_fax.ReadOnly = true;
            dgv_alarm.ReadOnly = true;
            dgv_message.VirtualMode = true;
            
     


            //string downpath = System.IO.Directory.GetCurrentDirectory() + "\\";
            //File.Delete(downpath + "TransmissionResultsView_old.exe");

            //Version_check();
            pnl_dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            dgv_message.Dock = DockStyle.Fill;
            pnl_dgv_fax.Dock = System.Windows.Forms.DockStyle.Fill;
            dgv_fax.Dock = DockStyle.Fill;
            pnl_dgv_alarm.Dock = System.Windows.Forms.DockStyle.Fill;
            dgv_alarm.Dock = DockStyle.Fill;

            dgv_message.AllowUserToAddRows = false;
            dgv_fax.AllowUserToAddRows = false;
            dgv_alarm.AllowUserToAddRows = false;

            // 컬럼크기 자동조정
            dgv_message.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dgv_fax.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dgv_alarm.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

            //dgv_message.TopLeftHeaderCell.Value = "Index";
            //dgv_message.RowHeadersWidth = 80;
            //dgv_fax.TopLeftHeaderCell.Value = "Index";
            //dgv_fax.RowHeadersWidth = 80;
            //dgv_alarm.TopLeftHeaderCell.Value = "Index";
            //dgv_alarm.RowHeadersWidth = 80;

            DateTime curr = DateTime.Now;

            if(curr.Month == 1)
            {
                dtp_since.Value = new DateTime(curr.Year - 1, 12, 01);
                dtp_until.Value = new DateTime(curr.Year, curr.Month, 02);

                dtp_since_fax.Value = new DateTime(curr.Year - 1, 12, 01);
                dtp_until_fax.Value = new DateTime(curr.Year, curr.Month, 02);

                dtp_since_alarm.Value = new DateTime(curr.Year - 1, 12, 01);
                dtp_until_alarm.Value = new DateTime(curr.Year, curr.Month, 02);
            }
            else
            {
                dtp_since.Value = new DateTime(curr.Year, curr.Month, 01);//DateTime.Now.AddMonths(-1);
                dtp_until.Value = new DateTime(curr.Year, curr.Month, curr.Day);
                //dtp_until.MaxDate = new DateTime(curr.Year, curr.Month + 1, 01).AddDays(-1);
                //dtp_since.MaxDate = new DateTime(curr.Year, curr.Month, 01);


                dtp_since_fax.Value = new DateTime(curr.Year, curr.Month, 01);//DateTime.Now.AddMonths(-1);
                dtp_until_fax.Value = new DateTime(curr.Year, curr.Month, curr.Day);
                //dtp_until_fax.MaxDate = new DateTime(curr.Year, curr.Month + 1, 01).AddDays(-1);
                //dtp_since_fax.MaxDate = new DateTime(curr.Year, curr.Month, 01);

                dtp_since_alarm.Value = new DateTime(curr.Year, curr.Month, 01);//DateTime.Now.AddMonths(-1);
                dtp_until_alarm.Value = new DateTime(curr.Year, curr.Month, curr.Day);
                //dtp_until_alarm.MaxDate = new DateTime(curr.Year, curr.Month + 1, 01).AddDays(-1);
                //dtp_since_alarm.MaxDate = new DateTime(curr.Year, curr.Month, 01);
            }


            tabcontrol.Controls.Remove(tp_SMS);
            tabcontrol.Controls.Remove(tp_alarmtalk);
            tabcontrol.Controls.Remove(tp_Fax);
            tabcontrol.Controls.Remove(tp_job);
            //tabcontrol.Controls.Remove(tp_test);
            //tp_SMS.Text = "";
            //tp_mms.Text = "MMS 전송결과";
            //tp_smsmms.Text = "SMS/MMS 통합조회";
            //tp_fax_task.Text = "FAX 전송결과(Task)";
            //tp_fax_job.Text = "FAX 전송결과(job)";
        }

        private void Init_DataGridView()
        {
            dgv_message.DoubleBuffered(true);
            dgv_fax.DoubleBuffered(true);
            dgv_alarm.DoubleBuffered(true);
        }

        public static void Convert_CSV_To_Excel(string filename, string filename2)
        {
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            Workbook wb = app.Workbooks.Open(filename, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            wb.SaveAs(filename2,XlFileFormat.xlOpenXMLWorkbook, Type.Missing,Type.Missing, Type.Missing, Type.Missing,XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            wb.Close();
            app.Quit();
            
        }
        private void btn_save_Click(object sender, EventArgs e)
        {
            if(dgv_message.Rows.Count == 0)
            {
                MessageBox.Show("저장할 자료가 없습니다.");
                return;
            }
            if (dgv_message.Rows.Count > 1048575)
            {
                MessageBox.Show("엑셀 최대 행수(1,048,576행)를 초과하였습니다. 개발팀에 문의해주세요.");
                return;
            }
            
            SaveFileDialog saveFileDialog = es.GetCsvSave();
            string userid = txt_sms_userID.Text;
            saveFileDialog.FileName = dtp_since.Text + " - " + dtp_until.Text + " " + userid + " 문자 전송결과";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;
                es.Save_csv(saveFileDialog.FileName, dgv_message, true);

                string filename = saveFileDialog.FileName;
                string filename2 = filename.Replace(".csv", ".xlsx");
                if (true)
                {
                    Convert_CSV_To_Excel(filename, filename2);
                    Thread.Sleep(200);
                    MessageBox.Show(filename2 + " 파일 저장 완료", "안내");
                    File.Delete(filename);
                }
                else
                {
                    //MessageBox.Show(filename + " 파일 저장 완료", "안내");
                }
                
            }
            Cursor.Current = Cursors.Default;
        }
        private void btn_save_alarm_Click(object sender, EventArgs e)
        {
            if (dgv_alarm.Rows.Count == 0)
            {
                MessageBox.Show("저장할 자료가 없습니다.", "안내");
                return;
            }
            if (dgv_alarm.Rows.Count > 1048575)
            {
                MessageBox.Show("엑셀 최대 행수(1,048,576행)를 초과하였습니다. 개발팀에 문의해주세요.");
                return;
            }
            
            SaveFileDialog saveFileDialog = es.GetCsvSave();
            string userid = txt_userid_alarm.Text;
            saveFileDialog.FileName = dtp_since_alarm.Text + " - " + dtp_until_alarm.Text + " " + userid + " 알림톡 전송결과";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;
                es.Save_csv(saveFileDialog.FileName, dgv_alarm, true); // dataGridView에 데이터를 세팅하는 메서드를 호출
                string filename = saveFileDialog.FileName;
                string filename2 = filename.Replace(".csv", ".xlsx");
                if (true)
                {
                    Convert_CSV_To_Excel(filename, filename2);
                    Thread.Sleep(200);
                    MessageBox.Show(filename2 + " 파일 저장 완료", "안내");
                    File.Delete(filename);
                }
                else
                {
                    //MessageBox.Show(filename + " 파일 저장 완료", "안내");
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void btn_save_fax_Click(object sender, EventArgs e)
        {
            if (dgv_fax.Rows.Count == 0)
            {
                MessageBox.Show("저장할 자료가 없습니다.", "안내");
                return;
            }
            if (dgv_fax.Rows.Count > 1048575)
            {
                MessageBox.Show("엑셀 최대 행수(1,048,576행)를 초과하였습니다. 개발팀에 문의해주세요.");
                return;
            }
            
            SaveFileDialog saveFileDialog = es.GetCsvSave();
            string userid = txt_userid_fax.Text;
            saveFileDialog.FileName = dtp_since_fax.Text + " - " + dtp_until_fax.Text + " " + userid + " 팩스 전송결과";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;
                es.Save_csv(saveFileDialog.FileName, dgv_fax, true); // dataGridView에 데이터를 세팅하는 메서드를 호출
                string filename = saveFileDialog.FileName;
                string filename2 = filename.Replace(".csv", ".xlsx");
                if (true)
                {
                    Convert_CSV_To_Excel(filename, filename2);
                    Thread.Sleep(200);
                    MessageBox.Show(filename2 + " 파일 저장 완료", "안내");
                    File.Delete(filename);
                }
                else
                {
                    //MessageBox.Show(filename + " 파일 저장 완료", "안내");
                }
                
            }
            Cursor.Current = Cursors.Default;
        }
        
 
        private async void btn_select_Click(object sender, EventArgs e)
        {
            //Stopwatch sw = new Stopwatch();
            //Stopwatch sw2 = new Stopwatch(); 
    
            if (dtp_since.Value > dtp_until.Value)
            {
                MessageBox.Show("조회기간을 확인하세요.", "오류");
                return;
            }
            if(txt_sms_userID.Text.Replace(" ", string.Empty) == "")
            {
                MessageBox.Show("아이디를 입력하세요.", "오류");
                return;
            }
            
            dgv_message.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            
            Cursor.Current = Cursors.WaitCursor;
            List<string> tableNames;
            List<string> tableNames_sms;
            List<string> tableNames_mms;
            string[] SQLs_message;
            string[] SQLs_message_sms;
            string[] SQLs_message_mms;
          
        
            if (rb_dotcom.Checked)
            {
                if (rb_sms.Checked)
                {
                    tableNames = sql.Get_tableNames(1);
                    SQLs_message = sql.SQLs_MessageResult(txt_sms_userID.Text, tableNames, dtp_since.Value, dtp_until.Value, true, false, true);
                }
                else if (rb_mms.Checked)
                {
                    tableNames = sql.Get_tableNames(2);
                    SQLs_message = sql.SQLs_MessageResult(txt_sms_userID.Text, tableNames, dtp_since.Value, dtp_until.Value, false, false, true);
                }
                else
                {
                    tableNames_sms = sql.Get_tableNames(1);
                    tableNames_mms = sql.Get_tableNames(2);
                    SQLs_message_sms = sql.SQLs_MessageResult(txt_sms_userID.Text, tableNames_sms, dtp_since.Value, dtp_until.Value, true, false, true);
                    SQLs_message_mms = sql.SQLs_MessageResult(txt_sms_userID.Text, tableNames_mms, dtp_since.Value, dtp_until.Value, false, false, true);
                    SQLs_message = SQLs_message_sms.Concat(SQLs_message_mms).ToArray();
                }
            }
            else
            {
                if (rb_sms.Checked)
                {
                    tableNames = sql.Get_tableNames(3);
                    SQLs_message = sql.SQLs_MessageResult(txt_sms_userID.Text, tableNames, dtp_since.Value, dtp_until.Value, true, false, false);
                }
                else if (rb_mms.Checked)
                {
                    tableNames = sql.Get_tableNames(4);
                    SQLs_message = sql.SQLs_MessageResult(txt_sms_userID.Text, tableNames, dtp_since.Value, dtp_until.Value, false, false, false);
                }
                else
                {
                  
                    tableNames_sms = sql.Get_tableNames(3);
                    tableNames_mms = sql.Get_tableNames(4);
                    SQLs_message_sms = sql.SQLs_MessageResult(txt_sms_userID.Text, tableNames_sms, dtp_since.Value, dtp_until.Value, true, false, false);
                    SQLs_message_mms = sql.SQLs_MessageResult(txt_sms_userID.Text, tableNames_mms, dtp_since.Value, dtp_until.Value, false, false, false);
                    SQLs_message = SQLs_message_sms.Concat(SQLs_message_mms).ToArray();
                }
            }
           
                    dgv_message.DataSource = await sql.SqlsToMergedDt(SQLs_message);
                    dgv_message.RowHeadersWidth = 70;

                    await Task.Run(() =>
                    {
                        setRowNumberMessage(dgv_message);
                        if (this.InvokeRequired)
                        {
                            this.BeginInvoke(new MethodInvoker(delegate
                            {
                                int cnt_dgv = dgv_message.Rows.Count;
                                if (cnt_dgv == 0)
                                {
                                    lbl_cnt.Text = cnt_dgv.ToString() + "건이 조회되었습니다.";
                                }
                                else
                                {
                                    lbl_cnt.Text = cnt_dgv.ToString() + "건이 조회되었습니다.";
                                    //MessageBox.Show(cnt_dgv.ToString() + "건이 조회되었습니다.","안내");
                                }
                                Cursor.Current = Cursors.Default;
                            }));
                        }
                    });
                    dgv_message.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                

               /* if (this.dgv_message.InvokeRequired)
                {
                    this.Invoke(new SqlMessageProcessDel(SqlMessageProcess), new object[]{
                        SQLs_message, 70
                    });
                }
                else
                {
                }*/

                /*              dgv_message.DataSource = sql.SqlsToMergedDt(SQLs_message);
                              dgv_message.RowHeadersWidth = 70;*/

          
            //sw.Start();

            // DataTable dt = sql.SqlsToMergedDt(SQLs_message); //53~59  50
            //sw.Stop();
            //sw2.Start();
            //string union = String.Join(" union all ", SQLs_message);
            //DataTable dt = sql.SqlToDt(union);
           
            
                    //sw2.Stop();

                    //MessageBox.Show("sw1 - " + sw.ElapsedMilliseconds);
                    //MessageBox.Show("sw2 - " + sw2.ElapsedMilliseconds);

        }

       /* private async void SqlMessageProcess(string[] SQLs_message, int width)
        {
            Console.WriteLine("Task_03");
            dgv_message.DataSource = await sql.SqlsToMergedDt(SQLs_message);
            dgv_message.RowHeadersWidth = width;
            Console.WriteLine("Task_07");

        }*/

        public async void setRowNumberMessage(DataGridView message)
        {             
            await Task.Run(() =>
            {               
                foreach (DataGridViewRow row in message.Rows)
                {
                    row.HeaderCell.Value = String.Format("{0}", row.Index + 1);
       
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke(new MethodInvoker(delegate
                        {
                            progressBar3.Minimum = 0;
                            progressBar3.Maximum = message.Rows.Count;
                            progressBar3.Value = row.Index + 1;
                            
                            if (row.Index + 1 == message.Rows.Count)
                            {
                                Delay(1000);
                                MessageBox.Show("조회가 완료되었습니다");
                                //progressBar3.Value = 0;                               
                            }
                        }));
                    }               
                }              
            });
        }

        private static DateTime Delay(int v)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, v);
            DateTime AfterWards = ThisMoment.Add(duration);

            while(AfterWards >= ThisMoment)
            {
                System.Windows.Forms.Application.DoEvents();
                ThisMoment = DateTime.Now;
            }
            return DateTime.Now;
        }

        public async void setRowNumberFax(DataGridView fax)
        {
            await Task.Run(() =>
            {
                foreach (DataGridViewRow row in fax.Rows)
                {
                    row.HeaderCell.Value = String.Format("{0}", row.Index + 1);

                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke(new MethodInvoker(delegate
                        {
                            progressBar2.Minimum = 0;
                            progressBar2.Maximum = fax.Rows.Count;
                            progressBar2.Value = row.Index + 1;

                            if (row.Index + 1 == fax.Rows.Count)
                            {
                                Delay(1000);
                                MessageBox.Show("조회가 완료되었습니다");
                                //progressBar2.Value = 0;
                            }
                        }));
                    }
                }
            });
        }

        public async void setRowNumberAlarm(DataGridView alarm)
        {
            await Task.Run(() =>
            {
                foreach (DataGridViewRow row in alarm.Rows)
                {
                    row.HeaderCell.Value = String.Format("{0}", row.Index + 1);

                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke(new MethodInvoker(delegate
                        {
                            progressBar1.Minimum = 0;
                            progressBar1.Maximum = alarm.Rows.Count;
                            progressBar1.Value = row.Index + 1;

                            if (row.Index + 1 == alarm.Rows.Count)
                            {
                                Delay(1000);
                                MessageBox.Show("조회가 완료되었습니다");
                                //progressBar1.Value = 0;
                            }
                        }));
                    }
                }
            });
        }

        private async void btn_select_fax_Click(object sender, EventArgs e)
        {
            if (dtp_since_fax.Value > dtp_until_fax.Value)
            {
                MessageBox.Show("조회기간을 확인하세요.", "오류");
                return;
            }
            if (txt_userid_fax.Text == "")
            {
                MessageBox.Show("아이디를 입력하세요.", "오류");
                return;
            }
            dgv_fax.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            
            Cursor.Current = Cursors.WaitCursor;
            List<string> tableNames;
            string[] SQLs_message;
            if (rb_dotcom_fax.Checked)
            {
                if (rb_job.Checked)
                {
                    SQLs_message = sql.SQL_FaxJobResult(txt_userid_fax.Text, dtp_since_fax.Value, dtp_until_fax.Value, true);
                }
                else // 닷컴, 팩스테스크
                {
                    tableNames =  sql.Get_tableNames(5);
        
                    SQLs_message = sql.SQLs_MessageResult(txt_userid_fax.Text, tableNames, dtp_since_fax.Value, dtp_until_fax.Value, false, true, true);
                }
            }
            else
            {
                if (rb_job.Checked)
                {
                    SQLs_message = sql.SQL_FaxJobResult(txt_userid_fax.Text, dtp_since_fax.Value, dtp_until_fax.Value, false);
                }
                else // 비즈 팩스 테스크
                {                 
                    tableNames =  sql.Get_tableNames(6);
                    SQLs_message = sql.SQLs_MessageResult(txt_userid_fax.Text, tableNames, dtp_since_fax.Value, dtp_until_fax.Value, false, true,false);
                }
            }
    
            DataTable dt = await sql.SqlsToMergedDt(SQLs_message);
       
            dgv_fax.DataSource = dt;
            dgv_fax.RowHeadersWidth = 70;
            await Task.Run(() =>
            {
                setRowNumberFax(dgv_fax);
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new MethodInvoker(delegate
                    {
                        int cnt_dgv = dgv_fax.Rows.Count;
                        if (cnt_dgv == 0)
                        {
                            lbl_cnt_fax.Text = cnt_dgv.ToString() + "건이 조회되었습니다.";
                        }
                        else
                        {
                            lbl_cnt_fax.Text = cnt_dgv.ToString() + "건이 조회되었습니다.";
                            //MessageBox.Show(cnt_dgv.ToString() + "건이 조회되었습니다.","안내");
                        }

                        Cursor.Current = Cursors.Default;

                    }));
                }
            });
            dgv_fax.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        }
        private async void btn_select_alarm_Click(object sender, EventArgs e)
        {
            if (dtp_since_alarm.Value > dtp_until_alarm.Value)
            {
                MessageBox.Show("조회기간을 확인하세요.", "오류");
                return;
            }
            if (txt_userid_alarm.Text == "")
            {
                MessageBox.Show("아이디를 입력하세요.", "오류");
                return;
            }
            dgv_alarm.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            Cursor.Current = Cursors.WaitCursor;
            List<string> tableNames;
            string[] SQLs_message;


            tableNames =  sql.Get_tableNames(7);
            SQLs_message = sql.SQLs_MessageResult(txt_userid_alarm.Text, tableNames, dtp_since_alarm.Value, dtp_until_alarm.Value, true, true);

            DataTable dt = await sql.SqlsToMergedDt(SQLs_message); //58~59;
            //53
            //string union = String.Join(" union all ", SQLs_message); 
            //DataTable dt = sql.SqlToDt(union);

            dgv_alarm.DataSource = dt;
            dgv_alarm.RowHeadersWidth = 70;

            await Task.Run(() =>
            {
                setRowNumberAlarm(dgv_alarm);
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new MethodInvoker(delegate
                    {
                        int cnt_dgv = dgv_alarm.Rows.Count;
                        if (cnt_dgv == 0)
                        {
                            lbl_cnt_alarm.Text = cnt_dgv.ToString() + "건이 조회되었습니다.";
                        }
                        else
                        {
                            lbl_cnt_alarm.Text = cnt_dgv.ToString() + "건이 조회되었습니다.";
                            //MessageBox.Show(cnt_dgv.ToString() + "건이 조회되었습니다.","안내");
                        }

                        Cursor.Current = Cursors.Default;
                    }));
                }

            });
            dgv_alarm.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;           
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (!tabcontrol.Controls.Contains(tp_SMS))
            {
                tabcontrol.Controls.Add(tp_SMS);
                tabcontrol.SelectedTab = tp_SMS;
            }
            else
            {
                tabcontrol.SelectedTab = tp_SMS;
            }
            txt_sms_userID.Focus();
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!tabcontrol.Controls.Contains(tp_Fax))
            {
                tabcontrol.Controls.Add(tp_Fax);
                tabcontrol.SelectedTab = tp_Fax;
            }
            else
            {
                tabcontrol.SelectedTab = tp_Fax;
            }
            txt_userid_fax.Focus();
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (!tabcontrol.Controls.Contains(tp_alarmtalk))
            {
                tabcontrol.Controls.Add(tp_alarmtalk);
                tabcontrol.SelectedTab = tp_alarmtalk;
            }
            else
            {
                tabcontrol.SelectedTab = tp_alarmtalk;
            }
            txt_userid_alarm.Focus();
        }
        private void txt_sms_userID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_select.PerformClick();
            }
        }
        private void txt_userid_fax_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_select_fax.PerformClick();
            }
        }
        private void txt_userid_alarm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_select_alarm.PerformClick();
            }
        }

        private void dgv_message_DataSourceChanged(object sender, EventArgs e)
        {
        }

        private void dgv_fax_DataSourceChanged(object sender, EventArgs e)
        {

        }

        private void dgv_alarm_DataSourceChanged(object sender, EventArgs e)
        {

        }

        private void rb_smsmms_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dtp_since_ValueChanged(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            progressBar1.Style = ProgressBarStyle.Blocks;
            progressBar2.Style = ProgressBarStyle.Blocks;
            progressBar3.Style = ProgressBarStyle.Blocks;
        }


        //버전체크, INI 파일, 공유폴더접근 등등
        //public void Version_check()
        //{
        //    Uri address = new Uri(@"https://www.moashot.com/download/lsw_test/");
        //    string downpath = System.IO.Directory.GetCurrentDirectory() + "\\";
        //    WebClient wc = new WebClient();
        //    wc.DownloadFile(address.ToString() + "version.ini", downpath + "version.ini");
        //    string version_server = "";
        //    version_server = GetIniValue("VERSION", "ver", downpath + "version.ini");
        //    string version_local = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        //    this.Text = "전송결과조회 ver" +version_local;


        //    if (version_local.Equals(version_server))
        //    {
        //        //MessageBox.Show("업데이트할게 없음.", "안내");
        //        File.Delete(downpath + "version.ini");
        //    }
        //    else
        //    {
        //        MessageBox.Show("업데이트가 있습니다.", "안내");

        //        //Rename(downpath, "TransmissionResultsView.exe", "TransmissionResultsView2.exe");
        //        Rename(downpath, "TransmissionResultsView.exe", "TransmissionResultsView_old.exe");
        //        wc.DownloadFile(address.ToString() + "TransmissionResultsView.exe", downpath + "TransmissionResultsView.exe");
        //        DialogResult dialogResult = MessageBox.Show("업데이트 완료! 프로그램을 재시작 하시겠습니까?", "안내",MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

        //        //OK 클릭 시

        //        if (dialogResult == DialogResult.OK)
        //        {
        //            File.Delete(downpath + "version.ini");
        //            Thread.Sleep(1000);
        //            System.Windows.Forms.Application.Exit();
        //            System.Diagnostics.Process.Start(downpath + "TransmissionResultsView.exe");

        //        }
        //        else if (dialogResult == DialogResult.Cancel)
        //        {
        //            MessageBox.Show("새로운 버전은 다음 실행부터");
        //        }
        //    }
        //}


        //public void Version_check_shared()
        //{
        //    string downpath = System.IO.Directory.GetCurrentDirectory() + "\\";
        //    string shared = @"\\192.168.0.15\Util\TransmissionResultView\";
        //    //File.Copy(shared + "TransmissionResultsView.exe", downpath + "TransmissionResultsView.exe");
        //    string version_server = "";
        //    version_server = GetIniValue("VERSION", "ver", shared + "version.ini");
        //    string version_local = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        //    this.Text = "전송결과조회 ver" + version_local;

        //    if (version_local.Equals(version_server))
        //    {

        //    }
        //    else
        //    {
        //        Rename(downpath, "TransmissionResultsView.exe", "TransmissionResultsView_old.exe");
        //        File.Copy(shared + "TransmissionResultsView.exe", downpath + "TransmissionResultsView.exe");
        //        DialogResult dialogResult = MessageBox.Show("업데이트 완료! 프로그램을 재시작 하시겠습니까?", "안내", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

        //        //OK 클릭 시

        //        if (dialogResult == DialogResult.OK)
        //        {
        //            Thread.Sleep(1000);
        //            System.Windows.Forms.Application.Exit();
        //            System.Diagnostics.Process.Start(downpath + "TransmissionResultsView.exe");

        //        }
        //        else if (dialogResult == DialogResult.Cancel)
        //        {
        //            MessageBox.Show("새로운 버전은 다음 실행부터 적용됩니다.");
        //        }
        //    }
        //}

        //[DllImport("kernel32.dll")]
        //private static extern int GetPrivateProfileString(    // INI Read
        //    String section,
        //    String key,
        //    String def,
        //    StringBuilder retVal,
        //    int size,
        //    String filePath);

        //// INI Write를 위한 API 선언
        //[DllImport("kernel32.dll")]
        //private static extern long WritePrivateProfileString(  // INI Write
        //    String section,
        //    String key,
        //    String val,
        //    String filePath);
        //public String GetIniValue(String Section, String Key, String iniPath)
        //{
        //    StringBuilder temp = new StringBuilder(255);
        //    int i = GetPrivateProfileString(Section, Key, "", temp, 255, iniPath);
        //    return temp.ToString();
        //}
        //public void SetIniValue(String Section, String Key, String Value, String iniPath)
        //{
        //    WritePrivateProfileString(Section, Key, Value, iniPath);
        //}

        //public static void Rename(string filepath, string oldfile, string newfile)
        //{
        //    oldfile = filepath + "\\" + oldfile;
        //    newfile = filepath + "\\" + newfile;

        //    FileInfo fi = new FileInfo(oldfile);
        //    if (fi.Exists)
        //    {
        //        File.Move(oldfile, newfile);
        //    }
        //    else
        //    {
        //        MessageBox.Show("파일이 존재하지 않음");
        //    }
        //}

        //SharedAPI sharedAPI = new SharedAPI();
        //private void Form1_Shown(object sender, EventArgs e)
        //{
        //    //int a = sharedAPI.ConnectRemoteServer(@"\\192.168.0.15\Util");
        //    //if (a == 0)
        //    //{
        //    //    Version_check_shared();
        //    //}
        //    //else
        //    //{
        //    //    DialogResult dialogResult = MessageBox.Show("버전확인을 위한 공유폴더에 접근할 수 없습니다.", "안내");
        //    //    //Version_check();
        //    //}
        //    ////Version_check();
        //}
    }
    public static class ExtenstionMethods { 
        public static void DoubleBuffered(this DataGridView dgv, bool setting) 
        { Type dgvType = dgv.GetType(); PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic); pi.SetValue(dgv, setting, null); 
        } 
    }
    
}
