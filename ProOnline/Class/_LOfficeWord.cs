using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Net;
using System.Threading;
using Word = Microsoft.Office.Interop.Word;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Word;
using System.Globalization;

namespace ProOnline.Class
{
    public class _LOfficeWord
    {
        public _LOfficeWord()
        {

        }

        #region Các biến cần đọc dữ liệu.
        public Word.Application xlApplication;
        public Word.Document doc;
        public Word.Bookmarks bookmarks = null;
        public Word.Bookmark myBookmark = null;
        public Word.Range bookmarkRange = null;
        public object objMiss = System.Reflection.Missing.Value;
        public object objEndOfDocFlag = "\\endofdoc"; /* \endofdoc is a predefined bookmark */
        Word.WdExportFormat paramExportFormat = Word.WdExportFormat.wdExportFormatPDF;
        Word.WdExportOptimizeFor paramExportOptimizeFor = Word.WdExportOptimizeFor.wdExportOptimizeForPrint;
        Word.WdExportRange paramWdExportRange = Word.WdExportRange.wdExportAllDocument;
        Word.WdExportItem paramWdExportItem = Word.WdExportItem.wdExportDocumentContent;
        Word.WdExportCreateBookmarks paramWdExportCreateBookmarks = Word.WdExportCreateBookmarks.wdExportCreateWordBookmarks;
        bool paramOpenAfterExport = false;
        bool paramIncludeDocProps = true;
        bool paramKeepIRM = true;
        bool paramDocStruc = true;
        bool paramBitmapMissingFont = true;
        bool paramUseISO19005 = true;
        int paramFromPage = 1;
        int paramToPage = 10;
        #endregion

        /// <summary>
        /// Mở tập tin word. đọc tập tin.
        /// </summary>
        /// <param name="fileOpen">Đường dẫn tập tin cần đọc.</param>
        /// <param name="Visible">True là hiển thị tập tin vừa đọc, fale là đọc với dạng ẩn tập tin.</param>
        public void OpendFileDOC(object fileOpen, bool visible)
        {
            try
            {
                doc = new Word.Document();
                xlApplication = new Word.Application();
                xlApplication.Visible = visible;

                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

                object missing = System.Type.Missing;
                doc = xlApplication.Documents.Open(ref fileOpen, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
                //doc = xlApplication.Documents.Open(fileOpen, ReadOnly: false, Visible: Visible);
            }
            catch 
            { }
        }
        /// <summary>
        /// Đóng tập tin word.
        /// </summary>
        public void CloseFileDOC()
        {
            try
            {
                if (xlApplication != null)
                {
                    Object saveChanges = Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges;
                    Object originalFormat = Type.Missing;
                    Object routeDocument = Type.Missing;

                    ((Word._Application)xlApplication).Documents.Close(ref saveChanges, ref originalFormat, ref routeDocument);
                    ((Word._Application)xlApplication).Quit(ref saveChanges, ref originalFormat, ref routeDocument);//<----- Line B

                    //Finally destroy all the objects.
                    if (xlApplication != null)
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApplication);
                    if (doc != null)
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);
                    xlApplication = null;
                    doc = null;
                }
            }
            catch
            { }
        }
        /// <summary>
        /// Save tập tin word.
        /// </summary>
        /// <param name="fileSave">Đường dẫn tập tin word, bằng rổng là save mặt định.</param>
        /// <param name="checkDelete">True là xóa tập tin.</param>
        /// <param name="closeFile">True là đóng tập tin.</param>
        public void SaveFileDOC(string fileSave, bool checkDelete, bool closeFile)
        {
            try
            {
                if (checkDelete)
                    if (System.IO.File.Exists(fileSave))
                        System.IO.File.Delete(fileSave);
                if (xlApplication != null && fileSave == "")
                    doc.Save();
                if (xlApplication != null && fileSave != "")
                {
                    object fileNameSave = fileSave;
                    object oMissing = System.Reflection.Missing.Value;
                    //Save tập tin word.

                    doc.SaveAs(ref fileNameSave, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref objMiss);

                    //if (_vfileSave.ToLower().IndexOf(".docx") != -1)
                    //    doc.SaveAs(ref _vFileNameSave, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocumentDefault, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                }
                if (closeFile) //Đóng tập tin lại
                    CloseFileDOC();
            }
            catch 
            {
                if (closeFile)
                    CloseFileDOC();
            }
        }
        /// <summary>
        /// Save tập tin HTML.
        /// </summary>
        /// <param name="fileSave">Đường dẫn tập tin word, bằng rổng là save mặt định.</param>
        /// <param name="checkDelete">True là xóa tập tin.</param>
        /// <param name="closeFile">True là đóng tập tin.</param>
        public void SaveFileHTML(string fileSave, bool checkDelete, bool closeFile)
        {
            try
            {
                if (checkDelete)
                    if (System.IO.File.Exists(fileSave))
                        System.IO.File.Delete(fileSave);
                if (xlApplication != null && fileSave == "")
                    doc.Save();
                if (xlApplication != null && fileSave != "")
                {
                    object fileNameSave = fileSave;
                    object oMissing = System.Reflection.Missing.Value;
                    object Format = (int)Word.WdSaveFormat.wdFormatFilteredHTML;
                    //Save tập tin HTML.
                    doc.SaveAs(ref fileNameSave, ref Format, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                }
                if (closeFile) //Đóng tập tin lại
                    CloseFileDOC();
            }
            catch 
            {
                if (closeFile)
                    CloseFileDOC();
            }
        }

        

        public void SaveFileDocToPdf(string pathFilePdf, string fileDoc, bool closeFile)
        {
            try
            {
                OpendFileDOC(fileDoc, false);

                if (doc != null)
                {
                    if (fileDoc.ToLower().IndexOf(".docx") != -1)
                        doc.ExportAsFixedFormat(pathFilePdf, paramExportFormat, paramOpenAfterExport, paramExportOptimizeFor, paramWdExportRange, paramFromPage, paramToPage, paramWdExportItem, paramIncludeDocProps, paramKeepIRM, paramWdExportCreateBookmarks, paramDocStruc, paramBitmapMissingFont, paramUseISO19005, ref objMiss);
                    //else
                    //    xlWorkbook.SaveAs(fileExcel, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                    //xlWorkbook.Saved = true;
                }
                if (closeFile) //Đóng tập tin lại
                    CloseFileDOC();
            }
            catch
            {
                if (closeFile)
                    CloseFileDOC();
            }
        }


        /// <summary>
        /// Lưu tập tin word mặt định.
        /// </summary>
        public void SaveFileDOC()
        {
            try
            {
                if (xlApplication != null)
                    doc.Save();
            }
            catch 
            { }
        }
        /// <summary>
        /// Thay thế nội dung của bảng dữ liệu trong word.
        /// </summary>
        /// <param name="fieldName">Biến cần thay đổi</param>
        /// <param name="values">Nội dung cần thay đổi</param>
        public void WriteDataFields(string fieldName, string values)
        {
            try
            {
                Microsoft.Office.Interop.Word.Range rng = doc.Content;
                for (int i = 0; i < rng.Paragraphs.Count; i++)
                {
                    if (rng.Paragraphs[i + 1].Range.Text.IndexOf(fieldName) != -1)
                        rng.Paragraphs[i + 1].Range.Text = rng.Paragraphs[i + 1].Range.Text.Replace(fieldName, values);
                }
            }
            catch
            { }
        }
        /// <summary>
        /// Ghi dữ liệu vào bảng
        /// </summary>
        /// <param name="indexTable">Bảng dữ liệu được tính bắt đầu từ 1 trở đi.</param>
        /// <param name="rows">Dòng dữ liệu được tính từ 1</param>
        /// <param name="columns">Cột dữ liệu được tính từ 1</param>
        /// <param name="values">Chuỗi dữ liệu cần ghi</param>
        public void WriteDataTables(int indexTable, int rows, int columns, string values)
        {
            try
            {
                Microsoft.Office.Interop.Word.Table dt = doc.Tables[indexTable];
                dt.Cell(rows, columns).Range.Text = values;
            }
            catch
            { }
        }
        /// <summary>
        /// Thêm một dòng vào bảng tại vị trí cuối.
        /// </summary>
        /// <param name="indexTable">Bảng dữ liệu được tính bắt đầu từ 1 trở đi.</param>
        public void InsertRowsInTables(int indexTable)
        {
            try
            {
                Microsoft.Office.Interop.Word.Table dt = doc.Tables[indexTable];
                object beforeRow = Type.Missing;
                dt.Rows.Add(ref beforeRow);
            }
            catch
            { }
        }
        /// <summary>
        /// Thêm một dòng vào bảng tại vị trí xác định
        /// </summary>
        /// <param name="indexTable">Bảng dữ liệu được tính bắt đầu từ 1 trở đi.</param>
        /// <param name="beforeRow">Dòng được tính bắc đầu từ 1</param>
        public void InsertRowsInTables(int indexTable, int beforeRow)
        {
            try
            {
                Microsoft.Office.Interop.Word.Table dt = doc.Tables[indexTable];
                Word.Range Range = dt.Range;
                object beforeRows = Range.Rows[beforeRow];
                dt.Rows.Add(ref beforeRows);
            }
            catch
            { }
        }
        /// <summary>
        /// Thêm dữ liệu ở cuối nội dung của word.
        /// </summary>
        /// <param name="values">Dữ liệu thêm</param>
        public void InsertRowsAfter(string values)
        {
            try
            {
                Microsoft.Office.Interop.Word.Range objWordRng = doc.Bookmarks.get_Item(ref objEndOfDocFlag).Range; //go to end of document
                objWordRng.InsertParagraphAfter(); //put enter in document
                objWordRng.InsertAfter(values);
            }
            catch
            { }
        }
        /// <summary>
        /// In dữ liệu
        /// </summary>
        public void PrintOut()
        {
            try
            {
                object yes = true;
                object no = false;
                object missing = System.Reflection.Missing.Value;

                doc.PrintOut(ref missing, ref missing, ref  missing, ref  missing, ref  missing, ref missing, ref missing, ref  missing, ref  missing, ref  missing, ref missing, ref missing, ref missing, ref  missing, ref  missing, ref  missing, ref missing, ref missing);
                // http://msdn.microsoft.com/en-us/library/b9f0ke7y

                //doc.PrintOut(true, false, Word.WdPrintOutRange.wdPrintAllDocument,
                //    Item: Word.WdPrintOutItem.wdPrintDocumentContent, Copies: "1", Pages: "",
                //    PageType: Word.WdPrintOutPages.wdPrintAllPages, PrintToFile: false, Collate: true,
                //    ManualDuplexPrint: false);
            }
            catch
            { }
        }
        //Mới nghiên cứu bổ sung các hàm. (16/03/2016)
        public void MergeCells(int tableIndex, int RowsStart, int ColumnsStart, int RowsEnd, int ColumnsEnd)
        {
            try
            {
                doc.Tables[tableIndex].Cell(RowsStart, ColumnsStart).Merge(doc.Tables[tableIndex].Cell(RowsEnd, ColumnsEnd));
            }
            catch
            { }
        }
        public void SetFontSizeTable(int indexTable, string font, int size, int bold, int italic, Word.WdUnderline underline, Word.WdParagraphAlignment alignment)
        {
            try
            {
                Word.Range rng = doc.Tables[indexTable].Range;
                rng.Font.Size = size;
                rng.Font.Name = font;
                rng.Font.Bold = bold;
                rng.Font.Italic = italic;
                rng.Font.Underline = underline;
                rng.ParagraphFormat.Alignment = alignment;
            }
            catch
            { }
        }
        public void SetFontSizeRowsTable(int indexTable, int rows, string font, int size, int bold, int italic, Word.WdUnderline underline, Word.WdParagraphAlignment alignment)
        {
            try
            {
                Word.Range rng = doc.Tables[indexTable].Range.Rows[rows].Range;
                rng.Font.Size = size;
                rng.Font.Name = font;
                rng.Font.Bold = bold;
                rng.Font.Italic = italic;
                rng.Font.Underline = underline;
                rng.ParagraphFormat.Alignment = alignment;
            }
            catch
            { }
        }
        public void SetFontSizeCellTable(int indexTable, int rows, int cell, string font, int size, int bold, int italic, Word.WdUnderline underline, Word.WdParagraphAlignment alignment)
        {
            try
            {
                Word.Range rng = doc.Tables[indexTable].Range.Rows[rows].Cells[cell].Range;
                rng.Font.Size = size;
                rng.Font.Name = font;
                rng.Font.Bold = bold;
                rng.Font.Italic = italic;
                rng.Font.Underline = underline;
                rng.ParagraphFormat.Alignment = alignment;
            }
            catch
            { }
        }
        public void DeleteRowsTable(int indexTable, int beforeRows)
        {
            try
            {
                Microsoft.Office.Interop.Word.Table dt = doc.Tables[indexTable];
                Word.Range Ranges = dt.Range;
                Ranges.Rows[beforeRows].Delete();
            }
            catch
            { }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathOld">Đường dẫn vật lý đến ổ đĩa. Ví dụ: C:\ABC.xls</param>
        /// <param name="urlFolderNew">Đường dẫn vật lý tới thư mục cần copy file vào. Ví dụ: D:\newfolder</param>
        /// <param name="fileNameNew">Tên file mới có phần mở rộng. Ví dụ ABC.xls</param>
        /// <param name="deleteAllFileInFolderNew">True nếu muốn xóa hết file trong thư mục mới</param>
        public void CopyFiles(string pathFileOld, string urlFolderNew, string fileNameNew, bool deleteAllFileInFolderNew)
        {
            string urlFolder = urlFolderNew;
            if (!System.IO.Directory.Exists(urlFolder))
            {
                System.IO.Directory.CreateDirectory(urlFolder);
            }
            if (deleteAllFileInFolderNew == true)
            {
                DirectoryInfo di = new DirectoryInfo(urlFolder);
                FileInfo[] rgFiles = di.GetFiles();
                foreach (FileInfo fi in rgFiles)
                {
                    fi.Delete();
                }
            }
            System.IO.File.Copy(pathFileOld, urlFolderNew + "/" + fileNameNew);
        }
        /// <summary>
        /// Chuyển số kiểu Interger sang kiểu số la mã
        /// </summary>
        /// <param name="num">Tham số kiểu Integer</param>
        /// <returns>string</returns>
        public string IntToRoman(int num)
        {
            string[] thou = { "", "M", "MM", "MMM" };
            string[] hun = { "", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM" };
            string[] ten = { "", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC" };
            string[] ones = { "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" };
            string roman = "";
            roman += thou[(int)(num / 1000) % 10];
            roman += hun[(int)(num / 100) % 10];
            roman += ten[(int)(num / 10) % 10];
            roman += ones[num % 10];

            return roman;
        }
        /// <summary>
        /// Chuyển kiểu số sang kiểu chuỗi
        /// </summary>
        /// <param name="num">Kiểu số Integer</param>
        /// <param name="toUpper">True nếu muốn trả về in hoa. False nếu muốn trả về in thường</param>
        /// <returns>string</returns>
        public string IntToAlphabet(int num, bool toUpper)
        {
            if (toUpper == true)
            {
                return ((char)(num + 64)).ToString();
            }
            else
            {
                return ((char)(num + 96)).ToString();
            }
        }
    }
}
