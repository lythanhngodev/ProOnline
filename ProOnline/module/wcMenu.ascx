<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wcMenu.ascx.cs" Inherits="ProOnline.module.wcMenu" %>
<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc2" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>
<link href="/css/menu.css" rel="stylesheet" type="text/css" />

<script type="text/javascript">
    function cboKyBC1_OnChange() {
        _txtTuNgay1.disable();
        _txtDenNgay1.disable();
        ctl00_wcMenu2_Calendar111.enabled = false;
        ctl00_wcMenu2_Calendar211.enabled = false;
        var now = new Date();
        var dayArray = new Array(9);
        var ngayHienTai = '';
        var thangHienTai = '';

        dayArray[0] = "01";
        dayArray[1] = "02";
        dayArray[2] = "03";
        dayArray[3] = "04";
        dayArray[4] = "05";
        dayArray[5] = "06";
        dayArray[6] = "07";
        dayArray[7] = "08";
        dayArray[8] = "09";
        //chuyển thành dạng chuổi 2 ký tự
        if (now.getDate() < 10)
            ngayHienTai = dayArray[now.getDate() - 1];
        else
            ngayHienTai = now.getDate();
        //chuyển thành dạng chuổi 2 ký tự    
        if (cboKyBC1.value() < 10)
            thangHienTai = dayArray[cboKyBC1.value() - 1];
        else
            thangHienTai = cboKyBC1.value();
        //Gán giá trị cho textbox
        if (cboKyBC1.value() < 13) {
            _txtTuNgay1.value('01/' + thangHienTai + '/' + now.getFullYear());
            _txtDenNgay1.value(getDaysOfMonth(new Date().getFullYear(), cboKyBC1.value()) + '/' + thangHienTai + '/' + now.getFullYear());
        }
        else if (cboKyBC1.value() == 13) {//Cả năm

            _txtTuNgay1.value('01/01/' + now.getFullYear());
            _txtDenNgay1.value('31/12/' + now.getFullYear());
        }
        else if (cboKyBC1.value() == 14) {//Cả năm 13 tháng

            _txtTuNgay1.value('01/01/' + now.getFullYear());
            _txtDenNgay1.value('31/01/' + parseInt(now.getFullYear() + 1));
        }
        else if (cboKyBC1.value() == 15) {//Quý I
            _txtTuNgay1.value('01/01/' + now.getFullYear());
            _txtDenNgay1.value('31/03/' + now.getFullYear());
        }
        else if (cboKyBC1.value() == 16) {//Quý II
            _txtTuNgay1.value('01/04/' + now.getFullYear());
            _txtDenNgay1.value('30/06/' + now.getFullYear());
        }
        else if (cboKyBC1.value() == 17) {//Quý III
            _txtTuNgay1.value('01/07/' + now.getFullYear());
            _txtDenNgay1.value('30/09/' + now.getFullYear());
        }
        else if (cboKyBC1.value() == 18) {//Quý IV
            _txtTuNgay1.value('01/10/' + now.getFullYear());
            _txtDenNgay1.value('31/12/' + now.getFullYear());
        }
        else if (cboKyBC1.value() == 21) {//Tùy chọn

            _txtTuNgay1.enable();
            _txtDenNgay1.enable();
            ctl00_wcMenu2_Calendar111.enabled = true;
            ctl00_wcMenu2_Calendar211.enabled = true;
        }

    }
    //lấy ngày hiện tại
    function getDaysOfMonth(year, month) {
        return new Date(year, month, 0).getDate();
    }

    function layNgayLocDieu() {
        document.getElementById("ctl00_wcMenu_hdfTuNgayLoc").value = _txtTuNgay1.value();
        document.getElementById("ctl00_wcMenu_hdfDenNgayLoc").value = _txtDenNgay1.value();
        return true;
    }
    jQuery(document).ready(function() {
        var offset = 150;
        var duration = 500;

        $('#menu span li').hover(function(e) {
            var toaDo = e.pageX;
            var manHinh = $(window).width();
            console.log(toaDo + '-' + ($(window).width() - e.pageX) / 2);
            var kcLeft = 0;
            var kcBien = (screen.width - 1000) / 2;
            $(this).find('.dropdown_1column').show();
            $(this).find('.dropdown_2columns').show();
            $(this).find('.dropdown_3columns').show();
            $(this).find('.dropdown_4columns').show();
            $(this).find('.dropdown_5columns').show();
            //            $(this).find('.dropdown_1column').css({ left: (e.pageX - ($('.dropdown_1column').width() / 2)) });
            $(this).find('.dropdown_2columns').css({ left: (e.pageX - ($('.dropdown_2columns').width() / 2)) });
            $(this).find('.dropdown_3columns').css({ left: (e.pageX - ($('.dropdown_3columns').width() / 2)) });
            if (toaDo > manHinh / 2) {
                $(this).find('.dropdown_4columns').css({ right: kcBien });
                $(this).find('.dropdown_5columns').css({ right: kcBien });
            }
            else {
                $(this).find('.dropdown_4columns').css({ left: kcBien });
                $(this).find('.dropdown_5columns').css({ left: kcBien });
            }
        }, function() {
            $(this).find('.dropdown_1column').hide();
            $(this).find('.dropdown_2columns').hide();
            $(this).find('.dropdown_3columns').hide();
            $(this).find('.dropdown_4columns').hide();
            $(this).find('.dropdown_5columns').hide();
        });
    })
    var viTri = 1;
    var thoiGian = 1000;
    function goiSession() {
        interval = setInterval(function() {
            if (viTri == 1000) {
                console.log(ProOnline.module.wcMenu.getSTTDuAn().value);
                clearInterval(interval);
                goiSession();
                viTri = 0;
            }
            else {
                viTri += 1;
                clearInterval(interval);
                goiSession();
            }
        }, 0);

    }
    $(document).ready(function() {
        goiSession();
        $("#object").hide(); // Giấu đối tượng
        $('#thongTin').click(function() {//Khi nút được nhấn
            $("#object").css("visibility", "visible");
            //            $("#object").show();
            $("#object").toggle();
            $("#divchonntt").hide();
        });

        //$("#divchonntt").hide();// Giấu đối tượng
        $('.namTT').click(function() {//Khi nút được nhấn
            $("#divchonntt").css("visibility", "visible");
            $("#divchonntt").toggle();
            $("#object").hide();
        });

        //        $('*:not(.namTT)').click(function(){
        //            $("#divchonntt").hide();
        //        });

        $('#idchinh').click(function() {//Khi nút được nhấn
            $("#divchonntt").hide();
            $("#object").hide();
        });

        //hiện fix menu
        var nav = $('#menu');
        $(window).bind('scroll', function() {
            if ($(window).scrollTop() > 50) {
                nav.addClass('fix-menu');
            } else {
                nav.removeClass('fix-menu');
            }
        });
    });
    function abc(s) {
        ProOnline.module.wcMenu.setSession(s.toString());
        return false;
    }
    function hienThiNamLamViec_onclick() {
        cboDuAnMNu.value(ProOnline.module.wcMenu.getSTTDuAn().value);
        return true;
    }
</script>

<asp:HiddenField ID="hdfTuNgayLoc" runat="server" />
<asp:HiddenField ID="hdfDenNgayLoc" runat="server" />
<%--tạo menu--%>
<ul id="menu" style="float: left; margin-right: 0px; width: 1000px;">
    <%--<p style="margin:0px;padding:0px" id="loadMenu"></p>--%>
    <span id="loadMenu" runat="server"></span>
    <div style="float: right; margin-top: -3px">
        <table border="0">
            <tr>
                <td>
                    <a href="javascript:void(0)">
                        <img id="thongTin" src="/images/icon4.png" height="30px" title="Thông tin tài khoản"
                            width="30px" /></a>
                </td>
                <td>
                    <span style="color: White; font-weight: bold; font-size: 16px;">|</span>
                </td>
                <td>
                    <a href="javascript:void(0)"><span id="hienThiNamLamViec" class="namTT" runat="server"
                        title="Thông tin niên độ" style="color: #FFFFFF; font-family: Arial, Helvetica, sans-serif;
                        font-weight: bold; font-size: 16px;" onclick="hienThiNamLamViec_onclick()"></span></a>
                </td>
            </tr>
        </table>
    </div>
</ul>
<div id="object" class="detailDiv" style="visibility: hidden; display: none">
    <table width="100%">
        <tr>
            <td colspan="2" style="text-align: center; font-weight: bold">
                <label id="idUser" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td>
                Mã đơn vị:
            </td>
            <td>
                <label id="idMaDV" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td>
                Tên đơn vị:
            </td>
            <td>
                <label id="idTenDV" runat="server">
                </label>
            </td>
        </tr>
    </table>
    <hr />
    <div style="float: left">
        <cc1:OboutButton ID="OboutButton1" runat="server" Text="Thay đổi mật khẩu" FolderStyle="~/App_Themes/Styles/Interface/OboutButton"
            OnClick="thayDoiMatKhau">
        </cc1:OboutButton>
    </div>
    <div style="float: right">
        <cc1:OboutButton ID="OboutButton2" runat="server" Text="Đăng xuất" OnClick="dangXuat">
        </cc1:OboutButton>
    </div>
</div>
<div id="divchonntt" style="visibility: hidden; display: none; font-size: 12px;">
    <table>
        <tr>
            <td>
                Dự án mặc định
            </td>
            <td colspan="2">
                <cc2:ComboBox ID="cboDuAnMNu" runat="server" Width="100%" Height="150px" MenuWidth="250px" FolderStyle="~/App_Themes/Styles/Interface/OboutComboBox">
                </cc2:ComboBox>
            </td>
        </tr>
        <tr>
            <td>
                Kỳ báo cáo
            </td>
            <td colspan="2">
                <cc2:ComboBox ID="cboKyBC1" runat="server" Width="100%" Height="150px" FolderStyle="~/App_Themes/Styles/Interface/OboutComboBox">
                    <ClientSideEvents OnSelectedIndexChanged="cboKyBC1_OnChange" />
                </cc2:ComboBox>
            </td>
        </tr>
        <tr>
            <td>
                Từ ngày
            </td>
            <td>
                <cc1:OboutTextBox ID="_txtTuNgay1" runat="server" MaxLength="10" Width="120px" FolderStyle="~/App_Themes/Styles/Interface/OboutTextBox"></cc1:OboutTextBox>
            </td>
            <td style="width: 20px">
                <obout:Calendar ID="Calendar111" runat="server" Columns="1" DateFormat="dd/MM/yyyy"
                    DatePickerImagePath="~/App_Themes/Styles/Date/date_picker1.gif" DatePickerMode="true"
                    TextBoxId="_txtTuNgay1" CultureName="vi-VN" Enabled="true">
                </obout:Calendar>
            </td>
        </tr>
        <tr>
            <td>
                Đến ngày
            </td>
            <td>
                <cc1:OboutTextBox ID="_txtDenNgay1" runat="server" Width="120px" MaxLength="10" FolderStyle="~/App_Themes/Styles/Interface/OboutTextBox"></cc1:OboutTextBox>
            </td>
            <td style="width: 20px">
                <obout:Calendar ID="Calendar211" runat="server" Columns="1" DateFormat="dd/MM/yyyy"
                    DatePickerImagePath="~/App_Themes/Styles/Date/date_picker1.gif" DatePickerMode="true"
                    TextBoxId="_txtDenNgay1" CultureName="vi-VN" Enabled="true">
                </obout:Calendar>
            </td>
        </tr>
        <tr>
            <td colspan="3" style="text-align: right">
                <cc1:OboutButton ID="OboutButton3" runat="server" Width="100px" Text="Cập nhật" OnClientClick="return layNgayLocDieu();"
                    OnClick="capnhatngay">
                </cc1:OboutButton>
            </td>
        </tr>
    </table>
</div>
<style>
    .detailDiv
    {
        min-height: 120px;
        width: 300px;
        background: #FFFFFF;
        position: absolute;
        box-shadow: 0px 0px 10px #CCC;
        -moz-box-shadow: 0px 0px 10px #CCC;
        -webkit-box-shadow: 0px 0px 10px #CCC;
        padding: 10px 20px 5px 20px;
        right: 30px;
        top: 50px;
        z-index: 9999999;
        font-size: 12px;
        line-height: 20px;
        border: 1px solid #CCCCCC;
    }
    #divchonntt
    {
        width: 250px;
        height: 165px;
        background: #FFFFFF;
        position: absolute;
        box-shadow: 0px 0px 10px #CCC;
        -moz-box-shadow: 0px 0px 10px #CCC;
        -webkit-box-shadow: 0px 0px 10px #CCC;
        padding: 10px;
        right: 0px;
        top: 45px;
        z-index: 999;
        font-size: 12px;
    }
</style>
