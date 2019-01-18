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
using System.Data.SqlClient;
using WEB_DLL;
using GemBox.Spreadsheet;
using Spire.Doc;
using System.IO;
using System.Collections.Generic;
using ClosedXML.Excel;
using ProOnline.Class;
/// <summary>
/// Summary description for SqlFunction
/// </summary>
public class SqlFunction
{
    SqlConnection con; //Dùng để kết nối vào cơ sở dữ liệu
    SqlDataAdapter da;  //Là đối tượng trung gian lấy dữ liệu FIll vào trong các đối tương Data
    SqlCommand cmd;// Các xử lý truy vấn SQL thêm, xóa, sửa
    string _vConnectString = "";
    public static string _err ="";

    public SqlFunction(string ConnectString)
    {
        _vConnectString = ConnectString;
    
    }
     
    /// <summary>
    /// Kiểm tra cảnh báo theo danh mục cảnh báo, nếu kết quả là true thì hiện cảnh báo
    /// </summary>
    /// <param name="sttDMCanhBaopr"></param>
    /// <param name="maDonVipr"></param>
    /// <returns></returns>
    /// 
    public void ghiLogs(string noiDung, string chucNang)
    {
        SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
        sqlFun.ExeCuteNonQuery(@"INSERT INTO [dbo].[tblLoiThaoTac] ([maDonVipr_sd], [nguoiThaoTac], [ngayThaoTac], [moTaLoi], [chucNang], [column11], [column12], [column13], [column14])
	                SELECT N'" + HttpContext.Current.Session.GetDonVi().maDonVi + "', '" + HttpContext.Current.Session.GetCurrentUserID() + "', getdate(), N'" + noiDung.Replace("'", "") + "', N'" + chucNang + "', '', '', '', ''");
    }
    public bool checkCanhBao(string sttDMCanhBaopr,string maDonVipr)
    {
        DataTable dt = new DataTable();
        try
        {
            OpenConnect();
            da = new SqlDataAdapter("SELECT sttCanhBaopr_sd FROM dbo.tblCanhBao WHERE sttCanhBaopr_sd='" + sttDMCanhBaopr + "' AND maDonVipr_sd=N'" + maDonVipr + "'", con);
            da.Fill(dt);
        }
        catch { }
        finally { CloseConnect(); }
        if (dt.Rows.Count > 0)
            return true;
        return false;
    }
    public string XlsxReadToSheets(string path)
    {
        try
        {
 
            string xlsxFile = HttpContext.Current.Server.MapPath(path);
            string htmlFile = HttpContext.Current.Server.MapPath(path).Replace(".xlsx", ".html");
            SpreadsheetInfo.SetLicense("ETZX-IT28-33Q6-1HA2");
            ExcelFile ef = ExcelFile.Load(xlsxFile);
            ef.Save(htmlFile);
            return File.ReadAllText(htmlFile);
        }
        catch
        {
            return "";
        } 
    }
    public string XlsxToHtml(string path)
    {
        try
        { 
            string xlsxFile = HttpContext.Current.Server.MapPath(path);
            string htmlFile = HttpContext.Current.Server.MapPath(path).Replace(".xlsx", ".html");
            SpreadsheetInfo.SetLicense("ETZX-IT28-33Q6-1HA2");
            ExcelFile ef = ExcelFile.Load(xlsxFile); 
             ef.Save(htmlFile);
        }
        catch
        {
        }
        return path.Replace(".xlsx", ".html").Replace("~", "");
    }
    public string DocxToHtml(string path)
    {
        string docxFile = HttpContext.Current.Server.MapPath(path);
        string htmlFile = HttpContext.Current.Server.MapPath(path).Replace(".docx", ".html");
        Document document = new Document();
        document.LoadFromFile(docxFile);
        document.SaveToFile(htmlFile, FileFormat.Html);
        string resultHTML = File.ReadAllText(htmlFile);
        //Remove Warning version free
        resultHTML = resultHTML.Replace("Evaluation Warning: The document was created with Spire.Doc for .NET.", "");
        System.IO.File.WriteAllText(htmlFile, resultHTML);
        return path.Replace(".docx", ".html").Replace("~", ""); 
    }

    private void OpenConnect()
    {
        if (con == null)
            con = new SqlConnection(_vConnectString);
        con.ConnectionString = _vConnectString;
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
    }

    public void CloseConnect()
    {
        try
        {
            con.Close();
            con.Dispose();
        }
        catch { }
    }

    public DataTable GetData(string sql)
    {
        DataTable dt = new DataTable();
        try
        {
            OpenConnect();
            da = new SqlDataAdapter(sql, con);
            da.Fill(dt);
        }
        catch { }
        finally { CloseConnect(); }
        return dt;
    }

    public DataTable GetData(string sql, string tableName)
    {
        DataTable dt = new DataTable();
        try
        {
            OpenConnect();
            da = new SqlDataAdapter(sql, con);
            da.Fill(dt);
            dt.TableName = tableName;
        }
        catch { }
        finally { CloseConnect(); }
        return dt;
    } 
    public bool GetOneBoolField(string sql)
    {
        bool t = false;
        try
        {
            OpenConnect();
            cmd = new SqlCommand(sql, con);
            t = (bool)cmd.ExecuteScalar();
        }
        catch { t = false; }
        finally { CloseConnect(); }
        return t;
    }

    public decimal GetOneDecimalField(string sql)
    {
        decimal t = 0;
        try
        {
            OpenConnect();
            cmd = new SqlCommand(sql, con);
            t = (decimal)cmd.ExecuteScalar();
        }
        catch { t = 0; }
        finally { CloseConnect(); }
        return t;
    }

    public string GetOneStringField(string sql)
    {
        string t = "";
        try
        {
            OpenConnect();
            cmd = new SqlCommand(sql, con);
            t = (string)cmd.ExecuteScalar();
            if (t == null) t = "";
        }
        catch { t = ""; }
        finally { CloseConnect(); }
        return t;
    }

    public DateTime GetOneDateTimeField(string sql)
    {
        DateTime t;
        try
        {
            OpenConnect();
            cmd = new SqlCommand(sql, con);
            t = (DateTime)cmd.ExecuteScalar();
        }
        catch { t = DateTime.Now; }
        finally { CloseConnect(); }
        return t;
    }

    public bool ExeCuteNonQuery(string sql)
        {
        try
        {
            OpenConnect();
            cmd = new SqlCommand(sql, con);
            cmd.ExecuteNonQuery();
        }
        catch { return false; }
        finally { CloseConnect(); }
        return true;
    }

    public bool CheckHasRecord(string sql)
    {
        DataTable dt = new DataTable();
        dt = GetData(sql);
        return (dt != null && dt.Rows.Count > 0 ? true : false);
    }

    public string _mTaoMaCaBiet(string mathietBi, string maDonvi)
    {
        string _vMaThietBi = "";
        decimal _vNewKey = GetOneDecimalField("SELECT MAX(CONVERT(DECIMAL,RIGHT(maCaBiet,4))) FROM dbo.tblThietBi WHERE mathietBipr_sd=N'" + mathietBi 
                                     + "' AND maDonvipr_sd=N'" + maDonvi + "'") + 1;
        _vMaThietBi = mathietBi + "." + _vNewKey.ToString("0000");
        return _vMaThietBi;
    }
    //Hàm đổi số thành chữ
    #region
    public string _mDocSoLeThanhChu(decimal soTien)
    {
        
        string n = soTien.ToString();
        string phanChan = n.Split(',')[0].ToString();
        string phanLe = n.Split(',')[1].ToString();
        if (int.Parse(phanLe) == 0)
            return _mDocSoThanhChu(decimal.Parse(phanChan));
        else
            return _mDocSoThanhChu(decimal.Parse(phanChan)) + " lẻ " + _mDocSoThanhChu(decimal.Parse(phanLe)).ToUpper().Replace("MƯỜI", "mươi").Replace("ĐỒNG", "");
    }
    public string _mDocSoThanhChu(decimal soTien)
    {
        int i;
        string s = "";
        string n = soTien.ToString();
        int[] A = new int[n.Length + 1];
        for (i = n.Length; i > 0; i--)
        {
            if (n.Substring(n.Length - i, 1) != ",")
            {
                A[i] = Int32.Parse(n.Substring(n.Length - i, 1));
                s += docso(i, A[i], n) + hang(i, A[i], n);
            }
            else
            {
                s += " lẻ ";
            }
        }
        if (s != "")
        {
            string kiTuDau = s.Substring(0, 1);
            s = kiTuDau.ToUpper() + s.Substring(1) + "đồng";
        }
        return s;
    }

    string docso(int i, int x, string n)
    {
        string s = "";
        switch (x)
        {
            case 0: if (i % 3 == 0 && (n.Substring(n.Length - i + 1, 2) != "00"))
                    s = "không ";
                else s = "";
                break;
            case 1:
                if (i % 3 == 2)
                    s = "";
                else
                {
                    if (n.Length != i && i % 3 == 1 && n.Substring(n.Length - i - 1, 1) != "0" && Convert.ToDecimal(n.Substring(n.Length - i - 1, 1)) > 1)
                        s = "mốt ";
                    else
                        s = "một ";
                }
                break;
            case 2:
                s = "hai ";
                break;
            case 3:
                s = "ba ";
                break;
            case 4:
                s = "bốn ";
                break;
            case 5:
                if (n.Length != i && i % 3 == 1 && n.Substring(n.Length - i - 1, 1) != "0")
                    s = "lăm ";
                else
                    s = "năm ";
                break;
            case 6:
                s = "sáu ";
                break;
            case 7:
                s = "bảy ";
                break;
            case 8:
                s = "tám ";
                break;
            case 9:
                s = "chín ";
                break;
        }
        return s;
    }

    string hang(int i, int x, string n)
    {
        string s = "";
        int t = i % 3;
        switch (t)
        {
            case 0: if (n.Substring(n.Length - i, 3) != "000")
                    s = "trăm ";
                else s = "";
                break;
            case 1:
                if (i % 9 == 1)
                {
                    if (i - 1 == 0)
                        s = "";
                    else
                        s = "tỷ ";
                }
                else if (i % 6 == 1)
                    if (n.Length > 9 && n.Substring(n.Length - i - 2, 3) == "000")
                        s = "";
                    else
                        s = "triệu ";
                else
                    if (n.Length > 6 && n.Substring(n.Length - i - 2, 3) == "000")
                        s = "";
                    else
                        s = "ngàn ";
                break;
            case 2:
                if (x == 0 && n.Substring(n.Length - i + 1, 1) != "0")
                    s = "lẻ ";
                else
                    if (n.Substring(n.Length - i, 2) == "00")
                        s = "";
                    else
                    {
                        if (n.Substring(n.Length - i, 1) == "1")
                            s = "mười ";
                        else
                            s = "mươi ";
                    }
                break;
        }
        return s;
    }
    #endregion
}
