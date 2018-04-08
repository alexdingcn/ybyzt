<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReRequest.aspx.cs" Inherits="Distributor_Pay_ReRequest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>

<script language="C#" runat="server">
    String message = (String)HttpContext.Current.Items["message"];
    String signature = (String)HttpContext.Current.Items["signature"];
    String action = (String)HttpContext.Current.Items["action"];
</script>

<script language="JavaScript" type="text/JavaScript">
    function doSubmit() {
        document.form1.submit();
    }
</script>

<body onload="doSubmit()" class="root3">
    <form action="<%=action%>" name="form1" method="post">
    <input type="hidden" name="message" value="<%=message %>" />
    <input type="hidden" name="signature" value="<%=signature %>" />
    </form>
</body>
</html>
