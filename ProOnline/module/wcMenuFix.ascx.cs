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
    public partial class wcMenuFix : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ProOnline.module.wcMenuFix));
            if (!IsPostBack)
            { 
                    //Lấy thông tin đăng nhập
                    idUser.InnerHtml = Session.GetCurrentUser().tenDangNhap;
                    idMaDV.InnerHtml = Session.GetDonVi().maDonVi;
                    idTenDV.InnerHtml = Session.GetDonVi().tenDonVi;
                    //tblUser _vuser = Session.GetCurrentUser();
                    //if (_vuser != null)
                        //CurrentUser = _vuser.tenDangNhap;
                    SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString1());
                    DataTable _dtMenu = new DataTable();
                    _dtMenu = sqlFun.GetData("SELECT * FROM tblFunctions WHERE functionIDpr IN ( SELECT functionIDpr_sd FROM dbo.tblUserPermiss WHERE maNguoidungpr_sd=" + Session.GetCurrentUserID().ToString() + ")  order by functionName");
                    loadMenu.InnerHtml = Server.HtmlDecode(stringMenu_cha("", "", _dtMenu));
                    //idUser.InnerHtml = Session.GetCurrentUser().tenDangNhap + "|" + Session.GetNamSudung();
                    SqlFunction sqlFun2 = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                    cboKyBC1.DataSource = sqlFun2.GetData("SELECT bangChu,bangSo FROM dbo.tblDMKyBaocao where bangSo not in ('19','20') ", "tblDMKyBaocao");
                    cboKyBC1.DataTextField = "bangChu";
                    cboKyBC1.DataValueField = "bangSo";
                    cboKyBC1.DataBind();
                    cboKyBC1.SelectedValue = "14";
                    _txtTuNgay1.Text = HttpContext.Current.Session.GetNgayDauKy().Substring(3, 2) + "/" + HttpContext.Current.Session.GetNgayDauKy().Substring(0, 2) + "/" + HttpContext.Current.Session.GetNgayDauKy().Substring(6, 4);
                    _txtDenNgay1.Text = HttpContext.Current.Session.GetNgayCuoiKy().Substring(3, 2) + "/" + HttpContext.Current.Session.GetNgayCuoiKy().Substring(0, 2) + "/" + HttpContext.Current.Session.GetNgayCuoiKy().Substring(6, 4);
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
        private string stringMenu_cha(string functionName, string strQuery, DataTable _dt)
        {
            try
            {
                DataView view = new DataView(_dt, string.IsNullOrEmpty(functionName) ? "len(isnull(IDMainMenu,'')) = 0" : "IDMainMenu = " + functionName + " ", "", DataViewRowState.CurrentRows);
                DataTable _dtcha = view.ToTable();
                strQuery += @" <div class='nav green-black'>  <ul class='dropdown clear'>";
                foreach (DataRow dr in _dtcha.Rows)
                {
                    //kiểm tra nếu có con thì qua stringMenu_con
                    DataView view1 = new DataView(_dt, "IDMainMenu = '" + dr["functionName"] + "'", "", DataViewRowState.CurrentRows);
                    DataTable _dtcon = view1.ToTable();
                    string sub = "";
                    if (_dtcon.Rows.Count > 0)
                    {
                        sub = " class='sub' ";
                    }
                    strQuery += @"<li" + sub + "><a  onclick ='abc(" + dr["functionIDpr"].ToString() + @");' href='" + (string.IsNullOrEmpty(dr["pathFile"].ToString()) ? "javascript://" : dr["pathFile"].ToString()) + @"' target='" + dr["positionpr_sd"] + "'>" + dr["dienGiai"] + @"</a>   ";

                    if (_dtcon.Rows.Count > 0)
                    {
                        strQuery += stringMenu_con(dr["functionName"].ToString(), strQuery, _dt);
                    }
                    strQuery += @"</li> <li class='divider'></li>";
                }
                //thong tin đăng nhập
                strQuery += @"<li class='namTT'>" + HttpContext.Current.Session.GetNamSudung() + "</li><li class='login'><img id=\"thongTin\" src='/images/icon4.png' style='width:41px' title='Thông tin tài khoản'/></li><li class='login'> " + getMessage() + "</li>";
                strQuery += @"</ul></div>";
                return strQuery;
            }
            catch { return ""; }
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
        private string stringMenu_con(string functionName, string strQuery, DataTable _dt)
        {
            try
            {
                DataView view2 = new DataView(_dt, "IDMainMenu = '" + functionName + "'", "", DataViewRowState.CurrentRows);
                DataTable _dtTemp = view2.ToTable();
                strQuery = @"<ul> ";
                foreach (DataRow dr1 in _dtTemp.Rows)
                {
                    //kiểm tra nếu có con thì qua stringMenu_con
                    DataView view1 = new DataView(_dt, "IDMainMenu = '" + dr1["functionName"] + "'", "", DataViewRowState.CurrentRows);
                    DataTable _dtcon = view1.ToTable();
                    string sub = "";
                    if (_dtcon.Rows.Count > 0)
                    {
                        sub = " class='sub' ";
                    }
                    strQuery += @"<li" + sub + "><a   onclick ='abc(" + dr1["functionIDpr"].ToString() + @");' href='" + (string.IsNullOrEmpty(dr1["pathFile"].ToString()) ? "javascript://" : dr1["pathFile"].ToString()) + @"' target='" + dr1["positionpr_sd"] + "'>" + dr1["dienGiai"] + @"</a>";
                    if (_dtcon.Rows.Count > 0)
                    {
                        strQuery += stringMenu_con(dr1["functionName"].ToString(), strQuery, _dt);
                    }
                    strQuery += @"</li>";
                }
                strQuery += @"</ul>";
                return strQuery;
            }
            catch
            {
                return "";
            }
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
                Session.SetNgayDauKy(hdfTuNgayLoc.Value);
                Session.SetNgayCuoiKy(hdfDenNgayLoc.Value);
                Page.Response.Redirect(Page.Request.Url.ToString(), true);
            }
        }
    }
}