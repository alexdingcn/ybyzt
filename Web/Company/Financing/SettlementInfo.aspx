<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SettlementInfo.aspx.cs" Inherits="Company_Financing_SettlementInfo" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>结算账户详情</title>
    <link href="../../css/odrerstyle.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="" />
    <div class="rightinfo">

    <!--当前位置 start-->
    <div class="place">
        <ul class="placeul">
            <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
            <li><a href="#">在线融资</a></li><li>></li>
            <li><a href="#">结算账户详情</a></li>
        </ul>
    </div>
        <div class="div_content">
            <div class="lbtb">
                <table class="dh">
                    <tr>
                        <td class="head" style="width: 10%">
                            <span>开销户帐号</span>
                        </td>
                        <td style="width: 23%">
                            <label id="lblAccNumver" runat="server">
                            </label>
                            &nbsp;
                        </td>
                        <td class="head" style="width: 10%">
                            <span>银行帐号</span>
                        </td>
                        <td style="width: 23%">
                            <label id="lblAccNo" runat="server">
                            </label>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="head">
                            <span>开户名称</span>
                        </td>
                        <td>
                            <label id="lblAccNm" runat="server">
                            </label>
                            &nbsp;
                        </td>
                            <td class="head" >
                            <span>结算户状态</span>
                        </td>
                        <td style="width: 23%">
                            <label id="lblState" runat="server">
                            </label>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="head">
                            <span>账户类别</span>
                        </td>
                        <td>
                            <label id="lblAccTp" runat="server">
                            </label>
                            &nbsp;
                        </td>
                        <td class="head" >
                            <span>跨行标示</span>
                        </td>
                        <td >
                            <label id="lblCrsMk" runat="server">
                            </label>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="head">
                            <span>分支行清算行号</span>
                        </td>
                        <td>
                            <label id="lblOpenBkCd" runat="server">
                            </label>
                            &nbsp;
                        </td>
                        <td class="head">
                            <span>分支行省份</span>
                        </td>
                        <td>
                            <label id="lblPrcCd" runat="server">
                            </label>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="head">
                            <span>分支行城市</span>
                        </td>
                        <td>
                            <label id="lblCityCd" runat="server">
                            </label>
                        </td>
                        <td class="head">
                            <span>开户行网点名称</span>
                        </td>
                        <td>
                            <label id="lblOpenBkNm" runat="server" >
                            </label>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="head">
                            <span>开户行网点地址</span>
                        </td>
                        <td>
                            <label id="lblOpenBkAddr" runat="server">
                            </label>
                        </td>
                        <td class="head">
                            <span>币种</span>
                        </td>
                        <td>
                            <label id="lblCcyCd" runat="server" >
                            </label>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
