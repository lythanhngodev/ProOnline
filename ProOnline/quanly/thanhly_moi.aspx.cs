using System;
using System.Collections;
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
using ProOnline.Class;
using Obout.Grid;
using System.Data.SqlClient;
using AjaxControlToolkit;
using System.IO;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;
using System.Globalization;

namespace ProOnline.quanly
{
    public partial class thanhly_moi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.AppendHeader("X-XSS-Protection", "0");
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ProOnline.Class.ntsLibraryFunctions));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ProOnline.quanly.thanhly_moi));
            if (!IsPostBack)
            {
                //Grid3.DataSource = null;
                //Grid3.DataBind();
                Grid2.DataSource = null;
                Grid2.DataBind();
                Grid1.DataSource = null;
                Grid1.DataBind();
                //cboHopDong.DataSource = null;
                //cboHopDong.DataTextField = "sttHopDongpr";
                //cboHopDong.DataValueField = "noidung";
                //cboHopDong.DataBind();
            }
            AsyncFileUpload.UploadedComplete += new EventHandler<AsyncFileUploadEventArgs>(AsyncFileUpload_UploadedComplete);
            AsyncFileUpload.UploadedFileError += new EventHandler<AsyncFileUploadEventArgs>(AsyncFileUpload_UploadedFileError);
        }
        void AsyncFileUpload_UploadedComplete(object sender, AsyncFileUploadEventArgs e)
        {
            try
            {
                string path = "";
                string urlFile = "";
                SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                string duongdanfilecu = sqlFun.GetOneStringField(@"select top 1 duongDanFile from  dbo.tblThanhLy WHERE sttThanhLypr='" + hdfSttTL.Value.ToString() + "'");
                //lấy mã dự án
                string sttDuAnpr = sqlFun.GetOneStringField("select top 1 CONVERT(NVARCHAR(50),sttDuAnpr_sd) from tblThanhLy where sttThanhLypr='" + hdfSttTL.Value.ToString() + "'");
                string tenFile = e.filename;//"VanBan" + hdfSTTVanBan.Value + ".pdf";
                if (!System.IO.Directory.Exists(Server.MapPath("~/VanBan/" + HttpContext.Current.Session.GetDonVi().maDonVi + "/" + sttDuAnpr + "/thanhLy")))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/VanBan/" + HttpContext.Current.Session.GetDonVi().maDonVi + "/" + sttDuAnpr + "/thanhLy"));
                }
                string strDate = DateTime.Now.ToString("dd_MM_yy_hhmmss");
                string fileExtension = Path.GetExtension(tenFile).Replace("-", "");
                tenFile = tenFile.Substring(tenFile.LastIndexOf("\\\\") + 1);
                tenFile = tenFile.Substring(0, tenFile.LastIndexOf(fileExtension)) + strDate + fileExtension;
                tenFile = tenFile.Replace(" ", "");
                //path = string.Concat(Server.MapPath("/VanBan/" + HttpContext.Current.Session.GetDonVi().maDonVi + "/KHoachDThau/"));
                path = string.Concat(Server.MapPath("/VanBan/" + HttpContext.Current.Session.GetDonVi().maDonVi + "/" + sttDuAnpr + "/thanhLy/" + tenFile + ""));

                if (!System.IO.File.Exists(path))
                {
                    //save lại file mới
                    AsyncFileUpload.SaveAs(path);
                    urlFile = "/VanBan/" + HttpContext.Current.Session.GetDonVi().maDonVi + "/" + sttDuAnpr + "/thanhLy/" + tenFile + "";
                    sqlFun.ExeCuteNonQuery("UPDATE dbo.tblThanhLy SET duongDanFile=N'" + urlFile + "' WHERE sttThanhLypr='" + hdfSttTL.Value.ToString() + "'");
                    //lấy mã dự án
                    //xóa file cũ
                    string duongdanfilecu_ = HttpContext.Current.Server.MapPath(duongdanfilecu.ToString());
                    if (System.IO.File.Exists(duongdanfilecu_))
                    {
                        System.IO.File.Delete(duongdanfilecu_);
                    }
                    AsyncFileUpload.ClearState();
                    AsyncFileUpload.Dispose();
                    return;
                }
                else
                {
                    //Xoa file neu da ton tai
                    System.IO.File.Delete(path);
                    AsyncFileUpload_UploadedComplete(sender, e);
                }
            }
            catch
            {
                return;
            }
            AsyncFileUpload.Dispose();
        }

        void AsyncFileUpload_UploadedFileError(object sender, AsyncFileUploadEventArgs e)
        {
        }
        protected void Grid1_OnRebind(object sender, EventArgs e)
        {
            try
            {
                SqlFunction sqlFunc = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                string sql = @"select sttThanhLypr,sttDuAnpr_sd,
                    sttHopDongpr_sd=(case when laHopDong=1 then 
                    'HD-'+ convert(varchar(18),dbo.tblThanhLy.sttHopDongpr_sd)+'-'+(select convert(varchar(18),sttHMCTrinhpr_sd)+'-'+ convert(varchar(18),sttDauThaupr_sd) FROM dbo.tblHopDong WHERE sttHopDongpr=dbo.tblThanhLy.sttHopDongpr_sd) 
                    else 
                    'PL-'+ convert(varchar(18),dbo.tblThanhLy.sttHopDongpr_sd)+'-'+ (select convert(varchar(18),sttHMCTrinhpr_sd)+'-'+ convert(varchar(18),sttDauThaupr_sd) FROM dbo.tblHopDong WHERE sttHopDongpr=(SELECT sttHopDongpr_sd FROM dbo.tblPhuLucHD WHERE sttPLHDpr=dbo.tblThanhLy.sttHopDongpr_sd))  
                    end)
                    ,tenDuAn=(select tenDuAn from tblDuAn where sttDuAnpr=sttDuAnpr_sd)
                    ,maDuAn=(select maDuAn from tblDuAn where sttDuAnpr=sttDuAnpr_sd)
                    ,soBienBan,ngayLap=convert(varchar(10),ngayLap,103),cacCanCu
                    ,soHopDong=(case when laHopDong = 1 then (select soHopDong from tblHopDong where sttHopDongpr=sttHopDongpr_sd)
                            else (select soPhuLuc from tblPhuLucHD where sttPLHDpr=tblthanhLy.sttHopDongpr_sd) end)
                    ,daiDienCDT,chucVuDDCDT,daiDienNThau,chucVuDDNThau
                    ,giaTriTL=(select SUM(isnull(giaTriThanhLy,0)) from tblthanhLyCT where sttThanhLypr=sttThanhLypr_sd)
                    from tblthanhLy where maDonVipr_sd=N'" + HttpContext.Current.Session.GetDonVi().maDonVi + "'";
                Grid1.DataSource = sqlFunc.GetData(sql);
                Grid1.DataBind();
            }
            catch
            {
            }
        }
        protected void Grid2_OnRebind(object sender, EventArgs e)
        {
            string sql = @"select sttThanhLyCTpr,sttThanhLypr_sd,maCPDauTuXDCTpr_sd,sttHDCTpr_sd
                        ,loai=(case when (select laHopDong from dbo.tblThanhLy where sttThanhLypr=sttThanhLypr_sd)=1 then 'HD' else 'PL' end)
                        ,tenCongViec=(select tenCPDauTuXD FROM tblDMCPDauTuXDCT WHERE maCPDauTuXDCTpr=maCPDauTuXDCTpr_sd) 
                         ,donViTinh,khoiLuong,donGia,giaTriThanhLy
                        ,giaTriHD=(case when (select laHopDong from dbo.tblThanhLy where sttThanhLypr=sttThanhLypr_sd)=1 then 
                        (select giaTriHD from tblHopDongCT where sttHDCTpr=sttHDCTpr_sd) else 
                         (select giaTriPL from tblPhuLucHDCT where sttPLHDCTpr=tblThanhLyCT.sttHDCTpr_sd)   end)
                        from dbo.tblThanhLyCT  where sttThanhLypr_sd='" + hdfSttTL.Value + "'";
            SqlFunction sqlFunc = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            Grid2.DataSource = sqlFunc.GetData(sql);
            Grid2.DataBind();
        }
        //protected void Grid3_OnRebind(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        SqlFunction sqlFunc = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
        //        string sql = "SELECT sttDuAnpr,maDuAn,tenDuAn,tenChuDauTu=(SELECT tenDonVi FROM tblDMDonVi WHERE maDonVipr=maDonVipr_chudautu),diaDiemXD FROM dbo.tblDuAn WHERE tblDuAn.maDonVipr_sd=N'" + HttpContext.Current.Session.GetDonVi().maDonVi + "'";
        //        Grid3.DataSource = sqlFunc.GetData(sql);
        //        Grid3.DataBind();
        //    }
        //    catch
        //    {
        //    }
        //}
        double cotTong = 0;
        public void Grid1_RowDataBound(object sender, GridRowEventArgs e)
        {
            if (e.Row.RowType == GridRowType.DataRow)
            {
                if (e.Row.Cells[4].Text != "")
                {
                    cotTong += double.Parse(e.Row.Cells[4].Text);
                }
            }
            else if (e.Row.RowType == GridRowType.ColumnFooter)
            {
                e.Row.Cells[0].Text = "Tổng cộng";
                e.Row.Cells[0].Style["font-weight"] = "bold";
                e.Row.Cells[4].Text = cotTong.ToString("N0");
                e.Row.Cells[4].Style["font-weight"] = "bold";

            }

        }
        double cot2 = 0;
        double cot5 = 0;
        double cot6 = 0;
        public void Grid2_RowDataBound(object sender, GridRowEventArgs e)
        {
            if (e.Row.RowType == GridRowType.DataRow)
            {
                if (e.Row.Cells[5].Text != "")
                {
                    cot5 += double.Parse(e.Row.Cells[5].Text);
                }
                if (e.Row.Cells[6].Text != "")
                {
                    cot6 += double.Parse(e.Row.Cells[6].Text);
                }
            }
            else if (e.Row.RowType == GridRowType.ColumnFooter)
            {
                e.Row.Cells[0].Text = "Tổng cộng";
                e.Row.Cells[0].Style["font-weight"] = "bold";
                e.Row.Cells[5].Text = cot5.ToString("N0");
                e.Row.Cells[5].Style["font-weight"] = "bold";
                e.Row.Cells[6].Text = cot6.ToString("N0");
                e.Row.Cells[6].Style["font-weight"] = "bold";
            }
        }
        [AjaxPro.AjaxMethod]
        public bool kiemTraNgay(string ngay)
        {
            try
            {
                if (ngay.Length == 10)
                {
                    DateTime dt = DateTime.Parse(ngay.ToString().Trim(), System.Globalization.CultureInfo.GetCultureInfo("en-gb"));
                    if (dt.Year < 1900)
                        return false;
                    else
                        return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        [AjaxPro.AjaxMethod]
        public DataTable getDanhSachHopDong(string sttDuAn)
        {
            string sql = @"SELECT '0'sttHopDongpr, ''noidung UNION ALL select sttHopDongpr,
                            noidung=soHopDong + ' - ' + (select tenDuAn from tblDuAn where sttDuAnpr='" + sttDuAn + @"')+ N' - Hạng mục: ' + isnull((select tenHangMuc from tblHangMucCTrinh where sttHMCTrinhpr=sttHMCTrinhpr_sd),'') from
                            (select sttHopDongpr= 'HD'+'-'+ convert(varchar(18),sttHopDongpr)+'-'+ convert(varchar(18),sttHMCTrinhpr_sd)+'-'+ convert(varchar(18),sttDauThaupr_sd)
                            ,soHopDong=(N'HĐ ' +soHopDong),sttHMCTrinhpr_sd from tblHopDong where ngoaiHopDong=0 and sttDuAnpr_sd='" + sttDuAn + @"'
                            union all
                            select 'PL'+'-'+ convert(varchar(18),sttPLHDpr)+'-'+(select convert(varchar(18),sttHMCTrinhpr_sd) FROM dbo.tblHopDong WHERE sttHopDongpr=sttHopDongpr_sd)
                            +'-'+(select convert(varchar(18),sttDauThaupr_sd) FROM dbo.tblHopDong WHERE sttHopDongpr=sttHopDongpr_sd)
                            ,N'PL ' +soPhuLuc,sttHMCTrinhpr_sd=(select sttHMCTrinhpr_sd from tblHopDong where sttHopDongpr=sttHopDongpr_sd) 
                            from tblPhuLucHD where sttHopDongpr_sd in(select sttHopDongpr from tblHopDong where ngoaiHopDong=0 and sttDuAnpr_sd='" + sttDuAn + "'))as temp";
            SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            return sqlFun.GetData(sql);
        }
        [AjaxPro.AjaxMethod]
        public DataTable getThongTinThanhToan(string sttHD, string sttDuAn)
        {
            var tab = new DataTable();
            if (sttHD == "") return tab;
            string sql = "";
            if (sttHD.Split('-')[0].ToString() == "HD")
                sql = @"
                SELECT soTien= isnull(CONVERT(VARCHAR(5000),(
                select sum(giaTriThanhLy) from tblThanhLyCT where sttThanhLypr_sd in
                (select sttThanhLypr from tblThanhLy where sttHopDongpr_sd='" + sttHD.Split('-')[1].ToString() + @"'))
                - 
                 (ISNULL((
                 select ISNULL(SUM(soTien),0) FROM dbo.tblChungTuChiCT WHERE sttCTChipr_sd IN 
                (SELECT sttCTChipr FROM dbo.tblChungTuChi WHERE sttHopDongpr_sd='" + sttHD.Split('-')[1].ToString() + @"' AND loaiChungTupr_sd IN('010','011'))
                ),0))),0)
                union all
                SELECT DISTINCT (SELECT soTien +'; ' AS [text()] FROM (
                SELECT (N'Tạm ứng: '+CONVERT(NVARCHAR(250),soTien)) AS soTien FROM (
                SELECT SUM(soTien) AS soTien FROM (
                SELECT ISNULL((SELECT SUM(soTien) FROM dbo.tblChungTuChiCT WHERE sttCTChipr_sd=tblChungTuChi.sttCTChipr),0) as soTien FROM dbo.tblChungTuChi WHERE sttHopDongpr_sd='" + sttHD.Split('-')[1].ToString() + @"' AND loaiChungTupr_sd='011'
                ) AS aa 
                ) AS tblTamung
                UNION ALL
                SELECT (N'Thanh toán đợt '+CONVERT(NVARCHAR(5),dot)+': '+CONVERT(NVARCHAR(250),soTien)) AS soTien FROM (
                SELECT ROW_NUMBER()OVER (ORDER BY ngayChungTu) AS dot,ISNULL((SELECT SUM(soTien) FROM dbo.tblChungTuChiCT WHERE sttCTChipr_sd=tblChungTuChi.sttCTChipr),0) as soTien FROM dbo.tblChungTuChi WHERE sttHopDongpr_sd='" + sttHD.Split('-')[1].ToString() + @"' AND loaiChungTupr_sd ='010'
                ) AS tblTamung
                ) AS abc ORDER BY soTien  FOR XML PATH(''))[soTien]
                union all
                SELECT  CONVERT(VARCHAR(5000),  (ISNULL((SELECT  SUM(giaTriHD) FROM dbo.tblHopDongCT WHERE sttHopDongpr_sd='" + sttHD.Split('-')[1].ToString() + @"'),0)))
                union all
                SELECT ISNULL(thuTruong,'') FROM tblDMDonVi WHERE maDonVipr=(SELECT maDonVipr_chudautu FROM tblDuAn WHERE sttDuAnpr='" + sttDuAn + @"')
                union all
                SELECT ISNULL(chucDanhTT,'') FROM tblDMDonVi WHERE maDonVipr=(SELECT maDonVipr_chudautu FROM tblDuAn WHERE sttDuAnpr='" + sttDuAn + @"')
                union all
                SELECT ISNULL(CASE WHEN ISNULL((SELECT nguoiDaiDien FROM tblDMToChuc WHERE maToChucpr = 
                                             (SELECT TOP 1 maToChucpr_trungthau FROM tblKeHoachDauThau WHERE sttDauThaupr_sd = '" + sttHD.Split('-')[3].ToString() + @"')),'') <> '' THEN 
                                             (SELECT nguoiDaiDien FROM tblDMToChuc WHERE maToChucpr = (SELECT TOP 1 maToChucpr_trungthau FROM tblKeHoachDauThau 
                                             WHERE sttDauThaupr_sd = '" + sttHD.Split('-')[3].ToString() + @"')) when  ISNULL((SELECT nguoiDaiDien FROM tblDMToChuc WHERE maToChucpr =
                                             (SELECT TOP 1 maToChucpr_thuchien FROM tblCVDaThucHien WHERE sttDauThaupr_sd = '" + sttHD.Split('-')[3].ToString() + @"')),'') <> '' THEN 
                                             (SELECT nguoiDaiDien FROM tblDMToChuc WHERE maToChucpr = (SELECT TOP 1 maToChucpr_thuchien FROM tblCVDaThucHien WHERE sttDauThaupr_sd = 
                                             '" + sttHD.Split('-')[3].ToString() + @"')) else   (SELECT nguoiDaiDien FROM tblDMToChuc WHERE maToChucpr = (SELECT TOP 1 maToChucpr_thuchien FROM 
                                             tblCVKhongDauThau WHERE sttDauThaupr_sd = '" + sttHD.Split('-')[3].ToString() + @"')) END,'')
                union all
                SELECT ISNULL(CASE WHEN ISNULL((SELECT chucVuDD FROM tblDMToChuc WHERE maToChucpr = 
                                             (SELECT TOP 1 maToChucpr_trungthau FROM tblKeHoachDauThau WHERE sttDauThaupr_sd = '" + sttHD.Split('-')[3].ToString() + @"')),'') <> '' THEN 
                                             (SELECT chucVuDD FROM tblDMToChuc WHERE maToChucpr = (SELECT TOP 1 maToChucpr_trungthau FROM tblKeHoachDauThau 
                                             WHERE sttDauThaupr_sd = '" + sttHD.Split('-')[3].ToString() + @"')) when  ISNULL((SELECT chucVuDD FROM tblDMToChuc WHERE maToChucpr =
                                             (SELECT TOP 1 maToChucpr_thuchien FROM tblCVDaThucHien WHERE sttDauThaupr_sd = '" + sttHD.Split('-')[3].ToString() + @"')),'') <> '' THEN 
                                             (SELECT chucVuDD FROM tblDMToChuc WHERE maToChucpr = (SELECT TOP 1 maToChucpr_thuchien FROM tblCVDaThucHien WHERE sttDauThaupr_sd = 
                                             '" + sttHD.Split('-')[3].ToString() + @"')) else   (SELECT chucVuDD FROM tblDMToChuc WHERE maToChucpr = (SELECT TOP 1 maToChucpr_thuchien FROM 
                                             tblCVKhongDauThau WHERE sttDauThaupr_sd = '" + sttHD.Split('-')[3].ToString() + @"')) END,'')
                ";
            else
            {
                sql = @" 

                SELECT soTien= isnull(CONVERT(VARCHAR(5000),(
                select sum(giaTriThanhLy) from tblThanhLyCT where sttThanhLypr_sd in
                (select sttThanhLypr from tblThanhLy where sttHopDongpr_sd='" + sttHD.Split('-')[1].ToString() + @"'))
                - 
                 (ISNULL((
                 select ISNULL(SUM(soTien),0) FROM dbo.tblChungTuChiCT WHERE sttCTChipr_sd IN 
                (SELECT sttCTChipr FROM dbo.tblChungTuChi WHERE sttPLHDpr_sd='" + sttHD.Split('-')[1].ToString() + @"' AND loaiChungTupr_sd IN('010','011'))
                ),0))),0)
                union all
                SELECT DISTINCT (SELECT soTien +'; ' AS [text()] FROM (
                SELECT (N'Tạm ứng: '+CONVERT(NVARCHAR(250),soTien)) AS soTien FROM (
                SELECT SUM(soTien) AS soTien FROM (
                SELECT ISNULL((SELECT SUM(soTien) FROM dbo.tblChungTuChiCT WHERE sttCTChipr_sd=tblChungTuChi.sttCTChipr),0) as soTien FROM dbo.tblChungTuChi WHERE sttPLHDpr_sd='" + sttHD.Split('-')[1].ToString() + @"' AND loaiChungTupr_sd='011'
                ) AS aa 
                ) AS tblTamung
                UNION ALL
                SELECT (N'Thanh toán đợt '+CONVERT(NVARCHAR(5),dot)+': '+CONVERT(NVARCHAR(250),soTien)) AS soTien FROM (
                SELECT ROW_NUMBER()OVER (ORDER BY ngayChungTu) AS dot,ISNULL((SELECT SUM(soTien) FROM dbo.tblChungTuChiCT WHERE sttCTChipr_sd=tblChungTuChi.sttCTChipr),0) as soTien FROM dbo.tblChungTuChi WHERE sttPLHDpr_sd='" + sttHD.Split('-')[1].ToString() + @"' AND loaiChungTupr_sd ='010'
                ) AS tblTamung
                ) AS abc ORDER BY soTien  FOR XML PATH(''))[soTien] 
                union all
                SELECT  CONVERT(VARCHAR(5000),  (ISNULL((SELECT  SUM(giaTriPL) FROM dbo.tblPhuLucHDCT WHERE sttPLHDpr_sd='" + sttHD.Split('-')[1].ToString() + @"'),0)))
                union all
                SELECT ISNULL(thuTruong,'') FROM tblDMDonVi WHERE maDonVipr=(SELECT maDonVipr_chudautu FROM tblDuAn WHERE sttDuAnpr='" + sttDuAn + @"')
                union all
                SELECT ISNULL(chucDanhTT,'') FROM tblDMDonVi WHERE maDonVipr=(SELECT maDonVipr_chudautu FROM tblDuAn WHERE sttDuAnpr='" + sttDuAn + @"')
                union all
                SELECT ISNULL(CASE WHEN ISNULL((SELECT nguoiDaiDien FROM tblDMToChuc WHERE maToChucpr = 
                                             (SELECT TOP 1 maToChucpr_trungthau FROM tblKeHoachDauThau WHERE sttDauThaupr_sd = '" + sttHD.Split('-')[3].ToString() + @"')),'') <> '' THEN 
                                             (SELECT nguoiDaiDien FROM tblDMToChuc WHERE maToChucpr = (SELECT TOP 1 maToChucpr_trungthau FROM tblKeHoachDauThau 
                                             WHERE sttDauThaupr_sd = '" + sttHD.Split('-')[3].ToString() + @"')) when  ISNULL((SELECT nguoiDaiDien FROM tblDMToChuc WHERE maToChucpr =
                                             (SELECT TOP 1 maToChucpr_thuchien FROM tblCVDaThucHien WHERE sttDauThaupr_sd = '" + sttHD.Split('-')[3].ToString() + @"')),'') <> '' THEN 
                                             (SELECT nguoiDaiDien FROM tblDMToChuc WHERE maToChucpr = (SELECT TOP 1 maToChucpr_thuchien FROM tblCVDaThucHien WHERE sttDauThaupr_sd = 
                                             '" + sttHD.Split('-')[3].ToString() + @"')) else   (SELECT nguoiDaiDien FROM tblDMToChuc WHERE maToChucpr = (SELECT TOP 1 maToChucpr_thuchien FROM 
                                             tblCVKhongDauThau WHERE sttDauThaupr_sd = '" + sttHD.Split('-')[3].ToString() + @"')) END,'')
                union all
                SELECT ISNULL(CASE WHEN ISNULL((SELECT chucVuDD FROM tblDMToChuc WHERE maToChucpr = 
                                             (SELECT TOP 1 maToChucpr_trungthau FROM tblKeHoachDauThau WHERE sttDauThaupr_sd = '" + sttHD.Split('-')[3].ToString() + @"')),'') <> '' THEN 
                                             (SELECT chucVuDD FROM tblDMToChuc WHERE maToChucpr = (SELECT TOP 1 maToChucpr_trungthau FROM tblKeHoachDauThau 
                                             WHERE sttDauThaupr_sd = '" + sttHD.Split('-')[3].ToString() + @"')) when  ISNULL((SELECT chucVuDD FROM tblDMToChuc WHERE maToChucpr =
                                             (SELECT TOP 1 maToChucpr_thuchien FROM tblCVDaThucHien WHERE sttDauThaupr_sd = '" + sttHD.Split('-')[3].ToString() + @"')),'') <> '' THEN 
                                             (SELECT chucVuDD FROM tblDMToChuc WHERE maToChucpr = (SELECT TOP 1 maToChucpr_thuchien FROM tblCVDaThucHien WHERE sttDauThaupr_sd = 
                                             '" + sttHD.Split('-')[3].ToString() + @"')) else   (SELECT chucVuDD FROM tblDMToChuc WHERE maToChucpr = (SELECT TOP 1 maToChucpr_thuchien FROM 
                                             tblCVKhongDauThau WHERE sttDauThaupr_sd = '" + sttHD.Split('-')[3].ToString() + @"')) END,'')
                ";
            }
            SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            return sqlFun.GetData(sql);
        }
        private string _mChuyenChuoiSangNgay(string ddMMyyyy)
        {
            return ddMMyyyy.Substring(3, 2) + "/" + ddMMyyyy.Substring(0, 2) + "/" + ddMMyyyy.Substring(6, 4);
        }

        // thêm , cập nhập thông tin thanh ly
        [AjaxPro.AjaxMethod]
        public string luuThongTin(object[] thanhLy, string coThaoTac)
        {
            try
            {
                var sqlConn = new SqlConnection(HttpContext.Current.Session.GetConnectionString2());
                string sql = "";
                if (coThaoTac == "0")
                {
                    /*
                        "HD-1392-0-2416" sttDauThaupr_sd lấy từ cboHopDong.value();
                     * 
                     */
                    sql = @"INSERT INTO tblThanhLy (sttDuAnpr_sd ,sttDauThaupr_sd ,sttHopDongpr_sd ,soBienBan ,ngayLap ,cacCanCu ,daiDienCDT ,chucVuDDCDT ,daiDienNThau ,chucVuDDNThau ,soHoaDon ,ngayHoaDon ,laHopDong ,maDonVipr_sd ,nguoiThaoTac ,ngayThaotac)
                           VALUES (@sttDuAnpr_sd ,@sttDauThaupr_sd ,@sttHopDongpr_sd ,@soBienBan ,@ngayLap ,@cacCanCu ,@daiDienCDT ,@chucVuDDCDT ,@daiDienNThau ,@chucVuDDNThau ,@soHoaDon ,@ngayHoaDon ,@laHopDong ,@maDonVipr_sd ,@nguoiThaoTac ,GETDATE())";
                    sql = @"INSERT INTO tblThanhLy (sttDuAnpr_sd ,sttDauThaupr_sd ,sttHopDongpr_sd ,soBienBan ,ngayLap ,cacCanCu ,daiDienCDT ,chucVuDDCDT ,daiDienNThau ,chucVuDDNThau ,soHoaDon ,ngayHoaDon ,laHopDong ,maDonVipr_sd ,nguoiThaoTac ,ngayThaotac)
                           VALUES (@sttDuAnpr_sd ,@sttDauThaupr_sd ,@sttHopDongpr_sd ,@soBienBan ,@ngayLap ,@cacCanCu ,@daiDienCDT ,@chucVuDDCDT ,@daiDienNThau ,@chucVuDDNThau ,@soHoaDon ,@ngayHoaDon ,@laHopDong ,@maDonVipr_sd ,@nguoiThaoTac ,GETDATE())";
                }

                else
                    sql = @"UPDATE tblThanhLy SET sttDuAnpr_sd = @sttDuAnpr_sd ,sttDauThaupr_sd = @sttDauThaupr_sd ,sttHopDongpr_sd = @sttHopDongpr_sd ,soBienBan = @soBienBan ,ngayLap = @ngayLap ,cacCanCu = @cacCanCu ,daiDienCDT = @daiDienCDT 
            ,chucVuDDCDT = @chucVuDDCDT ,daiDienNThau = @daiDienNThau ,chucVuDDNThau = @chucVuDDNThau ,soHoaDon = @soHoaDon ,ngayHoaDon = @ngayHoaDon ,laHopDong = @laHopDong ,maDonVipr_sd = @maDonVipr_sd ,nguoiThaoTac = @nguoiThaoTac ,ngayThaotac = GETDATE() WHERE sttThanhLypr = @sttThanhLypr";
                sqlConn.Open();
                var cmd = new SqlCommand(sql, sqlConn);
                if (coThaoTac != "0")
                {
                    // trường hợp cập nhật
                    cmd.Parameters.AddWithValue("@sttThanhLypr", thanhLy[0].ToString()); // cũ là 0
                }
                cmd.Parameters.AddWithValue("@sttDuAnpr_sd", thanhLy[1].ToString()); //  cũ là 1
                cmd.Parameters.AddWithValue("@sttDauThaupr_sd", thanhLy[2].ToString().Split('-')[2].ToString()); // cũ là 2
                cmd.Parameters.AddWithValue("@sttHopDongpr_sd", thanhLy[2].ToString().Split('-')[1].ToString()); // cũ là 2
                cmd.Parameters.AddWithValue("@soBienBan", thanhLy[3].ToString()); // cũ là 4
                cmd.Parameters.AddWithValue("@ngayLap", _mChuyenChuoiSangNgay(thanhLy[4].ToString())); // cũ là 5
                cmd.Parameters.AddWithValue("@cacCanCu", thanhLy[5].ToString()); // cũ là 6
                cmd.Parameters.AddWithValue("@daiDienCDT", thanhLy[6].ToString()); // cũ là 7
                cmd.Parameters.AddWithValue("@chucVuDDCDT", thanhLy[7].ToString()); // cũ là 8
                cmd.Parameters.AddWithValue("@daiDienNThau", thanhLy[8].ToString()); // cũ là 9
                cmd.Parameters.AddWithValue("@chucVuDDNThau", thanhLy[9].ToString()); // cũ là 10
                cmd.Parameters.AddWithValue("@soHoaDon", thanhLy[10].ToString()); // cũ là 11

                if (thanhLy[11].ToString() == "") // cũ là 12
                    cmd.Parameters.AddWithValue("@ngayHoaDon", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@ngayHoaDon", _mChuyenChuoiSangNgay(thanhLy[11].ToString()));

                if (thanhLy[2].ToString().Split('-')[0].ToString() == "HD") // cũ là 2
                    cmd.Parameters.AddWithValue("@laHopDong", "1");
                else
                    cmd.Parameters.AddWithValue("@laHopDong", "0");

                cmd.Parameters.AddWithValue("@maDonVipr_sd", HttpContext.Current.Session.GetDonVi().maDonVi);
                cmd.Parameters.AddWithValue("@nguoiThaoTac", HttpContext.Current.Session.GetCurrentUserID());
                if (cmd.ExecuteNonQuery() > 0)
                {
                    //Cập nhật nghiệm thu về table HopDong
                    if (thanhLy[2].ToString().Split('-')[0].ToString() == "HD")
                        sql = @"update tblHopDong set soBBThanhLy=@soBBThanhLy,ngayThanhLy=@ngayThanhLy,tinhTrangHDpr_sd='030', tinhTrangHDTruocTLy=tinhTrangHDpr_sd where sttHopDongpr=@stt";
                    else
                        sql = @"update tblPhuLucHD set soBBThanhLy=@soBBThanhLy,ngayThanhLy=@ngayThanhLy,tinhTrangPLHDpr_sd='030', tinhTrangPLHDTruocTLy=tinhTrangPLHDpr_sd  where sttPLHDpr=@stt";
                    cmd = new SqlCommand(sql);
                    if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();
                    cmd.Connection = sqlConn;
                    cmd.Parameters.AddWithValue("@soBBThanhLy", thanhLy[3].ToString()); // cũ là 4
                    cmd.Parameters.AddWithValue("@ngayThanhLy", _mChuyenChuoiSangNgay(thanhLy[4].ToString())); // cũ là 5
                    cmd.Parameters.AddWithValue("@stt", thanhLy[2].ToString().Split('-')[1].ToString()); // cũ là 2
                    cmd.ExecuteNonQuery();
                    /////
                    SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                    if (coThaoTac == "0")
                        return sqlFun.GetOneStringField("select convert(varchar(18), max(sttThanhLypr)) from tblThanhLy where maDonVipr_sd='" + HttpContext.Current.Session.GetDonVi().maDonVi + "'");
                    return "1";
                }
                return "0";
            }
            catch
            {
                return "0";
            }

        }
        [AjaxPro.AjaxMethod]
        public DataTable getDanhSachCongViec(string sttHD, string sttThanhLypr, string sttThanhLyCTpr)
        {
            try
            {
                string sql = "";
                if (sttHD.Split('-')[0].ToString() == "HD")
                    sql = @"
                select stt=isnull('" + sttHD.Split('-')[0].ToString() + @"@'+(SELECT CONVERT(VARCHAR(180),sttHDCTpr_sd)+'@'+maCPDauTuXDCTpr_sd FROM dbo.tblThanhLyCT WHERE sttThanhLyCTpr='" + (sttThanhLyCTpr.ToString() == "" ? "-1" : sttThanhLyCTpr) + @"'),'')
                ,noiDung=isnull((SELECT tenCPDauTuXD FROM tblDMCPDauTuXDCT WHERE maCPDauTuXDCTpr=(SELECT maCPDauTuXDCTpr_sd FROM dbo.tblThanhLyCT WHERE sttThanhLyCTpr='" + (sttThanhLyCTpr.ToString() == "" ? "-1" : sttThanhLyCTpr) + @"')),'')
                union all select stt='HD@'+(convert(varchar(18),sttHDCTpr)+'@'+maCPDauTuXDCTpr_sd)
                ,noiDung=(SELECT tenCPDauTuXD FROM tblDMCPDauTuXDCT WHERE maCPDauTuXDCTpr=maCPDauTuXDCTpr_sd)
                from tblHopDongCT where sttHopDongpr_sd='" + sttHD.Split('-')[1].ToString() + @"'
                AND sttHDCTpr NOT IN (SELECT sttHDCTpr_sd FROM dbo.tblThanhLyCT WHERE sttThanhLypr_sd='" + sttThanhLypr + "')";
                else
                {
                    sql = @" select stt=isnull('" + sttHD.Split('-')[0].ToString() + @"@'+(SELECT CONVERT(VARCHAR(180),sttHDCTpr_sd)+'@'+maCPDauTuXDCTpr_sd FROM dbo.tblThanhLyCT WHERE sttThanhLyCTpr='" + (sttThanhLyCTpr.ToString() == "" ? "-1" : sttThanhLyCTpr) + @"'),'')
                ,noiDung=isnull((SELECT tenCPDauTuXD FROM tblDMCPDauTuXDCT WHERE maCPDauTuXDCTpr=(SELECT maCPDauTuXDCTpr_sd FROM dbo.tblThanhLyCT WHERE sttThanhLyCTpr='" + (sttThanhLyCTpr.ToString() == "" ? "-1" : sttThanhLyCTpr) + @"')),'')
                union all  select stt='PL@'+(convert(varchar(18),sttPLHDCTpr)+'@'+maCPDauTuXDCTpr_sd)
                        ,noiDung=(SELECT tenCPDauTuXD FROM tblDMCPDauTuXDCT WHERE maCPDauTuXDCTpr=maCPDauTuXDCTpr_sd)
                        from tblPhuLucHDCT where sttPLHDpr_sd in(select sttPLHDpr from tblPhuLucHD 
                        where sttPLHDpr_sd='" + sttHD.Split('-')[1].ToString() + "'  AND sttPLHDCTpr NOT IN (SELECT sttHDCTpr_sd FROM dbo.tblThanhLyCT WHERE sttThanhLypr_sd='" + sttThanhLypr + "'))";
                }
                SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                return sqlFun.GetData(sql);
            }
            catch
            {
                return null;
            }
        }
        [AjaxPro.AjaxMethod]
        public string checkDuyetQT(string sttDuAn)
        {
            SqlFunction sqlFunc = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            return sqlFunc.GetOneStringField("select convert(char(1), duyetQToan) from tblDuAn where sttDuAnpr='" + sttDuAn + "'");
        }
        [AjaxPro.AjaxMethod]
        public string checkDuyetQToan(string sttDuAn)
        {
            SqlFunction _sqlfun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            return _sqlfun.GetOneStringField("select (case when duyetQToan = 1 then '1' else '0' end) from tblDuAn where sttDuAnpr='" + sttDuAn + "'");
        }
        [AjaxPro.AjaxMethod]
        public static DataTable thongTinCongViec(string maCongViec, string flag)
        {
            try
            {
                SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                if (maCongViec == "") return sqlFun.GetData(@"select maCPDauTuXDCTpr_sd=''
                        ,tenCongViec='' ,donViTinh='',khoiLuong=0 ,donGia=0,thanhTien=0,giaTriHD=0");
                string sql = "";
                if (flag == "0")
                {
                    //Chọn thêm chi tiết HĐ
                    if (maCongViec.Split('@')[0].ToString() == "HD")
                        sql = @"select maCPDauTuXDCTpr_sd
                        ,tenCongViec=(select tenCPDauTuXD FROM tblDMCPDauTuXDCT WHERE maCPDauTuXDCTpr=maCPDauTuXDCTpr_sd) 
                         ,donViTinh='',khoiLuong=1,donGia=giaTriHD,thanhTien=giaTriHD,giaTriHD 
                        from tblHopDongCT where sttHDCTpr='" + maCongViec.Split('@')[1].ToString() + "'";
                    else
                        //Chọn thêm chi tiết PL
                        sql = @"select maCPDauTuXDCTpr_sd
                        ,tenCongViec=(select tenCPDauTuXD FROM tblDMCPDauTuXDCT WHERE maCPDauTuXDCTpr=maCPDauTuXDCTpr_sd) 
                         ,donViTinh='',khoiLuong=1,donGia=giaTriPL,thanhTien=giaTriPL,giaTriHD=giaTriPL
                        from tblPhuLucHDCT where sttPLHDCTpr='" + maCongViec.Split('@')[1].ToString() + "'";
                }
                else
                {
                    sql = @"select sttNghiemThuCTpr,maCPDauTuXDCTpr_sd,sttHDCTpr_sd
                        ,tenCongViec=(select tenCPDauTuXD FROM tblDMCPDauTuXDCT WHERE maCPDauTuXDCTpr=maCPDauTuXDCTpr_sd)
                        donViTinh,khoiLuong,donGia,thanhTien,
                        ,giaTriHD=(case when (select laHopDong from dbo.tblNghiemThu where sttNghiemThupr=sttNghiemThupr_sd)=1 
                        then (select giaTriHD from tblHopDongCT where sttHDCTpr=sttHDCTpr_sd)
                        else (select giaTriPL from tblPhuLucHDCT where sttPLHDCTpr=sttHDCTpr_sd) end)                       
                        from tblNghiemThuCT where sttHopDongpr_sd='" + maCongViec.Split('@')[1].ToString() + "'";
                }

                return sqlFun.GetData(sql);
            }
            catch
            {
                return null;
            }
        }
        [AjaxPro.AjaxMethod]
        public string thanhTien(string soLuong, string donGia)
        {
            double kq = 0;
            kq = double.Parse(soLuong.Replace(".", "")) * double.Parse(donGia.Replace(".", ""));
            return kq.ToString();
        }
        [AjaxPro.AjaxMethod]
        public static string kiemTraCongViecThanhLy(string comBoCongViec, string sttThanhLy)
        {
            SqlFunction sqlFunc = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            string sql = @"select convert(varchar(18), sttThanhLypr) from tblThanhLy where laHopDong=" + (comBoCongViec.Split('@')[0].ToString() == "HD" ? "1" : "0") + @" and 
            sttThanhLypr =(select top 1 sttThanhLypr_sd from tblNghiemThuCT where sttThanhLyCTpr<>'" + (sttThanhLy == "" ? "0" : sttThanhLy) + @"' and sttHDCTpr_sd='" + comBoCongViec.Split('@')[1].ToString() + @"' and maCPDauTuXDCTpr_sd='" + comBoCongViec.Split('@')[2].ToString() + "')";
            return sqlFunc.GetOneStringField(sql);
        }
        [AjaxPro.AjaxMethod]
        public string soSanh(string soSS, string giaTriHopDong)
        {
            return (decimal.Parse(soSS.Replace(".", "")) > decimal.Parse(giaTriHopDong.Replace(".", ""))).ToString();

        }
        [AjaxPro.AjaxMethod]
        public static string luuThongTinThanhLyCT(object[] param, string flag)
        {
            try
            {
                string sql = "";
                if (flag == "0")
                    sql = @"INSERT INTO tblThanhLyCT ( sttThanhLypr_sd , sttHDCTpr_sd , maCPDauTuXDCTpr_sd , donViTinh , khoiLuong , donGia , giaTriThanhLy ) 
                            VALUES ( @sttThanhLypr_sd ,@sttHDCTpr_sd ,@maCPDauTuXDCTpr_sd ,@donViTinh ,@khoiLuong ,@donGia ,@giaTriThanhLy)";
                else
                    sql = @"UPDATE tblThanhLyCT SET sttThanhLypr_sd = @sttThanhLypr_sd ,sttHDCTpr_sd = @sttHDCTpr_sd ,maCPDauTuXDCTpr_sd = @maCPDauTuXDCTpr_sd ,donViTinh = @donViTinh ,khoiLuong = @khoiLuong 
                            ,donGia = @donGia ,giaTriThanhLy = @giaTriThanhLy  WHERE sttThanhLyCTpr=@sttThanhLyCTpr";
                SqlConnection sqlConn = new SqlConnection(HttpContext.Current.Session.GetConnectionString2());
                SqlCommand cmd = new SqlCommand(sql);
                sqlConn.Open();
                cmd.Connection = sqlConn;

                cmd.Parameters.AddWithValue("sttThanhLypr_sd", param[1].ToString());
                cmd.Parameters.AddWithValue("sttHDCTpr_sd", param[3].ToString().Split('@')[1].ToString());
                cmd.Parameters.AddWithValue("maCPDauTuXDCTpr_sd", param[3].ToString().Split('@')[2].ToString());
                cmd.Parameters.AddWithValue("donViTinh", param[4].ToString());
                cmd.Parameters.AddWithValue("khoiLuong", param[5].ToString().Replace(".", "").Replace(",", "."));
                cmd.Parameters.AddWithValue("donGia", param[6].ToString().Replace(".", "").Replace(",", "."));
                cmd.Parameters.AddWithValue("giaTriThanhLy", param[7].ToString().Replace(".", "").Replace(",", "."));
                if (flag == "1")
                    cmd.Parameters.AddWithValue("sttThanhLyCTpr", param[0].ToString());
                cmd.ExecuteNonQuery();
                return "1";
            }
            catch
            {
                return "0";
            }
        }
        [AjaxPro.AjaxMethod]
        public DataTable layThongTinVanBanTL(string sttThanhLypr)
        {
            string sql = "SELECT top 1 *,convert(nvarchar(12),ngayLap,103) as ngay,isnull(giaTri,0) as giaTri_,duongDanFile_=isnull(duongDanFile,'') FROM tblThanhLy WHERE sttThanhLypr=N'" + sttThanhLypr + "'";
            SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            return sqlFun.GetData(sql);
        }
        [AjaxPro.AjaxMethod]
        public bool luuThongTinVanBanTL(object[] param)
        {
            string sql = "update tblThanhLy set giaTri = N'" + (string.IsNullOrEmpty(param[2].ToString()) ? "0" : param[2].ToString().Replace(".", "").Replace(",", ".")) + "', noiDung = N'" + param[1].ToString() + "' WHERE sttThanhLypr=N'" + param[0].ToString() + "'";
            SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            return sqlFun.ExeCuteNonQuery(sql);
        }
        [AjaxPro.AjaxMethod]
        public static string xoaThongTin(string stt, string flag)
        {
            try
            {
                SqlConnection sqlConn = new SqlConnection(HttpContext.Current.Session.GetConnectionString2());
                SqlCommand cmd = new SqlCommand();

                if (flag == "0")
                {
                    //xóa chi tiet
                    cmd = new SqlCommand("delete dbo.tblThanhLy where sttThanhLypr=@sttThanhLypr");
                    if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();
                    cmd.Connection = sqlConn;
                    cmd.Parameters.AddWithValue("@sttThanhLypr", stt);
                    cmd.ExecuteNonQuery();
                    //cap nhat lai hop dong
                    cmd = new SqlCommand(@"if((select laHopDong from tblThanhLy where sttThanhLypr=@sttThanhLypr)=1)
                            begin
	                            update tblHopDong set soBBThanhLy=null,ngayThanhLy=null,tinhTrangHDpr_sd=tinhTrangHDTruocTLy where sttHopDongpr=
	                            (select sttHopDongpr_sd from tblThanhLy where sttThanhLypr=@sttThanhLypr)
	                        end
                            else
                            begin
	                            update tblPhuLucHD set soBBThanhLy=null,ngayThanhLy=null,tinhTrangPLHDpr_sd=tinhTrangPLHDTruocTLy where sttPLHDpr=
	                            (select sttHopDongpr_sd from tblThanhLy where sttThanhLypr=@sttThanhLypr) 
                            end");
                    if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();
                    cmd.Connection = sqlConn;
                    cmd.Parameters.AddWithValue("@sttThanhLypr", stt);
                    cmd.ExecuteNonQuery();
                    //xóa thong tin
                    cmd = new SqlCommand("delete dbo.tblNghiemThu where sttNghiemThupr=@sttNghiemThupr");
                    if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();
                    cmd.Connection = sqlConn;
                    cmd.Parameters.AddWithValue("@sttNghiemThupr", stt);
                    cmd.ExecuteNonQuery();
                }
                else
                {

                    //xoa thong tin chi tiet
                    cmd = new SqlCommand("delete dbo.tblThanhLyCT where sttThanhLyCTpr=@sttThanhLyCTpr");
                    if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();
                    cmd.Connection = sqlConn;
                    cmd.Parameters.AddWithValue("@sttThanhLyCTpr", stt);
                    cmd.ExecuteNonQuery();
                }
                return "1";
            }
            catch
            {
                return "0";

            }
        }
        [AjaxPro.AjaxMethod]
        public string xuatWord(string sttThanhLypr)
        {
            SqlFunction sqlFunc = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            string url = "~/xuatword" + "/" + HttpContext.Current.Session.GetCurrentUserID() + "/";
            string fileName = "bienBanThanhLy" + HttpContext.Current.Session.GetDonVi().maDonVi + "-" + (DateTime.Now.ToString("ddMMyyyyHHmmss")) + ".docx";

            if (!System.IO.Directory.Exists(Server.MapPath(url)))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath(url));
            }
            DirectoryInfo di = new DirectoryInfo(Server.MapPath(url));
            FileInfo[] rgFiles = di.GetFiles();
            foreach (FileInfo fi in rgFiles)
            {
                fi.Delete();
            }
            try
            {
                File.Copy(Server.MapPath("~/WordMau/bienBanThanhLy.docx"), Server.MapPath("~/xuatword" + "/" + HttpContext.Current.Session.GetCurrentUserID() + "/" + fileName), true);

                #region truy van
                string sql = @"SELECT  dbo.tblThanhLy.soBienBan,dbo.tblThanhLy.ngayLap
,tenBienBan=(CASE WHEN dbo.tblThanhLy.laHopDong=1 THEN N'BIÊN BẢN THANH LÝ HỢP ĐỒNG' ELSE N'BIÊN BẢN THANH LÝ PHỤ LỤC HỢP ĐỒNG' END)
,tenDuAn=(SELECT tenDuAn FROM tblDuAn WHERE sttDuAnpr=sttDuAnpr_sd)
,diaDiemXayDung=(SELECT diaDiemXD FROM tblDuAn WHERE sttDuAnpr=sttDuAnpr_sd)
,hangMucCongTrinh=(SELECT tenHangMuc FROM dbo.tblHangMucCTrinh WHERE sttHMCTrinhpr=
         (CASE WHEN laHopDong=1 
            THEN (SELECT sttHMCTrinhpr_sd FROM dbo.tblHopDong WHERE sttHopDongpr=sttHopDongpr_sd)
            ELSE (SELECT sttHMCTrinhpr_sd FROM dbo.tblHopDong WHERE sttHopDongpr=(SELECT sttHopDongpr_sd FROM dbo.tblPhuLucHD WHERE sttPLHDpr=dbo.tblThanhLy.sttHopDongpr_sd))
            END)
          )
,soHopDong=(CASE WHEN laHopDong=1 THEN (SELECT soHopDong FROM dbo.tblHopDong WHERE sttHopDongpr=sttHopDongpr_sd) ELSE
    (SELECT soPhuLuc FROM dbo.tblPhuLucHD WHERE sttPLHDpr=dbo.tblThanhLy.sttHopDongpr_sd) end)
,ngayKyHD=(CASE WHEN laHopDong=1 THEN (SELECT CONVERT(VARCHAR(10),ngayKy,103) FROM dbo.tblHopDong WHERE sttHopDongpr=sttHopDongpr_sd) ELSE
    (SELECT CONVERT(VARCHAR(10),ngayKyPL,103) FROM dbo.tblPhuLucHD WHERE sttPLHDpr= dbo.tblThanhLy.sttHopDongpr_sd) end)	
,tenCQCDT=(SELECT tenDonVi FROM tblDMDonVi WHERE maDonVipr=(SELECT maDonVipr_chudautu FROM tblDuAn WHERE sttDuAnpr=tblThanhLy.sttDuAnpr_sd))		 
,daiDienCDT=dbo.tblThanhLy.daiDienCDT
,chucVuCDT=dbo.tblThanhLy.daiDienCDT
,diaChiCDT=(SELECT diaChi FROM tblDMDonVi WHERE maDonVipr=(SELECT maDonVipr_chudautu FROM tblDuAn WHERE sttDuAnpr=tblThanhLy.sttDuAnpr_sd))		 
,dienThoaiCDT =(SELECT dienThoai FROM tblDMDonVi WHERE maDonVipr=(SELECT maDonVipr_chudautu FROM tblDuAn WHERE sttDuAnpr=tblThanhLy.sttDuAnpr_sd))
,faxCDT=(SELECT Fax FROM tblDMDonVi WHERE maDonVipr=(SELECT maDonVipr_chudautu FROM tblDuAn WHERE sttDuAnpr=tblThanhLy.sttDuAnpr_sd))
,taiKhoanCDT =(SELECT soTaiKhoan FROM tblDuAn WHERE sttDuAnpr=tblThanhLy.sttDuAnpr_sd)
,nganHangCDT =(SELECT tenDonVi FROM tblDMDonVi WHERE maDonVipr=(SELECT maDonVipr_noimotk FROM tblDuAn WHERE sttDuAnpr=tblThanhLy.sttDuAnpr_sd))
,maSoThueCDT =(SELECT maSoThue FROM tblDMDonVi WHERE maDonVipr=(SELECT maDonVipr_chudautu FROM tblDuAn WHERE sttDuAnpr=tblThanhLy.sttDuAnpr_sd))

,daiDienNT= dbo.tblThanhLy.daiDienNThau
,chucVuNT= dbo.tblThanhLy.chucVuDDNThau 
,soTienThanhLy=ISNULL((SELECT SUM(giaTriThanhLy) FROM dbo.tblThanhLyCT WHERE sttThanhLypr_sd=sttThanhLypr),0)
,soTienDaThanhToan= (CASE WHEN dbo.tblThanhLy.laHopDong=1 
                then  (SELECT ISNULL(SUM(soTien),0) AS soTien FROM dbo.tblChungTuChiCT WHERE sttCTChipr_sd IN (SELECT sttCTChipr FROM dbo.tblChungTuChi WHERE sttHopDongpr_sd=dbo.tblThanhLy.sttHopDongpr_sd AND loaiChungTupr_sd IN('010','011')))
                ELSE 	 (SELECT ISNULL(SUM(soTien),0) AS soTien FROM dbo.tblChungTuChiCT WHERE sttCTChipr_sd IN (SELECT sttCTChipr FROM dbo.tblChungTuChi WHERE sttPLHDpr_sd=dbo.tblThanhLy.sttHopDongpr_sd AND loaiChungTupr_sd IN('010','011')))
            END)
,soHoaDon
,ngayHoaDon=convert(varchar(10),ngayHoaDon,103)
,giaTriThanhLy=   ISNULL((SELECT  SUM(giaTriThanhLy) FROM  dbo.tblThanhLyCT WHERE sttThanhLypr_sd=sttThanhLypr),0)
            
,soTienCanThanhToan=(CASE WHEN laHopDong=1
                THEN (select ISNULL((SELECT  SUM(giaTriThanhLy) FROM  dbo.tblThanhLyCT WHERE sttThanhLypr_sd=sttThanhLypr),0)- ISNULL(SUM(soTien),0)  AS soTien FROM dbo.tblChungTuChiCT WHERE sttCTChipr_sd IN (SELECT sttCTChipr FROM dbo.tblChungTuChi WHERE sttHopDongpr_sd=dbo.tblThanhLy.sttHopDongpr_sd AND loaiChungTupr_sd IN('010','011')))
                ELSE (SELECT  (ISNULL((SELECT  SUM(giaTriThanhLy) FROM  dbo.tblThanhLyCT WHERE sttThanhLypr_sd=sttThanhLypr),0)- ISNULL(SUM(soTien),0))  AS soTien FROM dbo.tblChungTuChiCT WHERE sttCTChipr_sd IN (SELECT sttCTChipr FROM dbo.tblChungTuChi WHERE sttPLHDpr_sd=dbo.tblThanhLy.sttHopDongpr_sd AND loaiChungTupr_sd IN('010','011')))
             END) 
,cacCanCu
                        FROM dbo.tblThanhLy WHERE sttThanhLypr='" + sttThanhLypr + "'";
                DataTable dtNhaThau = sqlFunc.GetData(@"select * from tblDmToChuc where maToChucpr=(select CASE WHEN ISNULL((SELECT maToChucpr FROM tblDMToChuc WHERE maToChucpr = 
                            (SELECT TOP 1 maToChucpr_trungthau FROM tblKeHoachDauThau WHERE sttDauThaupr_sd = 
                            (select sttDauThaupr_sd from tblHopDong where sttHopDongpr=(SELECT (CASE WHEN laHopDong=1 THEN sttHopDongpr_sd ELSE (SELECT tblPhuLucHD.sttHopDongpr_sd FROM dbo.tblPhuLucHD WHERE sttPLHDpr=tblThanhLy.sttHopDongpr_sd) END) FROM dbo.tblThanhLy WHERE sttThanhLypr='" + sttThanhLypr + @"')))),'') <> '' THEN  
                            (SELECT maToChucpr FROM tblDMToChuc WHERE maToChucpr = 
                            (SELECT TOP 1 maToChucpr_trungthau FROM tblKeHoachDauThau WHERE sttDauThaupr_sd = 
                            (select sttDauThaupr_sd from tblHopDong where sttHopDongpr=(SELECT (CASE WHEN laHopDong=1 THEN sttHopDongpr_sd ELSE (SELECT tblPhuLucHD.sttHopDongpr_sd FROM dbo.tblPhuLucHD WHERE sttPLHDpr=tblThanhLy.sttHopDongpr_sd) END) FROM dbo.tblThanhLy WHERE sttThanhLypr='" + sttThanhLypr + @"')))) when  
                            ISNULL((SELECT maToChucpr FROM tblDMToChuc WHERE maToChucpr = 
                            (SELECT TOP 1 maToChucpr_thuchien FROM tblCVDaThucHien WHERE sttDauThaupr_sd = 
                            (select sttDauThaupr_sd from tblHopDong where sttHopDongpr=(SELECT (CASE WHEN laHopDong=1 THEN sttHopDongpr_sd ELSE (SELECT tblPhuLucHD.sttHopDongpr_sd FROM dbo.tblPhuLucHD WHERE sttPLHDpr=tblThanhLy.sttHopDongpr_sd) END) FROM dbo.tblThanhLy WHERE sttThanhLypr='" + sttThanhLypr + @"')))),'') <> '' THEN 
                            (SELECT maToChucpr FROM tblDMToChuc WHERE maToChucpr = 
                            (SELECT TOP 1 maToChucpr_thuchien FROM tblCVDaThucHien WHERE sttDauThaupr_sd = 
                            (select sttDauThaupr_sd from tblHopDong where sttHopDongpr=(SELECT (CASE WHEN laHopDong=1 THEN sttHopDongpr_sd ELSE (SELECT tblPhuLucHD.sttHopDongpr_sd FROM dbo.tblPhuLucHD WHERE sttPLHDpr=tblThanhLy.sttHopDongpr_sd) END) FROM dbo.tblThanhLy WHERE sttThanhLypr='" + sttThanhLypr + @"')))) else   
                            (SELECT maToChucpr FROM tblDMToChuc WHERE maToChucpr = (SELECT TOP 1 maToChucpr_thuchien FROM tblCVKhongDauThau WHERE sttDauThaupr_sd = 
                            (select sttDauThaupr_sd from tblHopDong where sttHopDongpr=(SELECT (CASE WHEN laHopDong=1 THEN sttHopDongpr_sd ELSE (SELECT tblPhuLucHD.sttHopDongpr_sd FROM dbo.tblPhuLucHD WHERE sttPLHDpr=tblThanhLy.sttHopDongpr_sd) END) FROM dbo.tblThanhLy WHERE sttThanhLypr='" + sttThanhLypr + @"')))) end)");
                #endregion
                var nfi = new NumberFormatInfo { NumberDecimalSeparator = ",", NumberGroupSeparator = "." };
                var dt1 = new DataTable();
                dt1 = sqlFunc.GetData(sql);
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(Server.MapPath(url + fileName), true))
                {
                    _LOfficeExcel vloExcel = new _LOfficeExcel();
                    MainDocumentPart mainPart = wordDoc.MainDocumentPart;
                    Body body = wordDoc.MainDocumentPart.Document.Body;
                    #region replace biến
                    var paras = body.Elements();
                    if (dt1.Rows.Count != 0)
                        foreach (var para1 in paras)
                        {
                            foreach (var run1 in para1.Elements<Run>())
                            {
                                foreach (var text in run1.Elements<Text>())
                                {
                                    if (text.Text.Contains("tenCQCDT"))
                                    {
                                        text.Text = text.Text.Replace("tenCQCDT", dt1.Rows[0]["tenCQCDT"].ToString());
                                    }
                                    if (text.Text.Contains("soThanhLy"))
                                    {
                                        text.Text = text.Text.Replace("soThanhLy", dt1.Rows[0]["soBienban"].ToString());
                                    }
                                    if (text.Text.Contains("tenBienBan"))
                                    {
                                        text.Text = text.Text.Replace("tenBienBan", dt1.Rows[0]["tenBienBan"].ToString());
                                    }
                                    if (text.Text.Contains("tieuDeThanhLy"))
                                    {
                                        text.Text = text.Text.Replace("tieuDeThanhLy", "(Hợp đồng số: " + dt1.Rows[0]["soHopDong"].ToString() + ", ngày " + dt1.Rows[0]["ngayKyHD"].ToString() + ")");
                                    }
                                    if (text.Text.Contains("tenDuAn"))
                                    {
                                        text.Text = text.Text.Replace("tenDuAn", dt1.Rows[0]["tenDuAn"].ToString());
                                    }
                                    if (text.Text.Contains("hangMucCongTrinh"))
                                    {
                                        text.Text = text.Text.Replace("hangMucCongTrinh", dt1.Rows[0]["hangMucCongTrinh"].ToString());
                                    }
                                    if (text.Text.Contains("diaDiemXayDung"))
                                    {
                                        text.Text = text.Text.Replace("diaDiemXayDung", dt1.Rows[0]["diaDiemXayDung"].ToString());
                                    }
                                    if (text.Text.Contains("ngayThanhLy"))
                                    {
                                        text.Text = text.Text.Replace("ngayThanhLy", (dt1.Rows[0]["ngayLap"].ToString() == "" ? "ngày...tháng...năm..." : "ngày " + dt1.Rows[0]["ngayLap"].ToString().Substring(0, 2) + " tháng " + dt1.Rows[0]["ngayLap"].ToString().Substring(3, 2) + " năm " + dt1.Rows[0]["ngayLap"].ToString().Substring(6, 4)));
                                    }
                                    if (text.Text.Contains("tenCQCDT"))
                                    {
                                        text.Text = text.Text.Replace("tenCQCDT", dt1.Rows[0]["tenCQCDT"].ToString());
                                    }
                                    if (text.Text.Contains("daiDienCDT"))
                                    {
                                        text.Text = text.Text.Replace("daiDienCDT", dt1.Rows[0]["daiDienCDT"].ToString());
                                    }
                                    if (text.Text.Contains("faxCDT"))
                                    {
                                        text.Text = text.Text.Replace("faxCDT", dt1.Rows[0]["faxCDT"].ToString());
                                    }
                                    if (text.Text.Contains("chucVuCDT"))
                                    {
                                        text.Text = text.Text.Replace("chucVuCDT", dt1.Rows[0]["chucVuCDT"].ToString());
                                    }
                                    if (text.Text.Contains("diaChiCDT"))
                                    {
                                        text.Text = text.Text.Replace("diaChiCDT", dt1.Rows[0]["diaChiCDT"].ToString());
                                    }
                                    if (text.Text.Contains("dienThoaiCDT"))
                                    {
                                        text.Text = text.Text.Replace("dienThoaiCDT", dt1.Rows[0]["dienThoaiCDT"].ToString());
                                    }
                                    if (text.Text.Contains("taiKhoanCDT"))
                                    {
                                        text.Text = text.Text.Replace("taiKhoanCDT", dt1.Rows[0]["taiKhoanCDT"].ToString());
                                    }
                                    if (text.Text.Contains("nganHangCDT"))
                                    {
                                        text.Text = text.Text.Replace("nganHangCDT", dt1.Rows[0]["nganHangCDT"].ToString());
                                    }
                                    if (text.Text.Contains("maSoThueCDT"))
                                    {
                                        text.Text = text.Text.Replace("maSoThueCDT", dt1.Rows[0]["maSoThueCDT"].ToString());
                                    }
                                    if (text.Text.Contains("tenDonVi"))
                                    {
                                        text.Text = text.Text.Replace("tenDonVi", HttpContext.Current.Session.GetDonVi().tenDonVi);
                                    }
                                    if (text.Text.Contains("tenCQNT"))
                                    {
                                        text.Text = text.Text.Replace("tenCQNT", dtNhaThau.Rows[0]["tenToChuc"].ToString());
                                    }

                                    if (text.Text.Contains("daiDienNT"))
                                    {
                                        text.Text = text.Text.Replace("daiDienNT", dt1.Rows[0]["daiDienNT"].ToString());
                                    }
                                    if (text.Text.Contains("chucVuNT"))
                                    {
                                        text.Text = text.Text.Replace("chucVuNT", dt1.Rows[0]["chucVuNT"].ToString());
                                    }
                                    if (text.Text.Contains("diaChiNT"))
                                    {
                                        text.Text = text.Text.Replace("diaChiNT", dtNhaThau.Rows[0]["diaChi"].ToString());
                                    }
                                    if (text.Text.Contains("dienThoaiNT"))
                                    {
                                        text.Text = text.Text.Replace("dienThoaiNT", dtNhaThau.Rows[0]["dienThoai"].ToString());
                                    }
                                    if (text.Text.Contains("faxNT"))
                                    {
                                        text.Text = text.Text.Replace("faxNT", dtNhaThau.Rows[0]["Fax"].ToString());
                                    }
                                    if (text.Text.Contains("taiKhoanNT"))
                                    {
                                        text.Text = text.Text.Replace("taiKhoanNT", dtNhaThau.Rows[0]["soTaiKhoan"].ToString());
                                    }
                                    if (text.Text.Contains("nganHangNT"))
                                    {
                                        text.Text = text.Text.Replace("nganHangNT", dtNhaThau.Rows[0]["noiMoTK"].ToString());
                                    }
                                    if (text.Text.Contains("maSoThueNT"))
                                    {
                                        text.Text = text.Text.Replace("maSoThueNT", dtNhaThau.Rows[0]["maSoThue"].ToString());
                                    }
                                    if (text.Text.Contains("soTienBangChu"))
                                    {
                                        text.Text = text.Text.Replace("soTienBangChu", sqlFunc._mDocSoThanhChu(decimal.Parse(dt1.Rows[0]["soTienThanhLy"].ToString())));
                                    }
                                    if (text.Text.Contains("_soTienDaThanhToan"))
                                    {
                                        text.Text = text.Text.Replace("_soTienDaThanhToan", decimal.Parse(dt1.Rows[0]["soTienDaThanhToan"].ToString()).ToString("N0"));
                                    }
                                    if (text.Text.Contains("soTienDaThanhToanBangChu"))
                                    {
                                        text.Text = text.Text.Replace("soTienDaThanhToanBangChu", sqlFunc._mDocSoThanhChu(decimal.Parse(dt1.Rows[0]["soTienDaThanhToan"].ToString())));
                                    }
                                    if (text.Text.Contains("soHoaDon"))
                                    {
                                        text.Text = text.Text.Replace("soHoaDon", dt1.Rows[0]["soHoaDon"].ToString());
                                    }
                                    if (text.Text.Contains("ngayHoaDon"))
                                    {
                                        text.Text = text.Text.Replace("ngayHoaDon", (dt1.Rows[0]["ngayHoaDon"].ToString() == "" ? "" : ", " + dt1.Rows[0]["ngayHoaDon"].ToString()));
                                    }
                                    if (text.Text.Contains("soTienHoaDon"))
                                    {
                                        text.Text = text.Text.Replace("soTienHoaDon", decimal.Parse(dt1.Rows[0]["giaTriThanhLy"].ToString()).ToString("N0"));
                                    }
                                    if (text.Text.Contains("soTienCanThanhToanBangChu"))
                                    {
                                        text.Text = text.Text.Replace("soTienCanThanhToanBangChu", sqlFunc._mDocSoThanhChu(decimal.Parse(dt1.Rows[0]["soTienCanThanhToan"].ToString().Replace("-", ""))));
                                    }
                                    if (text.Text.Contains("_soTienCanThanhToan"))
                                    {
                                        text.Text = text.Text.Replace("_soTienCanThanhToan", decimal.Parse(dt1.Rows[0]["soTienCanThanhToan"].ToString()).ToString("N0"));
                                    }

                                    if (text.Text.Contains("soHopDong"))
                                    {
                                        text.Text = text.Text.Replace("soHopDong", dt1.Rows[0]["soHopDong"].ToString());
                                    }
                                    if (text.Text.Contains("ngayKyHopDong"))
                                    {
                                        text.Text = text.Text.Replace("ngayKyHopDong", dt1.Rows[0]["ngayKyHD"].ToString());
                                    }
                                }
                            }
                        }
                    #endregion

                    var tables = mainPart.Document.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ToList();
                    object[] mangCacCanCu = dt1.Rows[0]["cacCanCu"].ToString().Split(';');
                    for (int i = 0; i < mangCacCanCu.Length; i++)
                    {
                        var tr = new DocumentFormat.OpenXml.Wordprocessing.TableRow();
                        var td = new DocumentFormat.OpenXml.Wordprocessing.TableCell();
                        td.Append(new TableCellProperties(
                            new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }),
                            new Paragraph(
                                new ParagraphProperties(new Justification() { Val = JustificationValues.Left }),
                                new DocumentFormat.OpenXml.Wordprocessing.Run(
                                    new RunFonts { Ascii = "Times New Roman", HighAnsi = "Times New Roman" },
                                    new RunProperties(new DocumentFormat.OpenXml.Wordprocessing.FontSize { Val = "24" }),
                                    new RunProperties(new DocumentFormat.OpenXml.Wordprocessing.Italic { Val = true }),
                                    new DocumentFormat.OpenXml.Wordprocessing.Text(mangCacCanCu[i].ToString())
                                    )
                            )
                            );
                        tr.Append(td);
                        tables[1].Append(tr);
                    }
                    #region ghi tam ung
                    var tab = new DataTable();
                    tab = sqlFunc.GetData(@"SELECT soChungTu,loaiChungTu=(SELECT tenLoaiChungTu FROM dbo.tblDMLoaiChungTu WHERE loaiChungTupr=loaiChungTupr_sd), ngayChungTu,soTien=ISNULL((SELECT SUM(soTien) FROM dbo.tblChungTuChiCT WHERE sttCTChipr_sd=sttCTChipr),0) FROM (
                                SELECT sttCTChipr,loaiChungTupr_sd,soChungTu,  ngayChungTu=CONVERT(VARCHAR(10),ngayChungTu,103) FROM dbo.tblChungTuChi WHERE loaiChungTupr_sd IN ('010','011') AND sttHopDongpr_sd 
                                =(SELECT CASE WHEN laHopDong=1 THEN sttHopDongpr_sd ELSE 0 END FROM dbo.tblThanhLy WHERE sttThanhLypr='" + sttThanhLypr + @"')
                                UNION ALL
                                SELECT sttCTChipr,loaiChungTupr_sd,soChungTu,ngayChungTu=CONVERT(VARCHAR(10),ngayChungTu,103) FROM dbo.tblChungTuChi WHERE loaiChungTupr_sd IN ('010','011') AND sttHopDongpr_sd 
                                =(SELECT CASE WHEN laHopDong=1 THEN 0 ELSE sttHopDongpr_sd END FROM dbo.tblThanhLy WHERE sttThanhLypr='" + sttThanhLypr + @"')
                                )AS temp ORDER BY  temp.loaiChungTupr_sd DESC,temp.ngayChungTu asc");
                    var stt = 0; var group = "";
                    var soTTXuat = 0;
                    foreach (DataRow dr in tab.Rows)
                    {
                        if (group != dr["loaiChungTu"].ToString())
                        {
                            group = dr["loaiChungTu"].ToString();
                            stt = 0;
                        }
                        soTTXuat += 1;
                        stt += 1;
                        var tr = new DocumentFormat.OpenXml.Wordprocessing.TableRow();
                        var td1 = new DocumentFormat.OpenXml.Wordprocessing.TableCell();
                        var td2 = new DocumentFormat.OpenXml.Wordprocessing.TableCell();
                        var td3 = new DocumentFormat.OpenXml.Wordprocessing.TableCell();
                        var td4 = new DocumentFormat.OpenXml.Wordprocessing.TableCell();
                        var td5 = new DocumentFormat.OpenXml.Wordprocessing.TableCell();
                        td1.Append(new TableCellProperties(
                            new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }),
                            new Paragraph(
                                new ParagraphProperties(new Justification() { Val = JustificationValues.Left }),
                                new DocumentFormat.OpenXml.Wordprocessing.Run(
                                    new RunFonts { Ascii = "Times New Roman", HighAnsi = "Times New Roman" },
                                    new RunProperties(new DocumentFormat.OpenXml.Wordprocessing.FontSize { Val = "24" }),
                                    new DocumentFormat.OpenXml.Wordprocessing.Text(soTTXuat.ToString())
                                    )
                            )
                            );
                        td2.Append(new TableCellProperties(
                            new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }),
                            new Paragraph(
                                new ParagraphProperties(new Justification() { Val = JustificationValues.Left }),
                                new DocumentFormat.OpenXml.Wordprocessing.Run(
                                    new RunFonts { Ascii = "Times New Roman", HighAnsi = "Times New Roman" },
                                    new RunProperties(new DocumentFormat.OpenXml.Wordprocessing.FontSize { Val = "24" }),
                                    new DocumentFormat.OpenXml.Wordprocessing.Text(dr["loaiChungTu"].ToString() + " đợt " + stt.ToString())
                                    )
                            )
                            );
                        td3.Append(new TableCellProperties(
                            new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }),
                            new Paragraph(
                                new ParagraphProperties(new Justification() { Val = JustificationValues.Left }),
                                new DocumentFormat.OpenXml.Wordprocessing.Run(
                                    new RunFonts { Ascii = "Times New Roman", HighAnsi = "Times New Roman" },
                                    new RunProperties(new DocumentFormat.OpenXml.Wordprocessing.FontSize { Val = "24" }),
                                    new DocumentFormat.OpenXml.Wordprocessing.Text(dr["soChungTu"].ToString())
                                    )
                            )
                            );
                        td4.Append(new TableCellProperties(
                           new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }),
                           new Paragraph(
                               new ParagraphProperties(new Justification() { Val = JustificationValues.Left }),
                               new DocumentFormat.OpenXml.Wordprocessing.Run(
                                   new RunFonts { Ascii = "Times New Roman", HighAnsi = "Times New Roman" },
                                   new RunProperties(new DocumentFormat.OpenXml.Wordprocessing.FontSize { Val = "24" }),
                                   new DocumentFormat.OpenXml.Wordprocessing.Text(dr["ngayChungTu"].ToString())
                                   )
                           )
                           );
                        td5.Append(new TableCellProperties(
                           new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }),
                           new Paragraph(
                               new ParagraphProperties(new Justification() { Val = JustificationValues.Right }),
                               new DocumentFormat.OpenXml.Wordprocessing.Run(
                                   new RunFonts { Ascii = "Times New Roman", HighAnsi = "Times New Roman" },
                                   new RunProperties(new DocumentFormat.OpenXml.Wordprocessing.FontSize { Val = "24" }),
                                   new DocumentFormat.OpenXml.Wordprocessing.Text(decimal.Parse(dr["soTien"].ToString()).ToString("N0"))
                                   )
                           )
                           );
                        tr.Append(td1, td2, td3, td4, td5);
                        tables[3].Append(tr);
                    }
                    //Tổng chân
                    var trx = new DocumentFormat.OpenXml.Wordprocessing.TableRow();
                    var td1x = new DocumentFormat.OpenXml.Wordprocessing.TableCell();
                    var td2x = new DocumentFormat.OpenXml.Wordprocessing.TableCell();
                    var td3x = new DocumentFormat.OpenXml.Wordprocessing.TableCell();
                    var td4x = new DocumentFormat.OpenXml.Wordprocessing.TableCell();
                    var td5x = new DocumentFormat.OpenXml.Wordprocessing.TableCell();
                    td2x.Append(new TableCellProperties(
                           new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }),
                           new Paragraph(
                               new ParagraphProperties(new Justification() { Val = JustificationValues.Right }),
                               new DocumentFormat.OpenXml.Wordprocessing.Run(
                                   new RunFonts { Ascii = "Times New Roman", HighAnsi = "Times New Roman" },
                                   new RunProperties(new DocumentFormat.OpenXml.Wordprocessing.FontSize { Val = "24" }),
                                      new RunProperties(new DocumentFormat.OpenXml.Wordprocessing.Bold { Val = true }),
                                   new DocumentFormat.OpenXml.Wordprocessing.Text("Tổng cộng")
                                   )
                           )
                           );
                    td5x.Append(new TableCellProperties(
                           new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }),
                           new Paragraph(
                               new ParagraphProperties(new Justification() { Val = JustificationValues.Right }),
                               new DocumentFormat.OpenXml.Wordprocessing.Run(
                                   new RunFonts { Ascii = "Times New Roman", HighAnsi = "Times New Roman" },
                                   new RunProperties(new DocumentFormat.OpenXml.Wordprocessing.FontSize { Val = "24" }),
                                   new RunProperties(new DocumentFormat.OpenXml.Wordprocessing.Bold { Val = true }),
                                   new DocumentFormat.OpenXml.Wordprocessing.Text(decimal.Parse(tab.Compute("Sum(soTien)", "").ToString()).ToString("N0"))
                                   )
                           )
                           );
                    trx.Append(td1x, td2x, td3x, td4x, td5x);
                    tables[3].Append(trx);
                    #endregion

                    #region ghi chi tiet thanh ly
                    tab = new DataTable();
                    tab = sqlFunc.GetData(@"select tenCongViec=(select tenCPDauTuXD FROM tblDMCPDauTuXDCT WHERE maCPDauTuXDCTpr=maCPDauTuXDCTpr_sd) 
                    ,donViTinh,khoiLuong,donGia,giaTriThanhLy from dbo.tblThanhLyCT WHERE sttThanhLypr_sd='" + sttThanhLypr + "'");
                    stt = 0;
                    foreach (DataRow dr in tab.Rows)
                    {
                        stt += 1;
                        var tr = new DocumentFormat.OpenXml.Wordprocessing.TableRow();
                        var td = new DocumentFormat.OpenXml.Wordprocessing.TableCell();
                        var td1 = new DocumentFormat.OpenXml.Wordprocessing.TableCell();
                        var td2 = new DocumentFormat.OpenXml.Wordprocessing.TableCell();
                        var td3 = new DocumentFormat.OpenXml.Wordprocessing.TableCell();
                        var td4 = new DocumentFormat.OpenXml.Wordprocessing.TableCell();
                        var td5 = new DocumentFormat.OpenXml.Wordprocessing.TableCell();

                        td.Append(new TableCellProperties(
                            new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }),
                            new Paragraph(
                                new ParagraphProperties(new Justification() { Val = JustificationValues.Left }),
                                new DocumentFormat.OpenXml.Wordprocessing.Run(
                                    new RunFonts { Ascii = "Times New Roman", HighAnsi = "Times New Roman" },
                                    new RunProperties(new DocumentFormat.OpenXml.Wordprocessing.FontSize { Val = "24" }),
                                    new DocumentFormat.OpenXml.Wordprocessing.Text(stt.ToString())
                                    )
                            )
                            );
                        td1.Append(new TableCellProperties(
                            new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }),
                            new Paragraph(
                                new ParagraphProperties(new Justification() { Val = JustificationValues.Left }),
                                new DocumentFormat.OpenXml.Wordprocessing.Run(
                                    new RunFonts { Ascii = "Times New Roman", HighAnsi = "Times New Roman" },
                                    new RunProperties(new DocumentFormat.OpenXml.Wordprocessing.FontSize { Val = "24" }),
                                    new DocumentFormat.OpenXml.Wordprocessing.Text(dr["tenCongViec"].ToString())
                                    )
                            ));
                        td2.Append(new TableCellProperties(
                           new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }),
                           new Paragraph(
                               new ParagraphProperties(new Justification() { Val = JustificationValues.Left }),
                               new DocumentFormat.OpenXml.Wordprocessing.Run(
                                   new RunFonts { Ascii = "Times New Roman", HighAnsi = "Times New Roman" },
                                   new RunProperties(new DocumentFormat.OpenXml.Wordprocessing.FontSize { Val = "24" }),
                                   new DocumentFormat.OpenXml.Wordprocessing.Text(dr["donViTinh"].ToString())
                                   )
                           ));
                        td3.Append(new TableCellProperties(
                          new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }),
                          new Paragraph(
                              new ParagraphProperties(new Justification() { Val = JustificationValues.Left }),
                              new DocumentFormat.OpenXml.Wordprocessing.Run(
                                  new RunFonts { Ascii = "Times New Roman", HighAnsi = "Times New Roman" },
                                  new RunProperties(new DocumentFormat.OpenXml.Wordprocessing.FontSize { Val = "24" }),
                                  new DocumentFormat.OpenXml.Wordprocessing.Text(decimal.Parse(dr["khoiLuong"].ToString()).ToString("N0"))
                                  )
                          ));
                        td4.Append(new TableCellProperties(
                          new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }),
                          new Paragraph(
                              new ParagraphProperties(new Justification() { Val = JustificationValues.Left }),
                              new DocumentFormat.OpenXml.Wordprocessing.Run(
                                  new RunFonts { Ascii = "Times New Roman", HighAnsi = "Times New Roman" },
                                  new RunProperties(new DocumentFormat.OpenXml.Wordprocessing.FontSize { Val = "24" }),
                                  new DocumentFormat.OpenXml.Wordprocessing.Text(decimal.Parse(dr["donGia"].ToString()).ToString("N0"))
                                  )
                          ));
                        td5.Append(new TableCellProperties(
                          new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }),
                          new Paragraph(
                              new ParagraphProperties(new Justification() { Val = JustificationValues.Left }),
                              new DocumentFormat.OpenXml.Wordprocessing.Run(
                                  new RunFonts { Ascii = "Times New Roman", HighAnsi = "Times New Roman" },
                                  new RunProperties(new DocumentFormat.OpenXml.Wordprocessing.FontSize { Val = "24" }),
                                  new DocumentFormat.OpenXml.Wordprocessing.Text(decimal.Parse(dr["giaTriThanhLy"].ToString()).ToString("N0"))
                                  )
                          ));
                        tr.Append(td, td1, td2, td3, td4, td5);
                        tables[2].Append(tr);
                    }
                    #endregion
                    #region ghi mục 3 hóa đơn bên A đã xuất cho bên B

                    #endregion
                    wordDoc.MainDocumentPart.Document.Save();
                    wordDoc.Close();
                }

                return sqlFunc.DocxToHtml("/xuatword/" + HttpContext.Current.Session.GetCurrentUserID() + "/" + fileName);

            }
            catch
            {
                return sqlFunc.DocxToHtml("/xuatword/" + HttpContext.Current.Session.GetCurrentUserID() + "/" + fileName);
            }
        }
    }
}