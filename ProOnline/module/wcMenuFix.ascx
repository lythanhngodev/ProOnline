<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wcMenuFix.ascx.cs" Inherits="ProOnline.module.wcMenuFix" %>
<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc2" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>
<link href="/css/flatmenu.css" rel='stylesheet' type='text/css' />

<script type="text/javascript" src="/js/jquery.min.js"></script>

<script type="text/javascript" src="/js/flatmenu-responsive.js"></script>
<style>
.detailDiv {
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
</style>
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
        document.getElementById("ctl00_wcMenu2_hdfTuNgayLoc").value = _txtTuNgay1.value();
        document.getElementById("ctl00_wcMenu2_hdfDenNgayLoc").value = _txtDenNgay1.value();
        return true;
    }

    $(document).ready(function() {
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
        ProOnline.module.wcMenuFix.setSession(s.toString());
        return false;
    } 
     
</script>

<asp:HiddenField ID="hdfTuNgayLoc" runat="server" />
<asp:HiddenField ID="hdfDenNgayLoc" runat="server" />
<%--Menu--%>
<div id="menu" class="menu1">
    <div id="loadMenu" runat="server">
    </div>
    <%--<li class="sub"><a href="#">Home</a>
                <ul>
                    <li><a href="#">Welcome</a></li>
                    <li><a href="#">Take Virtual Tour</a></li>
                </ul>
            </li>
            <li class="divider"></li>
            <li class="sub"><a href="#">The Company</a>
                <ul>
                    <li class="sub"><a href="#">Where We Operate</a>
                        <ul>
                            <li><a href="#">Canada</a></li>
                            <li><a href="#">Australia</a></li>
                            <li><a href="#">Germany</a></li>
                            <li><a href="#">Russia</a></li>
                            <li><a href="#">South Africa</a></li>
                        </ul>
                    </li>
                    <li class="sub"><a href="#">Our Partners</a>
                        <ul>
                            <li><a href="#">Microsoft</a></li>
                            <li><a href="#">Apple Inc</a></li>
                            <li><a href="#">Mastercard </a></li>
                            <li><a href="#">Some Partners usually have extremely long annoying names</a></li>
                        </ul>
                    </li>
                    <li><a href="#">Our Vision</a></li>
                    <li><a href="#">Background</a></li>
                    <li><a href="#">History</a></li>
                    <li><a href="#">Much More</a></li>
                </ul>
            </li>
            <li class="divider"></li>
            <li class="sub"><a href="#">Our Services</a><ul>
                <li><a href="#">Website Design </a></li>
                <li><a href="#">Application Development</a></li><li><a href="#">Search Engine Optimization</a></li><li>
                    <a href="#">3D Motion Graphics</a></li><li><a href="#">Corporate Branding</a></li></ul>
            </li>
            <li class="divider"></li>
            <li><a href="#">Latest</a></li><li class="divider"></li>
            <li><a href="#">Portfolio</a></li><li class="divider"></li>
            <li class="sub rtl"><a href="#">Multi Level</a>
                <ul>
                    <li><a href="#">Use Email</a></li>
                    <li><a href="#">Newsletter</a></li>
                    <li class="sub rtl"><a href="#">Use the following steps </a>
                        <ul>
                            <li><a href="#">Step 1 - Conceptualize</a></li>
                            <li><a href="#">Step 2 - Get A Paper</a></li>
                            <li><a href="#">Step 3 - Get A Pen</a></li>
                            <li><a href="#">Step 4 - Start Writing</a></li>
                            <li class="sub"><a href="#">Step 5 - Visualize</a>
                                <ul>
                                    <li><a href="#">Visualize Words</a></li>
                                    <li><a href="#">Visualize Letters</a></li>
                                    <li class="sub ltr"><a href="#">Visualize Characters</a>
                                        <ul>
                                            <li><a href="#">Continue Trail 1</a></li>
                                            <li><a href="#">Continue Trail 2</a></li>
                                            <li class="sub"><a href="#">Continue Trail 3</a>
                                                <ul>
                                                    <li><a href="#">Finally We Land</a></li>
                                                    <li><a href="#">Well Be Back </a></li>
                                                    <li><a href="#">How was the Journey?</a></li>
                                                </ul>
                                            </li>
                                            <li><a href="#">Continue Trail 4</a></li>
                                            <li><a href="#">Continue Trail 5</a></li>
                                        </ul>
                                    </li>
                                    <li><a href="#">Finish Visualization</a></li>
                                    <li><a href="#">Set The Tone </a></li>
                                </ul>
                            </li>
                        </ul>
                    </li>
                    <li><a href="#">Walk to our offices</a></li>
                    <li><a href="#">Fill this form</a></li>
                </ul>
            </li>
            <li class="divider"></li>--%>
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
    <div id="divchonntt" class="namtt" style="visibility: hidden; display: none; font-size: 12px;">
        <table>
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
                        DatePickerImagePath="~/App_Themes/Styles/Date/date_picker1.gif"
                        DatePickerMode="true" TextBoxId="_txtTuNgay1" CultureName="vi-VN" Enabled="true">
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
                        DatePickerImagePath="~/App_Themes/Styles/Date/date_picker1.gif"
                        DatePickerMode="true" TextBoxId="_txtDenNgay1" CultureName="vi-VN" Enabled="true">
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
</div>
