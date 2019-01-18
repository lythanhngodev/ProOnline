<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ProOnline._Default" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>
<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc3" %>
<%@ Register Assembly="obout_Window_NET" Namespace="OboutInc.Window" TagPrefix="owd" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="cc2" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="cc1" %>
<body>
    <form id="frm" runat="server">
        <%-- <cc2:OboutTextBox ID="txtcheck" runat="server" Width="100%" Height="70px" TextMode="MultiLine"></cc2:OboutTextBox>
        <cc2:OboutButton ID="xxxx" runat="server" OnClick="checkxxxxx" Text="Phân tích" FolderStyle="~/App_Themes/Styles/Interface/OboutButton"></cc2:OboutButton>--%>
        <table style="width: 100%;">
            <tr>
                <td style="width: 150px;">Chuỗi kết nối</td>
                <td>
                    <cc2:OboutTextBox ID="txtConn" runat="server" Width="100%" FolderStyle="~/App_Themes/Styles/Interface/OboutTextBox"></cc2:OboutTextBox></td>
            </tr>
            <tr>
                <td>Tên đăng nhập</td>
                <td>
                    <cc2:OboutTextBox ID="txtTenDangNhap" Text="banqldatambinh1" runat="server" Width="100%" FolderStyle="~/App_Themes/Styles/Interface/OboutTextBox"></cc2:OboutTextBox></td>
            </tr>
            <tr>
                <td>Năm thao tác</td>
                <td>
                    <cc2:OboutTextBox ID="txtNamSuDung" Text="2018" runat="server" Width="100%" FolderStyle="~/App_Themes/Styles/Interface/OboutTextBox"></cc2:OboutTextBox></td>
            </tr>
            <tr>
                <td colspan="2">
                    <cc2:OboutButton ID="OboutButton1" runat="server" OnClick="setMaDonVi1" Text="Set mã đơn vị" FolderStyle="~/App_Themes/Styles/Interface/OboutButton"></cc2:OboutButton>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="te">
                    <div id="value" runat="server"></div>
                </td>
            </tr>
        </table>

        <script type="text/javascript">
            document.getElementById("txtMACAdress").value = unescape(macAddress);
            document.getElementById("txtIPAdress").value = unescape(ipAddress);
            document.getElementById("txtComputerName").value = unescape(computerName);
        </script>
    </form>
    <script>
        var myExtension = {
            myListener: function (evt) {

                //netscape.security.PrivilegeManager.enablePrivilege( 'UniversalXPConnect' ); 
                var dnsComp = Components.classes["@mozilla.org/network/dns-service;1"];
                var dnsSvc = dnsComp.getService(Components.interfaces.nsIDNSService);
                var compName = dnsSvc.myHostName;
                alert(compName);
            }
        }
        window.onload = function () {
            console.log(myExtension);
        }
    </script>
    <style type='text/css'>
        ul#topnav {
            margin: 0;
            padding: 0;
            float: left;
            width: 100%;
            list-style: none;
            position: relative;
            font-size: 1.2em;
            background: #383838;
        }

            ul#topnav li {
                float: left;
                margin: 0;
                padding: 0; 
            }

                ul#topnav li a {
                    padding: 10px 15px;
                    display: block;
                    color: #f0f0f0;
                    text-decoration: none;
                }
                 
                ul#topnav li span {
                    float: left;
                    padding: 15px 0;
                    position: absolute;
                    left: 0;
                 
                    display: none;
                    width: 100%;
                    background: #1376c9;
                    color: #fff; 
                }

                ul#topnav li:hover span {
                    display: block;
                }

                ul#topnav li span a {
                    display: inline;
                }

                    ul#topnav li span a:hover {
                        text-decoration: underline; 
                    }
                    .dropdown_1column, .dropdown_2columns, .dropdown_3columns, .dropdown_4columns, .dropdown_5columns { 
                        display:block;
    float: left;
    position: absolute; 
    text-align: left;
    background-color:  black;
     text-align: left;
        width:initial;
  
}.col_1, .col_2, .col_3, .col_4, .col_5 {
    display: inline;
    float: left;
    position: relative; 
     text-align: left;
}
ul#topnav li span .dropdown_4columns .col_1 ul li {
    font-size: 12px; 
    position: relative;
    padding: 0;
    margin: 0;
    float: none;
    text-align: left; 
     width:initial;
     list-style:none;
     left:0px;
     margin:1px;
    
}
    </style>
    <script src='http://ajax.googleapis.com/ajax/libs/jquery/1.3/jquery.min.js' type='text/javascript' />
    <script type='text/javascript'>
        $(document).ready(function () {

            $("ul#topnav li").hover(function () { //Hover over event on list item
                $(this).css({ 'background': '#1376c9 url() repeat-x' }); //Add background color + image on hovered list item
                $(this).find("span").show(); //Show the subnav
            }, function () { //on hover out...
                $(this).css({ 'background': 'none' }); //Ditch the background
                $(this).find("span").hide(); //Hide the subnav
            });

        });
    </script>
     
</body>
