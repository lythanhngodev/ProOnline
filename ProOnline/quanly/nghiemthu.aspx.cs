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

namespace ProOnline.quanly
{
    public partial class nghiemthuhopdong : System.Web.UI.Page
    {
        [AjaxPro.AjaxMethod]
        public string checkDuyetQT(string sttDuAn)
        {
            SqlFunction sqlFunc = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            return sqlFunc.GetOneStringField("select convert(char(1), duyetQToan) from tblDuAn where sttDuAnpr='" + sttDuAn + "'");
        }
        [AjaxPro.AjaxMethod]
        public DataTable chonMacDinhDuAn(string sttDuAn)
        {
            SqlFunction sqlFunc = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            return sqlFunc.GetData("SELECT sttDuAnpr, maDuAn, tenDuAn FROM tblDuAn WHERE sttDuAnpr = " + Session["ntsSTTDuAn"]);
        }
        [AjaxPro.AjaxMethod]
        public string SoDongGrid2(string sttDuAn)
        {
            return Session["Grid2_SoDong"] == null ? "0" : Session["Grid2_SoDong"].ToString();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.AppendHeader("X-XSS-Protection", "0");
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ProOnline.Class.ntsLibraryFunctions));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ProOnline.quanly.nghiemthuhopdong));
            if (!IsPostBack)
            { 
                Grid2.DataSource = null;
                Grid2.DataBind();
                Grid1.DataSource = null;
                Grid1.DataBind();
                cboHopDong.DataSource = null;
                cboHopDong.DataTextField = "sttHopDongpr";
                cboHopDong.DataValueField = "noidung";
                cboHopDong.DataBind(); 
              
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
                string duongdanfilecu = sqlFun.GetOneStringField(@"select top 1 duongDanFile from  dbo.tblNghiemThu WHERE sttNghiemThupr='" + hdfSttNT.Value.ToString() + "'");
                //lấy mã dự án
                string sttDuAnpr = sqlFun.GetOneStringField("select top 1 CONVERT(NVARCHAR(50),sttDuAnpr_sd) from tblNghiemThu where sttNghiemThupr='" + hdfSttNT.Value.ToString() + "'");
                string tenFile = e.filename;//"VanBan" + hdfSTTVanBan.Value + ".pdf";
                if (!System.IO.Directory.Exists(Server.MapPath("~/VanBan/" + HttpContext.Current.Session.GetDonVi().maDonVi + "/" + sttDuAnpr + "/nghiemThu")))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/VanBan/" + HttpContext.Current.Session.GetDonVi().maDonVi + "/" + sttDuAnpr + "/nghiemThu"));
                }
                string strDate = DateTime.Now.ToString("dd_MM_yy_hhmmss");
                string fileExtension = Path.GetExtension(tenFile).Replace("-", "");
                tenFile = tenFile.Substring(tenFile.LastIndexOf("\\\\") + 1);
                tenFile = tenFile.Substring(0, tenFile.LastIndexOf(fileExtension)) + strDate + fileExtension;
                tenFile = tenFile.Replace(" ", "");
                //path = string.Concat(Server.MapPath("/VanBan/" + HttpContext.Current.Session.GetDonVi().maDonVi + "/KHoachDThau/"));
                path = string.Concat(Server.MapPath("/VanBan/" + HttpContext.Current.Session.GetDonVi().maDonVi + "/" + sttDuAnpr + "/nghiemThu/" + tenFile + ""));

                if (!System.IO.File.Exists(path))
                {
                    //save lại file mới
                    AsyncFileUpload.SaveAs(path);
                    urlFile = "/VanBan/" + HttpContext.Current.Session.GetDonVi().maDonVi + "/" + sttDuAnpr + "/nghiemThu/" + tenFile + "";
                    sqlFun.ExeCuteNonQuery("UPDATE dbo.tblNghiemThu SET duongDanFile=N'" + urlFile + "' WHERE sttNghiemThupr='" + hdfSttNT.Value.ToString() + "'");
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
        [AjaxPro.AjaxMethod]
        public string checkDuyetQToan(string sttDuAn)
        {
            SqlFunction _sqlfun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            return _sqlfun.GetOneStringField("select (case when duyetQToan = 1 then '1' else '0' end) from tblDuAn where sttDuAnpr='" + sttDuAn + "'");
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
        protected void Grid2_OnRebind(object sender, EventArgs e)
        {
            try
            { 
            string sql = @"select sttNghiemThuCTpr,maCPDauTuXDCTpr_sd,sttHDCTpr_sd
                    ,loai=(case when (select laHopDong from dbo.tblNghiemThu where sttNghiemThupr=sttNghiemThupr_sd)=1 then 'HD' else 'PL' end)
                    ,tenCongViec=(select tenCPDauTuXD FROM tblDMCPDauTuXDCT WHERE maCPDauTuXDCTpr=maCPDauTuXDCTpr_sd) 
                    ,giaTrungThau=(case when (select laHopDong from dbo.tblNghiemThu where sttNghiemThupr=sttNghiemThupr_sd)=1 
                    then (select giaTrungThau from tblHopDongCT where sttHDCTpr=tblNghiemThuCT.sttHDCTpr_sd)
                    else (select giaTrungThau from tblPhuLucHDCT where sttPLHDCTpr=tblNghiemThuCT.sttHDCTpr_sd) end) 
                    ,giaTriHD=ISNULL((case when (select laHopDong from dbo.tblNghiemThu where sttNghiemThupr=sttNghiemThupr_sd)=1
                    then (select giaTriHD from tblHopDongCT where sttHDCTpr=tblNghiemThuCT.sttHDCTpr_sd)
                    else (SELECT giaTriHD from tblHopDongCT WHERE tblHopDongCT.maCPDauTuXDCTpr_sd=tblNghiemThuCT.maCPDauTuXDCTpr_sd and
                      sttHopDongpr_sd =(SELECT sttHopDongpr_sd FROM dbo.tblPhuLucHD 
                    WHERE sttPLHDpr= (SELECT sttPLHDpr_sd FROM dbo.tblPhuLucHDCT 
                    WHERE  sttPLHDCTpr=tblNghiemThuCT.sttHDCTpr_sd))) end),0) 
                    ,giaTriHDDC=(case when (select laHopDong from dbo.tblNghiemThu where sttNghiemThupr=sttNghiemThupr_sd)=1 
                    then
                    (select giaTriHD from tblHopDongCT where sttHDCTpr=tblNghiemThuCT.sttHDCTpr_sd AND tblHopDongCT.maCPDauTuXDCTpr_sd=tblNghiemThuCT.maCPDauTuXDCTpr_sd)
                    -(SELECT ISNULL(SUM(giaTriDieuChinhGiam),0) from tblPhuLucHDCT where sttPLHDpr_sd in 
                    (select sttPLHDpr from tblPhuLucHD where sttHopDongpr_sd =(SELECT sttHopDongpr_sd FROM dbo.tblHopDongCT WHERE sttHDCTpr=tblNghiemThuCT.sttHDCTpr_sd)) AND dbo.tblPhuLucHDCT.maCPDauTuXDCTpr_sd=dbo.tblNghiemThuCT.maCPDauTuXDCTpr_sd) 
                    +(SELECT ISNULL(SUM(giaTriDieuChinhTang),0) from tblPhuLucHDCT where sttPLHDpr_sd in 
                    (select sttPLHDpr from tblPhuLucHD where sttHopDongpr_sd = (SELECT sttHopDongpr_sd FROM dbo.tblHopDongCT WHERE sttHDCTpr=tblNghiemThuCT.sttHDCTpr_sd)) AND dbo.tblPhuLucHDCT.maCPDauTuXDCTpr_sd=dbo.tblNghiemThuCT.maCPDauTuXDCTpr_sd)  
                    else 
					(select giaTriPL from tblPhuLucHDCT where sttPLHDCTpr=tblNghiemThuCT.sttHDCTpr_sd)
					end)
                    ,giaTriNghiemThu
                    ,giaTriNghiemThuLK=(case when (select laHopDong from dbo.tblNghiemThu where sttNghiemThupr=sttNghiemThupr_sd)=1 
                    then
                        (SELECT SUM(a.giaTriNghiemThu) FROM dbo.tblNghiemThuCT a WHERE  a.sttHDCTpr_sd=tblNghiemThuCT.sttHDCTpr_sd
                        AND a.sttNghiemThupr_sd IN (SELECT sttNghiemThupr FROM dbo.tblNghiemThu WHERE laHopDong=1 and sttDuAnpr_sd=N'" + hdfSttDuAnpr_uc.Value + @"' 
                        AND ngayLap<='" + ntsLibraryFunctions._mChuyenChuoiSangNgay(ngayLap.Text)  + @"'))
                    else    
                        (SELECT SUM(a.giaTriNghiemThu) FROM dbo.tblNghiemThuCT a WHERE  a.sttHDCTpr_sd=tblNghiemThuCT.sttHDCTpr_sd
                        AND a.sttNghiemThupr_sd IN (SELECT sttNghiemThupr FROM dbo.tblNghiemThu WHERE laHopDong=0 and sttDuAnpr_sd=N'" + hdfSttDuAnpr_uc.Value + @"' 
                        AND ngayLap<='" + ntsLibraryFunctions._mChuyenChuoiSangNgay(ngayLap.Text) + @"'))
                    end)
                    ,thoiGian,taiLieu=taiLieuThamChieu,thayDoi=thayDoiSoVoiThietKe,ketLuan,yeuCauKhac 
                    from tblNghiemThuCT  WHERE sttNghiemThupr_sd='" + hdfSttNT.Value + "'";
            SqlFunction sqlFunc = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            DataTable dt = sqlFunc.GetData(sql);
            Grid2.DataSource = dt;
            Grid2.DataBind();
            }
            catch (Exception)
            {
                Grid2.DataSource = null;
                Grid2.DataBind();
            }
        }
        protected void Grid1_OnRebind(object sender, EventArgs e)
        {
            try
            {
                SqlFunction sqlFunc = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
//                string sql = @"select sttNghiemThupr,sttDuAnpr_sd,
//		            sttHopDongpr_sd=(case when laHopDong=1 then 
//			            'HD-'+ convert(varchar(18),sttHopDongpr_sd)+'-'+convert(varchar(18),sttDauThaupr_sd) else 
//				            'PL-'+ convert(varchar(18),sttHopDongpr_sd)+'-'+convert(varchar(18),sttDauThaupr_sd)
//		            end)
//                    ,tenDuAn=(select tenDuAn from tblDuAn where sttDuAnpr=sttDuAnpr_sd)
//                    ,maDuAn=(select maDuAn from tblDuAn where sttDuAnpr=sttDuAnpr_sd)
//                    ,soBienBan,ngayLap=convert(varchar(10),ngayLap,103),cacCanCu
//                    ,soHopDong=(case when laHopDong = 1 then (select soHopDong from tblHopDong where sttHopDongpr=sttHopDongpr_sd)
//                                else (select soPhuLuc from tblPhuLucHD where sttPLHDpr=tblNghiemThu.sttHopDongpr_sd) end)
//                    ,giaTriNT=(select SUM(isnull(giaTriNghiemThu,0)) from tblNghiemThuCT where sttNghiemThupr=sttNghiemThupr_sd) 
//                    from tblNghiemThu where maDonVipr_sd=N'" + HttpContext.Current.Session.GetDonVi().maDonVi + "' and sttVBDApr_sd IN (SELECT sttVBDApr FROM dbo.tblVanBanDA WHERE maLoaiVBanpr_sd IN (SELECT maLoaiVBanpr FROM dbo.tblDMLoaiVanBan WHERE maLoaiVBanpr_cha='XXIII'))";
               // VỊnh chỉnh ngày 11/01/2019
                string sql = @"select sttNghiemThupr,sttDuAnpr_sd,
		            sttHopDongpr_sd=(case when laHopDong=1 then 
			            'HD-'+ convert(varchar(18),sttHopDongpr_sd)+'-'+convert(varchar(18),sttDauThaupr_sd) else 
				            'PL-'+ convert(varchar(18),sttHopDongpr_sd)+'-'+convert(varchar(18),sttDauThaupr_sd)
		            end)
                    ,tenDuAn=(select tenDuAn from tblDuAn where sttDuAnpr=sttDuAnpr_sd)
                    ,maDuAn=(select maDuAn from tblDuAn where sttDuAnpr=sttDuAnpr_sd)
                    ,soBienBan,ngayLap=convert(varchar(10),ngayLap,103),cacCanCu
                    ,soHopDong=(case when laHopDong = 1 then (select soHopDong from tblHopDong where sttHopDongpr=sttHopDongpr_sd)
                                else (select soPhuLuc from tblPhuLucHD where sttPLHDpr=tblNghiemThu.sttHopDongpr_sd) end)
                    ,giaTriNT=(select SUM(isnull(giaTriNghiemThu,0)) from tblNghiemThuCT where sttNghiemThupr=sttNghiemThupr_sd) 
                    ,taiBienBan  = N'<a href=""#"" onclick=""xemVB(''' +duongDanFile + N''')"">'+ISNULL(replace(ltrim(right(duongDanFile,CHARINDEX('/',REVERSE(duongDanFile),0))),'/',''),'')+'</a>'
                    from tblNghiemThu where maDonVipr_sd=N'" + HttpContext.Current.Session.GetDonVi().maDonVi + "'";
                
                Grid1.DataSource = sqlFunc.GetData(sql);
                Grid1.DataBind();
            }
            catch
            {
            }
        }
        [AjaxPro.AjaxMethod]
        public static string kiemTraCongViecNghiemThu(string comBoCongViec, string sttNghiemThu)
        {
            SqlFunction sqlFunc = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            string sql = @"select convert(varchar(18), sttNghiemThupr) from tblNghiemThu where laHopDong=" + (comBoCongViec.Split('@')[0].ToString() == "HD" ? "1" : "0") + @" and 
            sttNghiemThupr =(select top 1 sttNghiemThupr_sd from tblNghiemThuCT where sttNghiemThuCTpr<>'" + (sttNghiemThu == "" ? "0" : sttNghiemThu) + @"' and sttHDCTpr_sd='" + comBoCongViec.Split('@')[1].ToString() + @"' and maCPDauTuXDCTpr_sd='" + comBoCongViec.Split('@')[2].ToString() + "')";
            return sqlFunc.GetOneStringField(sql);
        }
        [AjaxPro.AjaxMethod]
        public DataTable getDanhSachHopDong(string sttDuAn)
        {
            string sql = @"select '0' sttHopDongpr,'' noidung union all select sttHopDongpr,noidung=soHopDong + N' thuộc gói thầu ' +(select tenGoiThau from tblDauThau where sttDauThaupr=sttDauThaupr_sd) from
                        (select sttHopDongpr= 'HD'+'-'+ convert(varchar(18),sttHopDongpr)+'-'+ convert(varchar(18),sttDauThaupr_sd),soHopDong=(N'Hợp đồng số ' +soHopDong),sttDauThaupr_sd from tblHopDong where ngoaiHopDong=0 and sttDuAnpr_sd='" + sttDuAn + @"'
                        union all
                        select 'PL'+'-'+ convert(varchar(18),sttPLHDpr)+'-'+(select convert(varchar(18),sttDauThaupr_sd) from tblHopDong where sttHopDongpr=sttHopDongpr_sd),N'Phụ lục hợp đồng số ' +soPhuLuc,sttDauThaupr_sd=(select sttDauThaupr_sd from tblHopDong where sttHopDongpr=sttHopDongpr_sd) 
                        from tblPhuLucHD where sttHopDongpr_sd in(select sttHopDongpr from tblHopDong where ngoaiHopDong=0 and sttDuAnpr_sd='" + sttDuAn + @"'))as temp";
            SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            return sqlFun.GetData(sql);
        }
        private string _mChuyenChuoiSangNgay(string ddMMyyyy)
        {
            return ddMMyyyy.Substring(3, 2) + "/" + ddMMyyyy.Substring(0, 2) + "/" + ddMMyyyy.Substring(6, 4);
        }
        [AjaxPro.AjaxMethod]
        public DataTable getDanhSachCongViec(string sttHD)
        {
            string sql = "";
            if (sttHD.Split('-')[0].ToString() == "HD")
                sql = @"select '' stt,'' noiDung union all select stt='HD@'+(convert(varchar(18),sttHDCTpr)+'@'+maCPDauTuXDCTpr_sd)
                ,noiDung=(SELECT tenCPDauTuXD FROM tblDMCPDauTuXDCT WHERE maCPDauTuXDCTpr=maCPDauTuXDCTpr_sd)
                from tblHopDongCT where sttHopDongpr_sd='" + sttHD.Split('-')[1].ToString() + "'";
            else
            {
                sql = @"select '' stt,'' noiDung union all select stt='PL@'+(convert(varchar(18),sttPLHDCTpr)+'@'+maCPDauTuXDCTpr_sd)
                        ,noiDung=(SELECT tenCPDauTuXD FROM tblDMCPDauTuXDCT WHERE maCPDauTuXDCTpr=maCPDauTuXDCTpr_sd)
                        from tblPhuLucHDCT where sttPLHDpr_sd in(select sttPLHDpr from tblPhuLucHD 
                        where sttPLHDpr_sd='" + sttHD.Split('-')[1].ToString() + "')";
            }
            SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            return sqlFun.GetData(sql);
        }
        [AjaxPro.AjaxMethod]
        public string ktraChoChonLaiHopDong()
        {
            return "";
        }
       [AjaxPro.AjaxMethod]
        public string getSttVanBan(string sttNghiemThupr)
        {
            SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            return sqlFun.GetOneStringField("SELECT CONVERT(NVARCHAR(18),sttVBDApr_sd) FROM dbo.tblNghiemThu WHERE sttNghiemThupr='" + sttNghiemThupr + "'");
        }
        [AjaxPro.AjaxMethod]
        public string capNhatGiaTriVanBan(string sttNghiemThupr)
       {
           //Cập nhật giá trị văn bản khi thay đổi gói thầu
           //Cập nhật giá trị văn bản khi thay đổi chi tiết hợp đồng 
            SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            return sqlFun.GetOneDecimalField(@" SELECT 
                 (CASE WHEN  (ISNULL((SELECT SUM(giaTri) FROM dbo.tblVanBanDA  WHERE sttVBDApr='" + getSttVanBan(sttNghiemThupr) + @"'),0)=0)
                 THEN (SELECT ISNULL(SUM(giaTriNghiemThu),0) FROM dbo.tblNghiemThuCT WHERE sttNghiemThupr_sd='" + sttNghiemThupr + @"')
                 ELSE (SELECT ISNULL(SUM(giaTri),0) FROM dbo.tblVanBanDA WHERE sttVBDApr='" + getSttVanBan(sttNghiemThupr) + "') END)").ToString();
        }
        [AjaxPro.AjaxMethod]
        public string luuThongTin(object[] param, string flag)
        {
            SqlConnection sqlCon = new SqlConnection(HttpContext.Current.Session.GetConnectionString2());
            SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());

            SqlConnection sqlConn = new SqlConnection(HttpContext.Current.Session.GetConnectionString2());
            try
            {
                string sql = ""; 
                DataTable _dtTemp = new DataTable();
                _dtTemp = new DataTable(); 
                // Kiểm tra có đổi hợp đồng không
                string sttHopDong = sqlFun.GetOneStringField("SELECT convert(nvarchar(50),sttHopDongpr_sd) FROM dbo.tblNghiemThu WHERE sttNghiemThupr = " + param[0].ToString());
                if (sttHopDong != param[2].ToString().Split('-')[1].ToString() && sttHopDong != "")
                {
                    xoaThongTin(param[0].ToString(), "0");
                   return  luuThongTin(param, "0");
                }
                //var sttVanBan = "";

                //string sqlQuery = "";
                  

                if (flag == "0")
                {
//                    sql = @"INSERT INTO dbo.tblVanBanDA(giaTri,soVanBan,noiDung, sttDuAnpr_sd,ngayKy,maLoaiVBanpr_sd,maDonVipr_sd ,nguoiThaoTac ,ngayThaotac)
//                            select @giaTri,@soVanBan,@noiDung,@sttDuAnpr_sd,@ngayKy,@maLoaiVBanpr_sd,@maDonVipr_sd ,@nguoiThaoTac ,getdate()";

//                if (sqlCon.State == ConnectionState.Closed)
//                    sqlCon.Open();
//                SqlCommand cmdVanBan = new SqlCommand(sql, sqlCon);
//                cmdVanBan.Parameters.AddWithValue("@sttDuAnpr_sd", param[1].ToString());
//                cmdVanBan.Parameters.AddWithValue("@soVanBan", param[4].ToString());
//                cmdVanBan.Parameters.AddWithValue("@ngayKy", _mChuyenChuoiSangNgay(param[5].ToString()));
//                cmdVanBan.Parameters.AddWithValue("@noiDung", param[3].ToString());
//                cmdVanBan.Parameters.AddWithValue("@giaTri", "0");
//                cmdVanBan.Parameters.AddWithValue("@maLoaiVBanpr_sd", sqlFun.GetOneStringField("SELECT top 1 maLoaiVBanpr FROM dbo.tblDMLoaiVanBan WHERE maLoaiVBanpr_cha='XXIII'"));
//                cmdVanBan.Parameters.AddWithValue("@maDonVipr_sd", Session.GetDonVi().maDonVi);
//                cmdVanBan.Parameters.AddWithValue("@nguoiThaoTac", Session.GetCurrentUserID());
//                if (cmdVanBan.ExecuteNonQuery() > 0)
//                    sttVanBan = sqlFun.GetOneStringField("SELECT CONVERT(NVARCHAR(50),MAX(sttVBDApr)) FROM dbo.tblVanBanDA WHERE sttDuAnpr_sd='" + param[1].ToString() + "' and maLoaiVBanpr_sd=( SELECT top 1 maLoaiVBanpr FROM dbo.tblDMLoaiVanBan WHERE maLoaiVBanpr_cha='XXIII')");
//                else
//                    return "0_Không tồn tại biên bản nghiệm thu!";
                    sql = @"INSERT INTO tblNghiemThu(sttVBDApr_sd,sttDuAnpr_sd,sttDauThaupr_sd,sttHopDongpr_sd,cacCanCu,soBienBan,ngayLap,maDonVipr_sd,nguoiThaoTac,ngayThaotac,laHopDong)
                            VALUES (@sttVBDApr_sd,@sttDuAnpr_sd,@sttDauThaupr_sd,@sttHopDongpr_sd,@cacCanCu,@soBienBan,@ngayLap,@maDonVipr_sd,@nguoiThaoTac,getdate(),@laHopDong)";
                }
                else
                {
                    //if (getSttVanBan(param[0].ToString()) == "0")
                    //    return "0_Không tồn tại biên bản nghiệm thu!";
                    sql = @"UPDATE tblNghiemThu set
                        soBienBan=@soBienBan,cacCanCu = @cacCanCu ,ngayLap = @ngayLap ,nguoiThaoTac = @nguoiThaoTac,maDonVipr_sd=@maDonVipr_sd ,laHopDong=@laHopDong,
                        sttHopDongpr_sd=@sttHopDongpr_sd, sttDauThaupr_sd=@sttDauThaupr_sd WHERE sttNghiemThupr=@sttNghiemThupr"; 
                }
                SqlCommand cmd = new SqlCommand(sql);
                sqlConn.Open();
                cmd.Connection = sqlConn;
                cmd.Parameters.AddWithValue("@sttDuAnpr_sd", param[1].ToString());
                cmd.Parameters.AddWithValue("@sttNghiemThupr", param[0].ToString());
                cmd.Parameters.AddWithValue("@sttHopDongpr_sd", param[2].ToString().Split('-')[1].ToString());
                cmd.Parameters.AddWithValue("@sttDauThaupr_sd", param[2].ToString().Split('-')[2].ToString());
                if (param[2].ToString().Split('-')[0].ToString() == "HD")
                    cmd.Parameters.AddWithValue("@laHopDong", "1");
                else
                    cmd.Parameters.AddWithValue("@laHopDong", "0");
                cmd.Parameters.AddWithValue("@cacCanCu", param[3].ToString());
                cmd.Parameters.AddWithValue("@sttVBDApr_sd", DBNull.Value);
                
                cmd.Parameters.AddWithValue("@soBienBan", param[4].ToString());
                cmd.Parameters.AddWithValue("@ngayLap", _mChuyenChuoiSangNgay(param[5].ToString()));
                cmd.Parameters.AddWithValue("@maDonVipr_sd", HttpContext.Current.Session.GetDonVi().maDonVi);
                cmd.Parameters.AddWithValue("@nguoiThaoTac", HttpContext.Current.Session.GetCurrentUserID());

                if (cmd.ExecuteNonQuery() > 0)
                {
                    string sttNghiemThupr = "0";
                    if (flag == "0")
                        sttNghiemThupr = sqlFun.GetOneStringField("select convert(varchar(18), max(sttNghiemThupr)) from tblNghiemThu where maDonVipr_sd='" + HttpContext.Current.Session.GetDonVi().maDonVi + "'");
                    else
                        sttNghiemThupr = param[0].ToString();
                    sqlFun.ExeCuteNonQuery("UPDATE dbo.tblKhoiLuongHoanThanh SET soBBNghiemThu=N'" + param[4].ToString() + "', ngayNghiemThu='" + ntsLibraryFunctions._mChuyenChuoiSangNgay(param[5].ToString()) + "' WHERE sttNghiemThupr_sd='" + sttNghiemThupr + "'");
 
                    capNhatGiaTriNTHopDong(param[2].ToString().Split('-')[0].ToString(), param[2].ToString().Split('-')[1].ToString());
                    if (flag == "0")
                    {
                        //lưu thông tin nghiệm thu chi tiết
                        SqlFunction sqlFunc = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                        _dtTemp = new DataTable();
                        //họp đồng
                        if (param[2].ToString().Split('-')[0].ToString() == "HD")
                        {
                            _dtTemp = sqlFunc.GetData(@"select 'HD@'+CONVERT(varchar(18),sttHDCTpr)+'@'+CONVERT(varchar(18),maCPDauTuXDCTpr_sd) AS sttHDCTpr,maCPDauTuXDCTpr_sd
                            ,tenCongViec=(select tenCPDauTuXD FROM tblDMCPDauTuXDCT WHERE maCPDauTuXDCTpr=maCPDauTuXDCTpr_sd) 
                            ,giaTrungThau,giaTriHD=giaTriHD
                                -(SELECT ISNULL(SUM(giaTriDieuChinhGiam),0) from tblPhuLucHDCT where sttPLHDpr_sd in 
                                (select sttPLHDpr from tblPhuLucHD where sttHopDongpr_sd = N'" + param[2].ToString().Split('-')[1].ToString() + @"' and dbo.tblPhuLucHDCT.maCPDauTuXDCTpr_sd=dbo.tblHopDongCT.maCPDauTuXDCTpr_sd))
                                +(SELECT ISNULL(SUM(giaTriDieuChinhTang),0) from tblPhuLucHDCT where sttPLHDpr_sd in 
                                (select sttPLHDpr from tblPhuLucHD where sttHopDongpr_sd = N'" + param[2].ToString().Split('-')[1].ToString() + @"' and
                                 dbo.tblPhuLucHDCT.maCPDauTuXDCTpr_sd=dbo.tblHopDongCT.maCPDauTuXDCTpr_sd))
                                -ISNULL((SELECT SUM(giaTriNghiemThu) FROM dbo.tblNghiemThuCT WHERE tblNghiemThuCT.maCPDauTuXDCTpr_sd=tblHopDongCT.maCPDauTuXDCTpr_sd and 
								sttNghiemThupr_sd IN(SELECT sttNghiemThupr FROM dbo.tblNghiemThu WHERE sttHopDongpr_sd= N'" + param[2].ToString().Split('-')[1].ToString() + @"')),0)
                            ,thoiGian='',taiLieu='',thayDoi='',ketLuan='',yeuCauKhac=''
                            from tblHopDongCT WHERE sttHopDongpr_sd = N'" + param[2].ToString().Split('-')[1].ToString() + "'");
                        }
                        else
                        {
                            //phụ lục
                            //  string sttPLHDpr_sd_ = sqlFunc.GetOneStringField("select convert(varchar(18), sttPLHDpr) from tblPhuLucHD where sttHopDongpr_sd= N'" + param[2].ToString().Split('-')[1].ToString() + "'");
                            _dtTemp = sqlFunc.GetData(@"select  'PL@'+CONVERT(varchar(18),sttPLHDCTpr)+'@'+CONVERT(varchar(18),maCPDauTuXDCTpr_sd) AS sttHDCTpr,maCPDauTuXDCTpr_sd
                        ,tenCongViec=(select tenCPDauTuXD FROM tblDMCPDauTuXDCT WHERE maCPDauTuXDCTpr=maCPDauTuXDCTpr_sd) 
                        ,giaTrungThau=ISNULL((SELECT giaTrungThau FROM dbo.tblHopDongCT WHERE sttHDCTpr=tblPhuLucHDCT.sttHDCTpr_sd),0)
                        ,giaTriHD=giaTriPL  -ISNULL((SELECT SUM(giaTriNghiemThu) FROM dbo.tblNghiemThuCT WHERE tblNghiemThuCT.maCPDauTuXDCTpr_sd=tblPhuLucHDCT.maCPDauTuXDCTpr_sd and 
						sttNghiemThupr_sd IN(SELECT sttNghiemThupr FROM dbo.tblNghiemThu WHERE sttHopDongpr_sd= N'" + param[2].ToString().Split('-')[1].ToString() + @"')),0)
                        ,thoiGian='',taiLieu='',thayDoi='',ketLuan='',yeuCauKhac=''
                        from tblPhuLucHDCT where sttPLHDpr_sd = N'" + param[2].ToString().Split('-')[1].ToString() + "'");
                        }
                        if (_dtTemp.Rows.Count != 0)
                        {
                            foreach (DataRow dr1 in _dtTemp.Rows)
                            {
                                string[] param_ = new string[10];
                                param_[0] = "";
                                param_[1] = sttNghiemThupr;
                                param_[2] = dr1["sttHDCTpr"].ToString();
                                param_[3] = dr1["giaTriHD"].ToString();
                                param_[4] = dr1["thoiGian"].ToString();
                                param_[5] = dr1["taiLieu"].ToString();
                                param_[6] = dr1["thayDoi"].ToString();
                                param_[7] = dr1["ketLuan"].ToString();
                                param_[8] = dr1["yeuCauKhac"].ToString();
                                param_[9] = param[2].ToString();
                                luuThongTinNghiemThuCT(param_, "0");

                            }
                        }
                        if (flag == "0")
                        {
                            sqlFun.ExeCuteNonQuery("update tblNghiemThu set giaTri = '" + sqlFun.GetOneDecimalField(@"SELECT ISNULL(SUM(giaTriNghiemThu),0) FROM dbo.tblNghiemThuCT WHERE sttNghiemThupr_sd='" + sttNghiemThupr + @"'") +"' where sttNghiemThupr = N'" + sttNghiemThupr + "'");
                        }
                        return sttNghiemThupr + "_Lưu thông tin nghiệm thu thành công!";
                    }
                       return sttNghiemThupr+"_Lưu thông tin nghiệm thu thành công!";
                }
                else
                {
                  //sqlFun.ExeCuteNonQuery("DELETE tblVanBanDA WHERE sttVBDApr='"+ sttVanBan +"'");
                    return "0_Lưu thông tin nghiệm thu không thành công";
                }  
            }
            catch
            {
              return "0_Lưu thông tin nghiệm thu không thành công";
            }
        }
        [AjaxPro.AjaxMethod]
        public static DataTable thongTinCongViec(string maCongViec, string flag)
        {
            SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            if (maCongViec == "") return sqlFun.GetData(@"select maCPDauTuXDCTpr_sd=''
                        ,tenCongViec='' ,giaTrungThau=0,giaTriHD=0 ,giaTriNghiemThu=0 ,thoiGian='',taiLieu='',thayDoi='',ketLuan='',yeuCauKhac=''");
            string sql = @"select 
                            giaTrungThau=(case when '" + maCongViec.Split('@')[0].ToString() + @"'= N'HD'
                            then (select giaTrungThau from tblHopDongCT where sttHDCTpr='" + maCongViec.Split('@')[1].ToString() + @"')
                            else (select giaTrungThau from tblPhuLucHDCT where sttPLHDCTpr='" + maCongViec.Split('@')[1].ToString() + @"') end) 
                            ,giaTriHD=(case when '" + maCongViec.Split('@')[0].ToString() + @"'= N'HD'
                            then 
	                            ISNULL((select giaTriHD from tblHopDongCT WHERE sttHDCTpr='" + maCongViec.Split('@')[1].ToString() + @"'),0)
                             else ISNULL((SELECT  giaTriHD from tblHopDongCT where maCPDauTuXDCTpr_sd=N'" + maCongViec.Split('@')[2].ToString() + @"'  and   
                                  sttHopDongpr_sd =(SELECT sttHopDongpr_sd FROM dbo.tblPhuLucHD 
                                WHERE sttPLHDpr= (SELECT sttPLHDpr_sd FROM dbo.tblPhuLucHDCT WHERE sttPLHDCTpr='" + maCongViec.Split('@')[1].ToString() + @"'))),0) 
                            end)
                            ,giaTriHDDC=(case when '" + maCongViec.Split('@')[0].ToString() + @"'= N'HD'
                            then
                            (select giaTriHD from tblHopDongCT where sttHDCTpr='" + maCongViec.Split('@')[1].ToString() + @"' AND tblHopDongCT.maCPDauTuXDCTpr_sd=N'" + maCongViec.Split('@')[2].ToString() + @"')
                            -(SELECT ISNULL(SUM(giaTriDieuChinhGiam),0) from tblPhuLucHDCT where sttPLHDpr_sd in 
                            (select sttPLHDpr from tblPhuLucHD where sttHopDongpr_sd =(SELECT sttHopDongpr_sd FROM dbo.tblHopDongCT WHERE sttHDCTpr='" + maCongViec.Split('@')[1].ToString() + @"')) AND dbo.tblPhuLucHDCT.maCPDauTuXDCTpr_sd=N'" + maCongViec.Split('@')[2].ToString() + @"') 
                            +(SELECT ISNULL(SUM(giaTriDieuChinhTang),0) from tblPhuLucHDCT where sttPLHDpr_sd in 
                            (select sttPLHDpr from tblPhuLucHD where sttHopDongpr_sd = (SELECT sttHopDongpr_sd FROM dbo.tblHopDongCT WHERE sttHDCTpr='" + maCongViec.Split('@')[1].ToString() + @"')) AND dbo.tblPhuLucHDCT.maCPDauTuXDCTpr_sd=N'" + maCongViec.Split('@')[2].ToString() + @"')  
                            else 
                             ISNULL((SELECT SUM(giaTriPL) FROM dbo.tblPhuLucHDCT WHERE sttPLHDCTpr='" + maCongViec.Split('@')[1].ToString() + @"'),0) 
                            end)
                            ,giaTriNThu=(case when '" + maCongViec.Split('@')[0].ToString() + @"'= N'HD'
                            then
                            (select giaTriHD from tblHopDongCT where sttHDCTpr='" + maCongViec.Split('@')[1].ToString() + @"' AND tblHopDongCT.maCPDauTuXDCTpr_sd=N'" + maCongViec.Split('@')[2].ToString() + @"')
                            -(SELECT ISNULL(SUM(giaTriDieuChinhGiam),0) from tblPhuLucHDCT where sttPLHDpr_sd in 
                            (select sttPLHDpr from tblPhuLucHD where sttHopDongpr_sd =(SELECT sttHopDongpr_sd FROM dbo.tblHopDongCT WHERE sttHDCTpr='" + maCongViec.Split('@')[1].ToString() + @"')) AND dbo.tblPhuLucHDCT.maCPDauTuXDCTpr_sd=N'" + maCongViec.Split('@')[2].ToString() + @"') 
                            +(SELECT ISNULL(SUM(giaTriDieuChinhTang),0) from tblPhuLucHDCT where sttPLHDpr_sd in 
                            (select sttPLHDpr from tblPhuLucHD where sttHopDongpr_sd = (SELECT sttHopDongpr_sd FROM dbo.tblHopDongCT WHERE sttHDCTpr='" + maCongViec.Split('@')[1].ToString() + @"')) AND dbo.tblPhuLucHDCT.maCPDauTuXDCTpr_sd=N'" + maCongViec.Split('@')[2].ToString() + @"')  
                            -(SELECT SUM(giaTriNghiemThu) FROM dbo.tblNghiemThuCT WHERE tblNghiemThuCT.maCPDauTuXDCTpr_sd=N'" + maCongViec.Split('@')[2].ToString() + @"' and 
								sttNghiemThupr_sd IN(SELECT sttNghiemThupr FROM dbo.tblNghiemThu WHERE sttHopDongpr_sd in (SELECT sttHopDongpr_sd FROM dbo.tblHopDongCT WHERE sttHDCTpr='" + maCongViec.Split('@')[1].ToString() + @"')))
                            else 
                             ISNULL((SELECT SUM(giaTriPL) FROM dbo.tblPhuLucHDCT WHERE sttPLHDCTpr='" + maCongViec.Split('@')[1].ToString() + @"'),0)
                                  -(SELECT SUM(giaTriNghiemThu) FROM dbo.tblNghiemThuCT WHERE tblNghiemThuCT.maCPDauTuXDCTpr_sd=N'" + maCongViec.Split('@')[2].ToString() + @"' and 
								sttNghiemThupr_sd IN(SELECT sttNghiemThupr FROM dbo.tblNghiemThu 
								WHERE sttHopDongpr_sd= (SELECT sttPLHDpr_sd FROM dbo.tblPhuLucHDCT WHERE sttPLHDCTpr='" + maCongViec.Split('@')[1].ToString() + @"')))
                            end)";
            return sqlFun.GetData(sql);
        }
        [AjaxPro.AjaxMethod]
        public   string luuThongTinNghiemThuCT(object[] param, string flag)
        {
            SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            try
            {
                string sql = "";
                if (flag == "0")
                    sql = @"INSERT INTO tblNghiemThuCT(sttNghiemThupr_sd,sttHDCTpr_sd,giaTriNghiemThu,thoiGian,taiLieuThamChieu,thayDoiSoVoiThietKe,ketLuan,yeuCauKhac,maCPDauTuXDCTpr_sd)
                        VALUES(@sttNghiemThupr_sd,@sttHDCTpr_sd,@giaTriNghiemThu,@thoiGian,@taiLieuThamChieu,@thayDoiSoVoiThietKe,@ketLuan,@yeuCauKhac,@maCPDauTuXDCTpr_sd)";
                else
                    sql = @"UPDATE tblNghiemThuCT
                         SET sttNghiemThupr_sd = @sttNghiemThupr_sd ,sttHDCTpr_sd = @sttHDCTpr_sd ,giaTriNghiemThu = @giaTriNghiemThu ,thoiGian = @thoiGian 
                         ,taiLieuThamChieu = @taiLieuThamChieu ,thayDoiSoVoiThietKe = @thayDoiSoVoiThietKe ,ketLuan = @ketLuan ,yeuCauKhac = @yeuCauKhac 
                         ,maCPDauTuXDCTpr_sd = @maCPDauTuXDCTpr_sd WHERE sttNghiemThuCTpr=@sttNghiemThuCTpr";
                SqlConnection sqlConn = new SqlConnection(HttpContext.Current.Session.GetConnectionString2());
                SqlCommand cmd = new SqlCommand(sql);
                sqlConn.Open();
                cmd.Connection = sqlConn;
                cmd.Parameters.AddWithValue("@sttNghiemThupr_sd", param[1].ToString());
                cmd.Parameters.AddWithValue("@sttHDCTpr_sd", param[2].ToString().Split('@')[1]);
                cmd.Parameters.AddWithValue("@maCPDauTuXDCTpr_sd ", param[2].ToString().Split('@')[2]);
                cmd.Parameters.AddWithValue("@giaTriNghiemThu", ntsLibraryFunctions.dinhDangSoSQL(param[3].ToString()));
                cmd.Parameters.AddWithValue("@thoiGian", param[4].ToString());
                cmd.Parameters.AddWithValue("@taiLieuThamChieu", param[5].ToString());
                cmd.Parameters.AddWithValue("@thayDoiSoVoiThietKe", param[6].ToString());
                cmd.Parameters.AddWithValue("@ketLuan", param[7].ToString());
                cmd.Parameters.AddWithValue("@yeuCauKhac", param[8].ToString());
                cmd.Parameters.AddWithValue("@sttNghiemThuCTpr", param[0].ToString());
                cmd.ExecuteNonQuery();
                //Cập nhật lại thông tin tin nghiệm thu về bảng hợp đồng, phụ lục hợp đồng
                DataTable tab = sqlFun.GetData("SELECT sttHopDongpr_sd,laHopDong FROM dbo.tblNghiemThu where sttNghiemThupr='" + param[1].ToString() + "'");
                capNhatGiaTriNTHopDong(tab.Rows[0]["laHopDong"].ToString(), tab.Rows[0]["sttHopDongpr_sd"].ToString());
                //capNhatGiaTriVanBan(param[1].ToString());
                return "1";
            }
            catch
            {
                return "0";
            }
        }
        public static void capNhatGiaTriNTHopDong(string loai, string sttHopDongpr_sd)
        {
            SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            DataTable tab = new DataTable();
            if (loai.ToUpper() == "HD" || loai.ToUpper() == "TRUE")
            {
                sqlFun.ExeCuteNonQuery(@"UPDATE tblhopdong SET 
                soBBNghiemThu=(SELECT TOP 1 soBienBan FROM dbo.tblNghiemThu WHERE laHopDong=1 and sttHopDongpr_sd=sttHopDongpr AND ngayLap= (SELECT MAX(ngayLap) FROM dbo.tblNghiemThu WHERE laHopDong=1 and sttHopDongpr_sd=sttHopDongpr))
                ,ngayNghiemThu=(SELECT TOP 1 ngayLap FROM dbo.tblNghiemThu WHERE laHopDong=1 and sttHopDongpr_sd=sttHopDongpr and ngayLap= (SELECT MAX(ngayLap) FROM dbo.tblNghiemThu WHERE laHopDong=1 and sttHopDongpr_sd=sttHopDongpr))
                ,tinhTrangHDpr_sd=(case when isnull(tinhTrangHDpr_sd,'010') not in ('','010','020') then tinhTrangHDpr_sd else '020' end)
                WHERE sttHopDongpr='" + sttHopDongpr_sd + "'");
                tab = sqlFun.GetData("SELECT sttHDCTpr FROM dbo.tblHopDongCT WHERE sttHopDongpr_sd='" + sttHopDongpr_sd + "'");
                foreach (DataRow dr in tab.Rows)
                {
                    sqlFun.ExeCuteNonQuery(@"update tblHopDongCT set giaTriNghiemThu=isnull((SELECT SUM(giaTriNghiemThu) FROM dbo.tblNghiemThuCT a WHERE a.sttHDCTpr_sd=sttHDCTpr and sttNghiemThupr_sd in(SELECT sttNghiemThupr FROM dbo.tblNghiemThu WHERE laHopDong=1 and sttHopDongpr_sd='" + sttHopDongpr_sd + @"')),0) where sttHDCTpr='" + dr["sttHDCTpr"].ToString() + "'");
                }
            }
            else
            {
                sqlFun.ExeCuteNonQuery(@"UPDATE tblPhuLucHD SET 
                soBBNghiemThu=(SELECT TOP 1 soBienBan FROM dbo.tblNghiemThu WHERE laHopDong=0 and sttHopDongpr_sd=sttPLHDpr AND ngayLap= (SELECT MAX(ngayLap) FROM dbo.tblNghiemThu WHERE laHopDong=0 and sttHopDongpr_sd=sttPLHDpr))
                ,ngayNghiemThu=(SELECT TOP 1 ngayLap FROM dbo.tblNghiemThu WHERE laHopDong=0 and sttHopDongpr_sd=sttPLHDpr and ngayLap= (SELECT MAX(ngayLap) FROM dbo.tblNghiemThu WHERE laHopDong=0 and sttHopDongpr_sd=sttPLHDpr))
                ,tinhTrangHDpr_sd=(case when isnull(tinhTrangHDpr_sd,'010') not in ('','010','020') then tinhTrangHDpr_sd else '020' end)
                WHERE sttPLHDpr='" + sttHopDongpr_sd + "'");
                tab = sqlFun.GetData("SELECT sttPLHDCTpr FROM dbo.tblPhuLucHDCT WHERE sttPLHDpr_sd='" + sttHopDongpr_sd + "'");
                foreach (DataRow dr in tab.Rows)
                {
                    sqlFun.ExeCuteNonQuery(@"UPDATE dbo.tblPhuLucHDCT set giaTriNghiemThu=isnull((SELECT SUM(giaTriNghiemThu) FROM dbo.tblNghiemThuCT a WHERE a.sttHDCTpr_sd=sttPLHDCTpr and sttNghiemThupr_sd in(SELECT sttNghiemThupr FROM dbo.tblNghiemThu WHERE laHopDong=0 and sttHopDongpr_sd='" + sttHopDongpr_sd + @"')),0) where sttPLHDCTpr='" + dr["sttPLHDCTpr"].ToString() + "'");
                }
            }
        }
        [AjaxPro.AjaxMethod]
        public string xoaThongTin(string sttNghiemThupr, string flag)
        {
            try
            {
                SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                SqlConnection sqlConn = new SqlConnection(HttpContext.Current.Session.GetConnectionString2());
                SqlCommand cmd = new SqlCommand(); 
                if (flag == "0")
                {
                    DataTable dtVanBan = new DataTable();
                    dtVanBan = sqlFun.GetData(@"SELECT  sttVBDApr, sttDuAnpr_sd, tenFile, sttHopDongpr_sd FROM dbo.tblVanBanDA WHERE 
                            sttVBDApr IN ( SELECT sttVBDApr_sd FROM dbo.tblNghiemThu WHERE sttNghiemThupr='" + sttNghiemThupr.ToString() + @"')");
                    if (dtVanBan.Rows.Count > 0)
                    {
                        if (System.IO.File.Exists(Server.MapPath(dtVanBan.Rows[0]["tenFile"].ToString())))
                        {
                            System.IO.File.Delete(Server.MapPath(dtVanBan.Rows[0]["tenFile"].ToString()));
                        }
                    }
                    sqlFun.ExeCuteNonQuery("DELETE dbo.tblVanBanDA WHERE sttVBDApr IN (SELECT sttVBDApr_sd FROM dbo.tblNghiemThu WHERE sttNghiemThupr='" + sttNghiemThupr.ToString() + @"')");
                    //xóa chi tiet 
                    DataTable tab = sqlFun.GetData("SELECT sttHopDongpr_sd,laHopDong FROM dbo.tblNghiemThu where sttNghiemThupr='" + sttNghiemThupr + "'");

                    cmd = new SqlCommand(@"DELETE dbo.tblNghiemThuCT where sttNghiemThupr_sd=@sttNghiemThupr
                                          ;DELETE dbo.tblNghiemThu where sttNghiemThupr=@sttNghiemThupr");
                    if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();
                    cmd.Connection = sqlConn;
                    cmd.Parameters.AddWithValue("@sttNghiemThupr", sttNghiemThupr);
                    cmd.ExecuteNonQuery();
                    capNhatGiaTriNTHopDong(tab.Rows[0]["laHopDong"].ToString(), tab.Rows[0]["sttHopDongpr_sd"].ToString()); 
                }
                else
                {

                    DataTable tab = sqlFun.GetData("SELECT sttHopDongpr_sd,laHopDong,sttNghiemThupr FROM dbo.tblNghiemThu WHERE sttNghiemThupr=(SELECT sttNghiemThupr_sd FROM dbo.tblNghiemThuCT WHERE sttNghiemThuCTpr='" + sttNghiemThupr + "')");
                   
                    cmd = new SqlCommand("delete dbo.tblNghiemThuCT where sttNghiemThuCTpr=@sttNghiemThuCTpr");
                    if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();
                    cmd.Connection = sqlConn;
                    cmd.Parameters.AddWithValue("@sttNghiemThuCTpr", sttNghiemThupr);
                    cmd.ExecuteNonQuery();
                    capNhatGiaTriNTHopDong(tab.Rows[0]["laHopDong"].ToString(), tab.Rows[0]["sttHopDongpr_sd"].ToString());
                    //capNhatGiaTriVanBan(tab.Rows[0]["sttNghiemThupr"].ToString());
                }
                return "1";
            }
            catch
            {
                return "0"; 
            }
        }
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
        double cot3 = 0;
        double cot4 = 0;
        double cot5 = 0;
        double cot6 = 0;
        public void Grid2_RowDataBound(object sender, GridRowEventArgs e)
        {
            if (e.Row.RowType == GridRowType.DataRow)
            {
                if (e.Row.Cells[2].Text != "")
                {
                    cot2 += double.Parse(e.Row.Cells[2].Text);
                }
                if (e.Row.Cells[3].Text != "")
                {
                    cot3 += double.Parse(e.Row.Cells[3].Text);
                }
                if (e.Row.Cells[4].Text != "")
                {
                    cot4 += double.Parse(e.Row.Cells[4].Text);
                }
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
                e.Row.Cells[2].Text = cot2.ToString("N0");
                e.Row.Cells[2].Style["font-weight"] = "bold";
                e.Row.Cells[3].Text = cot3.ToString("N0");
                e.Row.Cells[3].Style["font-weight"] = "bold";
                e.Row.Cells[4].Text = cot4.ToString("N0");
                e.Row.Cells[4].Style["font-weight"] = "bold";
                e.Row.Cells[5].Text = cot5.ToString("N0");
                e.Row.Cells[5].Style["font-weight"] = "bold";
                e.Row.Cells[6].Text = cot6.ToString("N0");
                e.Row.Cells[6].Style["font-weight"] = "bold";
            } 
        }
        [AjaxPro.AjaxMethod]
        public DataTable layThongTinVanBanNT(string sttNghiemThupr)
        {
            string sql = "SELECT top 1 *,convert(nvarchar(12),ngayLap,103) as ngay,isnull(giaTri,0) as giaTri_,duongDanFile_=isnull(duongDanFile,'') FROM tblNghiemThu WHERE sttNghiemThupr=N'" + sttNghiemThupr + "'";
            SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            return sqlFun.GetData(sql);
        }
        [AjaxPro.AjaxMethod]
        public bool luuThongTinVanBanNT(object[] param)
        {
            string sql = "update tblNghiemThu set giaTri = N'" + (string.IsNullOrEmpty(param[2].ToString()) ? "0" : param[2].ToString().Replace(".", "").Replace(",", ".")) + "', noiDung = N'" + param[1].ToString() + "' WHERE sttNghiemThupr=N'" + param[0].ToString() + "'";
            SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            return sqlFun.ExeCuteNonQuery(sql);
        }
    }
}
