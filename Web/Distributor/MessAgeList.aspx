<%@ Page Language="C#" AutoEventWireup="true" enableEventValidation="false" CodeFile="MessAgeList.aspx.cs" Inherits="Distributor_MessAgeList" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>我要咨询</title>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Company/js/jquery-1.11.1.min.js"></script>
    <script src="../js/layer/layer.js" type="text/javascript"></script>
    <script src="../js/layerCommon.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("input[type='text']").blur();
                    location.href = $("#A1").attr("href");
                }
            })
        })
        $(document).ready(function () {
            $('#orderBg tr:even').addClass("bg");
        });

    </script>
</head>
<body class="root3">
    <form id="form1" runat="server">
    <Head:Head ID="Head2" runat="server" />
    <div class="w1200">
    <Left:Left ID="Left1" runat="server" ShowID="nav-6" />
    <div class="rightCon">
    <div class="info"> <a id="navigation1" href="UserIndex.aspx">我的桌面</a>>
                <a id="navigation2" href="MessAgeList.aspx" class="cur">我要咨询</a></div>
        <!--功能条件 start-->
        <div class="userFun">
            <div class="left">
                <a href="MessAgeAdd.aspx" class="btnOr"><i class="addIcon"></i>新增</a></div>
            <div class="right">
                <ul class="term">
                    <li>
                        <label class="head">
                            标题：
                        </label>
                        <input id="txttitle" type="text" runat="server" class="box" style="width:110px;" maxlength="100" /></li>
                        <li>
                        <label class="head">
                            回复状态：
                        </label>
                            <select id="dllisanswer" class="xl" runat="server">
                            <option value="-1">全部</option>
                            
                            <option value="0">未回复</option>
                            <option value="1">已回复</option>
                            </select>
                        </li>
                        <li> <label class="head">每页</label><input type="text" onkeyup='KeyInt(this)' onblur="KeyInt(this);" id="txtPager" name="txtPager" runat="server"
                        class="box3" /><label class="head">行</label></li>
                </ul>
                <a href="#" id="A1" runat="server" onserverclick="A_Seek" class="btnBl"><i class="searchIcon"></i>搜索</a><a href="javascript:void(0);" onclick="javascript:location.href='MessAgeList.aspx'" class="btnBl"><i
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
                        <th style="width:350px;">
                            标题
                        </th>
                        <th>创建时间</th>
                        <th>
                            回复状态
                        </th>
                        <th>回复时间</th>
                        <th>
                            操作
                        </th>
                    </tr>
                </thead>
                </HeaderTemplate>
                <ItemTemplate>
                <tbody>
                    <tr>
                        <td>
                            <a href='<%#"MessAgeInfo.aspx?id="+ Eval("id") %>' style="float:left; padding-left:10px;" ><%#Eval("Title").ToString().Length>30?Eval("Title").ToString().Substring(0,30)+"...":Eval("Title")%></a>
                        </td>
                        <td>
                            <%#(DateTime)Eval("CreateDate") < DateTime.Parse("1991-01-01") ? "" : Common.GetDateTime((DateTime)Eval("CreateDate"),"yyyy-MM-dd")%>
                        </td>
                        <td><%#Eval("isanswer").ToString()=="0"?"未回复":"已回复" %></td>
                        <td><%#(DateTime)Eval("ReplyDate") < DateTime.Parse("1991-01-01") ? "" : Common.GetDateTime((DateTime)Eval("ReplyDate"),"yyyy-MM-dd") %></td>
                        <td>
                            <a href='<%#"MessAgeInfo.aspx?id="+ Eval("id") %>' >查看</a>　<a id="del" onserverclick="A_Del"  href="#" delid='<%#Eval("id") %>' runat="server" >删除</a>
                        </td>
                    </tr>
                </tbody>
                </ItemTemplate>
                </asp:Repeater>
            </table>
            
        </div>
        <!--订单管理 end-->

        <!--分页 start-->
        <div class="pagin">
            <webdiyer:AspNetPager ID="Pager" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
            NextPageText=">" PageSize="12" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
            ShowPageIndexBox="Always" TextAfterPageIndexBox="<span class='pagiPage'>页</span>"
            TextBeforePageIndexBox="<span class='jump'>跳转到:</span>" CustomInfoSectionWidth="20%"
            CustomInfoStyle="padding:5px 0 0 20px;cursor: default;color: #737373;" CustomInfoClass="message"
            ShowCustomInfoSection="Left" CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
            CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5" OnPageChanged="Pager_PageChanged">
        </webdiyer:AspNetPager>
        </div>
        <!--分页 end-->
    </div>
    </div>
    </form>
</body>
</html>