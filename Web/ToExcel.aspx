<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ToExcel.aspx.cs" Inherits="ToExcel" ValidateRequest="false" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <input type="hidden" runat="server" id="tbl" value="" />
      <asp:button runat="server" id="btn" onclick="btn_Click" style="display:none" />
      <script type="text/javascript">
          document.getElementById("<%=tbl.ClientID %>").value = window.opener.document.getElementById('<%=Request["cid"] %>').value;
          //window.opener.document.getElementById('<%=Request["cid"] %>').value = "";  //暂时屏蔽
          document.getElementById("<%=btn.ClientID %>").click();
    </script>
    </div>
    </form>
</body>
</html>
