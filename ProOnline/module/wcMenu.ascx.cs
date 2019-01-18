using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using ProOnline.Class;
using ProOnline.DataConnect;

namespace ProOnline.module
{
    public partial class wcMenu : System.Web.UI.UserControl
    {
        public string CurrentUser { get; set; }
        //DataTable _vdtbMenu;
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ProOnline.module.wcMenu));
            if (!IsPostBack)
            {
                //if (WEB_DLL.ntsSqlFunctions._IsRegis)
                //{

                    tblUser _vuser = Session.GetCurrentUser();
                    if (_vuser != null)
                        CurrentUser = _vuser.tenDangNhap;
                    //_mPermissMenu();

                    SqlFunction _vSql = new SqlFunction(Session.GetConnectionString1());
                    //_vdtbMenu = _vSql.GetData("SELECT IDMenu=functionName,textMenu=dienGiai, width, showEventpr_sd, positionpr_sd, styleFolder, folderIcon, IconName, OpenLinkUrl, iframeName, OnClientClick, IDMainMenu, pathFile, tabViewText FROM dbo.tblFunctions WHERE functionIDpr IN ( SELECT functionIDpr_sd FROM dbo.tblUserPermiss WHERE maNguoidungpr_sd=" + Session.GetCurrentUserID().ToString() + ")  ORDER BY functionName");
                    //Create Menu

                    loadMenu.InnerHtml = Server.HtmlDecode(stringMenu());
                    hienThiNamLamViec.InnerHtml = Session.GetNamSudung();
                    idUser.InnerHtml = Session.GetCurrentUser().tenDangNhap;
                    idMaDV.InnerHtml = Session.GetDonVi().maDonVi;
                    idTenDV.InnerHtml = Session.GetDonVi().tenDonVi;
                    //tblUser _vuser = Session.GetCurrentUser();
                    //if (_vuser != null)
                    //CurrentUser = _vuser.tenDangNhap;
                    SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString1());
                 
                    //idUser.InnerHtml = Session.GetCurrentUser().tenDangNhap + "|" + Session.GetNamSudung();
                    SqlFunction sqlFun2 = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                    cboKyBC1.DataSource = sqlFun2.GetData("SELECT bangChu,bangSo FROM dbo.tblDMKyBaocao where bangSo not in ('19','20') ", "tblDMKyBaocao");
                    cboKyBC1.DataTextField = "bangChu";
                    cboKyBC1.DataValueField = "bangSo";
                    cboKyBC1.DataBind();
                    cboKyBC1.SelectedValue = "14";
                    _txtTuNgay1.Text = HttpContext.Current.Session.GetNgayDauKy().Substring(3, 2) + "/" + HttpContext.Current.Session.GetNgayDauKy().Substring(0, 2) + "/" + HttpContext.Current.Session.GetNgayDauKy().Substring(6, 4);
                    _txtDenNgay1.Text = HttpContext.Current.Session.GetNgayCuoiKy().Substring(3, 2) + "/" + HttpContext.Current.Session.GetNgayCuoiKy().Substring(0, 2) + "/" + HttpContext.Current.Session.GetNgayCuoiKy().Substring(6, 4);
                    //load dự án
                    cboDuAnMNu.DataSource = sqlFun2.GetData("SELECT '0' AS sttDuAnpr,'' AS tenDuAn UNION ALL SELECT sttDuAnpr,tenDuAn FROM dbo.tblDuAn WHERE maDonVipr_sd=N'" + HttpContext.Current.Session.GetDonVi().maDonVi + "'", "tblDuAn");
                    cboDuAnMNu.DataTextField = "tenDuAn";
                    cboDuAnMNu.DataValueField = "sttDuAnpr";
                    cboDuAnMNu.DataBind();
                    cboDuAnMNu.SelectedValue = sqlFun2.GetOneStringField("SELECT sttDuAnpr_sd FROM dbo.tblDMDonVi WHERE maDonVipr=N'" + HttpContext.Current.Session.GetDonVi().maDonVi + "'");
            }
        }
        [AjaxPro.AjaxMethod]
        public string stringMenu()
        {
            string strQuery = "";
            SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString1());
            DataTable _dt = new DataTable();
            _dt = sqlFun.GetData("SELECT * FROM tblFunctions WHERE functionIDpr IN ( SELECT functionIDpr_sd FROM dbo.tblUserPermiss WHERE maNguoidungpr_sd=" + Session.GetCurrentUserID().ToString() + ")  order by CONVERT(DECIMAL(18,0), functionName)");
            //_dt = sqlFun.GetData("SELECT * FROM tblFunctions order by functionName ");

            DataView view1 = new DataView(_dt, "IDMainMenu<1", "", DataViewRowState.CurrentRows);
            DataTable _dtcap1 = view1.ToTable();
            foreach (DataRow dr in _dtcap1.Rows)
            {
                strQuery += @"<li><a href='" + (string.IsNullOrEmpty(dr["pathFile"].ToString()) ? "javascript://" : dr["pathFile"].ToString()) + @"' target='" + dr["positionpr_sd"] + "' class='drop'>" + dr["dienGiai"] + @"</a>   ";
                DataView view2 = new DataView(_dt, "IDMainMenu = " + (dr["functionName"].ToString() == "" ? "0" : Convert.ToDecimal( dr["functionName"].ToString()).ToString()) + "", "", DataViewRowState.CurrentRows);
                DataTable _dtcap2 = view2.ToTable();
                if (_dtcap2.Rows.Count > 0)
                {
                    strQuery += @"<div class='" + dr["showEventpr_sd"] + @"'>  ";
                    if (dr["showEventpr_sd"].ToString() == "dropdown_1column")
                    {
                        strQuery += @"<div class='col_1'>      
                                        <ul class='simple'>";
                        foreach (DataRow dr1 in _dtcap2.Rows)
                        {
                            strQuery += @"<li><a onclick ='abc(" + dr1["functionIDpr"].ToString() + @");' href='" + (string.IsNullOrEmpty(dr1["pathFile"].ToString()) ? "javascript://" : dr1["pathFile"].ToString()) + @"' target='" + dr1["positionpr_sd"] + "'>" + dr1["dienGiai"] + @"</a></li>";
                        }
                        strQuery += @"</ul> </div>";
                    }
                    else
                    {
                        foreach (DataRow dr1 in _dtcap2.Rows)
                        {
                            strQuery += @"<div class='col_1'>             
                                    <h3>" + dr1["dienGiai"] + @"</h3>
                                        <ul>";
                            DataView view3 = new DataView(_dt, "IDMainMenu = " + (dr1["functionName"].ToString() == "" ? "0" : Convert.ToDecimal(dr1["functionName"].ToString()).ToString()) + "", "", DataViewRowState.CurrentRows);
                            DataTable _dtcap3 = view3.ToTable();
                            foreach (DataRow dr2 in _dtcap3.Rows)
                            {
                                strQuery += @"<li><a onclick ='abc(" + dr2["functionIDpr"].ToString() + @");' href='" + (string.IsNullOrEmpty(dr2["pathFile"].ToString()) ? "javascript://" : dr2["pathFile"].ToString()) + @"' target='" + dr2["positionpr_sd"] + "'>" + dr2["dienGiai"] + @"</a></li>";
                            }
                            strQuery += @"</ul> </div>";
                        }
                    }
                }
                strQuery += @"</li>";
            }

            return strQuery;
        }
        protected void thayDoiMatKhau(object sender, EventArgs e)
        {
            Response.Redirect("/Hethong/ChangePassword.aspx");
        }
        protected void dangXuat(object sender, EventArgs e)
        {
            Response.Redirect("/Hethong/Logout.aspx");
        }
        public void capnhatngay(object sender, EventArgs e)
        {
            if (hdfDenNgayLoc.Value.ToString().Length == 10)
            {
                Session.setNamSudung(hdfDenNgayLoc.Value.Substring(6, 4));
                Session.SetSTTDuAn(cboDuAnMNu.SelectedValue);
                Session.SetNgayDauKy(hdfTuNgayLoc.Value);
                Session.SetNgayCuoiKy(hdfDenNgayLoc.Value);
                SqlFunction sqlFun2 = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                sqlFun2.ExeCuteNonQuery("UPDATE dbo.tblDMDonVi SET sttDuAnpr_sd=" + (HttpContext.Current.Session["ntsSTTDuAn"] == null ? "0" : HttpContext.Current.Session["ntsSTTDuAn"].ToString()) + " WHERE maDonVipr=N'" + HttpContext.Current.Session.GetDonVi().maDonVi + "'");
                Page.Response.Redirect(Page.Request.Url.ToString(), true);
            }
        }
        [AjaxPro.AjaxMethod]
        public string getSTTDuAn()
        {
            try
            {
                return HttpContext.Current.Session["ntsSTTDuAn"].ToString();
            }
            catch (Exception)
            {
                return "0";
            }
        }

        [AjaxPro.AjaxMethod]
        public void setSession(string s)
        {
            try
            {
                if (HttpContext.Current.Session["IDfunction"] == null)
                    HttpContext.Current.Session.Add("IDfunction", s.ToString());
                else
                    HttpContext.Current.Session["IDfunction"] = s;
            }
            catch (Exception)
            {
                HttpContext.Current.Session["IDfunction"] = s;
            }
        }
        public string getMessage()
        {
            SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            DataTable tab = new DataTable();
            decimal value = sqlFun.GetOneDecimalField("select convert(dec(18,0), count(nienDo)) from BCQT01CDT where maDonVipr_chuDauTu=N'" + HttpContext.Current.Session.GetDonVi().maDonVi + "' and trangThai=4");
            if (value > 0)
                return @" 
                       <a href='/quanly/guibaocaoquyettoan.aspx' title='Xem chi tiết báo cáo quyết toán bị từ chối'> <img src ='/images/message.png' />
                        <h6 style='position:absolute; margin-top:-28px;border-radius: 10px; margin-left:20px; font-size:13px; background-color:red;vertical-align:text-top, color:#FFFFFF; width:15px; height:15px; text-align:center;'>1</h6>
                        </a>";
            else
                return "";
        }
    }
}