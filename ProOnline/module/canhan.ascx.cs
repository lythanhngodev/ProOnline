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
    public partial class canhan : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SqlFunction sqlFunc = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ntsLibraryFunctions));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ProOnline.module.canhan), this.Page);
            if (!IsPostBack)
            {
                winDowThemCaNhan_maToChucpr_sd.DataSource = sqlFunc.GetData(@"SELECT '' maToChucpr,'' tenToChuc union all SELECT maToChucpr,tenToChuc FROM dbo.tblDMToChuc WHERE ngungSD=0 and maDonVipr_sd='" + HttpContext.Current.Session.GetDonVi().maDonVi + "'");
                winDowThemCaNhan_maToChucpr_sd.DataTextField = "tenToChuc";
                winDowThemCaNhan_maToChucpr_sd.DataValueField = "maToChucpr";
                winDowThemCaNhan_maToChucpr_sd.DataBind();

                winDowThemCaNhan_gioiTinhCaNhan.DataSource = sqlFunc.GetData("SELECT gioiTinh=N'Nam' UNION ALL SELECT gioiTinh=N'Nữ'");
                winDowThemCaNhan_gioiTinhCaNhan.DataTextField = "gioiTinh";
                winDowThemCaNhan_gioiTinhCaNhan.DataValueField = "gioiTinh";
                winDowThemCaNhan_gioiTinhCaNhan.DataBind();
            }
        }
        [AjaxPro.AjaxMethod]
        public string layMaCaNhan()
        {
            SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            string _vSoHoSo = "";
            try
            {
                decimal _vNewKey = sqlFun.GetOneDecimalField("SELECT MAX(CONVERT(DECIMAL,RIGHT(maCaNhanpr ,6))) FROM dbo.tblDMCaNhan WHERE maCaNhanpr LIKE N'" + HttpContext.Current.Session.GetDonVi().maDonVi + "-%' ") + 1;
                _vSoHoSo = HttpContext.Current.Session.GetDonVi().maDonVi + "-" + _vNewKey.ToString("000000");
            }
            catch { }
            return _vSoHoSo;
        }
        [AjaxPro.AjaxMethod]
        public string luuThongTinCaNhan( object[] param)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(HttpContext.Current.Session.GetConnectionString2());
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO tblDMCaNhan(dienThoai,chucVu,maCaNhanpr,tenCaNhan,ngungSD,maToChucpr_sd,gioiTinh,ngaySinh,diaChiTTru,CMND,ngayCap,noiCap,maDonVipr_sd,nguoiThaoTac,ngayThaotac)"
                    + " VALUES(@dienThoai,@chucVu,@maCaNhanpr,@tenCaNhan,@ngungSD,@maToChucpr_sd,@gioiTinh,@ngaySinh,@diaChiTTru,@CMND,@ngayCap,@noiCap,@maDonVipr_sd,@nguoiThaoTac,@ngayThaotac)", sqlCon);
                cmd.Parameters.Add(new SqlParameter("@maCaNhanpr", param[0].ToString().Trim()));
                cmd.Parameters.Add(new SqlParameter("@tenCaNhan", param[1].ToString()));
                cmd.Parameters.Add(new SqlParameter("@ngungSD", param[2].ToString()));
                cmd.Parameters.Add(new SqlParameter("@maToChucpr_sd", param[3].ToString()));
                cmd.Parameters.Add(new SqlParameter("@gioiTinh", param[4].ToString()));
                cmd.Parameters.Add(new SqlParameter("@ngaySinh", param[5].ToString()));
                cmd.Parameters.Add(new SqlParameter("@diaChiTTru", param[6].ToString()));
                cmd.Parameters.Add(new SqlParameter("@CMND", param[7].ToString()));
                cmd.Parameters.Add(new SqlParameter("@maDonVipr_sd", HttpContext.Current.Session.GetDonVi().maDonVi));
                cmd.Parameters.Add(new SqlParameter("@nguoiThaoTac", HttpContext.Current.Session.GetCurrentUserID()));//hinhThucNhap
                cmd.Parameters.Add(new SqlParameter("@ngayThaotac", HttpContext.Current.Session.GetCurrentDatetimeMMddyyyy("MM/dd/yyyy")));
                if (!string.IsNullOrEmpty(param[8].ToString()))
                {
                    cmd.Parameters.Add(new SqlParameter("@ngayCap", DateTime.Parse(param[8].ToString().Trim().Split(' ')[0], System.Globalization.CultureInfo.GetCultureInfo("en-gb"))));
                }
                else
                {
                    cmd.Parameters.Add(new SqlParameter("@ngayCap", DBNull.Value));
                }
                cmd.Parameters.Add(new SqlParameter("@noiCap", param[9].ToString()));
                cmd.Parameters.Add(new SqlParameter("@chucVu", param[10].ToString()));
                cmd.Parameters.Add(new SqlParameter("@dienThoai", param[11].ToString()));
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