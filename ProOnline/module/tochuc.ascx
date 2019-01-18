<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="tochuc.ascx.cs" Inherits="ProOnline.module.tochuc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>
<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc3" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="cc2" %>
<%@ Register Assembly="obout_Window_NET" Namespace="OboutInc.Window" TagPrefix="owd" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="cc1" %>
<script>
    function resetThongTinToChuc() {

        winDowThemToChuc_MaToChucpr.value(ProOnline.module.tochuc.layMaToChuc().value);
        winDowThemToChuc_tenToChuc.value('');
        winDowThemToChuc_maToChucpr_sd.value('');
        winDowThemToChuc_diaChi.value('');
        winDowThemToChuc_maSoThue.value();
        winDowThemToChuc_dienThoaiToChuc.value('');
        winDowThemToChuc_faxToChuc.value('');
        winDowThemToChuc_emailToChuc.value('');
        winDowThemToChuc_daiDienToChuc.value('');
        winDowThemToChuc_chucVuDaiDien.value('');
        winDowThemToChuc_nguoiLienHe.value('');
        winDowThemToChuc_chucVuLienHe.value('');
        winDowThemToChuc_dienThoaiLienHe.value('');
    }
    function luuThongTinToChuc() {
        var param = new Array();
        param[0] = winDowThemToChuc_MaToChucpr.value();
        param[1] = winDowThemToChuc_tenToChuc.value();
        param[3] = winDowThemToChuc_maToChucpr_sd.value();
        param[4] = winDowThemToChuc_diaChi.value();
        param[5] = winDowThemToChuc_maSoThue.value();
        param[6] = winDowThemToChuc_dienThoaiToChuc.value();
        param[7] = winDowThemToChuc_faxToChuc.value();
        param[8] = winDowThemToChuc_emailToChuc.value();
        param[9] = winDowThemToChuc_daiDienToChuc.value();
        param[10] = winDowThemToChuc_chucVuDaiDien.value();
        param[11] = winDowThemToChuc_nguoiLienHe.value();
        param[12] = winDowThemToChuc_chucVuLienHe.value();
        param[13] = winDowThemToChuc_dienThoaiLienHe.value(); 
        param[14] = "";
        param[15] = "";
        param[16] = "";
        if (isEmtyValue(param[0])) {
            return "Mã tổ chức không được rỗng"
        }
        if (isEmtyValue(param[1])) {
            return "Tên tổ chức không được rỗng";
        }
        return ProOnline.module.tochuc.luuThongTinToChuc(param).value;
    } 
</script>
<table style="width: 565px">
    <tr>
        <td style="width: 100px">Mã
        </td>
        <td>
            <cc2:OboutTextBox ID="winDowThemToChuc_MaToChucpr" Enabled="false" runat="server" Width="100%" FolderStyle="~/App_Themes/Styles/Interface/OboutTextBox">
            </cc2:OboutTextBox>
        </td>
        <td>
            <span style="color: red">*</span>
        </td>
        <td></td>
    </tr>
    <tr>
        <td>Tên tổ chức
        </td>
        <td colspan="3">
            <cc2:OboutTextBox ID="winDowThemToChuc_tenToChuc" runat="server" Width="100%" FolderStyle="~/App_Themes/Styles/Interface/OboutTextBox">
            </cc2:OboutTextBox>
        </td>
        <td style="width: 10px">
            <span style="color: red">*</span>
        </td>
    </tr>
    <tr>
        <td>Thuộc nhóm
        </td>
        <td colspan="3">
            <cc3:ComboBox ID="winDowThemToChuc_maToChucpr_sd" runat="server" Width="100%" Height="150" FolderStyle="~/App_Themes/Styles/Interface/OboutComboBox">
            </cc3:ComboBox>
        </td>
    </tr>
    <tr>
        <td>Địa chỉ
        </td>
        <td colspan="3">
            <cc2:OboutTextBox ID="winDowThemToChuc_diaChi" runat="server" Width="100%" FolderStyle="~/App_Themes/Styles/Interface/OboutTextBox">
            </cc2:OboutTextBox>
        </td>
    </tr>
    <tr>
        <td>Mã số thuế
        </td>
        <td style="width: 220px">
            <cc2:OboutTextBox ID="winDowThemToChuc_maSoThue" runat="server" Width="100%" FolderStyle="~/App_Themes/Styles/Interface/OboutTextBox">
            </cc2:OboutTextBox>
        </td>
        <td style="width: 100px">Điện thoại
        </td>
        <td style="width: 220px">
            <cc2:OboutTextBox ID="winDowThemToChuc_dienThoaiToChuc" runat="server" Width="100%" FolderStyle="~/App_Themes/Styles/Interface/OboutTextBox">
            </cc2:OboutTextBox>
        </td>
    </tr>
    <tr>
        <td>Fax
        </td>
        <td style="width: 220px">
            <cc2:OboutTextBox ID="winDowThemToChuc_faxToChuc" runat="server" Width="100%" FolderStyle="~/App_Themes/Styles/Interface/OboutTextBox">
            </cc2:OboutTextBox>
        </td>
        <td style="width: 100px">Email
        </td>
        <td style="width: 220px">
            <cc2:OboutTextBox ID="winDowThemToChuc_emailToChuc" runat="server" Width="100%" FolderStyle="~/App_Themes/Styles/Interface/OboutTextBox">
            </cc2:OboutTextBox>
        </td>
    </tr>
    <tr>
        <td>Người đại diện
        </td>
        <td style="width: 220px">
            <cc2:OboutTextBox ID="winDowThemToChuc_daiDienToChuc" runat="server" Width="100%" FolderStyle="~/App_Themes/Styles/Interface/OboutTextBox">
            </cc2:OboutTextBox>
        </td>
        <td style="width: 100px">Chức vụ
        </td>
        <td style="width: 220px">
            <cc2:OboutTextBox ID="winDowThemToChuc_chucVuDaiDien" runat="server" Width="100%" FolderStyle="~/App_Themes/Styles/Interface/OboutTextBox">
            </cc2:OboutTextBox>
        </td>
    </tr>
    <tr>
        <td>Người liên hệ
        </td>
        <td style="width: 220px">
            <cc2:OboutTextBox ID="winDowThemToChuc_nguoiLienHe" runat="server" Width="100%" FolderStyle="~/App_Themes/Styles/Interface/OboutTextBox">
            </cc2:OboutTextBox>
        </td>
        <td style="width: 100px">Chức vụ
        </td>
        <td style="width: 220px">
            <cc2:OboutTextBox ID="winDowThemToChuc_chucVuLienHe" runat="server" Width="100%" FolderStyle="~/App_Themes/Styles/Interface/OboutTextBox">
            </cc2:OboutTextBox>
        </td>
    </tr>
    <tr>
        <td>Điện thoại
        </td>
        <td colspan="3">
            <cc2:OboutTextBox ID="winDowThemToChuc_dienThoaiLienHe" runat="server" Width="100%" FolderStyle="~/App_Themes/Styles/Interface/OboutTextBox">
            </cc2:OboutTextBox>
        </td>
    </tr>
</table>
