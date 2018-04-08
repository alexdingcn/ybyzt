<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DisIntegral.aspx.cs" Inherits="Company_SysManager_DisIntegral" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>代理商积分</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $(".cancel").click(function () {
                window.parent.LayerClose();
            });

            //重置
            $("#li_Reset").click(function () {
                window.location.href = 'DisIntegral.aspx?DisId=<%=this.DisID %>';
            });

            //搜索
            $("#liSearch").click(function () {
                $("#btn_Search").trigger("click");
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="rightinfo" style=" margin-top:0px; margin-left:0px; width:auto;">
            <input type="button" runat="server" id="btn_Search" style="display: none;" onserverclick="btn_SearchClick" />
            <div class="tools">
                <div class="right">
                    <ul class="toolbar right">
                        <li id="liSearch"><span>
                            <img src="../../Company/images/t04.png" /></span>搜索</li>
                       
                    </ul>
                    <ul class="toolbar3">
                        <li>
                            订单编号：<input type="text" id="txtReceiptNo" runat="server" class="textBox" maxlength="50"/>
                        </li>
                        <li>
                            获取日期：
                            <input name="txtCreateDate" runat="server" onclick="WdatePicker()" style="width: 115px;"
                                id="txtCreateDate" readonly="readonly" type="text" class="Wdate" value="" />&nbsp;-&nbsp;
                            <input name="txtEndCreateDate" runat="server" onclick="WdatePicker()" style="width: 115px;"
                                id="txtEndCreateDate" readonly="readonly" type="text" class="Wdate" value="" />
                        </li>
                        <li>
                            每页<input type="text" onkeyup='KeyInt(this)' id="txtPager" name="txtPager" runat="server"
                                class="textBox" style="width: 40px;" />&nbsp;条
                        </li>
                    </ul>
                </div>
            </div>
            <table class="tablelist">
                <thead>
                     <tr>
                        <th class="t5">
                            代理商名称
                        </th>
                        <th class="t5">
                            订单编号
                        </th>
                        <th class="t5">
                            积分类型
                        </th>
                        <th class="t5">
                            期末积分
                        </th>
                        <th class="t5">
                            本次积分
                        </th>
                        <th class="t5">
                            最新积分
                        </th>
                        <th class="t5">
                            积分来源
                        </th>
                        <th class="t5">
                            获取日期
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="Rpt_Dis" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><div class="tc"><%# Common.GetDis(Eval("DisId").ToString().ToInt(0),"DisName") %></div></td>
                                <td><div class="tc"><%# OrderInfoType.getOrder(Eval("OrderID").ToString(), "ReceiptNo")%></div></td>
                                <td><div class="tc"><%# Enum.GetName(typeof(Enums.IntegralType), Eval("IntegralType").ToString().ToInt(0))%></div></td>
                                <td><div class="tc"><%# Eval("OldIntegral").ToString().ToDecimal(0).ToString("0.00") %></div></td>
                                <td><div class="tc"><%# (Eval("type").ToString() == "1" ? "" : "-") + Eval("Integral").ToString().ToDecimal(0).ToString("0.00")%> </div></td>
                                <td><div class="tc"><%# Eval("NewIntegral").ToString().ToDecimal(0).ToString("0.00") %></div></td>
                                <td><div class="tc"><%# Eval("Source") %></div></td>
                                <td><div class="tc"><%# Convert.ToDateTime(Eval("CreateDate")).ToString("yyyy-MM-dd") %></div></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
            <div class="pagin" style="height: 30px;">
                <webdiyer:AspNetPager ID="Pager" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                    NextPageText=">" PageSize="12" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
                    ShowPageIndexBox="Always" TextAfterPageIndexBox="<span class='pagiPage'>页</span>"
                    TextBeforePageIndexBox="<span class='jump'>跳转到:</span>" CustomInfoSectionWidth="30%"
                    CustomInfoStyle="padding:5px 0 0 20px;cursor: default;color: #737373;" CustomInfoClass="message"
                    ShowCustomInfoSection="Left" CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                    CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5"
                    OnPageChanged="Pager_PageChanged">
                </webdiyer:AspNetPager>
            </div>
            <div class="div_footer">
                <input name="" type="button" class="cancel" id="inpqx" value="关闭" />
            </div>
        </div>
    </form>
</body>
</html>
