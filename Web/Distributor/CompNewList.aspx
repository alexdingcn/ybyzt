<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompNewList.aspx.cs" Inherits="Distributor_CompNewList" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>企业公告信息</title>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link href="../css/layer.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Company/js/jquery-1.11.1.min.js"></script>
    <script type="text/javascript" src="../js/CommonJs.js"></script>
    <script src="../js/layer.js" type="text/javascript"></script>
     <script type="text/javascript">
         $(document).ready(function () {
             $(document).on("keydown", function (e) {
                 if (e.keyCode == 13) {
                     $("input[type='text']").blur();
                     location.href = $("#A1").attr("href");
                 }
             })

         })
       </script>
</head>
<body class="root3">
    <form id="form1" runat="server">
    <Head:Head ID="Head2" runat="server" />
    <div class="w1200">
    <Left:Left ID="Left1" runat="server" ShowID="nav-6" />
    <div class="rightCon">
    <div class="info">
         <a id="navigation1" href="UserIndex.aspx">我的桌面</a>>
          <a id="navigation2" href="CompNewList.aspx" class="cur">企业公告信息</a>
        </div>
        <!--功能条件 start-->
        <div class="userFun">
            <div class="left">
                </div>
            <div class="right">
                <ul class="term">
                    <li>
                        <label class="head">
                            信息标题：</label><input runat="server" id="txtnewtitle" type="text" class="box" style=" width:100px;" /></li>
                    <li>
                        <label class="head">信息类型：</label>
                        <asp:DropDownList ID="ddlNewType" runat="server" Width="72px" CssClass="xl">
                            <asp:ListItem Value="">所有</asp:ListItem>
                            <asp:ListItem Value="1">新闻</asp:ListItem>
                            <asp:ListItem Value="2">通知</asp:ListItem>
                            <asp:ListItem Value="3">公告</asp:ListItem>
                            <asp:ListItem Value="4">促销</asp:ListItem>
                            <asp:ListItem Value="5">商家动态</asp:ListItem>
                        </asp:DropDownList>
                     </li>
                    <li>
                        <label class="head">是否置顶：</label>
                            <asp:DropDownList ID="ddlNewTop" runat="server" Width="72px" CssClass="xl">
                            <asp:ListItem Value="">所有</asp:ListItem>
                            <asp:ListItem Value="1">置顶</asp:ListItem>
                            <asp:ListItem Value="0">非置顶</asp:ListItem>
                        </asp:DropDownList>
                    </li>
                    <li> <label class="head">每页</label><input type="text" onkeyup='KeyInt(this)' onblur="KeyInt(this);" id="txtPager" name="txtPager" runat="server"
                        class="box3" /><label class="head">行</label></li>
                 
                </ul>
                <a id="A1" href="" class="btnBl" onserverclick="A_Seek" runat="server"><i class="searchIcon"></i>搜索</a><a href="javascript:void(0);" onclick="javascript:location.href='CompNewList.aspx'" class="btnBl"><i
                    class="resetIcon"></i>重置</a>
            </div>
        </div>
        <!--功能条件 end-->
        <div class="blank10">
        </div>
        <!--订单管理 start-->
        <div class="orderNr">
            <table class="PublicList list" id="orderBg" border="0" cellspacing="0" cellpadding="0">
                <asp:Repeater ID="rptCompNew" runat="server" >
                    <HeaderTemplate>
                        <thead>
                            <tr>
                                <th>
                                    信息标题
                                </th>
                                <th>
                                    信息类型
                                </th>
                                <th>
                                  发布日期
                                </th>
                                <th>
                                    是否置顶
                                </th>
                            </tr>
                        </thead>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <a href='CompNewInfo.aspx?KeyID=<%# Common.DesEncrypt(Eval("ID").ToString(), Common.EncryptKey)%>&type1=CompNewList'><%#Util.GetSubString(Eval("NewsTitle").ToString(), 40)%>&nbsp;</a>
                            </td>
                            <td>
                                <%# Common.GetCPNewStateName(Eval("NewsType").ToString())%>&nbsp;
                            </td>
                             <td>
                                 <%# Eval("Createdate","{0:yyyy-MM-dd HH:mm}")%>&nbsp;
                            </td>
                            <td>
                              <%# System.Convert.ToString(DataBinder.Eval(Container.DataItem, "IsTop")) == "0" ? "非置顶" : "置顶"%>&nbsp;
                            </td>
                        </tr>
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
