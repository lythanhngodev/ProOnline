using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using Word = Microsoft.Office.Interop.Word;
using System.Data.OleDb;
namespace ProOnline.Class
{
    public class WordUltil
    {
        private Word.Application _app;
        Word.Document _doc;
        private object _pathFile;
        public WordUltil()
        {
        }
        public WordUltil(string vPath, bool vCreateApp, string path)
        {
            _pathFile = vPath;
            _app = new Word.Application();
            _app.Visible = vCreateApp;
            object ob = System.Reflection.Missing.Value;
            object notTrue = true; 
            _doc = _app.Documents.Add(ref _pathFile, ref ob, ref ob, ref ob);
            object destination = path;
            try
            {
                _doc.SaveAs(ref destination,ref ob,ref ob,ref ob,ref ob,ref ob,ref ob,ref ob,ref ob,ref ob,ref ob,ref ob,ref ob,ref ob,ref ob,ref ob);
            }
            catch
            {
            }
        }
        
        public void WriteFields(Dictionary<string, string> vValues)
        {
            foreach (Word.Field field in _doc.Fields)
            {
                string fieldName = field.Code.Text.Substring(11, field.Code.Text.IndexOf("\\") - 12).Trim();
                if (vValues.ContainsKey(fieldName))
                {
                    field.Select();
                    _app.Selection.TypeText(vValues[fieldName]);
                }
            }
        }
        public void stop()
        {
            try
            {
                object notTrue = true;
                object ob = System.Reflection.Missing.Value;
                _doc.Application.Quit(ref notTrue, ref ob, ref ob);
                _app.Quit(ref notTrue, ref ob, ref ob);
            }
            catch
            {
            }
        }
        public void WriteTable(DataTable vDataTable, int vIndexTable)
        {
            Word.Table tbl = _doc.Tables[vIndexTable];
            int lenrow = vDataTable.Rows.Count;
            int lencol = vDataTable.Columns.Count;
            for (int i = 0; i < lenrow; ++i)
            {
                object ob = System.Reflection.Missing.Value;
                tbl.Rows.Add(ref ob);
                for (int j = 0; j < lencol; ++j)
                {
                    tbl.Cell(i + 2, j + 1).Range.Text = vDataTable.Rows[i][j].ToString();
                }
            }
        }
    }
}
