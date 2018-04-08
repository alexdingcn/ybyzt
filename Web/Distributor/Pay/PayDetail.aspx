<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayDetail.aspx.cs" Inherits="Distributor_Pay_PayDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>支付明细</title>
    <link href="../css/global.css" rel="stylesheet" type="text/css" />
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../Company/js/jquery-1.11.1.min.js"></script>
    <script type="text/javascript" src="../../js/CommonJs.js"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#PayDetail tr:even').addClass("bg");
        });
    </script>
    <style type="text/css">
        #PayDetail
        {
           /* margin-top:10px;   */ 
        }
    </style>
</head>
<body class="root3">
    <form id="form1" runat="server">
        <div class="orderNr">
            <asp:Repeater runat="server" ID="rptPayDetail">
                <HeaderTemplate>
                    <table class="PublicList list" id="PayDetail">
                        <thead>
                            <tr>
                                <th>厂商名称</th>
                                <th>代理商名称</th>
                                <th>类型</th>
                                <th>支付金额</th>
                                <th>支付日期</th>
                                <th>手续费</th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                            <tr>
                                <td><%# new Hi.BLL.BD_Company().GetModel(Convert.ToInt32(Eval("CompID"))).CompName%></td>
                                <td><%# Common.GetDis(Eval("DisID").ToString().ToInt(0),"DisName")%></td>
                                <td><%# Eval("paytype ")%></td>
                                <td><%# Convert.ToDecimal(Eval("PayPrice ")).ToString("N")%></td>
                                <td><%# Convert.ToDateTime(Eval("paytime")).ToString("yyyy-MM-dd")%></td>
                                <td><%# Convert.ToDecimal(Eval("sxf") + "" == "" ? "0" : Eval("sxf")).ToString("N")%></td>
                            </tr>
                </ItemTemplate>
                <FooterTemplate>
                        </tbody>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </form>
</body>
</html>
