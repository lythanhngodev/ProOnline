<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeftBar.ascx.cs" Inherits="ProOnline.UserControl.LeftBar" %>
<asp:Repeater runat="server" ID="rpMainMenu" OnInit="rpMainMenu_OnInit">
    <HeaderTemplate>
        <div id="sidebar" class="sidebar responsive ace-save-state" style="position: absolute; z-index: 200" data-sidebar="true" data-sidebar-scroll="true" data-sidebar-hover="true">
            <%--			 <div id="sidebar" class="sidebar sidebar-fixed responsive menu-1024">	--%>
            <!-- sidebar menu start-->
            <ul class="nav nav-list">
    </HeaderTemplate>
    <ItemTemplate>
        <li class="<%# Eval("pathFile").ToString() == HttpContext.Current.Request.Url.AbsolutePath.ToString() ? "active":  CheckOpen(DataBinder.Eval(Container.DataItem, "functionName").ToString()) %> ">
            <a href="<%# Eval("pathFile") %>" class="<%# Eval("pathFile").ToString() =="" ? "dropdown-toggle": "" %>">
                <i class='menu-icon <%# Eval("IconName") %>'></i>
                <span class="menu-text"><%# Eval("dienGiai") %></span>
                <%# Eval("pathFile").ToString() =="" ? "<b class='arrow fa fa-angle-down'></b>": "" %>
            </a><b class="arrow"></b>
            <%# ChildMenu(DataBinder.Eval(Container.DataItem, "functionName").ToString())%>
        </li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
                <!-- sidebar menu end-->
        <div class="sidebar-toggle sidebar-collapse" id="sidebar-collapse">
            <i id="sidebar-toggle-icon" class="ace-icon fa fa-angle-double-left ace-save-state" data-icon1="ace-icon fa fa-angle-double-left" data-icon2="ace-icon fa fa-angle-double-right"></i>
        </div>
        </div>
    </FooterTemplate>
</asp:Repeater>
