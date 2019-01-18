using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.SessionState;
using WEB_DLL;
using AjaxPro;
using System.Data.SqlClient;
 
namespace ProOnline.Class
{
    [AjaxPro.AjaxNamespaceAttribute("ntsLibraryFunctions")]
    public static class ntsLibraryFunctions
    {
        public static string subString(string value, int IdxBegin, int length)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                    return "";
                else
                    return value.Substring(IdxBegin, length);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static void ghiLog(string chucNang, string moTa)
        {
            SqlFunction sqlFunc = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            sqlFunc.ExeCuteNonQuery(@"INSERT INTO sysLogs ( chucNang ,moTa ,link) VALUES (N'" + chucNang + "'  ,N'" + moTa + "',N'" + HttpContext.Current.Request.RawUrl + "')");
        }
        /// <summary>
        /// Cập nhật tblDuAn set giá trị = @giaTri where sttVBDApr=@sttVBDApr
        /// Gọi hàm này khi thực hiện thêm - sửa - xóa dữ liệu có ảnh hưởng đến giá trị văn bản
        /// </summary>
        /// <param name="sttVBDApr"></param>
        /// <param name="giaTri"></param> 
        public static bool capNhatGiaTriVanBan(string sttVBDApr, string giaTri)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(HttpContext.Current.Session.GetConnectionString2());
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();
                SqlCommand cmd = new SqlCommand("UPDATE dbo.tblVanBanDA SET giaTri=@giaTri WHERE sttVBDApr=@sttVBDApr", sqlCon);
                cmd.Parameters.AddWithValue("@giaTri", dinhDangSoSQL(giaTri));
                cmd.Parameters.AddWithValue("@sttVBDApr", sttVBDApr);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Tự xóa văn bản, đính kèm
        /// </summary>
        /// <returns></returns>
        /// [AjaxPro.AjaxMethod]
        public static bool xoaVanBan(string sttVBDApr)
        {
            try
            {
                SqlFunction _sqlfun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                string duongdan = _sqlfun.GetOneStringField("SELECT tenFile FROM tblVanBanDA where sttVBDApr = N'" + sttVBDApr.ToString() + "'");
                if (duongdan != "")
                {
                    string path = string.Concat(HttpContext.Current.Server.MapPath("~" + duongdan.ToString()));
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }
                SqlConnection sqlCon = new SqlConnection(HttpContext.Current.Session.GetConnectionString2());
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM tblVanBanDA WHERE sttVBDApr=@sttVBDApr", sqlCon);
                cmd.Parameters.AddWithValue("@sttVBDApr", sttVBDApr);
                if (cmd.ExecuteNonQuery() > 0)
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static DataTable danhSachDuAn()
        {
            SqlFunction sqlFunc = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            return sqlFunc.GetData("SELECT sttDuAnpr,maDuAn,tenDuAn,tenChuDauTu=(SELECT tenDonVi FROM tblDMDonVi WHERE maDonVipr=maDonVipr_chudautu),diaDiemXD FROM dbo.tblDuAn where maDonVipr_sd=N'" + HttpContext.Current.Session.GetDonVi().maDonVi + "' or  maDonVipr_gpmb='" + HttpContext.Current.Session.GetDonVi().maDonVi + "'");
        }
        [AjaxPro.AjaxMethod]
        public static DataTable danhSachHuyen(string maTinh)
        {
            SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            return sqlFun.GetData("SELECT '' maHuyenpr,''  tenHuyen union all SELECT maHuyenpr,tenHuyen FROM dbo.tblDMQuanHuyen WHERE maTinhpr_sd=N'" + maTinh + "' AND ngungSD=0");
        } 
        [AjaxPro.AjaxMethod]
        public static string nhan2So(string soA,string soB)
        {
            SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            return sqlFun.GetOneStringField("SELECT convert(nvarchar(18), " + dinhDangSoSQL(soA) + "*" + dinhDangSoSQL(soB) + ")").ToString().Replace(".", ",");

        }
        public static string dinhDangSoSQL(string value)
        {
            try
            {
                decimal.Parse(value.Replace(".", "").Replace(",", "."));
                if (string.IsNullOrEmpty(value))
                    return "0";
                return value.Replace(".", "").Replace(",", ".");
            }
            catch (Exception)
            {
                return "0";
            }
        } 
        [AjaxPro.AjaxMethod]
        public static DataTable danhSachXa(string maHuyen)
        {
            SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            return sqlFun.GetData("SELECT '' maXapr,'' tenXa  union all SELECT maXapr,tenXa FROM dbo.tblDMXa WHERE maHuyenpr_sd=N'" + maHuyen + "' AND ngungSD=0");
        }
        [AjaxPro.AjaxMethod]
        public static DataTable danhMucNguon(string sttDuAn)
        {
            SqlFunction sqlFunc = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            string sql = "";
            if (sqlFunc.CheckHasRecord(" SELECT sttCanhBaopr_sd FROM tblCanhBao WHERE sttCanhBaopr_sd=8 and maDonVipr_sd='"+ HttpContext.Current.Session.GetDonVi().maDonVi  +"'"))
            {
                sql = sql = @"SELECT '' maNguonVonpr,'' tenNguonVon union all 
                        SELECT maNguonVonpr,(CASE 
                        WHEN LEN(maNguonVonpr)=1 THEN tenNguonVon
                        WHEN LEN(maNguonVonpr)=2 THEN '--- '+tenNguonVon
                        WHEN (LEN(maNguonVonpr)=4 OR LEN(maNguonVonpr)=3) THEN '--- --- '+tenNguonVon
                        WHEN (LEN(maNguonVonpr)=6 OR LEN(maNguonVonpr)=5) THEN '--- --- --- '+tenNguonVon
                        ELSE '--- --- --- --- '+tenNguonVon END
                        ) AS tenNguonVon FROM dbo.tblDMNguonVon WHERE isnull(ngungSD,0) = 0
						and maCapQuanLypr_sd=(select maCapQLpr_sd from tblDuAn where sttDuAnpr='" + sttDuAn + "')  ORDER BY maNguonVonpr ";

            }
            else
            {
                sql = @"SELECT maNguonVonpr,(CASE 
                                    WHEN LEN(maNguonVonpr)=1 THEN tenNguonVon
                                    WHEN LEN(maNguonVonpr)=2 THEN '--- '+tenNguonVon
                                    WHEN (LEN(maNguonVonpr)=4 OR LEN(maNguonVonpr)=3) THEN '--- --- '+tenNguonVon
                                    WHEN (LEN(maNguonVonpr)=6 OR LEN(maNguonVonpr)=5) THEN '--- --- --- '+tenNguonVon
                                    ELSE '--- --- --- --- '+tenNguonVon END
                                    ) AS tenNguonVon FROM dbo.tblDMNguonVon WHERE isnull(ngungSD,0) = 0";


            }
            return sqlFunc.GetData(sql);
        }
        ///ghi Logs catch
        ///
        public static void ghiLogs(string noiDung)
        {
            try
            {
                System.IO.StreamWriter str = System.IO.File.AppendText(HttpContext.Current.Server.MapPath(@"~/Log.txt"));
                str.WriteLine("Time: " +DateTime.Now.ToString() + "- Mã NS: " + HttpContext.Current.Session.GetDonVi().maDonVi + "- UserID: " + HttpContext.Current.Session.GetCurrentUserID() + "-" + noiDung + "-" + HttpContext.Current.Request.Url.ToString());
                str.Close();
            }
            catch (Exception ex)
            {

            }
        } 
        [AjaxPro.AjaxMethod]
        public static string checkDuyetQT(string sttDuAn)
        {
            SqlFunction sqlFunc = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            return sqlFunc.GetOneStringField("select convert(char(1), duyetQToan) from tblDuAn where sttDuAnpr='" + sttDuAn + "'");

        }
        ///Chuyển ngày tháng có dạng ddMMyyyy sang MMddyyyyy
        public static string _mChuyenChuoiSangNgay(string ddMMyyyy)
        {
            return ddMMyyyy.Substring(3, 2) + "/" + ddMMyyyy.Substring(0, 2) + "/" + ddMMyyyy.Substring(6, 4);
        }
        ///Kiểm tra ngày tháng năm
        ///Nếu ngày hợp lệ trả về true
        [AjaxPro.AjaxMethod] 
        public static bool kiemTraNgay(string ngay)
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
        ///Kiểm tra chứng từ có ngày nhập và ngày niên độ không hợp lệ
        ///Nếu chứng từ rơi vào tháng 2 mà niên độ năm trước thì trả về false
        [AjaxPro.AjaxMethod]
        public static bool kiemTraChungTuNienDo13Thang(string nienDo, string ngayChungTu)
        {
            if (decimal.Parse(ngayChungTu.Split('/')[1].ToString()) > 1 && nienDo != ngayChungTu.Split('/')[2].ToString())
                return false;
            else
                return true;
        }
        ///User permission
        public static string loadTheoNguoiNhapLieu()
        {
            return (HttpContext.Current.Session["CurrentPermiss"].ToString().Split(';')[7] == "true" ? " nguoiThaoTac=" + HttpContext.Current.Session.GetCurrentUserID() : " 1=1 ");
        }
        [AjaxMethod]
        public static string phanQuyen()
        {
            return HttpContext.Current.Session["CurrentPermiss"].ToString().Split(';')[0];
        }
        [AjaxMethod]
        public static string phanQuyenRecord(string bangDuLieu, string recordID)
        {
            if (HttpContext.Current.Session["CurrentPermiss"].ToString().Split(';')[6].ToString() == "true")
            {
                SqlFunction sqlFunc = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                string khoaChinh = sqlFunc.GetOneStringField(@"SELECT TOP 1 KU.COLUMN_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TC INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KU ON TC.CONSTRAINT_TYPE = 'PRIMARY KEY'  AND KU.TABLE_NAME=N'" + bangDuLieu + "'");
                string nguoiThaoTac = sqlFunc.GetOneStringField("SELECT convert(nvarchar(18), nguoiThaoTac) FROM " + bangDuLieu + " WHERE " + khoaChinh + "=N'" + recordID + "'");

                if (nguoiThaoTac == "" || nguoiThaoTac == HttpContext.Current.Session.GetCurrentUserID().ToString())
                    return "true";
                else
                    return "false";
            }
            return "true";
        }
        //Hàm lấy giá trị tự tăng sử dụng AjaxPro
        [AjaxPro.AjaxMethod]
        public static decimal _mGetAutoID(string tableName, string columnName)
        {
            return ntsSqlFunctions._mGetAuToID(tableName, columnName, HttpContext.Current.Session[ntsEnumSessionName.ntsConnectionString1].ToString());
        }
        //Hàm lấy ngày tháng năm định dạng lại sử dụng AjaxPro
        [AjaxPro.AjaxMethod]
        public static string _mConvertDateTime(string DateTime)
        {
            try
            {
                return Convert.ToDateTime(DateTime).ToString("dd/MM/yyyy");
            }
            catch
            { return ""; }
        }
        [AjaxPro.AjaxMethod]
        public static bool _mCheckNumber(string _vstrValue)
        {
            try
            {
                if (string.IsNullOrEmpty(_vstrValue))
                    return false;
                double _vValue = Convert.ToDouble(_vstrValue);
                return true;
            }
            catch
            { }
            return false;
        }
        [AjaxPro.AjaxMethod]
        public static bool _mCheckDetermine(string _vstrValue)
        {
            try
            {
                DateTime _vValue = Convert.ToDateTime(_vstrValue);
                return true;
            }
            catch
            { }
            return false;
        }
        //kiem tra ma khi them vao bang DL

        //Ngày bỗ sung: 19/12/2018
        //Người bỗ sung: Vịnh
        //Nội dung: Các class dùng chung theme boostrap
        #region Các class dùng chung boostrap
         
        /// <summary>
        /// Kiêm tra trùng mã trong 1 bảng dữ liệu
        /// </summary>
        /// <param name="strMaKiemTra">Mã cần kiểm tra VD: 010</param>
        /// <param name="strBangKiemTra">Bảng cần kiểm tra VD: tblDMKho</param>
        /// <param name="strCotKiemTra">Cột cần kiểm tra VD: maKhopr</param>
        /// <returns>True: trùng mã, False: Không trùng mã</returns>
        public static bool kiemTraTrungMa(string strMaKiemTra, string strBangKiemTra, string strCotKiemTra)
        {
            try
            {

                SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                string strSQL = @"Select " + strCotKiemTra + " FROM " + strBangKiemTra + " WHERE " + strCotKiemTra + "=N'" + strMaKiemTra + "'";
                return sqlFun.CheckHasRecord(strSQL);
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Lấy mã tự tăng từ một bảng dữ liệu
        /// </summary>
        /// <param name="strKyTu">Số ký tự cần tự tăng VD: 3</param>
        /// <param name="strCotTang">Cột cần lấy giá trị tự tăng VD: maPhieuNhapKho</param>
        /// <param name="strBangTang">Bảng cần lấy giá trị cột tự tăng VD: tblPhieuNhapKho</param>
        /// <param name="strDinhDang">Định dạng cần lấy VD: 000</param>
        /// <returns>Giá trị mã tự tăng VD: NK002</returns>
        public static string layMaTuTang(string strKyTu, string strCotTang, string strBangTang, string strDinhDang)
        {
            try
            {

                SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                string soPhieu = "";
                string sql = "SELECT MAX(CONVERT(DECIMAL,RIGHT(" + strCotTang + ",4))) FROM dbo." + strBangTang;
                decimal _vNewKey = sqlFun.GetOneDecimalField(sql) + 1;
                soPhieu = strKyTu + _vNewKey.ToString(strDinhDang);
                return soPhieu;
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// Load dữ liệu cho combobox
        /// </summary>
        /// <param name="tenBang">Tên bảng cần lấy dữ liệu VD: tblDMTinh</param>
        /// <param name="cotTruyVan">Cột cần lấy VD: maTinhpr,tenTinh</param>
        /// <param name="dieuKien">Điều kiện lọc VD: ngungSD=0</param>
        /// <returns>DataTable bảng dữ liệu để load combobox</returns>
        public static DataTable loadCombobox(string tenBang, string cotTruyVan, string dieuKien)
        {
            try
            {
                SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                string str = "select " + cotTruyVan + " from " + tenBang + " " + dieuKien + "";
                return sqlFun.GetData(str);
            }
            catch
            {
                return null;
            }
        }

        
        /// <summary>
        /// Load mã theo cấu trúc định sẵn
        /// </summary>
        /// <param name="kyhieuLoaiPhieu">Ký hiệu phiếu cần lấy VD: NK/XK,....</param>
        /// <param name="bangDuLieu">Bảng dữ liệu cần lấy vd: tblNhapKho</param>
        /// <param name="cotDuLieu">Cột dữ liệu cần lấy VD: maPhieuNhapKho</param>
        /// <param name="dieuKienTruyVan">Điều kiện truy vấn VD: ngungSD=0</param>
        /// <param name="ngayLoc">Tạo theo cấu trúc theo tháng</param>
        /// <returns>chuỗi mã phiếu</returns>
        //HH, tblDMHangHoa,maHangHoa,'',null)
        //HD,tblHoaDon,soHoaDon,'and month(ngayhoadon) = N'01'',dd/mm/yyyy
        public static string taoMaTuTangTheoDM(string kyhieuLoaiPhieu, string bangDuLieu, string cotDuLieu, string dieuKienTruyVan, string ngayLoc)
        {

            SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            string _vKyhieuTruoc = "", _vKyhieuSau = "", _vKyhieuLoaiPhieu = kyhieuLoaiPhieu, _vDauCach = "";
            decimal _vchieuDaiChuoiTT = 0;
            bool _vTangTheoThang = false, _vTuTang = false, _vHienDauCach = false;
            DateTime ngayLap;
            _vKyhieuTruoc = sqlFun.GetOneStringField("Select kyHieuPhiatruoc From tblDMLoaiChungtu where maLoaiCTpr=N'" + _vKyhieuLoaiPhieu + "'");
            _vKyhieuSau = sqlFun.GetOneStringField("Select kyHieuPhiasau From tblDMLoaiChungtu where maLoaiCTpr=N'" + _vKyhieuLoaiPhieu + "'");
            _vchieuDaiChuoiTT = sqlFun.GetOneDecimalField("Select chieuDaiChuoiTT From tblDMLoaiChungtu where maLoaiCTpr=N'" + _vKyhieuLoaiPhieu + "'");
            _vTangTheoThang = Convert.ToBoolean(sqlFun.GetOneBoolField("Select tangTheoThang From tblDMLoaiChungtu where maLoaiCTpr=N'" + _vKyhieuLoaiPhieu + "'"));
            _vTuTang = Convert.ToBoolean(sqlFun.GetOneBoolField("Select tuTang From tblDMLoaiChungtu where maLoaiCTpr=N'" + _vKyhieuLoaiPhieu + "'"));
            _vHienDauCach = Convert.ToBoolean(sqlFun.GetOneBoolField("Select hienDauGach From tblDMLoaiChungtu where maLoaiCTpr=N'" + _vKyhieuLoaiPhieu + "'"));
            string format = "", _vSoPhieu = "", _vTruyVan = "", _vSoPhieuTT = "";
            decimal _vChieuDaiPhieu = 0, stt = 0, _vChieuDaiChuoiTTThang = 0;
            for (int i = 0; i < _vchieuDaiChuoiTT; i++)
            {
                format += "0";
            }
            try
            {
                if (_vTangTheoThang == true)
                {

                    ngayLap = Convert.ToDateTime(_mChuyenChuoiSangNgay(ngayLoc));
                    if (_vHienDauCach == true)
                        _vDauCach = "/";
                    else
                        _vDauCach = "";
                    //lay cau truc phieu (chua tinh so tu tang)
                    _vSoPhieu = _vKyhieuTruoc + ngayLap.Month.ToString("00") + _vDauCach + _vKyhieuSau;

                    //lấy chiều dài của phiếu 
                    _vChieuDaiPhieu = Convert.ToDecimal(_vSoPhieu.Length) + _vchieuDaiChuoiTT;//NK10/0010-2015/PNK

                    _vSoPhieuTT = _vKyhieuTruoc + ngayLap.Month.ToString("00") + _vDauCach;

                    _vChieuDaiChuoiTTThang = Convert.ToDecimal(_vSoPhieuTT.Length) + _vchieuDaiChuoiTT;

                    _vTruyVan = "SELECT MAX(CONVERT(DECIMAL,RIGHT(LEFT (" + cotDuLieu + "," + _vChieuDaiChuoiTTThang + ") ," + _vchieuDaiChuoiTT + "))) FROM " + bangDuLieu + " WHERE  LEN(" + cotDuLieu + ")="
                                 + _vChieuDaiPhieu + "" + dieuKienTruyVan + "";
                    stt = sqlFun.GetOneDecimalField(_vTruyVan) + 1;
                    return _vKyhieuTruoc + ngayLap.Month.ToString("00") + _vDauCach + stt.ToString(format) + _vKyhieuSau;
                }
                else
                {
                    //lay cau truc phieu (chua tinh so tu tang)
                    _vSoPhieu = _vKyhieuTruoc + _vKyhieuSau;
                    //lấy chiều dài của phiếu 
                    _vChieuDaiPhieu = Convert.ToDecimal(_vSoPhieu.Length) + _vchieuDaiChuoiTT;

                    _vChieuDaiChuoiTTThang = Convert.ToDecimal(_vKyhieuTruoc.Length) + _vchieuDaiChuoiTT;

                    _vTruyVan = "SELECT MAX(CONVERT(DECIMAL,RIGHT(LEFT (" + cotDuLieu + "," + _vChieuDaiChuoiTTThang + ")," + _vchieuDaiChuoiTT + "))) FROM " + bangDuLieu + " WHERE LEN(" + cotDuLieu + ")="
                                 + _vChieuDaiPhieu + " ";
                    stt = sqlFun.GetOneDecimalField(_vTruyVan) + 1;
                    return _vKyhieuTruoc + stt.ToString(format) + _vKyhieuSau;
                }
            }
            catch
            {
                //lay cau truc phieu (chua tinh so tu tang)
                _vSoPhieu = _vKyhieuTruoc + _vKyhieuSau;
                //lấy chiều dài của phiếu 
                _vChieuDaiPhieu = Convert.ToDecimal(_vSoPhieu.Length) + _vchieuDaiChuoiTT;

                _vChieuDaiChuoiTTThang = Convert.ToDecimal(_vKyhieuTruoc.Length) + _vchieuDaiChuoiTT;

                _vTruyVan = "SELECT MAX(CONVERT(DECIMAL,RIGHT(LEFT (" + cotDuLieu + "," + _vChieuDaiChuoiTTThang + ")," + _vchieuDaiChuoiTT + "))) FROM " + bangDuLieu + " WHERE LEN(" + cotDuLieu + ")="
                             + _vChieuDaiPhieu + " ";
                stt = sqlFun.GetOneDecimalField(_vTruyVan) + 1;
                return _vKyhieuTruoc + stt.ToString(format) + _vKyhieuSau;
            }
        }
        //kiem tram ma khi xoa
        public static bool kiemTraXoa(string sma, string schuoitru, string scot, string scotcon)
        {
            SqlFunction _sqlClass = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            if (scot == "sttNhapKhopr_sd" && _sqlClass.CheckHasRecord("SELECT sttNhapKhopr FROM dbo.tblNhapKho WHERE sttNhapKhopr = '" + sma + "' AND trangThai ='1'") == true)
            {
                return true;
            }
            if (scot == "sttTraNhapKhopr_sd" && _sqlClass.CheckHasRecord("SELECT sttTraNhapKhopr FROM dbo.tblTraNhapKho WHERE sttTraNhapKhopr = '" + sma + "' AND trangThai ='1'") == true)
            {
                return true;
            }
            if (scot == "sttHoaDonpr_sd" && _sqlClass.CheckHasRecord("SELECT sttHoaDonpr FROM dbo.tblHoaDon WHERE sttHoaDonpr = '" + sma + "' AND trangThai ='1'") == true)
            {
                return true;
            }
            if (scot == "sttKiemKhopr_sd" && _sqlClass.CheckHasRecord("SELECT sttKiemKhopr FROM dbo.tblKiemKho WHERE sttKiemKhopr = '" + sma + "' AND trangThai ='1'") == true)
            {
                return true;
            }
            bool travee = false;
            string strSQL = "SELECT TABLE_NAME tablename FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = N'" + scot + "' AND TABLE_NAME NOT IN (" + schuoitru + ")";
            DataTable _dt = new DataTable();
            _dt = _sqlClass.GetData(strSQL);
            strSQL = " ";
            if (_dt.Rows.Count > 0)
            {
                try
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {

                        if (i == 0)
                        {
                            strSQL = "select " + scot + " from " + _dt.Rows[i][0] + " where " + scot + " =N'" + sma + "'";
                        }
                        else
                        {
                            strSQL = strSQL + " UNION ALL select " + scot + " from " + _dt.Rows[i][0] + " where " + scot + " =N'" + sma + "'";
                        }
                    }
                    if (!_sqlClass.CheckHasRecord(strSQL))
                        if (schuoitru == "''" || schuoitru == "")
                            travee = false;
                        else
                        {
                            string strBangTru = schuoitru.Replace("'", "");
                            string[] mangBangTru = strBangTru.Split(',');
                            string[] mangCotCon = scotcon.Split(',');
                            for (int i = 0; i < mangBangTru.Length; i++)
                            {
                                travee = kiemTraXoa1(sma, mangCotCon[i].ToString(), mangBangTru[i].ToString(), scot);
                                if (travee)
                                    break;
                            }
                        }
                    else
                        travee = true;
                }
                catch { travee = true; }
            }
            else
            {
                travee = false;
            }

            return travee;
        }
        public static bool kiemTraXoa1(string sma, string scotcon, string strBangCon, string scotcha)
        {
            string strSQL = "SELECT TABLE_NAME tablename FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = N'" + scotcon + "_sd'";
            SqlFunction _sqlClass = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
            DataTable _dt = new DataTable();
            _dt = _sqlClass.GetData(strSQL);
            strSQL = " ";
            if (_dt.Rows.Count > 0)
            {
                try
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            strSQL = "select " + scotcon + "_sd from " + _dt.Rows[i][0] + " where " + scotcon + "_sd in  (select " + scotcon + " from  " + strBangCon + " where " + scotcha + " = N'" + sma + "')";
                        }
                        else
                        {
                            strSQL = strSQL + " UNION ALL select " + scotcon + "_sd from " + _dt.Rows[i][0] + " where " + scotcon + "_sd in  (select " + scotcon + " from  " + strBangCon + " where " + scotcha + " = N'" + sma + "')";
                        }
                    }
                    return _sqlClass.CheckHasRecord(strSQL);
                }
                catch { return true; }
            }
            else
            {
                return false;
            }


        }
        #endregion
    }
}
