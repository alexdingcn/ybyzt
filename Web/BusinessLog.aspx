<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BusinessLog.aspx.cs" Inherits="BusinessLog" EnableViewState="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>日志</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <link href="Company/css/style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body 
        {
            font-family: "微软雅黑";
            margin: 0 auto;
            min-width: 450px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <!--信息列表 start-->
        <div style="padding:10px;">
        <asp:Repeater runat="server" ID="rptLog">
            <HeaderTemplate>
                <table class="tablelist">
                    <thead>
                        <tr>
                            <th class="t1">操作说明</th>
                            <th class="t3">操作时间</th>
                            <th class="t1">操作人</th>
                            <th>备 注</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td> <div class="tc"> <%# Eval("LogType")%></div></td>
                    <td> <div class="tc"> <%# Convert.ToDateTime(Eval("LogTime")).ToString("yyyy-MM-dd HH:mm") %></div></td>
                    <td> <div class="tc"> <%# Eval("OperatePerson")%></div></td>
                    <td><div class="tcle"> <%# Eval("LogRemark")%></div></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                    </tbody>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        </div>
        <!--信息列表 end-->

        <div class="tipbtn" style=" margin-left: 340px;">
            <input name="" type="button"  class="cancel" value="关闭" onclick='window.parent.layerCommon.layerClose("hid_Alert");' />
	    </div>
    </form>

</body>
</html>
