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
    public partial class thongtinduan_b1 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GridChonDuAn_uc.DataSource = null;
                GridChonDuAn_uc.DataBind();
            }
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ntsLibraryFunctions));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ProOnline.module.thongtinduan_b1), this.Page);
        }
        protected void GridChonDuAn_uc_OnRebind(object sender, EventArgs e)
        {
            try
            {
                SqlFunction sqlFunc = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                GridChonDuAn_uc.DataSource = sqlFunc.GetData("SELECT sttDuAnpr as sttDuAnpr_uc,maDuAn as maDuAn_uc,tenDuAn as tenDuAn_uc,tenChuDauTu_uc=(SELECT tenDonVi FROM tblDMDonVi WHERE maDonVipr=maDonVipr_chudautu),diaDiemXD as diaDiemXD_uc FROM dbo.tblDuAn WHERE tblDuAn.maDonVipr_sd=N'" + HttpContext.Current.Session.GetDonVi().maDonVi + "'");
                GridChonDuAn_uc.DataBind();
            }
            catch
            {
            }
        }
        [AjaxPro.AjaxMethod]
        public string DMNguonVonTheoDuAn_uc(string sttDuAn)
        {
            SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            string sql = @"SELECT DISTINCT (SELECT tenNguonVon +'; ' AS [text()] FROM dbo.tblDMNguonVon WHERE
            maNguonVonpr IN (
            SELECT maNguonVonpr_sd FROM dbo.tblNguonVonDA where sttDuAnpr_sd='" + sttDuAn + @"'
            UNION ALL
            SELECT maNguonVonpr_sd FROM dbo.tblDieuChinhMucDTuCT WHERE sttDCMucDTupr_sd IN 
            (SELECT sttDCMucDTupr FROM dbo.tblDieuChinhMucDTu WHERE sttDuAnpr_sd='" + sttDuAn + @"'))
            GROUP BY tenNguonVon ORDER BY tenNguonVon  FOR XML PATH(''))[tenNguonVon]  
            FROM dbo.tblDMNguonVon ";
            return sqlFun.GetOneStringField(sql);
        }
        [AjaxPro.AjaxMethod]
        public string layTongMucDauTu_uc(string sttDuAn)
        {
            SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            string sql = @"SELECT convert (nvarchar(20),ISNULL((SELECT SUM(giaTriVonQD) FROM dbo.tblNguonVonDA WHERE sttDuAnpr_sd=N'" + sttDuAn + @"')-
            (SELECT ISNULL(SUM(giaTriVonQDGiam),0) FROM dbo.tblDieuChinhMucDTuCT WHERE 
            sttDCMucDTupr_sd IN (SELECT sttDCMucDTupr FROM dbo.tblDieuChinhMucDTu WHERE  sttDuAnpr_sd =N'" + sttDuAn + @"'))
            +(SELECT ISNULL(SUM(giaTriVonQDTang),0) FROM dbo.tblDieuChinhMucDTuCT WHERE 
            sttDCMucDTupr_sd IN (SELECT sttDCMucDTupr FROM dbo.tblDieuChinhMucDTu WHERE sttDuAnpr_sd =N'" + sttDuAn + @"')),0))";
            return sqlFun.GetOneStringField(sql);
        }
        [AjaxPro.AjaxMethod]
        public DataTable layThongTinDuAn_uc(string sttDuAn)
        {
            SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            string sql = @"select N'Ngày khởi công - Hoàn thành:' +  ISNULL(CONVERT(VARCHAR(10),thoiGianKCongDuyet,103),'') + ' - ' + ISNULL(CONVERT(VARCHAR(10),thoiGianHThanhDuyet,103),'') + N'; Ngày khởi công - Hoàn thành thực tế: ' +ISNULL(CONVERT(VARCHAR(10),(SELECT TOP 1 tblBanGiaoDA.thoiGianKCongTH FROM dbo.tblBanGiaoDA WHERE sttDuAnpr_sd = sttDuAnpr),103),'') + '-' + ISNULL(CONVERT(VARCHAR(10),(SELECT TOP 1 tblBanGiaoDA.thoiGianHThanhTH FROM dbo.tblBanGiaoDA WHERE sttDuAnpr_sd = sttDuAnpr),103),'') as thoiGianThucHien_uc
           ,diaDiemXD as  diaDiemXD_uc,maDuAn as maDuAn_uc,tenDuAn as tenDuAn_uc,tenChuDauTu_uc=(SELECT tenDonVi FROM tblDMDonVi WHERE maDonVipr=maDonVipr_chudautu)   from tblDuAn
            where sttDuAnpr = N'" + sttDuAn + "'";
            return sqlFun.GetData(sql);
        }
        [AjaxPro.AjaxMethod]
        public string setSttDuAnMatDinh()
        {
            try
            {
                return HttpContext.Current.Session["ntsSTTDuAn"].ToString();
            }
            catch
            { return ""; }
        }
        [AjaxPro.AjaxMethod]
        public string checkDuyetQT(string sttDuAn)
        {
            SqlFunction sqlFunc = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            return sqlFunc.GetOneStringField("select convert(char(1), duyetQToan) from tblDuAn where sttDuAnpr='" + sttDuAn + "'");

        }
    }
}