<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="thongtinduan_b1.ascx.cs" Inherits="ProOnline.module.thongtinduan_b1" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="cc2_ucthongtinduan_b1" %>
<%@ Register Assembly="obout_Window_NET" Namespace="OboutInc.Window" TagPrefix="owd_ucthongtinduan_b1" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="cc1_ucthongtinduan_b1" %>
<script>
    function btnChonDAClick_uc() {
        GridChonDuAn_uc.refresh();
        WindowChonDuAn_uc.Open();
        WindowChonDuAn_uc.screenCenter();
        return false;
    }
    var arrDA = new Array();
    function GridChonDuAnOnClientSelect_uc(record) {
       
        arrDA[0] = record[0].sttDuAnpr_uc;
        arrDA[1] = record[0].maDuAn_uc;
        arrDA[2] = record[0].tenDuAn_uc;
        arrDA[3] = record[0].tenChuDauTu_uc;
        arrDA[4] = record[0].diaDiemXD_uc;
        return false;
    }
    function GridChonDuAnOnDoubleClick_uc(record) {
        if (ntsLibraryFunctions.checkDuyetQT(GridChonDuAn_uc.Rows[record].Cells[0].Value).value == "1") {
            alert("Dự án đã được quyết toán, bạn không thể thực hiện thao tác này")
            return false;
        }
        arrDA[0] = GridChonDuAn_uc.Rows[record].Cells[0].Value;
        arrDA[1] = GridChonDuAn_uc.Rows[record].Cells[1].Value;
        arrDA[2] = GridChonDuAn_uc.Rows[record].Cells[2].Value;
        arrDA[3] = GridChonDuAn_uc.Rows[record].Cells[3].Value;
        arrDA[4] = GridChonDuAn_uc.Rows[record].Cells[4].Value;
        document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value = arrDA[0];
        ketqua = ProOnline.module.thongtinduan_b1.layThongTinDuAn_uc(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value);
        if (ketqua.value.Rows.length > 0) {
            txtMaDA_uc.value(ketqua.value.Rows[0].maDuAn_uc);
            txtTenDA_uc.value(ketqua.value.Rows[0].tenDuAn_uc);
            txtChuDauTu_uc.value(ketqua.value.Rows[0].tenChuDauTu_uc);
            txtDiaDiemXD_uc.value(ketqua.value.Rows[0].diaDiemXD_uc);
            txtNguonVon_uc.value(ProOnline.module.thongtinduan_b1.DMNguonVonTheoDuAn_uc(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value).value);
            txtTongMucDauTu_uc.value(formatNumber(ProOnline.module.thongtinduan_b1.layTongMucDauTu_uc(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value).value));
            txtThoiGianThucHien_uc.value(ketqua.value.Rows[0].thoiGianThucHien_uc);
        }
        else {
            txtMaDA_uc.value('');
            txtTenDA_uc.value('');
            txtChuDauTu_uc.value('');
            txtDiaDiemXD_uc.value('');
            txtNguonVon_uc.value('');
            txtTongMucDauTu_uc.value('');
            txtThoiGianThucHien_uc.value('');
            document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value = '';
        }
        WindowChonDuAn_uc.Close();
        arrDA = new Array();
        return false;
    }
    function btnChonDuAnClick_uc() {
        if (ntsLibraryFunctions.checkDuyetQT(arrDA[0]).value == "1") {
            alert("Dự án đã được quyết toán, bạn không thể thực hiện thao tác này")
            return false;
        }
        if (arrDA.length > 0) {
            document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value = arrDA[0];
            ketqua = ProOnline.module.thongtinduan_b1.layThongTinDuAn_uc(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value);
            if (ketqua.value.Rows.length > 0) {
                txtMaDA_uc.value(ketqua.value.Rows[0].maDuAn_uc);
                txtTenDA_uc.value(ketqua.value.Rows[0].tenDuAn_uc);
                txtChuDauTu_uc.value(ketqua.value.Rows[0].tenChuDauTu_uc);
                txtDiaDiemXD_uc.value(ketqua.value.Rows[0].diaDiemXD_uc);
                txtNguonVon_uc.value(ProOnline.module.thongtinduan_b1.DMNguonVonTheoDuAn_uc(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value).value);
                txtTongMucDauTu_uc.value(formatNumber(ProOnline.module.thongtinduan_b1.layTongMucDauTu_uc(document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value).value));
                txtThoiGianThucHien_uc.value(ketqua.value.Rows[0].thoiGianThucHien_uc);
            }
            else {
                txtMaDA_uc.value('');
                txtTenDA_uc.value('');
                txtChuDauTu_uc.value('');
                txtDiaDiemXD_uc.value('');
                txtNguonVon_uc.value('');
                txtTongMucDauTu_uc.value('');
                txtThoiGianThucHien_uc.value('');
                document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value = '';
            }

        } else {
            alert("Không có dòng dữ liệu nào được chọn!");
        }
        WindowChonDuAn_uc.Close();
        arrDA = new Array();
        return false;
    }
    function layThongTinDuAn_uc(sttDuAnpr) {
        debugger;
        if (sttDuAnpr == "0" && ntsLibraryFunctions.checkDuyetQT(ProOnline.module.thongtinduan_b1.setSttDuAnMatDinh().value).value == "1") {
            txtMaDA_uc.value('');
            txtTenDA_uc.value('');
            txtChuDauTu_uc.value('');
            txtDiaDiemXD_uc.value('');
            txtNguonVon_uc.value('');
            txtTongMucDauTu_uc.value('');
            txtThoiGianThucHien_uc.value('');
            document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value = '';
            alert("Dự án mặc định đã phê duyệt quyết toán nên bạn không thể thêm mới chứng từ!");
            return false;
        }
        if (sttDuAnpr != "" && sttDuAnpr != "0" && sttDuAnpr != '') {
            sttDuAnpr = sttDuAnpr;
        }
        else {
            sttDuAnpr = ProOnline.module.thongtinduan_b1.setSttDuAnMatDinh().value;
        }
        ketqua = ProOnline.module.thongtinduan_b1.layThongTinDuAn_uc(sttDuAnpr);
        if (ketqua.value.Rows.length > 0) {
            txtMaDA_uc.value(ketqua.value.Rows[0].maDuAn_uc);
            txtTenDA_uc.value(ketqua.value.Rows[0].tenDuAn_uc);
            txtChuDauTu_uc.value(ketqua.value.Rows[0].tenChuDauTu_uc);
            txtDiaDiemXD_uc.value(ketqua.value.Rows[0].diaDiemXD_uc);
            txtNguonVon_uc.value(ProOnline.module.thongtinduan_b1.DMNguonVonTheoDuAn_uc(sttDuAnpr).value);
            txtTongMucDauTu_uc.value(formatNumber(ProOnline.module.thongtinduan_b1.layTongMucDauTu_uc(sttDuAnpr).value));
            txtThoiGianThucHien_uc.value(ketqua.value.Rows[0].thoiGianThucHien_uc);
            document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value = sttDuAnpr;
        }
        else {
            txtMaDA_uc.value('');
            txtTenDA_uc.value('');
            txtChuDauTu_uc.value('');
            txtDiaDiemXD_uc.value('');
            txtNguonVon_uc.value('');
            txtTongMucDauTu_uc.value('');
            txtThoiGianThucHien_uc.value('');
            document.getElementById("ctl00_ContentPlaceHolder1_hdfSttDuAnpr_uc").value = '';
        }
        return false;

    }
</script>
<fieldset style="border: 1px solid #DBDBE1; margin: 3px; padding: 4px">
    <legend><b>Thông tin dự án</b></legend>
    <table style="width: 100%">
        <tr>
            <td style="width: 200px">Dự án
            </td>
            <td style="width: 18%">
                <cc2_ucthongtinduan_b1:OboutTextBox ID="txtMaDA_uc" runat="server" ReadOnly="true" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox">
                </cc2_ucthongtinduan_b1:OboutTextBox>
            </td>
            <td style="width: 55%">
                <cc2_ucthongtinduan_b1:OboutTextBox ID="txtTenDA_uc" runat="server" ReadOnly="true" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox">
                </cc2_ucthongtinduan_b1:OboutTextBox>
            </td>
            <td title="Chọn dự án" style="text-align: right">
                <cc2_ucthongtinduan_b1:OboutButton ID="btnChonDA_uc" runat="server" Text="Chọn dự án" Width="100px" FolderStyle="App_Themes/Styles/Interface/OboutButton"
                    OnClientClick="btnChonDAClick_uc();return false;">
                </cc2_ucthongtinduan_b1:OboutButton>
            </td>
            <td style="width: 10px">
                <span style="color: red">*</span>
            </td>
        </tr>
        <tr>
            <td>Tổng mức đầu tư
            </td>
            <td colspan="3">
                <cc2_ucthongtinduan_b1:OboutTextBox ID="txtTongMucDauTu_uc" ReadOnly="true" runat="server" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox">
                </cc2_ucthongtinduan_b1:OboutTextBox>
            </td>
        </tr>
        <tr>
            <td>Chủ đầu tư
            </td>
            <td colspan="3">
                <cc2_ucthongtinduan_b1:OboutTextBox ID="txtChuDauTu_uc" ReadOnly="true" runat="server" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox">
                </cc2_ucthongtinduan_b1:OboutTextBox>
            </td>
        </tr>
        <tr>
            <td>Nguồn vốn
            </td>
            <td colspan="3">
                <cc2_ucthongtinduan_b1:OboutTextBox ID="txtNguonVon_uc" ReadOnly="true" runat="server" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox">
                </cc2_ucthongtinduan_b1:OboutTextBox>
            </td>
        </tr>
        <tr>
            <td>Thời gian thực hiện
            </td>
            <td colspan="3">
                <cc2_ucthongtinduan_b1:OboutTextBox ID="txtThoiGianThucHien_uc" ReadOnly="true" runat="server" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox">
                </cc2_ucthongtinduan_b1:OboutTextBox>
            </td>
        </tr>
        <tr>
            <td title="Địa điểm xây dựng">Địa điểm, quy mô dự án
            </td>
            <td colspan="3">
                <cc2_ucthongtinduan_b1:OboutTextBox ID="txtDiaDiemXD_uc" ReadOnly="true" runat="server" Width="100%" FolderStyle="App_Themes/Styles/Interface/OboutTextBox">
                </cc2_ucthongtinduan_b1:OboutTextBox>
            </td>
        </tr>
    </table>
    <%--  Chọn dự án--%>
    <div>
        <owd_ucthongtinduan_b1:Window ID="WindowChonDuAn_uc" runat="server" IsModal="true" ShowCloseButton="true"
            Status="" ShowStatusBar="false" RelativeElementID="WindowPositionHelper" Height="500"
            Width="800" VisibleOnLoad="false" StyleFolder="~/App_Themes/Styles/wdstyles/dogma"
            Title="Chọn dự án" IsResizable="false">
            <div style="margin-top: 5px">
                <table style="width: 100%">
                    <tr>
                        <td>
                            <cc2_ucthongtinduan_b1:OboutButton ID="btnChonDuAn_uc" runat="server" Text="Chọn" Width="80px" OnClientClick="btnChonDuAnClick_uc();return false;"
                                FolderStyle="App_Themes/Styles/Interface/OboutButton">
                            </cc2_ucthongtinduan_b1:OboutButton>
                            <cc2_ucthongtinduan_b1:OboutButton ID="btnDong_uc" runat="server" Text="Đóng" Width="80px" OnClientClick="WindowChonDuAn_uc.Close(); return false;">
                            </cc2_ucthongtinduan_b1:OboutButton>
                        </td>
                        <td align="right">
                            <input type="text" style="width: 250px" placeholder="Nội dung tìm kiếm..." onkeyup="searchValue(GridChonDuAn_uc,1,this.value)"
                                class="searchCss" />
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <cc1_ucthongtinduan_b1:Grid ID="GridChonDuAn_uc" runat="server" AutoGenerateColumns="False" PageSize="15" AllowPaging="true"
                    AllowAddingRecords="false" Height="430" Width="100%" FolderStyle="~/App_Themes/Styles/style_7"
                    FilterType="ProgrammaticOnly" AllowFiltering="true" EnableRecordHover="true"
                    PageSizeOptions="15,30,50,100,300,500" OnRebind="GridChonDuAn_uc_OnRebind" AllowMultiRecordSelection="false">
                    <ScrollingSettings ScrollHeight="430" ScrollWidth="790px" EnableVirtualScrolling="false" />
                    <PagingSettings Position="Bottom" />
                    <FilteringSettings MatchingType="AnyFilter" FilterPosition="Top" FilterLinksPosition="Bottom" />
                    <ClientSideEvents OnClientSelect="GridChonDuAnOnClientSelect_uc" OnClientDblClick="GridChonDuAnOnDoubleClick_uc" />
                    <Columns>
                        <cc1_ucthongtinduan_b1:Column DataField="sttDuAnpr_uc" HeaderText="sttDuAnpr" Width="10" Visible="false">
                        </cc1_ucthongtinduan_b1:Column>
                        <cc1_ucthongtinduan_b1:Column DataField="maDuAn_uc" HeaderText="Mã" Width="100">
                        </cc1_ucthongtinduan_b1:Column>
                        <cc1_ucthongtinduan_b1:Column DataField="tenDuAn_uc" HeaderText="Tên dự án" Width="350" Wrap="true">
                        </cc1_ucthongtinduan_b1:Column>
                        <cc1_ucthongtinduan_b1:Column DataField="tenChuDauTu_uc" HeaderText="Chủ đầu tư" Width="240" Wrap="true">
                        </cc1_ucthongtinduan_b1:Column>
                        <cc1_ucthongtinduan_b1:Column DataField="diaDiemXD_uc" HeaderText="Địa điểm XD" Width="350" Wrap="true">
                        </cc1_ucthongtinduan_b1:Column>
                    </Columns>
                    <LocalizationSettings AddLink="Thêm mới" CancelAllLink="Hủy tất cả" CancelLink="Hủy"
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
                        ModifyLink="Chỉnh sửa" NoRecordsText="Không có dữ liệu" Paging_ManualPagingLink="Chuyển »"
                        Paging_PageSizeText="Số dòng:" Paging_PagesText="Trang:" Paging_RecordsText="Dòng:"
                        ResizingTooltipWidth="Rộng:" SaveAllLink="Lưu tất cả" SaveLink="Lưu" TypeValidationError="Giá trị mà bạn đã nhập vào trong cột XXXXX là không đúng."
                        UndeleteLink="Không xóa" UpdateLink="Lưu" />
                    <Templates>
                        <cc1_ucthongtinduan_b1:GridTemplate runat="server" ID="GridTemplateDuAn_uc">
                            <Template>
                                <b>Dự Án</b>
                            </Template>
                        </cc1_ucthongtinduan_b1:GridTemplate>
                    </Templates>
                </cc1_ucthongtinduan_b1:Grid>
            </div>

        </owd_ucthongtinduan_b1:Window>
    </div>
</fieldset>
