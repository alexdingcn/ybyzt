<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RebateInfo.aspx.cs" Inherits="Company_SysManager_RebateInfo" %>

<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<%@ Register TagPrefix="uc2" TagName="DisDemo" Src="~/Company/UserControl/TreeDisName.ascx" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>返利详情</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            $_def.ID = "btnAdd";
            $('.tb tbody tr td:even').addClass('odd');
            $("#txtDisID_txt_txtDisName").css("width", "300px");
            
            $("li#libtnDel").on("click", function() {
               layerCommon.confirm("确认删除？", function() { $("#btnDel").trigger("click"); }, "提示");
            });
            $("li#libtnEdit").on("click", function() {
                location.href = "RebateModify.aspx?KeyID=" + <%= Request.QueryString["KeyID"] %>;
            });
        });

        var t1, t2;
        function DefaultAddr(t1, t2) {
            //不能删除，下来地址栏的一个方法
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Top ID="top1" runat="server" ShowID="nav-2" />
    <div class="rightinfo">
        <!--当前位置 start-->
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx" target="_top">我的桌面 </a></li>
                <li>></li>
                <li><a href="../SysManager/RebateList.aspx">返利查询</a></li><li>></li>
                <li><a href="#">返利详情</a></li>
            </ul>
        </div>
        <!--当前位置 end-->
        <div class="tools">
            <ul class="toolbar left">
                <li id="libtnEdit" runat="server"><span>
                    <img src="../../Company/images/t02.png" /></span>编辑</li>
                <li id="libtnDel" runat="server"><span>
                    <img src="../../Company/images/t03.png" /></span>删除
                    <input type="button" runat="server" id="btnDel" onserverclick="btn_Del" style="display: none;" />
                </li>
                <li id="libtnback" runat="server" onclick="location.href = 'RebateList.aspx'"><span>
                    <img src="../../Company/images/tp3.png" /></span>返回</li>
            </ul>
        </div>
        <div class="div_content">
            <table class="tb">
                <tbody>
                    <tr>
                        <td width="120">
                            <span>代理商名称</span>
                        </td>
                        <td>
                            <label runat="server" id="txtDisID">
                            </label>
                        </td>
                        <td width="120">
                            <span>返利单编号</span>
                        </td>
                        <td>
                            <label runat="server" id="txtCode">
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>本次返利金额</span>
                        </td>
                        <td class="newspan" colspan="3">
                            <label runat="server" id="txtRebateAmount">
                            </label>
                        </td>
                        <%--<td>
                            <span>返利类型</span>
                        </td>
                        <td>
                            <label runat="server" id="txtType">
                            </label>
                        </td>--%>
                    </tr>
                    <tr>
                        <td>
                            <span>有效期</span>
                        </td>
                        <td class="newspan">
                            <label runat="server" id="txtStartDate" style="min-width: 70px">
                            </label>
                            &nbsp;--&nbsp;
                            <label runat="server" id="txtEndDate" style="min-width: 70px">
                            </label>
                            <label runat="server" id="lblQixian" style=" color:Red;">
                            </label>
                        </td>
                        <td>
                            <span>备 注</span>
                        </td>
                        <td class="newspan">
                            <label runat="server" id="txtRemark">
                            </label>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div>
                <div class="div_title">
                    返利使用详情:
                </div>
                <table class="tablelist">
                    <thead>
                        <tr>
                            <th class="t3">
                                订单编号
                            </th>
                            <th class="t3">
                                使用金额
                            </th>
                            <th class="t3">
                                使用日期
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="Rpt_RobateDetail" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <div class="tc">
                                            <%# Eval("ReceiptNo")%></div>
                                    </td>
                                    <td>
                                        <div class="tc">
                                            <%# Eval("Amount","{0:f2}")%></div>
                                    </td>
                                    <td>
                                        <div class="tc">
                                            <%# Eval("CreateDate","{0:d}")%></div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
                <div class="pagin" style="height: 30px;">
                    <webdiyer:AspNetPager ID="Pager" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                        NextPageText=">" PageSize="5" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
                        ShowPageIndexBox="Always" TextAfterPageIndexBox="<span class='pagiPage'>页</span>"
                        TextBeforePageIndexBox="<span class='jump'>跳转到:</span>" CustomInfoSectionWidth="20%"
                        CustomInfoStyle="padding:5px 0 0 20px;cursor: default;color: #737373;" CustomInfoClass="message"
                        ShowCustomInfoSection="Left" CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                        CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5"
                        OnPageChanged="Pager_PageChanged">
                    </webdiyer:AspNetPager>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
