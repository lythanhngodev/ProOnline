using System;
using System.Collections.Generic;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;
using System.Runtime.InteropServices;
using System.Collections;
using System.Reflection;
using System.Globalization;

namespace ProOnline.Class
{
    public class ClassExcelToWord
    {
        public ClassExcelToWord()
        {

        }

        #region Các biến cần đọc dữ liệu Excel.
        public Excel.Application xlApplicationExcel;
        public Excel.Workbook xlWorkbook;
        public Excel.Worksheet xlWorksheet;
        public object misValue = System.Reflection.Missing.Value;
        #endregion

        #region Các biến cần đọc dữ liệu Word.
        public Word.Application xlApplicationWord;
        public Word.Document doc;
        public Word.Bookmarks bookmarks = null;
        public Word.Bookmark myBookmark = null;
        public Word.Range bookmarkRange = null;
        public object objMiss = System.Reflection.Missing.Value;

        public object IconIndex = false;
        public object Link = false;
        public object Placement = Word.WdOLEPlacement.wdInLine;
        public object DisplayAsIcon = false;
        public object DataType = Word.WdPasteDataType.wdPasteOLEObject;
        public object IconFileName = true;
        public object IconLabel = true;


        public object objEndOfDocFlag = "\\endofdoc"; /* \endofdoc is a predefined bookmark */
        #endregion

        public void _mOpendFileDOC(object fileOpen, bool visible)
        {
            try
            {
                doc = new Word.Document();
                xlApplicationWord = new Word.Application();
                xlApplicationWord.Visible = visible;

                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

                //object missing = System.Type.Missing;
                //doc = xlApplicationWord.Documents.Open(ref fileOpen, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);

                doc = new Microsoft.Office.Interop.Word.Document();
                
            }
            catch 
            { }
        }
        public void _mOpendFileXLS(string fileOpen, bool visible)
        {
            try
            {
                _mSetDateTimeCultureInfo();

                xlApplicationExcel = new Excel.Application();
                xlApplicationExcel.Visible = visible;

                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                //
                xlWorkbook = xlApplicationExcel.Workbooks.Open(fileOpen, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.get_Item(1);
            }
            catch
            { }
        }
        public static void _mSetDateTimeCultureInfo()
        {
            // Cấu hình kiểu hiển thị thể hiện trên lưới.
            System.Globalization.CultureInfo cultureInfo =
                new System.Globalization.CultureInfo("en-US");

            ////Định dạng dấu chấm động.
            //NumberFormatInfo nfi = new NumberFormatInfo();
            //nfi.NumberDecimalSeparator = ",";
            //nfi.NumberGroupSeparator = ".";
            //// Gán nội dung
            //cultureInfo.NumberFormat = nfi;

            // Định dạng kiểu ngày tháng năm.
            System.Globalization.DateTimeFormatInfo dateTimeInfo =
                new System.Globalization.DateTimeFormatInfo();
            dateTimeInfo.DateSeparator = "/";
            dateTimeInfo.LongDatePattern = "dd-MMM-yyyy";
            dateTimeInfo.ShortDatePattern = "dd-MMM-yy";
            dateTimeInfo.LongTimePattern = "hh:mm:ss tt";
            dateTimeInfo.ShortTimePattern = "hh:mm tt";
            // Gán nội dung
            cultureInfo.DateTimeFormat = dateTimeInfo;

            // Assigning our custom Culture to the application.
            System.Windows.Forms.Application.CurrentCulture = cultureInfo;
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }
        public void _mCloseFileXLS()
        {
            try
            {
                if (xlWorkbook != null)
                {
                    xlWorkbook.Save();
                    xlWorkbook.Close(true, misValue, misValue);
                    xlApplicationExcel.Workbooks.Close();
                    xlApplicationExcel.Quit();

                    //Finally destroy all the objects.
                    if (xlWorksheet != null)
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorksheet);
                    if (xlWorkbook != null)
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkbook);
                    if (xlApplicationExcel != null)
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApplicationExcel);

                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    xlWorksheet = null; xlWorkbook = null; xlApplicationExcel = null;
                }
            }
            catch
            { }
        }
        public void _mSaveFileDOC(string fileSave, bool checkDelete, bool closeFile)
        {
            try
            {
                if (checkDelete)
                    if (System.IO.File.Exists(fileSave))
                        System.IO.File.Delete(fileSave);
                if (xlApplicationWord != null && fileSave == "")
                    doc.Save();
                if (xlApplicationWord != null && fileSave != "")
                {
                    object fileNameSave = fileSave;
                    object oMissing = System.Reflection.Missing.Value;
                    object FileFormat = Word.WdSaveFormat.wdFormatDocument;
                    object LockComments = false;
                    object Password = "";
                    object AddToRecentFiles = false;

                    //Save tập tin word.

                    doc.SaveAs(ref fileNameSave, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref objMiss);

                    //if (_vfileSave.ToLower().IndexOf(".docx") != -1)
                    //    doc.SaveAs(ref _vFileNameSave, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocumentDefault, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                }
                if (closeFile) //Đóng tập tin lại
                    _mCloseFileDOC();
            }
            catch
            {
                if (closeFile)
                    _mCloseFileDOC();
            }
        }
        public void _mCloseFileDOC()
        {
            try
            {
                if (xlApplicationWord != null)
                {
                    doc.Save();
                    Object saveChanges = Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges;
                    Object originalFormat = Type.Missing;
                    Object routeDocument = Type.Missing;

                    ((Word._Application)xlApplicationWord).Documents.Close(ref saveChanges, ref originalFormat, ref routeDocument);
                    ((Word._Application)xlApplicationWord).Quit(ref saveChanges, ref originalFormat, ref routeDocument);//<----- Line B

                    //Finally destroy all the objects.
                    if (xlApplicationWord != null)
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApplicationWord);
                    if (doc != null)
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);

                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    doc = null;
                    xlApplicationWord = null;
                }
            }
            catch
            { }
        }

        public void CopyExcelToWord(string FileExcel, string FileWord, bool Landscape)
        {
            try
            {
                _mOpendFileXLS(FileExcel, false);
                _mOpendFileDOC(FileWord, false);

                //Copy vùng dữ liệu.
                xlWorksheet.UsedRange.Cells.Select();
                xlWorksheet.UsedRange.Cells.Copy(misValue);

                //Paste vào word.
                doc = xlApplicationWord.Documents.Add(ref objMiss, ref objMiss, ref objMiss, ref objMiss);
                Word.Paragraph oPara1 = doc.Content.Paragraphs.Add(ref objMiss);
                //oPara1.Range.Font.Name = "Times New Roman";
                oPara1.Range.Paste();
                //oPara1.Range.PasteSpecial(ref IconIndex, ref Link, ref Placement, ref DisplayAsIcon, ref DataType, ref IconFileName, ref IconLabel);
                xlApplicationWord.DisplayAlerts = Microsoft.Office.Interop.Word.WdAlertLevel.wdAlertsAll;
                doc.PageSetup.PaperSize = Word.WdPaperSize.wdPaperA3;
                //object start = doc.Content.Start;
                //object end = doc.Content.End;

                Word.Range rng = doc.Tables[1].Range;
                //rng.Font.Size = 13; //định lại size
                rng.Font.Name = "Times New Roman";
                //rng.Font.Bold = 1;//in đậm
                //rng.Font.Italic = 1;//in nghiêng
                //rng.Font.Underline = Word.WdUnderline.wdUnderlineNone;//gạch chân
                //rng.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;//canh trái

                if (Landscape)
                    doc.PageSetup.Orientation = Microsoft.Office.Interop.Word.WdOrientation.wdOrientLandscape;
                else
                    doc.PageSetup.Orientation = Microsoft.Office.Interop.Word.WdOrientation.wdOrientPortrait;

                doc.PageSetup.TopMargin = 60;
                doc.PageSetup.LeftMargin = 60;
                doc.PageSetup.BottomMargin = 60;
                _mSaveFileDOC(FileWord, false, true);
                //Đóng file excel.
                _mCloseFileXLS();
            }
            catch
            {
                _mCloseFileXLS();
                _mCloseFileDOC();
            }
        }
    }
}
