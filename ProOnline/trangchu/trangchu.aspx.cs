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
using System.Data.SqlClient;
using Obout.Grid;
using System.Globalization;
using System.IO;
using ClosedXML.Excel;
namespace ProOnline.trangchu
{
    public partial class trangchu : System.Web.UI.Page
    {
        

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ProOnline.trangchu.trangchu));
            if (!IsPostBack)
            {
                Grid1.DataSource = null;
                Grid1.DataBind();
                Grid2.DataSource = null;
                Grid2.DataBind();
                Grid3.DataSource = null;
                Grid3.DataBind();
                SqlFunction sqlFunc = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                cboLoaiHinh.DataSource = sqlFunc.GetData("SELECT '' AS maLoaiHinhpr,'' AS tenLoaiHinh UNION ALL SELECT maLoaiHinhpr,tenLoaiHinh FROM dbo.tblDMLoaiHinh WHERE ngungTD='0' OR ngungTD IS NULL");
                cboLoaiHinh.DataTextField = "tenLoaiHinh";
                cboLoaiHinh.DataValueField = "maLoaiHinhpr";
                cboLoaiHinh.DataBind();
                cboLoaiDuAn.DataSource = sqlFunc.GetData("SELECT '' AS maLoaiDApr,'' AS tenLoaiDA UNION ALL SELECT maLoaiDApr,tenLoaiDA FROM dbo.tblDMLoaiDA WHERE ngungSD='0' OR ngungSD IS NULL");
                cboLoaiDuAn.DataTextField = "tenLoaiDA";
                cboLoaiDuAn.DataValueField = "maLoaiDApr";
                cboLoaiDuAn.DataBind();
                cboNguonVonTimKiem.DataSource = sqlFunc.GetData("SELECT '' as maNguonVonpr,'' as tenNguonVon UNION ALL SELECT maNguonVonpr,tenNguonVon FROM dbo.tblDMNguonVon WHERE ngungSD='0' OR ngungSD IS NULL");
                cboNguonVonTimKiem.DataTextField = "tenNguonVon";
                cboNguonVonTimKiem.DataValueField = "maNguonVonpr";
                cboNguonVonTimKiem.DataBind();
                //lbKQTimKiem.Text = "<a href=\"#\" class=\"btn\" data-popup-open=\"popup-1\">Xây dựng trụ sở làm việc UBND tỉnh Vĩnh Long</a></br></br>";
                if (sqlFunc.CheckHasRecord("SELECT maDonVipr_sd FROM dbo.tblCauHinhTimKiem WHERE maDonVipr_sd=N'" + HttpContext.Current.Session.GetDonVi().maDonVi + "'") == false)
                {
                    string sql = " INSERT INTO dbo.tblCauHinhTimKiem( maDuAn ,tenDuAn ,nguonVon ,loaiKhoan , loaiCongTrinh , khoiCongTu ,khoiCongDen ,hoanThanhTu ,hoanThanhDen , maLoaiDApr_sd , maLoaiHinhpr_sd ,maDonVipr_sd ,nguoiThaoTac ,ngayThaotac)"
                                + " SELECT '0' AS maDuAn ,'1' AS tenDuAn ,'0' AS nguonVon ,'0' AS loaiKhoan , '0' AS loaiCongTrinh , NULL AS khoiCongTu ,NULL AS khoiCongDen ,NULL AS hoanThanhTu ,NULL AS hoanThanhDen ,NULL AS  maLoaiDApr_sd , NULL AS maLoaiHinhpr_sd ,'" + HttpContext.Current.Session.GetDonVi().maDonVi + "' AS maDonVipr_sd ,'" + HttpContext.Current.Session.GetCurrentUserID() + "' AS nguoiThaoTac ,'" + HttpContext.Current.Session.GetCurrentDatetimeMMddyyyy("MM/dd/yyyy") + "' AS ngayThaotac";
                    sqlFunc.ExeCuteNonQuery(sql);
                }

            }
        }
        [AjaxPro.AjaxMethod]
        public DataTable taoPhanTrang(AjaxPro.JavaScriptArray param)
        {
            DataTable _dt = new DataTable();
            _dt.Columns.Add("phanTrang", typeof(string));
            _dt.Columns.Add("tongSoTrang", typeof(string));
            string trang = "";
            long tongSoTrang = 0;
            try
            {
                string dkWhere = "";

                SqlFunction sqlFuncUer = new SqlFunction(HttpContext.Current.Session.GetConnectionString1());
                string maDV = HttpContext.Current.Session.GetDonVi().maDonVi;
                string strWhereDV = "";
                //string idGroup=sqlFuncUer.GetOneStringField("SELECT idGrouppr_sd FROM dbo.tblUsers WHERE tenDangNhap=N'" + HttpContext.Current.Session.GetCurrentUser().tenDangNhap + "'").ToLower();
                //if (idGroup == "userSD".ToLower() || idGroup == "userTH".ToLower())
                //    strWhereDV = " maDonVipr_sd IN(SELECT maDonvipr FROM tblDMDonvi WHERE maDonvipr=N'" + maDV + "' OR maDonvipr_cha=N'" + maDV + "'  OR  maDonvipr_cha IN  (SELECT  maDonvipr FROM  tblDMDonvi WHERE  maDonvipr_cha=N'" + maDV + "')) ";
                //else
                //    strWhereDV = " maDonVipr_sd IS NOT NULL ";
                if (sqlFuncUer.GetOneStringField("SELECT idGrouppr_sd FROM dbo.tblUsers WHERE tenDangNhap=N'" + HttpContext.Current.Session.GetCurrentUser().tenDangNhap + "'") == "userSD")
                {
                    strWhereDV = " (maDonVipr_chudautu=N'" + maDV + "' OR maDonVipr_qlda=N'" + maDV + "') ";
                }
                else if (sqlFuncUer.GetOneStringField("SELECT idGrouppr_sd FROM dbo.tblUsers WHERE tenDangNhap=N'" + HttpContext.Current.Session.GetCurrentUser().tenDangNhap + "'") == "tc")
                {
                    strWhereDV = " maDonVipr_cqcq=N'" + maDV + "' ";
                }
                else if (sqlFuncUer.GetOneStringField("SELECT idGrouppr_sd FROM dbo.tblUsers WHERE tenDangNhap=N'" + HttpContext.Current.Session.GetCurrentUser().tenDangNhap + "'") == "kbnn")
                {
                    strWhereDV = " maDonVipr_noimotk=N'" + maDV + "' ";
                }
                else if (sqlFuncUer.GetOneStringField("SELECT idGrouppr_sd FROM dbo.tblUsers WHERE tenDangNhap=N'" + HttpContext.Current.Session.GetCurrentUser().tenDangNhap + "'") == "ubnd")
                {
                    strWhereDV = " maDonVipr_capqd=N'" + maDV + "' ";
                }
                else if (sqlFuncUer.GetOneStringField("SELECT idGrouppr_sd FROM dbo.tblUsers WHERE tenDangNhap=N'" + HttpContext.Current.Session.GetCurrentUser().tenDangNhap + "'") == "userTH")
                {
                    strWhereDV = " maDonVipr_chudautu IN (SELECT maDonVipr FROM dbo.tblDMDonVi WHERE maDonvipr_cha=N'" + maDV + "') ";
                }

                SqlFunction sqlFunc = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                DataTable dtCauHinh = sqlFunc.GetData("SELECT maDuAn ,tenDuAn ,nguonVon ,loaiKhoan ,loaiCongTrinh ,CONVERT(VARCHAR(10),khoiCongTu,103) AS khoiCongTu ,CONVERT(VARCHAR(10),khoiCongDen,103) AS khoiCongDen ,CONVERT(VARCHAR(10),hoanThanhTu,103) AS hoanThanhTu ,CONVERT(VARCHAR(10),hoanThanhDen,103) AS hoanThanhDen ,maLoaiDApr_sd ,maLoaiHinhpr_sd,maNguonVonpr_sd FROM dbo.tblCauHinhTimKiem WHERE maDonVipr_sd=N'" + HttpContext.Current.Session.GetDonVi().maDonVi + "'");
                if (dtCauHinh.Rows.Count > 0)
                {
                    if (dtCauHinh.Rows[0]["maDuAn"].ToString() == "True")
                    {
                        dkWhere += " maDuAn LIKE '%'+@maDuAn+'%'";
                    }
                    if (dtCauHinh.Rows[0]["tenDuAn"].ToString() == "True")
                    {
                        dkWhere += (dkWhere == "" ? "" : " OR") + " tenDuAn LIKE '%'+@tenDuAn+'%'";
                    }
                    //if (dtCauHinh.Rows[0]["maNguonVonpr_sd"].ToString().Trim() != "")
                    //{
                    //    dkWhere += (dkWhere == "" ? "" : " AND") + " sttDuAnpr IN (SELECT sttDuAnpr_sd FROM dbo.tblNguonVonDA WHERE maNguonVonpr_sd=N'" + dtCauHinh.Rows[0]["maNguonVonpr_sd"].ToString().Trim() + "')";
                    //}
                    //if (dtCauHinh.Rows[0]["maLoaiHinhpr_sd"].ToString().Trim() != "")
                    //{
                    //    dkWhere += (dkWhere == "" ? "" : " AND") + " maLoaiCTrinh_sd=N'" + dtCauHinh.Rows[0]["maLoaiHinhpr_sd"].ToString().Trim() + "'";
                    //}
                    //if (dtCauHinh.Rows[0]["maLoaiDApr_sd"].ToString().Trim() != "")
                    //{
                    //    dkWhere += (dkWhere == "" ? "" : " AND") + " maLoaiDApr_sd=N'" + dtCauHinh.Rows[0]["maLoaiDApr_sd"].ToString().Trim() + "'";
                    //}
                    if (dkWhere != "")
                        dkWhere = "(" + dkWhere + ")";

                    if (dtCauHinh.Rows[0]["maNguonVonpr_sd"].ToString() != "" && dtCauHinh.Rows[0]["maLoaiDApr_sd"].ToString() != "")
                    {
                        dkWhere += (dkWhere == "" ? "" : " AND ")
                            + " (sttDuAnpr IN (SELECT sttDuAnpr_sd FROM dbo.tblNguonVonDA WHERE maNguonVonpr_sd=N'" + dtCauHinh.Rows[0]["maNguonVonpr_sd"].ToString() + "')"
                            + " AND maLoaiDApr_sd=N'" + dtCauHinh.Rows[0]["maLoaiDApr_sd"].ToString().Trim() + "')";
                    }
                    else if (dtCauHinh.Rows[0]["maNguonVonpr_sd"].ToString() != "" && dtCauHinh.Rows[0]["maLoaiDApr_sd"].ToString() == "")
                    {
                        dkWhere += (dkWhere == "" ? "" : " AND ") + " (sttDuAnpr IN (SELECT sttDuAnpr_sd FROM dbo.tblNguonVonDA WHERE maNguonVonpr_sd=N'" + dtCauHinh.Rows[0]["maNguonVonpr_sd"].ToString() + "')" + " )";
                    }
                    else if (dtCauHinh.Rows[0]["maNguonVonpr_sd"].ToString() == "" && dtCauHinh.Rows[0]["maLoaiDApr_sd"].ToString() != "")
                    {
                        dkWhere += (dkWhere == "" ? "" : " AND ") + " maLoaiDApr_sd=N'" + dtCauHinh.Rows[0]["maLoaiDApr_sd"].ToString().Trim() + "'";
                    }
                    //dieu kien noi
                    if (dkWhere == "")
                        dkWhere = strWhereDV;
                    else
                        dkWhere = dkWhere + (strWhereDV != "" ? " AND " + strWhereDV : "");
                }

                long soDongTrenTrang = 5;
                //long tongSoDong = Convert.ToInt64(sqlFunc.GetOneDecimalField("select CONVERT(DECIMAL,COUNT(temp.sttMaCaBietpr)) from (SELECT sttMaCaBietpr,maCaBiet,maKho=(SELECT maLoaiAPpr_sd FROM dbo.tblBienMuc WHERE sttNhapAPCTpr_sd=tblMaCaBiet.sttNhapAPCTpr_sd),nhanDe=(SELECT nhanDe FROM dbo.tblBienMuc WHERE sttNhapAPCTpr_sd=tblMaCaBiet.sttNhapAPCTpr_sd) ,tacGia=(SELECT (SELECT tenTacGia FROM dbo.tblDMTacGia WHERE maTacGiapr=maTacGiapr_sd) FROM dbo.tblBienMuc WHERE sttNhapAPCTpr_sd=tblMaCaBiet.sttNhapAPCTpr_sd) ,maAnPham=(SELECT kyHieu FROM dbo.tblBienMuc WHERE sttNhapAPCTpr_sd=tblMaCaBiet.sttNhapAPCTpr_sd) ,namXB=(SELECT namXuatBan FROM dbo.tblBienMuc WHERE sttNhapAPCTpr_sd=tblMaCaBiet.sttNhapAPCTpr_sd) ,nhaXB=(SELECT (SELECT tenNhaXuatBan FROM dbo.tblDMNhaXuatBan WHERE maNhaXuatBanpr=maNXBpr_sd) FROM dbo.tblBienMuc WHERE sttNhapAPCTpr_sd=tblMaCaBiet.sttNhapAPCTpr_sd) FROM dbo.tblMaCaBiet WHERE dbo.tblMaCaBiet.maDonVipr_sd=N'" + HttpContext.Current.Session.GetDonVi().maDonVi + "' and " + trangThai + ") as temp where  " + nhanDe + " AND " + tacGia + maKho));
                SqlConnection sqlCon = new SqlConnection(HttpContext.Current.Session.GetConnectionString2());
                sqlCon.Open();
                //string sql = " SELECT CONVERT(DECIMAL,COUNT(mathietBipr)) FROM dbo.tblDMThietBi WHERE capHoc IN (" + items + ") " + strWhere;

                string sql = " SELECT CONVERT(DECIMAL,COUNT(sttDuAnpr)) FROM dbo.tblDuAn " + (dkWhere != "" ? " WHERE " + dkWhere : dkWhere)+" AND chinhThuc='1' ";

                SqlCommand cmd = new SqlCommand(sql, sqlCon);
                if (dtCauHinh.Rows.Count > 0)
                {
                    if (dtCauHinh.Rows[0]["maDuAn"].ToString() == "True")
                    {
                        cmd.Parameters.Add(new SqlParameter("@maDuAn", param[0].ToString()));
                    }
                    if (dtCauHinh.Rows[0]["tenDuAn"].ToString() == "True")
                    {
                        cmd.Parameters.Add(new SqlParameter("@tenDuAn", param[0].ToString()));
                    }
                    //if (dtCauHinh.Rows[0]["nguonVon"].ToString() == "True")
                    //{
                    //    cmd.Parameters.Add(new SqlParameter("@nguonVon", param[0].ToString()));
                    //}
                    //if (dtCauHinh.Rows[0]["loaiCongTrinh"].ToString() == "True")
                    //{
                    //    cmd.Parameters.Add(new SqlParameter("@loaiCongTrinh", param[0].ToString()));
                    //}
                    //if (dtCauHinh.Rows[0]["maLoaiDApr_sd"].ToString().Trim() != "")
                    //{
                    //    cmd.Parameters.Add(new SqlParameter("@loaiDuAn", param[0].ToString()));
                    //}
                    if (dtCauHinh.Rows[0]["khoiCongTu"].ToString().Trim().Length > 0)
                    {
                        dkWhere += (dkWhere == "" ? "" : " OR") + " thoiGianKCongDuyet>= '" + _mChuyenChuoiSangNgay(dtCauHinh.Rows[0]["khoiCongTu"].ToString()) + "'";
                    }
                    if (dtCauHinh.Rows[0]["khoiCongDen"].ToString().Trim().Length > 0)
                    {

                        dkWhere += (dkWhere == "" ? "" : " OR") + " thoiGianKCongDuyet<= '" + _mChuyenChuoiSangNgay(dtCauHinh.Rows[0]["khoiCongDen"].ToString()) + "'";
                    }
                    if (dtCauHinh.Rows[0]["hoanThanhTu"].ToString().Trim().Length > 0)
                    {
                        dkWhere += (dkWhere == "" ? "" : " OR") + " thoiGianHThanhDuyet>= '" + _mChuyenChuoiSangNgay(dtCauHinh.Rows[0]["hoanThanhDen"].ToString()) + "'";
                    }
                    if (dtCauHinh.Rows[0]["hoanThanhDen"].ToString().Trim().Length > 0)
                    {
                        dkWhere += (dkWhere == "" ? "" : " OR") + " thoiGianHThanhDuyet<= '" + _mChuyenChuoiSangNgay(dtCauHinh.Rows[0]["hoanThanhDen"].ToString()) + "'";
                    }
                }
                //if (param[0].ToString() == "1")
                //{
                //    cmd.Parameters.Add(new SqlParameter("@mathietBipr_sd", (param[1].ToString().ToUpper() != "Nhập nội dung cần tìm".ToUpper() ? param[1].ToString() : "")));
                //}
                //else if (param[0].ToString() == "2")
                //{
                //    cmd.Parameters.Add(new SqlParameter("@tenThietBi", (param[1].ToString().ToUpper() != "Nhập nội dung cần tìm".ToUpper() ? param[1].ToString() : "")));
                //}
                //else if (param[0].ToString() == "3")
                //{
                //    cmd.Parameters.Add(new SqlParameter("@hienTrang", (param[1].ToString().ToUpper() != "Nhập nội dung cần tìm".ToUpper() ? param[1].ToString() : "")));
                //}
                //else if (param[0].ToString() == "5")
                //{
                //    cmd.Parameters.Add(new SqlParameter("@monHoc", param[1].ToString()));
                //}
                //else
                //{
                //    cmd.Parameters.Add(new SqlParameter("@maCaBiet", (param[1].ToString().ToUpper() != "Nhập nội dung cần tìm".ToUpper() ? param[1].ToString() : "")));
                //}
                object result = cmd.ExecuteScalar();
                cmd.Dispose();
                sqlCon.Close();
                long tongSoDong = Convert.ToInt64((result != null ? result : 0));

                if (tongSoDong % soDongTrenTrang == 0)
                {
                    tongSoTrang = tongSoDong / soDongTrenTrang;
                }
                else
                {
                    tongSoTrang = (tongSoDong / soDongTrenTrang) + 1;
                }
                trang = "<span><a class='cTrang' onclick='loadPageDau()' href='javascript://'>Đầu</a></span> <span><a class='cTrang' onclick='loadPageTruoc()' href='javascript://'>Trước </a></span>  ";
                for (int i = 0; i < tongSoTrang; i++)
                {
                    if (i == 0)
                    {
                        trang += "<span id='trang" + (i + 1) + "'><a id='cTrang" + (i + 1) + "' class='cTrang' href='javascript://' onclick='loadPage(" + (i + 1) + ")'>1</a></span>";
                    }
                    else
                    {
                        trang += "<span id='trang" + (i + 1) + "'> <a id='cTrang" + (i + 1) + "' class='cTrang' href='javascript://' onclick='loadPage(" + (i + 1) + ")'>" + (i + 1).ToString() + "</a></span>";
                    }
                }
                if (tongSoTrang == 0)
                {
                    trang = "<span id='trang1'><a id='cTrang1' class='cTrang' href='javascript://'>1</a></span>" + "  <span><a class='cTrang' href='javascript://' onclick='loadPageSau()'>Sau</a></span>  <span><a class='cTrang' onclick='loadPageCuoi()' href='javascript://'>Cuối </a></span>";
                }
                var nfi = new NumberFormatInfo { NumberDecimalSeparator = ",", NumberGroupSeparator = "." };
                trang += "<span style='float:left'>Tổng số dự án: " + tongSoDong.ToString("N0", nfi) + " </span> <span><a class='cTrang' href='javascript://' onclick='loadPageSau()'>Sau</a></span>  <span><a class='cTrang' onclick='loadPageCuoi()' href='javascript://'>Cuối </a></span>";
            }
            catch
            {
                //trang = "Trang: 1" + "Tổng số dòng: 1";
                trang = "<span style='float:left'>Tổng số dự án: 0 </span> <span id='trang1'><a id='cTrang1' class='cTrang' href='javascript://'>Trang: 1</a></span>" + "  <span><a class='cTrang' href='javascript://' onclick='loadPageSau()'>Sau</a></span>  <span><a class='cTrang' onclick='loadPageCuoi()' href='javascript://'>Cuối </a></span>";

            }
            _dt.Rows.Add(trang, tongSoTrang.ToString());
            return _dt;
        }
        [AjaxPro.AjaxMethod]
        public string layDuLieu(AjaxPro.JavaScriptArray param)
        {
            SqlFunction sqlFunc = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            SqlFunction sqlFuncUer = new SqlFunction(HttpContext.Current.Session.GetConnectionString1());
            string maDV = HttpContext.Current.Session.GetDonVi().maDonVi;
            string strWhereDV = "";
            //string idGroup = sqlFuncUer.GetOneStringField("SELECT idGrouppr_sd FROM dbo.tblUsers WHERE tenDangNhap=N'" + HttpContext.Current.Session.GetCurrentUser().tenDangNhap + "'").ToLower();
            //if (idGroup == "userSD".ToLower() || idGroup == "userTH".ToLower())
            //    strWhereDV = " maDonVipr_sd IN(SELECT maDonvipr FROM tblDMDonvi WHERE maDonvipr=N'" + maDV + "' OR maDonvipr_cha=N'" + maDV + "'  OR  maDonvipr_cha IN  (SELECT  maDonvipr FROM  tblDMDonvi WHERE  maDonvipr_cha=N'" + maDV + "')) ";
            //else
            //    strWhereDV = " maDonVipr_sd IS NOT NULL ";

            if (sqlFuncUer.GetOneStringField("SELECT idGrouppr_sd FROM dbo.tblUsers WHERE tenDangNhap=N'" + HttpContext.Current.Session.GetCurrentUser().tenDangNhap + "'") == "userSD")
            {
                strWhereDV = " (maDonVipr_chudautu=N'" + maDV + "'  OR maDonVipr_qlda=N'" + maDV + "') ";
            }
            else if (sqlFuncUer.GetOneStringField("SELECT idGrouppr_sd FROM dbo.tblUsers WHERE tenDangNhap=N'" + HttpContext.Current.Session.GetCurrentUser().tenDangNhap + "'") == "tc")
            {
                strWhereDV = " maDonVipr_cqcq=N'" + maDV + "' ";
            }
            else if (sqlFuncUer.GetOneStringField("SELECT idGrouppr_sd FROM dbo.tblUsers WHERE tenDangNhap=N'" + HttpContext.Current.Session.GetCurrentUser().tenDangNhap + "'") == "kbnn")
            {
                strWhereDV = " maDonVipr_noimotk=N'" + maDV + "' ";
            }
            else if (sqlFuncUer.GetOneStringField("SELECT idGrouppr_sd FROM dbo.tblUsers WHERE tenDangNhap=N'" + HttpContext.Current.Session.GetCurrentUser().tenDangNhap + "'") == "ubnd")
            {
                strWhereDV = " maDonVipr_capqd=N'" + maDV + "' ";
            }
            else if (sqlFuncUer.GetOneStringField("SELECT idGrouppr_sd FROM dbo.tblUsers WHERE tenDangNhap=N'" + HttpContext.Current.Session.GetCurrentUser().tenDangNhap + "'") == "userTH")
            {
                strWhereDV = "  maDonVipr_chudautu IN (SELECT maDonVipr FROM dbo.tblDMDonVi WHERE maDonvipr_cha=N'" + maDV + "') ";
            }

            string strWhere = "</br>";
            string dkWhere = "";
            DataTable dtCauHinh = sqlFunc.GetData("SELECT maDuAn ,tenDuAn ,nguonVon ,loaiKhoan ,loaiCongTrinh ,CONVERT(VARCHAR(10),khoiCongTu,103) AS khoiCongTu ,CONVERT(VARCHAR(10),khoiCongDen,103) AS khoiCongDen ,CONVERT(VARCHAR(10),hoanThanhTu,103) AS hoanThanhTu ,CONVERT(VARCHAR(10),hoanThanhDen,103) AS hoanThanhDen ,maLoaiDApr_sd ,maLoaiHinhpr_sd,maNguonVonpr_sd FROM dbo.tblCauHinhTimKiem WHERE maDonVipr_sd=N'" + HttpContext.Current.Session.GetDonVi().maDonVi + "'");
            if (dtCauHinh.Rows.Count > 0)
            {
                if (dtCauHinh.Rows[0]["maDuAn"].ToString() == "True")
                {
                    dkWhere += " maDuAn LIKE '%'+@maDuAn+'%'";
                }
                if (dtCauHinh.Rows[0]["tenDuAn"].ToString() == "True")
                {
                    dkWhere += (dkWhere == "" ? "" : " OR") + " tenDuAn LIKE '%'+@tenDuAn+'%'";
                }
                //if (dtCauHinh.Rows[0]["maNguonVonpr_sd"].ToString() != "" && dtCauHinh.Rows[0]["maLoaiHinhpr_sd"].ToString() != "" && dtCauHinh.Rows[0]["maLoaiDApr_sd"].ToString() != "")
                //{
                //    dkWhere += (dkWhere == "" ? "" : " AND ") 
                //        + " (sttDuAnpr IN (SELECT sttDuAnpr_sd FROM dbo.tblNguonVonDA WHERE maNguonVonpr_sd=N'" + dtCauHinh.Rows[0]["maNguonVonpr_sd"].ToString() + "')"
                //        + " AND maLoaiHinhpr_sd=N'" + dtCauHinh.Rows[0]["maLoaiHinhpr_sd"].ToString().Trim() + "'"
                //        + " AND maLoaiDApr_sd=N'" + dtCauHinh.Rows[0]["maLoaiDApr_sd"].ToString().Trim() + "')"
                //        ;
                //}else if (dtCauHinh.Rows[0]["maNguonVonpr_sd"].ToString() == "" && dtCauHinh.Rows[0]["maLoaiHinhpr_sd"].ToString() != "" && dtCauHinh.Rows[0]["maLoaiDApr_sd"].ToString() != "")
                //{
                //    dkWhere += (dkWhere == "" ? "" : " AND ")
                //        + " (maLoaiHinhpr_sd=N'" + dtCauHinh.Rows[0]["maLoaiHinhpr_sd"].ToString().Trim() + "'"
                //        + " AND maLoaiDApr_sd=N'" + dtCauHinh.Rows[0]["maLoaiDApr_sd"].ToString().Trim() + "')"
                //        ;
                //}else if (dtCauHinh.Rows[0]["maNguonVonpr_sd"].ToString() == "" && dtCauHinh.Rows[0]["maLoaiHinhpr_sd"].ToString() == "" && dtCauHinh.Rows[0]["maLoaiDApr_sd"].ToString() != "")
                //{
                //    dkWhere += (dkWhere == "" ? "" : " AND ")
                //        + " AND (maLoaiDApr_sd=N'" + dtCauHinh.Rows[0]["maLoaiDApr_sd"].ToString().Trim() + "')"
                //        ;
                //}
                //else if (dtCauHinh.Rows[0]["maNguonVonpr_sd"].ToString() != "" && dtCauHinh.Rows[0]["maLoaiHinhpr_sd"].ToString() == "" && dtCauHinh.Rows[0]["maLoaiDApr_sd"].ToString() != "")
                //{
                //    dkWhere += (dkWhere == "" ? "" : " AND ")
                //        + " (sttDuAnpr IN (SELECT sttDuAnpr_sd FROM dbo.tblNguonVonDA WHERE maNguonVonpr_sd=N'" + dtCauHinh.Rows[0]["maNguonVonpr_sd"].ToString() + "')"
                //        + " AND maLoaiDApr_sd=N'" + dtCauHinh.Rows[0]["maLoaiDApr_sd"].ToString().Trim() + "')"
                //        ;
                //}else if (dtCauHinh.Rows[0]["maNguonVonpr_sd"].ToString() != "" && dtCauHinh.Rows[0]["maLoaiHinhpr_sd"].ToString() != "" && dtCauHinh.Rows[0]["maLoaiDApr_sd"].ToString() == "")
                //{
                //    dkWhere += (dkWhere == "" ? "" : " AND ")
                //        + " (sttDuAnpr IN (SELECT sttDuAnpr_sd FROM dbo.tblNguonVonDA WHERE maNguonVonpr_sd=N'" + dtCauHinh.Rows[0]["maNguonVonpr_sd"].ToString() + "')"
                //        + " AND maLoaiHinhpr_sd=N'" + dtCauHinh.Rows[0]["maLoaiHinhpr_sd"].ToString().Trim() + "')"
                //        ;
                //}
                //else if (dtCauHinh.Rows[0]["maNguonVonpr_sd"].ToString() != "" && dtCauHinh.Rows[0]["maLoaiHinhpr_sd"].ToString() == "" && dtCauHinh.Rows[0]["maLoaiDApr_sd"].ToString() == "")
                //{
                //    dkWhere += (dkWhere == "" ? "" : " AND ")
                //        + " (sttDuAnpr IN (SELECT sttDuAnpr_sd FROM dbo.tblNguonVonDA WHERE maNguonVonpr_sd=N'" + dtCauHinh.Rows[0]["maNguonVonpr_sd"].ToString() + "'))"
                //        ;
                //}
                //if (dtCauHinh.Rows[0]["loaiCongTrinh"].ToString() == "True")
                //{
                //    dkWhere += (dkWhere == "" ? "" : " OR") + " maLoaiCTrinh_sd IN(SELECT maLoaiCTrinhpr FROM dbo.tblDMLoaiCTrinh WHERE tenLoaiCTrinh LIKE '%'+@loaiCongTrinh+'%')";
                //}
                //if (dtCauHinh.Rows[0]["maLoaiDApr_sd"].ToString().Trim() != "")
                //{
                //    dkWhere += (dkWhere == "" ? "" : " OR") + " maLoaiDApr_sd IN(SELECT maLoaiDApr FROM dbo.tblDMLoaiDA WHERE tenLoaiDA LIKE '%'+@loaiDuAn+'%')";
                //}
                //if (dtCauHinh.Rows[0]["maLoaiHinhpr_sd"].ToString().Trim() != "")
                //{
                //    dkWhere += (dkWhere == "" ? "" : " AND ") + " maLoaiCTrinh_sd=N'" + dtCauHinh.Rows[0]["maLoaiHinhpr_sd"].ToString().Trim() + "'";
                //}
                //if (dtCauHinh.Rows[0]["maLoaiDApr_sd"].ToString().Trim() != "")
                //{
                //    dkWhere += (dkWhere == "" ? "" : " AND ") + " maLoaiDApr_sd=N'" + dtCauHinh.Rows[0]["maLoaiDApr_sd"].ToString().Trim() + "')";
                //}
                if (dkWhere != "")
                    dkWhere = "(" + dkWhere + ")";

                if (dtCauHinh.Rows[0]["maNguonVonpr_sd"].ToString() != "" && dtCauHinh.Rows[0]["maLoaiDApr_sd"].ToString() != "")
                {
                    dkWhere += (dkWhere == "" ? "" : " AND ")
                        + " (sttDuAnpr IN (SELECT sttDuAnpr_sd FROM dbo.tblNguonVonDA WHERE maNguonVonpr_sd=N'" + dtCauHinh.Rows[0]["maNguonVonpr_sd"].ToString() + "')"
                        + " AND maLoaiDApr_sd=N'" + dtCauHinh.Rows[0]["maLoaiDApr_sd"].ToString().Trim() + "')";
                }
                else if (dtCauHinh.Rows[0]["maNguonVonpr_sd"].ToString() != "" && dtCauHinh.Rows[0]["maLoaiDApr_sd"].ToString() == "")
                {
                    dkWhere += (dkWhere == "" ? "" : " AND ")
                       + " (sttDuAnpr IN (SELECT sttDuAnpr_sd FROM dbo.tblNguonVonDA WHERE maNguonVonpr_sd=N'" + dtCauHinh.Rows[0]["maNguonVonpr_sd"].ToString() + "')"
                       + " )";
                }
                else if (dtCauHinh.Rows[0]["maNguonVonpr_sd"].ToString() == "" && dtCauHinh.Rows[0]["maLoaiDApr_sd"].ToString() != "")
                {
                    dkWhere += (dkWhere == "" ? "" : " AND ")
                        + " maLoaiDApr_sd=N'" + dtCauHinh.Rows[0]["maLoaiDApr_sd"].ToString().Trim() + "'";
                }
                if (dkWhere == "")
                    dkWhere = strWhereDV;
                else
                    dkWhere = dkWhere + (strWhereDV != "" ? " AND " + strWhereDV : "");
            }

            long soDongTrenTrang = 5;
            DataTable dtDanhMucThietBi = new DataTable();
            int trang = Convert.ToInt32((param[1].ToString() == "" ? "0" : param[1].ToString()));
            SqlConnection sqlCon = new SqlConnection(HttpContext.Current.Session.GetConnectionString2());

            string sql = "select Top " + soDongTrenTrang + " * from ( SELECT sttDuAnpr, maDuAn, tenDuAn,chinhThuc FROM dbo.tblDuAn " + (dkWhere != "" ? " WHERE " + dkWhere : dkWhere) + " AND chinhThuc='1') as a " +
                                         " where a.sttDuAnpr not in (SELECT TOP " + ((trang - 1) * soDongTrenTrang) + " sttDuAnpr FROM dbo.tblDuAn " + (dkWhere != "" ? " WHERE " + dkWhere : dkWhere) + " AND chinhThuc='1'  ORDER BY sttDuAnpr ) ORDER BY sttDuAnpr ";

            SqlCommand cmd = new SqlCommand(sql, sqlCon);
            //cmd.Parameters.Add(new SqlParameter("@hienTrang", (txtNoiDung.Text.ToUpper() != "Nhập nội dung cần tìm".ToUpper() ? txtNoiDung.Text : "")));


            if (dtCauHinh.Rows.Count > 0)
            {
                if (dtCauHinh.Rows[0]["maDuAn"].ToString() == "True")
                {
                    cmd.Parameters.Add(new SqlParameter("@maDuAn", param[0].ToString()));
                }
                if (dtCauHinh.Rows[0]["tenDuAn"].ToString() == "True")
                {
                    cmd.Parameters.Add(new SqlParameter("@tenDuAn", param[0].ToString()));
                }
                //if (dtCauHinh.Rows[0]["nguonVon"].ToString() == "True")
                //{
                //    cmd.Parameters.Add(new SqlParameter("@nguonVon", param[0].ToString()));
                //}
                //if (dtCauHinh.Rows[0]["loaiCongTrinh"].ToString() == "True")
                //{
                //    cmd.Parameters.Add(new SqlParameter("@loaiCongTrinh", param[0].ToString()));
                //}
                //if (dtCauHinh.Rows[0]["maLoaiDApr_sd"].ToString().Trim() != "")
                //{
                //    cmd.Parameters.Add(new SqlParameter("@loaiDuAn", param[0].ToString()));
                //}
                //if (dtCauHinh.Rows[0]["maLoaiHinhpr_sd"].ToString().Trim() != "")
                //{
                //    dkWhere += (dkWhere == "" ? "" : " AND") + " maLoaiCTrinh_sd=N'" + dtCauHinh.Rows[0]["maLoaiHinhpr_sd"].ToString().Trim() + "'";
                //}
                //if (dtCauHinh.Rows[0]["maLoaiDApr_sd"].ToString().Trim() != "")
                //{
                //    dkWhere += (dkWhere == "" ? "" : " AND") + " maLoaiDApr_sd=N'" + dtCauHinh.Rows[0]["maLoaiDApr_sd"].ToString().Trim() + "'";
                //}
                if (dtCauHinh.Rows[0]["khoiCongTu"].ToString().Trim().Length > 0)
                {
                    dkWhere += (dkWhere == "" ? "" : " OR") + " thoiGianKCongDuyet>= '" + _mChuyenChuoiSangNgay(dtCauHinh.Rows[0]["khoiCongTu"].ToString()) + "'";
                }
                if (dtCauHinh.Rows[0]["khoiCongDen"].ToString().Trim().Length > 0)
                {

                    dkWhere += (dkWhere == "" ? "" : " OR") + " thoiGianKCongDuyet<= '" + _mChuyenChuoiSangNgay(dtCauHinh.Rows[0]["khoiCongDen"].ToString()) + "'";
                }
                if (dtCauHinh.Rows[0]["hoanThanhTu"].ToString().Trim().Length > 0)
                {

                    dkWhere += (dkWhere == "" ? "" : " OR") + " thoiGianHThanhDuyet>= '" + _mChuyenChuoiSangNgay(dtCauHinh.Rows[0]["hoanThanhDen"].ToString()) + "'";

                }
                if (dtCauHinh.Rows[0]["hoanThanhDen"].ToString().Trim().Length > 0)
                {
                    dkWhere += (dkWhere == "" ? "" : " OR") + " thoiGianHThanhDuyet<= '" + _mChuyenChuoiSangNgay(dtCauHinh.Rows[0]["hoanThanhDen"].ToString()) + "'";
                }

            }
            try
            {
                SqlDataAdapter sqlDa = new SqlDataAdapter(cmd);
                sqlDa.Fill(dtDanhMucThietBi);
                cmd.Dispose();
                sqlCon.Close();
            }
            catch
            {
            }
            for (int i = 0; i < dtDanhMucThietBi.Rows.Count; i++)
            {
                strWhere += "<a class=\"btn\" data-popup-open=\"popup-1\" href=\"#\" onclick=\"openWinDow('" + dtDanhMucThietBi.Rows[i]["sttDuAnpr"].ToString() + "')\">" + dtDanhMucThietBi.Rows[i]["tenDuAn"].ToString() + "</a></br></br>";
            }
            return strWhere;
            //onclick=\"openWinDow('" + dtDanhMucThietBi.Rows[i]["sttDuAnpr"].ToString() + "')\"
        }
        [AjaxPro.AjaxMethod]
        public DataTable layTTDuAn(object stt)
        {
            DataTable dt = new DataTable();
            SqlFunction sqlFunc = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            try
            {
                dt = sqlFunc.GetData(@"SELECT sttDuAnpr,maDuAn,tenDuAn,diaDiemXD
                                    ,coQuanQD=(SELECT tenDonVi FROM tblDMDonVi WHERE maDonVipr=maDonVipr_capqd)
                                    ,banQLDA=(SELECT tenDonVi FROM tblDMDonVi WHERE maDonVipr=maDonVipr_qlda)
                                    ,tenChuDauTu=(SELECT tenDonVi FROM tblDMDonVi WHERE maDonVipr=maDonVipr_chudautu)
                                    ,thoiGianKCong=ISNULL(CONVERT(VARCHAR(10),thoiGianKCongDuyet,103),'')
                                    ,thoiGianHThanh=ISNULL(CONVERT(VARCHAR(10),thoiGianHThanhDuyet,103),'')
                                    ,thoiGianKCongTH=ISNULL(CONVERT(VARCHAR(10),(SELECT TOP 1 tblBanGiaoDA.thoiGianKCongTH FROM dbo.tblBanGiaoDA WHERE sttDuAnpr_sd = sttDuAnpr),103),'')
                                    ,thoiGianHThanhTH=ISNULL(CONVERT(VARCHAR(10),(SELECT TOP 1 tblBanGiaoDA.thoiGianHThanhTH FROM dbo.tblBanGiaoDA WHERE sttDuAnpr_sd = sttDuAnpr),103),'')
                                    FROM dbo.tblDuAn WHERE sttDuAnpr=N'" + stt + "'");
                return dt;
            }
            catch
            {
                return dt;
            }
        }
        protected void Grid1_OnRebind(object sender, EventArgs e)
        {
            try
            {
                SqlFunction sqlFunc = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                SqlFunction sqlFuncUer = new SqlFunction(HttpContext.Current.Session.GetConnectionString1());
                string maDV = HttpContext.Current.Session.GetDonVi().maDonVi;
                string strWhere = "";
                if (sqlFuncUer.GetOneStringField("SELECT idGrouppr_sd FROM dbo.tblUsers WHERE tenDangNhap=N'" + HttpContext.Current.Session.GetCurrentUser().tenDangNhap + "'") == "userSD" || sqlFuncUer.GetOneStringField("SELECT idGrouppr_sd FROM dbo.tblUsers WHERE tenDangNhap=N'" + HttpContext.Current.Session.GetCurrentUser().tenDangNhap + "'") == "userTH")
                    strWhere = " maDonVipr_sd IN(SELECT maDonvipr FROM tblDMDonvi WHERE maDonvipr=N'" + maDV + "' OR maDonvipr_cha=N'" + maDV + "'  OR  maDonvipr_cha IN  (SELECT  maDonvipr FROM  tblDMDonvi WHERE  maDonvipr_cha=N'" + maDV + "'))";
                else
                    strWhere = " maDonVipr_sd IS NOT NULL ";

                string sttDA = hdfSTTDA.Value.ToString();
                //                string sql = @"SELECT * FROM (
                //                                SELECT tenNguonVon
                //                                ,tongMucDauTu=ISNULL((SELECT ISNULL(SUM(giaTriVonDKien),0) FROM dbo.tblNguonVonDA WHERE sttDuAnpr_sd='" + sttDA + "' AND maNguonVonpr_sd=b.maNguonVonpr AND " + strWhere + @"),0)
                //                                ,keHoachVon=ISNULL((SELECT ISNULL(SUM(keHoachvonnam),0)+ISNULL(SUM(keHoachvonnamTraXDCB),0) FROM dbo.tblGiaoKehoachVonCT WHERE sttDuAnpr_sd='" + sttDA + "' AND maNguonVonpr_sd=b.maNguonVonpr AND sttGiaoKHVonpr_sd IN (SELECT sttGiaoKHVonpr FROM dbo.tblGiaoKehoachVon WHERE  " + strWhere + @")),0)
                //                                ,giaTriNghiemThu=ISNULL((SELECT ISNULL(SUM(giaTriNghiemThu),0) FROM dbo.tblHopDongCT WHERE maNguonVonpr_sd=b.maNguonVonpr AND sttHopDongpr_sd IN (SELECT sttHopDongpr FROM dbo.tblHopDong WHERE sttDuAnpr_sd='" + sttDA + "' AND  " + strWhere + @")),0)
                //                                ,khoiLuongGNgan=ISNULL((SELECT ISNULL(SUM(soTien),0) FROM dbo.tblChungTuChiCT WHERE maNguonVonpr_sd=b.maNguonVonpr AND sttCTChipr_sd IN (SELECT sttCTChipr FROM dbo.tblChungTuChi WHERE sttDuAnpr_sd='" + sttDA + "' AND " + strWhere + @")),0)
                //                                ,khoiLuongQToan=ISNULL((SELECT ISNULL(SUM(giaTriQuyetToan),0) FROM dbo.tblTapHopCP WHERE sttCPDTuXDCTpr_sd IN (SELECT sttCPDTuXDCTpr FROM dbo.tblCPDauTuXDCT WHERE maNguonVonpr_sd=b.maNguonVonpr) AND LEN(ISNULL(maCPDauTuXDCTpr_sd,''))>0 AND  sttHSQTpr_sd IN(SELECT sttHSQTpr FROM dbo.tblHoSoQT WHERE sttDuAnpr_sd='" + sttDA + "' AND  " + strWhere + @")),0)
                //                                FROM (
                //                                SELECT * FROM dbo.tblDMNguonVon WHERE maNguonVonpr IN(SELECT maNguonVonpr_sd FROM dbo.tblNguonVonDA WHERE sttDuAnpr_sd='" + sttDA + @"')
                //                                ) AS b
                //                                ) AS c WHERE tongMucDauTu>0";
                string nienDo = HttpContext.Current.Session.GetNamSudung();
                decimal nienDoSau = Convert.ToDecimal(HttpContext.Current.Session.GetNamSudung()) + 1;
                //string tuNgay = "01/01/" + HttpContext.Current.Session.GetNamSudung();
                //string denNgay = "01/31/" + nienDoSau;
                string sql = @"SELECT tenNguonVon,SUM(tongMucDauTu) tongMucDauTu,SUM(keHoachVon) keHoachVon
                                ,SUM(giaTriNghiemThu) giaTriNghiemThu,SUM(khoiLuongGNgan) khoiLuongGNgan,SUM(khoiLuongQToan) khoiLuongQToan FROM (
                                SELECT tenNguonVon,tongMucDauTu,keHoachVon
                                ,giaTriNghiemThu,khoiLuongGNgan,khoiLuongQToan FROM (
                                SELECT (SELECT tenNguonVon FROM dbo.tblDMNguonVon WHERE maNguonVonpr=b.maNguonVonpr_sd) AS tenNguonVon
                                ,tongMucDauTu=(giaTriVonQD+tongMucDauTuTang-tongMucDauTuGiam)
                                ,keHoachVon=(keHoachVonNam+keHoachVonTang-keHoachVonGiam),giaTriNghiemThu=(giaTriTHien+giaTriTHienPL)
                                ,khoiLuongGNgan=giaTriGiaiNgan,khoiLuongQToan FROM(
                                SELECT sttDuAnpr_sd,maNguonVonpr_sd
                                ,giaTriVonQD=(SELECT ISNULL(SUM(giaTriVonQD),0) FROM tblNguonVonDA d WHERE d.sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND d.maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd)
                                ,tongMucDauTuTang=(SELECT ISNULL(SUM(giaTriVonQDTang),0) FROM dbo.tblDieuChinhMucDTuCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttDCMucDTupr_sd IN (SELECT sttDCMucDTupr FROM dbo.tblDieuChinhMucDTu WHERE sttDuAnpr_sd =tblNguonVonDA.sttDuAnpr_sd))
                                ,tongMucDauTuGiam=(SELECT ISNULL(SUM(giaTriVonQDGiam),0) FROM dbo.tblDieuChinhMucDTuCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttDCMucDTupr_sd IN (SELECT sttDCMucDTupr FROM dbo.tblDieuChinhMucDTu WHERE sttDuAnpr_sd =tblNguonVonDA.sttDuAnpr_sd))
                                ,keHoachVonNam=(SELECT ISNULL(SUM(keHoachvonnam),0) FROM dbo.tblKehoachVonCT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttKHVonpr_sd IN (SELECT sttKHVonpr FROM dbo.tblKehoachVon WHERE loai IN('1','3')))
                                ,keHoachVonTang=(SELECT ISNULL(SUM(keHoachVonTang),0) FROM dbo.tblKehoachVonCT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttKHVonpr_sd IN (SELECT sttKHVonpr FROM dbo.tblKehoachVon WHERE loai='2'))
                                ,keHoachVonGiam=(SELECT ISNULL(SUM(keHoachVonGiam),0) FROM dbo.tblKehoachVonCT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttKHVonpr_sd IN (SELECT sttKHVonpr FROM dbo.tblKehoachVon WHERE loai='2'))
                                ,giaTriTHien=(SELECT ISNULL(SUM(giaTriNghiemThu),0) FROM dbo.tblHopDongCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttHopDongpr_sd IN (SELECT sttHopDongpr FROM dbo.tblHopDong WHERE sttDuAnpr_sd= tblNguonVonDA.sttDuAnpr_sd ) )
                                ,giaTriTHienPL=(SELECT ISNULL(SUM(giaTriNghiemThu),0) FROM dbo.tblPhuLucHDCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttPLHDpr_sd IN (SELECT sttPLHDpr FROM dbo.tblPhuLucHD WHERE sttHopDongpr_sd IN (SELECT sttHopDongpr FROM dbo.tblHopDong WHERE sttDuAnpr_sd = tblNguonVonDA.sttDuAnpr_sd)) )
                                ,giaTriGiaiNgan=(SELECT ISNULL(SUM(soTien),0) FROM dbo.tblChungTuChiCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttCTChipr_sd IN(SELECT sttCTChipr FROM dbo.tblChungTuChi WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND loaiChungTupr_sd IN (N'010',N'011')))
                                ,khoiLuongQToan=(SELECT ISNULL(SUM(giaTriQuyetToanCQTC),0) FROM dbo.tblTapHopCP WHERE LEN(ISNULL(maCPDauTuXDCTpr_sd,''))>0 AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttHSQTpr_sd IN(SELECT sttHSQTpr FROM dbo.tblHoSoQT WHERE ISNULL(duyetHS,0)=1 AND sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd ))
                                FROM (
                                SELECT DISTINCT sttDuAnpr_sd,maNguonVonpr_sd FROM(
                                SELECT sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblNguonVonDA 
                                UNION ALL 
                                SELECT (SELECT sttDuAnpr_sd FROM dbo.tblDieuChinhMucDTu WHERE sttDCMucDTupr=tblDieuChinhMucDTuCT.sttDCMucDTupr_sd) AS sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblDieuChinhMucDTuCT
                                UNION ALL 
                                SELECT (SELECT sttDuAnpr_sd FROM dbo.tblHopDong WHERE sttHopDongpr=tblHopDongCT.sttHopDongpr_sd) AS sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblHopDongCT
                                UNION ALL 
                                SELECT (SELECT sttDuAnpr_sd FROM dbo.tblChungTuChi WHERE sttCTChipr=tblChungTuChiCT.sttCTChipr_sd) AS sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblChungTuChiCT
                                UNION ALL
                                SELECT  ( SELECT (SELECT sttDuAnpr_sd FROM dbo.tblHopDong a WHERE a.sttHopDongpr=b.sttHopDongpr_sd) FROM tblPhuLucHD b WHERE sttPLHDpr=tblPhuLucHDCT.sttPLHDpr_sd ) AS  sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblPhuLucHDCT
                                ) AS tblDMNguon 
                                WHERE sttDuAnpr_sd ='" + sttDA + @"'
                                ) AS tblNguonVonDA
                                ) AS b 
                                ) AS c
                                ) AS d GROUP BY tenNguonVon ORDER BY tongMucDauTu DESC ";
                Grid1.DataSource = sqlFunc.GetData(sql);
                Grid1.DataBind();
            }
            catch
            {
            }
        }
        protected void Grid2_OnRebind(object sender, EventArgs e)
        {
            try
            {
                SqlFunction sqlFunc = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                string sttDA = hdfSTTDA.Value.ToString();
                //                string sql = @"SELECT *,tangGiam=(ISNULL(giaTriQuyetToan,0)-ISNULL(duToanDuocDuyet,0)) FROM ( SELECT noiDung=(SELECT tenCPDauTuXD FROM dbo.tblDMCPDauTuXDCT WHERE maCPDauTuXDCTpr=a.maCPDauTuXDCTpr_sd)
                //                            ,duToanDuocDuyet=ISNULL((SELECT giaTriDuToan FROM dbo.tblCPDauTuXDCT WHERE sttCPDTuXDCTpr=a.sttCPDTuXDCTpr_sd AND sttDuAnpr_sd=N'" + sttDA + @"'),0)
                //                            ,giaTriQuyetToan FROM (
                //                            SELECT maCPDauTuXDCTpr_sd,giaTriQuyetToan,sttCPDTuXDCTpr_sd FROM dbo.tblTapHopCP WHERE LEN(ISNULL(maCPDauTuXDCTpr_sd,''))>0 AND sttHSQTpr_sd IN(SELECT sttHSQTpr FROM dbo.tblHoSoQT WHERE sttDuAnpr_sd=N'" + sttDA + @"')
                //                            ) AS a) AS b";
                string sql = @"SELECT maChiPhiDTXDpr_sd,noiDung,duToanDuocDuyet=(giaTriDuToan+giaTriDuToanTang-giaTriDuToanGiam),giaTriQuyetToan
                            ,tangGiam=(giaTriQuyetToan-(giaTriDuToan+giaTriDuToanTang-giaTriDuToanGiam)) FROM (
                            SELECT maChiPhiDTXDpr_sd,noiDung=(SELECT tenChiPhiDTXD FROM dbo.tblDMCPDauTuXD WHERE maChiPhiDTXDpr=b.maChiPhiDTXDpr_sd)
                            ,giaTriDuToan
                            ,giaTriDuToanTang=ISNULL((SELECT SUM(giaTriDuToanTang) FROM dbo.tblDieuChinhDuToanCT WHERE maChiPhiDTXDpr_sd=b.maChiPhiDTXDpr_sd AND sttDCDuToanpr_sd IN 
                            (SELECT sttDCDuToanpr FROM dbo.tblDieuChinhDuToan WHERE sttDuAnpr_sd='" + sttDA + @"' AND lanDieuChinh=(SELECT MAX(lanDieuChinh) FROM tblDieuChinhDuToan WHERE sttDuAnpr_sd='" + sttDA + @"'))),0)
                            ,giaTriDuToanGiam=ISNULL((SELECT SUM(giaTriDuToanGiam) FROM dbo.tblDieuChinhDuToanCT WHERE maChiPhiDTXDpr_sd=b.maChiPhiDTXDpr_sd AND sttDCDuToanpr_sd IN 
                            (SELECT sttDCDuToanpr FROM dbo.tblDieuChinhDuToan WHERE sttDuAnpr_sd='" + sttDA + @"' AND lanDieuChinh=(SELECT MAX(lanDieuChinh) FROM tblDieuChinhDuToan WHERE sttDuAnpr_sd='" + sttDA + @"'))),0)
                            ,giaTriQuyetToan=ISNULL((SELECT SUM(giaTriNghiemThu) FROM dbo.tblHopDongCT WHERE maChiPhiDTXDpr_sd=b.maChiPhiDTXDpr_sd AND sttHopDongpr_sd IN (SELECT sttHopDongpr FROM dbo.tblHopDong WHERE sttDuAnpr_sd='" + sttDA + @"')),0)
                            +ISNULL((SELECT SUM(giaTriNghiemThu) FROM dbo.tblPhuLucHDCT WHERE maChiPhiDTXDpr_sd=b.maChiPhiDTXDpr_sd AND sttPLHDpr_sd IN(SELECT sttPLHDpr FROM dbo.tblPhuLucHD WHERE sttHopDongpr_sd IN( SELECT sttHopDongpr FROM dbo.tblHopDong WHERE sttHopDongpr_sd IN (SELECT sttHopDongpr FROM dbo.tblHopDong WHERE sttDuAnpr_sd='" + sttDA + @"')))),0) FROM (
                            SELECT maChiPhiDTXDpr_sd,SUM(giaTriDuToan) AS giaTriDuToan FROM (
                            SELECT maChiPhiDTXDpr_sd,giaTriDuToan FROM dbo.tblCPDauTuXDCT WHERE sttDuAnpr_sd='" + sttDA + @"' 
                            ) AS a GROUP BY maChiPhiDTXDpr_sd
                            ) AS b
                            ) AS c";
                Grid2.DataSource = sqlFunc.GetData(sql);
                Grid2.DataBind();
            }
            catch
            {
            }
        }
        protected void Grid3_OnRebind(object sender, EventArgs e)
        {
            try
            {
                SqlFunction sqlFunc = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                SqlFunction sqlFuncUer = new SqlFunction(HttpContext.Current.Session.GetConnectionString1());
                string maDV = HttpContext.Current.Session.GetDonVi().maDonVi;
                string strWhere = "";
                string nienDo = HttpContext.Current.Session.GetNamSudung();
                decimal nienDoSau = Convert.ToDecimal(HttpContext.Current.Session.GetNamSudung()) + 1;
                //string tuNgay = "01/01/" + HttpContext.Current.Session.GetNamSudung();
                //string denNgay = "01/31/" + nienDoSau;
                string tuNgay = Session.GetNgayDauKy();
                string denNgay = Session.GetNgayCuoiKy();
                if (sqlFuncUer.GetOneStringField("SELECT idGrouppr_sd FROM dbo.tblUsers WHERE tenDangNhap=N'" + HttpContext.Current.Session.GetCurrentUser().tenDangNhap + "'") == "userSD")
                {
                    strWhere = " (maDonVipr_chudautu=N'" + maDV + "'  OR maDonVipr_qlda=N'" + maDV + "') AND ";
                }
                else if (sqlFuncUer.GetOneStringField("SELECT idGrouppr_sd FROM dbo.tblUsers WHERE tenDangNhap=N'" + HttpContext.Current.Session.GetCurrentUser().tenDangNhap + "'") == "userTH")
                {
                    strWhere = " maDonVipr_chudautu IN (SELECT maDonVipr FROM dbo.tblDMDonVi WHERE maDonvipr_cha=N'" + maDV + "') AND";
                }
                else
                {
                    strWhere = " maDonVipr_chudautu IN (SELECT maDonVipr FROM dbo.tblDMDonVi WHERE (maDonvipr_cha IN (SELECT maDonvipr_cha FROM dbo.tblDMDonVi WHERE maDonVipr=N'" + maDV + "')) OR (maDonvipr_cha IN ( SELECT maDonVipr FROM dbo.tblDMDonVi WHERE maDonvipr_cha IN( SELECT maDonvipr_cha FROM dbo.tblDMDonVi WHERE maDonVipr=N'" + maDV + "')))) AND ";
                }

                string sql = @"SELECT nguonVon,SUM(slDuAn)slDuAn ,SUM(tongMucDauTu) tongMucDauTu,SUM(keHoachVon) keHoachVon
                                ,SUM(giaTriNghiemThu) giaTriNghiemThu,SUM(khoiLuongGNgan) khoiLuongGNgan,SUM(khoiLuongQToan) khoiLuongQToan,''AS maNguonVonpr_sd,'1' AS sttSort FROM (
                                SELECT nguonVon,slDuAn ,tongMucDauTu,keHoachVon
                                ,giaTriNghiemThu,khoiLuongGNgan,khoiLuongQToan FROM (
                                SELECT N'+ Dự án cấp Trung ương' AS nguonVon
                                ,slDuAn,tongMucDauTu=(giaTriVonQD+tongMucDauTuTang-tongMucDauTuGiam)
                                ,keHoachVon=(keHoachVonNam+keHoachVonTang-keHoachVonGiam),giaTriNghiemThu=(giaTriTHien+giaTriTHienPL)
                                ,khoiLuongGNgan=giaTriGiaiNgan,khoiLuongQToan FROM(
                                SELECT sttDuAnpr_sd,maNguonVonpr_sd,slDuAn=1
                                ,giaTriVonQD=(SELECT ISNULL(SUM(giaTriVonQD),0) FROM tblNguonVonDA d WHERE d.sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND d.maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd)
                                ,tongMucDauTuTang=(SELECT ISNULL(SUM(giaTriVonQDTang),0) FROM dbo.tblDieuChinhMucDTuCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttDCMucDTupr_sd IN (SELECT sttDCMucDTupr FROM dbo.tblDieuChinhMucDTu WHERE ngayLap<='" + denNgay + @"'
                                AND lanDieuChinh=(SELECT MAX(lanDieuChinh) FROM tblDieuChinhMucDTu WHERE ngayLap<='" + denNgay + @"') AND sttDuAnpr_sd =tblNguonVonDA.sttDuAnpr_sd))
                                ,tongMucDauTuGiam=(SELECT ISNULL(SUM(giaTriVonQDGiam),0) FROM dbo.tblDieuChinhMucDTuCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttDCMucDTupr_sd IN (SELECT sttDCMucDTupr FROM dbo.tblDieuChinhMucDTu WHERE ngayLap<='" + denNgay + @"'
                                AND lanDieuChinh=(SELECT MAX(lanDieuChinh) FROM tblDieuChinhMucDTu WHERE ngayLap<='" + denNgay + @"') AND sttDuAnpr_sd =tblNguonVonDA.sttDuAnpr_sd))
                                ,keHoachVonNam=(SELECT ISNULL(SUM(keHoachvonnam),0) FROM dbo.tblKehoachVonCT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttKHVonpr_sd IN (SELECT sttKHVonpr FROM dbo.tblKehoachVon WHERE nienDo='" + nienDo + @"' AND loai IN ('1','3')))
                                ,keHoachVonTang=(SELECT ISNULL(SUM(keHoachVonTang),0) FROM dbo.tblKehoachVonCT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttKHVonpr_sd IN (SELECT sttKHVonpr FROM dbo.tblKehoachVon WHERE nienDo='" + nienDo + @"' AND loai='2'))
                                ,keHoachVonGiam=(SELECT ISNULL(SUM(keHoachVonGiam),0) FROM dbo.tblKehoachVonCT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttKHVonpr_sd IN (SELECT sttKHVonpr FROM dbo.tblKehoachVon WHERE nienDo='" + nienDo + @"' AND loai='2'))
                                ,giaTriTHien=(SELECT ISNULL(SUM(giaTriNghiemThu),0) FROM dbo.tblHopDongCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttHopDongpr_sd IN (SELECT sttHopDongpr FROM dbo.tblHopDong WHERE sttDuAnpr_sd= tblNguonVonDA.sttDuAnpr_sd AND (ngayNghiemThu BETWEEN '" + tuNgay + @"' AND '" + denNgay + @"')) )
                                ,giaTriTHienPL=(SELECT ISNULL(SUM(giaTriNghiemThu),0) FROM dbo.tblPhuLucHDCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttPLHDpr_sd IN (SELECT sttPLHDpr FROM dbo.tblPhuLucHD WHERE sttHopDongpr_sd IN(SELECT sttHopDongpr FROM dbo.tblHopDong WHERE sttDuAnpr_sd = tblNguonVonDA.sttDuAnpr_sd) AND (ngayNghiemThu BETWEEN '" + tuNgay + @"' AND '" + denNgay + @"')) )
                                ,giaTriGiaiNgan=(SELECT ISNULL(SUM(soTien),0) FROM dbo.tblChungTuChiCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttCTChipr_sd IN(SELECT sttCTChipr FROM dbo.tblChungTuChi WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND  (ngayChungTu BETWEEN '" + tuNgay + @"' AND '" + denNgay + @"') AND loaiChungTupr_sd IN (N'010',N'011')))
                                ,khoiLuongQToan=(SELECT ISNULL(SUM(giaTriQuyetToanCQTC),0) FROM dbo.tblTapHopCP WHERE LEN(ISNULL(maCPDauTuXDCTpr_sd,''))>0 AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttHSQTpr_sd IN(SELECT sttHSQTpr FROM dbo.tblHoSoQT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND  ISNULL(duyetHS,0)=1 AND   (ngayHChinhHS BETWEEN '" + tuNgay + @"' AND '" + denNgay + @"')))
                                FROM (
                                SELECT DISTINCT sttDuAnpr_sd,maNguonVonpr_sd FROM(
                                SELECT sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblNguonVonDA 
                                UNION ALL 
                                SELECT (SELECT sttDuAnpr_sd FROM dbo.tblDieuChinhMucDTu WHERE sttDCMucDTupr=tblDieuChinhMucDTuCT.sttDCMucDTupr_sd) AS sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblDieuChinhMucDTuCT
                                UNION ALL 
                                SELECT (SELECT sttDuAnpr_sd FROM dbo.tblHopDong WHERE sttHopDongpr=tblHopDongCT.sttHopDongpr_sd) AS sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblHopDongCT
                                UNION ALL 
                                SELECT (SELECT sttDuAnpr_sd FROM dbo.tblChungTuChi WHERE sttCTChipr=tblChungTuChiCT.sttCTChipr_sd) AS sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblChungTuChiCT
                                UNION ALL
                                SELECT  ( SELECT (SELECT sttDuAnpr_sd FROM dbo.tblHopDong a WHERE a.sttHopDongpr=b.sttHopDongpr_sd) FROM tblPhuLucHD b WHERE sttPLHDpr=tblPhuLucHDCT.sttPLHDpr_sd ) AS  sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblPhuLucHDCT
                                ) AS tblDMNguon
                                WHERE LEN(sttDuAnpr_sd)>0 AND LEN(maNguonVonpr_sd)>0 AND sttDuAnpr_sd IN (
                                SELECT sttDuAnpr FROM dbo.tblDuAn WHERE maCapQLpr_sd=N'01' AND " + strWhere + @" chinhThuc='1')
                                ) AS tblNguonVonDA
                                ) AS b 
                                ) AS c
                                ) AS d GROUP BY nguonVon
                                UNION ALL
                                SELECT nguonVon,SUM(slDuAn)slDuAn ,SUM(tongMucDauTu) tongMucDauTu,SUM(keHoachVon) keHoachVon
                                ,SUM(giaTriNghiemThu) giaTriNghiemThu,SUM(khoiLuongGNgan) khoiLuongGNgan,SUM(khoiLuongQToan) khoiLuongQToan,maNguonVonpr_sd,'1' AS sttSort FROM (
                                SELECT nguonVon,slDuAn ,tongMucDauTu,keHoachVon
                                ,giaTriNghiemThu,khoiLuongGNgan,khoiLuongQToan,maNguonVonpr_sd FROM (
                                SELECT (SELECT tenNguonVon FROM dbo.tblDMNguonVon WHERE maNguonVonpr=b.maNguonVonpr_sd) AS nguonVon
                                ,slDuAn,tongMucDauTu=(giaTriVonQD+tongMucDauTuTang-tongMucDauTuGiam)
                                ,keHoachVon=(keHoachVonNam+keHoachVonTang-keHoachVonGiam),giaTriNghiemThu=(giaTriTHien+giaTriTHienPL)
                                ,khoiLuongGNgan=giaTriGiaiNgan,khoiLuongQToan,maNguonVonpr_sd FROM(
                                SELECT sttDuAnpr_sd,maNguonVonpr_sd,slDuAn=1
                                ,giaTriVonQD=(SELECT ISNULL(SUM(giaTriVonQD),0) FROM tblNguonVonDA d WHERE d.sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND d.maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd)
                                ,tongMucDauTuTang=(SELECT ISNULL(SUM(giaTriVonQDTang),0) FROM dbo.tblDieuChinhMucDTuCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttDCMucDTupr_sd IN (SELECT sttDCMucDTupr FROM dbo.tblDieuChinhMucDTu WHERE ngayLap<='" + denNgay + @"'
                                AND lanDieuChinh=(SELECT MAX(lanDieuChinh) FROM tblDieuChinhMucDTu WHERE ngayLap<='" + denNgay + @"') AND sttDuAnpr_sd =tblNguonVonDA.sttDuAnpr_sd))
                                ,tongMucDauTuGiam=(SELECT ISNULL(SUM(giaTriVonQDGiam),0) FROM dbo.tblDieuChinhMucDTuCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttDCMucDTupr_sd IN (SELECT sttDCMucDTupr FROM dbo.tblDieuChinhMucDTu WHERE ngayLap<='" + denNgay + @"'
                                AND lanDieuChinh=(SELECT MAX(lanDieuChinh) FROM tblDieuChinhMucDTu WHERE ngayLap<='" + denNgay + @"') AND sttDuAnpr_sd =tblNguonVonDA.sttDuAnpr_sd))
                                ,keHoachVonNam=(SELECT ISNULL(SUM(keHoachvonnam),0) FROM dbo.tblKehoachVonCT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttKHVonpr_sd IN (SELECT sttKHVonpr FROM dbo.tblKehoachVon WHERE nienDo='" + nienDo + @"' AND loai IN ('1','3')))
                                ,keHoachVonTang=(SELECT ISNULL(SUM(keHoachVonTang),0) FROM dbo.tblKehoachVonCT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttKHVonpr_sd IN (SELECT sttKHVonpr FROM dbo.tblKehoachVon WHERE nienDo='" + nienDo + @"' AND loai='2'))
                                ,keHoachVonGiam=(SELECT ISNULL(SUM(keHoachVonGiam),0) FROM dbo.tblKehoachVonCT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttKHVonpr_sd IN (SELECT sttKHVonpr FROM dbo.tblKehoachVon WHERE nienDo='" + nienDo + @"' AND loai='2'))
                                ,giaTriTHien=(SELECT ISNULL(SUM(giaTriNghiemThu),0) FROM dbo.tblHopDongCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttHopDongpr_sd IN (SELECT sttHopDongpr FROM dbo.tblHopDong WHERE sttDuAnpr_sd= tblNguonVonDA.sttDuAnpr_sd AND (ngayNghiemThu BETWEEN '" + tuNgay + @"' AND '" + denNgay + @"')) )
                                ,giaTriTHienPL=(SELECT ISNULL(SUM(giaTriNghiemThu),0) FROM dbo.tblPhuLucHDCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttPLHDpr_sd IN (SELECT sttPLHDpr FROM dbo.tblPhuLucHD WHERE sttHopDongpr_sd IN(SELECT sttHopDongpr FROM dbo.tblHopDong WHERE sttDuAnpr_sd = tblNguonVonDA.sttDuAnpr_sd) AND (ngayNghiemThu BETWEEN '" + tuNgay + @"' AND '" + denNgay + @"')) )
                                ,giaTriGiaiNgan=(SELECT ISNULL(SUM(soTien),0) FROM dbo.tblChungTuChiCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttCTChipr_sd IN(SELECT sttCTChipr FROM dbo.tblChungTuChi WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd   AND (ngayChungTu BETWEEN '" + tuNgay + @"' AND '" + denNgay + @"') AND loaiChungTupr_sd IN (N'010',N'011')))
                                ,khoiLuongQToan=(SELECT ISNULL(SUM(giaTriQuyetToanCQTC),0) FROM dbo.tblTapHopCP WHERE LEN(ISNULL(maCPDauTuXDCTpr_sd,''))>0 AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttHSQTpr_sd IN(SELECT sttHSQTpr FROM dbo.tblHoSoQT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd  AND  ISNULL(duyetHS,0)=1 AND  (ngayHChinhHS BETWEEN '" + tuNgay + @"' AND '" + denNgay + @"')))
                                FROM (
                                SELECT DISTINCT sttDuAnpr_sd,maNguonVonpr_sd FROM(
                                SELECT sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblNguonVonDA 
                                UNION ALL 
                                SELECT (SELECT sttDuAnpr_sd FROM dbo.tblDieuChinhMucDTu WHERE sttDCMucDTupr=tblDieuChinhMucDTuCT.sttDCMucDTupr_sd) AS sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblDieuChinhMucDTuCT
                                UNION ALL 
                                SELECT (SELECT sttDuAnpr_sd FROM dbo.tblHopDong WHERE sttHopDongpr=tblHopDongCT.sttHopDongpr_sd) AS sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblHopDongCT
                                UNION ALL 
                                SELECT (SELECT sttDuAnpr_sd FROM dbo.tblChungTuChi WHERE sttCTChipr=tblChungTuChiCT.sttCTChipr_sd) AS sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblChungTuChiCT
                                UNION ALL
                                SELECT  ( SELECT (SELECT sttDuAnpr_sd FROM dbo.tblHopDong a WHERE a.sttHopDongpr=b.sttHopDongpr_sd) FROM tblPhuLucHD b WHERE sttPLHDpr=tblPhuLucHDCT.sttPLHDpr_sd ) AS  sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblPhuLucHDCT
                                ) AS tblDMNguon 
                                WHERE LEN(sttDuAnpr_sd)>0 AND LEN(maNguonVonpr_sd)>0 AND sttDuAnpr_sd IN (
                                SELECT sttDuAnpr FROM dbo.tblDuAn WHERE maCapQLpr_sd=N'01' AND " + strWhere + @" chinhThuc='1')
                                ) AS tblNguonVonDA
                                ) AS b 
                                ) AS c
                                ) AS d GROUP BY nguonVon,maNguonVonpr_sd 
                                UNION ALL
                                SELECT nguonVon,SUM(slDuAn)slDuAn ,SUM(tongMucDauTu) tongMucDauTu,SUM(keHoachVon) keHoachVon
                                ,SUM(giaTriNghiemThu) giaTriNghiemThu,SUM(khoiLuongGNgan) khoiLuongGNgan,SUM(khoiLuongQToan) khoiLuongQToan,''AS maNguonVonpr_sd,'2' AS sttSort FROM (
                                SELECT nguonVon,slDuAn ,tongMucDauTu,keHoachVon
                                ,giaTriNghiemThu,khoiLuongGNgan,khoiLuongQToan FROM (
                                SELECT N'+ Dự án cấp Tỉnh' AS nguonVon
                                ,slDuAn,tongMucDauTu=(giaTriVonQD+tongMucDauTuTang-tongMucDauTuGiam)
                                ,keHoachVon=(keHoachVonNam+keHoachVonTang-keHoachVonGiam),giaTriNghiemThu=(giaTriTHien+giaTriTHienPL)
                                ,khoiLuongGNgan=giaTriGiaiNgan,khoiLuongQToan FROM(
                                SELECT sttDuAnpr_sd,maNguonVonpr_sd,slDuAn=1
                                ,giaTriVonQD=(SELECT ISNULL(SUM(giaTriVonQD),0) FROM tblNguonVonDA d WHERE d.sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND d.maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd)
                                ,tongMucDauTuTang=(SELECT ISNULL(SUM(giaTriVonQDTang),0) FROM dbo.tblDieuChinhMucDTuCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttDCMucDTupr_sd IN (SELECT sttDCMucDTupr FROM dbo.tblDieuChinhMucDTu WHERE ngayLap<='" + denNgay + @"'
                                AND lanDieuChinh=(SELECT MAX(lanDieuChinh) FROM tblDieuChinhMucDTu WHERE ngayLap<='" + denNgay + @"') AND sttDuAnpr_sd =tblNguonVonDA.sttDuAnpr_sd))
                                ,tongMucDauTuGiam=(SELECT ISNULL(SUM(giaTriVonQDGiam),0) FROM dbo.tblDieuChinhMucDTuCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttDCMucDTupr_sd IN (SELECT sttDCMucDTupr FROM dbo.tblDieuChinhMucDTu WHERE ngayLap<='" + denNgay + @"'
                                AND lanDieuChinh=(SELECT MAX(lanDieuChinh) FROM tblDieuChinhMucDTu WHERE ngayLap<='" + denNgay + @"') AND sttDuAnpr_sd =tblNguonVonDA.sttDuAnpr_sd))
                                ,keHoachVonNam=(SELECT ISNULL(SUM(keHoachvonnam),0) FROM dbo.tblKehoachVonCT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttKHVonpr_sd IN (SELECT sttKHVonpr FROM dbo.tblKehoachVon WHERE nienDo='" + nienDo + @"' AND loai IN ('1','3')))
                                ,keHoachVonTang=(SELECT ISNULL(SUM(keHoachVonTang),0) FROM dbo.tblKehoachVonCT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttKHVonpr_sd IN (SELECT sttKHVonpr FROM dbo.tblKehoachVon WHERE nienDo='" + nienDo + @"' AND loai='2'))
                                ,keHoachVonGiam=(SELECT ISNULL(SUM(keHoachVonGiam),0) FROM dbo.tblKehoachVonCT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttKHVonpr_sd IN (SELECT sttKHVonpr FROM dbo.tblKehoachVon WHERE nienDo='" + nienDo + @"' AND loai='2'))
                                ,giaTriTHien=(SELECT ISNULL(SUM(giaTriNghiemThu),0) FROM dbo.tblHopDongCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttHopDongpr_sd IN (SELECT sttHopDongpr FROM dbo.tblHopDong WHERE sttDuAnpr_sd= tblNguonVonDA.sttDuAnpr_sd AND (ngayNghiemThu BETWEEN '" + tuNgay + @"' AND '" + denNgay + @"')) )
                                ,giaTriTHienPL=(SELECT ISNULL(SUM(giaTriNghiemThu),0) FROM dbo.tblPhuLucHDCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttPLHDpr_sd IN (SELECT sttPLHDpr FROM dbo.tblPhuLucHD WHERE sttHopDongpr_sd IN(SELECT sttHopDongpr FROM dbo.tblHopDong WHERE sttDuAnpr_sd = tblNguonVonDA.sttDuAnpr_sd) AND (ngayNghiemThu BETWEEN '" + tuNgay + @"' AND '" + denNgay + @"')) )
                                ,giaTriGiaiNgan=(SELECT ISNULL(SUM(soTien),0) FROM dbo.tblChungTuChiCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttCTChipr_sd IN(SELECT sttCTChipr FROM dbo.tblChungTuChi WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND  (ngayChungTu BETWEEN '" + tuNgay + @"' AND '" + denNgay + @"') AND loaiChungTupr_sd IN (N'010',N'011')))
                                ,khoiLuongQToan=(SELECT ISNULL(SUM(giaTriQuyetToanCQTC),0) FROM dbo.tblTapHopCP WHERE LEN(ISNULL(maCPDauTuXDCTpr_sd,''))>0 AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttHSQTpr_sd IN(SELECT sttHSQTpr FROM dbo.tblHoSoQT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND  ISNULL(duyetHS,0)=1 AND   (ngayHChinhHS BETWEEN '" + tuNgay + @"' AND '" + denNgay + @"')))
                                FROM (
                                SELECT DISTINCT sttDuAnpr_sd,maNguonVonpr_sd FROM(
                                SELECT sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblNguonVonDA 
                                UNION ALL 
                                SELECT (SELECT sttDuAnpr_sd FROM dbo.tblDieuChinhMucDTu WHERE sttDCMucDTupr=tblDieuChinhMucDTuCT.sttDCMucDTupr_sd) AS sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblDieuChinhMucDTuCT
                                UNION ALL 
                                SELECT (SELECT sttDuAnpr_sd FROM dbo.tblHopDong WHERE sttHopDongpr=tblHopDongCT.sttHopDongpr_sd) AS sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblHopDongCT
                                UNION ALL 
                                SELECT (SELECT sttDuAnpr_sd FROM dbo.tblChungTuChi WHERE sttCTChipr=tblChungTuChiCT.sttCTChipr_sd) AS sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblChungTuChiCT
                                UNION ALL
                                SELECT  ( SELECT (SELECT sttDuAnpr_sd FROM dbo.tblHopDong a WHERE a.sttHopDongpr=b.sttHopDongpr_sd) FROM tblPhuLucHD b WHERE sttPLHDpr=tblPhuLucHDCT.sttPLHDpr_sd ) AS  sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblPhuLucHDCT
                                ) AS tblDMNguon
                                WHERE LEN(sttDuAnpr_sd)>0 AND LEN(maNguonVonpr_sd)>0 AND sttDuAnpr_sd IN (
                                SELECT sttDuAnpr FROM dbo.tblDuAn WHERE maCapQLpr_sd=N'02' AND " + strWhere + @" chinhThuc='1')
                                ) AS tblNguonVonDA
                                ) AS b 
                                ) AS c
                                ) AS d GROUP BY nguonVon
                                UNION ALL
                                SELECT nguonVon,SUM(slDuAn)slDuAn ,SUM(tongMucDauTu) tongMucDauTu,SUM(keHoachVon) keHoachVon
                                ,SUM(giaTriNghiemThu) giaTriNghiemThu,SUM(khoiLuongGNgan) khoiLuongGNgan,SUM(khoiLuongQToan) khoiLuongQToan,maNguonVonpr_sd,'2' AS sttSort FROM (
                                SELECT nguonVon,slDuAn ,tongMucDauTu,keHoachVon
                                ,giaTriNghiemThu,khoiLuongGNgan,khoiLuongQToan,maNguonVonpr_sd FROM (
                                SELECT (SELECT tenNguonVon FROM dbo.tblDMNguonVon WHERE maNguonVonpr=b.maNguonVonpr_sd) AS nguonVon
                                ,slDuAn,tongMucDauTu=(giaTriVonQD+tongMucDauTuTang-tongMucDauTuGiam)
                                ,keHoachVon=(keHoachVonNam+keHoachVonTang-keHoachVonGiam),giaTriNghiemThu=(giaTriTHien+giaTriTHienPL)
                                ,khoiLuongGNgan=giaTriGiaiNgan,khoiLuongQToan,maNguonVonpr_sd FROM(
                                SELECT sttDuAnpr_sd,maNguonVonpr_sd,slDuAn=1
                                ,giaTriVonQD=(SELECT ISNULL(SUM(giaTriVonQD),0) FROM tblNguonVonDA d WHERE d.sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND d.maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd)
                                ,tongMucDauTuTang=(SELECT ISNULL(SUM(giaTriVonQDTang),0) FROM dbo.tblDieuChinhMucDTuCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttDCMucDTupr_sd IN (SELECT sttDCMucDTupr FROM dbo.tblDieuChinhMucDTu WHERE ngayLap<='" + denNgay + @"'
                                AND lanDieuChinh=(SELECT MAX(lanDieuChinh) FROM tblDieuChinhMucDTu WHERE ngayLap<='" + denNgay + @"') AND sttDuAnpr_sd =tblNguonVonDA.sttDuAnpr_sd))
                                ,tongMucDauTuGiam=(SELECT ISNULL(SUM(giaTriVonQDGiam),0) FROM dbo.tblDieuChinhMucDTuCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttDCMucDTupr_sd IN (SELECT sttDCMucDTupr FROM dbo.tblDieuChinhMucDTu WHERE ngayLap<='" + denNgay + @"'
                                AND lanDieuChinh=(SELECT MAX(lanDieuChinh) FROM tblDieuChinhMucDTu WHERE ngayLap<='" + denNgay + @"') AND sttDuAnpr_sd =tblNguonVonDA.sttDuAnpr_sd))
                                ,keHoachVonNam=(SELECT ISNULL(SUM(keHoachvonnam),0) FROM dbo.tblKehoachVonCT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttKHVonpr_sd IN (SELECT sttKHVonpr FROM dbo.tblKehoachVon WHERE nienDo='" + nienDo + @"' AND loai IN ('1','3')))
                                ,keHoachVonTang=(SELECT ISNULL(SUM(keHoachVonTang),0) FROM dbo.tblKehoachVonCT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttKHVonpr_sd IN (SELECT sttKHVonpr FROM dbo.tblKehoachVon WHERE nienDo='" + nienDo + @"' AND loai='2'))
                                ,keHoachVonGiam=(SELECT ISNULL(SUM(keHoachVonGiam),0) FROM dbo.tblKehoachVonCT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttKHVonpr_sd IN (SELECT sttKHVonpr FROM dbo.tblKehoachVon WHERE nienDo='" + nienDo + @"' AND loai='2'))
                                ,giaTriTHien=(SELECT ISNULL(SUM(giaTriNghiemThu),0) FROM dbo.tblHopDongCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttHopDongpr_sd IN (SELECT sttHopDongpr FROM dbo.tblHopDong WHERE sttDuAnpr_sd= tblNguonVonDA.sttDuAnpr_sd AND (ngayNghiemThu BETWEEN '" + tuNgay + @"' AND '" + denNgay + @"')) )
                                ,giaTriTHienPL=(SELECT ISNULL(SUM(giaTriNghiemThu),0) FROM dbo.tblPhuLucHDCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttPLHDpr_sd IN (SELECT sttPLHDpr FROM dbo.tblPhuLucHD WHERE sttHopDongpr_sd IN(SELECT sttHopDongpr FROM dbo.tblHopDong WHERE sttDuAnpr_sd = tblNguonVonDA.sttDuAnpr_sd) AND (ngayNghiemThu BETWEEN '" + tuNgay + @"' AND '" + denNgay + @"')) )
                                ,giaTriGiaiNgan=(SELECT ISNULL(SUM(soTien),0) FROM dbo.tblChungTuChiCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttCTChipr_sd IN(SELECT sttCTChipr FROM dbo.tblChungTuChi WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd  AND (ngayChungTu BETWEEN '" + tuNgay + @"' AND '" + denNgay + @"') AND loaiChungTupr_sd IN (N'010',N'011')))
                                ,khoiLuongQToan=(SELECT ISNULL(SUM(giaTriQuyetToanCQTC),0) FROM dbo.tblTapHopCP WHERE LEN(ISNULL(maCPDauTuXDCTpr_sd,''))>0 AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttHSQTpr_sd IN(SELECT sttHSQTpr FROM dbo.tblHoSoQT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND  ISNULL(duyetHS,0)=1 AND  (ngayHChinhHS BETWEEN '" + tuNgay + @"' AND '" + denNgay + @"')))
                                FROM (
                                SELECT DISTINCT sttDuAnpr_sd,maNguonVonpr_sd FROM(
                                SELECT sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblNguonVonDA 
                                UNION ALL 
                                SELECT (SELECT sttDuAnpr_sd FROM dbo.tblDieuChinhMucDTu WHERE sttDCMucDTupr=tblDieuChinhMucDTuCT.sttDCMucDTupr_sd) AS sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblDieuChinhMucDTuCT
                                UNION ALL 
                                SELECT (SELECT sttDuAnpr_sd FROM dbo.tblHopDong WHERE sttHopDongpr=tblHopDongCT.sttHopDongpr_sd) AS sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblHopDongCT
                                UNION ALL 
                                SELECT (SELECT sttDuAnpr_sd FROM dbo.tblChungTuChi WHERE sttCTChipr=tblChungTuChiCT.sttCTChipr_sd) AS sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblChungTuChiCT
                                UNION ALL
                                SELECT  ( SELECT (SELECT sttDuAnpr_sd FROM dbo.tblHopDong a WHERE a.sttHopDongpr=b.sttHopDongpr_sd) FROM tblPhuLucHD b WHERE sttPLHDpr=tblPhuLucHDCT.sttPLHDpr_sd ) AS  sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblPhuLucHDCT
                                ) AS tblDMNguon
WHERE LEN(sttDuAnpr_sd)>0 AND LEN(maNguonVonpr_sd)>0 AND sttDuAnpr_sd IN (
                                SELECT sttDuAnpr FROM dbo.tblDuAn WHERE maCapQLpr_sd=N'02' AND " + strWhere + @" chinhThuc='1')
                                ) AS tblNguonVonDA
                                ) AS b 
                                ) AS c
                                ) AS d GROUP BY nguonVon,maNguonVonpr_sd
UNION ALL
SELECT nguonVon,SUM(slDuAn)slDuAn ,SUM(tongMucDauTu) tongMucDauTu,SUM(keHoachVon) keHoachVon
                                ,SUM(giaTriNghiemThu) giaTriNghiemThu,SUM(khoiLuongGNgan) khoiLuongGNgan,SUM(khoiLuongQToan) khoiLuongQToan,''AS maNguonVonpr_sd,'3' AS sttSort FROM (
                                SELECT nguonVon,slDuAn ,tongMucDauTu,keHoachVon
                                ,giaTriNghiemThu,khoiLuongGNgan,khoiLuongQToan FROM (
                                SELECT N'+ Dự án cấp Huyện' AS nguonVon
                                ,slDuAn,tongMucDauTu=(giaTriVonQD+tongMucDauTuTang-tongMucDauTuGiam)
                                ,keHoachVon=(keHoachVonNam+keHoachVonTang-keHoachVonGiam),giaTriNghiemThu=(giaTriTHien+giaTriTHienPL)
                                ,khoiLuongGNgan=giaTriGiaiNgan,khoiLuongQToan FROM(
                                SELECT sttDuAnpr_sd,maNguonVonpr_sd,slDuAn=1
                                ,giaTriVonQD=(SELECT ISNULL(SUM(giaTriVonQD),0) FROM tblNguonVonDA d WHERE d.sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND d.maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd)
                                ,tongMucDauTuTang=(SELECT ISNULL(SUM(giaTriVonQDTang),0) FROM dbo.tblDieuChinhMucDTuCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttDCMucDTupr_sd IN (SELECT sttDCMucDTupr FROM dbo.tblDieuChinhMucDTu WHERE ngayLap<='" + denNgay + @"'
                                AND lanDieuChinh=(SELECT MAX(lanDieuChinh) FROM tblDieuChinhMucDTu WHERE ngayLap<='" + denNgay + @"') AND sttDuAnpr_sd =tblNguonVonDA.sttDuAnpr_sd))
                                ,tongMucDauTuGiam=(SELECT ISNULL(SUM(giaTriVonQDGiam),0) FROM dbo.tblDieuChinhMucDTuCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttDCMucDTupr_sd IN (SELECT sttDCMucDTupr FROM dbo.tblDieuChinhMucDTu WHERE ngayLap<='" + denNgay + @"'
                                AND lanDieuChinh=(SELECT MAX(lanDieuChinh) FROM tblDieuChinhMucDTu WHERE ngayLap<='" + denNgay + @"') AND sttDuAnpr_sd =tblNguonVonDA.sttDuAnpr_sd))
                                ,keHoachVonNam=(SELECT ISNULL(SUM(keHoachvonnam),0) FROM dbo.tblKehoachVonCT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttKHVonpr_sd IN (SELECT sttKHVonpr FROM dbo.tblKehoachVon WHERE nienDo='" + nienDo + @"' AND loai IN ('1','3')))
                                ,keHoachVonTang=(SELECT ISNULL(SUM(keHoachVonTang),0) FROM dbo.tblKehoachVonCT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttKHVonpr_sd IN (SELECT sttKHVonpr FROM dbo.tblKehoachVon WHERE nienDo='" + nienDo + @"' AND loai='2'))
                                ,keHoachVonGiam=(SELECT ISNULL(SUM(keHoachVonGiam),0) FROM dbo.tblKehoachVonCT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttKHVonpr_sd IN (SELECT sttKHVonpr FROM dbo.tblKehoachVon WHERE nienDo='" + nienDo + @"' AND loai='2'))
                                ,giaTriTHien=(SELECT ISNULL(SUM(giaTriNghiemThu),0) FROM dbo.tblHopDongCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttHopDongpr_sd IN (SELECT sttHopDongpr FROM dbo.tblHopDong WHERE sttDuAnpr_sd= tblNguonVonDA.sttDuAnpr_sd AND (ngayNghiemThu BETWEEN '" + tuNgay + @"' AND '" + denNgay + @"')) )
                                ,giaTriTHienPL=(SELECT ISNULL(SUM(giaTriNghiemThu),0) FROM dbo.tblPhuLucHDCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttPLHDpr_sd IN (SELECT sttPLHDpr FROM dbo.tblPhuLucHD WHERE sttHopDongpr_sd IN(SELECT sttHopDongpr FROM dbo.tblHopDong WHERE sttDuAnpr_sd = tblNguonVonDA.sttDuAnpr_sd) AND (ngayNghiemThu BETWEEN '" + tuNgay + @"' AND '" + denNgay + @"')) )
                                ,giaTriGiaiNgan=(SELECT ISNULL(SUM(soTien),0) FROM dbo.tblChungTuChiCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttCTChipr_sd IN(SELECT sttCTChipr FROM dbo.tblChungTuChi WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND  (ngayChungTu BETWEEN '" + tuNgay + @"' AND '" + denNgay + @"') AND loaiChungTupr_sd IN (N'010',N'011')))
                                ,khoiLuongQToan=(SELECT ISNULL(SUM(giaTriQuyetToanCQTC),0) FROM dbo.tblTapHopCP WHERE LEN(ISNULL(maCPDauTuXDCTpr_sd,''))>0 AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttHSQTpr_sd IN(SELECT sttHSQTpr FROM dbo.tblHoSoQT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND  ISNULL(duyetHS,0)=1 AND   (ngayHChinhHS BETWEEN '" + tuNgay + @"' AND '" + denNgay + @"')))
                                FROM (
                                SELECT DISTINCT sttDuAnpr_sd,maNguonVonpr_sd FROM(
                                SELECT sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblNguonVonDA 
                                UNION ALL 
                                SELECT (SELECT sttDuAnpr_sd FROM dbo.tblDieuChinhMucDTu WHERE sttDCMucDTupr=tblDieuChinhMucDTuCT.sttDCMucDTupr_sd) AS sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblDieuChinhMucDTuCT
                                UNION ALL 
                                SELECT (SELECT sttDuAnpr_sd FROM dbo.tblHopDong WHERE sttHopDongpr=tblHopDongCT.sttHopDongpr_sd) AS sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblHopDongCT
                                UNION ALL 
                                SELECT (SELECT sttDuAnpr_sd FROM dbo.tblChungTuChi WHERE sttCTChipr=tblChungTuChiCT.sttCTChipr_sd) AS sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblChungTuChiCT
                                UNION ALL
                                SELECT  ( SELECT (SELECT sttDuAnpr_sd FROM dbo.tblHopDong a WHERE a.sttHopDongpr=b.sttHopDongpr_sd) FROM tblPhuLucHD b WHERE sttPLHDpr=tblPhuLucHDCT.sttPLHDpr_sd ) AS  sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblPhuLucHDCT
                                ) AS tblDMNguon 
WHERE LEN(sttDuAnpr_sd)>0 AND LEN(maNguonVonpr_sd)>0 AND sttDuAnpr_sd IN (
                                SELECT sttDuAnpr FROM dbo.tblDuAn WHERE maCapQLpr_sd=N'03' AND " + strWhere + @" chinhThuc='1')
                                ) AS tblNguonVonDA
                                ) AS b 
                                ) AS c
                                ) AS d GROUP BY nguonVon
UNION ALL
SELECT nguonVon,SUM(slDuAn)slDuAn ,SUM(tongMucDauTu) tongMucDauTu,SUM(keHoachVon) keHoachVon
                                ,SUM(giaTriNghiemThu) giaTriNghiemThu,SUM(khoiLuongGNgan) khoiLuongGNgan,SUM(khoiLuongQToan) khoiLuongQToan,maNguonVonpr_sd,'3' AS sttSort FROM (
                                SELECT nguonVon,slDuAn ,tongMucDauTu,keHoachVon
                                ,giaTriNghiemThu,khoiLuongGNgan,khoiLuongQToan,maNguonVonpr_sd FROM (
                                SELECT (SELECT tenNguonVon FROM dbo.tblDMNguonVon WHERE maNguonVonpr=b.maNguonVonpr_sd) AS nguonVon
                                ,slDuAn,tongMucDauTu=(giaTriVonQD+tongMucDauTuTang-tongMucDauTuGiam)
                                ,keHoachVon=(keHoachVonNam+keHoachVonTang-keHoachVonGiam),giaTriNghiemThu=(giaTriTHien+giaTriTHienPL)
                                ,khoiLuongGNgan=giaTriGiaiNgan,khoiLuongQToan,maNguonVonpr_sd FROM(
                                SELECT sttDuAnpr_sd,maNguonVonpr_sd,slDuAn=1
                                ,giaTriVonQD=(SELECT ISNULL(SUM(giaTriVonQD),0) FROM tblNguonVonDA d WHERE d.sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND d.maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd)
                                ,tongMucDauTuTang=(SELECT ISNULL(SUM(giaTriVonQDTang),0) FROM dbo.tblDieuChinhMucDTuCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttDCMucDTupr_sd IN (SELECT sttDCMucDTupr FROM dbo.tblDieuChinhMucDTu WHERE ngayLap<='" + denNgay + @"'
                                AND lanDieuChinh=(SELECT MAX(lanDieuChinh) FROM tblDieuChinhMucDTu WHERE ngayLap<='" + denNgay + @"') AND sttDuAnpr_sd =tblNguonVonDA.sttDuAnpr_sd))
                                ,tongMucDauTuGiam=(SELECT ISNULL(SUM(giaTriVonQDGiam),0) FROM dbo.tblDieuChinhMucDTuCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttDCMucDTupr_sd IN (SELECT sttDCMucDTupr FROM dbo.tblDieuChinhMucDTu WHERE ngayLap<='" + denNgay + @"'
                                AND lanDieuChinh=(SELECT MAX(lanDieuChinh) FROM tblDieuChinhMucDTu WHERE ngayLap<='" + denNgay + @"') AND sttDuAnpr_sd =tblNguonVonDA.sttDuAnpr_sd))
                                ,keHoachVonNam=(SELECT ISNULL(SUM(keHoachvonnam),0) FROM dbo.tblKehoachVonCT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttKHVonpr_sd IN (SELECT sttKHVonpr FROM dbo.tblKehoachVon WHERE nienDo='" + nienDo + @"' AND loai IN ('1','3')))
                                ,keHoachVonTang=(SELECT ISNULL(SUM(keHoachVonTang),0) FROM dbo.tblKehoachVonCT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttKHVonpr_sd IN (SELECT sttKHVonpr FROM dbo.tblKehoachVon WHERE nienDo='" + nienDo + @"' AND loai='2'))
                                ,keHoachVonGiam=(SELECT ISNULL(SUM(keHoachVonGiam),0) FROM dbo.tblKehoachVonCT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttKHVonpr_sd IN (SELECT sttKHVonpr FROM dbo.tblKehoachVon WHERE nienDo='" + nienDo + @"' AND loai='2'))
                                ,giaTriTHien=(SELECT ISNULL(SUM(giaTriNghiemThu),0) FROM dbo.tblHopDongCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttHopDongpr_sd IN (SELECT sttHopDongpr FROM dbo.tblHopDong WHERE sttDuAnpr_sd= tblNguonVonDA.sttDuAnpr_sd AND (ngayNghiemThu BETWEEN '" + tuNgay + @"' AND '" + denNgay + @"')) )
                                ,giaTriTHienPL=(SELECT ISNULL(SUM(giaTriNghiemThu),0) FROM dbo.tblPhuLucHDCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttPLHDpr_sd IN (SELECT sttPLHDpr FROM dbo.tblPhuLucHD WHERE sttHopDongpr_sd IN(SELECT sttHopDongpr FROM dbo.tblHopDong WHERE sttDuAnpr_sd = tblNguonVonDA.sttDuAnpr_sd) AND (ngayNghiemThu BETWEEN '" + tuNgay + @"' AND '" + denNgay + @"')) )
                                ,giaTriGiaiNgan=(SELECT ISNULL(SUM(soTien),0) FROM dbo.tblChungTuChiCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttCTChipr_sd IN(SELECT sttCTChipr FROM dbo.tblChungTuChi WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND  (ngayChungTu BETWEEN '" + tuNgay + @"' AND '" + denNgay + @"') AND loaiChungTupr_sd IN (N'010',N'011')))
                                ,khoiLuongQToan=(SELECT ISNULL(SUM(giaTriQuyetToanCQTC),0) FROM dbo.tblTapHopCP WHERE LEN(ISNULL(maCPDauTuXDCTpr_sd,''))>0 AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttHSQTpr_sd IN(SELECT sttHSQTpr FROM dbo.tblHoSoQT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND  ISNULL(duyetHS,0)=1 AND  (ngayHChinhHS BETWEEN '" + tuNgay + @"' AND '" + denNgay + @"')))
                                FROM (
                                SELECT DISTINCT sttDuAnpr_sd,maNguonVonpr_sd FROM(
                                SELECT sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblNguonVonDA 
                                UNION ALL 
                                SELECT (SELECT sttDuAnpr_sd FROM dbo.tblDieuChinhMucDTu WHERE sttDCMucDTupr=tblDieuChinhMucDTuCT.sttDCMucDTupr_sd) AS sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblDieuChinhMucDTuCT
                                UNION ALL 
                                SELECT (SELECT sttDuAnpr_sd FROM dbo.tblHopDong WHERE sttHopDongpr=tblHopDongCT.sttHopDongpr_sd) AS sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblHopDongCT
                                UNION ALL 
                                SELECT (SELECT sttDuAnpr_sd FROM dbo.tblChungTuChi WHERE sttCTChipr=tblChungTuChiCT.sttCTChipr_sd) AS sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblChungTuChiCT
                                UNION ALL
                                SELECT  ( SELECT (SELECT sttDuAnpr_sd FROM dbo.tblHopDong a WHERE a.sttHopDongpr=b.sttHopDongpr_sd) FROM tblPhuLucHD b WHERE sttPLHDpr=tblPhuLucHDCT.sttPLHDpr_sd ) AS  sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblPhuLucHDCT
                                ) AS tblDMNguon
WHERE LEN(sttDuAnpr_sd)>0 AND LEN(maNguonVonpr_sd)>0 AND sttDuAnpr_sd IN (
                                SELECT sttDuAnpr FROM dbo.tblDuAn WHERE maCapQLpr_sd=N'03' AND " + strWhere + @" chinhThuc='1')
                                ) AS tblNguonVonDA
                                ) AS b 
                                ) AS c
                                ) AS d GROUP BY nguonVon,maNguonVonpr_sd
UNION ALL
SELECT nguonVon,SUM(slDuAn)slDuAn ,SUM(tongMucDauTu) tongMucDauTu,SUM(keHoachVon) keHoachVon
                                ,SUM(giaTriNghiemThu) giaTriNghiemThu,SUM(khoiLuongGNgan) khoiLuongGNgan,SUM(khoiLuongQToan) khoiLuongQToan,''AS maNguonVonpr_sd,'4' AS sttSort FROM (
                                SELECT nguonVon,slDuAn ,tongMucDauTu,keHoachVon
                                ,giaTriNghiemThu,khoiLuongGNgan,khoiLuongQToan FROM (
                                SELECT N'+ Dự án cấp Xã' AS nguonVon
                                ,slDuAn,tongMucDauTu=(giaTriVonQD+tongMucDauTuTang-tongMucDauTuGiam)
                                ,keHoachVon=(keHoachVonNam+keHoachVonTang-keHoachVonGiam),giaTriNghiemThu=(giaTriTHien+giaTriTHienPL)
                                ,khoiLuongGNgan=giaTriGiaiNgan,khoiLuongQToan FROM(
                                SELECT sttDuAnpr_sd,maNguonVonpr_sd,slDuAn=1
                                ,giaTriVonQD=(SELECT ISNULL(SUM(giaTriVonQD),0) FROM tblNguonVonDA d WHERE d.sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND d.maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd)
                                ,tongMucDauTuTang=(SELECT ISNULL(SUM(giaTriVonQDTang),0) FROM dbo.tblDieuChinhMucDTuCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttDCMucDTupr_sd IN (SELECT sttDCMucDTupr FROM dbo.tblDieuChinhMucDTu WHERE ngayLap<='" + denNgay + @"'
                                AND lanDieuChinh=(SELECT MAX(lanDieuChinh) FROM tblDieuChinhMucDTu WHERE ngayLap<='" + denNgay + @"') AND sttDuAnpr_sd =tblNguonVonDA.sttDuAnpr_sd))
                                ,tongMucDauTuGiam=(SELECT ISNULL(SUM(giaTriVonQDGiam),0) FROM dbo.tblDieuChinhMucDTuCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttDCMucDTupr_sd IN (SELECT sttDCMucDTupr FROM dbo.tblDieuChinhMucDTu WHERE ngayLap<='" + denNgay + @"'
                                AND lanDieuChinh=(SELECT MAX(lanDieuChinh) FROM tblDieuChinhMucDTu WHERE ngayLap<='" + denNgay + @"') AND sttDuAnpr_sd =tblNguonVonDA.sttDuAnpr_sd))
                                ,keHoachVonNam=(SELECT ISNULL(SUM(keHoachvonnam),0) FROM dbo.tblKehoachVonCT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttKHVonpr_sd IN (SELECT sttKHVonpr FROM dbo.tblKehoachVon WHERE nienDo='" + nienDo + @"' AND loai IN ('1','3')))
                                ,keHoachVonTang=(SELECT ISNULL(SUM(keHoachVonTang),0) FROM dbo.tblKehoachVonCT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttKHVonpr_sd IN (SELECT sttKHVonpr FROM dbo.tblKehoachVon WHERE nienDo='" + nienDo + @"' AND loai='2'))
                                ,keHoachVonGiam=(SELECT ISNULL(SUM(keHoachVonGiam),0) FROM dbo.tblKehoachVonCT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttKHVonpr_sd IN (SELECT sttKHVonpr FROM dbo.tblKehoachVon WHERE nienDo='" + nienDo + @"' AND loai='2'))
                                ,giaTriTHien=(SELECT ISNULL(SUM(giaTriNghiemThu),0) FROM dbo.tblHopDongCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttHopDongpr_sd IN (SELECT sttHopDongpr FROM dbo.tblHopDong WHERE sttDuAnpr_sd= tblNguonVonDA.sttDuAnpr_sd AND (ngayNghiemThu BETWEEN '" + tuNgay + @"' AND '" + denNgay + @"')) )
                                ,giaTriTHienPL=(SELECT ISNULL(SUM(giaTriNghiemThu),0) FROM dbo.tblPhuLucHDCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttPLHDpr_sd IN (SELECT sttPLHDpr FROM dbo.tblPhuLucHD WHERE sttHopDongpr_sd IN(SELECT sttHopDongpr FROM dbo.tblHopDong WHERE sttDuAnpr_sd = tblNguonVonDA.sttDuAnpr_sd) AND (ngayNghiemThu BETWEEN '" + tuNgay + @"' AND '" + denNgay + @"')) )
                                ,giaTriGiaiNgan=(SELECT ISNULL(SUM(soTien),0) FROM dbo.tblChungTuChiCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttCTChipr_sd IN(SELECT sttCTChipr FROM dbo.tblChungTuChi WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND  (ngayChungTu BETWEEN '" + tuNgay + @"' AND '" + denNgay + @"') AND loaiChungTupr_sd IN (N'010',N'011')))
                                ,khoiLuongQToan=(SELECT ISNULL(SUM(giaTriQuyetToanCQTC),0) FROM dbo.tblTapHopCP WHERE LEN(ISNULL(maCPDauTuXDCTpr_sd,''))>0 AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttHSQTpr_sd IN(SELECT sttHSQTpr FROM dbo.tblHoSoQT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND  ISNULL(duyetHS,0)=1 AND  (ngayHChinhHS BETWEEN '" + tuNgay + @"' AND '" + denNgay + @"')))
                                FROM (
                                SELECT DISTINCT sttDuAnpr_sd,maNguonVonpr_sd FROM(
                                SELECT sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblNguonVonDA 
                                UNION ALL 
                                SELECT (SELECT sttDuAnpr_sd FROM dbo.tblDieuChinhMucDTu WHERE sttDCMucDTupr=tblDieuChinhMucDTuCT.sttDCMucDTupr_sd) AS sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblDieuChinhMucDTuCT
                                UNION ALL 
                                SELECT (SELECT sttDuAnpr_sd FROM dbo.tblHopDong WHERE sttHopDongpr=tblHopDongCT.sttHopDongpr_sd) AS sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblHopDongCT
                                UNION ALL 
                                SELECT (SELECT sttDuAnpr_sd FROM dbo.tblChungTuChi WHERE sttCTChipr=tblChungTuChiCT.sttCTChipr_sd) AS sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblChungTuChiCT
                                UNION ALL
                                SELECT  ( SELECT (SELECT sttDuAnpr_sd FROM dbo.tblHopDong a WHERE a.sttHopDongpr=b.sttHopDongpr_sd) FROM tblPhuLucHD b WHERE sttPLHDpr=tblPhuLucHDCT.sttPLHDpr_sd ) AS  sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblPhuLucHDCT
                                ) AS tblDMNguon 
WHERE LEN(sttDuAnpr_sd)>0 AND LEN(maNguonVonpr_sd)>0 AND sttDuAnpr_sd IN (
                                SELECT sttDuAnpr FROM dbo.tblDuAn WHERE maCapQLpr_sd=N'04' AND " + strWhere + @" chinhThuc='1')
                                ) AS tblNguonVonDA
                                ) AS b 
                                ) AS c
                                ) AS d GROUP BY nguonVon
UNION ALL
SELECT nguonVon,SUM(slDuAn)slDuAn ,SUM(tongMucDauTu) tongMucDauTu,SUM(keHoachVon) keHoachVon
                                ,SUM(giaTriNghiemThu) giaTriNghiemThu,SUM(khoiLuongGNgan) khoiLuongGNgan,SUM(khoiLuongQToan) khoiLuongQToan,maNguonVonpr_sd,'4' AS sttSort FROM (
                                SELECT nguonVon,slDuAn ,tongMucDauTu,keHoachVon
                                ,giaTriNghiemThu,khoiLuongGNgan,khoiLuongQToan,maNguonVonpr_sd FROM (
                                SELECT (SELECT tenNguonVon FROM dbo.tblDMNguonVon WHERE maNguonVonpr=b.maNguonVonpr_sd) AS nguonVon
                                ,slDuAn,tongMucDauTu=(giaTriVonQD+tongMucDauTuTang-tongMucDauTuGiam)
                                ,keHoachVon=(keHoachVonNam+keHoachVonTang-keHoachVonGiam),giaTriNghiemThu=(giaTriTHien+giaTriTHienPL)
                                ,khoiLuongGNgan=giaTriGiaiNgan,khoiLuongQToan,maNguonVonpr_sd FROM(
                                SELECT sttDuAnpr_sd,maNguonVonpr_sd,slDuAn=1
                                ,giaTriVonQD=(SELECT ISNULL(SUM(giaTriVonQD),0) FROM tblNguonVonDA d WHERE d.sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND d.maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd)
                                ,tongMucDauTuTang=(SELECT ISNULL(SUM(giaTriVonQDTang),0) FROM dbo.tblDieuChinhMucDTuCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttDCMucDTupr_sd IN (SELECT sttDCMucDTupr FROM dbo.tblDieuChinhMucDTu WHERE ngayLap<='" + denNgay + @"'
                                AND lanDieuChinh=(SELECT MAX(lanDieuChinh) FROM tblDieuChinhMucDTu WHERE ngayLap<='" + denNgay + @"') AND sttDuAnpr_sd =tblNguonVonDA.sttDuAnpr_sd))
                                ,tongMucDauTuGiam=(SELECT ISNULL(SUM(giaTriVonQDGiam),0) FROM dbo.tblDieuChinhMucDTuCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttDCMucDTupr_sd IN (SELECT sttDCMucDTupr FROM dbo.tblDieuChinhMucDTu WHERE ngayLap<='" + denNgay + @"'
                                AND lanDieuChinh=(SELECT MAX(lanDieuChinh) FROM tblDieuChinhMucDTu WHERE ngayLap<='" + denNgay + @"') AND sttDuAnpr_sd =tblNguonVonDA.sttDuAnpr_sd))
                                ,keHoachVonNam=(SELECT ISNULL(SUM(keHoachvonnam),0) FROM dbo.tblKehoachVonCT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttKHVonpr_sd IN (SELECT sttKHVonpr FROM dbo.tblKehoachVon WHERE nienDo='" + nienDo + @"' AND loai IN ('1','3')))
                                ,keHoachVonTang=(SELECT ISNULL(SUM(keHoachVonTang),0) FROM dbo.tblKehoachVonCT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttKHVonpr_sd IN (SELECT sttKHVonpr FROM dbo.tblKehoachVon WHERE nienDo='" + nienDo + @"' AND loai='2'))
                                ,keHoachVonGiam=(SELECT ISNULL(SUM(keHoachVonGiam),0) FROM dbo.tblKehoachVonCT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttKHVonpr_sd IN (SELECT sttKHVonpr FROM dbo.tblKehoachVon WHERE nienDo='" + nienDo + @"' AND loai='2'))
                                ,giaTriTHien=(SELECT ISNULL(SUM(giaTriNghiemThu),0) FROM dbo.tblHopDongCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttHopDongpr_sd IN (SELECT sttHopDongpr FROM dbo.tblHopDong WHERE sttDuAnpr_sd= tblNguonVonDA.sttDuAnpr_sd AND (ngayNghiemThu BETWEEN '" + tuNgay + @"' AND '" + denNgay + @"')) )
                                ,giaTriTHienPL=(SELECT ISNULL(SUM(giaTriNghiemThu),0) FROM dbo.tblPhuLucHDCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttPLHDpr_sd IN (SELECT sttPLHDpr FROM dbo.tblPhuLucHD WHERE sttHopDongpr_sd IN(SELECT sttHopDongpr FROM dbo.tblHopDong WHERE sttDuAnpr_sd = tblNguonVonDA.sttDuAnpr_sd) AND (ngayNghiemThu BETWEEN '" + tuNgay + @"' AND '" + denNgay + @"')) )
                                ,giaTriGiaiNgan=(SELECT ISNULL(SUM(soTien),0) FROM dbo.tblChungTuChiCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttCTChipr_sd IN(SELECT sttCTChipr FROM dbo.tblChungTuChi WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND  (ngayChungTu BETWEEN '" + tuNgay + @"' AND '" + denNgay + @"') AND loaiChungTupr_sd IN (N'010',N'011')))
                                ,khoiLuongQToan=(SELECT ISNULL(SUM(giaTriQuyetToanCQTC),0) FROM dbo.tblTapHopCP WHERE LEN(ISNULL(maCPDauTuXDCTpr_sd,''))>0 AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttHSQTpr_sd IN(SELECT sttHSQTpr FROM dbo.tblHoSoQT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND  ISNULL(duyetHS,0)=1 AND (ngayHChinhHS BETWEEN '" + tuNgay + @"' AND '" + denNgay + @"')))
                                FROM (
                                SELECT DISTINCT sttDuAnpr_sd,maNguonVonpr_sd FROM(
                                SELECT sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblNguonVonDA 
                                UNION ALL 
                                SELECT (SELECT sttDuAnpr_sd FROM dbo.tblDieuChinhMucDTu WHERE sttDCMucDTupr=tblDieuChinhMucDTuCT.sttDCMucDTupr_sd) AS sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblDieuChinhMucDTuCT
                                UNION ALL 
                                SELECT (SELECT sttDuAnpr_sd FROM dbo.tblHopDong WHERE sttHopDongpr=tblHopDongCT.sttHopDongpr_sd) AS sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblHopDongCT
                                UNION ALL 
                                SELECT (SELECT sttDuAnpr_sd FROM dbo.tblChungTuChi WHERE sttCTChipr=tblChungTuChiCT.sttCTChipr_sd) AS sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblChungTuChiCT
                                UNION ALL
                                SELECT  ( SELECT (SELECT sttDuAnpr_sd FROM dbo.tblHopDong a WHERE a.sttHopDongpr=b.sttHopDongpr_sd) FROM tblPhuLucHD b WHERE sttPLHDpr=tblPhuLucHDCT.sttPLHDpr_sd ) AS  sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblPhuLucHDCT
                                ) AS tblDMNguon 
WHERE LEN(sttDuAnpr_sd)>0 AND LEN(maNguonVonpr_sd)>0 AND sttDuAnpr_sd IN (
                                SELECT sttDuAnpr FROM dbo.tblDuAn WHERE maCapQLpr_sd=N'04' AND " + strWhere + @" chinhThuc='1')
                                ) AS tblNguonVonDA
                                ) AS b 
                                ) AS c
                                ) AS d GROUP BY nguonVon,maNguonVonpr_sd
                                ORDER BY sttSort,maNguonVonpr_sd ";
                // Grid3.DataSource = sqlFunc.GetData("SELECT nguonVon='', slDuAn='0',tongMucDauTu='0',keHoachVon='0',khoiLuongTHien='0',khoiLuongGNgan='0',khoiLuongQToan='0'  FROM dbo.tblCPDauTuXDCT");
                Grid3.DataSource = sqlFunc.GetData(sql);
                Grid3.DataBind();
            }
            catch
            {
            }
        }
        double tongMucDauTu = 0;
        double keHoachVon = 0;
        double khoiLuongTHien = 0;
        double khoiLuongGNgan = 0;
        double khoiLuongQToan = 0;
        protected void Grid1_OnRowDataBound(object sender, GridRowEventArgs e)
        {
            if (e.Row.RowType == GridRowType.DataRow)
            {
                if (e.Row.Cells[1].Text != "")
                {
                    tongMucDauTu += double.Parse(e.Row.Cells[1].Text);
                }
                if (e.Row.Cells[2].Text != "")
                {
                    keHoachVon += double.Parse(e.Row.Cells[2].Text);
                }
                if (e.Row.Cells[3].Text != "")
                {
                    khoiLuongTHien += double.Parse(e.Row.Cells[3].Text);
                }
                if (e.Row.Cells[4].Text != "")
                {
                    khoiLuongGNgan += double.Parse(e.Row.Cells[4].Text);
                }
                if (e.Row.Cells[5].Text != "")
                {
                    khoiLuongQToan += double.Parse(e.Row.Cells[5].Text);
                }
            }
            else if (e.Row.RowType == GridRowType.ColumnFooter)
            {
                e.Row.Cells[0].Text = "Tổng cộng";
                e.Row.Cells[0].Style["font-weight"] = "bold";
                e.Row.Cells[1].Text = tongMucDauTu.ToString("N0");
                e.Row.Cells[1].Style["font-weight"] = "bold";
                e.Row.Cells[2].Text = keHoachVon.ToString("N0");
                e.Row.Cells[2].Style["font-weight"] = "bold";
                e.Row.Cells[3].Text = khoiLuongTHien.ToString("N0");
                e.Row.Cells[3].Style["font-weight"] = "bold";
                e.Row.Cells[4].Text = khoiLuongGNgan.ToString("N0");
                e.Row.Cells[4].Style["font-weight"] = "bold";
                e.Row.Cells[5].Text = khoiLuongQToan.ToString("N0");
                e.Row.Cells[5].Style["font-weight"] = "bold";
                //e.Row.Cells[3].Text = unitsInStock.ToString();
                //e.Row.Cells[4].Text = unitsOnOrder.ToString();
            }

        }
        double duToanDuocDuyet = 0;
        double deNghiQToan = 0;
        double tangGiamVoiDToan = 0;

        protected void Grid2_OnRowDataBound(object sender, GridRowEventArgs e)
        {
            if (e.Row.RowType == GridRowType.DataRow)
            {
                if (e.Row.Cells[1].Text != "")
                {
                    duToanDuocDuyet += double.Parse(e.Row.Cells[1].Text);
                }
                if (e.Row.Cells[2].Text != "")
                {
                    deNghiQToan += double.Parse(e.Row.Cells[2].Text);
                }
                if (e.Row.Cells[3].Text != "")
                {
                    tangGiamVoiDToan += double.Parse(e.Row.Cells[3].Text);
                }
            }
            else if (e.Row.RowType == GridRowType.ColumnFooter)
            {
                e.Row.Cells[0].Text = "Tổng cộng";
                e.Row.Cells[0].Style["font-weight"] = "bold";
                e.Row.Cells[1].Text = duToanDuocDuyet.ToString("N0");
                e.Row.Cells[1].Style["font-weight"] = "bold";
                e.Row.Cells[2].Text = deNghiQToan.ToString("N0");
                e.Row.Cells[2].Style["font-weight"] = "bold";
                e.Row.Cells[3].Text = tangGiamVoiDToan.ToString("#,##0;(#,##0);0");
                e.Row.Cells[3].Style["font-weight"] = "bold";
            }

        }
        static double cot02 = 0;
        static double cot03 = 0;
        static double cot04 = 0;
        static double cot05 = 0;
        static double cot06 = 0;
        static double cot07 = 0;
        protected void Grid3_OnRowDataBound(object sender, GridRowEventArgs e)
        {
            if (e.Row.Cells[Grid3.Columns["nguonVon"].Index].Text == "+ Dự án cấp Trung ương" || e.Row.Cells[Grid3.Columns["nguonVon"].Index].Text == "+ Dự án cấp Tỉnh" || e.Row.Cells[Grid3.Columns["nguonVon"].Index].Text == "+ Dự án cấp Huyện" || e.Row.Cells[Grid3.Columns["nguonVon"].Index].Text == "+ Dự án cấp Xã")
            {
                e.Row.Cells[0].Style["font-weight"] = "bold";
                e.Row.Cells[1].Style["font-weight"] = "bold";
                e.Row.Cells[2].Style["font-weight"] = "bold";
                e.Row.Cells[3].Style["font-weight"] = "bold";
                e.Row.Cells[4].Style["font-weight"] = "bold";
                e.Row.Cells[5].Style["font-weight"] = "bold";
                e.Row.Cells[6].Style["font-weight"] = "bold";
            }
            //if (e.Row.RowType == GridRowType.DataRow)
            //{
            //    //if (e.Row.Cells[1].Text != "" && e.Row.Cells[0].Text != "+ Dự án cấp Trung ương" && e.Row.Cells[0].Text != "+ Dự án cấp Tỉnh" && e.Row.Cells[0].Text != "+ Dự án cấp Huyện" && e.Row.Cells[0].Text != "+ Dự án cấp Xã")
            //    //{
            //    //    cot02 += double.Parse(e.Row.Cells[1].Text);
            //    //}
            //    if (e.Row.Cells[2].Text != "" && e.Row.Cells[0].Text != "+ Dự án cấp Trung ương" && e.Row.Cells[0].Text != "+ Dự án cấp Tỉnh" && e.Row.Cells[0].Text != "+ Dự án cấp Huyện" && e.Row.Cells[0].Text != "+ Dự án cấp Xã")
            //    {
            //        cot03 += double.Parse(e.Row.Cells[2].Text);
            //    }
            //    if (e.Row.Cells[3].Text != "" && e.Row.Cells[0].Text != "+ Dự án cấp Trung ương" && e.Row.Cells[0].Text != "+ Dự án cấp Tỉnh" && e.Row.Cells[0].Text != "+ Dự án cấp Huyện" && e.Row.Cells[0].Text != "+ Dự án cấp Xã")
            //    {
            //        cot04 += double.Parse(e.Row.Cells[3].Text);
            //    }
            //    if (e.Row.Cells[3].Text != "" && e.Row.Cells[0].Text != "+ Dự án cấp Trung ương" && e.Row.Cells[0].Text != "+ Dự án cấp Tỉnh" && e.Row.Cells[0].Text != "+ Dự án cấp Huyện" && e.Row.Cells[0].Text != "+ Dự án cấp Xã")
            //    {
            //        cot05 += double.Parse(e.Row.Cells[4].Text);
            //    }
            //    if (e.Row.Cells[3].Text != "" && e.Row.Cells[0].Text != "+ Dự án cấp Trung ương" && e.Row.Cells[0].Text != "+ Dự án cấp Tỉnh" && e.Row.Cells[0].Text != "+ Dự án cấp Huyện" && e.Row.Cells[0].Text != "+ Dự án cấp Xã")
            //    {
            //        cot06 += double.Parse(e.Row.Cells[5].Text);
            //    }
            //    if (e.Row.Cells[3].Text != "" && e.Row.Cells[0].Text != "+ Dự án cấp Trung ương" && e.Row.Cells[0].Text != "+ Dự án cấp Tỉnh" && e.Row.Cells[0].Text != "+ Dự án cấp Huyện" && e.Row.Cells[0].Text != "+ Dự án cấp Xã")
            //    {
            //        cot07 += double.Parse(e.Row.Cells[6].Text);
            //    }
            //}
            //else 
            if (e.Row.RowType == GridRowType.ColumnFooter)
            {
                if (Grid3.DataSource != null)
                {
                    if (Grid3.DataSource.GetType().ToString() == "System.Data.DataTable")
                    {
                        if (((DataTable)Grid3.DataSource).Rows.Count > 0)
                        {
                            cot02 = double.Parse(((DataTable)Grid3.DataSource).Compute("SUM(slDuAn)", "nguonVon<>'+ Dự án cấp Trung ương' AND nguonVon<>'+ Dự án cấp Tỉnh' AND nguonVon<>'+ Dự án cấp Huyện' AND nguonVon<>'+ Dự án cấp Xã'").ToString());
                            cot03 = double.Parse(((DataTable)Grid3.DataSource).Compute("SUM(tongMucDauTu)", "nguonVon<>'+ Dự án cấp Trung ương' AND nguonVon<>'+ Dự án cấp Tỉnh' AND nguonVon<>'+ Dự án cấp Huyện' AND nguonVon<>'+ Dự án cấp Xã'").ToString());
                            cot04 = double.Parse(((DataTable)Grid3.DataSource).Compute("SUM(keHoachVon)", "nguonVon<>'+ Dự án cấp Trung ương' AND nguonVon<>'+ Dự án cấp Tỉnh' AND nguonVon<>'+ Dự án cấp Huyện' AND nguonVon<>'+ Dự án cấp Xã'").ToString());
                            cot05 = double.Parse(((DataTable)Grid3.DataSource).Compute("SUM(giaTriNghiemThu)", "nguonVon<>'+ Dự án cấp Trung ương' AND nguonVon<>'+ Dự án cấp Tỉnh' AND nguonVon<>'+ Dự án cấp Huyện' AND nguonVon<>'+ Dự án cấp Xã'").ToString());
                            cot06 = double.Parse(((DataTable)Grid3.DataSource).Compute("SUM(khoiLuongGNgan)", "nguonVon<>'+ Dự án cấp Trung ương' AND nguonVon<>'+ Dự án cấp Tỉnh' AND nguonVon<>'+ Dự án cấp Huyện' AND nguonVon<>'+ Dự án cấp Xã'").ToString());
                            cot07 = double.Parse(((DataTable)Grid3.DataSource).Compute("SUM(khoiLuongQToan)", "nguonVon<>'+ Dự án cấp Trung ương' AND nguonVon<>'+ Dự án cấp Tỉnh' AND nguonVon<>'+ Dự án cấp Huyện' AND nguonVon<>'+ Dự án cấp Xã'").ToString());
                        }
                    }
                }
                else
                {
                    cot02 = 0;
                    cot03 = 0;
                    cot04 = 0;
                    cot05 = 0;
                    cot06 = 0;
                    cot07 = 0;
                }
                e.Row.Cells[0].Text = "Tổng cộng";
                e.Row.Cells[0].Style["font-weight"] = "bold";
                e.Row.Cells[1].Text = cot02.ToString("N0");
                e.Row.Cells[1].Style["font-weight"] = "bold";
                e.Row.Cells[2].Text = cot03.ToString("N0");
                e.Row.Cells[2].Style["font-weight"] = "bold";
                e.Row.Cells[3].Text = cot04.ToString("N0");
                e.Row.Cells[3].Style["font-weight"] = "bold";
                e.Row.Cells[4].Text = cot05.ToString("N0");
                e.Row.Cells[4].Style["font-weight"] = "bold";
                e.Row.Cells[5].Text = cot06.ToString("N0");
                e.Row.Cells[5].Style["font-weight"] = "bold";
                e.Row.Cells[6].Text = cot07.ToString("N0");
                e.Row.Cells[6].Style["font-weight"] = "bold";
            }
        }
        [AjaxPro.AjaxMethod]
        public bool luuCauHinh(AjaxPro.JavaScriptArray param)
        {

            try
            {
                SqlFunction sqlFunc = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                SqlConnection sqlCon = new SqlConnection(HttpContext.Current.Session.GetConnectionString2());
                sqlCon.Open();
                if (sqlFunc.CheckHasRecord("SELECT maDonVipr_sd FROM dbo.tblCauHinhTimKiem WHERE maDonVipr_sd=N'" + HttpContext.Current.Session.GetDonVi().maDonVi + "'") == true)
                {
                    SqlCommand cmd = new SqlCommand("UPDATE dbo.tblCauHinhTimKiem SET maDuAn=@maDuAn ,tenDuAn=@tenDuAn ,nguonVon=@nguonVon ,loaiKhoan=@loaiKhoan ,loaiCongTrinh=@loaiCongTrinh ,khoiCongTu=@khoiCongTu ,khoiCongDen=@khoiCongDen"
                        + ",hoanThanhTu=@hoanThanhTu ,hoanThanhDen=@hoanThanhDen ,maLoaiDApr_sd=@maLoaiDApr_sd ,maLoaiHinhpr_sd=@maLoaiHinhpr_sd,maNguonVonpr_sd=@maNguonVonpr_sd WHERE maDonVipr_sd=@maDonVipr_sd", sqlCon);
                    cmd.Parameters.Add(new SqlParameter("@maDuAn", param[0].ToString()));
                    cmd.Parameters.Add(new SqlParameter("@tenDuAn", param[1].ToString()));
                    cmd.Parameters.Add(new SqlParameter("@nguonVon", param[2].ToString()));
                    cmd.Parameters.Add(new SqlParameter("@loaiKhoan", param[3].ToString()));
                    cmd.Parameters.Add(new SqlParameter("@loaiCongTrinh", param[4].ToString()));
                    if (param[5].ToString().Trim().Length > 0)
                    {
                        cmd.Parameters.Add(new SqlParameter("@khoiCongTu", _mChuyenChuoiSangNgay(param[5].ToString())));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@khoiCongTu", DBNull.Value));
                    }
                    if (param[6].ToString().Trim().Length > 0)
                    {
                        cmd.Parameters.Add(new SqlParameter("@khoiCongDen", _mChuyenChuoiSangNgay(param[6].ToString())));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@khoiCongDen", DBNull.Value));
                    }
                    if (param[7].ToString().Trim().Length > 0)
                    {
                        cmd.Parameters.Add(new SqlParameter("@hoanThanhTu", _mChuyenChuoiSangNgay(param[7].ToString())));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@hoanThanhTu", DBNull.Value));
                    }
                    if (param[8].ToString().Trim().Length > 0)
                    {
                        cmd.Parameters.Add(new SqlParameter("@hoanThanhDen", _mChuyenChuoiSangNgay(param[8].ToString())));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@hoanThanhDen", DBNull.Value));
                    }
                    cmd.Parameters.Add(new SqlParameter("@maLoaiDApr_sd", param[9].ToString()));
                    cmd.Parameters.Add(new SqlParameter("@maLoaiHinhpr_sd", param[10].ToString()));
                    cmd.Parameters.Add(new SqlParameter("@maDonVipr_sd", HttpContext.Current.Session.GetDonVi().maDonVi));
                    cmd.Parameters.Add(new SqlParameter("@maNguonVonpr_sd", param[11].ToString()));
                    cmd.ExecuteNonQuery();
                    sqlCon.Close();
                    cmd.Dispose();
                    return true;
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO dbo.tblCauHinhTimKiem(maDuAn ,tenDuAn ,nguonVon ,loaiKhoan ,loaiCongTrinh ,khoiCongTu ,khoiCongDen ,hoanThanhTu ,hoanThanhDen ,maLoaiDApr_sd ,maLoaiHinhpr_sd ,maDonVipr_sd ,nguoiThaoTac ,ngayThaotac,maNguonVonpr_sd)"
                        + " VALUES(@maDuAn ,@tenDuAn ,@nguonVon ,@loaiKhoan ,@loaiCongTrinh ,@khoiCongTu ,@khoiCongDen ,@hoanThanhTu ,@hoanThanhDen ,@maLoaiDApr_sd ,@maLoaiHinhpr_sd,@maDonVipr_sd ,@nguoiThaoTac ,@ngayThaotac,@maNguonVonpr_sd)", sqlCon);
                    cmd.Parameters.Add(new SqlParameter("@maDuAn", param[0].ToString()));
                    cmd.Parameters.Add(new SqlParameter("@tenDuAn", param[1].ToString()));
                    cmd.Parameters.Add(new SqlParameter("@nguonVon", param[2].ToString()));
                    cmd.Parameters.Add(new SqlParameter("@loaiKhoan", param[3].ToString()));
                    cmd.Parameters.Add(new SqlParameter("@loaiCongTrinh", param[4].ToString()));
                    if (param[5].ToString().Trim().Length > 0)
                    {
                        cmd.Parameters.Add(new SqlParameter("@khoiCongTu", _mChuyenChuoiSangNgay(param[5].ToString())));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@khoiCongTu", DBNull.Value));
                    }
                    if (param[6].ToString().Trim().Length > 0)
                    {
                        cmd.Parameters.Add(new SqlParameter("@khoiCongDen", _mChuyenChuoiSangNgay(param[6].ToString())));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@khoiCongDen", DBNull.Value));
                    }
                    if (param[7].ToString().Trim().Length > 0)
                    {
                        cmd.Parameters.Add(new SqlParameter("@hoanThanhTu", _mChuyenChuoiSangNgay(param[7].ToString())));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@hoanThanhTu", DBNull.Value));
                    }
                    if (param[8].ToString().Trim().Length > 0)
                    {
                        cmd.Parameters.Add(new SqlParameter("@hoanThanhDen", _mChuyenChuoiSangNgay(param[8].ToString())));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@hoanThanhDen", DBNull.Value));
                    }
                    cmd.Parameters.Add(new SqlParameter("@maLoaiDApr_sd", param[9].ToString()));
                    cmd.Parameters.Add(new SqlParameter("@maLoaiHinhpr_sd", param[10].ToString()));
                    cmd.Parameters.Add(new SqlParameter("@maNguonVonpr_sd", param[11].ToString()));
                    cmd.Parameters.Add(new SqlParameter("@maDonVipr_sd", HttpContext.Current.Session.GetDonVi().maDonVi));
                    cmd.Parameters.Add(new SqlParameter("@nguoiThaoTac", HttpContext.Current.Session.GetCurrentUserID()));
                    cmd.Parameters.Add(new SqlParameter("@ngayThaotac", HttpContext.Current.Session.GetCurrentDatetimeMMddyyyy("MM/dd/yyyy")));
                    cmd.ExecuteNonQuery();
                    sqlCon.Close();
                    cmd.Dispose();
                    return true;
                }
            }
            catch
            {
                return false;
            }


        }
        private string _mChuyenChuoiSangNgay(string ddMMyyyy)
        {
            return ddMMyyyy.Substring(3, 2) + "/" + ddMMyyyy.Substring(0, 2) + "/" + ddMMyyyy.Substring(6, 4);
        }
        [AjaxPro.AjaxMethod]
        public DataTable dtCauHinh()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlFunction sqlFunc = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                dt = sqlFunc.GetData("SELECT maDuAn ,tenDuAn ,nguonVon ,loaiKhoan ,loaiCongTrinh ,CONVERT(VARCHAR(10),khoiCongTu,103) AS khoiCongTu ,CONVERT(VARCHAR(10),khoiCongDen,103) AS khoiCongDen ,CONVERT(VARCHAR(10),hoanThanhTu,103) AS hoanThanhTu ,CONVERT(VARCHAR(10),hoanThanhDen,103) AS hoanThanhDen ,maLoaiDApr_sd ,maLoaiHinhpr_sd,maNguonVonpr_sd FROM dbo.tblCauHinhTimKiem WHERE maDonVipr_sd=N'" + HttpContext.Current.Session.GetDonVi().maDonVi + "'");
                return dt;
            }
            catch
            {

            }
            return dt;
        }
        _LOfficeExcel _vLOfficeExcel = new _LOfficeExcel();
        protected void linkXuatExcel_Click(object sender, EventArgs e)
        {
            try
            {

                int _vDongXuat = 17;
                string _vNoiDungXuat = "";
                string url = "~/xuatexcel" + "/" + HttpContext.Current.Session.GetCurrentUserID() + "/";
                string fileName = "tonghopquyettoan" + HttpContext.Current.Session.GetDonVi().maDonVi + "-" + (DateTime.Now.ToString("ddMMyyyyHHmmss")) + ".xlsx";
               // string chkHienChiPhiCon = chkHienChiPhiCon.va;
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
                
                File.Copy(Server.MapPath("~/ExcelMau/tonghopquyettoan_mau.xlsx"), Server.MapPath("~/xuatexcel" + "/" + HttpContext.Current.Session.GetCurrentUserID() + "/" + fileName), true);

                var wb = new XLWorkbook(Server.MapPath("~/xuatexcel" + "/" + HttpContext.Current.Session.GetCurrentUserID() + "/" + fileName));
                var ws = wb.Worksheet("Sheet1");

                SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                DataTable dt = new DataTable();//= sqlFun.GetData("SELECT tenDA=(SELECT tenDuAn FROM dbo.tblDuAn WHERE sttDuAnpr=sttDuAnpr_sd),tenHMCT=(SELECT tenHangMuc FROM dbo.tblHangMucCTrinh WHERE sttHMCTrinhpr=sttHMCTrinhpr_sd) ,chuDauTu=(SELECT tenDonVi FROM tblDMDonVi WHERE maDonVipr=(SELECT maDonVipr_chudautu FROM dbo.tblDuAn WHERE sttDuAnpr=tblHoSoQT.sttDuAnpr_sd)) FROM tblHoSoQT WHERE sttHSQTpr='" + hdfsttHSQTpr.Value.ToString() + "'");
                dt = sqlFun.GetData("SELECT sttDuAnpr,maDuAn,tenDuAn,tenChuDauTu=(SELECT tenDonVi FROM tblDMDonVi WHERE maDonVipr=maDonVipr_chudautu),diaDiemXD,thoiGianKCong=ISNULL(CONVERT(VARCHAR(10),thoiGianKCongDuyet,103),''),thoiGianHThanh=ISNULL(CONVERT(VARCHAR(10),thoiGianHThanhDuyet,103),''),thoiGianKCongTH=ISNULL(CONVERT(VARCHAR(10),(SELECT TOP 1 tblBanGiaoDA.thoiGianKCongTH FROM dbo.tblBanGiaoDA WHERE sttDuAnpr_sd = sttDuAnpr),103),''),thoiGianHThanhTH=ISNULL(CONVERT(VARCHAR(10),(SELECT TOP 1 tblBanGiaoDA.thoiGianHThanhTH FROM dbo.tblBanGiaoDA WHERE sttDuAnpr_sd = sttDuAnpr),103),'') FROM dbo.tblDuAn WHERE sttDuAnpr=N'" + hdfSTTDA.Value.ToString() + "'");
                if (dt.Rows.Count > 0)
                {
                    ws.Cell(2, "A").Value = "Mã dự án: " + dt.Rows[0]["maDuAn"].ToString();
                    ws.Cell(3, "A").Value = "Tên dự án: " + dt.Rows[0]["tenDuAn"].ToString();
                    ws.Cell(4, "A").Value = "Chủ đầu tư: " + dt.Rows[0]["tenChuDauTu"].ToString();
                    ws.Cell(5, "A").Value = "Ngày khởi công: " + dt.Rows[0]["thoiGianKCong"].ToString() + ". Ngày hoàn thành: " + dt.Rows[0]["thoiGianHThanh"].ToString() + "- Ngày khởi công thực tế: " + dt.Rows[0]["thoiGianKCongTH"].ToString() + ". Ngày hoàn thành thực tế: " + dt.Rows[0]["thoiGianHThanhTH"].ToString();
                }
                DataTable dt2 = new DataTable();
                SqlFunction sqlFunc = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                SqlFunction sqlFuncUer = new SqlFunction(HttpContext.Current.Session.GetConnectionString1());
                string maDV = HttpContext.Current.Session.GetDonVi().maDonVi;
                string strWhere = "";
                if (sqlFuncUer.GetOneStringField("SELECT idGrouppr_sd FROM dbo.tblUsers WHERE tenDangNhap=N'" + HttpContext.Current.Session.GetCurrentUser().tenDangNhap + "'") == "userSD" || sqlFuncUer.GetOneStringField("SELECT idGrouppr_sd FROM dbo.tblUsers WHERE tenDangNhap=N'" + HttpContext.Current.Session.GetCurrentUser().tenDangNhap + "'") == "userTH")
                    strWhere = " maDonVipr_sd IN(SELECT maDonvipr FROM tblDMDonvi WHERE maDonvipr=N'" + maDV + "' OR maDonvipr_cha=N'" + maDV + "'  OR  maDonvipr_cha IN  (SELECT  maDonvipr FROM  tblDMDonvi WHERE  maDonvipr_cha=N'" + maDV + "'))";
                else
                    strWhere = " maDonVipr_sd IS NOT NULL ";

                string sttDA = hdfSTTDA.Value.ToString();
                string nienDo = HttpContext.Current.Session.GetNamSudung();
                decimal nienDoSau = Convert.ToDecimal(HttpContext.Current.Session.GetNamSudung()) + 1;
                string tuNgay = "01/01/" + HttpContext.Current.Session.GetNamSudung();
                string denNgay = "01/31/" + nienDoSau;
                string sql = @"SELECT tenNguonVon,SUM(tongMucDauTu) tongMucDauTu,SUM(keHoachVon) keHoachVon
                                ,SUM(giaTriNghiemThu) giaTriNghiemThu,SUM(khoiLuongGNgan) khoiLuongGNgan,SUM(khoiLuongQToan) khoiLuongQToan FROM (
                                SELECT tenNguonVon,tongMucDauTu,keHoachVon
                                ,giaTriNghiemThu,khoiLuongGNgan,khoiLuongQToan FROM (
                                SELECT (SELECT tenNguonVon FROM dbo.tblDMNguonVon WHERE maNguonVonpr=b.maNguonVonpr_sd) AS tenNguonVon
                                ,tongMucDauTu=(giaTriVonQD+tongMucDauTuTang-tongMucDauTuGiam)
                                ,keHoachVon=(keHoachVonNam+keHoachVonTang-keHoachVonGiam),giaTriNghiemThu=(giaTriTHien+giaTriTHienPL)
                                ,khoiLuongGNgan=giaTriGiaiNgan,khoiLuongQToan FROM(
                                SELECT sttDuAnpr_sd,maNguonVonpr_sd
                                ,giaTriVonQD=(SELECT ISNULL(SUM(giaTriVonQD),0) FROM tblNguonVonDA d WHERE d.sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND d.maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd)
                                ,tongMucDauTuTang=(SELECT ISNULL(SUM(giaTriVonQDTang),0) FROM dbo.tblDieuChinhMucDTuCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttDCMucDTupr_sd IN (SELECT sttDCMucDTupr FROM dbo.tblDieuChinhMucDTu WHERE lanDieuChinh=(SELECT MAX(lanDieuChinh) FROM tblDieuChinhMucDTu ) AND sttDuAnpr_sd =tblNguonVonDA.sttDuAnpr_sd))
                                ,tongMucDauTuGiam=(SELECT ISNULL(SUM(giaTriVonQDGiam),0) FROM dbo.tblDieuChinhMucDTuCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttDCMucDTupr_sd IN (SELECT sttDCMucDTupr FROM dbo.tblDieuChinhMucDTu WHERE lanDieuChinh=(SELECT MAX(lanDieuChinh) FROM tblDieuChinhMucDTu ) AND sttDuAnpr_sd =tblNguonVonDA.sttDuAnpr_sd))
                                ,keHoachVonNam=(SELECT ISNULL(SUM(keHoachvonnam),0) FROM dbo.tblKehoachVonCT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttKHVonpr_sd IN (SELECT sttKHVonpr FROM dbo.tblKehoachVon WHERE loai IN('1','3')))
                                ,keHoachVonTang=(SELECT ISNULL(SUM(keHoachVonTang),0) FROM dbo.tblKehoachVonCT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttKHVonpr_sd IN (SELECT sttKHVonpr FROM dbo.tblKehoachVon WHERE loai='2'))
                                ,keHoachVonGiam=(SELECT ISNULL(SUM(keHoachVonGiam),0) FROM dbo.tblKehoachVonCT WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND
                                sttKHVonpr_sd IN (SELECT sttKHVonpr FROM dbo.tblKehoachVon WHERE loai='2'))
                                ,giaTriTHien=(SELECT ISNULL(SUM(giaTriNghiemThu),0) FROM dbo.tblHopDongCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttHopDongpr_sd IN (SELECT sttHopDongpr FROM dbo.tblHopDong WHERE sttDuAnpr_sd= tblNguonVonDA.sttDuAnpr_sd ) )
                                ,giaTriTHienPL=(SELECT ISNULL(SUM(giaTriNghiemThu),0) FROM dbo.tblPhuLucHDCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttPLHDpr_sd IN (SELECT sttPLHDpr FROM dbo.tblPhuLucHD WHERE sttHopDongpr_sd IN (SELECT sttHopDongpr FROM dbo.tblHopDong WHERE sttDuAnpr_sd = tblNguonVonDA.sttDuAnpr_sd)) )
                                ,giaTriGiaiNgan=(SELECT ISNULL(SUM(soTien),0) FROM dbo.tblChungTuChiCT WHERE maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttCTChipr_sd IN(SELECT sttCTChipr FROM dbo.tblChungTuChi WHERE sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd AND loaiChungTupr_sd IN (N'010',N'011')))
                                ,khoiLuongQToan=(SELECT ISNULL(SUM(giaTriQuyetToanCQTC),0) FROM dbo.tblTapHopCP WHERE LEN(ISNULL(maCPDauTuXDCTpr_sd,''))>0 AND maNguonVonpr_sd=tblNguonVonDA.maNguonVonpr_sd AND sttHSQTpr_sd IN(SELECT sttHSQTpr FROM dbo.tblHoSoQT WHERE ISNULL(duyetHS,0)=1 AND sttDuAnpr_sd=tblNguonVonDA.sttDuAnpr_sd ))
                                FROM (
                                SELECT DISTINCT sttDuAnpr_sd,maNguonVonpr_sd FROM(
                                SELECT sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblNguonVonDA 
                                UNION ALL 
                                SELECT (SELECT sttDuAnpr_sd FROM dbo.tblDieuChinhMucDTu WHERE sttDCMucDTupr=tblDieuChinhMucDTuCT.sttDCMucDTupr_sd) AS sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblDieuChinhMucDTuCT
                                UNION ALL 
                                SELECT (SELECT sttDuAnpr_sd FROM dbo.tblHopDong WHERE sttHopDongpr=tblHopDongCT.sttHopDongpr_sd) AS sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblHopDongCT
                                UNION ALL 
                                SELECT (SELECT sttDuAnpr_sd FROM dbo.tblChungTuChi WHERE sttCTChipr=tblChungTuChiCT.sttCTChipr_sd) AS sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblChungTuChiCT
                                UNION ALL
                                SELECT  ( SELECT (SELECT sttDuAnpr_sd FROM dbo.tblHopDong a WHERE a.sttHopDongpr=b.sttHopDongpr_sd) FROM tblPhuLucHD b WHERE sttPLHDpr=tblPhuLucHDCT.sttPLHDpr_sd ) AS  sttDuAnpr_sd,maNguonVonpr_sd FROM dbo.tblPhuLucHDCT
                                ) AS tblDMNguon 
                                WHERE sttDuAnpr_sd ='" + sttDA + @"'
                                ) AS tblNguonVonDA
                                ) AS b 
                                ) AS c
                                ) AS d GROUP BY tenNguonVon ORDER BY tongMucDauTu DESC ";

                dt2 = sqlFunc.GetData(sql);
                ws.Range("A6:G6").CopyTo(ws.Range("A" + _vDongXuat + ":G" + _vDongXuat));
                _vDongXuat += 1;
                ws.Range("A7:G7").CopyTo(ws.Range("A" + _vDongXuat + ":G" + _vDongXuat));
                _vDongXuat += 1;
                if (dt2.Rows.Count > 0)
                {
                    int sttDong = 0;
                    foreach (DataRow dr in dt2.Rows)
                    {
                        sttDong += 1;
                        if (sttDong == 0)
                        {
                            ws.Range("A6:G6").CopyTo(ws.Range("A" + _vDongXuat + ":G" + _vDongXuat));
                            _vDongXuat += 1;
                            ws.Range("A7:G7").CopyTo(ws.Range("A" + _vDongXuat + ":G" + _vDongXuat));
                            _vDongXuat += 1;
                        }
                        if (sttDong == dt2.Rows.Count)
                        {
                            ws.Range("A10:G10").CopyTo(ws.Range("A" + _vDongXuat + ":G" + _vDongXuat));
                        }
                        else
                        {
                            ws.Range("A9:G9").CopyTo(ws.Range("A" + _vDongXuat + ":G" + _vDongXuat));
                        }
                        ws.Cell(_vDongXuat, "A").Value = sttDong;
                        ws.Cell(_vDongXuat, "B").Value = dr["tenNguonVon"].ToString();
                        ws.Cell(_vDongXuat, "C").Value = dr["tongMucDauTu"].ToString();
                        ws.Cell(_vDongXuat, "D").Value = dr["keHoachVon"].ToString();
                        ws.Cell(_vDongXuat, "E").Value = dr["giaTriNghiemThu"].ToString();
                        ws.Cell(_vDongXuat, "F").Value = dr["khoiLuongGNgan"].ToString();
                        ws.Cell(_vDongXuat, "G").Value = dr["khoiLuongQToan"].ToString();
                        _vDongXuat += 1;
                    }
                }
                _vDongXuat += 1;
                DataTable dt3 = new DataTable();
                if (chkHienChiPhiCon.Checked == true)
                {
                    string sql2 = @"SELECT maCPDauTuXDCTpr_sd,maChiPhiDTXDpr_sd,tenchiphi,tennhomchiphi,duToanDuocDuyet=(giaTriDuToan+giaTriDuToanTang-giaTriDuToanGiam),giaTriQuyetToan
                    ,tangGiam=(giaTriQuyetToan-(giaTriDuToan+giaTriDuToanTang-giaTriDuToanGiam)) FROM (
                    SELECT maCPDauTuXDCTpr_sd,maChiPhiDTXDpr_sd,tenchiphi=(SELECT tenCPDauTuXD FROM dbo.tblDMCPDauTuXDCT WHERE maChiPhiDTXDpr_sd=b.maChiPhiDTXDpr_sd AND maCPDauTuXDCTpr = b.maCPDauTuXDCTpr_sd)
                    ,tennhomchiphi=(SELECT tenChiPhiDTXD FROM tblDMCPDauTuXD WHERE maChiPhiDTXDpr=b.maChiPhiDTXDpr_sd)
                    ,giaTriDuToan
                    ,giaTriDuToanTang=ISNULL((SELECT SUM(giaTriDuToanTang) FROM dbo.tblDieuChinhDuToanCT WHERE maChiPhiDTXDpr_sd=b.maChiPhiDTXDpr_sd AND maCPDauTuXDCTpr_sd = b.maCPDauTuXDCTpr_sd AND sttDCDuToanpr_sd IN 
                    (SELECT sttDCDuToanpr FROM dbo.tblDieuChinhDuToan WHERE sttDuAnpr_sd='" + sttDA + @"' AND lanDieuChinh=(SELECT MAX(lanDieuChinh) FROM tblDieuChinhDuToan WHERE sttDuAnpr_sd='" + sttDA + @"'))),0)
                    ,giaTriDuToanGiam=ISNULL((SELECT SUM(giaTriDuToanGiam) FROM dbo.tblDieuChinhDuToanCT WHERE maChiPhiDTXDpr_sd=b.maChiPhiDTXDpr_sd AND maCPDauTuXDCTpr_sd = b.maCPDauTuXDCTpr_sd AND sttDCDuToanpr_sd IN 
                    (SELECT sttDCDuToanpr FROM dbo.tblDieuChinhDuToan WHERE sttDuAnpr_sd='" + sttDA + @"' AND lanDieuChinh=(SELECT MAX(lanDieuChinh) FROM tblDieuChinhDuToan WHERE sttDuAnpr_sd='" + sttDA + @"'))),0)
                    ,
                    giaTriQuyetToan=ISNULL((SELECT SUM(giaTriNghiemThu) FROM dbo.tblHopDongCT WHERE maChiPhiDTXDpr_sd=b.maChiPhiDTXDpr_sd AND maCPDauTuXDCTpr_sd = b.maCPDauTuXDCTpr_sd AND sttHopDongpr_sd 
                    IN (SELECT sttHopDongpr FROM dbo.tblHopDong WHERE sttDuAnpr_sd='" + sttDA + @"')),0)
                    +
                    ISNULL((SELECT SUM(giaTriNghiemThu) FROM dbo.tblPhuLucHDCT WHERE maChiPhiDTXDpr_sd=b.maChiPhiDTXDpr_sd AND maCPDauTuXDCTpr_sd = b.maCPDauTuXDCTpr_sd AND sttPLHDpr_sd 
                    IN(SELECT sttPLHDpr FROM dbo.tblPhuLucHD WHERE sttHopDongpr_sd IN( SELECT sttHopDongpr FROM dbo.tblHopDong WHERE sttHopDongpr_sd 
                    IN (SELECT sttHopDongpr FROM dbo.tblHopDong WHERE sttDuAnpr_sd='" + sttDA + @"')))),0) FROM (

                    SELECT maCPDauTuXDCTpr_sd,maChiPhiDTXDpr_sd,SUM(giaTriDuToan) AS giaTriDuToan FROM (
                    SELECT maCPDauTuXDCTpr_sd,maChiPhiDTXDpr_sd,giaTriDuToan FROM dbo.tblCPDauTuXDCT WHERE sttDuAnpr_sd='" + sttDA + @"' 
                    ) AS a GROUP BY maCPDauTuXDCTpr_sd,maChiPhiDTXDpr_sd

                    ) AS b
                    ) AS c";
                    dt3 = sqlFunc.GetData(sql2);
                    ws.Range("A12:G12").CopyTo(ws.Range("A" + _vDongXuat + ":G" + _vDongXuat));
                    _vDongXuat += 1;
                    ws.Range("A13:G13").CopyTo(ws.Range("A" + _vDongXuat + ":G" + _vDongXuat));
                    _vDongXuat += 1;
                    if (dt3.Rows.Count > 0)
                    {
                        int stt = 0, sttNhom = 0;
                        string nhom = "";
                        int sttDong = 0;
                        string rangeStart = "A", rangeEnd = "G";
                        try
                        {
                            foreach (DataRow dr in dt3.Rows)
                            {
                                sttDong += 1;
                                if (sttDong == 0)
                                {
                                    ws.Range("A12:G12").CopyTo(ws.Range("A" + _vDongXuat + ":G" + _vDongXuat));
                                    _vDongXuat += 1;
                                    ws.Range("A13:G13").CopyTo(ws.Range("A" + _vDongXuat + ":G" + _vDongXuat));
                                    _vDongXuat += 1;
                                }
                                if (sttDong == dt3.Rows.Count)
                                {
                                    ws.Range("A16:G16").CopyTo(ws.Range("A" + _vDongXuat + ":G" + _vDongXuat));
                                }
                                else
                                {
                                    ws.Range("A15:G15").CopyTo(ws.Range("A" + _vDongXuat + ":G" + _vDongXuat));
                                }
                                if (nhom.ToLower() != dr["tennhomchiphi"].ToString().ToLower())
                                {
                                    sttNhom += 1;
                                    ws.Range(rangeStart + _vDongXuat + ":" + rangeEnd + _vDongXuat).InsertRowsBelow(1);
                                    ws.Range(rangeStart + _vDongXuat + ":" + rangeEnd + _vDongXuat).CopyTo(ws.Range(rangeStart + (_vDongXuat + 1) + ":" + rangeEnd + (_vDongXuat + 1)));
                                    ws.Range(rangeStart + _vDongXuat + ":" + rangeEnd + _vDongXuat).Style.Font.SetBold(true).Alignment.SetWrapText(true).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin);
                                    ws.Cell(_vDongXuat, "A").Value = _vLOfficeExcel.IntToRoman(sttNhom);
                                    nhom = dr["tennhomchiphi"].ToString().ToUpper();
                                    ws.Cell(_vDongXuat, "B").Value = nhom;
                                    ws.Cell(_vDongXuat, "C").Value = Convert.ToDecimal(dt3.Compute("Sum(duToanDuocDuyet)", "tennhomchiphi='" + dr["tennhomchiphi"].ToString() + "'")).ToString();
                                    ws.Cell(_vDongXuat, "E").Value = Convert.ToDecimal(dt3.Compute("Sum(giaTriQuyetToan)", "tennhomchiphi='" + dr["tennhomchiphi"].ToString() + "'")).ToString();
                                    ws.Cell(_vDongXuat, "G").Value = Convert.ToDecimal(dt3.Compute("Sum(tangGiam)", "tennhomchiphi='" + dr["tennhomchiphi"].ToString() + "'")).ToString();
                                    _vDongXuat += 1;
                                    stt = 0;
                                }
                                stt += 1;
                                ws.Range(rangeStart + _vDongXuat + ":" + rangeEnd + _vDongXuat).InsertRowsBelow(1);
                                ws.Range(rangeStart + _vDongXuat + ":" + rangeEnd + _vDongXuat).CopyTo(ws.Range(rangeStart + (_vDongXuat + 1) + ":" + rangeEnd + (_vDongXuat + 1)));
                                ws.Range(rangeStart + _vDongXuat + ":" + rangeEnd + _vDongXuat).Style.Font.SetBold(false).Alignment.SetWrapText(true).Border.SetTopBorder(XLBorderStyleValues.Dotted).Border.SetBottomBorder(XLBorderStyleValues.Dotted).Border.SetRightBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin);
                                ws.Cell(_vDongXuat, "A").Value = stt;
                                ws.Cell(_vDongXuat, "B").Value = dr["tenchiphi"].ToString();
                                ws.Cell(_vDongXuat, "C").Value = dr["duToanDuocDuyet"].ToString();
                                ws.Cell(_vDongXuat, "E").Value = dr["giaTriQuyetToan"].ToString();
                                ws.Cell(_vDongXuat, "G").Value = dr["tangGiam"].ToString();
                                _vDongXuat += 1;
                            }
                        }
                        catch
                        {
                            _vDongXuat += 2;
                        }
                        ws.Range(rangeStart + _vDongXuat + ":" + rangeEnd + _vDongXuat).Delete(XLShiftDeletedCells.ShiftCellsUp);
                        ws.Range(rangeStart + _vDongXuat + ":" + rangeEnd + _vDongXuat).Style.Font.SetBold(false).Alignment.SetWrapText(true).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetBottomBorder(XLBorderStyleValues.None).Border.SetRightBorder(XLBorderStyleValues.None).Border.SetLeftBorder(XLBorderStyleValues.None);
                        }
                }
                else
                {
                    
                    string sql2 = @"SELECT maChiPhiDTXDpr_sd,noiDung,duToanDuocDuyet=(giaTriDuToan+giaTriDuToanTang-giaTriDuToanGiam),giaTriQuyetToan
                            ,tangGiam=(giaTriQuyetToan-(giaTriDuToan+giaTriDuToanTang-giaTriDuToanGiam)) FROM (
                            SELECT maChiPhiDTXDpr_sd,noiDung=(SELECT tenChiPhiDTXD FROM dbo.tblDMCPDauTuXD WHERE maChiPhiDTXDpr=b.maChiPhiDTXDpr_sd)
                            ,giaTriDuToan
                            ,giaTriDuToanTang=ISNULL((SELECT SUM(giaTriDuToanTang) FROM dbo.tblDieuChinhDuToanCT WHERE maChiPhiDTXDpr_sd=b.maChiPhiDTXDpr_sd AND sttDCDuToanpr_sd IN 
                            (SELECT sttDCDuToanpr FROM dbo.tblDieuChinhDuToan WHERE sttDuAnpr_sd='" + sttDA + @"' AND lanDieuChinh=(SELECT MAX(lanDieuChinh) FROM tblDieuChinhDuToan WHERE sttDuAnpr_sd='" + sttDA + @"'))),0)
                            ,giaTriDuToanGiam=ISNULL((SELECT SUM(giaTriDuToanGiam) FROM dbo.tblDieuChinhDuToanCT WHERE maChiPhiDTXDpr_sd=b.maChiPhiDTXDpr_sd AND sttDCDuToanpr_sd IN 
                            (SELECT sttDCDuToanpr FROM dbo.tblDieuChinhDuToan WHERE sttDuAnpr_sd='" + sttDA + @"' AND lanDieuChinh=(SELECT MAX(lanDieuChinh) FROM tblDieuChinhDuToan WHERE sttDuAnpr_sd='" + sttDA + @"'))),0)
                            ,giaTriQuyetToan=ISNULL((SELECT SUM(giaTriNghiemThu) FROM dbo.tblHopDongCT WHERE maChiPhiDTXDpr_sd=b.maChiPhiDTXDpr_sd AND sttHopDongpr_sd IN (SELECT sttHopDongpr FROM dbo.tblHopDong WHERE sttDuAnpr_sd='" + sttDA + @"')),0)
                            +ISNULL((SELECT SUM(giaTriNghiemThu) FROM dbo.tblPhuLucHDCT WHERE maChiPhiDTXDpr_sd=b.maChiPhiDTXDpr_sd AND sttPLHDpr_sd IN(SELECT sttPLHDpr FROM dbo.tblPhuLucHD WHERE sttHopDongpr_sd IN( SELECT sttHopDongpr FROM dbo.tblHopDong WHERE sttHopDongpr_sd IN (SELECT sttHopDongpr FROM dbo.tblHopDong WHERE sttDuAnpr_sd='" + sttDA + @"')))),0) FROM (
                            SELECT maChiPhiDTXDpr_sd,SUM(giaTriDuToan) AS giaTriDuToan FROM (
                            SELECT maChiPhiDTXDpr_sd,giaTriDuToan FROM dbo.tblCPDauTuXDCT WHERE sttDuAnpr_sd='" + sttDA + @"' 
                            ) AS a GROUP BY maChiPhiDTXDpr_sd
                            ) AS b
                            ) AS c";
                    dt3 = sqlFunc.GetData(sql2);
                    ws.Range("A12:G12").CopyTo(ws.Range("A" + _vDongXuat + ":G" + _vDongXuat));
                    _vDongXuat += 1;
                    ws.Range("A13:G13").CopyTo(ws.Range("A" + _vDongXuat + ":G" + _vDongXuat));
                    _vDongXuat += 1;
                    if (dt3.Rows.Count > 0)
                    {
                        int sttDong = 0;
                        foreach (DataRow dr in dt3.Rows)
                        {
                            sttDong += 1;
                            if (sttDong == 0)
                            {
                                ws.Range("A12:G12").CopyTo(ws.Range("A" + _vDongXuat + ":G" + _vDongXuat));
                                _vDongXuat += 1;
                                ws.Range("A13:G13").CopyTo(ws.Range("A" + _vDongXuat + ":G" + _vDongXuat));
                                _vDongXuat += 1;
                            }
                            if (sttDong == dt3.Rows.Count)
                            {
                                ws.Range("A16:G16").CopyTo(ws.Range("A" + _vDongXuat + ":G" + _vDongXuat));
                            }
                            else
                            {
                                ws.Range("A15:G15").CopyTo(ws.Range("A" + _vDongXuat + ":G" + _vDongXuat));
                            }
                            ws.Cell(_vDongXuat, "A").Value = sttDong;
                            ws.Cell(_vDongXuat, "B").Value = dr["noiDung"].ToString();
                            ws.Cell(_vDongXuat, "C").Value = dr["duToanDuocDuyet"].ToString();
                            ws.Cell(_vDongXuat, "E").Value = dr["giaTriQuyetToan"].ToString();
                            ws.Cell(_vDongXuat, "G").Value = dr["tangGiam"].ToString();
                            _vDongXuat += 1;
                        }
                    }
                   

                }
                if (dt2.Rows.Count > 0 || dt3.Rows.Count > 0)
                {
                    ws.Range("A6:G6").Delete(XLShiftDeletedCells.ShiftCellsUp);
                    ws.Range("A6:G6").Delete(XLShiftDeletedCells.ShiftCellsUp);
                    ws.Range("A6:G6").Delete(XLShiftDeletedCells.ShiftCellsUp);
                    ws.Range("A6:G6").Delete(XLShiftDeletedCells.ShiftCellsUp);
                    ws.Range("A6:G6").Delete(XLShiftDeletedCells.ShiftCellsUp);
                    ws.Range("A6:G6").Delete(XLShiftDeletedCells.ShiftCellsUp);
                    ws.Range("A6:G6").Delete(XLShiftDeletedCells.ShiftCellsUp);
                    ws.Range("A6:G6").Delete(XLShiftDeletedCells.ShiftCellsUp);
                    ws.Range("A6:G6").Delete(XLShiftDeletedCells.ShiftCellsUp);
                    ws.Range("A6:G6").Delete(XLShiftDeletedCells.ShiftCellsUp);
                    ws.Range("A6:G6").Delete(XLShiftDeletedCells.ShiftCellsUp);
                }
                wb.Save();
                Response.Redirect(url + fileName, true);
            }
            catch
            {
            }
        }
        [AjaxPro.AjaxMethod]
        public string checkCanhBao()
        {
            try
            { 
            SqlFunction sqlFuncUer = new SqlFunction(HttpContext.Current.Session.GetConnectionString1());
            string maDV = HttpContext.Current.Session.GetDonVi().maDonVi;
            string strWhere = "";
            if (sqlFuncUer.GetOneStringField("SELECT idGrouppr_sd FROM dbo.tblUsers WHERE tenDangNhap=N'" + HttpContext.Current.Session.GetCurrentUser().tenDangNhap + "'") == "userSD")
            {
                strWhere = " (maDonVipr_chudautu=N'" + maDV + "'  OR maDonVipr_qlda=N'" + maDV + "')";
            }
            else if (sqlFuncUer.GetOneStringField("SELECT idGrouppr_sd FROM dbo.tblUsers WHERE tenDangNhap=N'" + HttpContext.Current.Session.GetCurrentUser().tenDangNhap + "'") == "tc")
            {
                strWhere = " maDonVipr_cqcq=N'" + maDV + "' AND  chinhThuc=1";
            }
            else if (sqlFuncUer.GetOneStringField("SELECT idGrouppr_sd FROM dbo.tblUsers WHERE tenDangNhap=N'" + HttpContext.Current.Session.GetCurrentUser().tenDangNhap + "'") == "kbnn")
            {
                strWhere = " maDonVipr_noimotk=N'" + maDV + "' AND  chinhThuc=1";
            }
            else if (sqlFuncUer.GetOneStringField("SELECT idGrouppr_sd FROM dbo.tblUsers WHERE tenDangNhap=N'" + HttpContext.Current.Session.GetCurrentUser().tenDangNhap + "'") == "ubnd")
            {
                strWhere = " maDonVipr_capqd=N'" + maDV + "' AND chinhThuc=1 ";
            }
            else if (sqlFuncUer.GetOneStringField("SELECT idGrouppr_sd FROM dbo.tblUsers WHERE tenDangNhap=N'" + HttpContext.Current.Session.GetCurrentUser().tenDangNhap + "'") == "userTH")
            {
                strWhere = " maDonVipr_chudautu IN (SELECT maDonVipr FROM dbo.tblDMDonVi WHERE maDonvipr_cha=N'" + maDV + "') and  chinhThuc=1";
            }
            SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            string chuoiTruyVan = "";
            string sql = @"select distinct groupBy,tenDuAn=(select tenDuAn from tblDuAn where sttDuAnpr=sttDuAnpr_sd) from (
                    select  
                    groupBy=N'Tổng dự toán lớn hơn tổng mức đầu tư'
                    ,sttDuAnpr_sd                         
                    ,giaTriDuAn=isnull((select giaTriDuAn from tblCPDuAnXDCT b where b.maCPDauTuXDCTpr_sd=a.maCPDauTuXDCTpr_sd),0) 
                    ,giaTriDuToan
                    ,giaTriHD=isnull((select sum(giaTriPL) from tblPhuLucHDCT c where  c.maCPDauTuXDCTpr_sd=a.maCPDauTuXDCTpr_sd),0)
                    + isnull((select sum(giaTriHD) from tblHopDongCT d where  d.maCPDauTuXDCTpr_sd=a.maCPDauTuXDCTpr_sd),0)
                    from tblCPDauTuXDCT a  where sttDuAnpr_sd in( select sttDuAnpr from tblDuAn where "+ strWhere +"))as temp where giaTriDuToan>giaTriDuAn";
            DataTable tab = new DataTable();
            tab = sqlFun.GetData(sql);
            if (tab.Select("groupBy='Tổng dự toán lớn hơn tổng mức đầu tư'").Length != 0)
                chuoiTruyVan = "+ Dự toán > tổng mức ĐTư: " + tab.Select("groupBy='Tổng dự toán lớn hơn tổng mức đầu tư'").Length.ToString() + "<br/>";
            sql = @"select distinct
                    tenDuAn=(select tenDuAn from tblDuAn where sttDuAnpr=(select top 1 sttDuAnpr_sd from tblDauThau where sttDauThaupr=sttDauThaupr_sd))
                    ,groupBy=(case when giaTriGoiThau>giaTrungThau then N'Giá trúng thầu lớn hơn giá gói thầu'
                              when giaTriHD>giaTrungThau then N'Tổng giá trị các hợp đồng thuộc gói thầu lớn hơn giá trị gói thầu'	
                            end) from (
                    select 
                    sttDauThaupr_sd, giaTriGoiThau
                    ,giaTrungThau
                    ,giaTriHD=
                    isnull((select sum(giaTriPL) from tblPhuLucHDCT where sttPLHDpr_sd in 
                    (select sttPLHDpr from tblPhuLucHD where sttHopDongpr_sd in
                    (select sttHopDongpr from tblHopDong d where  d.sttDauThaupr_sd=a.sttDauThaupr_sd))),0)+
                    isnull((select sum(giaTriHD) from tblHopDongCT where sttHopDongpr_sd 
                    in(select sttHopDongpr from tblHopDong d where  d.sttDauThaupr_sd=a.sttDauThaupr_sd)),0)
                    from tblDauThauCT a where sttDauThaupr_sd in (select sttDauThaupr from tblDauThau where sttDuAnpr_sd in( select sttDuAnpr from tblDuAn where " + strWhere + ")))as temp  where giaTriGoiThau>giaTrungThau or giaTriHD>giaTrungThau";
            tab = new DataTable();
            tab = sqlFun.GetData(sql);
            if (tab.Select("groupBy='Giá trúng thầu lớn hơn giá gói thầu'").Length != 0)
                chuoiTruyVan = chuoiTruyVan + "+ Giá trúng thầu > giá gói thầu: " + tab.Select("groupBy='Giá trúng thầu lớn hơn giá gói thầu'").Length.ToString() + "<br/>";

            if (tab.Select("groupBy='Tổng giá trị các hợp đồng thuộc gói thầu lớn hơn giá trị gói thầu'").Length != 0)
                chuoiTruyVan = chuoiTruyVan + "+ Giá HĐ > giá gói thầu: " + tab.Select("groupBy='Tổng giá trị các hợp đồng thuộc gói thầu lớn hơn giá trị gói thầu'").Length.ToString() + "<br/>";

            sql = @"select 
                    groupBy=(case when ((ngayNghiemThu is null or ngayNghiemThu='') and ngayKetThuc<getdate()) then N'Hợp đồng đã qua ngày kết thúc dự kiến nhưng chưa nghiệm thu'
			                    when ngayKetThuc<ngayNghiemThu then N'Hợp đồng trễ tiến độ' end),
                    * from(select soHopDong,ngayKy,ngayHieuLuc,ngayKetThuc,ngayNghiemThu,ngayThanhLy
                    ,giaTriHopDong=isnull((select sum(giaTriHD) from tblHopDongCT where sttHopDongpr_sd=sttHopDongpr),0)
                    + isnull((select sum(giaTriNghiemThu) from tblPhuLucHDCT where sttPLHDpr_sd in 
	                    (select sttPLHDpr from tblPhuLucHD where sttHopDongpr_sd=sttHopDongpr)),0)
                    ,giaTriNghiemThu=isnull((select sum(giaTriHD) from tblHopDongCT where sttHopDongpr_sd=sttHopDongpr),0)
                    + isnull((select sum(giaTriNghiemThu) from tblPhuLucHDCT where sttPLHDpr_sd in 
	                    (select sttPLHDpr from tblPhuLucHD where sttHopDongpr_sd=sttHopDongpr)),0)
                    from tblHopDong where sttDuAnpr_sd in( select sttDuAnpr from tblDuAn where " + strWhere + "))as temp where ((ngayNghiemThu is null or ngayNghiemThu='') and ngayKetThuc<getdate()) or ngayKetThuc<ngayNghiemThu";
            tab = new DataTable();
            tab = sqlFun.GetData(sql);
            if (tab.Select("groupBy='Hợp đồng đã qua ngày kết thúc dự kiến nhưng chưa nghiệm thu'").Length != 0)
                chuoiTruyVan = chuoiTruyVan + "+ Hợp đồng chưa nghiệm thu: " + tab.Select("groupBy='Hợp đồng đã qua ngày kết thúc dự kiến nhưng chưa nghiệm thu'").Length.ToString() + "<br/>";

            if (tab.Select("groupBy='Hợp đồng trễ tiến độ'").Length != 0)
                chuoiTruyVan = chuoiTruyVan + "+ Hợp đồng chậm tiến độ: " + tab.Select("groupBy='Hợp đồng trễ tiến độ'").Length.ToString() + "</br>";

            if (chuoiTruyVan != "")
                return "<p style='color:Red; font-weight:bold;margin-left:15px; text-align:center'>Cảnh báo (ĐVT: dự án)</p>" + chuoiTruyVan + "<a href='/quanly/canhbao.aspx' style='color:Red;'>Xem chi tiết</a>";
            else
                return "";
            }
            catch
            {
                return "";
            }
        }
        [AjaxPro.AjaxMethod]
        public string khongHienThiLanSau()
        {
            SqlFunction sqlFuncUer = new SqlFunction(HttpContext.Current.Session.GetConnectionString1());
            sqlFuncUer.ExeCuteNonQuery(@"update tblUsers set thongBaoNC = '0' WHERE tenDangNhap= N'" + HttpContext.Current.Session.GetCurrentUser().tenDangNhap + "'");
            return "";
        }
        [AjaxPro.AjaxMethod]
        public bool kiemTraThongBaoNangCap()
        {
            SqlFunction sqlFuncUer = new SqlFunction(HttpContext.Current.Session.GetConnectionString1());
            return sqlFuncUer.CheckHasRecord(@"select thongBaoNC from tblUsers WHERE isnull(thongBaoNC,0) = '1' and tenDangNhap= N'" + HttpContext.Current.Session.GetCurrentUser().tenDangNhap + "'");
        }
    }
}
