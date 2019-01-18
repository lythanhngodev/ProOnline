<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="chonduan_us.ascx.cs" Inherits="ProOnline.UserControl.chonduan_us" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="cc2_us" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="cc1_us" %>
<div id="chonduan_us" class="modal fade" tabindex="-1" data-backdrop="static">
    <div class="modal-dialog" style="width: 60%">
        <div class="modal-content">
            <div class="modal-header no-padding">
                <div class="table-header">
                    <button id="btnDongChonDuAn_us1" type="button" class="close" data-dismiss="modal" aria-hidden="true" >
                        <span class="white">&times;</span>
                    </button>
                    <span>Chọn dự án</span>
                </div>
            </div>
            <div class="modal-body">
                <div>
                    <div class="row" style="padding-bottom: 4px">
                        <div class="col-md-12" style="padding: 5px; padding-right: 10px;">
                            <span class="input-icon" style="float: right;">
                                <input type="text" placeholder="Nhập vào từ cần tìm kiếm..." class="nav-search-input" id="Text13" autocomplete="off" onkeyup="searchValue(GridChonDuAn_us,1,this.value)">
                                <i class="ace-icon fa fa-search nav-search-icon"></i>
                            </span>
                        </div>
                    </div>
                    <div class="row" style="padding-bottom: 4px">
                        <div class="col-md-12">
                            <cc1_us:Grid ID="GridChonDuAn_us" runat="server" AutoGenerateColumns="False" PageSize="10" AllowPaging="true" FilterType="ProgrammaticOnly"
                                AllowAddingRecords="False" Height="412" Width="100%" FolderStyle="~/App_Themes/Styles/style_7"
                                AllowFiltering="true" EnableRecordHover="true" AllowMultiRecordSelection="false"
                                PageSizeOptions="5,10,15,20" OnRebind="GridChonDuAn_us_OnRebind">
                                <AddEditDeleteSettings AddLinksPosition="Top" />
                                <PagingSettings Position="Bottom" />
                                <FilteringSettings MatchingType="AnyFilter" FilterPosition="Top" FilterLinksPosition="Bottom" />
                                <ScrollingSettings ScrollHeight="100%" ScrollWidth="100%" EnableVirtualScrolling="false" />
                                <ClientSideEvents OnClientDblClick="GridChonDuAn_us_onDoubleClick" />
                                <Columns>
                                    <cc1_us:Column DataField="sttDuAnpr" HeaderText="sttDuAnpr" Width="10" Visible="false">
                                    </cc1_us:Column>
                                    <cc1_us:Column DataField="maDuAn" HeaderText="Mã" Width="100">
                                    </cc1_us:Column>
                                    <cc1_us:Column DataField="tenDuAn" HeaderText="Tên dự án" Width="350" Wrap="true">
                                    </cc1_us:Column>
                                    <cc1_us:Column DataField="tenChuDauTu" HeaderText="Chủ đầu tư" Width="200" Wrap="true">
                                    </cc1_us:Column>
                                    <cc1_us:Column DataField="diaDiemXD" HeaderText="Địa điểm XD" Width="350" Wrap="true">
                                    </cc1_us:Column>
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
                            </cc1_us:Grid>
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <a id="btnChonDuAn_us" href="#" class="btn btn-primary">
                    <i class="fa fa-check"></i>&nbsp;Chọn và đóng</a> &nbsp;&nbsp;
                               <a  id="btnDongChonDuAn_us2" href="#"  class="btn btn-danger" data-dismiss="modal"><i class="fa fa-close"></i>&nbsp;Đóng</a>
            </div>
        </div>
    </div>
</div>
<script>
    function layDuLieuDuAn_us() {
        GridChonDuAn_us.refresh();
    }
    </script>
