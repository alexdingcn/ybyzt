<%@ Page Language="C#" AutoEventWireup="true" CodeFile="message.aspx.cs" Inherits="Distributor_message" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>我要咨询</title>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/jquery.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#orderBg tr:even').addClass("bg");
        });
    </script>
</head>
<body class="root3">
    <form id="form1" runat="server">
    <Head:Head ID="Head1" runat="server" />
    <div class="w1200">
    <Left:Left ID="Left1" runat="server" ShowID="message" />
    <div class="rightCon">
    <div class="info"> <a id="navigation1" href="UserIndex.aspx">我的桌面</a>>
                <a id="navigation2" href="#" class="cur">我要咨询</a></div>
        <!--功能条件 start-->
        <div class="userFun">
            <div class="left">
                <a href="user12Add.html" class="btnOr"><i class="addIcon"></i>新增</a></div>
            <div class="right">
                <ul class="term">
                    <li>
                        <label class="head">
                            标题
                        </label>
                        <input type="text" class="box" /></li>
                        <li>每页<input type="text" onkeyup='KeyInt(this)' id="txtPager" name="txtPager" runat="server"
                        class="textBox" style="width: 40px;" />行</li>
                </ul>
                <a href="" class="btnBl"><i class="searchIcon"></i>搜索</a><a href="" class="btnBl"><i
                    class="resetIcon"></i>重置</a>
            </div>
        </div>
        <!--功能条件 end-->
        <div class="blank10">
        </div>
        <!--订单管理 start-->
        <div class="orderNr">
            <table class="PublicList list" id="orderBg" border="0" cellspacing="0" cellpadding="0">
            <asp:Repeater ID="rptmessage" runat="server">
            <HeaderTemplate>
                <thead>
                    <tr>
                        <th>
                            标题
                        </th>
                        <th>
                            回复内容
                        </th>
                        <th>回复时间</th>
                        <th width="80">
                            操作
                        </th>
                    </tr>
                </thead>
                </HeaderTemplate>
                <ItemTemplate>
                <tbody>
                    <tr>
                        <td>
                            <a href="user12view.html"><%#Eval("Title")%></a>
                        </td>
                        <td>
                            <%#Eval("Remark") %>
                        </td>
                        <td><%#(DateTime)Eval("ReplyDate") < DateTime.Parse("1991-01-01") ? "" : Eval("ReplyDate") %></td>
                        <td>
                            <a href="" class="btnOr">查看</a><a href="" class="txtn">取消</a>
                        </td>
                    </tr>
                </tbody>
                </ItemTemplate>
                </asp:Repeater>
            </table>
            <!--分页 start-->
            <div class="page">
                <webdiyer:AspNetPager ID="Pager" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                     NextPageText=">"  PageSize="12" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
                     ShowPageIndexBox="Always" TextAfterPageIndexBox="<span style='margin-left:5px;'>页</span>"
                       TextBeforePageIndexBox="<span>跳转到: </span>"
                     CssClass="list" CurrentPageButtonClass="paginItem"
                    OnPageChanged="Pager_PageChanged" >
                </webdiyer:AspNetPager>
            </div>
            <!--分页 end-->
        </div>
        <!--订单管理 end-->
    </div>
    </div>
    </form>
</body>
</html>
