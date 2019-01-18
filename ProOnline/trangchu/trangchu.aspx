<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="trangchu.aspx.cs"
    Inherits="ProOnline.trangchu.trangchu" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc3" %>
<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Window_NET" Namespace="OboutInc.Window" TagPrefix="owd" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="cc2" %>

<script language="C#" runat="server">	
    string number = "#,##0;(#,##0);0";
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <%--<script type="text/javascript" src="/js/jquery-2.2.3.min.js"></script>--%>

    <script src="../js/jquery.min.js" type="text/javascript"></script>
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

        #settingSear:hover {
            background-color: #eee;
        }

        .TTcaiDat {
            min-height: 120px;
            width: 300px;
            background: #FFFFFF;
            position: absolute;
            box-shadow: 0px 0px 10px #CCC;
            -moz-box-shadow: 0px 0px 10px #CCC;
            -webkit-box-shadow: 0px 0px 10px #CCC;
            padding: 10px 20px 5px 20px;
            z-index: 999;
            font-size: 12px;
            line-height: 20px;
            border: 1px solid #CCCCCC;
            margin: 0px auto 0px auto;
        }

        .cTrang {
            padding: 3px 5px;
            text-decoration: none;
            color: #4285F4;
        }

            .cTrang:hover {
                text-decoration: underline;
            }

            .cTrang:visited {
                text-decoration: none;
                color: #000;
            }
        /*
        .cTrang a:link
        {
            color: blue;
        }
        .cTrang a:hover
        {
            text-decoration:underline;
        }
        .cTrang a:active
        {
            background-color: red;
        }
        .cTrang a:visited
        {
            background-color: red;
        }*/ a:link {
            color: Blue;
            text-decoration: none;
        }

        a:hover {
            color: Blue;
            text-decoration: underline;
        }
        /* Show Popup*/ /* Outer */

        .popup {
            width: 100%;
            height: 100%;
            display: none;
            position: fixed;
            top: 0px;
            left: 0px;
            background: rgba(0,0,0,0.75);
            z-index: 1000;
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

        @-webkit-keyframes my {
            0% {
                color: red;
            }

            50% {
                color: #fff;
            }

            100% {
                color: red;
            }
        }

        @-moz-keyframes my {
            0% {
                color: red;
            }

            50% {
                color: #fff;
            }

            100% {
                color: red;
            }
        }

        @-o-keyframes my {
            0% {
                color: red;
            }

            50% {
                color: #fff;
            }

            100% {
                color: red;
            }
        }

        @keyframes my {
            0% {
                color: red;
            }

            50% {
                color: #fff;
            }

            100% {
                color: red;
            }
        }
    </style>

    <script type="text/javascript">



        $(document).ready(function () {
            $("#divcaiDat").hide(); // Giấu đối tượng
            $('#settingSear').click(function () {//Khi nút được nhấn
                $("#divcaiDat").css("visibility", "visible");
                //$("#divcaiDat").show();
                $("#divcaiDat").toggle();
            });
            //       $('#settingSear').mouseout(function(){ // Khi nhấn chuột vào chỗ khác
            //            $('*:not(#settingSear)').click(function(){
            //                $("#divcaiDat").hide();
            //            }); 
            //        });
            var kq = ProOnline.trangchu.trangchu.kiemTraThongBaoNangCap();
            if (kq.value == true) {
                $('[data-popup="popup-2"]').fadeIn(350);
                $(function () {
                    //----- OPEN
                    $('[data-popup-open]').on('click', function (e) {
                        var targeted_popup_class = jQuery(this).attr('data-popup-open');
                        $('[data-popup="' + targeted_popup_class + '"]').fadeIn(350);
                        //alert(targeted_popup_class);
                        // Grid1.refresh();
                        // Grid2.refresh();
                        // Grid3.refresh();
                        e.preventDefault();
                    });

                    //----- CLOSE
                    $('[data-popup-close]').on('click', function (e) {
                        var targeted_popup_class = jQuery(this).attr('data-popup-close');
                        $('[data-popup="' + targeted_popup_class + '"]').fadeOut(350);

                        e.preventDefault();
                    });
                });
            }
        });
        //
        var tongSoTrang = "0";
        function NapDuLieu() {
            var param = new Array();
            param[0] = txtTimKiem.value();
            document.getElementById("ctl00_ContentPlaceHolder1_hdfPage").value = "1";

            var kq = ProOnline.trangchu.trangchu.taoPhanTrang(param);
            if (kq.value.Rows.length > 0) {
                document.getElementById("phanTrang").innerHTML = kq.value.Rows[0].phanTrang;
                tongSoTrang = kq.value.Rows[0].tongSoTrang;
            }
            if (parseInt(tongSoTrang) >= 5) {
                var val = parseInt(document.getElementById("ctl00_ContentPlaceHolder1_hdfPage").value);
                for (var k = 1; k <= tongSoTrang; k++) {
                    if (k >= (val) && k <= (val + 4)) {
                        document.getElementById("trang" + k).style.visibility = "visible";
                        document.getElementById("trang" + k).style.display = "inline";
                    } else {
                        document.getElementById("trang" + k).style.visibility = "hidden";
                        document.getElementById("trang" + k).style.display = "none";
                    }
                }
                loadPage(document.getElementById("ctl00_ContentPlaceHolder1_hdfPage").value);
            } else {
                for (var k = 1; k <= tongSoTrang; k++) {
                    document.getElementById("trang" + k).style.visibility = "visible";
                    document.getElementById("trang" + k).style.display = "inline";
                }
                loadPage(document.getElementById("ctl00_ContentPlaceHolder1_hdfPage").value);
            }

        }

        //Gan phan trang
        function loadPage(val) {
            document.getElementById("cTrang" + document.getElementById("ctl00_ContentPlaceHolder1_hdfPage").value).style.color = "#4285F4";
            document.getElementById("ctl00_ContentPlaceHolder1_hdfPage").value = val + "";
            document.getElementById("cTrang" + val).style.color = "#000";
            //Xuất dữ liệu
            var param1 = new Array();
            param1[0] = txtTimKiem.value();
            param1[1] = document.getElementById("ctl00_ContentPlaceHolder1_hdfPage").value;

            var dulieu = ProOnline.trangchu.trangchu.layDuLieu(param1).value;

            document.getElementById("duLieuTimKiem").innerHTML = dulieu;

            //$("#duLieuTimKiem").html(dulieu);
            if (parseInt(val) % 5 == 0 && parseInt(val) < tongSoTrang) {
                for (var m = parseInt(val) - (parseInt(val) - 1) ; m < parseInt(val) ; m++) {
                    document.getElementById("trang" + m).style.visibility = "hidden";
                    document.getElementById("trang" + m).style.display = "none";
                }
                for (var k = parseInt(val) ; k < (parseInt(val) + 6) ; k++) {
                    document.getElementById("trang" + k).style.visibility = "visible";
                    document.getElementById("trang" + k).style.display = "inline";
                }
                if ((parseInt(val) + 6) < tongSoTrang) {
                    for (var n = parseInt(val) + 6; n < tongSoTrang + 1; n++) {
                        document.getElementById("trang" + n).style.visibility = "hidden";
                        document.getElementById("trang" + n).style.display = "none";
                    }
                }
            }
        }
        function loadPageDau() {
            document.getElementById("cTrang" + document.getElementById("ctl00_ContentPlaceHolder1_hdfPage").value).style.color = "#4285F4";
            document.getElementById("ctl00_ContentPlaceHolder1_hdfPage").value = "1";
            if (parseInt(tongSoTrang) >= 5) {
                var val = parseInt(document.getElementById("ctl00_ContentPlaceHolder1_hdfPage").value);
                for (var k = 1; k <= tongSoTrang; k++) {
                    if (k >= (val) && k <= (val + 4)) {
                        document.getElementById("trang" + k).style.visibility = "visible";
                        document.getElementById("trang" + k).style.display = "inline";
                    } else {
                        document.getElementById("trang" + k).style.visibility = "hidden";
                        document.getElementById("trang" + k).style.display = "none";
                    }
                }
                loadPage(document.getElementById("ctl00_ContentPlaceHolder1_hdfPage").value);
            } else {
                for (var k = 1; k <= tongSoTrang; k++) {
                    document.getElementById("trang" + k).style.visibility = "visible";
                    document.getElementById("trang" + k).style.display = "inline";
                }
                loadPage(document.getElementById("ctl00_ContentPlaceHolder1_hdfPage").value);
            }
        }
        function loadPageCuoi() {
            document.getElementById("cTrang" + document.getElementById("ctl00_ContentPlaceHolder1_hdfPage").value).style.color = "#4285F4";
            document.getElementById("ctl00_ContentPlaceHolder1_hdfPage").value = tongSoTrang + "";
            if (parseInt(tongSoTrang) >= 5) {
                var val = parseInt(document.getElementById("ctl00_ContentPlaceHolder1_hdfPage").value);
                var t = val;
                if (val > val - (val % 5)) {
                    t = val - (val % 5);
                } else {
                    t = t - 5;
                }
                for (var k = 1; k <= tongSoTrang; k++) {
                    if (k >= (t) && k <= (val)) {
                        document.getElementById("trang" + k).style.visibility = "visible";
                        document.getElementById("trang" + k).style.display = "inline";
                    } else {
                        document.getElementById("trang" + k).style.visibility = "hidden";
                        document.getElementById("trang" + k).style.display = "none";
                    }
                }
                loadPage(document.getElementById("ctl00_ContentPlaceHolder1_hdfPage").value);
            } else {
                for (var k = 1; k <= tongSoTrang; k++) {
                    document.getElementById("trang" + k).style.visibility = "visible";
                    document.getElementById("trang" + k).style.display = "inline";
                }
                loadPage(document.getElementById("ctl00_ContentPlaceHolder1_hdfPage").value);
            }
        }
        function loadPageTruoc() {
            var val = parseInt(document.getElementById("ctl00_ContentPlaceHolder1_hdfPage").value);
            if (val > 1) {
                document.getElementById("cTrang" + val).style.color = "#4285F4";
                document.getElementById("ctl00_ContentPlaceHolder1_hdfPage").value = (val - 1) + "";
                loadPage(document.getElementById("ctl00_ContentPlaceHolder1_hdfPage").value);
                if (parseInt(val) % 5 == 0) {
                    for (var k = 1; k <= tongSoTrang; k++) {
                        if (k >= (val - 5) && k <= (val)) {
                            document.getElementById("trang" + k).style.visibility = "visible";
                            document.getElementById("trang" + k).style.display = "inline";
                        } else {
                            document.getElementById("trang" + k).style.visibility = "hidden";
                            document.getElementById("trang" + k).style.display = "none";
                        }
                    }
                }
            }
        }
        function loadPageSau() {
            var val = parseInt(document.getElementById("ctl00_ContentPlaceHolder1_hdfPage").value);
            if (val < tongSoTrang) {
                document.getElementById("cTrang" + val).style.color = "#4285F4";
                document.getElementById("ctl00_ContentPlaceHolder1_hdfPage").value = (val + 1) + "";
                document.getElementById("cTrang" + (val + 1)).style.color = "#000";
                loadPage(document.getElementById("ctl00_ContentPlaceHolder1_hdfPage").value);
                if (parseInt(val) % 5 == 0) {
                    for (var k = 1; k <= tongSoTrang; k++) {
                        if (k >= (val) && k <= (val + 5)) {
                            document.getElementById("trang" + k).style.visibility = "visible";
                            document.getElementById("trang" + k).style.display = "inline";
                        } else {
                            document.getElementById("trang" + k).style.visibility = "hidden";
                            document.getElementById("trang" + k).style.display = "none";
                        }
                    }
                }
            }
        }
        ////
        //    $(function() {
        //    //----- OPEN
        //    $('[data-popup-open]').on('click', function(e)  {
        //        var targeted_popup_class = jQuery(this).attr('data-popup-open');
        //        $('[data-popup="' + targeted_popup_class + '"]').fadeIn(350);
        //        
        //        Grid1.refresh();
        //        Grid2.refresh();
        //        Grid3.refresh();
        //        e.preventDefault();
        //    });
        // 
        //    //----- CLOSE
        //    $('[data-popup-close]').on('click', function(e)  {
        //        var targeted_popup_class = jQuery(this).attr('data-popup-close');
        //        $('[data-popup="' + targeted_popup_class + '"]').fadeOut(350);
        // 
        //        e.preventDefault();
        //    });
        //    });
        function btnTiepTuc_OnClientClick() {
            debugger;
            if (rdoKhongHienThiLS.checked() == true) {
                dulieu = ProOnline.trangchu.trangchu.khongHienThiLanSau().value;
            }
            $('[data-popup="popup-2"]').fadeOut(350);
            return false;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdfPage" runat="server" />
    <asp:HiddenField ID="hdfSTTDA" runat="server" Value="0" />
    <div style="font-family: arial,sans-serif,Verdana">
        <fieldset style="border: 1px solid #DBDBE1; margin: -7px 1px 0px 1px">
            <div style="font-size: 20px; font-weight: bold; color: #0078D7; text-align: center; height: 30px; margin-top: 5px">
                HỆ THỐNG THÔNG TIN QUẢN LÝ NGÂN SÁCH DỰ ÁN ĐẦU TƯ
            </div>
            <div style="font-size: 22px; font-weight: bold; color: Red; text-align: center; height: 30px">
                PIBMIS
            </div>
            <div style="text-align: center">
                <table style="margin: 0px auto 0px auto">
                    <tr>
                        <td style="width: 500px">
                            <cc2:OboutTextBox ID="txtTimKiem" Width="100%" runat="server" FolderStyle="~/App_Themes/Styles/Interface/OboutTextBox"></cc2:OboutTextBox>
                        </td>
                        <td style="width: 20px">
                            <img alt="Cấu hình" onclick="cauHinhTimKiem()" src="../images/setting.png" id="settingSear"
                                style="width: 20px" title="Cấu hình tìm kiếm" />
                        </td>
                        <td style="width: 90px">
                            <cc2:OboutButton ID="OboutButton1" Width="100%" runat="server" Text="Tìm kiếm" OnClientClick="NapDuLieu(); return false;"
                                FolderStyle="~/App_Themes/Styles/Interface/OboutButton">
                            </cc2:OboutButton>
                        </td>
                    </tr>
                </table>
                <div id="divcaiDat" class="TTcaiDat" style="visibility: hidden; display: none">
                </div>
            </div>
            <div id="duLieuTimKiem" style="font-size: 13px; line-height: 12px; margin-left: 3px">
            </div>
            <div id="phanTrang" style="margin: 10px; font-size: 13px; text-align: center">
            </div>
        </fieldset>
        <div>
            <table>
                <tr>
                </tr>
            </table>
        </div>
        <div style="margin: -2px 1px 1px 1px">
            <table style="width: 100%;">
                <tr>
                    <td style="font-size: 13px; font-weight: bold; font-family: Verdana;">Thống kê theo nguồn vốn đầu tư 
                    </td>
                    <td align="right" style="width: 250px;">
                        <input style="width: 100%" type="text" placeholder="Nội dung tìm kiếm..." onkeyup="searchValue(Grid3,0,this.value)" class="searchCss" />
                    </td>
                </tr>
            </table>
            <cc1:Grid ID="Grid3" runat="server" AutoGenerateColumns="False" PageSize="10" AllowPaging="true"
                FilterType="ProgrammaticOnly" AllowAddingRecords="false" Height="435" Width="960"
                FolderStyle="~/App_Themes/Styles/style_7" AllowFiltering="true" EnableRecordHover="true"
                PageSizeOptions="5,10,15,20" AllowMultiRecordSelection="false" OnRebind="Grid3_OnRebind"
                ShowColumnsFooter="true" OnRowDataBound="Grid3_OnRowDataBound">
                <%-- <ScrollingSettings ScrollHeight="420" ScrollWidth="100%" EnableVirtualScrolling="false" />
                <PagingSettings Position="Bottom" />--%>
                <FilteringSettings MatchingType="AnyFilter" FilterPosition="Top" FilterLinksPosition="Bottom" />
                <Columns>
                    <cc1:Column DataField="nguonVon" HeaderText="Nguồn vốn đầu tư" Width="260" Wrap="true" ShowFilterCriterias="false">
                    </cc1:Column>
                    <cc1:Column DataField="slDuAn" HeaderText="Số lượng" Width="58" Align="center" DataFormatString="{0:N0}"
                        ShowFilterCriterias="false">
                        <FilterOptions>
                            <cc1:FilterOption Type="Contains" />
                        </FilterOptions>
                    </cc1:Column>
                    <cc1:Column DataField="tongMucDauTu" HeaderText="Tổng mức đầu tư" Width="140" Align="right"
                        DataFormatString="{0:N0}" ShowFilterCriterias="false">
                        <FilterOptions>
                            <cc1:FilterOption Type="Contains" />
                        </FilterOptions>
                    </cc1:Column>
                    <cc1:Column DataField="keHoachVon" HeaderText="Kế hoạch vốn năm" Width="130" Align="right"
                        DataFormatString="{0:N0}" ShowFilterCriterias="false">
                        <FilterOptions>
                            <cc1:FilterOption Type="Contains" />
                        </FilterOptions>
                    </cc1:Column>
                    <cc1:Column DataField="giaTriNghiemThu" HeaderText="Giá trị thực hiện" Width="130"
                        Align="right" DataFormatString="{0:N0}" ShowFilterCriterias="false">
                        <FilterOptions>
                            <cc1:FilterOption Type="Contains" />
                        </FilterOptions>
                    </cc1:Column>
                    <cc1:Column DataField="khoiLuongGNgan" HeaderText="Giá trị giải ngân" Width="130"
                        Align="right" DataFormatString="{0:N0}" ShowFilterCriterias="false">
                        <FilterOptions>
                            <cc1:FilterOption Type="Contains" />
                        </FilterOptions>
                    </cc1:Column>
                    <cc1:Column DataField="khoiLuongQToan" HeaderText="Giá trị quyết toán" Width="130"
                        Align="right" DataFormatString="{0:N0}" ShowFilterCriterias="false">
                        <FilterOptions>
                            <cc1:FilterOption Type="Contains" />
                        </FilterOptions>
                    </cc1:Column>
                    <cc1:Column DataField="" HeaderText="" Width="20">
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
                <%--<TemplateSettings HeadingTemplateId="GridTemplate3" />--%>
                <Templates>
                    <cc1:GridTemplate runat="server" ID="GridTemplate3">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 100%; text-align: left;">
                                    <b>Thống kê</b>
                                </td>

                                <td align="right">
                                    <input type="text" placeholder="Nội dung tìm kiếm..." onkeyup="searchValue(Grid1,3,this.value)"
                                        class="searchCss" />
                                </td>
                            </tr>
                        </table>
                    </cc1:GridTemplate>
                </Templates>
            </cc1:Grid>
        </div>
        <style>
            .msgCanhBao {
                bottom: 10px;
                display: none;
                outline: medium none;
                padding: 0;
                position: fixed;
                right: 5px;
                width: 170px;
                text-align: left;
                z-index: 999;
                color: Yellow;
            }
        </style>
        <div class="msgCanhBao">
        </div>

        <script type="text/javascript">
            window.onload = function () {
                Grid3.refresh();
                var result = ProOnline.trangchu.trangchu.checkCanhBao().value;
                if (result != "") {
                    $('.msgCanhBao').html(result);
                    $('.msgCanhBao').show();
                }
                else {
                    $('.msgCanhBao').html('');
                    $('.msgCanhBao').hide();
                }
            }
            function setPosition() {
                var screenWidth = screen.width;
                var screenHeight = screen.height;
                var WindowCauHinhSize = WindowCauHinh.getSize();
                WindowCauHinh.setPosition(parseFloat((parseFloat(screenWidth) - parseFloat(WindowCauHinhSize.width)) / 2), 160);
            }
            function dongChiTiet() {
                document.getElementById("divChiTiet").style.visibility = "hidden";
                document.getElementById("divChiTiet").style.display = "none";
            }
            function openWinDow(id) {
                $('[data-popup="popup-1"]').fadeIn(350);
                $(function () {
                    //----- OPEN
                    $('[data-popup-open]').on('click', function (e) {
                        var targeted_popup_class = jQuery(this).attr('data-popup-open');
                        $('[data-popup="' + targeted_popup_class + '"]').fadeIn(350);
                        //alert(targeted_popup_class);
                        // Grid1.refresh();
                        // Grid2.refresh();
                        // Grid3.refresh();
                        e.preventDefault();
                    });

                    //----- CLOSE
                    $('[data-popup-close]').on('click', function (e) {
                        var targeted_popup_class = jQuery(this).attr('data-popup-close');
                        $('[data-popup="' + targeted_popup_class + '"]').fadeOut(350);

                        e.preventDefault();
                    });
                });

                //document.getElementById("divChiTiet").style.visibility="visible";
                //document.getElementById("divChiTiet").style.display="block";
                var result = ProOnline.trangchu.trangchu.layTTDuAn(id).value;
                if (result.Rows.length > 0) {
                    //                    document.getElementById("maDuAn").innerHTML = "- Mã dự án: " + "<b>"+result.Rows[0].maDuAn+"</b>";
                    document.getElementById("tenDA").innerHTML = "- Tên dự án: " + "<b>" + result.Rows[0].maDuAn + " - " + result.Rows[0].tenDuAn + "</b>";
                    document.getElementById("diaDiemXD").innerHTML = "- Địa điểm xây dựng: " + "<b>" + result.Rows[0].diaDiemXD + "</b>";
                    document.getElementById("coQuanQD").innerHTML = "- Cơ quan phê duyệt dự án: " + "<b>" + result.Rows[0].coQuanQD + "</b>";
                    document.getElementById("tenChuDauTu").innerHTML = "- Chủ đầu tư: " + "<b>" + result.Rows[0].tenChuDauTu + "</b>";
                    document.getElementById("banQLDA").innerHTML = "- Cơ quan quản lý dự án: " + "<b>" + result.Rows[0].banQLDA + "</b>";
                    document.getElementById("tGianKCongHThanh").innerHTML = "- Ngày khởi công - Hoàn thành: " + "<b>" + result.Rows[0].thoiGianKCong + " - " + result.Rows[0].thoiGianHThanh + "</b>" + " Ngày khởi công - Hoàn thành thực tế: " + "<b>" + result.Rows[0].thoiGianKCongTH + " - " + result.Rows[0].thoiGianHThanhTH + "</b></br>";
                } else {
                    //                    document.getElementById("maDuAn").innerHTML = "- Mã dự án:... ";
                    document.getElementById("tenDA").innerHTML = "- Tên dự án:... ";
                    document.getElementById("diaDiemXD").innerHTML = "- Địa điểm xây dựng:... ";
                    document.getElementById("coQuanQD").innerHTML = "- Cơ quan phê duyệt dự án:... ";
                    document.getElementById("tenChuDauTu").innerHTML = "- Chủ đầu tư:... ";
                    document.getElementById("banQLDA").innerHTML = "- Cơ quan quản lý dự án:...";
                    document.getElementById("tGianKCongHThanh").innerHTML = "- Ngày khởi công:...Ngày hoàn thành:...- Ngày khởi công thực tế:... Hoàn thành thực tế:...";
                }
                document.getElementById("ctl00_ContentPlaceHolder1_hdfSTTDA").value = id + "";
                Grid1.refresh();
                Grid2.refresh();
                Grid3.refresh();
            }
            function cauHinhTimKiem() {
                chkTenDuAn.disable();
                setPosition();
                WindowCauHinh.Open();
                var result1 = ProOnline.trangchu.trangchu.dtCauHinh().value;
                if (result1.Rows.length > 0) {
                    if (result1.Rows[0].maDuAn == true) {
                        chkMaDuAn.checked(true);
                    } else {
                        chkMaDuAn.checked(false);
                    }
                    if (result1.Rows[0].tenDuAn == true) {
                        chkTenDuAn.checked(true);
                    } else {
                        chkTenDuAn.checked(false);
                    }
                    if (result1.Rows[0].nguonVon == true) {
                        chkNguonVon.checked(true);
                    } else {
                        chkNguonVon.checked(false);
                    }
                    if (result1.Rows[0].loaiKhoan == true) {
                        chkLoaiKhoan.checked(true);
                    } else {
                        chkLoaiKhoan.checked(false);
                    }
                    if (result1.Rows[0].loaiCongTrinh == true) {
                        chkLoaiCTrinh.checked(true);
                    } else {
                        chkLoaiCTrinh.checked(false);
                    }
                    txtNgayKhoiCongTu.value(result1.Rows[0].khoiCongTu);
                    txtNgayKhoiCongDen.value(result1.Rows[0].khoiCongDen);
                    txtNgayHoanThanhTu.value(result1.Rows[0].hoanThanhTu);
                    txtNgayHoanThanhDen.value(result1.Rows[0].hoanThanhDen);
                    cboLoaiDuAn.value(result1.Rows[0].maLoaiDApr_sd);
                    cboLoaiHinh.value(result1.Rows[0].maLoaiHinhpr_sd);
                    cboNguonVonTimKiem.value(result1.Rows[0].maNguonVonpr_sd);
                } else {
                    chkMaDuAn.checked(false);
                    chkTenDuAn.checked(false);
                    chkNguonVon.checked(false);
                    chkLoaiKhoan.checked(false);
                    chkLoaiCTrinh.checked(false);
                    txtNgayKhoiCongTu.value("");
                    txtNgayKhoiCongDen.value("");
                    txtNgayHoanThanhTu.value("");
                    txtNgayHoanThanhDen.value("");
                    cboLoaiDuAn.value("");
                    cboLoaiHinh.value("");
                    cboNguonVonTimKiem.value("");
                }
            }
            function btnLuuCauHinhClick() {
                var param = new Array();
                param[0] = chkMaDuAn.checked();
                param[1] = chkTenDuAn.checked();
                param[2] = chkNguonVon.checked();
                param[3] = chkLoaiKhoan.checked();
                param[4] = chkLoaiCTrinh.checked();
                param[5] = txtNgayKhoiCongTu.value();
                param[6] = txtNgayKhoiCongDen.value();
                param[7] = txtNgayHoanThanhTu.value();
                param[8] = txtNgayHoanThanhDen.value();
                param[9] = cboLoaiDuAn.value();
                param[10] = cboLoaiHinh.value();
                param[11] = cboNguonVonTimKiem.value();
                var ktLuu = ProOnline.trangchu.trangchu.luuCauHinh(param).value;
                if (ktLuu == true) {
                    alert("Lưu cấu hình thành công!");
                    WindowCauHinh.Close();
                } else {
                    alert("Lưu cấu hình không thành công!");
                }
                return false;
            }
            function truocKhiIn() {
                dalInToTrinh.Open();
                return false;
            }
        </script>

        <div class="popup" data-popup="popup-1">
            <div class="popup-inner">
                <div id="divChiTiet" style="">
                    <div style="text-align: center; font-weight: bold; font-size: 16px">
                        TÌNH HÌNH THỰC HIỆN VÀ QUYẾT TOÁN VỐN ĐẦU TƯ
                    </div>
                    <div>
                        <fieldset style="border: 1px solid #DBDBE1; margin-bottom: 3px">
                            <legend><b>Thông tin dự án</b></legend>
                            <table style="width: 94%">
                                <tr>
                                    <td align="left">
                                        <span id="maDuAn"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="tenDA"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <span id="diaDiemXD"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <span id="coQuanQD"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="tenChuDauTu"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="banQLDA"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="tGianKCongHThanh"></span><span style="float: right">
                                            <asp:LinkButton ID="linkXuatExcel" runat="server" OnClientClick="truocKhiIn(); return false;">Xuất Excel</asp:LinkButton>
                                        </span>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <fieldset style="border: 1px solid #DBDBE1; margin-bottom: 3px">
                            <legend><b>Nguồn vốn đầu tư</b></legend>
                            <table style="width: 905px">
                                <tr>
                                    <td style="width: 50%">
                                        <b></b>
                                    </td>
                                    <td style="width: 50%; text-align: right; padding-right: 10px; font-style: italic;">Đơn vị tính: đồng
                                    </td>
                                </tr>
                            </table>
                            <cc1:Grid ID="Grid1" runat="server" AutoGenerateColumns="False" PageSize="10" AllowPaging="false"
                                AllowAddingRecords="false" Height="110" Width="935" FolderStyle="~/App_Themes/Styles/style_7"
                                AllowFiltering="false" EnableRecordHover="true" PageSizeOptions="5,10,15,20"
                                OnRebind="Grid1_OnRebind" ShowColumnsFooter="true" OnRowDataBound="Grid1_OnRowDataBound"
                                ShowFooter="false">
                                <%--<ScrollingSettings NumberOfFixedColumns="0" ScrollHeight="400" ScrollWidth="100%"
                                EnableVirtualScrolling="false" />--%>
                                <PagingSettings Position="Bottom" />
                                <FilteringSettings InitialState="Hidden" FilterPosition="Top" FilterLinksPosition="Bottom" />
                                <Columns>
                                    <cc1:Column DataField="tenNguonVon" HeaderText="Nguồn vốn" Width="285" Wrap="true">
                                    </cc1:Column>
                                    <cc1:Column DataField="tongMucDauTu" HeaderText="Tổng mức đầu tư" Width="130" Align="right"
                                        DataFormatString="{0:N0}">
                                    </cc1:Column>
                                    <cc1:Column DataField="keHoachVon" HeaderText="Kế hoạch vốn năm" Width="130" Align="right"
                                        DataFormatString="{0:N0}">
                                    </cc1:Column>
                                    <cc1:Column DataField="giaTriNghiemThu" HeaderText="Giá trị thực hiện" Width="130"
                                        Align="right" DataFormatString="{0:N0}">
                                    </cc1:Column>
                                    <cc1:Column DataField="khoiLuongGNgan" HeaderText="Giá trị giải ngân" Width="130"
                                        Align="right" DataFormatString="{0:N0}">
                                    </cc1:Column>
                                    <cc1:Column DataField="khoiLuongQToan" HeaderText="Giá trị quyết toán" Width="130"
                                        Align="right" DataFormatString="{0:N0}">
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
                                <%--<TemplateSettings HeadingTemplateId="GridTemplate1" />--%>
                                <Templates>
                                    <cc1:GridTemplate runat="server" ID="GridTemplate1">
                                    </cc1:GridTemplate>
                                </Templates>
                            </cc1:Grid>
                        </fieldset>

                    </div>
                    <div style="height: 4px">
                    </div>
                    <div>
                        <fieldset style="border: 1px solid #DBDBE1; margin-bottom: 3px">
                            <legend><b>Tình hình thực hiện</b></legend>
                            <table style="width: 905px">
                                <tr>
                                    <td style="width: 50%"></td>
                                    <td style="width: 50%; text-align: right; padding-right: 10px; font-style: italic;">Đơn vị tính: đồng
                                    </td>
                                </tr>
                            </table>
                            <cc1:Grid ID="Grid2" runat="server" AutoGenerateColumns="False" PageSize="10" AllowPaging="false"
                                AllowAddingRecords="false" Height="220" Width="935" FolderStyle="~/App_Themes/Styles/style_7"
                                AllowFiltering="false" EnableRecordHover="true" PageSizeOptions="10" OnRebind="Grid2_OnRebind"
                                ShowColumnsFooter="true" ShowFooter="false" OnRowDataBound="Grid2_OnRowDataBound">
                                <%--<ScrollingSettings NumberOfFixedColumns="0" ScrollHeight="400" ScrollWidth="960"
                                EnableVirtualScrolling="false" />--%>
                                <PagingSettings Position="Bottom" />
                                <FilteringSettings InitialState="Hidden" FilterPosition="Top" FilterLinksPosition="Bottom" />
                                <Columns>
                                    <cc1:Column DataField="noiDung" HeaderText="Nội dung" Width="285">
                                    </cc1:Column>
                                    <cc1:Column DataField="duToanDuocDuyet" HeaderText="Dự toán được duyệt" Width="215"
                                        Align="right" DataFormatString="{0:N0}">
                                    </cc1:Column>
                                    <cc1:Column DataField="giaTriQuyetToan" HeaderText="Đề nghị quyết toán" Width="215"
                                        Align="right" DataFormatString="{0:N0}">
                                    </cc1:Column>
                                    <cc1:Column DataField="tangGiam" HeaderText="Tăng, giảm so với dự toán" Width="220"
                                        Align="right" DataFormatString="{0:N0}">
                                        <TemplateSettings TemplateId="numberTemplate" />
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
                                <%--<TemplateSettings HeadingTemplateId="GridTemplate2" />--%>
                                <Templates>
                                    <cc1:GridTemplate runat="server" ID="GridTemplate2">
                                        <Template>
                                        </Template>
                                    </cc1:GridTemplate>
                                    <cc1:GridTemplate runat="server" ID="numberTemplate">
                                        <Template>
                                            <%# Container.Value != String.Empty && Container.Value != "&#160;" ?  (number == "{0:n:1}")? 
							            String.Format( number, Container.Value + "") : Convert.ToDecimal( Container.Value).ToString( number )   : ""%>
                                        </Template>
                                    </cc1:GridTemplate>
                                </Templates>
                            </cc1:Grid>
                        </fieldset>
                    </div>
                </div>
                <br />
                <%--<span>
                <a data-popup-close="popup-1" href="#">Đóng</a> </span>--%>
                <a class="popup-close" data-popup-close="popup-1" href="#">x</a>
            </div>
        </div>
    </div>
    <div>
        <owd:Window ID="WindowCauHinh" runat="server" IsModal="true" ShowCloseButton="true"
            Status="" ShowStatusBar="false" RelativeElementID="WindowPositionHelper" Height="240"
            Width="500" VisibleOnLoad="false" StyleFolder="~/App_Themes/Styles/wdstyles/dogma"
            Title="Cấu hình tìm kiếm" IsResizable="false">
            <table>
                <tr>
                    <td>
                        <cc2:OboutButton ID="btnLuuCauHinh" Width="90px" runat="server" Text="Đồng ý" OnClientClick="btnLuuCauHinhClick();return false;"
                            FolderStyle="~/App_Themes/Styles/Interface/OboutButton">
                        </cc2:OboutButton>
                    </td>
                    <td>
                        <cc2:OboutButton ID="OboutButton2" Width="90px" runat="server" Text="Bỏ qua" OnClientClick="WindowCauHinh.Close(); return false;"
                            FolderStyle="~/App_Themes/Styles/Interface/OboutButton">
                        </cc2:OboutButton>
                    </td>
                </tr>
            </table>
            <table style="width: 100%">
                <tr>
                    <td>
                        <cc2:OboutCheckBox ID="chkMaDuAn" runat="server" Text="Mã dự án" FolderStyle="~/App_Themes/Styles/Interface/OboutCheckBox">
                        </cc2:OboutCheckBox>
                    </td>
                    <td>
                        <cc2:OboutCheckBox ID="chkTenDuAn" runat="server" Text="Tên dự án" FolderStyle="~/App_Themes/Styles/Interface/OboutCheckBox">
                        </cc2:OboutCheckBox>
                    </td>
                    <td style="visibility: hidden">
                        <cc2:OboutCheckBox ID="chkNguonVon" runat="server" Text="Nguồn vốn" FolderStyle="~/App_Themes/Styles/Interface/OboutCheckBox">
                        </cc2:OboutCheckBox>
                    </td>
                </tr>
                <tr style="visibility: hidden; display: none">
                    <td>
                        <cc2:OboutCheckBox ID="chkLoaiKhoan" runat="server" Text="Loại, Khoản" FolderStyle="~/App_Themes/Styles/Interface/OboutCheckBox">
                        </cc2:OboutCheckBox>
                    </td>
                    <td>
                        <cc2:OboutCheckBox ID="chkLoaiCTrinh" runat="server" Text="Loại công trình" FolderStyle="~/App_Themes/Styles/Interface/OboutCheckBox">
                        </cc2:OboutCheckBox>
                    </td>
                </tr>
            </table>
            <table style="width: 100%">
                <tr>
                    <td style="width: 90px">Ngày khởi công từ
                    </td>
                    <td style="width: 120px">
                        <cc2:OboutTextBox ID="txtNgayKhoiCongTu" Width="100%" runat="server" FolderStyle="~/App_Themes/Styles/Interface/OboutTextBox"></cc2:OboutTextBox>
                    </td>
                    <td align="right">
                        <obout:Calendar ID="Calendar1" runat="server" DateFormat="dd/MM/yyyy" DatePickerMode="true"
                            TextBoxId="txtNgayKhoiCongTu" DatePickerImagePath="~/App_Themes/Styles/Date/date_picker1.gif"
                            DatePickerImageTooltip="Chọn ngày" TimeSelectorType="HtmlList" YearSelectorType="HtmlList"
                            MonthSelectorType="HtmlList" CultureName="vi-VN" Columns="1">
                        </obout:Calendar>
                    </td>
                    <td style="width: 60px; text-align: right">đến
                    </td>
                    <td style="width: 120px">
                        <cc2:OboutTextBox ID="txtNgayKhoiCongDen" Width="100%" runat="server" FolderStyle="~/App_Themes/Styles/Interface/OboutTextBox"></cc2:OboutTextBox>
                    </td>
                    <td align="right">
                        <obout:Calendar ID="Calendar2" runat="server" DateFormat="dd/MM/yyyy" DatePickerMode="true"
                            TextBoxId="txtNgayKhoiCongDen" DatePickerImagePath="~/App_Themes/Styles/Date/date_picker1.gif"
                            DatePickerImageTooltip="Chọn ngày" TimeSelectorType="HtmlList" YearSelectorType="HtmlList"
                            MonthSelectorType="HtmlList" CultureName="vi-VN" Columns="1">
                        </obout:Calendar>
                    </td>
                </tr>
                <tr>
                    <td>Ngày hoàn thành từ
                    </td>
                    <td>
                        <cc2:OboutTextBox ID="txtNgayHoanThanhTu" Width="100%" runat="server" FolderStyle="~/App_Themes/Styles/Interface/OboutTextBox"></cc2:OboutTextBox>
                    </td>
                    <td align="right">
                        <obout:Calendar ID="Calendar3" runat="server" DateFormat="dd/MM/yyyy" DatePickerMode="true"
                            TextBoxId="txtNgayHoanThanhTu" DatePickerImagePath="~/App_Themes/Styles/Date/date_picker1.gif"
                            DatePickerImageTooltip="Chọn ngày" TimeSelectorType="HtmlList" YearSelectorType="HtmlList"
                            MonthSelectorType="HtmlList" CultureName="vi-VN" Columns="1">
                        </obout:Calendar>
                    </td>
                    <td style="text-align: right">đến
                    </td>
                    <td>
                        <cc2:OboutTextBox ID="txtNgayHoanThanhDen" Width="100%" runat="server" FolderStyle="~/App_Themes/Styles/Interface/OboutTextBox"></cc2:OboutTextBox>
                    </td>
                    <td align="right">
                        <obout:Calendar ID="Calendar4" runat="server" DateFormat="dd/MM/yyyy" DatePickerMode="true"
                            TextBoxId="txtNgayHoanThanhDen" DatePickerImagePath="~/App_Themes/Styles/Date/date_picker1.gif"
                            DatePickerImageTooltip="Chọn ngày" TimeSelectorType="HtmlList" YearSelectorType="HtmlList"
                            MonthSelectorType="HtmlList" CultureName="vi-VN" Columns="1">
                        </obout:Calendar>
                    </td>
                </tr>
                <tr>
                    <td style="visibility: hidden; display: none">Loại hình
                    </td>
                    <td colspan="2" style="visibility: hidden; display: none">
                        <cc3:ComboBox ID="cboLoaiHinh" runat="server" MenuWidth="150%" Height="150" Width="100%"
                            FolderStyle="~/App_Themes/Styles/Interface/OboutComboBox">
                        </cc3:ComboBox>
                    </td>
                    <td>Loại dự án
                    </td>
                    <td colspan="5">
                        <cc3:ComboBox ID="cboLoaiDuAn" runat="server" Height="150" Width="100%" FolderStyle="~/App_Themes/Styles/Interface/OboutComboBox">
                        </cc3:ComboBox>
                    </td>
                </tr>
                <tr>
                    <td>Nguồn vốn
                    </td>
                    <td colspan="5">
                        <cc3:ComboBox ID="cboNguonVonTimKiem" runat="server" MenuWidth="100%" Height="150"
                            Width="100%" FolderStyle="~/App_Themes/Styles/Interface/OboutComboBox">
                        </cc3:ComboBox>
                    </td>
                </tr>
            </table>
        </owd:Window>
    </div>
    <%--<a class="btn" data-popup-open="popup-1" href="#">Open Popup #1</a>
    <div class="popup" data-popup="popup-1">
        <div class="popup-inner">
            <h2>Wow! This is Awesome! (Popup #1)</h2>
            <p>
                Donec in volutpat nisi. In quam lectus, aliquet rhoncus cursus a, congue et arcu.
                Vestibulum tincidunt neque id nisi pulvinar aliquam. Nulla luctus luctus ipsum at
                ultricies. Nullam nec velit dui. Nullam sem eros, pulvinar sed pellentesque ac,
                feugiat et turpis. Donec gravida ipsum cursus massa malesuada tincidunt. Nullam
                finibus nunc mauris, quis semper neque ultrices in. Ut ac risus eget eros imperdiet
                posuere nec eu lectus.</p>
            <p><a data-popup-close="popup-1" href="#">Close</a></p>
            <a class="popup-close" data-popup-close="popup-1" href="#">x</a>
        </div>
    </div>--%>
    <%--   <a href="#" class="btn" data-popup-open="popup-1">Xây dựng trụ sở làm việc UBND tỉnh Vĩnh Long</a></br></br>
    --%>
    <owd:Dialog ID="dalInToTrinh" runat="server" IsModal="true" ShowCloseButton="true"
        Top="0" Left="250" Height="140" Width="350" VisibleOnLoad="false" StyleFolder="~/App_Themes/Styles/wdstyles/dogma"
        Title="Tùy chọn thông tin xuất chi tiết chi phí" zIndex="1001">
        <fieldset style="border: 1px solid #DBDBE1; margin-top: 10px; padding: 10px;">
            <legend><b>Tùy chọn thông tin xuất chi tiết chi phí</b></legend>
            <div align="center">
                <table style="width: 100%">
                    <tr style="width: 100%">
                        <td>
                            <cc2:OboutCheckBox ID="chkHienChiPhiCon" runat="server" Text="Hiện chi tiết chi phí" FolderStyle="App_Themes/Styles/Interface/OboutCheckBox">
                            </cc2:OboutCheckBox>
                        </td>
                    </tr>
                    <tr style="width: 100%">

                        <td>
                            <cc2:OboutButton ID="btnInToTrinh" runat="server" Text="Xuất Excel" Width="120px" OnClick="linkXuatExcel_Click">
                            </cc2:OboutButton>
                        </td>
                    </tr>
                </table>
            </div>
        </fieldset>
    </owd:Dialog>
    <div class="popup" data-popup="popup-2">
        <div class="popup-inner">
            <div id="div1" style="">
                <div style="text-align: center; font-weight: bold; font-size: 16px; color: red; -webkit-animation: my 700ms infinite; -moz-animation: my 700ms infinite; -o-animation: my 700ms infinite; animation: my 700ms infinite;">
                    THÔNG BÁO NÂNG CẤP PHẦN MỀM
                </div>
                <div style="text-align: right;">
                    <cc2:OboutCheckBox ID="rdoKhongHienThiLS" runat="server" Text="Không hiện thông báo nữa" FolderStyle="App_Themes/Styles/Interface/OboutCheckBox">
                    </cc2:OboutCheckBox>
                    <cc2:OboutButton ID="btnTiepTuc" runat="server" Text="Tiếp tục sử dụng phần mềm" Width="175px" OnClientClick="btnTiepTuc_OnClientClick();return false;">
                    </cc2:OboutButton>
                </div>
                <div>
                    <fieldset style="border: 1px solid #DBDBE1; margin-bottom: 3px">
                        <legend><b>Nhật ký nâng cấp phần mềm</b></legend>
                        <embed src="../Trogiup/HDSD.pdf" type="application/pdf" width="100%" height="530px" />
                    </fieldset>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
