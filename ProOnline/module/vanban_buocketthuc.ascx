<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="vanban_buocketthuc.ascx.cs" Inherits="ProOnline.module.vanban_buocketthuc" %>
<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc3_ucvanban" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="cc2_ucvanban" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit_ucvanban" %>
<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout_ucvanban" %>
<%@ Register Assembly="obout_Window_NET" Namespace="OboutInc.Window" TagPrefix="owd" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="cc2" %>
<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc3" %>
<%@ Register Src="~/module/canhan.ascx" TagName="canhan_uc" TagPrefix="canhan_uc" %>

<script>
    function uploadError_uc(sender, args) {
        alert("Tải tập tin thất bại");
    }
 
    function uploadComplete_uc(sender, args) {
        var fileExtension = args.get_fileName();
        var ketqua = ProOnline.module.vanban_buocketthuc.layThongTinVanBan(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttVBDApr_uc").value);
        document.getElementById('tdNoiDung').innerHTML = (ketqua.value.Rows[0].tenFile_ == "" ? "<p style=\"color:red;text-decoration:none\">Chưa đính kèm</p>" : "<a style=\"color:Blue;text-decoration:none\" onclick=\"xemVB('" + ketqua.value.Rows[0].tenFile_ + "')\" href=\"#\">Xem đính kèm</a>");
        
    }
    function uploadStarted_uc(sender, args) {
        var fileExtension = args.get_fileName();
        if (fileExtension.indexOf('.pdf') != -1 || fileExtension.indexOf('.PDF') != -1 || fileExtension.indexOf('.doc') != -1 || fileExtension.indexOf('.DOC') != -1 || fileExtension.indexOf('.docx') != -1 || fileExtension.indexOf('.DOCX') != -1) {
        }
        else {
            alert("Tập tin không đúng định dạng.");
            return;
        }
    }
   
    function cboCoQuanBanHanh_uc_OnSelectedIndexChanged() {
        var result = ProOnline.module.vanban_buocketthuc.danhSachDoiTuongTheoToChuc(cboCoQuanBanHanh_uc.value());
        cboNguoiKy_uc.options.clear();
        txtChucDanh_uc.value('');
        if (result.value != null) {
            for (var i = 0; i < result.value.Rows.length; i++) {
                var ten = result.value.Rows[i].tenCaNhan;
                var ma = result.value.Rows[i].chucVu;
                cboNguoiKy_uc.options.add(ten, ten, i);
            }
        }
    }
    function cboNguoiKy_uc_OnSelectedIndexChanged() {
        txtChucDanh_uc.value(ProOnline.module.vanban_buocketthuc.chucVuTheoCaNhan(cboCoQuanBanHanh_uc.value(), cboNguoiKy_uc.value()).value);
        return false;
    }
    function loadLoaiVanBan_uc(maLoaiVBanpr_cha, maGiaiDoan){
        var kq = ProOnline.module.vanban_buocketthuc.loadLoaiVanBan(maLoaiVBanpr_cha, maGiaiDoan);
        cboLoaiVanBan_uc.options.clear();
        for (var i=0; i<kq.value.Rows.length;i++){
            var ma = kq.value.Rows[i].maLoaiVBanpr;
            var ten = kq.value.Rows[i].tenLoaiVBan;
            cboLoaiVanBan_uc.options.add(ten, ma, i);
        }
       return false;
    }
    function btnThemCQBH(){
        WindowCQBH_uc.Open();
        WindowCQBH_uc.screenCenter();
        WindowCQBH_uc.setTitle("Thêm tổ chức");
        return false;
    }
    var flagToChuc = '0';
    function WindowCQBH_ucOnOpen() {
            WindowCQBH_uc.bringToFront();
            txtMa_uc.disable();
            txtMa_uc.value(ProOnline.module.vanban_buocketthuc.layMaToChuc().value);
            txtTen_uc.value("");
            cboThuocNhomToChuc_uc.value("");
            txtDiaChi_uc.value("");
            txtMaSoThue_uc.value("");
            txtDienThoai_uc.value("");
            txtFax_uc.value("");
            txtEmail_uc.value("");
            txtNguoiDaiDien_uc.value("");
            txtChucVuDaiDien_uc.value("");
            txtNguoiLienHe_uc.value("");
            txtChucVuLienHe_uc.value("");
            txtDienThoaiToChuc_uc.value("");
            setTimeout(function() { txtTen.focus(); }, 3);
            return false;
        }
        
    function LuuvaDongCQBH_uc() {
        var param = new Array();
        param[0] = txtMa_uc.value();
        param[1] = txtTen_uc.value();
        param[2] = "0";
        param[3] = cboThuocNhomToChuc_uc.value();
        param[4] = txtDiaChi_uc.value();
        param[5] = txtMaSoThue_uc.value();
        param[6] = txtDienThoai_uc.value();
        param[7] = txtFax_uc.value();
        param[8] = txtEmail_uc.value();
        param[9] = txtNguoiDaiDien_uc.value();
        param[10] = txtChucVuDaiDien_uc.value();
        param[11] = txtNguoiLienHe_uc.value();
        param[12] = txtChucVuLienHe_uc.value();
        param[13] = txtDienThoaiToChuc_uc.value();
        if (isEmptyText(param[0])) {
            alert("Mã tổ chức không được rỗng");
            return false;
        }
        if (isEmptyText(param[1])) {
            alert("Tên tổ chức không được rỗng");
            return false;
        }
        var kiemTra = ProOnline.module.vanban_buocketthuc.kiemTraLuuToChuc(param[0]);
        if (kiemTra.value == false) {
            ProOnline.module.vanban_buocketthuc.themToChuc(param);
            loadCoQuanBH_uc();
            cboCoQuanBanHanh_uc.value(txtMa_uc.value());
            winDowThemCaNhan_maToChucpr_sd.value(txtMa_uc.value());
            WindowCQBH_uc.Close(); 
        } else {
            alert("Mã tổ chức này đã tồn tại!");
            return false;
        }
        return false;
    }
    function loadCoQuanBH_uc() {
        var kq = ProOnline.module.vanban_buocketthuc.loadCoQuanBanHanh();
        cboCoQuanBanHanh_uc.options.clear();
        winDowThemCaNhan_maToChucpr_sd.options.clear();

        for (var i = 0; i < kq.value.Rows.length; i++) {
            var ma = kq.value.Rows[i].maToChucpr;
            var ten = kq.value.Rows[i].tenToChuc;
            cboCoQuanBanHanh_uc.options.add(ten, ma, i);
            winDowThemCaNhan_maToChucpr_sd.options.add(ten, ma, i);
        }
        return false;
    } 
    function isEmptyText(value) {
        var len = value.replace(new RegExp(" ", "g"), '').length;
        if (len == 0) {
            return true;
        }
        else
            return false;
    }
    function luuVanBan_uc() {
        //if (document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value == "" || document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value == "0") {
        //    alert("Không tồn tại dự án để thực hiện kết thúc quá trình thao tác!");
        //    return false;
        //}
        if (document.getElementById("ctl00_ContentPlaceHolder1_hdfSttVBDApr_uc").value == "" || document.getElementById("ctl00_ContentPlaceHolder1_hdfSttVBDApr_uc").value == "0") {
            alert("Không tồn tại thông tin tờ trình trước khi thực hiện kết thúc quá trình thao tác!");
            return false;
        }
        var param = new Array();
        param[0] = document.getElementById("ctl00_ContentPlaceHolder1_hdfSttVBDApr_uc").value;
        param[1] = txtSoVanBan_uc.value();
        param[2] = txtNgayKy_uc.value();
        param[3] = cboCoQuanBanHanh_uc.value();
        param[4] = cboNguoiKy_uc.text();
        param[5] = txtChucDanh_uc.value();
        param[6] = cboLoaiVanBan_uc.value();
        param[7] = txtNoiDung_uc.value();
        param[8] = txtGiaTri_uc.value();
        if (param[8] != "") {
            if (isNaN(param[8].replaceAll(".", "").replaceAll(",", "."))) {
                alert("Giá trị phải là kiểu số!");
                setTimeout(function () { txtGiaTri_uc.focus(); }, 1);
                return false;
            }
        }
        if (param[2] != "") {
            var ktNgay = ntsLibraryFunctions.kiemTraNgay(param[2]);
            if (ktNgay.value == false) {
                alert("Ngày ký phải có 10 ký tự dạng dd/MM/yyyy");
                setTimeout(function () { txtNgayKy_uc.focus(); }, 1);
                return false;
            }
        }
        if (param[6] == "") {
            alert("Loại văn bản không được bỏ trống!");
            return false;
        }
        var ketqua = ProOnline.module.vanban_buocketthuc.suaVanBan(param).value;
        if (ketqua != "thanhCong") {
            alert(ketqua);
            return false;
        }
        else {
            return ketqua;
        }
        
    }
    function xemVB(url) {
        window.open(url); 
        return false;
    }
    function loadThongTinVanBan_uc(sttVBDApr) {
        var ketqua = ProOnline.module.vanban_buocketthuc.layThongTinVanBan(sttVBDApr);
        if (ketqua.value.Rows.length > 0) {
            document.getElementById("ctl00_ContentPlaceHolder1_vanban_buocketthuc_sttVBDApr").value = sttVBDApr; 
            txtSoVanBan_uc.value(ketqua.value.Rows[0].soVanBan);
            txtNgayKy_uc.value(ketqua.value.Rows[0].ngay);
            cboCoQuanBanHanh_uc.value(ketqua.value.Rows[0].maToChucpr_phathanh);
            cboNguoiKy_uc.value(ketqua.value.Rows[0].tenNguoiKy);
            txtChucDanh_uc.value(ketqua.value.Rows[0].chucDanhNguoiKy);
            cboLoaiVanBan_uc.value(ketqua.value.Rows[0].maLoaiVBanpr_sd);
            txtNoiDung_uc.value(ketqua.value.Rows[0].noiDung);
            txtGiaTri_uc.value(formatNumber(ketqua.value.Rows[0].giaTri_));
            document.getElementById('tdNoiDung').innerHTML = (ketqua.value.Rows[0].tenFile_ == "" ? "<p style=\"color:red;text-decoration:none\">Chưa đính kèm</p>" : "<a style=\"color:Blue;text-decoration:none\" onclick=\"xemVB('" + ketqua.value.Rows[0].tenFile_ + "')\" href=\"#\">Xem đính kèm</a>");
        }
        else {
            document.getElementById("ctl00_ContentPlaceHolder1_vanban_buocketthuc_sttVBDApr").value = '';
            txtSoVanBan_uc.value('');
            txtNgayKy_uc.value('');
            cboCoQuanBanHanh_uc.value('');
            cboNguoiKy_uc.value('');
            txtChucDanh_uc.value('');
            cboLoaiVanBan_uc.value('');
            txtNoiDung_uc.value('');
            txtGiaTri_uc.value('0');
            document.getElementById('tdNoiDung').innerHTML = "<a style=\"color:Blue;text-decoration:none\" onclick=\"xemVB('" + ketqua.value.Rows[0].tenFile_ + "')\" href=\"#\">Xem đính kèm</a>";
            alert("Không tồn tại thông tin tờ trình trước khi thực hiện kết thúc quá trình thao tác!");
            e.preventDefault();
        }
    }
    function luuThongTinCaNhanModule() {
        debugger;
        var ketQua = luuThongTinCaNhan();
        if (ketQua == "1") {
            winDowThemCaNhan.Close();
            var result = ProOnline.module.vanban_buocketthuc.danhSachDoiTuongTheoToChuc(cboCoQuanBanHanh_uc.value());
            cboNguoiKy_uc.options.clear();
            txtChucDanh_uc.value('');
            if (result.value != null) {
                for (var i = 0; i < result.value.Rows.length; i++) {
                    var ten = result.value.Rows[i].tenCaNhan;
                    var ma = result.value.Rows[i].chucVu;
                    cboNguoiKy_uc.options.add(ten, ten, i);
                }
            }
        }
        else if (ketQua == "0") {
            alert("Thông tin cá nhân chưa được lưu!");
        }
        else {
            alert(ketQua);
        }
        return false;
    }
</script>
<fieldset style="border: 1px solid #DBDBE1; margin: 3px; padding: 4px">
    <asp:HiddenField ID="sttVBDApr" runat="server" Value="0" />
     <asp:HiddenField ID="hdfTenForm" runat="server" Value="" />
    <legend><b>Văn bản</b></legend>
    <table style="width: 100%">
        <tr>
            <td style="width: 100px">Số văn bản</td>
            <td style="width: 300px">
                <cc2_ucvanban:OboutTextBox ID="txtSoVanBan_uc" runat="server" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox">
                </cc2_ucvanban:OboutTextBox></td>
            <td style="width: 100px">Ngày ký</td>
            <td style="width: 300px">
                <cc2_ucvanban:OboutTextBox ID="txtNgayKy_uc" runat="server" Width="35%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox">
                </cc2_ucvanban:OboutTextBox>
                <obout_ucvanban:Calendar ID="Calendar_uc" runat="server" DatePickerMode="true" TextBoxId="txtNgayKy_uc"
                    DateFormat="dd/MM/yyyy" TitleText="Chọn ngày" Columns="1" DatePickerImagePath="~/App_Themes/Styles/Date/date_picker1.gif"
                    CultureName="vi-VN">
                </obout_ucvanban:Calendar>
            </td>
            <td style="width: 10px"></td>
        </tr>
        <tr class="hiden">
            <td style="width: 120px">Cơ quan ban hành</td>
            <td>
                <cc3_ucvanban:ComboBox ID="cboCoQuanBanHanh_uc" runat="server" Width="100%" MenuWidth="100%" Height="180px"
                    AppendDataBoundItems="false" FilterType="Contains" FolderStyle="App_Themes/Styles/Interface/OboutComboBox">
                    <ClientSideEvents OnSelectedIndexChanged="cboCoQuanBanHanh_uc_OnSelectedIndexChanged" />
                    <FooterTemplate>
                        <div onclick="btnThemCQBH(); return false;">Thêm tổ chức</div>
                    </FooterTemplate>
                </cc3_ucvanban:ComboBox>
            </td>
            <td style="width: 120px">Người ký</td>
            <td>
                <cc3_ucvanban:ComboBox ID="cboNguoiKy_uc" runat="server" Width="100%" MenuWidth="100%" Height="180px"
                    AppendDataBoundItems="false" FilterType="Contains" FolderStyle="App_Themes/Styles/Interface/OboutComboBox">
                    <ClientSideEvents OnSelectedIndexChanged="cboNguoiKy_uc_OnSelectedIndexChanged" />
                     <FooterTemplate>
                         <div style="cursor: pointer" onclick="dinhDangWindow(winDowThemCaNhan);return false;">
                            Thêm người ký
                        </div>
                    </FooterTemplate>
                </cc3_ucvanban:ComboBox>
            </td>
            <td style="width: 10px"></td>
        </tr>
        <tr>
            <td style="width: 120px"  class="hiden">Chức danh</td>
            <td class="hiden">
                <cc2_ucvanban:OboutTextBox ID="txtChucDanh_uc" runat="server" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox">
                </cc2_ucvanban:OboutTextBox>
            </td>
            <td style="width: 120px">Loại văn bản</td>
            <td>
                <cc3_ucvanban:ComboBox ID="cboLoaiVanBan_uc" runat="server" Width="100%" MenuWidth="100%" Height="180px"
                    AppendDataBoundItems="false" FilterType="Contains" FolderStyle="App_Themes/Styles/Interface/OboutComboBox">
                </cc3_ucvanban:ComboBox>
            </td>
            <td style="width: 10px">
                <span style="color: red">*</span>
            </td>
        </tr>
        <tr>
            <td style="width: 120px">Nội dung</td>
            <td colspan="3">
                <cc2_ucvanban:OboutTextBox ID="txtNoiDung_uc" TextMode="MultiLine" runat="server" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox">
                </cc2_ucvanban:OboutTextBox>
            </td>
            <td style="width: 10px"></td>
        </tr>
        <tr>
            <td style="width: 120px">Giá trị</td>
            <td> 
                <cc2_ucvanban:OboutTextBox ID="txtGiaTri_uc" runat="server" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox">
                                                                <ClientSideEvents OnTextChanged="dinhDangSo"/>
                </cc2_ucvanban:OboutTextBox>
            </td>
            <td colspan="2"> 
                <table style="width: 460px;display: contents;">
                    <tr>
                        <td style="width:300px"> 
                <ajaxToolkit_ucvanban:ToolkitScriptManager ID="ToolkitScriptManager_uc" runat="server">
                </ajaxToolkit_ucvanban:ToolkitScriptManager>
                <ajaxToolkit_ucvanban:AsyncFileUpload OnClientUploadError="uploadError_uc" OnClientUploadComplete="uploadComplete_uc" 
                    runat="server" ID="AsyncFileUpload_uc" Width="390px" UploaderStyle="Traditional"
                    OnClientUploadStarted="uploadStarted_uc" UploadingBackColor="" ThrobberID="myThrobber"
                    BorderStyle="NotSet" Font-Underline="False" Font-Strikeout="False" />
                <br />
                <div>
                    <span style="color: Red;"><span><i>Chỉ cho phép tải lên tập tin định dạng: *.doc, *.docx,*.pdf</i></span></span>
                </div>
                            </td>
                        <td  style="width: 170px" id="tdNoiDung"></td>
                        </tr>
                    </table>
            </td>
            <td style="width: 10px"></td>
        </tr>
    </table>
</fieldset>

  <div>
        <owd:Window ID="WindowCQBH_uc" runat="server" IsModal="true" ShowCloseButton="true" Status=""
            ShowStatusBar="false" RelativeElementID="WindowPositionHelper" Height="400" Width="600"
            OnClientOpen="WindowCQBH_ucOnOpen()" VisibleOnLoad="false" StyleFolder="~/App_Themes/Styles/wdstyles/dogma"
            IsResizable="false">
            <div style="margin-top: 5px">
                <table>
                    <tr>
                        <td>
                            <cc2:OboutButton ID="bttLuuvaDongCN" runat="server" Text="Lưu và đóng" Width="100px"
                                OnClientClick="LuuvaDongCQBH_uc();return false;" FolderStyle="App_Themes/Styles/Interface/OboutButton">
                            </cc2:OboutButton>
                        </td>
                        <td>
                            <cc2:OboutButton ID="bttDongCN" runat="server" Text="Đóng" Width="100px" OnClientClick="WindowCQBH_uc.Close(); return false;">
                            </cc2:OboutButton>
                        </td>
                        <td style="width: 270px">
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <fieldset style="border: 1px solid #DBDBE1">
                    <legend>Thông tin tổ chức</legend>
                    <table style="width: 565px">
                        <tr>
                            <td style="width: 100px">
                                Mã
                            </td>
                            <td>
                                <cc2:OboutTextBox ID="txtMa_uc" runat="server" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox">
                                </cc2:OboutTextBox>
                            </td>
                            <td>
                                <span style="color: red">*</span>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Tên tổ chức
                            </td>
                            <td colspan="3">
                                <cc2:OboutTextBox ID="txtTen_uc" runat="server" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox">
                                </cc2:OboutTextBox>
                            </td>
                            <td style="width: 10px">
                                <span style="color: red">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Thuộc nhóm
                            </td>
                            <td colspan="3">
                                <cc3:ComboBox ID="cboThuocNhomToChuc_uc" runat="server" Width="100%" Height="150" FolderStyle="App_Themes/Styles/Interface/OboutComboBox">
                                </cc3:ComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Địa chỉ
                            </td>
                            <td colspan="3">
                                <cc2:OboutTextBox ID="txtDiaChi_uc" runat="server" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox">
                                </cc2:OboutTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Mã số thuế
                            </td>
                            <td style="width: 220px">
                                <cc2:OboutTextBox ID="txtMaSoThue_uc" runat="server" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox">
                                </cc2:OboutTextBox>
                            </td>
                            <td style="width: 100px">
                                Điện thoại
                            </td>
                            <td style="width: 220px">
                                <cc2:OboutTextBox ID="txtDienThoai_uc" runat="server" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox">
                                </cc2:OboutTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Fax
                            </td>
                            <td style="width: 220px">
                                <cc2:OboutTextBox ID="txtFax_uc" runat="server" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox">
                                </cc2:OboutTextBox>
                            </td>
                            <td style="width: 100px">
                                Email
                            </td>
                            <td style="width: 220px">
                                <cc2:OboutTextBox ID="txtEmail_uc" runat="server" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox">
                                </cc2:OboutTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Người đại diện
                            </td>
                            <td style="width: 220px">
                                <cc2:OboutTextBox ID="txtNguoiDaiDien_uc" runat="server" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox">
                                </cc2:OboutTextBox>
                            </td>
                            <td style="width: 100px">
                                Chức vụ
                            </td>
                            <td style="width: 220px">
                                <cc2:OboutTextBox ID="txtChucVuDaiDien_uc" runat="server" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox">
                                </cc2:OboutTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Người liên hệ
                            </td>
                            <td style="width: 220px">
                                <cc2:OboutTextBox ID="txtNguoiLienHe_uc" runat="server" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox">
                                </cc2:OboutTextBox>
                            </td>
                            <td style="width: 100px">
                                Chức vụ
                            </td>
                            <td style="width: 220px">
                                <cc2:OboutTextBox ID="txtChucVuLienHe_uc" runat="server" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox">
                                </cc2:OboutTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Điện thoại
                            </td>
                            <td colspan="3">
                                <cc2:OboutTextBox ID="txtDienThoaiToChuc_uc" runat="server" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox">
                                </cc2:OboutTextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </owd:Window>
      <owd:Window ID="winDowThemCaNhan" runat="server" IsModal="true" ShowCloseButton="true" Status="" Title="Thêm cá nhân"
    ShowStatusBar="false" RelativeElementID="WindowPositionHelper" Height="400" Width="600" VisibleOnLoad="false" StyleFolder="~/App_Themes/Styles/wdstyles/dogma"
    IsResizable="false" OnClientOpen="resetThongTinCaNhan()">
    <div style="margin-top: 5px">
        <table>
            <tr>
                <td>
                    <cc2:OboutButton ID="OboutButton1" runat="server" Text="Lưu và đóng" Width="100px"
                        OnClientClick="luuThongTinCaNhanModule();return false;" FolderStyle="App_Themes/Styles/Interface/OboutButton">
                    </cc2:OboutButton>
                </td>
                <td>
                    <cc2:OboutButton ID="OboutButton2" runat="server" Text="Đóng" Width="100px" OnClientClick="winDowThemCaNhan.Close(); return false;">
                    </cc2:OboutButton>
                </td>
                <td style="width: 270px"></td>
            </tr>
        </table>
    </div>
    <div>
        <fieldset style="border: 1px solid #DBDBE1">
            <legend><b>Thông tin cá nhân</b></legend>
            <canhan_uc:canhan_uc ID="canhan_uc" runat="server" />

        </fieldset>
    </div>
</owd:Window>
    </div>

