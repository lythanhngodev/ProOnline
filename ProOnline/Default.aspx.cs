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
using System.Web.SessionState;
using System.Xml.Linq;
using ProOnline.Class;
using ProOnline.DataConnect;
using WEB_DLL;
using System.Text;

namespace ProOnline
{
    public partial class _Default : System.Web.UI.Page
    {
        [AjaxPro.AjaxMethod]
        public void setSession(string s)
        {
            try
            {
                if (s != "")
                    Session.Add("IDfunction1", s.ToString());
                string t = Session["IDfunction1"].ToString();
            }
            catch
            {

            }
        }
        
        protected void setMaDonVi1(object sender, EventArgs e)
        {
            txtNamSuDung.Text = "2018";
            txtTenDangNhap.Text = "banqldatambinh1";
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ProOnline._Default));
            Session.SetConnectionString2(txtConn.Text);
            Session.SetConnectionString1(@"Data Source=.\SQLEXPRESS;Initial Catalog=UserProOnline_laptrinh;Integrated Security=True");
            Session.SetConnectionString2(@"Data Source=.\SQLEXPRESS;Initial Catalog=ProOnline_laptrinh2016;Integrated Security=True");
            //Session.SetConnectionString2(@"Data Source=115.79.35.88;Initial Catalog=ProOnline_LT2016;User ID=laptrinh;Password=@Laptrinh@123");
            //Session.SetConnectionString1(@"Data Source=115.79.35.88;Initial Catalog=UserProOnline_LT;User ID=laptrinh;Password=@Laptrinh@123");
            SqlFunction sqlFun = new SqlFunction(Session.GetConnectionString1());
            UsersDataContext db = new UsersDataContext();
            SqlFunction _vSql = new SqlFunction(Session.GetConnectionString2());
            IQueryable<tblDMDonvi> tblDMDonVi = from tdbDvi in db.tblDMDonvis
                                                where tdbDvi.maDonVi.ToLower() == sqlFun.GetOneStringField("SELECT CONVERT(nvarchar(18), maDonVipr_sd) FROM tblUsers WHERE tenDangNhap=N'" + txtTenDangNhap.Text + "'") //"94000083"//"94000087" //
                                                select tdbDvi;
            tblDMDonvi _vdbDonVi = tblDMDonVi.FirstOrDefault();
            Session.SetDonVi(_vdbDonVi);
            Session.SetDonViCapTren("Đơn vị cấp trên");
            Session.SetHienNgayInBC(true);
            Session.SetNgayInBC("01/01/" + (txtNamSuDung.Text == "" ? DateTime.Now.Year.ToString() : txtNamSuDung.Text));
            Session.SetMaChuong("1");
            Session.SetDiaDanh("Vĩnh Long");
            tblUser _vuser = new tblUser();
            _vuser.ngayThaotac = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            _vuser.nguoiThaoTac = sqlFun.GetOneDecimalField("SELECT CONVERT(dec(18,0), maNguoidungpr) FROM tblUsers WHERE tenDangNhap=N'" + txtTenDangNhap.Text + "'");
            _vuser.maNguoidungpr = sqlFun.GetOneDecimalField("SELECT CONVERT(dec(18,0), maNguoidungpr) FROM tblUsers WHERE tenDangNhap=N'" + txtTenDangNhap.Text + "'");
            _vuser.tenDangNhap = txtTenDangNhap.Text;

            //_vuser.sttNhanVienpr_sd = sqlFun.GetOneDecimalField("SELECT CONVERT(dec(18,0), sttNhanVienpr_sd) FROM tblUsers WHERE tenDangNhap=N'" + txtTenDangNhap.Text + "'");
            _vuser.sttPhongBanpr_sd = sqlFun.GetOneDecimalField("SELECT CONVERT(dec(18,0), sttPhongBanpr_sd) FROM tblUsers WHERE tenDangNhap=N'" + txtTenDangNhap.Text + "'");
            _vuser.idGrouppr_sd = sqlFun.GetOneStringField("SELECT CONVERT(nvarchar(250), idGrouppr_sd) FROM tblUsers WHERE tenDangNhap=N'" + txtTenDangNhap.Text + "'");
            Session.SetCurrentUser(_vuser);
            Session.SetNgayDauKy("01/01/" + (txtNamSuDung.Text == "" ? DateTime.Now.Year.ToString() : txtNamSuDung.Text));
            Session.SetNgayCuoiKy("31/12/" + (txtNamSuDung.Text == "" ? DateTime.Now.Year.ToString() : txtNamSuDung.Text));
            Session.SetCurrentDatetime(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
            Session.setNamSudung((txtNamSuDung.Text == "" ? DateTime.Now.Year.ToString() : txtNamSuDung.Text));
            try
            {
                Session.SetTenDonVi("Tên đơn vị báo cáo");
                txtConn.Text = @"Data Source=.\SQLEXPRESS;Initial Catalog=UserProOnline_laptrinh;Integrated Security=True";
                txtTenDangNhap.Text = HttpContext.Current.Session.GetCurrentUser().tenDangNhap;
                txtNamSuDung.Text = DateTime.Now.Year.ToString();
                value.InnerText = HttpContext.Current.Session.GetDonVi().maDonVi + " Năm thao tác " + HttpContext.Current.Session.GetNamSudung() + "</br>";
                value.InnerText += Session.GetConnectionString2().ToString();
                Response.Redirect("/quanly/thanhly_moi.aspx");
            }
            catch
            {
                value.InnerText = "Chưa cấp session";

            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Add("CurrentPermiss", "true;true;true;true;true;true;false;false;true");
            txtConn.Text = @"Data Source=.\SQLEXPRESS;Initial Catalog=UserProOnline_laptrinh;Integrated Security=True";
        }
    }
}
