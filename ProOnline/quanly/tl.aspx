<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="tl.aspx.cs" Inherits="ProOnline.quanly.tl" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>
<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc3" %>
<%@ Register TagPrefix="oem" Namespace="OboutInc.EasyMenu_Pro" Assembly="obout_EasyMenu_Pro" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="cc2" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Window_NET" Namespace="OboutInc.Window" TagPrefix="owd" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/module/thongtinduan_b1.ascx" TagName="thongtinduan_b1" TagPrefix="uc4" %>
<%@ Register Src="~/module/vanban_buocketthuc.ascx" TagName="vanban_buocketthuc" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/App_Themes/BuocThaoTac/css/ace.min.css" />
    <link href="/App_Themes/BuocThaoTac/font-awesome/4.5.0/css/font-awesome.min.css"
        rel="stylesheet" type="text/css" />
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
    <style type="text/css">
        /* Show Popup*/ /* Outer */

        .popup {
            width: 100%;
            height: 100%;
            display: none;
            position: fixed;
            top: 0px;
            left: 0px;
            background: rgba(0,0,0,0.75);
            z-index: 9999999999;
        }
        /* Inner */ .popup-inner {
            max-width: 960px;
            width: 90%;
            padding: 10px 20px 10px 20px;
            position: absolute;
            top: 50%;
            left: 50%;
            -webkit-transform: translate(-50%, -50%);
            transform: translate(-50%, -50%);
            box-shadow: 0px 2px 6px rgba(0,0,0,1);
            border-radius: 3px;
            background: #fff;
            height: 600px;
        }
        /* Close Button */ .popup-close {
            width: 30px;
            height: 30px;
            padding-top: 8px;
            display: inline-block;
            position: absolute;
            top: 10px;
            right: 10px;
            transition: ease 0.25s all;
            -webkit-transform: translate(50%, -50%);
            transform: translate(50%, -50%);
            border-radius: 1000px;
            background: rgba(0,0,0,0.8);
            font-family: Arial, Sans-Serif;
            font-size: 20px;
            text-align: center;
            line-height: 100%;
            color: #fff;
        }

            .popup-close:hover {
                -webkit-transform: translate(50%, -50%) rotate(180deg);
                transform: translate(50%, -50%) rotate(180deg);
                background: rgba(0,0,0,1);
                text-decoration: none;
            }

        .item {
            position: relative !important;
            display: -moz-inline-stack;
            display: inline-block;
            zoom: 1;
            *display: inline;
            overflow: hidden;
            white-space: nowrap;
        }
    </style>
    <style type="text/css">
        .searchCss
        {
            border-top: 1px solid #CCC;
            border-left: 1px solid #CCC;
            border-right: 1px solid #CCC;
            border-bottom: 1px solid #CCC;
            height: 23px;
        }
        .searchCss:focus
        {
            border-top: 1px solid #009DCC;
            border-left: 1px solid #009DCC;
            border-right: 1px solid #009DCC;
            border-bottom: 1px solid #009DCC;
        }
        .searchCss:hover
        {
            border-top: 1px solid #009DCC;
            border-left: 1px solid #009DCC;
            border-right: 1px solid #009DCC;
            border-bottom: 1px solid #009DCC;
        }
        .thonTin
        {
            font-size: 13px;
            font-weight: bold;
        }
    </style>
    <script type="text/javascript">
        var coThaoTac = "0";
        var sttThanhLypr = "";
        var daLuu = "0";
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
        window.onload = function () {
            if ("<%= ProOnline.Class.ntsLibraryFunctions.phanQuyen() %>" == "true") {
                OboutButton5.disable();
                OboutButton6.disable();
                OboutButton7.disable();
            }
            Grid1.refresh();
            Grid2.refresh();
        }
        function luuThongTin() {
            if (ProOnline.quanly.tl.checkDuyetQToan(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDA").value).value == "1") {
                alert("Dự án đã được duyệt quyết toán, bạn không thể thực hiện thao tác này!");
                return false;
            }
            var param = new Array();
            param[0] = document.getElementById("ctl00_ContentPlaceHolder1_hdfSttTL").value;
            param[1] = document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDA").value;
            param[2] = cboHopDong.value()
            param[3] = cboHopDong.value()
            param[4] = soBienBan.value()
            param[5] = ngayLap.value()
            param[6] = cacCanCu.value();
            param[7] = daiDienCDT.value();
            param[8] = chucVuCDT.value();
            param[9] = daiDienNT.value();
            param[10] = chucVuNT.value();
            param[11] = soHoaDon.value();
            param[12] = ngayHoaDon.value();
            if (document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDA").value == "") {
                alert("Bạn chưa chọn dự án");
                return false;
            }
            if (param[3] == "") {
                alert("Bạn chưa chọn hợp đồng hoặc phụ lục HĐ cần thanh lý");
                return false;
            }
            if (param[4] == "") {
                alert("Bạn chưa nhập số biên bản");
                return false;
            }
            var ktNgay = ProOnline.quanly.tl.kiemTraNgay(param[5]);
            if (ktNgay.value == false) {
                alert("Ngày lập bảng kê phải có 10 ký tự dạng dd/MM/yyyy");
                setTimeout(function () { ngayLap.focus(); }, 1);
                return false;
            }
            if (param[12] != "") {
                var ktNgay = ProOnline.quanly.tl.kiemTraNgay(param[12]);
                if (ktNgay.value == false) {
                    alert("Ngày hóa đơn phải có 10 ký tự dạng dd/MM/yyyy");
                    setTimeout(function () { ngayLap.focus(); }, 1);
                    return false;
                }
            }
            var result = ProOnline.quanly.tl.luuThongTin(param, coThaoTac);
            if (result.value == "0") {
                alert("Thông tin thanh lý chưa được lưu");
                return false;
            }
            if (coThaoTac == "0") {
                document.getElementById("ctl00_ContentPlaceHolder1_hdfSttTL").value = result.value;

            }
            daLuu = "1";
            anHienControl("l");
            return false;
        }
        function loadDanhSachCongViec(a, b, c) {
            debugger;
            var result = ProOnline.quanly.tl.getDanhSachCongViec(a, b, c);
            cboCongViec.options.clear();
            for (var i = 0; i < result.value.Rows.length; i++) {
                var noiDung = result.value.Rows[i].noiDung;
                var ma = result.value.Rows[i].stt;
                cboCongViec.options.add(noiDung, ma, i);
            }
        }
        function changeCongViec() {
            var result = ProOnline.quanly.tl.thongTinCongViec(cboCongViec.value(), coThaoTac);
            if (result.value.Rows.length > 0) {
                eKhoiLuong.value(result.value.Rows[0].khoiLuong);
                eDonGia.value(format(result.value.Rows[0].donGia));
                eThanhTien.value(format(result.value.Rows[0].thanhTien));
                eGiaTriHD.value(format(result.value.Rows[0].giaTriHD));
            }
            else {
                eKhoiLuong.value('');
                eDonGia.value('');
                eThanhTien.value('');
                eGiaTriHD.value('');
            }
            return false;
        }
        function anHienControl(flag) {
            if (flag == "t") {
                btnchonDuAn.enable();
                soBienBan.value('');
                ngayLap.value('');
                cacCanCu.value('');
                daiDienCDT.value('');
                chucVuCDT.value('');
                daiDienNT.value('');
                chucVuNT.value('');
                soHoaDon.value('');
                ngayHoaDon.value('');
                soTienTU_TT.value('');
                soTienChuaTT.value('');
                cboHopDong.enable();
                soBienBan.enable();
                ngayLap.enable();
                cacCanCu.enable();
                daiDienCDT.enable();
                chucVuCDT.enable();
                daiDienNT.enable();
                chucVuNT.enable();
                soHoaDon.enable();
                ngayHoaDon.enable();
                btnchonDuAn.enable();
                btnLuu.enable();
                btnThanhLy.disable();
                btnSua.disable();
                document.getElementById("thongTinThanhLyCT").style.visibility = "visible";
                document.getElementById("thongTinThanhLyCT").style.display = "Block";
                document.getElementById("thongTinThanhLy").style.visibility = "hidden";
                document.getElementById("thongTinThanhLy").style.display = "none";
                document.getElementById("ctl00_ContentPlaceHolder1_hdfSttTL").value = "0";
                //cboHopDong.options.clear();
                cboHopDong.selectedIndex(-1);
                giaTriHopDong.value("");
                daLuu = "";
                sttThanhLypr = "";
            }
            if (flag == "s") {
                Grid2.refresh();
                cboHopDong.disable();
                soBienBan.enable();
                ngayLap.enable();
                cacCanCu.enable();
                daiDienCDT.enable();
                chucVuCDT.enable();
                daiDienNT.enable();
                chucVuNT.enable();
                soHoaDon.enable();
                ngayHoaDon.enable();
                btnchonDuAn.enable();
                btnLuu.enable();
                btnchonDuAn.disable();
                btnThanhLy.disable();
                btnSua.disable();
                document.getElementById("thongTinThanhLyCT").style.visibility = "visible";
                document.getElementById("thongTinThanhLyCT").style.display = "Block";
                document.getElementById("thongTinThanhLy").style.visibility = "hidden";
                document.getElementById("thongTinThanhLy").style.display = "none";
                daLuu = "";
            }
            if (flag == "l") {
                Grid2.refresh();
                cboHopDong.disable();
                soBienBan.disable();
                ngayLap.disable();
                cacCanCu.disable();
                daiDienCDT.disable();
                chucVuCDT.disable();
                daiDienNT.disable();
                chucVuNT.disable();
                soHoaDon.disable();
                ngayHoaDon.disable();
                btnLuu.disable();
                btnSua.enable();
                btnchonDuAn.disable();
                btnThanhLy.enable();
                daLuu = "1";
            }
            if (flag == "p") {
                Grid1.refresh();
                btnThanhLy.enable();
                btnSua.enable();
                document.getElementById("thongTinThanhLy").style.visibility = "visible";
                document.getElementById("thongTinThanhLy").style.display = "Block";
                document.getElementById("thongTinThanhLyCT").style.visibility = "hidden";
                document.getElementById("thongTinThanhLyCT").style.display = "none";
                coThaoTac = "";
                sttXoa = "";
                sttThanhLypr = "";
                sttTLCT = "";
                document.getElementById("ctl00_ContentPlaceHolder1_hdfSttTL").value = "";
                document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDA").value = "";
                loadHopDong();
            }
        }
        function suaThongTinThanhLy1() {
            daLuu = "";
            coThaoTac = "1";
            Grid2.refresh();
            anHienControl("s");
            return false;
        }
        function suaThongTinThanhLy() {
            if (Grid1.SelectedRecords.length > 0) {
                debugger;
                var record = Grid1.SelectedRecords[0];
                if ("<%= ProOnline.Class.ntsLibraryFunctions.phanQuyen() %>" == "true") {
                    alert("User đang sử dụng chỉ được xem dữ liệu, không thực hiện được thao tác sửa!");
                    return false;
                }
                if (ntsLibraryFunctions.phanQuyenRecord("tblThanhLy", record.sttThanhLypr).value == "false") {
                    alert("Dòng dữ liệu đang chọn của User khác nhập liệu, Bạn không được phép sửa!");
                    return false;
                }
                document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value = record.sttDuAnpr_sd;
                layThongTinDuAn_uc(record.sttDuAnpr_sd);//nếu không truyền tham số là lấy mặt định từ session vd: layThongTinDuAn_uc(0);
                btnChonDA_uc.disable(); // ẩn nút Chọn dự án
                loadHopDong();  // ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc // load luôn giá trị combobox
                cboHopDong.value(record.sttHopDongpr_sd); // gán giá trị combobox Hợp đồng
                cacCanCu.value(record.cacCanCu);
                soBienBan.value(record.soBienBan);
                ngayLap.value(record.ngayLap);
                sttThanhLypr = record.sttThanhLypr;
                coThaoTac = "1";
                document.getElementById("ctl00_ContentPlaceHolder1_hdfSttTL").value = record.sttThanhLypr;
                anHienControl("s"); // hiện step Sửa
                Grid2.refresh();
                /*if (ProOnline.quanly.nghiemthuhopdong.SoDongGrid2().value == "0") {
                    cboHopDong.enable();
                } else { cboHopDong.disable(); }*/
            }
            else {
                alert("Không có dòng dữ liệu nào được chọn!");
            }
        }
        //xóa lưới ngoai
        function xoaThongTinTL() {
            debugger;
            coThaoTac = "0";
            if (Grid1.SelectedRecords.length > 0) {
                var record = Grid1.SelectedRecords[0];
                if ("<%= ProOnline.Class.ntsLibraryFunctions.phanQuyen() %>" == "true") {
                    alert("User đang sử dụng chỉ được xem dữ liệu, không thực hiện được thao tác xóa!");
                    return false;
                }
                if (ntsLibraryFunctions.phanQuyenRecord("tblThanhLy", record.sttThanhLypr).value == "false") {
                    alert("Dòng dữ liệu đang chọn của User khác nhập liệu, Bạn không được phép xóa!");
                    return false;
                }
                if (ProOnline.quanly.tl.checkDuyetQT(record.sttDuAnpr_sd).value == "1") {
                    alert("Dự án đã được quyết toán, bạn không thể thực hiện thao tác này")
                    return false;
                }
                sttXoa = record.sttThanhLypr;
                Dialog3.Open();
                return false;
            }
            else {
                alert("Không có dòng dữ liệu nào được chọn!");
            }

        }
        function themThongTinThanhLy() {
            coThaoTac = "0";
            anHienControl("t");
            Grid2.refresh();
            maDuAn.value('');
            tenDuAn.value('');
            document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDA").value = "";
        }
        function themThongTinThanhLy2() {
            coThaoTac = "0";
            anHienControl("t");
            Grid2.refresh();
        }
        function Grid1OnClientDblClick(record) {
            suaThongTinThanhLy();
            return;
        }
        function openWindows() {
            debugger;
            if (Grid1.SelectedRecords.length > 0) {
                var record = Grid1.SelectedRecords[0];
                document.getElementById("ctl00_ContentPlaceHolder1_hdfSttTL").value = record.sttThanhLypr;
            }
            else {
                alert("Bạn chưa chọn thông tin để in");
                return false;
            }
            $('[data-popup="popup-1"]').fadeIn(350);
            $(function () {
                //----- OPEN
                $('[data-popup-open]').on('click', function (e) {
                    var targeted_popup_class = jQuery(this).attr('data-popup-open');
                    $('[data-popup="' + targeted_popup_class + '"]').fadeIn(350);
                    e.preventDefault();
                });
                //----- CLOSE
                $('[data-popup-close]').on('click', function (e) {
                    var targeted_popup_class = jQuery(this).attr('data-popup-close');
                    $('[data-popup="' + targeted_popup_class + '"]').fadeOut(350);

                    e.preventDefault();
                });
            });
            var kq = ProOnline.quanly.tl.xuatWord(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttTL").value).value;
            document.getElementById("content33").setAttribute("src", kq);
        }
    </script>
    <script type="text/javascript">
        function themThongTinThanhLy() {
            layThongTinDuAn_uc(0);//nếu không truyền tham số là lấy mặt định từ session vd: layThongTinDuAn_uc(0); //
            btnChonDA_uc.enable();
            daLuu = "";
            coThaoTac = "0";
            document.getElementById("ctl00_ContentPlaceHolder1_hdfSttTL").value = "";
            anHienControl("t");
        }
        function anHienControl(flag) {
            if (flag == "t") {
                debugger;
                cboHopDong.value('');
                //canCuNghiemThu.value('');
                soBienBan.value('');
                ngayLap.value('');
                cboHopDong.enable();
                //canCuNghiemThu.enable();
                soBienBan.enable();
                ngayLap.enable();
                document.getElementById("thongTinThanhLyCT").style.visibility = "visible";
                document.getElementById("thongTinThanhLyCT").style.display = "Block";
                document.getElementById("thongTinThanhLy").style.visibility = "hidden";
                document.getElementById("thongTinThanhLy").style.display = "none";
                cboHopDong.value("0");
                sttThanhLypr = "";
            }
            if (flag == "s") {
                document.getElementById("thongTinThanhLyCT").style.visibility = "visible";
                document.getElementById("thongTinThanhLyCT").style.display = "Block";
                document.getElementById("thongTinThanhLy").style.visibility = "hidden";
                document.getElementById("thongTinThanhLy").style.display = "none";
                daLuu = "0";
            }
            if (flag == "p") {
                Grid1.refresh();
                document.getElementById("thongTinThanhLy").style.visibility = "visible";
                document.getElementById("thongTinThanhLy").style.display = "Block";
                document.getElementById("thongTinThanhLyCT").style.visibility = "hidden";
                document.getElementById("thongTinThanhLyCT").style.display = "none";
                coThaoTac = "";
                sttXoa = "";
                sttNghiemThupr = "";
                sttNTCT = "";
                daLuu = "0";
                document.getElementById("ctl00_ContentPlaceHolder1_hdfSttTL").value = "";
                document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value = "";
                loadHopDong();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <b style="padding: 3px;">THANH LÝ</b>
    <asp:HiddenField ID="hdfSttDuAnpr_uc" runat="server" Value="" />
    <asp:HiddenField ID="hdfSttDA" runat="server" Value="" />
    <asp:HiddenField ID="hdfSttTL" runat="server" Value="" />
    <asp:HiddenField ID="hdfSttVBDApr_uc" runat="server" Value="" />
    <div id="thongTinThanhLy">
        <div style="margin-left: 7px;">
            <table style="width: 100%;">
                <tr>
                    <td>
                        <cc2:OboutButton ID="OboutButton5" OnClientClick="themThongTinThanhLy(); return false;" runat="server" Text="Thêm" Width="100" FolderStyle="App_Themes/Styles/Interface/OboutButton">
                        </cc2:OboutButton>
                        <cc2:OboutButton ID="OboutButton6" OnClientClick="suaThongTinThanhLy(); return false;" runat="server" Text="Sửa" Width="100" FolderStyle="App_Themes/Styles/Interface/OboutButton">
                        </cc2:OboutButton>
                        <cc2:OboutButton ID="btnOpenWord" runat="server" OnClientClick="openWindows();return false;" Width="120"
                            Text="In" FolderStyle="~/App_Themes/Styles/Interface/OboutButton">
                        </cc2:OboutButton>
                        <cc2:OboutButton ID="OboutButton7" OnClientClick="xoaThongTinTL(); return false;" runat="server" Text="Xóa" Width="100" FolderStyle="App_Themes/Styles/Interface/OboutButton">
                        </cc2:OboutButton>
                    </td>
                    <td style="width: 286px;"></td>
                    <td align="right">
                        <input type="text" style="width: 250px" placeholder="Nội dung tìm kiếm..." onkeyup="searchValue(Grid1,1,this.value)"
                            class="searchCss" />
                    </td>
                </tr>
            </table>
        </div>
        <div style="border: 0px solid #DBDBE1; margin-left: 0px; margin: 4px; padding: 5px">

            <cc1:Grid ID="Grid1" runat="server" AutoGenerateColumns="False" PageSize="15" AllowPaging="True" AllowAddingRecords="false"
                Height="500" FolderStyle="~/App_Themes/Styles/style_7" GroupBy="tenDuAn" AllowGrouping="true" OnRebind="Grid1_OnRebind" FilterType="ProgrammaticOnly"
                AllowFiltering="true" ShowColumnsFooter="true" EnableRecordHover="true" OnRowDataBound="Grid1_RowDataBound"
                PageSizeOptions="10,15,20,50,100">
                <AddEditDeleteSettings AddLinksPosition="Top" />
                <PagingSettings Position="Bottom" />
                <ClientSideEvents OnClientDblClick="Grid1OnClientDblClick" />
                <FilteringSettings MatchingType="AnyFilter" FilterPosition="Top" FilterLinksPosition="Bottom" />
                <ScrollingSettings ScrollHeight="100%" EnableVirtualScrolling="false" ScrollWidth="979" />
                <Columns>
                    <cc1:Column HeaderText="Dự án" Width="290px" DataField="tenDuAn" Wrap="true"></cc1:Column>
                    <cc1:Column HeaderText="Số biên bản" Width="120px" DataField="soBienBan"></cc1:Column>
                    <cc1:Column HeaderText="Ngày lập" Width="120px" DataField="ngayLap"></cc1:Column>
                    <cc1:Column HeaderText="Số hợp đồng" Width="120px" DataField="soHopDong"></cc1:Column>
                    <cc1:Column HeaderText="Giá trị TL" Width="120px" DataField="giaTriTL" DataFormatString="{0:n0}" Align="right"></cc1:Column>
                    <cc1:Column HeaderText="Các căn cứ" Width="350px" DataField="cacCanCu" Wrap="true"></cc1:Column>
                    <cc1:Column HeaderText="" Width="0px" DataField="sttThanhLypr" Visible="false"></cc1:Column>
                    <cc1:Column HeaderText="" Width="0px" DataField="sttDuAnpr_sd" Visible="false"></cc1:Column>
                    <cc1:Column HeaderText="" Width="0px" DataField="maDuAn" Visible="false"></cc1:Column>
                    <cc1:Column HeaderText="" Width="0px" DataField="sttHopDongpr_sd" Visible="false"></cc1:Column>

                    <cc1:Column HeaderText="" Width="0px" DataField="daiDienCDT" Visible="false"></cc1:Column>
                    <cc1:Column HeaderText="" Width="0px" DataField="chucVuDDCDT" Visible="false"></cc1:Column>
                    <cc1:Column HeaderText="" Width="0px" DataField="daiDienNThau" Visible="false"></cc1:Column>
                    <cc1:Column HeaderText="" Width="0px" DataField="chucVuDDNThau" Visible="false"></cc1:Column>
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
            </cc1:Grid>
        </div>
    </div> <!-- id="thongTinThanhLy" -->
    <div id="thongTinThanhLyCT" style="display: none"><!-- id=thongTinThanhLyCT -->
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
                                                    <span class="title">Thông tin thanh lý</span>
                                                </li>
                                                <li data-step="3">
                                                    <span class="step">B3</span>
                                                    <span class="title">Đối tượng thanh lý</span>
                                                </li>
                                                <li data-step="4">
                                                    <span class="step">B4</span>
                                                    <span class="title">Văn bản</span>
                                                </li>
                                            </ul>
                                        </div>
                                        <div class="step-content pos-rel">
                                            <!-- Bước 1 -->
                                            <div id="step1" class="step-pane active" data-step="1">
                                                <uc4:thongtinduan_b1 ID="thongtinduan_b1" runat="server" />
                                            </div>
                                            <!-- Bước 2 -->
                                            <div id="step2" class="step-pane" data-step="2">
                                                <fieldset style="border: 1px solid #DBDBE1; margin-left: 0px; margin: 3px; padding: 4px">
                                                    <legend><b>Thông tin chung</b></legend>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td></td>
                                                            <td style="width: 180px"></td>
                                                            <td style="width: 120px"></td>
                                                            <td style="width: 110px"></td>
                                                            <td></td>
                                                            <td style="width: 180px"></td>
                                                            <td style="width: 60px"></td>
                                                            <td style="width: 25px"></td>
                                                            <td style="width: 30px"></td>
                                                            <td style="width: 15px; text-align: right;"></td>
                                                        </tr>
                                                        <%--<tr>
                                                            <td style="width: 120px">Dự án 
                                                            </td>
                                                            <td style="width: 180px">
                                                                <cc2:OboutTextBox ID="maDuAn" runat="server" Width="100%" Enabled="false" FolderStyle="App_Themes/Styles/Interface/OboutTextBox"></cc2:OboutTextBox>
                                                            </td>
                                                            <td colspan="5">
                                                                <cc2:OboutTextBox ID="tenDuAn" Width="100%" runat="server" Enabled="false" FolderStyle="App_Themes/Styles/Interface/OboutTextBox"></cc2:OboutTextBox>
                                                            </td>
                                                            <td colspan="3">
                                                                <cc2:OboutButton ID="btnchonDuAn" OnClientClick="setPosition(); Window3.Open(); Grid3.refresh(); return false;" runat="server" Text="Chọn dự án" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutButton">
                                                                </cc2:OboutButton>
                                                            </td>
                                                        </tr>--%>
                                                        <tr>
                                                            <td>Hợp đồng 
                                                            </td>
                                                            <td colspan="8">
                                                                <script type="text/javascript">
                                                                    // khi chọn combobox hộp đồng load dữ liệu cho 
                                                                    function chonHopDong() {
                                                                        var result = ProOnline.quanly.tl.getThongTinThanhToan(cboHopDong.value(), document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value);
                                                                        if (result.value.Rows.length > 0) {
                                                                            //debugger;
                                                                            soTienTU_TT.value(result.value.Rows[1].soTien);
                                                                            soTienChuaTT.value(format(result.value.Rows[0].soTien));
                                                                            giaTriHopDong.value(format(result.value.Rows[2].soTien));
                                                                            daiDienCDT.value(result.value.Rows[3].soTien);
                                                                            chucVuCDT.value(result.value.Rows[4].soTien);
                                                                            daiDienNT.value(result.value.Rows[5].soTien);
                                                                            chucVuNT.value(result.value.Rows[6].soTien);
                                                                            return false;
                                                                        }
                                                                        else {
                                                                            debugger;
                                                                            soTienTU_TT.value('');
                                                                            soTienChuaTT.value('');
                                                                            giaTriHopDong.value('');
                                                                            daiDienCDT.value('');
                                                                            chucVuCDT.value('');
                                                                            daiDienNT.value('');
                                                                            chucVuNT.value('');
                                                                            return false;
                                                                        }
                                                                    }
                                                                </script>
                                                                <cc3:ComboBox ID="cboHopDong" runat="server" Width="100%" MenuWidth="100%" FilterType="Contains"
                                                                    FolderStyle="App_Themes/Styles/Interface/OboutComboBox">
                                                                    <ClientSideEvents OnSelectedIndexChanged="chonHopDong" />
                                                                </cc3:ComboBox>
                                                            </td>
                                                            <td style="width: 15px" align="right">
                                                                <span style="color: Red;">*</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 120px">Số biên bản 
                                                            </td>
                                                            <td colspan="2">
                                                                <cc2:OboutTextBox ID="soBienBan" runat="server" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox"></cc2:OboutTextBox>
                                                            </td>
                                                            <td colspan="2" style="width: 120px">
                                                                <span style="color: Red;">*</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Ngày thanh lý
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
                                                            <td style="width: 15px" align="right">
                                                                <span style="color: Red; text-align: right;">*</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Căn cứ thanh lý
                                                            </td>
                                                            <td colspan="9">
                                                                <cc2:OboutTextBox ID="cacCanCu" TextMode="MultiLine" Height="100px" runat="server"
                                                                    FolderStyle="App_Themes/Styles/Interface/OboutTextBox" Width="100%"></cc2:OboutTextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 120px">Đại diện chủ đầu tư 
                                                            </td>
                                                            <td colspan="2">
                                                                <cc2:OboutTextBox ID="daiDienCDT" runat="server" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox"></cc2:OboutTextBox>
                                                            </td>
                                                            <td colspan="2" style="width: 120px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Chức vụ
                                                            </td>
                                                            <td colspan="5">
                                                                <cc2:OboutTextBox ID="chucVuCDT" runat="server" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox"></cc2:OboutTextBox>
                                                            </td>

                                                        </tr>
                                                        <tr>
                                                            <td style="width: 120px">Đại diện nhà thầu
                                                            </td>
                                                            <td colspan="2">
                                                                <cc2:OboutTextBox ID="daiDienNT" runat="server" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox"></cc2:OboutTextBox>
                                                            </td>
                                                            <td colspan="2" style="width: 120px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Chức vụ
                                                            </td>
                                                            <td colspan="5">
                                                                <cc2:OboutTextBox ID="chucVuNT" runat="server" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox"></cc2:OboutTextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 120px">Số hóa đơn
                                                            </td>
                                                            <td colspan="2">
                                                                <cc2:OboutTextBox ID="soHoaDon" runat="server" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox"></cc2:OboutTextBox>
                                                            </td>
                                                            <td colspan="2" style="width: 120px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Ngày hóa đơn
                                                            </td>
                                                            <td colspan="4">
                                                                <cc2:OboutTextBox ID="ngayHoaDon" runat="server" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox"></cc2:OboutTextBox>
                                                            </td>
                                                            <td>
                                                                <obout:Calendar ID="Calendar1" runat="server" DatePickerMode="true" TextBoxId="ngayHoaDon"
                                                                    DateFormat="dd/MM/yyyy" TitleText="Chọn ngày" Columns="1" DatePickerImagePath="~/App_Themes/Styles/Date/date_picker1.gif"
                                                                    CultureName="vi-VN">
                                                                </obout:Calendar>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 120px">Giá trị hợp đồng
                                                            </td>
                                                            <td colspan="2">
                                                                <cc2:OboutTextBox ID="giaTriHopDong" Enabled="false" runat="server" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox"></cc2:OboutTextBox>
                                                            </td>
                                                            <td colspan="2" style="width: 120px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Số tiền còn<br />
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;lại chưa thanh toán
                                                            </td>
                                                            <td colspan="5">
                                                                <cc2:OboutTextBox ID="soTienChuaTT" Enabled="false" runat="server" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox"></cc2:OboutTextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Số tiền tạm ứng và thanh toán
                                                            </td>
                                                            <td colspan="9">
                                                                <cc2:OboutTextBox ID="soTienTU_TT" TextMode="MultiLine" Height="50px" runat="server" Enabled="false"
                                                                    FolderStyle="App_Themes/Styles/Interface/OboutTextBox" Width="100%"></cc2:OboutTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </div>
                                            <!-- Bước 3 -->
                                            <div id="step3" class="step-pane" data-step="3">
                                                <fieldset style="border: 1px solid #DBDBE1; margin-left: 0px; margin: 3px; padding: 4px">
                                                    <legend><b>Đối tượng thanh lý</b></legend>
                                                    <script type="text/javascript">

                                                        function luuThanhLyCT(record) {
                                                            debugger;
                                                            var param = new Array();
                                                            param[0] = sttTLCT;
                                                            param[1] = document.getElementById("ctl00_ContentPlaceHolder1_hdfSttTL").value;
                                                            param[2] = cboHopDong.value();
                                                            param[3] = cboCongViec.value();
                                                            param[4] = record.donViTinh;
                                                            param[5] = eKhoiLuong.value();
                                                            param[6] = eDonGia.value();
                                                            param[7] = eThanhTien.value();
                                                            if (param[3] == "") {
                                                                alert("Bạn chưa chọn công việc thanh lý");
                                                                return false;
                                                            }
                                                            if (isNaN(param[5].replaceAll(".", "").replaceAll(",", ""))) {
                                                                alert("Khối lượng phải là kiểu số");
                                                                return false;
                                                            }
                                                            if (isNaN(param[6].replaceAll(".", "").replaceAll(",", ""))) {
                                                                alert("Đơn giá phải là kiểu số");
                                                                return false;
                                                            }
                                                            if (isNaN(param[7].replaceAll(".", "").replaceAll(",", ""))) {
                                                                alert("Thành tiền phải là kiểu số");
                                                                return false;
                                                            }
                                                            if (ProOnline.quanly.tl.soSanh(param[6], eGiaTriHD.value()).value == true) {
                                                                alert("Đơn giá không được lớn hơn giá trị hợp đồng");
                                                                return false;
                                                            }
                                                            if (ProOnline.quanly.tl.soSanh(param[7], eGiaTriHD.value()).value == true) {
                                                                alert("Thành tiền không được lớn hơn giá trị hợp đồng");
                                                                return false;
                                                            }
                                                            var result = ProOnline.quanly.tl.kiemTraCongViecThanhLy(cboCongViec.value(), sttTLCT);
                                                            if (result.value != "") {
                                                                alert("Công việc đã được thanh lý");
                                                                return false;
                                                            }
                                                            var result = ProOnline.quanly.tl.luuThongTinThanhLyCT(param, coThaoTac);
                                                            if (result.value == "0") {
                                                                alert("Thông tin thanh lý chi tiết chưa được lưu");
                                                                return false;
                                                            }
                                                            chonHopDong();
                                                            Grid2.refresh();
                                                            return false;
                                                        }
                                                        //Xóa lưới trong
                                                        function truocKhiXoa(record) {
                                                            debugger;
                                                            if (daLuu == "0") {
                                                                alert("Bạn phải lưu thông tin trước");
                                                                return false;
                                                            }
                                                            if (ProOnline.quanly.tl.checkDuyetQToan(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDA").value).value == "1") {
                                                                alert("Dự án đã được duyệt quyết toán, bạn không thể thực hiện thao tác này!");
                                                                return false;
                                                            }
                                                            coThaoTac = "1";
                                                            sttXoa = record.sttThanhLyCTpr;
                                                            Dialog3.Open();
                                                        }
                                                        function Grid2OnClientDblClick(record) {
                                                            debugger;
                                                            if (daLuu == "0") {
                                                                alert("Bạn phải lưu thông tin thanh lý trước");
                                                                return false;
                                                            }
                                                            if (ProOnline.quanly.tl.checkDuyetQToan(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDA").value).value == "1") {
                                                                alert("Dự án đã được duyệt quyết toán, bạn không thể thực hiện thao tác này!");
                                                                return false;
                                                            }
                                                            Grid2.editRecord(record);
                                                            document.getElementById("ctl00_ContentPlaceHolder1_hdfSttTL").value == Grid2.Rows[record].Cells["sttThanhLyCTpr"].Value;
                                                            sttTLCT = Grid2.Rows[record].Cells["sttThanhLyCTpr"].Value;
                                                            eKetLuan.value(Grid2.Rows[record].Cells["ketLuan"].Value);
                                                            cboCongViec.value(Grid2.Rows[record].Cells["loai"].Value + '@' + Grid2.Rows[record].Cells["sttHDCTpr_sd"].Value + '@' + Grid2.Rows[record].Cells["maCPDauTuXDCTpr_sd"].Value);
                                                            changeCongViec();
                                                            coThaoTac = "1";
                                                        }
                                                        function eDonGia_OnTextChanged() {
                                                            debugger;
                                                            var result = ProOnline.quanly.tl.thanhTien(eKhoiLuong.value(), eDonGia.value());
                                                            eThanhTien.value(format(result.value));
                                                        }
                                                        function eKhoiLuong_OnTextChanged() {
                                                            debugger;
                                                            var result = ProOnline.quanly.tl.thanhTien(eKhoiLuong.value(), eDonGia.value());
                                                            eThanhTien.value(format(result.value));
                                                        }

                                                        var sttXoa = "";
                                                    </script>
                                                    <cc1:Grid ID="Grid2" runat="server" AutoGenerateColumns="False" PageSize="15" AllowPaging="true"
                                                        Height="400" FolderStyle="~/App_Themes/Styles/style_7" OnRebind="Grid2_OnRebind" OnRowDataBound="Grid2_RowDataBound"
                                                        AllowFiltering="true" ShowColumnsFooter="true" AllowAddingRecords="true" EnableRecordHover="true" FilterType="ProgrammaticOnly"
                                                        PageSizeOptions="10,15,20,50,100">
                                                        <AddEditDeleteSettings AddLinksPosition="Top" />
                                                        <ClientSideEvents OnClientDblClick="Grid2OnClientDblClick" OnBeforeClientAdd="truocKhiThem" OnBeforeClientInsert="luuThanhLyCT" OnBeforeClientEdit="truocKhiSua" OnBeforeClientUpdate="luuThanhLyCT" OnBeforeClientDelete="truocKhiXoa" />
                                                        <PagingSettings Position="Bottom" />
                                                           <FilteringSettings MatchingType="AnyFilter" FilterPosition="Top" FilterLinksPosition="Bottom" />
                                                        <ScrollingSettings ScrollHeight="100%" EnableVirtualScrolling="false" ScrollWidth="979" />
                                                        <Columns>
                                                            <cc1:Column AllowDelete="true" AllowEdit="true" HeaderText="Thao tác" Width="80px"></cc1:Column>
                                                            <cc1:Column HeaderText="Tên công việc thanh lý" Width="270px" DataField="tenCongViec">
                                                                <TemplateSettings EditTemplateId="etenCongViec" />
                                                            </cc1:Column>
                                                            <cc1:Column HeaderText="ĐVT" Width="120px" DataField="donViTinh"></cc1:Column>

                                                            <cc1:Column HeaderText="Khối lượng" Width="120px" DataField="khoiLuong" DataFormatString="{0:n0}">
                                                                <TemplateSettings EditTemplateId="editKhoiLuong" />
                                                            </cc1:Column>
                                                            <cc1:Column HeaderText="Đơn giá" Width="125px" DataField="donGia" DataFormatString="{0:n0}">
                                                                <TemplateSettings EditTemplateId="editDonGia" />
                                                            </cc1:Column>
                                                            <cc1:Column HeaderText="Thành tiền" Width="150px" DataField="giaTriThanhLy" DataFormatString="{0:n0}">
                                                                <TemplateSettings EditTemplateId="editThanhTien" />
                                                            </cc1:Column>
                                                            <cc1:Column HeaderText="Giá trị HD" Width="125px" DataField="giaTriHD" DataFormatString="{0:n0}">
                                                                <TemplateSettings EditTemplateId="editGiaTriHD" />
                                                            </cc1:Column>
                                                            <cc1:Column HeaderText="" Width="0px" DataField="sttHDCTpr_sd" Visible="false"></cc1:Column>
                                                            <cc1:Column HeaderText="" Width="0px" DataField="maCPDauTuXDCTpr_sd" Visible="false"></cc1:Column>
                                                            <cc1:Column HeaderText="" Width="0px" DataField="loai" Visible="false"></cc1:Column>
                                                            <cc1:Column HeaderText="" Width="0px" DataField="sttThanhLyCTpr" Visible="false"></cc1:Column>
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
                                                                     <input type="text" style="width: 250px" placeholder="Nội dung tìm kiếm..." onkeyup="searchValue(Grid2,1,this.value)"
                                                                                class="searchCss" />
                                                                    </Template>
                                                                </cc1:GridTemplate>
                                                            <cc1:GridTemplate runat="server" ControlID="cboCongViec" ID="etenCongViec" ControlPropertyName="value">
                                                                <Template>
                                                                    <cc3:ComboBox ID="cboCongViec" runat="server" AppendDataBoundItems="false" Width="100%" FilterType="Contains"
                                                                        FolderStyle="~/App_Themes/Styles/Interface/OboutComboBox" Height="100px" MenuWidth="150%"
                                                                        EnableVirtualScrolling="false">
                                                                        <ClientSideEvents OnSelectedIndexChanged="changeCongViec" />
                                                                    </cc3:ComboBox>
                                                                </Template>
                                                            </cc1:GridTemplate>
                                                            <cc1:GridTemplate runat="server" ControlID="eKhoiLuong" ID="editKhoiLuong" ControlPropertyName="value">
                                                                <Template>
                                                                    <cc2:OboutTextBox ID="eKhoiLuong" runat="server" Width="100%">
                                                                 <ClientSideEvents OnTextChanged="eKhoiLuong_OnTextChanged" />
                                                                    </cc2:OboutTextBox>
                                                                </Template>
                                                            </cc1:GridTemplate>
                                                            <cc1:GridTemplate runat="server" ControlID="eDonGia" ID="editDonGia" ControlPropertyName="value">
                                                                <Template>
                                                                    <cc2:OboutTextBox ID="eDonGia" runat="server" Width="100%">
                                                                 <ClientSideEvents OnTextChanged="eDonGia_OnTextChanged" />
                                                                    </cc2:OboutTextBox>
                                                                </Template>
                                                            </cc1:GridTemplate>
                                                            <cc1:GridTemplate runat="server" ControlID="eThanhTien" ID="editThanhTien" ControlPropertyName="value">
                                                                <Template>
                                                                    <cc2:OboutTextBox ID="eThanhTien" Enabled="false" runat="server" Width="100%"></cc2:OboutTextBox>
                                                                </Template>
                                                            </cc1:GridTemplate>

                                                            <cc1:GridTemplate runat="server" ControlID="eGiaTriHD" ID="editGiaTriHD" ControlPropertyName="value">
                                                                <Template>
                                                                    <cc2:OboutTextBox ID="eGiaTriHD" runat="server" Enabled="false" Width="100%"></cc2:OboutTextBox>
                                                                </Template>
                                                            </cc1:GridTemplate>
                                                        </Templates>
                                                    </cc1:Grid>
                                                </fieldset>
                                            </div>
                                            <!-- Bước 4 -->
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
       
        <script type="text/javascript">
            var sttXoa = "";
            function xoaThanhLy() {
                debugger;
                var result = ProOnline.quanly.tl.xoaThongTin(sttXoa, coThaoTac);
                if (result.value == "0") {
                    alert("Thông tin chưa được xóa");
                    return false;
                }
                Dialog3.Close();
                if (coThaoTac == "0") {
                    Grid1.refresh();
                }
                else {
                    Grid2.refresh();
                }
                coThaoTac = "";
                sttXoa = "";
                sttThanhLypr = "";
                sttNTCT = "";
                document.getElementById("ctl00_ContentPlaceHolder1_hdfSttTL").value;
            }
        </script>
        <script type="text/javascript">
            function uploadError(sender, args) {
                alert("Tải tập tin thất bại");
            }
            function uploadComplete(sender, args) {
                debugger;
                var fileExtension = args.get_fileName();
                var ketqua = ProOnline.quanly.tl.layThongTinVanBanTL(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttTL").value);
                document.getElementById('tdNoiDung').innerHTML = (ketqua.value.Rows[0].duongDanFile_ == "" ? "<p style=\"color:red;text-decoration:none\">Chưa đính kèm</p>" : "<a style=\"color:Blue;text-decoration:none\" onclick=\"xemVB('" + ketqua.value.Rows[0].duongDanFile_ + "')\" href=\"#\">Xem đính kèm</a>");

            }
            function uploadStarted(sender, args) {
                debugger;
                var fileExtension = args.get_fileName();
                if (fileExtension.indexOf('.pdf') != -1 || fileExtension.indexOf('.PDF') != -1 || fileExtension.indexOf('.doc') != -1 || fileExtension.indexOf('.DOC') != -1 || fileExtension.indexOf('.docx') != -1 || fileExtension.indexOf('.DOCX') != -1) {
                }
                else {
                    alert("Tập tin không đúng định dạng.");
                    return;
                }
            }

            // trước thi thêm Grid 2 // step 3
            function truocKhiThem() {
                debugger;
                if (daLuu == "0") {
                    alert("Bạn phải lưu thông tin thanh lý trước");
                    return false;
                }
                if (ProOnline.quanly.tl.checkDuyetQToan(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDA").value).value == "1") {
                    alert("Dự án đã được duyệt quyết toán, bạn không thể thực hiện thao tác này!");
                    return false;
                }
                loadDanhSachCongViec(cboHopDong.value(), document.getElementById("ctl00_ContentPlaceHolder1_hdfSttTL").value, "0");
                coThaoTac = "0";
                sttTLCT = "";
            }
            var sttTLCT = "";
            function truocKhiSua(record) {
                debugger;
                if (daLuu == "0") {
                    alert("Bạn phải lưu thông tin thanh lý trước"); t
                    return false;
                }
                if (ProOnline.quanly.tl.checkDuyetQToan(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDA").value).value == "1") {
                    alert("Dự án đã được duyệt quyết toán, bạn không thể thực hiện thao tác này!");
                    return false;
                }
                sttTLCT = record.sttThanhLyCTpr;
                loadDanhSachCongViec(cboHopDong.value(), document.getElementById("ctl00_ContentPlaceHolder1_hdfSttTL").value, sttTLCT);
                cboCongViec.value(record.loai + '@' + record.sttHDCTpr_sd + '@' + record.maCPDauTuXDCTpr_sd);
                changeCongViec();
                coThaoTac = "1";
            }

            function luuNghiemThuCT(record) {
                var param = new Array();
                param[0] = sttNTCT;
                param[1] = document.getElementById("ctl00_ContentPlaceHolder1_hdfSttTL").value;
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
                //var result = ProOnline.quanly.tl.luuthongTinThanhLyCT(param, coThaoTac);
                //coThaoTac = 0 => thêm;
                //coThaoTac = 1 => cập nhật;
                var result = ProOnline.quanly.tl.luuThongTinThanhLyCT(param, coThaoTac);
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
                if (checkEmtyValue(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value)) {
                    alert("Bạn phải lưu thông tin trước!");
                    return false;
                }
                if (ProOnline.quanly.tl.checkDuyetQToan(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value).value == "1") {
                    alert("Dự án đã được duyệt quyết toán, bạn không thể thực hiện thao tác này!");
                    return false;
                }
                coThaoTac = "1";
                sttXoa = record.sttNghiemThuCTpr;
                Dialog3.Open();
                return false;
            }
            function Grid2OnClientDblClick(record) {
                if (checkEmtyValue(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttTL").value)) {
                    alert("Bạn phải lưu thông tin trước!");
                    return false;
                }
                if (ProOnline.quanly.tl.checkDuyetQToan(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value).value == "1") {
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
    </div>
    <script type="text/javascript" src="/App_Themes/BuocThaoTac/js/jquery-2.1.4.min.js"></script>
    <script type="text/javascript" src="/App_Themes/BuocThaoTac/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="/App_Themes/BuocThaoTac/js/ace.min.js"></script>
    <script type="text/javascript" src="/App_Themes/BuocThaoTac/js/wizard.min.js"></script>
    <script type="text/javascript" src="/App_Themes/BuocThaoTac/js/ace-elements.min.js"></script>
    <script type="text/javascript">
        function loadHopDong() {
            debugger;
            //var result = ProOnline.quanly.tl.getDanhSachHopDong(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDA").value);
            var result = ProOnline.quanly.tl.getDanhSachHopDong(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value);
            cboHopDong.options.clear();
            for (var i = 0; i < result.value.Rows.length; i++) {
                var noiDung = result.value.Rows[i].noidung;
                var ma = result.value.Rows[i].sttHopDongpr;
                cboHopDong.options.add(noiDung, ma, i);
            }
        }
    </script>
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
                        //canCuNghiemThu.disable();
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
                        // 2. Thông tin thanh lý // phần thêm // làm mới các control
                        cboHopDong.enable();
                        soBienBan.enable();
                        ngayLap.enable();
                        cacCanCu.enable();
                        daiDienCDT.enable();
                        chucVuCDT.enable();
                        daiDienNT.enable();
                        chucVuNT.enable();
                        soHoaDon.enable();
                        ngayHoaDon.enable();
                        giaTriHopDong.disable();
                        soTienChuaTT.disable();
                        soTienTU_TT.disable();
                    }
                    if (isEmtyValue(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttTL").value)) {
                        loadHopDong();
                    }

                }
                if (!tmp_click && info.step == 2) {
                    if (document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value == "" || document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value == "0") {
                        alert("Không tồn tại dự án để thực hiện kết thúc quá trình thao tác!");
                        e.preventDefault();
                    } else {
                        // lưu dữ liệu step 2
                        var param = new Array();
                        param[0] = document.getElementById("ctl00_ContentPlaceHolder1_hdfSttTL").value;  // sớ thứ tự thanh lý
                        param[1] = document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value; // số dự án
                        param[2] = cboHopDong.value(); // shojn hợp đồng
                        param[3] = soBienBan.value(); // số biên bản
                        param[4] = ngayLap.value(); // ngày thanh lý
                        param[5] = cacCanCu.value(); // căn cứ thanh lý
                        param[6] = daiDienCDT.value(); // đại diện chủ đầu tư
                        param[7] = chucVuCDT.value(); // chức vụ chủ đầu tư
                        param[8] = daiDienNT.value(); // đại diện nhà thầu
                        param[9] = chucVuNT.value(); // chức vụ nhà thầu
                        param[10] = soHoaDon.value(); // số hóa đơn
                        param[11] = ngayHoaDon.value(); // ngày hóa đơn
                        if (param[1] == "") {
                            alert("Bạn chưa chọn dự án");
                            return false;
                        }
                        if (param[2] == "") {
                            alert("Bạn chưa chọn hợp đồng hoặc phụ lục HĐ cần thanh lý");
                            return false;
                        }
                        if (param[4] == "") {
                            alert("Bạn chưa chọn ngày thanh lý");
                            return false;
                        }
                        var ktNgay = ProOnline.quanly.tl.kiemTraNgay(param[4]);
                        if (ktNgay.value == false) {
                            alert("Ngày thanh lý phải có 10 ký tự dạng dd/MM/yyyy");
                            setTimeout(function () { ngayLap.focus(); }, 1);
                            return false;
                        }
                        // coThaoTac = 0 -> thêm
                        // coThaoTac = 1 -> sửa
                        var result = ProOnline.quanly.tl.luuThongTin(param, coThaoTac);
                        if (result.value.split('_')[0] == "0") {
                            alert(result.value.split('_')[1]);
                            e.preventDefault();
                            return false;
                        }
                        if (coThaoTac == "0")
                            document.getElementById("ctl00_ContentPlaceHolder1_hdfSttTL").value = result.value.split('_')[0];
                        if (!isEmtyValue(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttTL").value))
                            coThaoTac = "1";
                        daLuu = "1";
                        //loadDanhSachCongViec();
                        loadDanhSachCongViec(cboHopDong.value(), document.getElementById("ctl00_ContentPlaceHolder1_hdfSttTL").value, "");
                        //document.getElementById("ctl00_ContentPlaceHolder1_vanban_buocketthuc_sttVBDApr").value = document.getElementById("ctl00_ContentPlaceHolder1_hdfSttVBDApr_uc").value = ProOnline.quanly.nghiemthuhopdong.getSttVanBan(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttTL").value).value;
                        Grid2.refresh();
                    }
                } else { Grid2.refresh(); }
                if (!tmp_click && info.step == 3) {
                    debugger;
                    if (document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value == "" || document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value == "0") {
                        alert("Không tồn tại dự án để thực hiện kết thúc quá trình thao tác!");
                        e.preventDefault();
                    }
                    else if (document.getElementById("ctl00_ContentPlaceHolder1_hdfSttTL").value == "" || document.getElementById("ctl00_ContentPlaceHolder1_hdfSttTL").value == "0") {
                        alert("Không tồn tại thông tin chứng từ để thực hiện bước tiếp theo!");
                        e.preventDefault();
                    }
                    var ketqua = ProOnline.quanly.tl.layThongTinVanBanTL(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttTL").value);
                    txtSoVanBan_uc.value(ketqua.value.Rows[0].soBienBan); //
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
                param[0] = document.getElementById("ctl00_ContentPlaceHolder1_hdfSttTL").value;
                param[1] = txtNoiDung_uc.value();
                param[2] = txtGiaTri_uc.value();
                var ketqua_luuvb = ProOnline.quanly.tl.luuThongTinVanBanTL(param).value;
                if (ketqua_luuvb == true) {
                    wizard.currentStep = 1;
                    wizard.setState();
                    HienThiControl("thongTinThanhLy", true);
                    HienThiControl("thongTinThanhLyCT", false);
                    alert("Bạn đã lập xong biên bản thanh lý!");
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
                HienThiControl("thongTinThanhLy", true);
                HienThiControl("thongTinThanhLyCT", false);
                Grid1.refresh();
            }
        });
        $('#btnTiep').click(function (e) {
            tmp_click = false;
        });
        function xemVB(url) {
            debugger;
            window.open(url);
            return false;
        }
    </script>
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
                            <cc2:OboutButton ID="btnXoaNghiemTu" runat="server" Text="Thực hiện" Width="90px" OnClientClick="xoaThanhLy(); return false;"
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
    <div class="popup" data-popup="popup-1">
         <div style="clear: both;  padding: 5px; font-size: 16px; font-weight: 500;">
                <a style="text-decoration: none;" href="javascript:void(0)" onclick="taiExcel(); return false;"
                    title="Tải file docx"><b>Tải về</b>
                </a><a style="text-decoration: none;" href="javascript:void(0)" onclick="inFile(); return false;"
                    title="In báo cáo"><b>In File</b></a>
            </div>

            <script>
                function taiExcel() {
                    if (document.getElementById("content33").src == "")
                        return false;
                    window.open(document.getElementById("content33").src.replace('.html', '.docx'));
                    return false;
                }
                function inFile() {
                    var frm = document.getElementById("content33").contentWindow;
                    frm.focus();
                    frm.print();
                    return false;
                }
            </script> 
        <div class="popup-inner">
            <iframe id="content33" name="content33" style="height: 600px; text-align: center; border: 1px solid #eee"
                src="" width="100%" height="100%"></iframe>
            <a class="popup-close" data-popup-close="popup-1" href="#">x</a>
        </div>
    </div>
</asp:Content>