<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="canhan.ascx.cs" Inherits="ProOnline.module.canhan" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>
<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc3" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="cc2" %>
<%@ Register Assembly="obout_Window_NET" Namespace="OboutInc.Window" TagPrefix="owd" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="cc1" %>
<script>
    function resetThongTinCaNhan() { 
        winDowThemCaNhan_maCaNhanpr.value(ProOnline.module.canhan.layMaCaNhan().value); 
        winDowThemCaNhan_tenCaNhan.value(''); 
        winDowThemCaNhan_maToChucpr_sd.value('');
        winDowThemCaNhan_gioiTinhCaNhan.value('');
        winDowThemCaNhan_ngaySinhCaNhan.value('');
        winDowThemCaNhan_diaChiCaNhan.value('');
        winDowThemCaNhan_CMNDCaNhan.value('');
        winDowThemCaNhan_ngayCapCaNhan.value('');
        winDowThemCaNhan_noiCapCaNhan.value('');
        winDowThemCaNhan_chucVuCaNhan.value('');
        winDowThemCaNhan_dienThoaiCaNhan.value('');
    }
    function luuThongTinCaNhan() {
        var param = new Array();
        param[0] = winDowThemCaNhan_maCaNhanpr.value();
        param[1] = winDowThemCaNhan_tenCaNhan.value();
        param[2] = "0";
        param[3] = winDowThemCaNhan_maToChucpr_sd.value();
        param[4] = winDowThemCaNhan_gioiTinhCaNhan.value();
        param[5] = winDowThemCaNhan_ngaySinhCaNhan.value();
        param[6] = winDowThemCaNhan_diaChiCaNhan.value();
        param[7] = winDowThemCaNhan_CMNDCaNhan.value();
        param[8] = winDowThemCaNhan_ngayCapCaNhan.value();
        param[9] = winDowThemCaNhan_noiCapCaNhan.value();
        param[10] = winDowThemCaNhan_chucVuCaNhan.value();
        param[11] = winDowThemCaNhan_dienThoaiCaNhan.value();
        if (checkEmtyValue(param[0])) {
            return "Mã cá nhân không được bỏ trống";
        }
        if (checkEmtyValue(param[1])) {
            return "Tên cá nhân không được bỏ trống";
        }
        if (checkEmtyValue(param[1])) {
            return "Tên cá nhân không được bỏ trống";
        }
        if (checkEmtyValue(param[3])) {
            return "Bạn chưa chọn tổ chức";
        }
        return ProOnline.module.canhan.luuThongTinCaNhan(param).value;
    }
</script>
<table style="width: 565px">
    <tr>
        <td style="width: 100px">Mã
        </td>
        <td>
            <cc2:OboutTextBox ID="winDowThemCaNhan_maCaNhanpr" Enabled="false" runat="server" Width="100%" FolderStyle="~/App_Themes/Styles/Interface/OboutTextBox">
            </cc2:OboutTextBox>
        </td>
        <td>
            <span style="color: red">*</span>
        </td>
        <td></td>
    </tr>
    <tr>
        <td>Tên cá nhân
        </td>
        <td colspan="3">
            <cc2:OboutTextBox ID="winDowThemCaNhan_tenCaNhan" runat="server" Width="100%" FolderStyle="~/App_Themes/Styles/Interface/OboutTextBox">
            </cc2:OboutTextBox>
        </td>
        <td style="width: 10px">
            <span style="color: red">*</span>
        </td>
    </tr>
    <tr>
        <td>Chức vụ
        </td>
        <td style="width: 220px">
            <cc2:OboutTextBox ID="winDowThemCaNhan_chucVuCaNhan" runat="server" Width="100%" FolderStyle="~/App_Themes/Styles/Interface/OboutTextBox">
            </cc2:OboutTextBox>
        </td>
        <td style="width: 100px; padding-left: 10px;">Số điện thoại
        </td>
        <td style="width: 220px">
            <cc2:OboutTextBox ID="winDowThemCaNhan_dienThoaiCaNhan" runat="server" Width="100%" FolderStyle="~/App_Themes/Styles/Interface/OboutTextBox">
            </cc2:OboutTextBox>
        </td>
    </tr>
    <tr>
        <td>Thuộc tổ chức
        </td>
        <td colspan="3">
            <cc3:ComboBox ID="winDowThemCaNhan_maToChucpr_sd" runat="server" Width="100%" FilterType="Contains" Height="150" FolderStyle="~/App_Themes/Styles/Interface/OboutComboBox">
            </cc3:ComboBox>
        </td>
        <td style="width: 10px">
            <span style="color: red">*</span>
        </td>
    </tr>
    <tr>
        <td>Giới tính
        </td>
        <td style="width: 220px">
            <cc3:ComboBox ID="winDowThemCaNhan_gioiTinhCaNhan" runat="server" Width="100%" FolderStyle="~/App_Themes/Styles/Interface/OboutComboBox">
            </cc3:ComboBox>
        </td>
        <td style="width: 100px; padding-left: 10px;">Ngày sinh
        </td>
        <td style="width: 220px">
            <cc2:OboutTextBox ID="winDowThemCaNhan_ngaySinhCaNhan" runat="server" Width="80%" FolderStyle="~/App_Themes/Styles/Interface/OboutTextBox">
            </cc2:OboutTextBox>
            <obout:Calendar ID="Calendar6" runat="server" DatePickerMode="true" TextBoxId="winDowThemCaNhan_ngaySinhCaNhan"
                DateFormat="dd/MM/yyyy" TitleText="Chọn ngày" Columns="1" DatePickerImagePath="~/App_Themes/Styles/Date/date_picker1.gif"
                CultureName="vi-VN">
            </obout:Calendar>
        </td>
    </tr>
    <tr>
        <td>Địa chỉ thường trú
        </td>
        <td colspan="3">
            <cc2:OboutTextBox ID="winDowThemCaNhan_diaChiCaNhan" runat="server" Width="100%" FolderStyle="~/App_Themes/Styles/Interface/OboutTextBox">
            </cc2:OboutTextBox>
        </td>
    </tr>
    <tr>
        <td>CMND
        </td>
        <td style="width: 220px">
            <cc2:OboutTextBox ID="winDowThemCaNhan_CMNDCaNhan" runat="server" Width="100%" FolderStyle="~/App_Themes/Styles/Interface/OboutTextBox">
            </cc2:OboutTextBox>
        </td>
        <td style="width: 100px; padding-left: 10px;">Ngày cấp
        </td>
        <td style="width: 220px">
            <cc2:OboutTextBox ID="winDowThemCaNhan_ngayCapCaNhan" runat="server" Width="80%" FolderStyle="~/App_Themes/Styles/Interface/OboutTextBox">
            </cc2:OboutTextBox>
            <obout:Calendar ID="Calendar7" runat="server" DatePickerMode="true" TextBoxId="winDowThemCaNhan_ngayCapCaNhan"
                DateFormat="dd/MM/yyyy" TitleText="Chọn ngày" Columns="1" DatePickerImagePath="~/App_Themes/Styles/Date/date_picker1.gif"
                CultureName="vi-VN">
            </obout:Calendar>
        </td>
    </tr>
    <tr>
        <td>Nơi cấp
        </td>
        <td colspan="3">
            <cc2:OboutTextBox ID="winDowThemCaNhan_noiCapCaNhan" runat="server" Width="100%" FolderStyle="~/App_Themes/Styles/Interface/OboutTextBox">
            </cc2:OboutTextBox>
        </td>
    </tr>
</table>
