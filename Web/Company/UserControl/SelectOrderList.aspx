<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectOrderList.aspx.cs" Inherits="Company_UserControl_SelectOrderList" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/DisAreaTreeBox.ascx" TagPrefix="uc1" TagName="DisArea" %>
<%@ Register Src="~/Company/UserControl/DisTypeTreeBox.ascx" TagPrefix="uc2" TagName="DisType" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <link href="../../Company/css/style.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
    <script src="../../js/layerCommon.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
    <script src="../../js/CommonJs.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            $(".tablelist tbody a#A_DisSelect").on("click", function () {
                var id = $(this).attr("disid");
                var name = $(this).siblings("#Hid_receiptno").val();
                window.parent.selectOrder(id, name);
            });

            $(".tablelist tbody tr").on("click", function () {
                var id = $(this).find("a#A_DisSelect").attr("disid");
                var name = $(this).find("a#A_DisSelect").siblings("#Hid_receiptno").val();
                window.parent.selectOrder(id, name);
            });

            $("#inpqx").on("click", function () {
                window.parent.CloseDialogTo_order();
            })

            $("#inpqk").on("click", function () {
                var id = "";
                var name = "";
                window.parent.selectOrder(id, name);
            })

            $(document).ready(function () {
                $('.tablelist tbody tr:odd').addClass('odd');
                $("li#liSearch").on("click", function () {
                    $("#btn_Search").trigger("click");
                })

            })

        })

        //        function onClickDis(id, Name) {
        //            //alert(id + " :" + Name);
        //            window.parent.selectDis(id, Name);
        //        }
    </script>
    <style>
        .clears
        {
            background: url(../images/btnbg2.png) repeat-x;
            color: #000;
            font-weight: normal;
        }
    </style>
</head>
<body style="min-width: 800px;">
    <form id="form1" runat="server">
    <div class="rightinfo" style=" margin-left:0px;">
        <div class="tools">
            <input name="" type="button" class="sure" id="inpqk" value="清空选择" />

            <div class="right">
                <ul class="toolbar right">
                    <li id="liSearch"><span>
                        <img src="../../Company/images/t04.png" /></span>搜索</li>
                </ul>
                <input type="button" runat="server" id="btn_Search" style="display: none;" onserverclick="btn_SearchClick" />
                <ul class="toolbar3">
                    <li>订单编号:<input runat="server" id="txtOrdercode" type="text" class="textBox" />&nbsp;&nbsp;</li>
                    <%--<li>代理商简称:<input runat="server" id="txtDisSname" type="text" class="textBox" />&nbsp;&nbsp;</li>--%>
                   <%-- <li>代理商分类:<uc2:DisType runat="server" Id="txtDisType" /></li>
                    <li>代理商区域:<uc1:DisArea runat="server" Id="txtDisAreaBox" /></li>--%>
                </ul>
            </div>
        </div>
        <table class="tablelist">
            <thead>
                <tr>
                     <th>
                                    订单编号
                                </th>
                                <th>
                                    创建时间
                                </th>
                                <th>
                                    订单金额（元）
                                </th>
                                <th>
                                    订单类型
                                </th>
                                <th>
                                    订单状态
                                </th>
                                <th>
                                    支付状态
                                </th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="Rpt_Dis" runat="server">
                    <ItemTemplate>
                        <tr style=" cursor:pointer;">
                            <td>
                                <a id="A_DisSelect" style="cursor: pointer; text-decoration: underline; display:block" disid="<%#Eval("ID")%>">
                                   </a> <%# Eval("receiptno")%>
                                <input type="hidden" value="<%#Eval("receiptno")%>" id="Hid_receiptno" />  </td>
                            <td>
                                <%#Common.GetDateTime((DateTime)Eval("createdate"),"yyyy-MM-dd") %>
                            </td>
                            <td>
                                <%# Math.Round((decimal)Eval("AuditAmount"),2)%>&nbsp;
                            </td>
                            <td>
                                <%# OrderInfoType.OType((int)Eval("otype")) %>&nbsp;
                            </td>
                            <td>
                                <%# (int)Eval("returnstate")==0?OrderInfoType.OState((int)Eval("id")):"退货处理中" %>&nbsp;
                            </td>
                            <td style="width:100px;">
                                <%# OrderInfoType.PayState((int)Eval("paystate")) == "已支付" ? "<a title='查看支付清单' href='javascript:void(0)' onclick='showPayDetail(" + Eval("ID") + ")'>已支付</a>" : OrderInfoType.PayState((int)Eval("paystate"))%>&nbsp;
                            </td>
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
        <%--<div class="div_footer">
            <input name="" type="button" class="cancel" id="inpqx" value="取消" />
            <input name="" type="button" class="cancel" id="inpqk" value="清空" />
        </div>--%>
    </div>
    </form>
</body>
</html>
