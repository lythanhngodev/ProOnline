<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="baocaogiamsatdautu.ascx.cs" Inherits="ProOnline.module.baocaogiamsatdautu" %>
<%@ Register Assembly="obout_Window_NET" Namespace="OboutInc.Window" TagPrefix="owd" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>
<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc3" %>
<%@ Register TagPrefix="oem" Namespace="OboutInc.EasyMenu_Pro" Assembly="obout_EasyMenu_Pro" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="cc2" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="cc1" %>
<script>
    var thaoTacLuoi1 = 0;
    function setPosition() {
        var screenWidth = screen.width;
        var screenHeight = screen.height;
        var Window3DuAnSize = Window3DuAn.getSize();
        Window3DuAn.setPosition(parseFloat((parseFloat(screenWidth) - parseFloat(Window3DuAnSize.width)) / 2), 50);
    }
    function Grid_ChuTruongDauTu_OnBeforeClientAdd() {
        Grid1_duAn.refresh();
        setPosition();
        Window3DuAn.Open();
        return false;
    }
    
    function Grid_ChuTruongDauTu_OnBeforeClientDelete(record) {
        document.getElementById('ctl00_ContentPlaceHolder1_baocaogiamsatdautu1_hdfsttBCGSDauTuCTpr').value = record.sttBCGSDauTuCTpr;
        idDialogYesNo.Open();
        idDialogYesNo.screenCenter();
        return false;
    }
    function xoaBCGSCT() {
        var ketqua = ProOnline.module.baocaogiamsatdautu.xoaBCGSDauTuCT(document.getElementById('ctl00_ContentPlaceHolder1_baocaogiamsatdautu1_hdfsttBCGSDauTuCTpr').value).value;
        if (ketqua != "0") {
            idDialogYesNo.Close();
            Grid_ChuTruongDauTu.refresh();
            return false;
        }
        else {
            idDialogYesNo.Close();
            alert("Báo cáo giám sát chi tiết chưa được xóa.");
            return false;
        }
    }
    function Grid1_duAn_OnClientDblClick(index) {
        var arrDA = new Array();
        arrDA[0] = Grid1_duAn.Rows[index].Cells["sttDuAnpr_sd"].Value;
        arrDA[1] = Grid1_duAn.Rows[index].Cells["sttVBDApr"].Value;
        arrDA[2] = document.getElementById('ctl00_ContentPlaceHolder1_hdfsttBCGSDauTupr').value;
        arrDA[3] = document.getElementById('ctl00_ContentPlaceHolder1_soBuoc').value;
        ProOnline.module.baocaogiamsatdautu.themBCGSDauTuCT(arrDA);
        Grid1_duAn.deselectRecord(index);
        Window3DuAn.Close();
        Grid_ChuTruongDauTu.refresh();
        return false;
    }
    function _luuDuAn() {
        if (Grid1_duAn.SelectedRecords.length > 0) {
            for (var i = 0; i < Grid1_duAn.SelectedRecords.length; i++) {
                var record = Grid1_duAn.SelectedRecords[i];
                var arrDA = new Array();
                arrDA[0] = record.sttDuAnpr_sd;
                arrDA[1] = record.sttVBDApr;
                arrDA[2] = document.getElementById('ctl00_ContentPlaceHolder1_hdfsttBCGSDauTupr').value;
                arrDA[3] = document.getElementById('ctl00_ContentPlaceHolder1_soBuoc').value;
                ProOnline.module.baocaogiamsatdautu.themBCGSDauTuCT(arrDA);
            }
        }
        else {
            alert("Bạn chưa chọn dự án.");
            return false;
        }
        for (var i = 0; i < Grid1_duAn.SelectedRecords.length; i++) {
            Grid1_duAn.deselectRecord(i);
        }
        Window3DuAn.Close();
        Grid_ChuTruongDauTu.refresh();
        return false;
    }
</script>
<asp:HiddenField runat ="server" ID="hdfsttBCGSDauTuCTpr" />
<table style="width: 100%">
                    <tr style="height: 30px">
                        <td class="obDS_AdDNew" style="width:60px;" >
                            <div onclick="Grid_ChuTruongDauTu.addRecord();return false; ">
                                Thêm mới</div> 
                        </td> 
                      
                        <td align="right">
                            <input style="width: 200px" type="text" placeholder="Nội dung tìm kiếm..." onkeyup="searchValue(Grid_ChuTruongDauTu,1,this.value)"
                                class="searchCss">
                        </td>
                    </tr>
                </table>    
                <style>
                    .obDS_AdDNew, .obDS_SaveCancel {
                        font-family: arial,sans-serif,Verdana;
                        font-size: 12px;
                        color: #27489d;
                        font-weight: normal;
                        text-decoration: none;
                        padding-right: 1px;
                        cursor: pointer;
                    }
                </style>
 <cc1:Grid ID="Grid_ChuTruongDauTu" runat="server" AutoGenerateColumns="False" PageSize="50" AllowPaging="true" FilterType="ProgrammaticOnly"
     AllowAddingRecords="false" Height="404" FolderStyle="~/App_Themes/Styles/style_7" OnRebind="Grid_ChuTruongDauTu_OnRebind" AllowGrouping="true" GroupBy="loaiVanBan"
    AllowFiltering="true" EnableRecordHover="true" PageSizeOptions="50,100,200,500,-1" >
<ScrollingSettings ScrollHeight="340" NumberOfFixedColumns="0" ScrollWidth="968px" EnableVirtualScrolling="false" />
    <AddEditDeleteSettings AddLinksPosition="Top" />
     <ClientSideEvents OnBeforeClientAdd ="Grid_ChuTruongDauTu_OnBeforeClientAdd"
         OnBeforeClientDelete="Grid_ChuTruongDauTu_OnBeforeClientDelete" />
    <FilteringSettings MatchingType="AnyFilter" FilterPosition="Top" FilterLinksPosition="Bottom" />
    <Columns>
        <cc1:Column AllowEdit="false" AllowDelete="true" HeaderText="Thao tác" Width="60" ></cc1:Column>
         <cc1:Column DataField="sttBCGSDauTuCTpr" HeaderText="sttBCGSDauTuCTpr" Width="250" Visible="false">
        </cc1:Column>
         <cc1:Column DataField="maDuAn" HeaderText="Mã dự án" Width="250">
        </cc1:Column>
        <cc1:Column DataField="tenDuAn" HeaderText="Tên dự án" Width="250">
        </cc1:Column>
        <cc1:Column DataField="chuDauTu" HeaderText="Chủ đầu tư" Width="200" Wrap="true">
        </cc1:Column>
            <cc1:Column DataField="nhomDuAn" HeaderText="Nhóm dự án" Width="200" Wrap="true">
        </cc1:Column>
            <cc1:Column DataField="nguonVon" HeaderText="Nguồn vốn" Width="200" Wrap="true">
        </cc1:Column>
            <cc1:Column DataField="ngayPheDuyet" HeaderText="Ngày phê duyệt chủ trương đầu tư" Width="200" Wrap="true">
        </cc1:Column>
            <cc1:Column DataField="thoiGian" HeaderText="Ngày KC-HT" Width="200" Wrap="true">
        </cc1:Column>
           <cc1:Column DataField="loaiVanBan" Visible="false" HeaderText="" >
        </cc1:Column>
        
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
        Paging_PageSizeText="Số dòng 1 trang:" Paging_PagesText="Trang:" Paging_RecordsText="Dòng:"
        ResizingTooltipWidth="Rộng:" SaveAllLink="Lưu tất cả" SaveLink="Lưu" TypeValidationError="Giá trị mà bạn đã nhập vào trong cột XXXXX là không đúng."
        UndeleteLink="Không xóa" UpdateLink="Lưu" />
</cc1:Grid>

<owd:Window ID="Window3DuAn" runat="server" IsModal="true" ShowCloseButton="true"
            Status="" ShowStatusBar="false" RelativeElementID="WindowPositionHelper" Height="500"
            Width="800" VisibleOnLoad="false" StyleFolder="~/App_Themes/Styles/wdstyles/dogma"
            Title="Chọn dự án" IsResizable="false">
    <table style="padding:5px">
        <tr>
            <td style="width:200px">
                  <cc2:OboutButton ID="btnChon" runat="server" Text="Chọn" FolderStyle="~/App_Themes/Styles/Interface/OboutButton" Width="90px" OnClientClick="_luuDuAn(); return false;">
                            </cc2:OboutButton>
                  <cc2:OboutButton ID="btnThoat" runat="server" Text="Thoát" FolderStyle="~/App_Themes/Styles/Interface/OboutButton" Width="90px" OnClientClick="Window3DuAn.Close(); return false;">
                            </cc2:OboutButton>
            </td>
            <td style="text-align:right;width:600px">
                 <input style="width: 200px" type="text" placeholder="Nội dung tìm kiếm..." onkeyup="searchValue(Grid1_duAn,1,this.value)" class="searchCss">
            </td>
        </tr>
    </table>
    <cc1:Grid ID="Grid1_duAn" runat="server" AutoGenerateColumns="False" PageSize="15" AllowPaging="true" FilterType="ProgrammaticOnly"
     AllowAddingRecords="false" Height="480" FolderStyle="~/App_Themes/Styles/style_7" OnRebind="Grid1_duAn_OnRebind" 
    AllowFiltering="true" EnableRecordHover="true" PageSizeOptions="5,10,15,20,-1">
     <ScrollingSettings ScrollHeight="340" NumberOfFixedColumns="0" ScrollWidth="800px" EnableVirtualScrolling="false" />
     <ClientSideEvents  OnClientDblClick="Grid1_duAn_OnClientDblClick" />
    <FilteringSettings MatchingType="AnyFilter" FilterPosition="Top" FilterLinksPosition="Bottom" />
    <Columns>
        <cc1:CheckBoxSelectColumn Width="40px"></cc1:CheckBoxSelectColumn>
        <cc1:Column DataField="maDuAn" HeaderText="Mã dự án" Width="140">
        </cc1:Column>
        <cc1:Column DataField="tenDuAn" HeaderText="Tên dự án" Width="250">
        </cc1:Column>
        <cc1:Column DataField="chuDauTu" HeaderText="Chủ đầu tư" Width="170" Wrap="true">
        </cc1:Column>
            <cc1:Column DataField="diaDiemXD" HeaderText="Địa điểm XD" Width="250" Wrap="true">
        </cc1:Column>
            <cc1:Column DataField="sttDuAnpr_sd" HeaderText="sttDuAnpr_sd" Visible="false" Width="200" Wrap="true">
        </cc1:Column>
            <cc1:Column DataField="sttVBDApr" HeaderText="sttVBDApr" Visible="false" Width="200" Wrap="true">
        </cc1:Column>
          <cc1:Column DataField="" HeaderText="" Width="20" Wrap="true">
        </cc1:Column>
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
        Paging_PageSizeText="Số dòng 1 trang:" Paging_PagesText="Trang:" Paging_RecordsText="Dòng:"
        ResizingTooltipWidth="Rộng:" SaveAllLink="Lưu tất cả" SaveLink="Lưu" TypeValidationError="Giá trị mà bạn đã nhập vào trong cột XXXXX là không đúng."
        UndeleteLink="Không xóa" UpdateLink="Lưu" />
</cc1:Grid>
</owd:Window>
   <owd:Dialog ID="idDialogYesNo" runat="server" IsModal="true" ShowCloseButton="true"
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
                            <cc2:OboutButton ID="btnDongY" runat="server" Text="Thực hiện" FolderStyle="~/App_Themes/Styles/Interface/OboutButton" Width="90px" OnClientClick="xoaBCGSCT(); return false;">
                            </cc2:OboutButton>
                        </td>
                        <td>
                            <cc2:OboutButton ID="btnHuy" runat="server" Text="Bỏ qua" Width="90px" OnClientClick="idDialogYesNo.Close(); return false;">
                            </cc2:OboutButton>
                        </td>
                    </tr>
                </table>
            </div>
        </owd:Dialog>

