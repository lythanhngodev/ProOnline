using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using WEB_DLL;
using ProOnline.Class;
using Obout.Grid;
using System.Data.SqlClient;
using AjaxControlToolkit;
using System.IO;
using System.Collections.Generic;
using System; 
namespace ProOnline.module
{
    public partial class tochuc : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SqlFunction sqlFunc = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ntsLibraryFunctions));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ProOnline.module.tochuc), this.Page);
            if (!IsPostBack)
            {
                winDowThemToChuc_maToChucpr_sd.DataSource  = sqlFunc.GetData("SELECT '' as maNhomToChucpr,'' as tenNhomToChuc UNION ALL SELECT maNhomToChucpr,tenNhomToChuc FROM dbo.tblDMNhomToChuc WHERE ngungSD='0'");
                winDowThemToChuc_maToChucpr_sd.DataTextField = "tenNhomToChuc";
                winDowThemToChuc_maToChucpr_sd.DataValueField = "maNhomToChucpr";
                winDowThemToChuc_maToChucpr_sd.DataBind(); 
            }
        }
        [AjaxPro.AjaxMethod]
        public string layMaToChuc()
        {
            SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            string _vSoHoSo = "";
            try
            {
                decimal _vNewKey = sqlFun.GetOneDecimalField("SELECT MAX(CONVERT(DECIMAL,RIGHT(maToChucpr,6))) FROM dbo.tblDMToChuc WHERE maDonVipr_sd='" + HttpContext.Current.Session.GetDonVi().maDonVi + "' AND maToChucpr LIKE N'" + HttpContext.Current.Session.GetDonVi().maDonVi + "-%' ") + 1;
                _vSoHoSo = HttpContext.Current.Session.GetDonVi().maDonVi + "-" + _vNewKey.ToString("000000");
            }
            catch { }
            return _vSoHoSo;
        }
        [AjaxPro.AjaxMethod]
        public string luuThongTinToChuc(object[] param)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(HttpContext.Current.Session.GetConnectionString2());
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand(@"INSERT INTO tblDMToChuc(maToChucpr,tenToChuc,ngungSD,maNhomToChucpr_sd,diaChi,maSoThue,dienThoai,Fax,Email,nguoiDaiDien,chucVuDD,nguoiLienHe,chucVuLH,dienThoaiLH,maDonVipr_sd,nguoiThaoTac,ngayThaotac,soTaiKhoan, noiMoTK,soDKKD)"
                    + " VALUES(@maToChucpr,@tenToChuc,@ngungSD,@maNhomToChucpr_sd,@diaChi,@maSoThue,@dienThoai,@Fax,@Email,@nguoiDaiDien,@chucVuDD,@nguoiLienHe,@chucVuLH,@dienThoaiLH,@maDonVipr_sd,@nguoiThaoTac,getdate(),@soTaiKhoan,@noiMoTK,@soDKKD)", sqlCon);
                cmd.Parameters.Add(new SqlParameter("@maToChucpr", param[0].ToString().Trim()));
                cmd.Parameters.Add(new SqlParameter("@tenToChuc", param[1].ToString()));
                cmd.Parameters.Add(new SqlParameter("@ngungSD", "0"));
                cmd.Parameters.Add(new SqlParameter("@maNhomToChucpr_sd", param[3].ToString()));
                cmd.Parameters.Add(new SqlParameter("@diaChi", param[4].ToString()));
                cmd.Parameters.Add(new SqlParameter("@maSoThue", param[5].ToString()));
                cmd.Parameters.Add(new SqlParameter("@dienThoai", param[6].ToString()));
                cmd.Parameters.Add(new SqlParameter("@Fax", param[7].ToString()));
                cmd.Parameters.Add(new SqlParameter("@Email", param[8].ToString()));
                cmd.Parameters.Add(new SqlParameter("@nguoiDaiDien", param[9].ToString()));
                cmd.Parameters.Add(new SqlParameter("@chucVuDD", param[10].ToString()));
                cmd.Parameters.Add(new SqlParameter("@nguoiLienHe", param[11].ToString()));
                cmd.Parameters.Add(new SqlParameter("@chucVuLH", param[12].ToString()));
                cmd.Parameters.Add(new SqlParameter("@dienThoaiLH", param[13].ToString()));
                cmd.Parameters.Add(new SqlParameter("@soTaiKhoan", param[14].ToString()));
                cmd.Parameters.Add(new SqlParameter("@noiMoTK", param[15].ToString()));
                cmd.Parameters.Add(new SqlParameter("@soDKKD", param[16].ToString()));
                cmd.Parameters.Add(new SqlParameter("@maDonVipr_sd", HttpContext.Current.Session.GetDonVi().maDonVi));
                cmd.Parameters.Add(new SqlParameter("@nguoiThaoTac", HttpContext.Current.Session.GetCurrentUserID()));
                cmd.ExecuteNonQuery();
                sqlCon.Close();
                return "1";
            }
            catch
            {
                return "0";

            }
        }

    }
}