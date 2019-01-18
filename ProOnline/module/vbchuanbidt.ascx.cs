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
    public partial class vbchuanbidt : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.AppendHeader("X-XSS-Protection", "0");
            string _pageName = Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath);
            userControlVanBanChucNang.Value = _pageName + "_pr_0";
            hdfMaDonVi.Value = Session.GetDonVi().maDonVi;
            String _Query = "";
            switch (_pageName)
            {
                case "bcnctienkhathi":
                    lblTieuDe.Text = "BÁO CÁO NGHIÊN CỨU TIỀN KHẢ THI";
                    hdf_page_name.Value = _pageName;
                    _Query = " WHERE maLoaiVBanpr_cha = N'I' AND isnull(ngungSD,0) = 0 ORDER BY maLoaiVBanpr";
                    break;

                case "thamdinhbcnctienkhathi":
                    lblTieuDe.Text = "THẨM ĐỊNH BÁO CÁO NGHIÊN CỨU TIỀN KHẢ THI";
                    hdf_page_name.Value = _pageName;
                    _Query = " WHERE maLoaiVBanpr_cha = N'II' AND isnull(ngungSD,0) = 0 ORDER BY maLoaiVBanpr";
                    break;

                case "quyetdinhpheduyetchutruongdautu":
                    lblTieuDe.Text = "QUYẾT ĐỊNH PHÊ DUYỆT CHỦ TRƯƠNG ĐẦU TƯ";
                    hdf_page_name.Value = _pageName;
                    _Query = " WHERE maLoaiVBanpr_cha = N'III' AND isnull(ngungSD,0) = 0 ORDER BY maLoaiVBanpr";
                    break;

                case "lapbaocaonghiencuukhathi":
                    lblTieuDe.Text = "BÁO CÁO NGHIÊN CỨU KHẢ THI";
                    hdf_page_name.Value = _pageName;
                    _Query = " WHERE maLoaiVBanpr_cha = N'IV' AND isnull(ngungSD,0) = 0 ORDER BY maLoaiVBanpr";
                    break;

                case "thamdinhnghiencuubaocaokhathi":
                    lblTieuDe.Text = "THẨM ĐỊNH BÁO CÁO NGHIÊN CỨU KHẢ THI";
                    hdf_page_name.Value = _pageName;
                    _Query = " WHERE maLoaiVBanpr_cha = N'V' AND isnull(ngungSD,0) = 0 ORDER BY maLoaiVBanpr";
                    break;

                case "quyetdinhpheduyetduan":
                    lblTieuDe.Text = "QUYẾT ĐỊNH PHÊ DUYỆT DỰ ÁN";
                    hdf_page_name.Value = _pageName;
                    _Query = " WHERE maLoaiVBanpr_cha = N'VIII' AND isnull(ngungSD,0) = 0 ORDER BY maLoaiVBanpr";
                    break;

                case "giayphepxaydung":
                    lblTieuDe.Text = "GIẤY PHÉP XÂY DỰNG";
                    hdf_page_name.Value = _pageName;
                    gridVanBan.Columns["soVanBan"].HeaderText = "Số giấy phép xây dựng";
                    _Query = " WHERE maLoaiVBanpr_cha = N'XII' AND isnull(ngungSD,0) = 0 ORDER BY maLoaiVBanpr";
                    break;

                case "qdpdkehoachdthaucbdt":
                    lblTieuDe.Text = "QUYẾT ĐỊNH PHÊ DUYỆT KẾ HOẠCH LỰA CHỌN NHÀ THẦU GIAI ĐOẠN CHUẨN BỊ ĐẦU TƯ";
                    hdf_page_name.Value = _pageName;
                    gridVanBan.Columns["soVanBan"].HeaderText = "Số quyết định";
                    _Query = " WHERE maLoaiVBanpr_cha = N'XIII' AND isnull(ngungSD,0) = 0 ORDER BY maLoaiVBanpr";
                    break;

                case "qdpddckehoachdthaucbdt":
                    lblTieuDe.Text = "QUYẾT ĐỊNH PHÊ DUYỆT ĐIỀU CHỈNH KẾ HOẠCH LỰA CHỌN NHÀ THẦU GIAI ĐOẠN CHUẨN BỊ ĐẦU TƯ";
                    hdf_page_name.Value = _pageName;
                    gridVanBan.Columns["soVanBan"].HeaderText = "Số quyết định";
                    _Query = " WHERE maLoaiVBanpr_cha = N'XIV' AND isnull(ngungSD,0) = 0 ORDER BY maLoaiVBanpr";
                    break;

                case "qdpdkehoachdthauthdt":
                    lblTieuDe.Text = "QUYẾT ĐỊNH PHÊ DUYỆT KẾ HOẠCH LỰA CHỌN NHÀ THẦU GIAI ĐOẠN THỰC HIỆN ĐẦU TƯ";
                    hdf_page_name.Value = _pageName;
                    gridVanBan.Columns["soVanBan"].HeaderText = "Số quyết định";
                    _Query = " WHERE maLoaiVBanpr_cha = N'XVIII' AND isnull(ngungSD,0) = 0 ORDER BY maLoaiVBanpr";
                    break;
                    
                case "qdpddckehoachdthauthdt":
                    lblTieuDe.Text = "QUYẾT ĐỊNH PHÊ DUYỆT ĐIỀU CHỈNH KẾ HOẠCH LỰA CHỌN NHÀ THẦU GIAI ĐOẠN THỰC HIỆN ĐẦU TƯ";
                    hdf_page_name.Value = _pageName;
                    gridVanBan.Columns["soVanBan"].HeaderText = "Số quyết định";
                    _Query = " WHERE maLoaiVBanpr_cha = N'XIX' AND isnull(ngungSD,0) = 0 ORDER BY maLoaiVBanpr";
                    break;

                case "bckinhtekythuat":
                    lblTieuDe.Text = "BÁO CÁO KINH TẾ KỸ THUẬT";
                    hdf_page_name.Value = _pageName;
                    gridVanBan.Columns["soVanBan"].HeaderText = "Số văn bản";
                    _Query = " WHERE maLoaiVBanpr_cha = N'XXXV' AND isnull(ngungSD,0) = 0 ORDER BY maLoaiVBanpr";
                    break;

                default:
                    break;
            }

            if (!IsPostBack)
            {
                gridVanBan.DataSource = null;
                gridVanBan.DataBind();
            }
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ntsLibraryFunctions));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ProOnline.module.vbchuanbidt), this.Page);
            AsyncFileUpload1.UploadedComplete += new EventHandler<AsyncFileUploadEventArgs>(AsyncFileUpload1_UploadedComplete);
            AsyncFileUpload1.UploadedFileError += new EventHandler<AsyncFileUploadEventArgs>(AsyncFileUpload1_UploadedFileError);

            sdsCoQuanBanHanh.ConnectionString = HttpContext.Current.Session.GetConnectionString2();
            sdsCoQuanBanHanh.SelectCommand = "SELECT '' as maToChucpr,'' as tenToChuc UNION ALL SELECT maToChucpr,tenToChuc FROM dbo.tblDMToChuc WHERE maDonVipr_sd=N'" + HttpContext.Current.Session.GetDonVi().maDonVi + "' AND isnull(ngungSD,0)=0 ORDER BY maToChucpr";
            sdsCoQuanBanHanh.DataBind();

            sdsLoaiVanBan.ConnectionString = HttpContext.Current.Session.GetConnectionString2();
            sdsLoaiVanBan.SelectCommand = @"SELECT '' as maLoaiVBanpr,'' as tenLoaiVBan UNION ALL 
                                            SELECT maLoaiVBanpr,tenLoaiVBan FROM dbo.tblDMLoaiVanBan" + _Query;
            sdsLoaiVanBan.DataBind();
        }

        [AjaxPro.AjaxMethod]
        public DataTable layDuAnTheoDonVi(string maDonVi)
        {
            try
            {
                String _Query = "SELECT sttDuAnpr, tenDuAn FROM dbo.tblDuAn WHERE maDonVipr_sd = N'" + maDonVi + "'";
                SqlFunction _sqlfun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                return _sqlfun.GetData(_Query);
            }
            catch
            {
                return null;
            }
        }

        [AjaxPro.AjaxMethod]
        public string luuThongTinVanBan(object[] param)
        {
            try
            {
                SqlFunction _sqlfun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                string sql = "";
                if (param[1].ToString() == "" || param[1].ToString() == "0")
                    sql = @"INSERT INTO dbo.tblVanBanDA (sttDuAnpr_sd,soVanBan,ngayKy,ngayPhatHanh,maToChucpr_phathanh,noiDung,giaTri,maLoaiVBanpr_sd,maDonVipr_sd,nguoiThaoTac,ngayThaotac ,kyHieu ,chucDanhNguoiKy,tenNguoiKy,maGiaiDoanpr_sd)
                       SELECT @sttDuAnpr_sd,@soVanBan,@ngayKy,@ngayPhatHanh,@maToChucpr_phathanh,@noiDung,@giaTri,@maLoaiVBanpr_sd,@maDonVipr_sd,@nguoiThaoTac,GETDATE() ,@kyHieu ,@chucDanhNguoiKy,@tenNguoiKy,@maGiaiDoanpr_sd";
                else
                    sql = @"UPDATE dbo.tblVanBanDA set sttDuAnpr_sd=@sttDuAnpr_sd,soVanBan=@soVanBan,ngayKy=@ngayKy,ngayPhatHanh=@ngayPhatHanh,maToChucpr_phathanh=@maToChucpr_phathanh
                                                   ,noiDung=@noiDung,giaTri=@giaTri,maLoaiVBanpr_sd=@maLoaiVBanpr_sd,kyHieu=@kyHieu ,chucDanhNguoiKy=@chucDanhNguoiKy,tenNguoiKy=@tenNguoiKy,maGiaiDoanpr_sd=@maGiaiDoanpr_sd
                        where sttVBDApr = @sttVBDApr";
                SqlConnection sqlConn = new SqlConnection(HttpContext.Current.Session.GetConnectionString2());
                sqlConn.Open();
                SqlCommand cmd = new SqlCommand(sql, sqlConn);
                // cmd.Parameters.AddWithValue("@sttDuAnpr_sd", param[2].ToString() == "" ? "0" : param[2].ToString());
                cmd.Parameters.AddWithValue("@soVanBan", param[3].ToString());
                cmd.Parameters.AddWithValue("@ngayKy", ntsLibraryFunctions._mChuyenChuoiSangNgay(param[4].ToString()));
                cmd.Parameters.AddWithValue("@ngayPhatHanh", DBNull.Value);
                cmd.Parameters.AddWithValue("@maToChucpr_phathanh", param[6].ToString());
                cmd.Parameters.AddWithValue("@noiDung", param[7].ToString());
                cmd.Parameters.AddWithValue("@giaTri", (param[8].ToString() == "" ? "0" : param[8].ToString().Replace(".", "").Replace(",", "")));
                cmd.Parameters.AddWithValue("@maLoaiVBanpr_sd", param[9].ToString());
                cmd.Parameters.AddWithValue("@kyHieu", param[10].ToString());
                cmd.Parameters.AddWithValue("@chucDanhNguoiKy", param[11].ToString());
                cmd.Parameters.AddWithValue("@tenNguoiKy", param[12].ToString());
                cmd.Parameters.AddWithValue("@sttDuAnpr_sd", param[13].ToString());
                if (param[1].ToString() == "" || param[1].ToString() == "0")
                {
                    //cmd.Parameters.AddWithValue("@" + param[0].ToString().Split('_')[1].ToString() + "_sd", param[0].ToString().Split('_')[2].ToString());
                    cmd.Parameters.AddWithValue("@maDonVipr_sd", HttpContext.Current.Session.GetDonVi().maDonVi);
                    cmd.Parameters.AddWithValue("@nguoiThaoTac", HttpContext.Current.Session.GetCurrentUserID());
                }
                else cmd.Parameters.AddWithValue("@sttVBDApr", param[1].ToString());
                cmd.Parameters.AddWithValue("@maGiaiDoanpr_sd", _sqlfun.GetOneStringField("SELECT top 1 maGiaiDoanpr_sd FROM dbo.tblDMLoaiVanBan WHERE maLoaiVBanpr =N'" + param[9].ToString() + "'"));
                cmd.ExecuteNonQuery();
                return "1";
            }
            catch
            {
                return "0";
            }
        }
        protected void gridVanBan_OnRebind(object sender, EventArgs e)
        {
            try
            {
                string _pageName = Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath);
               
                String _Query = "";
                switch (_pageName)
                {
                    case "bcnctienkhathi":
                        _Query = " AND maLoaiVBanpr_sd IN (SELECT maLoaiVBanpr FROM dbo.tblDMLoaiVanBan WHERE maLoaiVBanpr_cha = N'I')";
                        break;

                    case "thamdinhbcnctienkhathi":

                        _Query = " AND maLoaiVBanpr_sd IN (SELECT maLoaiVBanpr FROM dbo.tblDMLoaiVanBan WHERE maLoaiVBanpr_cha = N'II')";
                        break;

                    case "quyetdinhpheduyetchutruongdautu":

                        _Query = " AND maLoaiVBanpr_sd IN (SELECT maLoaiVBanpr FROM dbo.tblDMLoaiVanBan WHERE maLoaiVBanpr_cha = N'III')";
                        break;

                    case "lapbaocaonghiencuukhathi":

                        _Query = " AND maLoaiVBanpr_sd IN (SELECT maLoaiVBanpr FROM dbo.tblDMLoaiVanBan WHERE maLoaiVBanpr_cha = N'IV')";
                        break;

                    case "thamdinhnghiencuubaocaokhathi":

                        _Query = " AND maLoaiVBanpr_sd IN (SELECT maLoaiVBanpr FROM dbo.tblDMLoaiVanBan WHERE maLoaiVBanpr_cha = N'V')";
                        break;

                    case "quyetdinhpheduyetduan":

                        _Query = " AND maLoaiVBanpr_sd IN (SELECT maLoaiVBanpr FROM dbo.tblDMLoaiVanBan WHERE maLoaiVBanpr_cha = N'VIII')";
                        break;

                    case "giayphepxaydung":

                        _Query = " AND maLoaiVBanpr_sd IN (SELECT maLoaiVBanpr FROM dbo.tblDMLoaiVanBan WHERE maLoaiVBanpr_cha = N'XII')";
                        break;

                    case "qdpdkehoachdthaucbdt":

                        _Query = " AND maLoaiVBanpr_sd IN (SELECT maLoaiVBanpr FROM dbo.tblDMLoaiVanBan WHERE maLoaiVBanpr_cha = N'XIII')";
                        break;

                    case "qdpddckehoachdthaucbdt":

                        _Query = " AND maLoaiVBanpr_sd IN (SELECT maLoaiVBanpr FROM dbo.tblDMLoaiVanBan WHERE maLoaiVBanpr_cha = N'XIV')";
                        break;

                    case "qdpdkehoachdthauthdt":

                        _Query = " AND maLoaiVBanpr_sd IN (SELECT maLoaiVBanpr FROM dbo.tblDMLoaiVanBan WHERE maLoaiVBanpr_cha = N'XVIII')";
                        break;

                    case "qdpddckehoachdthauthdt":

                        _Query = " AND maLoaiVBanpr_sd IN (SELECT maLoaiVBanpr FROM dbo.tblDMLoaiVanBan WHERE maLoaiVBanpr_cha = N'XIX')";
                        break;

                    case "bckinhtekythuat":

                        _Query = " AND maLoaiVBanpr_sd IN (SELECT maLoaiVBanpr FROM dbo.tblDMLoaiVanBan WHERE maLoaiVBanpr_cha = N'XXXV')";
                        break;

                    default:
                        break;
                }

                SqlFunction _sqlfun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                String qr = "SELECT maNguoiKy=tenNguoiKy, *,(SELECT tblDMLoaiVanBan.tenLoaiVBan FROM tblDMLoaiVanBan WHERE tblDMLoaiVanBan.maLoaiVBanpr=maLoaiVBanpr_sd) loaiVB,(SELECT tblDMToChuc.tenToChuc FROM tblDMToChuc WHERE tblDMToChuc.maToChucpr=tblVanBanDA.maToChucpr_phathanh) coQuanBanHanh,CASE WHEN LEN(isnull(tenFile,'')) > 0 THEN (N'<a style=\"color:Blue;text-decoration:none\" onclick=\"xemVB('+CHAR(39)+tenFile+CHAR(39)+')\" href=\"#\">'+ISNULL(replace(ltrim(right(tenFile,CHARINDEX('/',REVERSE(tenFile),0))),'/',''),'')+'</a>' + N' <a style=\"color:Blue;\" onclick=\"xoaDinhKemVanBan('+CHAR(39)+tenFile+CHAR(39)+','+CHAR(39)+CONVERT(NVARCHAR(50),sttVBDApr)+CHAR(39)+')\" href=\"#\">Xóa</a>') ELSE N'<a style=\"color:Blue;\" onclick=dinKemFileVanBan(\"'+CONVERT(NVARCHAR(50),sttVBDApr)+N'\") href=\"#\">Đính kèm file</a>' end as dinhKem, (SELECT tenDuAn FROM dbo.tblDuAn WHERE sttDuAnpr = tblVanBanDA.sttDuAnpr_sd) AS tenDuAn, sttDuAnpr_sd  FROM tblVanBanDA WHERE maDonVipr_sd=N'" + HttpContext.Current.Session.GetDonVi().maDonVi + "'" + _Query + " order by sttVBDApr";
                gridVanBan.DataSource = _sqlfun.GetData(qr);
                gridVanBan.DataBind();
            }
            catch
            {
                gridVanBan.DataSource = null;
                gridVanBan.DataBind();
            }
        }
        [AjaxPro.AjaxMethod]
        public bool XoaVanBan(object stt)
        {
            SqlFunction _sqlfun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            string duongdan = _sqlfun.GetOneStringField("SELECT tenFile FROM tblVanBanDA where sttVBDApr = N'" + stt.ToString() + "'");
            if (duongdan != "")
            {
                string path = HttpContext.Current.Server.MapPath(duongdan.ToString());
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            return _sqlfun.ExeCuteNonQuery("DELETE FROM tblVanBanDA WHERE sttVBDApr='" + stt.ToString() + "' and maDonVipr_sd = N'" + HttpContext.Current.Session.GetDonVi().maDonVi + "' ");
        }
        [AjaxPro.AjaxMethod]
        public void xoaDinhKemFile(object duongdan, object sttVB)
        {
            try
            { 
                SqlFunction _sqlfun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                _sqlfun.ExeCuteNonQuery("UPDATE dbo.tblVanBanDA SET tenFile = NULL WHERE sttVBDApr='" + sttVB.ToString() + "' and maDonVipr_sd = N'" + HttpContext.Current.Session.GetDonVi().maDonVi + "'");
                string path = HttpContext.Current.Server.MapPath(  duongdan.ToString());
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [AjaxPro.AjaxMethod]
        public DataTable danhMucToChuc()
        {
            SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            return sqlFun.GetData(@"SELECT '' maToChucpr,'' tenToChuc union all SELECT maToChucpr,tenToChuc FROM dbo.tblDMToChuc WHERE ngungSD=0 and maDonVipr_sd='" + HttpContext.Current.Session.GetDonVi().maDonVi + "'");
        }
        [AjaxPro.AjaxMethod]
        public DataTable danhSachDoiTuongTheoToChuc(string maToChuc)
        {
            SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            return sqlFun.GetData(@"SELECT '' chucVu,'' tenCaNhan union all SELECT chucVu=ISNULL(chucVu,''),tenCaNhan FROM dbo.tblDMCaNhan WHERE maToChucpr_sd=N'" + maToChuc + "'");
        }
        [AjaxPro.AjaxMethod]
        public string chucVuTheoCaNhan(string maToChuc, string tenCaNhan)
        {
            SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            return sqlFun.GetOneStringField(@"SELECT chucVu FROM dbo.tblDMCaNhan WHERE maToChucpr_sd=N'" + maToChuc + "' AND tenCaNhan=N'" + tenCaNhan + "'");
        }
        #region "Upload file quyết định"
        void AsyncFileUpload1_UploadedComplete(object sender, AsyncFileUploadEventArgs e)
        {
            try
            {
                string path = "";
                string urlFile = "";
                if (Path.GetExtension(e.filename).Contains(".pdf") == false && Path.GetExtension(e.filename).Contains(".zip") && Path.GetExtension(e.filename).Contains(".rar"))
                {
                    return;
                }
                SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                //lấy mã dự án
                //string maDuAn = sqlFun.GetOneStringField("select maDuAn from tblDuAn where sttDuAnpr = N'" + hdfSttDA.Value + "'");
                string tenFile = e.filename;//"VanBan" + hdfSTTVanBan.Value + ".pdf";
                if (!System.IO.Directory.Exists(Server.MapPath("~/VanBan/" + HttpContext.Current.Session.GetDonVi().maDonVi + "/" + userControlVanBanChucNang.Value.ToString().Split('_')[0].ToString())))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/VanBan/" + HttpContext.Current.Session.GetDonVi().maDonVi + "/" + userControlVanBanChucNang.Value.ToString().Split('_')[0].ToString()));
                }
                string strDate = DateTime.Now.ToString("dd_MM_yy_hhmmss");
                string fileExtension = Path.GetExtension(tenFile).Replace(".", "");
                tenFile = tenFile.Substring(tenFile.LastIndexOf("\\\\") + 1);
                tenFile = tenFile.Substring(0, tenFile.LastIndexOf(fileExtension)) + strDate + "." + fileExtension;
                tenFile = tenFile.Replace(" ", "");
                path = string.Concat(Server.MapPath("/VanBan/" + HttpContext.Current.Session.GetDonVi().maDonVi + "/" + userControlVanBanChucNang.Value.ToString().Split('_')[0].ToString() + "/" + tenFile + ""));
                 
                if (!System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                    AsyncFileUpload1.SaveAs(path);
                    urlFile = "/VanBan/" + HttpContext.Current.Session.GetDonVi().maDonVi + "/" + userControlVanBanChucNang.Value.ToString().Split('_')[0].ToString() + "/" + tenFile + "";
                    sqlFun.ExeCuteNonQuery("UPDATE dbo.tblVanBanDA SET tenFile=N'" + urlFile + "' WHERE sttVBDApr='" + userControlVanBanpr.Value.ToString() + "'");
                    AsyncFileUpload1.ClearState();
                    AsyncFileUpload1.Dispose();
                    return;
                }
                else
                {
                    //Xoa file neu da ton tai
                    System.IO.File.Delete(path);
                    AsyncFileUpload1_UploadedComplete(sender, e);
                }
            }
            catch
            {
            }
            AsyncFileUpload1.Dispose();
        }
        void AsyncFileUpload1_UploadedFileError(object sender, AsyncFileUploadEventArgs e)
        {
        }
        
        #endregion
        [AjaxPro.AjaxMethod]
        public string getSTTDuAn()
        {
            try
            {
                return HttpContext.Current.Session["ntsSTTDuAn"].ToString();
            }
            catch (Exception)
            {
                return  "0";
            }

        }
    }
}