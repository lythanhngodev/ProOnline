using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using ProOnline.Class;
using Obout.Grid;
using System.Data.SqlClient;
using AjaxControlToolkit;
using System.IO;
using WEB_DLL;

namespace ProOnline.module
{
    public partial class baocaogiamsatdautu : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ProOnline.Class.ntsLibraryFunctions));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ProOnline.module.baocaogiamsatdautu));
            if (!IsPostBack) {
                Grid_ChuTruongDauTu.DataSource = null;
                Grid_ChuTruongDauTu.DataBind();
                Grid1_duAn.DataSource = null;
                Grid1_duAn.DataBind();
            }
        }
        protected void Grid_ChuTruongDauTu_OnRebind(object sender, EventArgs e)
        {
            string sttBCGSDauTupr="",maGiaiDoanpr_sd="";
             if (Session["sttBCGSDauTupr"] != null)
            {
                sttBCGSDauTupr = Session["sttBCGSDauTupr"].ToString();
            }
            if (Session["maGiaiDoanpr_sd"] != null)
            {
                maGiaiDoanpr_sd = Session["maGiaiDoanpr_sd"].ToString();
            }
            SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            string sql = @"SELECT 
                                sttDuAnpr_sd,sttBCGSDauTuCTpr
                                ,maDuAn=(SELECT maDuAn FROM dbo.tblDuAn WHERE sttDuAnpr=sttDuAnpr_sd)
                                ,tenDuAn=(SELECT tenDuAn FROM dbo.tblDuAn WHERE sttDuAnpr=sttDuAnpr_sd)
                                ,chuDauTu=(SELECT chuDauTu=(SELECT tenDonVi FROM dbo.tblDMDonVi WHERE maDonVipr=maDonVipr_chudautu) FROM dbo.tblDuAn WHERE sttDuAnpr=sttDuAnpr_sd)
                                ,nhomDuAn = (select tenNhomDA from tblDMNhomDA where maNhomDApr = (select maNhomDApr_sd FROM dbo.tblDuAn WHERE sttDuAnpr=sttDuAnpr_sd))
                                ,ngayPheDuyet= (SELECT ngayQuyetDinh fROM dbo.tblDuAn where  sttDuAnpr=sttDuAnpr_sd)
                                ,thoiGian=(SELECT thoiGian=thoiGianKCongDuyet+'-'+ thoiGianHThanhDuyet FROM dbo.tblDuAn where  sttDuAnpr=sttDuAnpr_sd)
                                ,nguonVon = (SELECT nguonVon=(SELECT nhomBCPL1_GSDT+';' FROM dbo.tblDMNguonLDTC WHERE maNguonLDTCpr IN 
                                (SELECT (SELECT maNguonLDTCpr_sd FROM dbo.tblDMNguonVon WHERE maNguonVonpr=maNguonVonpr_sd)  
                                FROM dbo.tblNguonVonDA WHERE sttDuAnpr_sd=sttDuAnpr) FOR XML PATH('')) FROM dbo.tblDuAn where  sttDuAnpr=sttDuAnpr_sd)
                                ,loaiVanBan=(SELECT (SELECT tenLoaiVBan FROM dbo.tblDMLoaiVanBan WHERE maLoaiVBanpr=maLoaiVBanpr_sd) FROM dbo.tblVanBanDA WHERE sttVBDApr=sttVBDApr_sd)
                                from tblBCGSDauTuCT where sttBuocThaoTac=N'" + maGiaiDoanpr_sd + "' and sttBCGSDauTupr_sd=N'" + sttBCGSDauTupr + "'";
            Grid_ChuTruongDauTu.DataSource = sqlFun.GetData(sql);
            Grid_ChuTruongDauTu.DataBind();
        }
        
        protected void Grid1_duAn_OnRebind(object sender, EventArgs e)
        {
            string maGiaiDoanpr_sd = "", sttBCGSDauTupr = "";
            SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            if (Session["sttBCGSDauTupr"] != null)
            {
                sttBCGSDauTupr = Session["sttBCGSDauTupr"].ToString();
            }
            if (Session["maGiaiDoanpr_sd"] != null)
            {
                if (Session["maGiaiDoanpr_sd"].ToString() == "2")
                    maGiaiDoanpr_sd = "I";
                else
                    if (Session["maGiaiDoanpr_sd"].ToString() == "3")
                    maGiaiDoanpr_sd = "II";
                else
                    maGiaiDoanpr_sd = Session["maGiaiDoanpr_sd"].ToString();
            }
            string sql = "";
            if (maGiaiDoanpr_sd == "4")
            {
                sql = @"SELECT sttDuAnpr_sd,sttVBDApr=0,maDuAn,tenDuAn,chuDauTu,diaDiemXD
,loaiVanBan=(CASE WHEN CONVERT(SMALLDATETIME,temp.thangKC+'/'+temp.ngayKC+'/'+temp.namKC)<(select denNgay from tblBCGSDauTu where sttBCGSDauTupr=N'" + sttBCGSDauTupr + @"')
				 THEN N'Số dự án chuyển tiếp' ELSE N'Số dự án khởi công trong kỳ' END)	

FROM(
SELECT sttDuAnpr_sd=sttDuAnpr,sttVBDApr=0,maDuAn,tenDuAn
,chuDauTu=(SELECT tenDonVi FROM dbo.tblDMDonVi WHERE maDonVipr=maDonVipr_chudautu)
,diaDiemXD  
,ngayKC=(CASE WHEN LEN(thoiGianKCongDuyet)=4  OR LEN(thoiGianKCongDuyet)=7 THEN '01' ELSE SUBSTRING(thoiGianKCongDuyet,1,2)END) 
,thangKC=(CASE WHEN LEN(thoiGianKCongDuyet)=4 THEN '01'
			   WHEN LEN(thoiGianKCongDuyet)=7 THEN SUBSTRING(thoiGianKCongDuyet,0,2)
			   ELSE SUBSTRING(thoiGianKCongDuyet,4,2) END) 
,namKC=(CASE WHEN LEN(thoiGianKCongDuyet)=4 THEN thoiGianKCongDuyet
			   WHEN LEN(thoiGianKCongDuyet)=7 THEN SUBSTRING(thoiGianKCongDuyet,3,4)
			   ELSE SUBSTRING(thoiGianKCongDuyet,7,4)
			   END) 
FROM dbo.tblDuAn WHERE chinhThuc=1 and maDonVipr_sd='"+HttpContext.Current.Session.GetConnectionString2()+@"') AS temp WHERE 
CONVERT(SMALLDATETIME,temp.thangKC+'/'+temp.ngayKC+'/'+temp.namKC)<(select denNgay from tblBCGSDauTu where sttBCGSDauTupr=N'" + sttBCGSDauTupr + @"')";
            }
            else
                if (maGiaiDoanpr_sd == "5")
                { 
                    sql=@"SELECT sttDuAnpr_sd=sttDuAnpr,sttVBDApr=0,maDuAn,tenDuAn
                                ,chuDauTu=(SELECT tenDonVi FROM dbo.tblDMDonVi WHERE maDonVipr=maDonVipr_chudautu)
                                ,diaDiemXD
                                ,loaiVanBan=N''
                                FROM dbo.tblDuAn WHERE chinhThuc=1 and maDonVipr_sd='"+HttpContext.Current.Session.GetConnectionString2()+"'";
                }
                else
            sql = @"SELECT 
                                sttDuAnpr_sd,sttVBDApr
                                ,maDuAn=(SELECT maDuAn FROM dbo.tblDuAn WHERE sttDuAnpr=sttDuAnpr_sd)
                                ,tenDuAn=(SELECT tenDuAn FROM dbo.tblDuAn WHERE sttDuAnpr=sttDuAnpr_sd)
                                ,chuDauTu=(SELECT chuDauTu=(SELECT tenDonVi FROM dbo.tblDMDonVi WHERE maDonVipr=maDonVipr_chudautu) FROM dbo.tblDuAn WHERE sttDuAnpr=sttDuAnpr_sd)
                                ,diaDiemXD=(SELECT diaDiemXD FROM dbo.tblDuAn WHERE sttDuAnpr=sttDuAnpr_sd)
                                ,loaiVanBan=(SELECT tenLoaiVBan FROM dbo.tblDMLoaiVanBan WHERE maLoaiVBanpr=maLoaiVBanpr_sd)
                                FROM  dbo.tblVanBanDA WHERE 
                                ngayKy BETWEEN (select tuNgay from tblBCGSDauTu where sttBCGSDauTupr=N'" + sttBCGSDauTupr + @"') AND (select denNgay from tblBCGSDauTu where sttBCGSDauTupr=N'" + sttBCGSDauTupr + @"')
                                AND sttDuAnpr_sd IN 
                                (SELECT sttDuAnpr FROM dbo.tblDuAn WHERE maDonVipr_sd='" +HttpContext.Current.Session.GetDonVi().maDonVi+@"' AND chinhThuc=1)
                                AND maLoaiVBanpr_sd IN (SELECT maLoaiVBanpr FROM dbo.tblDMLoaiVanBan WHERE maGiaiDoanpr_sd=N'" + maGiaiDoanpr_sd + "') ";
            Grid1_duAn.DataSource = sqlFun.GetData(sql);
            Grid1_duAn.DataBind();
        }
        [AjaxPro.AjaxMethod]
        public void themBCGSDauTuCT(AjaxPro.JavaScriptArray param) {
            try
            {
                SqlConnection sqlCon = new SqlConnection(HttpContext.Current.Session.GetConnectionString2());
                sqlCon.Open();
                string sql = @"INSERT INTO dbo.tblBCGSDauTuCT(sttBCGSDauTupr_sd,sttBuocThaoTac,sttDuAnpr_sd,sttVBDApr_sd)
                                                       VALUES(@sttBCGSDauTupr_sd,@sttBuocThaoTac,@sttDuAnpr_sd,@sttVBDApr_sd)";
                SqlCommand sqlCom = new SqlCommand(sql, sqlCon);
                sqlCom.Parameters.Add(new SqlParameter("@sttBCGSDauTupr_sd", param[2].ToString()));
                sqlCom.Parameters.Add(new SqlParameter("@sttBuocThaoTac", param[3].ToString()));
                sqlCom.Parameters.Add(new SqlParameter("@sttDuAnpr_sd", param[0].ToString()));
                sqlCom.Parameters.Add(new SqlParameter("@sttVBDApr_sd", param[1].ToString()));
                sqlCom.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
        }
        [AjaxPro.AjaxMethod]
        public string xoaBCGSDauTuCT(string sttBCGSDauTuCTpr)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(HttpContext.Current.Session.GetConnectionString2());
                SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                sqlCon.Open();
                string sql = @"delete from dbo.tblBCGSDauTuCT where sttBCGSDauTuCTpr=@sttBCGSDauTuCTpr";
                SqlCommand sqlCom = new SqlCommand(sql, sqlCon);
                sqlCom.Parameters.Add(new SqlParameter("@sttBCGSDauTuCTpr", sttBCGSDauTuCTpr.ToString()));
                if (sqlCom.ExecuteNonQuery() > 0)
                    return "1";
                else
                    return "0";
            }
            catch (Exception ex)
            {
                return "0";
            }
        }

    }
}