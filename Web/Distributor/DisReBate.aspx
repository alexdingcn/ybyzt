<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DisReBate.aspx.cs" Inherits="Distributor_DisReBate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>代理商返利</title>
    <link href="../Company/css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    
    <script type="text/javascript">
         $(document).ready(function () {
             $(".cancel").click(function () {
                 window.parent.layerCommon.layerClose("hid_Alert");
             });
         });
    </script>

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
                            <th class="t1">返利单号</th>
                            <%--<th class="t3">厂商</th>--%>
                            <th class="t1">返利类型</th>
                            <th class="t1">返利金额</th>
                            <th class="t1">已使用金额</th>
                            <th class="t1">可用返利余额</th>
                            <th class="t1">有效期开始日期</th>
                            <th class="t1">有效期结束日期</th>
                            <th>备 注</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><div class="tc"><%# Eval("ReceiptNo")%></div></td>
                    <%--<td><div class="tc"><%# Common.GetCompValue(Eval("CompID").ToString().ToInt(0),"CompName") %></div></td>--%>
                    <td><div class="tc"><%# Eval("RebateType").ToString() == "1" ? "整单返利" : "分摊返利"%></div></td>
                    <td><div class="tc"><%# Eval("RebateAmount").ToString().ToDecimal(0).ToString("N") %></div></td>
                    <td><div class="tc"><%# Eval("UserdAmount").ToString().ToDecimal(0).ToString("N")%></div></td>
                    <td><div class="tc"><%# Eval("EnableAmount").ToString().ToDecimal(0).ToString("N")%></div></td>
                    <td><div class="tc"><%# Eval("StartDate").ToString().ToDateTime().ToString("yyyy-MM-dd") %></div></td>
                    <td><div class="tc"><%# Eval("EndDate").ToString().ToDateTime().ToString("yyyy-MM-dd")%></div></td>
                    <td><div class="tcle"> <%# Eval("Remark") %></div></td>
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
            <input name="" type="button"  class="cancel" value="关闭" />
	    </div>
    </form>
</body>
</html>
