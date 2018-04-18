<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SpendingUserInfo.aspx.cs" Inherits="Company_Financing_SpendingUserInfo" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>开销户详情</title>
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
    <div class="rightinfo" id="btnright" runat="server">
        
    <!--当前位置 start-->
    <div class="place" id="btntitle" runat="server">
        <ul class="placeul">
            <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
            <li><a href="#">在线融资</a></li><li>></li>
            <li><a href="#">开销户详情</a></li>
        </ul>
    </div>
        <div class="div_content" >
            <div class="lbtb">
            <table class="dh">
                <tr>
                    <td style="width:10%;">
                        <span>账户号</span>
                    </td>
                    <td style=" width:30%;">
                        <label id="lblAccNumver" runat="server">
                        </label>
                        &nbsp;
                    </td>
                    <td style="width:10%;">
                        <span>账户名称</span>
                    </td>
                    <td style=" width:30%;">
                        <label id="lblAccName" runat="server">
                                </label>
                                &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width:10%;">
                        <span>代理商名称</span>
                    </td>
                    <td style=" width:30%;">
                        <label id="lblAccountName" runat="server">
                                </label>
                                &nbsp;
                    </td>
                    <td style="width:10%;">
                        <span>代理商性质</span>
                    </td>
                    <td style=" width:30%;">
                        <label id="lblAccountNature" runat="server">
                                </label>
                                &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width:10%;">
                        <span>证件类型</span>
                    </td>
                    <td style=" width:30%;">
                        <label id="lblDocumentType" runat="server">
                                </label>
                                &nbsp;
                    </td>
                    <td style="width:10%;">
                        <span>证件号</span>
                    </td>
                    <td style=" width:30%;">
                        <label id="lblDocumentCode" runat="server">
                                </label>
                                &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width:10%;">
                        <span>组织机构代码证</span>
                    </td>
                    <td style=" width:30%;">
                        <label id="lblOrgCode" runat="server">
                                </label>
                                &nbsp;
                    </td>
                    <td style="width:10%;">
                        <span>营业执照</span>
                    </td>
                    <td style=" width:30%;">
                        <label id="lblBusinessLicense" runat="server">
                                </label>
                    </td>
                </tr>
                <tr>
                    <td style="width:10%;">
                        <span>融资开户状态</span>
                    </td>
                    <td style=" width:30%;">
                        <label id="lblState" runat="server">
                                </label>
                                &nbsp;
                    </td>
                    <td style="width:10%;">
                        <span>性别</span>
                    </td>
                    <td style=" width:30%;">
                        <label id="lblSex" runat="server" style="line-height: 20px;">
                                </label>
                                &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width:10%;">
                        <span>国籍</span>
                    </td>
                    <td style=" width:30%;">
                        <label id="lblNationality" runat="server">
                                </label>
                                &nbsp;
                    </td>
                    <td style="width:10%;">
                        <span>电话号码</span>
                    </td>
                    <td style=" width:30%;">
                        <label id="lblPhoneNumbe" runat="server">
                                </label>
                                &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width:10%;">
                        <span>传真</span>
                    </td>
                    <td style=" width:30%;">
                        <label id="lblFax" runat="server">
                                </label>
                                &nbsp;
                    </td>
                    <td style="width:10%;">
                        <span>手机</span>
                    </td>
                    <td style=" width:30%;">
                        <label id="lblPhone" runat="server">
                                </label>
                                &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width:10%;">
                        <span>邮箱</span>
                    </td>
                    <td style=" width:30%;">
                        <label id="lblEmail" runat="server">
                                </label>
                                &nbsp;
                    </td>
                    <td style="width:10%;">
                        <span>邮政编码</span>
                    </td>
                    <td style=" width:30%;">
                        <label id="lblPostCode" runat="server">
                                </label>
                                &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width:10%;">
                        <span>代理商地址</span>
                    </td>
                    <td colspan="3">
                        <label id="lblAccAddress" runat="server">
                        </label>
                        &nbsp;
                    </td>
                </tr>
            </table>
            </div>
        </div>
    </div>
    <!--当前位置 end-->
    </form>
</body>
</html>
