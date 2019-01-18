<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="vbchuanbidt.ascx.cs" Inherits="ProOnline.module.vbchuanbidt" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>
<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc3" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="cc2" %>
<%@ Register Assembly="obout_Window_NET" Namespace="OboutInc.Window" TagPrefix="owd" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="cc1" %>
<%@ Register Src="tochuc.ascx" TagName="tochuc" TagPrefix="uc1" %>
<%@ Register Src="canhan.ascx" TagName="canhan" TagPrefix="uc1" %>
<script>

    window.onload = function () {
        gridVanBan.refresh();
    }

    $(document).ready(function () {
        bindData(cboLoaiDuAn, ProOnline.module.vbchuanbidt.layDuAnTheoDonVi(document.getElementById("ctl00_ContentPlaceHolder1_vbchuanbidt1_hdfMaDonVi").value).value);
    });

    function truocKhiThemVanBan() {
        cboLoaiDuAn.value(ProOnline.module.vbchuanbidt.getSTTDuAn().value);
    }
    function luuThongTinVanBan(record) {
        var param = new Array();
        param[0] = document.getElementById("ctl00_ContentPlaceHolder1_vbchuanbidt1_userControlVanBanChucNang").value;
        param[1] = record.sttVBDApr;
        param[2] = record.sttVBDApr;
        param[3] = record.soVanBan;
        param[4] = record.ngayKy;
        param[5] = record.ngayKy;
        param[6] = cboCoQuanBanHanh.value();
        param[7] = record.noiDung;
        param[8] = record.giaTri;
        param[9] = cboLoaiVanBan.value();
        param[10] = "";
        param[11] = record.chucDanhNguoiKy;
        param[12] = nguoiKyVanBan.value();
        param[13] = cboLoaiDuAn.value();

        if (isEmty(param[13])) {
            alert("Tên dự án không được bỏ trống!");
            return false;
        }

        if (ntsLibraryFunctions.kiemTraNgay(param[5]).value == false) {
            alert("Ngày ký phải có 10 ký tự dạng ddMMyyyy");
            return false;
        }
        if (isEmty(param[4])) {
            alert("Số văn bản không được bỏ trống!");
            return false;
        }
        if (isEmty(param[9])) {
            alert("Loại văn bản không được bỏ trống!");
            return false;
        }
        if (ProOnline.module.vbchuanbidt.luuThongTinVanBan(param).value == "0") {
            alert("Thông tin văn bản chưa được lưu");
            return false;
        }
        gridVanBan.refresh();
        $('.obDS_AdDNew').show();
        $('.obDS_SaveCancel').hide();
        return false;

    }
    function truocKhiSuaVanBan(record) { 
        cboLoaiVanBan.value(record.maLoaiVBanpr_sd);
        cboCoQuanBanHanh.value(record.maToChucpr_phathanh);
        cboCoQuanBanHanh_OnSelectedIndexChanged();
        cboLoaiDuAn.value(record.sttDuAnpr_sd);
        var result = ProOnline.module.vbchuanbidt.danhSachDoiTuongTheoToChuc(record.maToChucpr_phathanh).value;
        nguoiKyVanBan.options.clear();
        if (result != null) {
            for (var i = 0; i < result.Rows.length; i++) {
                var maToChuc = result.Rows[i].tenCaNhan;
                var tenToChuc = result.Rows[i].tenCaNhan;
                nguoiKyVanBan.options.add(tenToChuc, maToChuc, i);

            }
            nguoiKyVanBan.value(record.maNguoiKy);
        }
        chucDanhNguoiKy.value(record.chucDanhNguoiKy);
        return false;
    }
    function gridVanBan_OnClientDblClick(iRecordIndex) {
        gridVanBan.editRecord(iRecordIndex);
    }
    function cboCoQuanBanHanh_OnSelectedIndexChanged() {
        var result = ProOnline.module.vbchuanbidt.danhSachDoiTuongTheoToChuc(cboCoQuanBanHanh.value());
        nguoiKyVanBan.options.clear();
        chucDanhNguoiKy.value('');
        if (result.value != null) {
            for (var i = 0; i < result.value.Rows.length; i++) {
                var ten = result.value.Rows[i].tenCaNhan;
                var ma = result.value.Rows[i].chucVu;
                nguoiKyVanBan.options.add(ten, ten, i);
            }
        }
    }
    function nguoiKyVanBan_OnSelectedIndexChanged() {
        chucDanhNguoiKy.value(ProOnline.module.vbchuanbidt.chucVuTheoCaNhan(cboCoQuanBanHanh.value(), nguoiKyVanBan.value()).value);
        return false;
    }
    function truocKhiXoaVanBan(record) {
        document.getElementById("ctl00_ContentPlaceHolder1_vbchuanbidt1_userControlVanBanpr").value = record.sttVBDApr;
        yesNoDialogXoaVanBan.Open();
        return false;
    }
    function xoaVanBan() {
        var kt = ProOnline.module.vbchuanbidt.XoaVanBan(document.getElementById("ctl00_ContentPlaceHolder1_vbchuanbidt1_userControlVanBanpr").value);
        if (kt.value == false) {
            alert("Xóa văn bản không thành công!");
            return false;
        }
        yesNoDialogXoaVanBan.Close();
        gridVanBan.refresh();
        return false;
    }
</script>
<asp:HiddenField ID="userControlVanBanpr" runat="server" />
<asp:HiddenField ID="userControlVanBanChucNang" runat="server" Value="chucnang_pr_0" />
<asp:HiddenField ID="hdf_page_name" runat="server" Value="" />
<asp:HiddenField ID="hdfMaDonVi" runat="server" Value="" />


<b>
    <asp:Label ID="lblTieuDe" runat="server"></asp:Label></b>

<table style="width: 100%">
    <tr style="height: 30px">
        <td class="obDS_AdDNew" style="width: 60px;">
            <div onclick="gridVanBan.addRecord(); $('.obDS_AdDNew').hide(); $('.obDS_SaveCancel').show();">
                Thêm mới
            </div>
        </td>
        <td class="obDS_SaveCancel" style="display: none; width: 55px;">
            <div>
                <div style="float: left; padding-right: 3px;" onclick="gridVanBan.insertRecord();return false;">
                    Lưu
                </div>
                |
                <div style="float: right; padding-left: 2px;" onclick="gridVanBan.cancelNewRecord(); $('.obDS_AdDNew').show(); $('.obDS_SaveCancel').hide();return false;">
                                    Hủy
                </div>
            </div>
        </td>
        <td align="right">
            <input type="text" style="width: 250px; padding: 5px; float: right; margin-bottom: 5px;" placeholder="Nội dung tìm kiếm..." onkeyup="searchValue(gridVanBan, 2, this.value)"
                class="searchCss" />

            <%--   <input style="width: 200px" type="text" placeholder="Nội dung tìm kiếm..." onkeyup="searchValue(gridVanBan,1,this.value)"
                                class="searchCss">--%>
        </td>
    </tr>
</table>
<style>
    .obDS_AdDNew, .obDS_SaveCancel
    {
        font-family: arial,sans-serif,Verdana;
        font-size: 12px;
        color: #27489d;
        font-weight: normal;
        text-decoration: none;
        padding-right: 1px;
        cursor: pointer;
    }
</style>



<cc1:Grid ID="gridVanBan" runat="server" FolderStyle="~/App_Themes/Styles/style_7" AllowPaging="true"
    PageSizeOptions="5,10,15,20" PageSize="15" AutoGenerateColumns="false" AllowFiltering="true"
    FilterType="ProgrammaticOnly" EnableRecordHover="true" AllowGrouping="true"
    GroupBy="tenDuAn" Height="500" OnRebind="gridVanBan_OnRebind" Width="1000px" AllowAddingRecords="false" AllowMultiRecordSelection="False">
    <AddEditDeleteSettings AddLinksPosition="Top" />
    <PagingSettings Position="Bottom" />
    <FilteringSettings MatchingType="AnyFilter" FilterPosition="Top" FilterLinksPosition="Bottom" />
    <ScrollingSettings ScrollHeight="100%" EnableVirtualScrolling="false" ScrollWidth="1000px" NumberOfFixedColumns="2" />
    <ClientSideEvents OnBeforeClientAdd="truocKhiThemVanBan" OnBeforeClientInsert="luuThongTinVanBan" OnBeforeClientUpdate="luuThongTinVanBan"
        OnClientEdit="truocKhiSuaVanBan" OnBeforeClientDelete="truocKhiXoaVanBan" OnClientDblClick="gridVanBan_OnClientDblClick" />
    <Columns>
        <cc1:Column HeaderText="Thao tác" AllowDelete="true" AllowEdit="true" Width="70px">
        </cc1:Column>
        
        <cc1:Column HeaderText="maDuan" DataField="sttDuAnpr_sd" Width="300px" Wrap="true" Visible="false">
        </cc1:Column>
        <cc1:Column HeaderText="sttVBDApr" DataField="sttVBDApr" Width="230px" ReadOnly="false"
            Wrap="true" Visible="false">
        </cc1:Column>
        <cc1:Column HeaderText="Số văn bản" DataField="soVanBan" Width="150px" Wrap="true">
        </cc1:Column>
        <cc1:Column HeaderText="Ngày ký" DataField="ngayKy" Width="100px" Wrap="true" DataFormatString="{0:dd/MM/yyyy}"
            ApplyFormatInEditMode="true">
        </cc1:Column>
        <cc1:Column HeaderText="Cơ quan ban hành" DataField="coQuanBanHanh" Width="200px"
            Wrap="true">
            <TemplateSettings EditTemplateId="tmpCQBH" />
        </cc1:Column>
        <cc1:Column HeaderText="Người ký" DataField="tenNguoiKy" Width="150px" Wrap="true"
            ApplyFormatInEditMode="true">
            <TemplateSettings EditTemplateId="tmpNguoiKy" />
        </cc1:Column>
        <cc1:Column HeaderText="Chức danh" DataField="chucDanhNguoiKy" Width="150px" Wrap="true"
            ApplyFormatInEditMode="true">
            <TemplateSettings EditTemplateId="tmpchucDanhNguoiKy" />
        </cc1:Column>
        <cc1:Column HeaderText="Nội dung" DataField="noiDung" Width="250px" Wrap="true">
        </cc1:Column>
        <cc1:Column HeaderText="Giá trị" DataField="giaTri" Width="150px" Wrap="true" DataFormatString="{0:n0}"
            Align="right" ApplyFormatInEditMode="true">
        </cc1:Column>
        <cc1:Column HeaderText="maLoaiVBanpr_sd" DataField="maLoaiVBanpr_sd" Width="180px"
            Wrap="true" Visible="false">
        </cc1:Column>

        <cc1:Column HeaderText="maToChucpr_phathanh" DataField="maToChucpr_phathanh" Width="180px"
            Wrap="true" Visible="false">
        </cc1:Column>
        <cc1:Column HeaderText="maNguoiKy" DataField="maNguoiKy" Width="0"
            Wrap="true" Visible="false">
        </cc1:Column>
        <cc1:Column HeaderText="Loại văn bản" DataField="loaiVB" Width="250px" Wrap="true">
            <TemplateSettings EditTemplateId="tmpLoaiVB" />
        </cc1:Column>
        <cc1:Column HeaderText="Đính kèm file" DataField="dinhKem" Width="180px" Wrap="true"
            ReadOnly="true" ParseHTML="true">
        </cc1:Column>
<cc1:Column HeaderText="Dự án" DataField="tenDuAn" Width="500px" Wrap="true">
            <TemplateSettings EditTemplateId="tmpDuAn" />
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
        Paging_PageSizeText="Số dòng 1 trang:" Paging_PagesText="Trang:" Paging_RecordsText="Dòng:"
        ResizingTooltipWidth="Rộng:" SaveAllLink="Lưu tất cả" SaveLink="Lưu" TypeValidationError="Giá trị mà bạn đã nhập vào trong cột XXXXX là không đúng."
        UndeleteLink="Không xóa" UpdateLink="Lưu" />
    <TemplateSettings HeadingTemplateId="idHeadinggridVanBan" />
    <Templates>
        <cc1:GridTemplate runat="server" ID="tmpCQBH" ControlID="cboCoQuanBanHanh">
            <Template>
                <cc3:ComboBox runat="server" ID="cboCoQuanBanHanh" Width="100%" Height="150" AppendDataBoundItems="false"
                    MenuWidth="300" FilterType="Contains" DataSourceID="sdsCoQuanBanHanh" DataValueField="maToChucpr"
                    DataTextField="tenToChuc">
                    <ClientSideEvents OnSelectedIndexChanged="cboCoQuanBanHanh_OnSelectedIndexChanged" />
                    <FooterTemplate>
                        <div style="cursor: pointer" onclick="dinhDangWindow(winDowThemToChuc);return false;">
                            Thêm cơ quan ban hành
                        </div>
                    </FooterTemplate>
                </cc3:ComboBox>
            </Template>
        </cc1:GridTemplate>
        <cc1:GridTemplate runat="server" ID="tmpNguoiKy" ControlID="nguoiKyVanBan">
            <Template>
                <cc3:ComboBox runat="server" ID="nguoiKyVanBan" Width="100%" Height="150" MenuWidth="300"
                    FilterType="Contains">
                    <ClientSideEvents OnSelectedIndexChanged="nguoiKyVanBan_OnSelectedIndexChanged" />
                    <FooterTemplate>
                        <div style="cursor: pointer" onclick="dinhDangWindow(winDowThemCaNhan);return false;">
                            Thêm người ký
                        </div>
                    </FooterTemplate>
                </cc3:ComboBox>
            </Template>
        </cc1:GridTemplate>
        <cc1:GridTemplate runat="server" ID="tmpchucDanhNguoiKy" ControlID="chucDanhNguoiKy">
            <Template>
                <cc2:OboutTextBox ID="chucDanhNguoiKy" runat="server" Width="100%"></cc2:OboutTextBox>
            </Template>
        </cc1:GridTemplate>
        <cc1:GridTemplate runat="server" ID="tmpLoaiVB" ControlID="cboLoaiVanBan">
            <Template>
                <cc3:ComboBox runat="server" ID="cboLoaiVanBan" Width="100%" Height="150" AppendDataBoundItems="false" MenuWidth="350px"
                    FilterType="Contains" DataSourceID="sdsLoaiVanBan" DataValueField="maLoaiVBanpr"
                    DataTextField="tenLoaiVBan" />
            </Template>
        </cc1:GridTemplate>

        <cc1:GridTemplate runat="server" ID="tmpDuAn" ControlID="cboLoaiDuAn">
            <Template>
                <cc3:ComboBox runat="server" ID="cboLoaiDuAn" Width="100%" Height="150" AppendDataBoundItems="false" MenuWidth="200%"
                    FilterType="Contains">
                </cc3:ComboBox>
            </Template>
        </cc1:GridTemplate>

    </Templates>
</cc1:Grid>
<owd:Dialog ID="yesNoDialogXoaVanBan" runat="server" IsModal="true" ShowCloseButton="true"
    Top="0" Left="250" Height="150" Width="350" VisibleOnLoad="false" StyleFolder="~/App_Themes/Styles/wdstyles/dogma"
    Title="Cảnh báo">
    <div align="center" style="height: 50px; margin-top: 15px">
        Bạn có thật sự muốn xóa dòng dữ liệu đã chọn không? Đồng ý xóa chọn 'Thực hiện',
                không đồng ý chọn 'Bỏ qua'.
    </div>
    <div align="center">
        <table>
            <tr>
                <td>
                    <cc2:OboutButton ID="OboutButton12" runat="server" Text="Thực hiện" Width="90px"
                        OnClientClick="xoaVanBan(); return false;" FolderStyle="App_Themes/Styles/Interface/OboutButton">
                    </cc2:OboutButton>

                    <cc2:OboutButton ID="btnHuy" runat="server" Text="Bỏ qua" Width="90px" OnClientClick="yesNoDialogXoaVanBan.Close(); return false;">
                    </cc2:OboutButton>
            </tr>
        </table>
    </div>
</owd:Dialog>

<%--///Thêm tổ chức--%>
<script>
    function luuThongTinToChucModule() {
        var ketQua = luuThongTinToChuc();
        if (ketQua == "1") {
            winDowThemToChuc.Close();
            var result = ProOnline.module.vbchuanbidt.danhMucToChuc().value;
            cboCoQuanBanHanh.options.clear();
            if (result != null) {
                for (var i = 0; i < result.Rows.length; i++) {
                    var maToChuc = result.Rows[i].maToChucpr;
                    var tenToChuc = result.Rows[i].tenToChuc;
                    cboCoQuanBanHanh.options.add(tenToChuc, maToChuc, i);
                }
            }
        }
        else if (ketQua == "0") {
            alert("Thông tin tổ chức chưa được lưu!");
        }
        else {
            alert(ketQua);
        }
        return false;
    }
</script>
<owd:Window ID="winDowThemToChuc" runat="server" IsModal="true" ShowCloseButton="true" Status="" Title="Thêm tổ chức"
    ShowStatusBar="false" RelativeElementID="WindowPositionHelper" Height="400" Width="600" VisibleOnLoad="false" StyleFolder="~/App_Themes/Styles/wdstyles/dogma"
    IsResizable="false" OnClientOpen="resetThongTinToChuc()">
    <div style="margin-top: 5px">
        <table>
            <tr>
                <td>
                    <cc2:OboutButton ID="btnLuuThongTinToChuc" runat="server" Text="Lưu và đóng" Width="100px"
                        OnClientClick="luuThongTinToChucModule();return false;" FolderStyle="App_Themes/Styles/Interface/OboutButton">
                    </cc2:OboutButton>
                </td>
                <td>
                    <cc2:OboutButton ID="btnDong" runat="server" Text="Đóng" Width="100px" OnClientClick="winDowThemToChuc.Close(); return false;">
                    </cc2:OboutButton>
                </td>
                <td style="width: 270px"></td>
            </tr>
        </table>
    </div>
    <div>
        <fieldset style="border: 1px solid #DBDBE1">
            <legend><b>Thông tin tổ chức</b></legend>
            <uc1:tochuc ID="tochuc1" runat="server" />

        </fieldset>
    </div>
</owd:Window>

<%--///Thêm cá nhân--%>
<script>
    function luuThongTinCaNhanModule() {
        var ketQua = luuThongTinCaNhan();
        if (ketQua == "1") {
            winDowThemCaNhan.Close();
            var result = ProOnline.module.vbchuanbidt.danhSachDoiTuongTheoToChuc(cboCoQuanBanHanh.value()).value;
            nguoiKyVanBan.options.clear();
            if (result != null) {
                for (var i = 0; i < result.Rows.length; i++) {
                    var maToChuc = result.Rows[i].tenCaNhan;
                    var tenToChuc = result.Rows[i].tenCaNhan;
                    nguoiKyVanBan.options.add(tenToChuc, maToChuc, i);
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
            <uc1:canhan ID="canhan1" runat="server" />

        </fieldset>
    </div>
</owd:Window>


<%--///Đính kèm file văn bản--%>
<script>
    function dinKemFileVanBan(sttVanBanpr) {
        document.getElementById("ctl00_ContentPlaceHolder1_vbchuanbidt1_userControlVanBanpr").value = sttVanBanpr;
        dinhDangWindow(winDowDinhKemFileVanBan);
        return false;
    }

    function uploadVanBanError(sender, args) {
        document.getElementById("msgDinhKemVanBan").innerHTML = "<br/>Kèm tập tin thất bại.";
        HienThiControl("statusDinhKemVanBan", false);
        return;
    }
    function uploadVanBanComplete(sender, args) {
        var fileExtension = args.get_fileName();
        if (fileExtension.indexOf('.pdf') != -1 || fileExtension.indexOf('.zip') != -1 || fileExtension.indexOf('.rar') != -1) {
            document.getElementById("msgDinhKemVanBan").innerHTML = "";
        } else {
            document.getElementById("msgDinhKemVanBan").innerHTML = "<br/>Tập tin không đúng định dạng.";
            return;
        }
        HienThiControl("statusDinhKemVanBan", false);
        winDowDinhKemFileVanBan.Close();
        gridVanBan.refresh();
    }
    function uploadVanBanStarted(sender, args) {
        document.getElementById("msgDinhKemVanBan").innerHTML = "";
        HienThiControl("statusDinhKemVanBan", true);
    }
    function xemVB(url) {
        window.open(url, 'mywindow', 'width=800,height=600');
        return false;
    }
    function xoaDinhKemVanBan(url, sttVB) {
        ProOnline.module.vbchuanbidt.xoaDinhKemFile(url, sttVB);
        gridVanBan.refresh();
        return false;
    }
</script>


<owd:Window ID="winDowDinhKemFileVanBan" runat="server" IsModal="true" ShowCloseButton="true" Top="0"
    Left="250" Height="150" Width="350" VisibleOnLoad="false" StyleFolder="~/App_Themes/Styles/wdstyles/dogma"
    Title="Chọn tệp tin">
    <div style="padding-top: 10px">
        <ajaxToolkit:ToolkitScriptManager runat="Server" EnablePartialRendering="true" ID="ToolkitScriptManager1" />
        <ajaxToolkit:AsyncFileUpload runat="server" ID="AsyncFileUpload1" OnClientUploadError="uploadVanBanError"
            ClientIDMode="AutoID" OnClientUploadComplete="uploadVanBanComplete" OnClientUploadStarted="uploadVanBanStarted" />
        <span style="font-style: italic; color: Red"><i>Chỉ cho phép tải lên file
                    có định dạng: *.pdf, *.zip, *.rar </i></span>
        <br />
        <br />
        <br />
        <a style="color: Blue; text-decoration: none"></a><span id="msgDinhKemVanBan" style="color: Red"></span>
        <div id="statusDinhKemVanBan" style="visibility: hidden; display: block;">
            <img alt="" src="/images/029.gif" width="120px" />
        </div>
    </div>
</owd:Window>
<asp:SqlDataSource ID="sdsCoQuanBanHanh" runat="server"></asp:SqlDataSource>
<asp:SqlDataSource ID="sdsLoaiVanBan" runat="server"></asp:SqlDataSource>
