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
    public partial class vanban_buocketthuc : System.Web.UI.UserControl
    {
        string tenForm = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            tenForm = Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath);
            Response.AppendHeader("X-XSS-Protection", "0");
            hdfTenForm.Value = tenForm;
            if (!IsPostBack)
            {
                SqlFunction sqlFunc = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                cboCoQuanBanHanh_uc.DataSource = sqlFunc.GetData("SELECT '' AS maToChucpr,'' AS tenToChuc UNION ALL SELECT maToChucpr,tenToChuc FROM dbo.tblDMToChuc WHERE maDonVipr_sd=N'" + HttpContext.Current.Session.GetDonVi().maDonVi + "' AND isnull(ngungSD,0)=0 ORDER BY tenToChuc");
                cboCoQuanBanHanh_uc.DataValueField = "maToChucpr";
                cboCoQuanBanHanh_uc.DataTextField = "tenToChuc";
                cboCoQuanBanHanh_uc.DataBind();
                cboLoaiVanBan_uc.DataSource = sqlFunc.GetData("SELECT maLoaiVBanpr,tenLoaiVBan FROM dbo.tblDMLoaiVanBan WHERE maLoaiVBanpr_cha = N'VIII' and maGiaiDoanpr_sd =N'010' ORDER BY maLoaiVBanpr");
                cboLoaiVanBan_uc.DataValueField = "maLoaiVBanpr";
                cboLoaiVanBan_uc.DataTextField = "tenLoaiVBan";
                cboLoaiVanBan_uc.DataBind();

                cboThuocNhomToChuc_uc.DataSource = sqlFunc.GetData("SELECT '' as maNhomToChucpr,'' as tenNhomToChuc UNION ALL SELECT maNhomToChucpr,tenNhomToChuc FROM dbo.tblDMNhomToChuc WHERE ngungSD='0'");
                cboThuocNhomToChuc_uc.DataTextField = "tenNhomToChuc";
                cboThuocNhomToChuc_uc.DataValueField = "maNhomToChucpr";
                cboThuocNhomToChuc_uc.DataBind();
            }
            AsyncFileUpload_uc.UploadedComplete += new EventHandler<AsyncFileUploadEventArgs>(AsyncFileUpload_uc_UploadedComplete);
            AsyncFileUpload_uc.UploadedFileError += new EventHandler<AsyncFileUploadEventArgs>(AsyncFileUpload_uc_UploadedFileError);
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ntsLibraryFunctions));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ProOnline.module.vanban_buocketthuc), this.Page);
        }
        void AsyncFileUpload_uc_UploadedComplete(object sender, AsyncFileUploadEventArgs e)
        {
            try
            {
                string path = "";
                string urlFile = "";
                SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                string duongdanfilecu = sqlFun.GetOneStringField(@"select top 1 tenFile from  dbo.tblVanBanDA WHERE sttVBDApr='" + sttVBDApr.Value.ToString() + "'");
                //lấy mã dự án
                string sttDuAnpr = sqlFun.GetOneStringField("select top 1 CONVERT(NVARCHAR(50),sttDuAnpr_sd) from tblVanBanDA where sttVBDApr='" + sttVBDApr.Value.ToString() + "'");
                string tenFile = e.filename;//"VanBan" + hdfSTTVanBan.Value + ".pdf";
                if (!System.IO.Directory.Exists(Server.MapPath("~/VanBan/" + HttpContext.Current.Session.GetDonVi().maDonVi + "/" + sttDuAnpr + "/" + tenForm + "")))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/VanBan/" + HttpContext.Current.Session.GetDonVi().maDonVi + "/" + sttDuAnpr + "/" + tenForm + ""));
                }
                string strDate = DateTime.Now.ToString("dd_MM_yy_hhmmss");
                string fileExtension = Path.GetExtension(tenFile).Replace("-", "");
                tenFile = tenFile.Substring(tenFile.LastIndexOf("\\\\") + 1);
                tenFile = tenFile.Substring(0, tenFile.LastIndexOf(fileExtension)) + strDate + fileExtension;
                tenFile = tenFile.Replace(" ", "");
                //path = string.Concat(Server.MapPath("/VanBan/" + HttpContext.Current.Session.GetDonVi().maDonVi + "/KHoachDThau/"));
                path = string.Concat(Server.MapPath("/VanBan/" + HttpContext.Current.Session.GetDonVi().maDonVi + "/" + sttDuAnpr + "/" + tenForm + "/" + tenFile + ""));

                if (!System.IO.File.Exists(path))
                {
                    //save lại file mới
                    AsyncFileUpload_uc.SaveAs(path);
                    urlFile = "/VanBan/" + HttpContext.Current.Session.GetDonVi().maDonVi + "/" + sttDuAnpr + "/" + tenForm + "/" + tenFile + "";
                    sqlFun.ExeCuteNonQuery("UPDATE dbo.tblVanBanDA SET tenFile=N'" + urlFile + "' WHERE sttVBDApr='" + sttVBDApr.Value.ToString() + "'");
                    //lấy mã dự án
                    //xóa file cũ
                    string duongdanfilecu_ = HttpContext.Current.Server.MapPath(duongdanfilecu.ToString());
                    if (System.IO.File.Exists(duongdanfilecu_))
                    {
                        System.IO.File.Delete(duongdanfilecu_);
                    }
                    AsyncFileUpload_uc.ClearState();
                    AsyncFileUpload_uc.Dispose();
                    return;
                }
                else
                {
                    //Xoa file neu da ton tai
                    System.IO.File.Delete(path);
                    AsyncFileUpload_uc_UploadedComplete(sender, e);
                }
            }
            catch
            {
                return;
            }
            AsyncFileUpload_uc.Dispose();
        }

        void AsyncFileUpload_uc_UploadedFileError(object sender, AsyncFileUploadEventArgs e)
        {
        }
        [AjaxPro.AjaxMethod]
        public decimal layGiaTriDuAn(string sqlQuery)
        {
            try
            {
                SqlFunction _sqlfun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                return _sqlfun.GetOneDecimalField(sqlQuery);
            }
            catch
            {
                return 0;
            }
        }
        //Xử lý cá nhân
        [AjaxPro.AjaxMethod]
        public string xoaVanBanCu(string sttVBDApr)
        {
            SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            try
            {
                string duongdanfilecu = sqlFun.GetOneStringField(@"select top 1 tenFile from  dbo.tblVanBanDA WHERE sttVBDApr='" + sttVBDApr + "'");
                //lấy mã dự án
                //xóa file cũ
                string duongdanfilecu_ = HttpContext.Current.Server.MapPath(duongdanfilecu.ToString());
                if (System.IO.File.Exists(duongdanfilecu_))
                {
                    System.IO.File.Delete(duongdanfilecu_);
                }
                return "thanhCong";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        //Xử lý cá nhân
        [AjaxPro.AjaxMethod]
        public string chucVuTheoCaNhan(string maToChuc, string tenCaNhan)
        {
            SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            return sqlFun.GetOneStringField(@"SELECT chucVu FROM dbo.tblDMCaNhan WHERE maToChucpr_sd=N'" + maToChuc + "' AND tenCaNhan=N'" + tenCaNhan + "'");
        }
        [AjaxPro.AjaxMethod]
        public DataTable danhSachDoiTuongTheoToChuc(string maToChuc)
        {
            SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            return sqlFun.GetData(@"SELECT chucVu=ISNULL(chucVu,''),tenCaNhan FROM dbo.tblDMCaNhan WHERE maToChucpr_sd=N'" + maToChuc + "'");
        }

        [AjaxPro.AjaxMethod]
        public DataTable loadLoaiVanBan(object maLoaiVBanpr_cha, object maGiaiDoan)
        {
            try
            {
                SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                string sql = "SELECT maLoaiVBanpr,tenLoaiVBan FROM dbo.tblDMLoaiVanBan WHERE maLoaiVBanpr_cha = N'" + maLoaiVBanpr_cha.ToString() + "' and maGiaiDoanpr_sd =N'" + maGiaiDoan.ToString() + "' ORDER BY maLoaiVBanpr";
                return sqlFun.GetData(sql);
            }
            catch
            {
                return null;
            }
        }

        //xuly them nhanh to chuc
        [AjaxPro.AjaxMethod]//Lấy số tự tăng số hồ sơ
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
        public bool kiemTraLuuToChuc(string ma)
        {
            try
            {
                SqlFunction sqlFunc = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                return sqlFunc.CheckHasRecord("select maToChucpr from tblDMToChuc where maToChucpr=N'" + ma + "'");
            }
            catch
            {
            }
            return false;
        }

        [AjaxPro.AjaxMethod]
        public bool themToChuc(AjaxPro.JavaScriptArray param)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(HttpContext.Current.Session.GetConnectionString2());
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO tblDMToChuc(maToChucpr,tenToChuc,ngungSD,maNhomToChucpr_sd,diaChi,maSoThue,dienThoai,Fax,Email,nguoiDaiDien,chucVuDD,nguoiLienHe,chucVuLH,dienThoaiLH,maDonVipr_sd,nguoiThaoTac,ngayThaotac)"
                    + " VALUES(@maToChucpr,@tenToChuc,@ngungSD,@maNhomToChucpr_sd,@diaChi,@maSoThue,@dienThoai,@Fax,@Email,@nguoiDaiDien,@chucVuDD,@nguoiLienHe,@chucVuLH,@dienThoaiLH,@maDonVipr_sd,@nguoiThaoTac,@ngayThaotac)", sqlCon);
                cmd.Parameters.Add(new SqlParameter("@maToChucpr", param[0].ToString().Trim()));
                cmd.Parameters.Add(new SqlParameter("@tenToChuc", param[1].ToString()));
                cmd.Parameters.Add(new SqlParameter("@ngungSD", param[2].ToString()));
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
                cmd.Parameters.Add(new SqlParameter("@maDonVipr_sd", HttpContext.Current.Session.GetDonVi().maDonVi));
                cmd.Parameters.Add(new SqlParameter("@nguoiThaoTac", HttpContext.Current.Session.GetCurrentUserID()));//hinhThucNhap
                cmd.Parameters.Add(new SqlParameter("@ngayThaotac", HttpContext.Current.Session.GetCurrentDatetimeMMddyyyy("MM/dd/yyyy")));
                cmd.ExecuteNonQuery();
                sqlCon.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        [AjaxPro.AjaxMethod]
        public DataTable loadCoQuanBanHanh()
        {
            try
            {
                SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                string sql = "SELECT '' AS maToChucpr,'' AS tenToChuc UNION ALL SELECT maToChucpr,tenToChuc FROM dbo.tblDMToChuc WHERE maDonVipr_sd=N'" + HttpContext.Current.Session.GetDonVi().maDonVi + "' AND isnull(ngungSD,0)=0 ORDER BY tenToChuc";
                return sqlFun.GetData(sql);
            }
            catch
            {
                return null;
            }
        }
        [AjaxPro.AjaxMethod]
        public string suaVanBan(AjaxPro.JavaScriptArray param)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(HttpContext.Current.Session.GetConnectionString2());
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand(@"update dbo.tblVanBanDA
                set soVanBan =@soVanBan,ngayKy=@ngayKy,maToChucpr_phathanh=@maToChucpr_phathanh,noiDung=@noiDung,giaTri=@giaTri,maLoaiVBanpr_sd=@maLoaiVBanpr_sd,chucDanhNguoiKy=@chucDanhNguoiKy,tenNguoiKy=@tenNguoiKy
                WHERE sttVBDApr=@sttVBDApr", sqlCon);
                cmd.Parameters.Add(new SqlParameter("@soVanBan", param[1].ToString()));
                if ((String.IsNullOrEmpty(param[2].ToString())) || (param[2].ToString() == ""))
                {
                    cmd.Parameters.Add(new SqlParameter("@ngayKy", DBNull.Value));
                }
                else
                {
                    cmd.Parameters.Add(new SqlParameter("@ngayKy", _mChuyenChuoiSangNgay(param[2].ToString())));
                }
                cmd.Parameters.Add(new SqlParameter("@maToChucpr_phathanh", param[3].ToString()));
                cmd.Parameters.Add(new SqlParameter("@noiDung", param[7].ToString()));
                cmd.Parameters.Add(new SqlParameter("@giaTri", (string.IsNullOrEmpty(param[8].ToString()) ? "0" : param[8].ToString().Replace(".", "").Replace(",", "."))));
                cmd.Parameters.Add(new SqlParameter("@maLoaiVBanpr_sd", param[6].ToString()));
                cmd.Parameters.Add(new SqlParameter("@chucDanhNguoiKy", param[5].ToString()));
                cmd.Parameters.Add(new SqlParameter("@tenNguoiKy", param[4].ToString()));
                cmd.Parameters.Add(new SqlParameter("@sttVBDApr", param[0].ToString()));
                if (cmd.ExecuteNonQuery() > 0)
                {
                    sqlCon.Close();
                    return "thanhCong";
                }
                else
                {
                    sqlCon.Close();
                    return "Cập nhật văn bản lỗi!";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        private string _mChuyenChuoiSangNgay(string ddMMyyyy)
        {
            return ddMMyyyy.Substring(3, 2) + "/" + ddMMyyyy.Substring(0, 2) + "/" + ddMMyyyy.Substring(6, 4);
        }
        [AjaxPro.AjaxMethod]
        public DataTable layThongTinVanBan(string sttVBDApr)
        {
            SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            string sql = @"select top 1 *,convert(nvarchar(12),ngayKy,103) as ngay,isnull(giaTri,0) as giaTri_,tenFile_=isnull(tenFile,'') from tblVanBanDA where sttVBDApr=N'" + sttVBDApr + "'";
            return sqlFun.GetData(sql);
        }
    }
}