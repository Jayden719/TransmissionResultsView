
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataTable = System.Data.DataTable;



namespace TransmissionResultsView
{
    internal class ExcelSave
    {
        //public void TOExcel(DataTable dt, string path)
        //{

        //    Excel.Application appliCation;
        //    Excel.Workbook workBook;
        //    Excel.Worksheet workSheet;
        //    //Excel.Worksheet newWorksheet;
            
        //    try
        //    {
        //        appliCation = new Excel.Application();
        //        workBook = (Excel.Workbook)(appliCation.Workbooks.Add(true));
        //        workSheet = (Excel.Worksheet)workBook.ActiveSheet;
        //        //newWorksheet = (Excel.Worksheet)appliCation.Worksheets.Add();
        //        appliCation.Visible = false;
        //        appliCation.UserControl = false;
        //        object TypMissing = Type.Missing;
        //        int iRow = 0;
        //        string[] headers = new string[dt.Columns.Count];
        //        string[] columns = new string[dt.Columns.Count];
        //        string[,] item = new string[dt.Rows.Count, dt.Columns.Count];

        //        //데이터데이플에있는 값들을 엑셀에담기위해서 item이라는 배열에담기
        //        if (dt.Rows.Count > 0)
        //        {
        //            for (int c = 0; c < dt.Columns.Count; c++)
        //            {
        //                //DataTable 첫 Row에있는 컬럼명을 담기
        //                headers[c] = dt.Columns[c].ColumnName;
        //                //컬럼 위치값을 가져오기
        //                columns[c] = ExcelColumnIndexToName(c);
        //            }


        //            for (int rowNo = 0; rowNo < dt.Rows.Count; rowNo++)
        //            {
        //                for (int colNo = 0; colNo < dt.Columns.Count; colNo++)
        //                {
        //                    item[rowNo, colNo] = dt.Rows[rowNo][colNo].ToString();
        //                }

        //                iRow++;
        //            }
        //        }

        //        //해당위치에 컬럼명을 담기
        //        workSheet.get_Range("A1", columns[dt.Columns.Count - 1] + "1").Value2 = headers;
        //        //해당위치부터 데이터정보를 담기
        //        workSheet.get_Range("A2", columns[dt.Columns.Count - 1] + (dt.Rows.Count + 1).ToString()).Value = item;
        //        workSheet.Cells.NumberFormat = @"@";
        //        workSheet.Columns.AutoFit();
        //        workBook.SaveAs(path, Excel.XlFileFormat.xlWorkbookNormal, null, null, false, false,
        //        Excel.XlSaveAsAccessMode.xlShared, false, false, null, null, null);
        //        appliCation.Quit();
        //        releaseObject(appliCation);
        //        releaseObject(workSheet);
        //        releaseObject(workBook);
        //        MessageBox.Show("파일 저장 완료!");
        //    }
        //    catch (Exception theException)
        //    {
        //        MessageBox.Show(theException.Message.ToString());
        //    }
        //}

        //public void TOExcel_messages(DataTable mms_dt,DataTable sms_dt, string path)
        //{

        //    Excel.Application appliCation;
        //    Excel.Workbook workBook;
        //    Excel.Worksheet sms_sheet;
        //    Excel.Worksheet mms_sheet;

        //    try
        //    {
        //        appliCation = new Excel.Application();
        //        workBook = (Excel.Workbook)(appliCation.Workbooks.Add(true));
        //        mms_sheet = (Excel.Worksheet)workBook.ActiveSheet;
        //        sms_sheet = (Excel.Worksheet)appliCation.Worksheets.Add(After: workBook.Worksheets.Item[workBook.Worksheets.Count]);

        //        appliCation.Visible = false;
        //        appliCation.UserControl = false;
        //        object TypMissing = Type.Missing;
        //        sms_sheet.Name = "SMS";
        //        mms_sheet.Name = "MMS";
        //        MessageSheet(sms_sheet, sms_dt);
        //        MessageSheet(mms_sheet, mms_dt);
                
        //        workBook.SaveAs(path, Excel.XlFileFormat.xlWorkbookNormal, null, null, false, false,
        //        Excel.XlSaveAsAccessMode.xlShared, false, false, null, null, null);
        //        appliCation.Quit();
        //        releaseObject(appliCation);
        //        releaseObject(sms_sheet);
        //        releaseObject(workBook);
        //        MessageBox.Show("파일 저장 완료!");
        //    }
        //    catch (Exception theException)
        //    {
        //        MessageBox.Show(theException.Message.ToString());
        //    }
        //}

        //public void MessageSheet(Excel.Worksheet ws, DataTable dt)
        //{
        //    string[] headers = new string[dt.Columns.Count];
        //    string[] columns = new string[dt.Columns.Count];
        //    string[,] item = new string[dt.Rows.Count, dt.Columns.Count];
        //    int iRow = 0;
        //    //데이터데이플에있는 값들을 엑셀에담기위해서 item이라는 배열에담기
        //    if (dt.Rows.Count > 0)
        //    {
        //        for (int c = 0; c < dt.Columns.Count; c++)
        //        {
        //            //DataTable 첫 Row에있는 컬럼명을 담기
        //            headers[c] = dt.Columns[c].ColumnName;
        //            //컬럼 위치값을 가져오기
        //            columns[c] = ExcelColumnIndexToName(c);
        //        }


        //        for (int rowNo = 0; rowNo < dt.Rows.Count; rowNo++)
        //        {
        //            for (int colNo = 0; colNo < dt.Columns.Count; colNo++)
        //            {

        //                item[rowNo, colNo] = dt.Rows[rowNo][colNo].ToString();
        //            }

        //            iRow++;
        //        }
        //    }

        //    //해당위치에 컬럼명을 담기
        //    ws.get_Range("A1", columns[dt.Columns.Count - 1] + "1").Value2 = headers;
        //    //해당위치부터 데이터정보를 담기
        //    ws.get_Range("A2", columns[dt.Columns.Count - 1] + (dt.Rows.Count + 1).ToString()).Value = item;
        //    ws.Cells.NumberFormat = @"@";
        //    ws.Columns.AutoFit();
            
        //}
        //public static void releaseObject(object obj)
        //{
        //    try
        //    {
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
        //        obj = null;
        //    }
        //    catch
        //    {
        //        obj = null;
        //    }
        //    finally
        //    {
        //        GC.Collect();
        //    }
        //}
        //private string ExcelColumnIndexToName(int Index)
        //{
        //    string range = "";
        //    if (Index < 0) return range;
        //    for (int i = 1; Index + i > 0; i = 0)
        //    {
        //        range = ((char)(65 + Index % 26)).ToString() + range;
        //        Index /= 26;
        //    }
        //    if (range.Length > 1)
        //        range = ((char)((int)range[0] - 1)).ToString() + range.Substring(1);
        //    return range;
        //}


        public void Save_csv(string fileName, DataGridView dgview, bool header)
        {
            string delimiter = ",";  // 구분자
            FileStream fs = new FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            StreamWriter csvExport = new StreamWriter(fs, System.Text.Encoding.UTF8); //UTF8로 엔코딩

            if (dgview.Rows.Count == 0) return;

            // header가 true면 헤더정보 출력
            if (header)
            {
                for (int i = 0; i < dgview.Columns.Count; i++)
                {
                    csvExport.Write(dgview.Columns[i].HeaderText);
                    if (i != dgview.Columns.Count - 1)
                    {
                        csvExport.Write(delimiter);
                    }
                }
            }

            csvExport.Write(csvExport.NewLine); // add new line

            // 데이터 출력
            foreach (DataGridViewRow row in dgview.Rows)
            {
                if (!row.IsNewRow)
                {
                    for (int i = 0; i < dgview.Columns.Count; i++)
                    {

                        string val = row.Cells[i].Value.ToString();
                        if (val == null)
                        {
                            val = "";
                        }
                        else if (val.Contains('\n'))
                        {
                            val = val.Replace("\n", " ");
                            val = "\"" + val + "\"";
                        }
                        else
                        {
                            val = "=\"" + val + "\"";
                        }
                         
                        csvExport.Write(val);
                        if (i != dgview.Columns.Count - 1)
                        {
                            csvExport.Write(delimiter);
                        }
                    }
                    csvExport.Write(csvExport.NewLine); // write new line
                }
            }

            csvExport.Flush();
            csvExport.Close();
            fs.Close();

            
        }
        public SaveFileDialog GetCsvSave()
        {
            //Getting the location and file name of the excel to save from user.
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.CheckPathExists = true;
            saveDialog.AddExtension = true;
            saveDialog.ValidateNames = true;

            //string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //string filepath = Path.GetDirectoryName(path);


            saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            saveDialog.DefaultExt = ".csv";
            saveDialog.Filter = "csv (*.csv) | *.csv";
            //saveDialog.FileName = DateTime.Now.ToString();

            return saveDialog;
        }



        public void Save_Csv_dt(string fileName, DataTable dt, bool header)
        {
            string delimiter = ",";  // 구분자
            FileStream fs = new FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            StreamWriter csvExport = new StreamWriter(fs, System.Text.Encoding.UTF8);

            if (dt.Rows.Count == 0) return;

            // 헤더정보 출력
            if (header)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    csvExport.Write(dt.Columns[i].ColumnName);
                    if (i != dt.Columns.Count - 1)
                    {
                        csvExport.Write(delimiter);
                    }
                }
                csvExport.Write(csvExport.NewLine);
            }

            csvExport.Write(csvExport.NewLine); // add new line

            // 데이터 출력
            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string val = row[i].ToString();
                    if (val == null)
                    {
                        val = "";
                    }
                    else if (val.Contains('\n'))
                    {
                        val = val.Replace("\n", " ");
                        val = "\"" + val + "\"";
                    }
                    else
                    {
                        val = "=\"" + val + "\"";
                    }
                    csvExport.Write(val);
                    if (i != dt.Columns.Count - 1)
                    {
                        csvExport.Write(delimiter);
                    }
                }
                csvExport.Write(csvExport.NewLine); // write new line

            }

            csvExport.Flush(); // flush from the buffers.
            csvExport.Close();
            fs.Close();



            MessageBox.Show("파일 저장 완료!", "안내");


        }

    }


}
