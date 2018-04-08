<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PromotionInfo2.aspx.cs" Inherits="Company_PmtManager_PromotionInfo2" %>

<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>促销详细</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <link href="../../css/odrerstyle.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <style type="text/css">
        input[type='text']
        {
            width: 150px;
        }
        .send
        {
            border-bottom: 1px solid #d6dee3;
            outline: 0;
            border: 0;
            height: 14px;
            line-height: 18px;
            border-bottom: 1px solid #d6dee3;
            color: #555;
            padding: 2px 12px;
            font-size: 12px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            //返回
            $("#cancel").click(function () {
                window.location.href = 'PromotionList.aspx?type=<%=Request["Type"] %>';
            });

            //编辑
            $("#liEdit").click(function () {
                window.location.href = 'PromotionAdd2.aspx?KeyId=<%=Common.DesEncrypt(this.KeyID.ToString(), Common.EncryptKey) %>&type=<%=Request["Type"] %>';
            });

            //停用
            $("#liNo").click(function () {
                layerCommon.confirm("确认停用该促销活动吗？", function () { $("#btnNo").trigger("click"); }, "提示");
            });

            //删除
            $("#liDel").click(function () {
                layerCommon.confirm("确认删除该促销活动吗？", function () { $("#btnDel").trigger("click"); }, "提示");
            });

            //启用
            $("#liOk").click(function () {
                layerCommon.confirm("确认启用该促销活动吗？", function () { $("#btnOk").trigger("click"); }, "提示");
            });

        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-3" />
    <div class="rightinfo" id="btnright" runat="server">
        <!--当前位置 start-->
        <div class="place" id="btntitle" runat="server">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面</a></li><li>></li>
                 <li><a id="protitle" runat="server"></a></li><li>></li>
                <li><a href="javascript:void(0);">促销详细</a></li>
            </ul>
        </div>
        <!--当前位置 end-->
        <!--功能按钮 start-->
        <div class="tools" id="btn" runat="server" style="padding-left: 5px;">
            <ul class="toolbar left">
                <li id="liEdit" runat="server"><span>
                    <img src="../images/t02.png" alt="" /></span>编辑 </li>
                <li id="liNo" runat="server"><span>
                    <img src="../images/lock.png" alt="" /></span>停用 </li>
                <li id="liOk" runat="server"><span>
                    <img src="../images/nlock.png" alt="" /></span>启用 </li>
                <li id="liDel" runat="server"><span>
                    <img src="../images/t03.png" alt="" /></span>删除 </li>
                <li id="cancel" runat="server"><span>
                    <img src="../images/tp3.png" alt="" /></span>返回 </li>
            </ul>
        </div>
        <!--功能按钮 end-->
        <asp:Button ID="btnNo" runat="server" OnClick="btnNo_Click" Text="禁用" Style="display: none" />
        <asp:Button ID="btnOk" runat="server" OnClick="btnOk_Click" Text="启用" Style="display: none" />
        <asp:Button ID="btnDel" runat="server" OnClick="btnDel_Click" Text="删除" Style="display: none" />
        <div class="div_content">
            <!--促销主体 start-->
            <div style="padding-top: 5px;">
                <div class="lbtb">
                    <table class="dh">
                        <tr>
                            <td style="width: 140px;">
                                <span>促销开始日期</span>
                            </td>
                            <td>
                                <label id="lblProStartDate" runat="server">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span>促销结束日期</span>
                            </td>
                            <td>
                                <label id="lblProEndDate" runat="server">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span>是否启用</span>
                            </td>
                            <td>
                                <label id="lblProIsEnabled" runat="server">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span>是否启用阶梯促销</span>
                            </td>
                            <td>
                                <label id="lblProIsJieTi" runat="server">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span>促销方式</span>
                            </td>
                            <td>
                                <label id="lblProType" runat="server">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="background: #f6f6f6 none repeat scroll 0 0;">
                                <span>促销条件</span>
                            </td>
                            <td>
                                <div class="SendFull" id="SendFull" runat="server">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span>促销描述</span>
                            </td>
                            <td>
                                <label id="lblProInfos" runat="server">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <!--促销主体 end-->
            <!--清除浮动-->
            <div style="clear: none;">
            </div>
        </div>
    </div>
    </form>
</body>
</html>
