<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PromotionList.aspx.cs" Inherits="Company_PmtManager_PromotionList" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>促销管理</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            //首次打开页面加载 
            var type = '<%=Type %>';
            //var ProType = $("#ddrProType").val();
            $("#ddrProType").empty();
            var ProType = "<option value=\"-1\">全部</option>";
            if (type.toString() == "0") {
                ProType += "<option value=\"1\">赠品</option><option value=\"2\">优惠</option>";
            } else if (type.toString() == "1") {
                ProType += "<option value=\"3\">满送</option><option value=\"4\">打折</option>";
            } else {
                ProType += "<option value=\"5\">满减</option><option value=\"6\">打折</option>";
                $(".toolbar3 li:first").hide();
                //                $(".tablelist thead th:first").hide();
                //                $(".tablelist tbody tr td:nth-child(1)").hide();
            }
            $("#ddrProType").append(ProType);
            //$("#ddrProType").val(ProType);

            //新增促销
            $("#libtnAdd").click(function () {
                if (type == 2) {
                    window.location.href = 'PromotionAdd2.aspx?type=<%= this.Type %>';
                } else
                { window.location.href = 'PromotionAdd.aspx?type=<%= this.Type %>'; }
            });

            ///重置
            $("#li_Reset").click(function () {
                window.location.href = 'PromotionList.aspx';
            });
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("#btnSearch").trigger("click");
                }
            })

        });

        //搜索
        function ChkPage() {
            $("#btnSearch").trigger("click");
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-3" />
    
    <asp:Button ID="btnSearch" Text="搜索" runat="server" OnClick="btnSearch_Click" Style="display: none" />
    <div class="rightinfo">
        <!--当前位置 start-->
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx" target="_top">我的桌面 </a></li><li>></li>
                <li><a id="protitle" runat="server">促销管理</a></li>
            </ul>
        </div>
        <!--当前位置 end-->

        <!--功能按钮 start-->
        <div class="tools">
            <ul class="toolbar left">
                <li id="libtnAdd"><span>
                    <img src="../../Company/images/t01.png" /></span>新增促销</li>
            </ul>
            <div class="right">
                <ul class="toolbar right">
                    <li onclick="ChkPage()"><span>
                        <img src="../../Company/images/t04.png" /></span>搜索</li>
                    <%--<li class="liSenior"><span>
                            <img src="../../Company/images/t07.png" /></span>高级</li>--%>
                </ul>
                <ul class="toolbar3">
                    <li>促销标题:
                        <input runat="server" id="txtProtitle" type="text" class="textBox" />
                    </li>
                    <li>促销方式:
                        <select name="ProType" runat="server" id="ddrProType" class="downBox">
                            <option value="-1">全部</option>
                            <option value="1">赠品</option>
                            <option value="2">优惠</option>
                        </select>
                    </li>
                    <li>促销日期:
                        <input name="txtDate" runat="server" onclick="WdatePicker()" style="width: 115px;"
                            id="txtProDate" readonly="readonly" type="text" class="Wdate" value="" />&nbsp;-&nbsp;
                        <input name="txtDate" runat="server" onclick="WdatePicker()" style="width: 115px;"
                            id="txtEndProDate" readonly="readonly" type="text" class="Wdate" value="" />
                    </li>
                    <li>每页<input name="txtPageSize" type="text" class="textBox txtPageSize" id="txtPageSize"
                        style="width: 45px" runat="server" value="12" onkeyup="KeyInt(this);" onblur="KeyInt(this);" />条</li>
                </ul>
            </div>
        </div>
        <!--功能按钮 end-->
        <!--信息列表 start-->
        <table class="tablelist">
            <thead>
                <tr>
                    <th class="t6">
                        促销标题
                    </th>
                    <th class="t2">
                        促销开始日期
                    </th>
                    <th class="t2">
                        促销结束日期
                    </th>
                    <th class="t2">
                        促销方式
                    </th>
                    <th class="t5">
                        状 态
                    </th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rpt_Promotion" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                               <div class="tcle"> <% if (this.Type == "2")
                                   {%>
                                <a style="text-decoration: underline;" href='PromotionInfo2.aspx?KeyId=<%# Common.DesEncrypt(Eval("ID").ToString(),Common.EncryptKey) %>&type=<%# this.Type %>'>
                                    <%# Eval("ProTitle")%></a>&nbsp;
                                <%}
                                   else
                                   {  %>
                                <a style="text-decoration: underline;" href='PromotionInfo.aspx?KeyId=<%# Common.DesEncrypt(Eval("ID").ToString(),Common.EncryptKey) %>&type=<%# this.Type %>'>
                                    <%# Eval("ProTitle")%></a>&nbsp;
                                <%} %></div>
                            </td>
                            <td>
                               <div class="tc"> <%# Convert.ToDateTime(Eval("ProStartTime")).ToString("yyyy-MM-dd") %></div>
                              
                            </td>
                            <td>
                                <div class="tc"><%# Convert.ToDateTime(Eval("ProEndTime")).ToString("yyyy-MM-dd") %></div>
                            
                            </td>
                            <td>
                               <div class="tc"> <%# Common.GetProType(Eval("ProType")) %></div>
                              
                            </td>
                            <td>
                               <div class="tc"> <%# Common.GetIsEnabled(Eval("IsEnabled"))%></div>
                             
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <!--信息列表 end-->
        <!--列表分页 start-->
        <div class="pagin">
            <webdiyer:AspNetPager ID="Pager" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                NextPageText=">" PageSize="12" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
                ShowPageIndexBox="Always" TextAfterPageIndexBox="<span class='pagiPage'>页</span>"
                TextBeforePageIndexBox="<span class='jump'>跳转到:</span>" CustomInfoSectionWidth="20%"
                CustomInfoStyle="padding:5px 0 0 20px;cursor: default;color: #737373;" CustomInfoClass="message"
                ShowCustomInfoSection="Left" CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5"
                OnPageChanged="Pager_PageChanged">
            </webdiyer:AspNetPager>
        </div>
        <!--列表分页 end-->
    </div>
    </form>
</body>
</html>
