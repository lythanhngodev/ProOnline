<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true"
    CodeBehind="nghiemthu.aspx.cs" Inherits="ProOnline.quanly.nghiemthuhopdong" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>
<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc3" %>
<%@ Register TagPrefix="oem" Namespace="OboutInc.EasyMenu_Pro" Assembly="obout_EasyMenu_Pro" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="cc2" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Window_NET" Namespace="OboutInc.Window" TagPrefix="owd" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/module/thongtinduan_b1.ascx" TagName="thongtinduan_b1" TagPrefix="uc4" %>
<%@ Register Src="~/module/vanban_buocketthuc.ascx" TagName="vanban_buocketthuc"
    TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/App_Themes/BuocThaoTac/css/ace.min.css" />
    <link href="/App_Themes/BuocThaoTac/font-awesome/4.5.0/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .searchCss {
            border-top: 1px solid #CCC;
            border-left: 1px solid #CCC;
            border-right: 1px solid #CCC;
            border-bottom: 1px solid #CCC;
            height: 23px;
        }

            .searchCss:focus {
                border-top: 1px solid #009DCC;
                border-left: 1px solid #009DCC;
                border-right: 1px solid #009DCC;
                border-bottom: 1px solid #009DCC;
            }

            .searchCss:hover {
                border-top: 1px solid #009DCC;
                border-left: 1px solid #009DCC;
                border-right: 1px solid #009DCC;
                border-bottom: 1px solid #009DCC;
            }

        .thonTin {
            font-size: 13px;
            font-weight: bold;
        }
    </style>
    <script type="text/javascript">
        var coThaoTac = "0";
        var sttNghiemThupr = "";
        var daLuu = "0";
        window.onload = function () {
            Grid1.refresh();
            eKetLuan.options.add('Đạt', 'Đạt', 0);
            eKetLuan.options.add('Không đạt', 'Không đạt', 1);
            if ("<%= ProOnline.Class.ntsLibraryFunctions.phanQuyen() %>" == "true") {
                OboutButton5.disable();
                OboutButton6.disable();
                OboutButton7.disable();
            }
            //loadLoaiVanBan_uc("XXIII", "020");
            txtSoVanBan_uc.disable();
            txtNgayKy_uc.disable();
            //cboLoaiVanBan_uc.disable();
        }
        function loadDanhSachCongViec() {
            debugger;
            var result = ProOnline.quanly.nghiemthuhopdong.getDanhSachCongViec(cboHopDong.value());
            cboCongViec.options.clear();
            for (var i = 0; i < result.value.Rows.length; i++) {
                var noiDung = result.value.Rows[i].noiDung;
                var ma = result.value.Rows[i].stt;
                cboCongViec.options.add(noiDung, ma, i);
            }
        }
        function changeCongViec() {
            debugger;
            var result = ProOnline.quanly.nghiemthuhopdong.thongTinCongViec(cboCongViec.value(), coThaoTac);
            if (result.value.Rows.length > 0) {
                egiaTrungThau.value(formatNumber(result.value.Rows[0].giaTrungThau + ""));
                egiaHopDong.value(formatNumber(result.value.Rows[0].giaTriHD + ""));
                egiaHopDongDC.value(formatNumber(result.value.Rows[0].giaTriHDDC + ""));
                egiaNghiemThu.value(formatNumber(result.value.Rows[0].giaTriNThu + ""));
                giaNghiemThuLK.value('');
                return false;
            }
            return false;
        }
        function format(number) {
            return String(number).replace(/(.)(?=(\d{3})+$)/g, '$1.');
        }
        String.prototype.replaceAll = function (strTarget, strSubString) {
            var strText = this;
            if (strText.length > 0) {
                var intIndexOfMatch = strText.indexOf(strTarget);
                while (intIndexOfMatch != -1) {
                    strText = strText.replace(strTarget, strSubString)
                    intIndexOfMatch = strText.indexOf(strTarget);
                }
                return (strText);
            }
            else {
                return "";
            }
        }
        function anHienControl(flag) {
            if (flag == "t") {
                debugger;
                cboHopDong.value('');
                canCuNghiemThu.value('');
                soBienBan.value('');
                ngayLap.value('');
                cboHopDong.enable();
                canCuNghiemThu.enable();
                soBienBan.enable();
                ngayLap.enable();
                document.getElementById("thongTinNghiemThuCT").style.visibility = "visible";
                document.getElementById("thongTinNghiemThuCT").style.display = "Block";
                document.getElementById("thongTinNghiemThu").style.visibility = "hidden";
                document.getElementById("thongTinNghiemThu").style.display = "none";
                cboHopDong.value("0");
                sttNghiemThupr = "";
            }
            if (flag == "s") {
                debugger;
                document.getElementById("thongTinNghiemThuCT").style.visibility = "visible";
                document.getElementById("thongTinNghiemThuCT").style.display = "Block";
                document.getElementById("thongTinNghiemThu").style.visibility = "hidden";
                document.getElementById("thongTinNghiemThu").style.display = "none";
                daLuu = "0";
            }
            if (flag == "p") {
                debugger;
                Grid1.refresh();
                document.getElementById("thongTinNghiemThu").style.visibility = "visible";
                document.getElementById("thongTinNghiemThu").style.display = "Block";
                document.getElementById("thongTinNghiemThuCT").style.visibility = "hidden";
                document.getElementById("thongTinNghiemThuCT").style.display = "none";
                coThaoTac = "";
                sttXoa = "";
                sttNghiemThupr = "";
                sttNTCT = "";
                daLuu = "0";
                document.getElementById("ctl00_ContentPlaceHolder1_hdfSttNT").value = "";
                document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value = "";
                loadHopDong();
            }
        }
        function loadHopDong() {
            debugger;
            var result = ProOnline.quanly.nghiemthuhopdong.getDanhSachHopDong(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value);

            cboHopDong.options.clear();
            for (var i = 0; i < result.value.Rows.length; i++) {
                var noiDung = result.value.Rows[i].noidung + "";
                var ma = result.value.Rows[i].sttHopDongpr + "";
                cboHopDong.options.add(noiDung, ma, i);
            }
            cboHopDong.selectedIndex(0);
        }
        function suaThongTinNghiemThu() {
            if (Grid1.SelectedRecords.length > 0) {
                debugger;
                var record = Grid1.SelectedRecords[0];
                if ("<%= ProOnline.Class.ntsLibraryFunctions.phanQuyen() %>" == "true") {
                    alert("User đang sử dụng chỉ được xem dữ liệu, không thực hiện được thao tác sửa!");
                    return false;
                }
                if (ntsLibraryFunctions.phanQuyenRecord("tblNghiemThu", record.sttNghiemThupr).value == "false") {
                    alert("Dòng dữ liệu đang chọn của User khác nhập liệu, Bạn không được phép sửa!");
                    return false;
                }
                document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value = record.sttDuAnpr_sd;
                layThongTinDuAn_uc(record.sttDuAnpr_sd);//nếu không truyền tham số là lấy mặt định từ session vd: layThongTinDuAn_uc(0);
                btnChonDA_uc.disable();
                loadHopDong();
                cboHopDong.value(record.sttHopDongpr_sd);
                canCuNghiemThu.value(record.cacCanCu);
                soBienBan.value(record.soBienBan);
                ngayLap.value(record.ngayLap);
                sttNghiemThupr = record.sttNghiemThupr;
                coThaoTac = "1";
                document.getElementById("ctl00_ContentPlaceHolder1_hdfSttNT").value = record.sttNghiemThupr;
                anHienControl("s");
                Grid2.refresh();
                if (ProOnline.quanly.nghiemthuhopdong.SoDongGrid2().value == "0") {
                    cboHopDong.enable();
                } else { cboHopDong.disable(); }
            }
            else {
                alert("Không có dòng dữ liệu nào được chọn!");
            }
        }
        //xóa lưới ngoai
        function xoaThongTinNT() {
            debugger;
            coThaoTac = "0";
            if (Grid1.SelectedRecords.length > 0) {
                var record = Grid1.SelectedRecords[0];
                if ("<%= ProOnline.Class.ntsLibraryFunctions.phanQuyen() %>" == "true") {
                    alert("User đang sử dụng chỉ được xem dữ liệu, không thực hiện được thao tác xóa!");
                    return false;
                }
                if (ntsLibraryFunctions.phanQuyenRecord("tblNghiemThu", record.sttNghiemThupr).value == "false") {
                    alert("Dòng dữ liệu đang chọn của User khác nhập liệu, Bạn không được phép xóa!");
                    return false;
                }
                if (ProOnline.quanly.nghiemthuhopdong.checkDuyetQT(record.sttDuAnpr_sd).value == "1") {
                    alert("Dự án đã được quyết toán, bạn không thể thực hiện thao tác này")
                    return false;
                }
                sttXoa = record.sttNghiemThupr;
                Dialog3.Open();
                return false;
            }
            else {
                alert("Không có dòng dữ liệu nào được chọn!");
            }
        }
        function themThongTinNghiemThu() {
            debugger;
            layThongTinDuAn_uc(0);//nếu không truyền tham số là lấy mặt định từ session vd: layThongTinDuAn_uc(0); 
            btnChonDA_uc.enable();
            daLuu = "";
            coThaoTac = "0";
            document.getElementById("ctl00_ContentPlaceHolder1_hdfSttNT").value = "";
            anHienControl("t");
        }
        function themThongTinNghiemThu2() {
            debugger;
            daLuu = "";
            coThaoTac = "0";
            document.getElementById("ctl00_ContentPlaceHolder1_hdfSttNT").value = "";
            anHienControl("t");
        }
        function Grid1OnClientDblClick(record) {
            debugger;
            suaThongTinNghiemThu();
            return false;
        }
        function suaThongTinNghiemThu1() {
            debugger;
            daLuu = "";
            coThaoTac = "1";
            anHienControl("s");
            if (ProOnline.quanly.nghiemthuhopdong.SoDongGrid2().value == "0") {
                cboHopDong.enable();
            } else { cboHopDong.disable(); }
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <b style="padding: 3px;">NGHIỆM THU</b>
    <asp:HiddenField ID="hdfSttDuAnpr_uc" runat="server" Value="" />
    <asp:HiddenField ID="hdfSttNT" runat="server" Value="" />
    <asp:HiddenField ID="hdfSttVBDApr_uc" runat="server" Value="" />
    <div id="thongTinNghiemThu" style="margin-top: 15px">
        <div style="margin-top: 15px; margin-left: 5px;">
            <table style="width: 100%;">
                <tr>
                    <td>
                        <cc2:OboutButton ID="OboutButton5" OnClientClick="themThongTinNghiemThu(); return false;"
                            runat="server" Text="Thêm" Width="100" FolderStyle="App_Themes/Styles/Interface/OboutButton">
                        </cc2:OboutButton>
                        <cc2:OboutButton ID="OboutButton6" OnClientClick="suaThongTinNghiemThu(); return false;"
                            runat="server" Text="Sửa" Width="100" FolderStyle="App_Themes/Styles/Interface/OboutButton">
                        </cc2:OboutButton>
                        <cc2:OboutButton ID="OboutButton7" OnClientClick="xoaThongTinNT(); return false;"
                            runat="server" Text="Xóa" Width="100" FolderStyle="App_Themes/Styles/Interface/OboutButton">
                        </cc2:OboutButton>
                    </td>
                    <td style="width: 400px;"></td>
                    <td align="right" style="width: 200px">
                        <div id="div3">
                            <input type="text" style="width: 250px; padding-left: 5px; margin-right: 9px;" placeholder="Nội dung tìm kiếm..."
                                onkeyup="searchValue(Grid1,0,this.value)"
                                class="searchCss" />
                        </div>
                    </td>
                </tr>
            </table>

        </div><!-- đã giải quyết giao diện-->
        <div style="border: 0px solid #DBDBE1; margin-left: 0px; margin: 4px; padding: 5px">
            <cc1:Grid ID="Grid1" runat="server" AutoGenerateColumns="False" PageSize="15" AllowPaging="true"
                AllowAddingRecords="false"
                Height="500" FolderStyle="~/App_Themes/Styles/style_7" OnRebind="Grid1_OnRebind"
                GroupBy="tenDuAn" AllowGrouping="true"
                AllowFiltering="true" ShowColumnsFooter="true" EnableRecordHover="true" OnRowDataBound="Grid1_RowDataBound"
                FilterType="ProgrammaticOnly"
                PageSizeOptions="10,15,20,50,100">
                <AddEditDeleteSettings AddLinksPosition="Top" />
                <PagingSettings Position="Bottom" />
                <ClientSideEvents OnClientDblClick="Grid1OnClientDblClick" />
                <FilteringSettings MatchingType="AnyFilter" FilterPosition="Top" FilterLinksPosition="Bottom" />
                <ScrollingSettings ScrollHeight="100%" EnableVirtualScrolling="false" ScrollWidth="979" />

                <Columns>
                    <cc1:Column HeaderText="Dự án" Width="390px" DataField="tenDuAn" Wrap="true"></cc1:Column>
                    <cc1:Column HeaderText="Số biên bản" Width="120px" DataField="soBienBan"></cc1:Column>
                    <cc1:Column HeaderText="Ngày lập" Width="120px" DataField="ngayLap"></cc1:Column>
                    <cc1:Column HeaderText="Số hợp đồng" Width="120px" DataField="soHopDong"></cc1:Column>
                    <cc1:Column HeaderText="Giá trị NT" Width="120px" DataField="giaTriNT" DataFormatString="{0:n0}"
                        Align="right">
                    </cc1:Column>
                    <cc1:Column HeaderText="Các căn cứ" Width="250px" DataField="cacCanCu"></cc1:Column>
                    <cc1:Column HeaderText="Xem biên bản" Width="120px" DataField="taiBienBan" Wrap="true" ParseHTML="true"></cc1:Column>
                    <cc1:Column HeaderText="" Width="0px" DataField="sttNghiemThupr" Visible="false">
                    </cc1:Column>
                    <cc1:Column HeaderText="" Width="0px" DataField="sttDuAnpr_sd" Visible="false"></cc1:Column>
                    <cc1:Column HeaderText="" Width="0px" DataField="maDuAn" Visible="false"></cc1:Column>
                    <cc1:Column HeaderText="" Width="0px" DataField="sttHopDongpr_sd" Visible="false">
                    </cc1:Column>
                    <cc1:Column HeaderText="" Width="0px" DataField="sttHopDongpr" Visible="false"></cc1:Column>
                </Columns>
                <LocalizationSettings CancelAllLink="Hủy tất cả" AddLink="Thêm mới" CancelLink="Hủy"
                    DeleteLink="Xóa" EditLink="Sửa" Filter_ApplyLink="Tìm kiếm" Filter_HideLink="Đóng tìm kiếm"
                    Filter_RemoveLink="Xóa tìm kiếm" Filter_ShowLink="Mở tìm kiếm" FilterCriteria_NoFilter="Không tìm kiếm"
                    FilterCriteria_Contains="Chứa" FilterCriteria_DoesNotContain="Không chứa" FilterCriteria_StartsWith="Bắt đầu với"
                    FilterCriteria_EndsWith="Kết thúc với" FilterCriteria_EqualTo="Bằng" FilterCriteria_NotEqualTo="Không bằng"
                    FilterCriteria_SmallerThan="Nhỏ hơn" FilterCriteria_GreaterThan="Lớn hơn" FilterCriteria_SmallerThanOrEqualTo="Nhỏ hơn hoặc bằng"
                    FilterCriteria_GreaterThanOrEqualTo="Lớn hơn hoặc bằng" FilterCriteria_IsNull="Rỗng"
                    FilterCriteria_IsNotNull="Không rỗng" FilterCriteria_IsEmpty="Trống" FilterCriteria_IsNotEmpty="Không trống"
                    Paging_OfText="của" Grouping_GroupingAreaText="Kéo tiêu đề cột vào đây để nhóm theo cột đó"
                    JSWarning="Có một lỗi khởi tạo lưới với ID '[GRID_ID]'. \ N \ n [Chú ý] \ n \ nHãy liên hệ bộ phận bảo trì của Nhất Tâm Soft để được giúp đỡ."
                    LoadingText="Đang tải...." MaxLengthValidationError="Giá trị mà bạn đã nhập vào trong cột XXXXX vượt quá số lượng tối đa ký tự YYYYY cho phép cột này."
                    ModifyLink="Chỉnh sửa" NoRecordsText="Không có dữ liệu" Paging_ManualPagingLink="Trang kế »"
                    Paging_PageSizeText="Số dòng 1 trang:" Paging_PagesText="Trang:" Paging_RecordsText="Dòng:"
                    Grouping_GroupInformationNextPage="" Grouping_GroupInformationPreviousPage=""
                    ResizingTooltipWidth="Rộng:" SaveAllLink="Lưu tất cả" SaveLink="Lưu" TypeValidationError="Giá trị mà bạn đã nhập vào trong cột XXXXX là không đúng."
                    UndeleteLink="Không xóa" UpdateLink="Lưu" />
            </cc1:Grid>
        </div><!-- đã giải quyết giao diện-->
    </div>
    <div id="thongTinNghiemThuCT">
        <div class="main-content-inner">
            <div class="page-content">
                <div class="row">
                    <div class="col-xs-12">
                        <div class="widget-box">
                            <div class="widget-body">
                                <div class="widget-main">
                                    <div id="fuelux-wizard-container">
                                        <div>
                                            <ul class="steps">
                                                <li data-step="1" class="active">
                                                    <span class="step">B1</span>
                                                    <span class="title">Mô tả tóm tắt dự án</span>
                                                </li>
                                                <li data-step="2">
                                                    <span class="step">B2</span>
                                                    <span class="title">Thông tin nghiệm thu</span>
                                                </li>
                                                <li data-step="3">
                                                    <span class="step">B3</span>
                                                    <span class="title">Đối tượng nghiệm thu</span>
                                                </li>
                                                <li data-step="4">
                                                    <span class="step">B4</span>
                                                    <span class="title">Văn bản</span>
                                                </li>
                                            </ul>
                                        </div>
                                        <div class="step-content pos-rel">
                                            <div id="step1" class="step-pane active" data-step="1">
                                                <uc4:thongtinduan_b1 ID="thongtinduan_b1" runat="server" />
                                            </div>
                                            <div id="step2" class="step-pane" data-step="2">
                                                <fieldset style="border: 1px solid #DBDBE1; margin-left: 0px; margin: 3px; padding: 4px">
                                                    <legend><b>Thông tin chung</b></legend>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td></td>
                                                            <td style="width: 180px"></td>
                                                            <td style="width: 120px"></td>
                                                            <td style="width: 60px"></td>
                                                            <td></td>
                                                            <td style="width: 180px"></td>
                                                            <td style="width: 70px"></td>
                                                            <td style="width: 65px"></td>
                                                            <td style="width: 30px"></td>
                                                            <td style="width: 15px"></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Hợp đồng, phụ lục 
                                                            </td>
                                                            <td colspan="8">
                                                                <cc3:ComboBox ID="cboHopDong" runat="server" Width="100%" MenuWidth="100%" FilterType="Contains"
                                                                    FolderStyle="App_Themes/Styles/Interface/OboutComboBox">
                                                                </cc3:ComboBox>
                                                            </td>
                                                            <td style="width: 15px">
                                                                <span style="color: Red">*</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 120px">Số biên bản 
                                                            </td>
                                                            <td colspan="2">
                                                                <cc2:OboutTextBox ID="soBienBan" runat="server" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox"></cc2:OboutTextBox>
                                                            </td>
                                                            <td colspan="2" style="width: 120px">
                                                                <span style="color: Red">*</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Ngày nghiệm
                                                                thu
                                                            </td>
                                                            <td colspan="3">
                                                                <cc2:OboutTextBox ID="ngayLap" runat="server" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox"></cc2:OboutTextBox>
                                                            </td>
                                                            <td>
                                                                <obout:Calendar ID="Calendar3" runat="server" DatePickerMode="true" TextBoxId="ngayLap"
                                                                    DateFormat="dd/MM/yyyy" TitleText="Chọn ngày" Columns="1" DatePickerImagePath="~/App_Themes/Styles/Date/date_picker1.gif"
                                                                    CultureName="vi-VN">
                                                                </obout:Calendar>
                                                            </td>
                                                            <td style="width: 15px">
                                                                <span style="color: Red">*</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Căn cứ nghiệm thu
                                                            </td>
                                                            <td colspan="9">
                                                                <cc2:OboutTextBox ID="canCuNghiemThu" TextMode="MultiLine" Height="50px" runat="server"
                                                                    FolderStyle="App_Themes/Styles/Interface/OboutTextBox" Width="100%"></cc2:OboutTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </div>
                                            <div id="step3" class="step-pane" data-step="3">
                                                <fieldset style="border: 1px solid #DBDBE1; margin-left: 0px; margin: 3px; padding: 4px">
                                                    <legend><b>Đối tượng nghiệm thu</b></legend>
                                                    <cc1:Grid ID="Grid2" runat="server" AutoGenerateColumns="False" PageSize="15" AllowPaging="true"
                                                        FilterType="ProgrammaticOnly"
                                                        Height="400" FolderStyle="~/App_Themes/Styles/style_7" OnRebind="Grid2_OnRebind"
                                                        OnRowDataBound="Grid2_RowDataBound"
                                                        AllowFiltering="true" ShowColumnsFooter="true" AllowAddingRecords="true" EnableRecordHover="true"
                                                        PageSizeOptions="10,15,20,50,100">
                                                        <AddEditDeleteSettings AddLinksPosition="Top" />
                                                        <ClientSideEvents OnClientDblClick="Grid2OnClientDblClick" OnBeforeClientAdd="truocKhiThem"
                                                            OnBeforeClientInsert="luuNghiemThuCT" OnClientEdit="truocKhiSua" OnBeforeClientEdit="truocKhiSua"
                                                            OnBeforeClientUpdate="luuNghiemThuCT" OnBeforeClientDelete="truocKhiXoa" />
                                                        <PagingSettings Position="Bottom" />
                                                        <FilteringSettings MatchingType="AnyFilter" FilterPosition="Top" FilterLinksPosition="Bottom" />
                                                        <ScrollingSettings ScrollHeight="100%" EnableVirtualScrolling="false" ScrollWidth="979" />
                                                        <Columns>
                                                            <cc1:Column AllowDelete="true" AllowEdit="true" HeaderText="Thao tác" Width="80px">
                                                            </cc1:Column>
                                                            <cc1:Column HeaderText="Tên công việc được nghiệm thu" Width="250px" DataField="tenCongViec">
                                                                <TemplateSettings EditTemplateId="etenCongViec" />
                                                            </cc1:Column>
                                                            <cc1:Column HeaderText="Giá trị thực hiện" Width="120px" DataField="giaTrungThau" Align="Right"
                                                                DataFormatString="{0:n0}">
                                                                <TemplateSettings EditTemplateId="editgiaTrungThau" />
                                                            </cc1:Column>
                                                            <cc1:Column HeaderText="Giá trị hợp đồng" Width="120px" DataField="giaTriHD" DataFormatString="{0:n0}"
                                                                Align="Right">
                                                                <TemplateSettings EditTemplateId="editgiaHopDong" />
                                                            </cc1:Column>
                                                            <cc1:Column HeaderText="Giá trị điều chỉnh" Width="120px" DataField="giaTriHDDC"
                                                                Align="Right"
                                                                DataFormatString="{0:n0}">
                                                                <TemplateSettings EditTemplateId="editgiaHopDongDC" />
                                                            </cc1:Column>
                                                            <cc1:Column HeaderText="Giá trị nghiệm thu" Width="120px" DataField="giaTriNghiemThu"
                                                                Align="Right"
                                                                DataFormatString="{0:n0}">
                                                                <TemplateSettings EditTemplateId="editgiaNghiemThu" />
                                                            </cc1:Column>
                                                            <cc1:Column HeaderText="Giá trị nghiệm thu LK" Width="120px" DataField="giaTriNghiemThuLK"
                                                                Align="Right" DataFormatString="{0:n0}">
                                                                <TemplateSettings EditTemplateId="editgiaNghiemThuLK" />
                                                            </cc1:Column>
                                                            <cc1:Column HeaderText="Thời gian" Width="120px" DataField="thoiGian"></cc1:Column>
                                                            <cc1:Column HeaderText="Tài liệu tham chiếu" Width="120px" DataField="taiLieu"></cc1:Column>
                                                            <cc1:Column HeaderText="Thay đổi so với thiết kế (nếu có)" Width="120px" DataField="thayDoi">
                                                            </cc1:Column>
                                                            <cc1:Column HeaderText="Kết luận" Width="120px" DataField="ketLuan">
                                                                <TemplateSettings EditTemplateId="editKetLuan" />
                                                            </cc1:Column>

                                                            <cc1:Column HeaderText="Yêu cầu khác" Width="120px" DataField="yeuCauKhac"></cc1:Column>
                                                            <cc1:Column HeaderText="" Width="0px" DataField="sttHDCTpr_sd" Visible="false"></cc1:Column>
                                                            <cc1:Column HeaderText="aaaa" Width="0px" DataField="maCPDauTuXDCTpr_sd" Visible="false">
                                                            </cc1:Column>
                                                            <cc1:Column HeaderText="" Width="0px" DataField="loai" Visible="false"></cc1:Column>
                                                            <cc1:Column HeaderText="" Width="0px" DataField="sttNghiemThuCTpr" Visible="false">
                                                            </cc1:Column>
                                                        </Columns>
                                                        <LocalizationSettings CancelAllLink="Hủy tất cả" AddLink="Thêm mới" CancelLink="Hủy"
                                                            DeleteLink="Xóa" EditLink="Sửa" Filter_ApplyLink="Tìm kiếm" Filter_HideLink="Đóng tìm kiếm"
                                                            Filter_RemoveLink="Xóa tìm kiếm" Filter_ShowLink="Mở tìm kiếm" FilterCriteria_NoFilter="Không tìm kiếm"
                                                            FilterCriteria_Contains="Chứa" FilterCriteria_DoesNotContain="Không chứa" FilterCriteria_StartsWith="Bắt đầu với"
                                                            FilterCriteria_EndsWith="Kết thúc với" FilterCriteria_EqualTo="Bằng" FilterCriteria_NotEqualTo="Không bằng"
                                                            FilterCriteria_SmallerThan="Nhỏ hơn" FilterCriteria_GreaterThan="Lớn hơn" FilterCriteria_SmallerThanOrEqualTo="Nhỏ hơn hoặc bằng"
                                                            FilterCriteria_GreaterThanOrEqualTo="Lớn hơn hoặc bằng" FilterCriteria_IsNull="Rỗng"
                                                            FilterCriteria_IsNotNull="Không rỗng" FilterCriteria_IsEmpty="Trống" FilterCriteria_IsNotEmpty="Không trống"
                                                            Paging_OfText="của" Grouping_GroupingAreaText="Kéo tiêu đề cột vào đây để loại theo cột đó"
                                                            JSWarning="Có một lỗi khởi tạo lưới với ID '[GRID_ID]'. \ N \ n [Chú ý] \ n \ nHãy liên hệ bộ phận bảo trì của Nhất Tâm Soft để được giúp đỡ."
                                                            LoadingText="Đang tải...." MaxLengthValidationError="Giá trị mà bạn đã nhập vào trong cột XXXXX vượt quá số lượng tối đa ký tự YYYYY cho phép cột này."
                                                            ModifyLink="Chỉnh sửa" NoRecordsText="Không có dữ liệu" Paging_ManualPagingLink="Trang kế »"
                                                            Paging_PageSizeText="Số dòng" Paging_PagesText="Trang:" Paging_RecordsText="Dòng:"
                                                            ResizingTooltipWidth="Rộng:" SaveAllLink="Lưu tất cả" SaveLink="Lưu" TypeValidationError="Giá trị mà bạn đã nhập vào trong cột XXXXX là không đúng."
                                                            UndeleteLink="Không xóa" UpdateLink="Lưu" />
                                                        <TemplateSettings HeadingTemplateId="GridTemplate2" />
                                                        <Templates>
                                                            <cc1:GridTemplate runat="server" ID="GridTemplate2">
                                                                <Template>
                                                                    <input type="text" style="width: 250px; padding-left: 5px; margin-right: -20px;"
                                                                        placeholder="Nội dung tìm kiếm..." onkeyup="searchValue(Grid2,1,this.value)"
                                                                        class="searchCss" />
                                                                </Template>
                                                            </cc1:GridTemplate>
                                                            <cc1:GridTemplate runat="server" ControlID="cboCongViec" ID="etenCongViec" ControlPropertyName="value">
                                                                <Template>
                                                                    <cc3:ComboBox ID="cboCongViec" runat="server" AppendDataBoundItems="false" Width="100%"
                                                                        FilterType="Contains"
                                                                        FolderStyle="~/App_Themes/Styles/Interface/OboutComboBox" Height="100px" MenuWidth="150%"
                                                                        EnableVirtualScrolling="false">
                                                                        <ClientSideEvents OnSelectedIndexChanged="changeCongViec" />
                                                                    </cc3:ComboBox>
                                                                </Template>
                                                            </cc1:GridTemplate>
                                                            <cc1:GridTemplate runat="server" ControlID="eKetLuan" ID="editKetLuan" ControlPropertyName="value">
                                                                <Template>
                                                                    <cc3:ComboBox ID="eKetLuan" runat="server" AppendDataBoundItems="false" Width="100%"
                                                                        FilterType="Contains"
                                                                        FolderStyle="~/App_Themes/Styles/Interface/OboutComboBox" Height="50px" MenuWidth="150%"
                                                                        EnableVirtualScrolling="false">
                                                                    </cc3:ComboBox>
                                                                </Template>
                                                            </cc1:GridTemplate>
                                                            <cc1:GridTemplate runat="server" ControlID="egiaTrungThau" ID="editgiaTrungThau"
                                                                ControlPropertyName="value">
                                                                <Template>
                                                                    <cc2:OboutTextBox ID="egiaTrungThau" runat="server" Enabled="false" Width="100%"></cc2:OboutTextBox>
                                                                </Template>
                                                            </cc1:GridTemplate>
                                                            <cc1:GridTemplate runat="server" ControlID="egiaHopDong" ID="editgiaHopDong" ControlPropertyName="value">
                                                                <Template>
                                                                    <cc2:OboutTextBox ID="egiaHopDong" runat="server" Enabled="false" Width="100%"></cc2:OboutTextBox>
                                                                </Template>
                                                            </cc1:GridTemplate>
                                                            <cc1:GridTemplate runat="server" ControlID="egiaHopDongDC" ID="editgiaHopDongDC"
                                                                ControlPropertyName="value">
                                                                <Template>
                                                                    <cc2:OboutTextBox ID="egiaHopDongDC" runat="server" Enabled="false" Width="100%"></cc2:OboutTextBox>
                                                                </Template>
                                                            </cc1:GridTemplate>

                                                            <cc1:GridTemplate runat="server" ControlID="egiaNghiemThu" ID="editgiaNghiemThu"
                                                                ControlPropertyName="value">
                                                                <Template>
                                                                    <cc2:OboutTextBox ID="egiaNghiemThu" runat="server" Width="100%"></cc2:OboutTextBox>
                                                                </Template>
                                                            </cc1:GridTemplate>

                                                            <cc1:GridTemplate runat="server" ControlID="giaNghiemThuLK" ID="editgiaNghiemThuLK"
                                                                ControlPropertyName="value">
                                                                <Template>
                                                                    <cc2:OboutTextBox ID="giaNghiemThuLK" runat="server" Enabled="false" Width="100%"></cc2:OboutTextBox>
                                                                </Template>
                                                            </cc1:GridTemplate>

                                                        </Templates>
                                                    </cc1:Grid>
                                                </fieldset>
                                            </div>
                                            <div id="step4" class="step-pane" data-step="4">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 100px">Số văn bản</td>
                                                        <td style="width: 300px">
                                                            <cc2:OboutTextBox ID="txtSoVanBan_uc" runat="server" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox">
                                                            </cc2:OboutTextBox>
                                                        </td>
                                                        <td style="width: 100px">Ngày ký</td>
                                                        <td style="width: 300px">
                                                            <cc2:OboutTextBox ID="txtNgayKy_uc" runat="server" Width="35%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox">
                                                            </cc2:OboutTextBox>
                                                            <obout:Calendar ID="Calendar_uc" runat="server" DatePickerMode="true" TextBoxId="txtNgayKy_uc"
                                                                DateFormat="dd/MM/yyyy" TitleText="Chọn ngày" Columns="1" DatePickerImagePath="~/App_Themes/Styles/Date/date_picker1.gif"
                                                                CultureName="vi-VN">
                                                            </obout:Calendar>
                                                        </td>
                                                        <td style="width: 10px"></td>
                                                    </tr>


                                                    <tr>
                                                        <td style="width: 120px">Nội dung</td>
                                                        <td colspan="3">
                                                            <cc2:OboutTextBox ID="txtNoiDung_uc" TextMode="MultiLine" runat="server" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox">
                                                            </cc2:OboutTextBox>
                                                        </td>
                                                        <td style="width: 10px"></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 120px">Giá trị</td>
                                                        <td>
                                                            <cc2:OboutTextBox ID="txtGiaTri_uc" runat="server" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox">
                                                                <ClientSideEvents OnTextChanged="dinhDangSo"/>
                                                            </cc2:OboutTextBox>
                                                        </td>
                                                        <td colspan="2">
                                                            <table style="width: 460px; display: contents">
                                                                <tr>
                                                                    <td style="width: 300px">
                                                                        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager" runat="server">
                                                                        </ajaxToolkit:ToolkitScriptManager>
                                                                        <ajaxToolkit:AsyncFileUpload OnClientUploadError="uploadError" OnClientUploadComplete="uploadComplete"
                                                                            runat="server" ID="AsyncFileUpload" Width="390px" UploaderStyle="Traditional"
                                                                            OnClientUploadStarted="uploadStarted" UploadingBackColor="" ThrobberID="myThrobber"
                                                                            BorderStyle="NotSet" Font-Underline="False" Font-Strikeout="False" />
                                                                        <br />
                                                                        <div>
                                                                            <span style="color: Red;"><span><i>Chỉ cho phép tải lên tập tin định dạng: *.doc, *.docx,*.pdf</i></span></span>
                                                                        </div>
                                                                    </td>
                                                                    <td style="width: 170px" id="tdNoiDung"></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="width: 10px"></td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                    <hr class="hr" />
                                    <div class="wizard-actions">
                                        <button id="btnLui" type="button" class="btn btn-primary btn-prev" style="height: 30px;">
                                            <i class="ace-icon fa fa-arrow-left"></i>
                                            Quay lại
                                        </button>

                                        <button id="btnTiep" type="button" class="btn btn-primary btn-next" style="height: 30px;"
                                            data-last="Kết thúc">
                                            Tiếp theo
													<i class="ace-icon fa fa-arrow-right icon-on-right"></i>
                                        </button>
                                    </div>
                                </div>
                            </div>
                            <!-- /.widget-body -->
                        </div>
                    </div>
                    <!-- /.col -->
                </div>
                <!-- /.row -->
            </div>
            <!-- /.page-content -->
        </div>
        <script>
            function uploadError(sender, args) {
                alert("Tải tập tin thất bại");
            }
            function uploadComplete(sender, args) {
                var fileExtension = args.get_fileName();
                var ketqua = ProOnline.quanly.nghiemthuhopdong.layThongTinVanBanNT(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttNT").value);
                document.getElementById('tdNoiDung').innerHTML = (ketqua.value.Rows[0].duongDanFile_ == "" ? "<p style=\"color:red;text-decoration:none\">Chưa đính kèm</p>" : "<a style=\"color:Blue;text-decoration:none\" onclick=\"xemVB('" + ketqua.value.Rows[0].duongDanFile_ + "')\" href=\"#\">Xem đính kèm</a>");

            }
            function uploadStarted(sender, args) {
                var fileExtension = args.get_fileName();
                if (fileExtension.indexOf('.pdf') != -1 || fileExtension.indexOf('.PDF') != -1 || fileExtension.indexOf('.doc') != -1 || fileExtension.indexOf('.DOC') != -1 || fileExtension.indexOf('.docx') != -1 || fileExtension.indexOf('.DOCX') != -1) {
                }
                else {
                    alert("Tập tin không đúng định dạng.");
                    return;
                }
            }
            function truocKhiThem(record) {
                debugger;
                if (checkEmtyValue(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttNT").value)) {
                    alert("Bạn phải lưu thông tin trước!");
                    return false;
                }
                if (ProOnline.quanly.nghiemthuhopdong.checkDuyetQToan(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value).value == "1") {
                    alert("Dự án đã được duyệt quyết toán, bạn không thể thực hiện thao tác này!");
                    return false;
                }
                coThaoTac = "0";
                sttNTCT = "";
                loadDanhSachCongViec();
            }
            var sttNTCT = "";
            function truocKhiSua(record) {
                debugger;
                if (checkEmtyValue(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttNT").value)) {
                    alert("Bạn phải lưu thông tin trước!");
                    return false;
                }
                if (ProOnline.quanly.nghiemthuhopdong.checkDuyetQToan(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value).value == "1") {
                    alert("Dự án đã được duyệt quyết toán, bạn không thể thực hiện thao tác này!");
                    return false;
                }
                loadDanhSachCongViec();
                cboCongViec.value(record.loai + '@' + record.sttHDCTpr_sd + '@' + record.maCPDauTuXDCTpr_sd);
                // changeCongViec();
                egiaTrungThau.value(formatNumber(record.giaTrungThau));
                egiaHopDong.value(formatNumber(record.giaTriHD));
                egiaHopDongDC.value(formatNumber(record.giaTriHDDC));
                egiaNghiemThu.value(formatNumber(record.giaTriNghiemThu));
                sttNTCT = record.sttNghiemThuCTpr;
                eKetLuan.value(record.ketLuan);
                giaNghiemThuLK.value('');
                coThaoTac = "1";
            }
            function luuNghiemThuCT(record) {
                debugger;
                var param = new Array();
                param[0] = sttNTCT;
                param[1] = document.getElementById("ctl00_ContentPlaceHolder1_hdfSttNT").value;
                param[2] = cboCongViec.value();
                param[3] = egiaNghiemThu.value();
                param[4] = record.thoiGian;
                param[5] = record.taiLieu;
                param[6] = record.thayDoi
                param[7] = record.ketLuan;
                param[8] = record.yeuCauKhac;
                param[9] = cboHopDong.value();
                if (param[2] == "") {
                    alert("Bạn chưa chọn công việc nghiệm thu");
                    return false;
                }
                if (isNaN(param[3].replaceAll(".", "").replaceAll(",", ""))) {
                    alert("Giá trị nghiệm thu phải là kiểu số");
                    return false;
                }
                var result = ProOnline.quanly.nghiemthuhopdong.luuThongTinNghiemThuCT(param, coThaoTac);
                if (result.value == "0") {
                    alert("Thông tin nghiệm thu chi tiết chưa được lưu");
                    return false;
                }
                Grid2.refresh();
                if (ProOnline.quanly.nghiemthuhopdong.SoDongGrid2().value == "0") {
                    cboHopDong.enable();
                } else { cboHopDong.disable(); }
                return false;
            }
            //Xóa lưới trong
            function truocKhiXoa(record) {
                debugger;
                if (checkEmtyValue(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttNT").value)) {
                    alert("Bạn phải lưu thông tin trước!");
                    return false;
                }
                if (ProOnline.quanly.nghiemthuhopdong.checkDuyetQToan(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value).value == "1") {
                    alert("Dự án đã được duyệt quyết toán, bạn không thể thực hiện thao tác này!");
                    return false;
                }
                coThaoTac = "1";
                sttXoa = record.sttNghiemThuCTpr;
                Dialog3.Open();
                return false;
            }
            function Grid2OnClientDblClick(record) {
                debugger;
                if (checkEmtyValue(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttNT").value)) {
                    alert("Bạn phải lưu thông tin trước!");
                    return false;
                }
                if (ProOnline.quanly.nghiemthuhopdong.checkDuyetQToan(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value).value == "1") {
                    alert("Dự án đã được duyệt quyết toán, bạn không thể thực hiện thao tác này!");
                    return false;
                }
                Grid2.editRecord(record);
                sttNTCT = Grid2.Rows[record].Cells["sttNghiemThuCTpr"].Value;
                eKetLuan.value(Grid2.Rows[record].Cells["ketLuan"].Value);
                cboCongViec.value(Grid2.Rows[record].Cells["loai"].Value + '@' + Grid2.Rows[record].Cells["sttHDCTpr_sd"].Value + '@' + Grid2.Rows[record].Cells["maCPDauTuXDCTpr_sd"].Value);
                changeCongViec();
                coThaoTac = "1";
            }
            var sttXoa = "";
        </script>

        <owd:Window ID="Window3" runat="server" IsModal="true" ShowCloseButton="true" Status=""
            ShowStatusBar="false" RelativeElementID="WindowPositionHelper" Height="500" Width="800"
            VisibleOnLoad="false" StyleFolder="~/App_Themes/Styles/wdstyles/dogma" Title="Chọn dự án"
            IsResizable="false">
            <div style="margin-top: 2px">
                <table>
                    <tr>
                        <td>
                            <cc2:OboutButton ID="OboutButton1" runat="server" Text="Chọn" Width="80px" OnClientClick="chonDuAn();return false;"
                                FolderStyle="~/App_Themes/Styles/Interface/OboutButton">
                            </cc2:OboutButton>
                        </td>
                        <td>
                            <cc2:OboutButton ID="OboutButton2" runat="server" Text="Đóng" Width="80px" OnClientClick="Window3.Close(); return false;">
                            </cc2:OboutButton>
                        </td>
                        <td style="width: 400px"></td>
                        <td align="right">
                            <td align="right">
                                <input type="text" style="width: 250px" placeholder="Nội dung tìm kiếm..." onkeyup="searchValue(Grid3,1,this.value)"
                                    class="searchCss" />
                            </td>
                        </td>
                    </tr>
                </table>
            </div>

        </owd:Window>
        <script>
            function xoaNghiemThu() {
                debugger;
                var result = ProOnline.quanly.nghiemthuhopdong.xoaThongTin(sttXoa, coThaoTac);
                if (result.value == "0") {
                    alert("Thông tin chưa được xóa");
                    return false;
                }
                Dialog3.Close();
                if (coThaoTac == "0")
                    Grid1.refresh();
                else {
                    Grid2.refresh();
                    if (ProOnline.quanly.nghiemthuhopdong.SoDongGrid2().value == "0") {
                        cboHopDong.enable();
                        coThaoTac = "1";
                        return false;
                    }
                }
                coThaoTac = "";
                sttXoa = "";
                sttNghiemThupr = "";
                sttNTCT = "";
            }
        </script>
    </div>
    <owd:Dialog ID="Dialog3" runat="server" IsModal="true" ShowCloseButton="true" Top="0"
        Left="250" Height="150" Width="350" VisibleOnLoad="false" StyleFolder="~/App_Themes/Styles/wdstyles/dogma"
        Title="Cảnh báo">
        <div align="center" style="height: 50px; margin-top: 15px">
            Bạn có thật sự muốn xóa thông tin này? Đồng ý xóa chọn 'Thực hiện', không đồng
            ý chọn 'Bỏ qua'.
        </div>
        <div align="center">
            <table>
                <tr>
                    <td>
                        <cc2:OboutButton ID="btnXoaNghiemTu" runat="server" Text="Thực hiện" Width="90px"
                            OnClientClick="xoaNghiemThu(); return false;"
                            FolderStyle="App_Themes/Styles/Interface/OboutButton">
                        </cc2:OboutButton>
                    </td>
                    <td>
                        <cc2:OboutButton ID="btnHuyXoa" runat="server" Text="Bỏ qua" Width="90px" OnClientClick="Dialog3.Close(); return false;">
                        </cc2:OboutButton>
                    </td>
                </tr>
            </table>
        </div>
    </owd:Dialog>
    <script type="text/javascript" src="/App_Themes/BuocThaoTac/js/jquery-2.1.4.min.js"></script>
    <script type="text/javascript" src="/App_Themes/BuocThaoTac/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="/App_Themes/BuocThaoTac/js/ace.min.js"></script>
    <script type="text/javascript" src="/App_Themes/BuocThaoTac/js/wizard.min.js"></script>
    <script type="text/javascript" src="/App_Themes/BuocThaoTac/js/ace-elements.min.js"></script>
    <script type="text/javascript">
        jQuery(function ($) {
            $('[data-rel=tooltip]').tooltip();
            var $validation = true;
            $('#fuelux-wizard-container')
            .ace_wizard({
                step: 1, //optional argument. wizard will jump to step "2" at first
                buttons: '.wizard-actions:eq(0)'
            })
            .on('actionclicked.fu.wizard', function (e, info) {
                debugger;
                if (!tmp_click && info.step == 1) {
                    if (document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value == "" || document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value == "0") {
                        alert("Chọn dự án trước khi thực hiện bước tiếp theo!");
                        e.preventDefault();
                    }
                    if (ntsLibraryFunctions.checkDuyetQT(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value).value == "1") {
                        cboHopDong.disable();
                        canCuNghiemThu.disable();
                        soBienBan.disable();
                        ngayLap.disable();

                        txtSoVanBan_uc.disable();
                        txtNgayKy_uc.disable();
                        //cboCoQuanBanHanh_uc.disable();
                        //cboNguoiKy_uc.disable();
                        //txtChucDanh_uc.disable();
                        //cboLoaiVanBan_uc.disable();
                        txtNoiDung_uc.disable();
                    }
                    else {
                        txtSoVanBan_uc.enable();
                        txtNgayKy_uc.enable();
                        //cboCoQuanBanHanh_uc.enable();
                        //cboNguoiKy_uc.enable();
                        //txtChucDanh_uc.enable();
                        txtNoiDung_uc.enable();
                    }
                    if (isEmtyValue(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttNT").value)) {
                        loadHopDong();
                    }

                }
                if (!tmp_click && info.step == 2) {
                    debugger;
                    if (document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value == "" || document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value == "0") {
                        alert("Không tồn tại dự án để thực hiện kết thúc quá trình thao tác!");
                        e.preventDefault();
                    } else {
                        var param = new Array();
                        param[0] = document.getElementById("ctl00_ContentPlaceHolder1_hdfSttNT").value;
                        param[1] = document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value;
                        param[2] = cboHopDong.value();
                        param[3] = canCuNghiemThu.value();
                        param[4] = soBienBan.value();
                        param[5] = ngayLap.value();
                        if (document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value == "") {
                            alert("Bạn chưa chọn dự án");
                            return false;
                        }
                        if (param[2] == "") {
                            alert("Bạn chưa chọn hợp đồng hoặc phụ lục HĐ cần nghiệm thu");
                            return false;
                        }
                        if (param[4] == "") {
                            alert("Bạn chưa nhập số biên bản");
                            return false;
                        }
                        var ktNgay = ProOnline.quanly.nghiemthuhopdong.kiemTraNgay(param[5]);
                        if (ktNgay.value == false) {
                            alert("Ngày lập bảng kê phải có 10 ký tự dạng dd/MM/yyyy");
                            setTimeout(function () { ngayLap.focus(); }, 1);
                            return false;
                        }
                        var result = ProOnline.quanly.nghiemthuhopdong.luuThongTin(param, coThaoTac);
                        if (result.value.split('_')[0] == "0") {
                            alert(result.value.split('_')[1]);
                            e.preventDefault();
                            return false;
                        }
                        if (coThaoTac == "0")
                            document.getElementById("ctl00_ContentPlaceHolder1_hdfSttNT").value = result.value.split('_')[0];
                        if (!isEmtyValue(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttNT").value))
                            coThaoTac = "1";
                        daLuu = "1";
                        loadDanhSachCongViec();
                        //document.getElementById("ctl00_ContentPlaceHolder1_vanban_buocketthuc_sttVBDApr").value = document.getElementById("ctl00_ContentPlaceHolder1_hdfSttVBDApr_uc").value = ProOnline.quanly.nghiemthuhopdong.getSttVanBan(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttNT").value).value;
                        Grid2.refresh();
                    }
                } else { Grid2.refresh(); }
                if (!tmp_click && info.step == 3) {
                    debugger;
                    if (document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value == "" || document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value == "0") {
                        alert("Không tồn tại dự án để thực hiện kết thúc quá trình thao tác!");
                        e.preventDefault();
                    }
                    else if (document.getElementById("ctl00_ContentPlaceHolder1_hdfSttNT").value == "" || document.getElementById("ctl00_ContentPlaceHolder1_hdfSttNT").value == "0") {
                        alert("Không tồn tại thông tin chứng từ để thực hiện bước tiếp theo!");
                        e.preventDefault();
                    }
                    //loadThongTinVanBan_uc(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttVBDApr_uc").value);
                    //txtGiaTri_uc.value(formatNumber(ProOnline.quanly.nghiemthuhopdong.capNhatGiaTriVanBan(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttNT").value).value));
                    //cboLoaiVanBan_uc.value('0902');
                    //lấy thông tin văn bản nghiệm thu
                    var ketqua = ProOnline.quanly.nghiemthuhopdong.layThongTinVanBanNT(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttNT").value);
                    txtSoVanBan_uc.value(ketqua.value.Rows[0].soBienBan);
                    txtNgayKy_uc.value(ketqua.value.Rows[0].ngay);
                    txtGiaTri_uc.value(ketqua.value.Rows[0].giaTri_);
                    txtNoiDung_uc.value(ketqua.value.Rows[0].noiDung);
                    txtSoVanBan_uc.disable();
                    txtNgayKy_uc.disable();
                    document.getElementById('tdNoiDung').innerHTML = (ketqua.value.Rows[0].duongDanFile_ == "" ? "<p style=\"color:red;text-decoration:none\">Chưa đính kèm</p>" : "<a style=\"color:Blue;text-decoration:none\" onclick=\"xemVB('" + ketqua.value.Rows[0].duongDanFile_ + "')\" href=\"#\">Xem đính kèm</a>");
                }
            })
            .on('finished.fu.wizard', function (e) {
                debugger;
                var wizard = $('#fuelux-wizard-container').data('fu.wizard');
                if (document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value == "" || document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value == "0") {
                    alert("Không tồn tại dự án để thực hiện kết thúc quá trình thao tác");
                    return false;
                }
                var param = new Array();
                param[0] = document.getElementById("ctl00_ContentPlaceHolder1_hdfSttNT").value;
                param[1] = txtNoiDung_uc.value();
                param[2] = txtGiaTri_uc.value();
                var ketqua_luuvb = ProOnline.quanly.nghiemthuhopdong.luuThongTinVanBanNT(param).value;
                if (ketqua_luuvb == true) {
                    wizard.currentStep = 1;
                    wizard.setState();
                    HienThiControl("thongTinNghiemThu", true);
                    HienThiControl("thongTinNghiemThuCT", false);
                    alert("Bạn đã lập xong biên bản nghiệm thu!");
                    Grid1.refresh();
                }
            });
        });
        var buoc;
        var tmp_click;
        $('#btnLui').on('click', function (e) {
            debugger;
            tmp_click = true;
            var dem = $('.step-content').find('.active').attr('data-step');
            if (dem + "" == "1") {
                HienThiControl("thongTinNghiemThu", true);
                HienThiControl("thongTinNghiemThuCT", false);
                Grid1.refresh();
            }
        });
        $('#btnTiep').click(function (e) {
            debugger;
            tmp_click = false;
        });
        function xemVB(url) {
            debugger;
            window.open(url);
            return false;
        }
    </script>
</asp:Content>

