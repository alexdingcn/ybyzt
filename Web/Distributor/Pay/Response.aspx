<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Response.aspx.cs" Inherits="Distributor_Pay_Response" %>

<%@ Import Namespace="CFCA.Payment.Api" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head id="Head1" runat="server">
<meta http-equiv="X-UA-Compatible" content="IE=edge">
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
<title>模拟企业</title>
<link rel="stylesheet" type="text/css" href="~/WebRoot/css/Common.css"/>
<link rel="stylesheet" type="text/css" href="~/WebRoot/css/Form.css"/>
<link rel="stylesheet" type="text/css" href="~/WebRoot/css/Table.css"/>
</head>

<script language="C#" runat="server">
    String plainText = XmlUtil.formatXmlString((String)HttpContext.Current.Items["plainText"]);

    String txCode = (String)HttpContext.Current.Items["txCode"];
    String txName = (String)HttpContext.Current.Items["txName"];
</script>
<body class="root3">
<p class="title">模拟企业</p>
<table width="100%" cellpadding="2" cellspacing="1" border="0" align="center" bgcolor="#DDDDDD">
    <tr>
        <td class="head" height="24" colspan="2"><%=txName%>(<%=txCode%>)</td>
    </tr>
    <tr class="mouseout">
        <td width="120" class="label" height="400">响应报文</td>
        <td width="*" class="content">            
            <textarea name="RequestPlainText" cols="100" rows="20" wrap="off"><%=plainText%></textarea>
        </td>
    </tr>
</table>
</body>
</html>