<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ntsFormModel.aspx.cs" Inherits="ProOnline.ntsFormModel" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="cc1" %>
<%@ Register TagPrefix="obout" Namespace="Obout.SuperForm" Assembly="obout_SuperForm" %>
<%@ Register TagPrefix="obout" Namespace="Obout.Grid" Assembly="obout_Grid_NET" %>
<%@ Register TagPrefix="owd" Namespace="OboutInc.Window" Assembly="obout_Window_NET" %>
<%@ Register assembly="obout_Interface" namespace="Obout.Interface" tagprefix="obout" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <script type="text/javascript">
      
    </script>
</head>
<body>
    <form id="form1" runat="server">   

    <div>

    </div>
    </form>
</body>
</html>

<script type="text/javascript" language="JavaScript"> 
    function clickIE() { if (event.button == 2) {  return false; } } 
    function clickNS(e) { 
    if (document.layers || (document.getElementById && !document.all)) { 
    if (e.which == 2 || e.which == 3) {  return false; } 
    } 
    } 
    if (document.layers) { document.captureEvents(Event.MOUSEDOWN); document.onmousedown = clickNS; } 
    else if (document.all && !document.getElementById) { document.onmousedown = clickIE; } 
    document.oncontextmenu = new Function('return false') 
</script>