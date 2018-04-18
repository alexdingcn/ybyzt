<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SpendingUserInfo.aspx.cs"
    Inherits="Financing_SpendingUserInfo" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>开销户</title>
    <link href="../Distributor/css/global.css" rel="stylesheet" type="text/css" />
    <script src="../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/CommonJs.js" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            $("#editIcon").on("click", function () {
                location.href = 'SpendingUserAdd.aspx';
            })
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <Head:Head ID="Head1" runat="server" />
    <div class="w1200">
        <Left:Left ID="Left1" runat="server" ShowID="SpendingUserInfo" />
        <div class="rightCon">
            <div class="info">
                <span class="homeIcon"></span><a id="navigation1" href="#">基本资料</a>><a id="navigation2"
                    href="#" class="cur">基本信息</a></div>
            <div class="userFun">
                <div class="left">
                    <a href="javascript:void(0)" class="btnBl" id="editIcon" runat="server"><i class="editIcon">
                    </i>编辑</a>
                </div>
            </div>
            <div class="blank10">
            </div>
            <div class="orderNr">
                <table class="orderDet" border="0" cellspacing="0" cellpadding="0" width="100%">
                    <tbody>
                        <tr>
                            <td class="head" style="width: 10%">
                                账户号
                            </td>
                            <td style="width: 23%">
                                <label id="lblAccNumver" runat="server">
                                </label>
                                &nbsp;
                            </td>
                             <td class="head" style="width: 10%">
                                账户名称
                            </td>
                            <td  style="width: 23%">
                                <label id="lblAccName" runat="server">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>

                            <td class="head">
                                代理商名称
                            </td>
                            <td>
                                <label id="lblAccountName" runat="server">
                                </label>
                                &nbsp;
                            </td>
                              <td class="head">
                                代理商性质
                            </td>
                            <td>
                                <label id="lblAccountNature" runat="server">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="head" style="width: 10%">
                                证件类型
                            </td>
                            <td>
                                <label id="lblDocumentType" runat="server">
                                </label>
                                &nbsp;
                            </td>
                            <td class="head">
                                证件号
                            </td>
                            <td>
                                <label id="lblDocumentCode" runat="server">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="head">
                                组织机构代码证
                            </td>
                            <td>
                                <label id="lblOrgCode" runat="server">
                                </label>
                                &nbsp;
                            </td>
                            <td class="head">
                                营业执照
                            </td>
                            <td>
                                <label id="lblBusinessLicense" runat="server">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td class="head"  >
                                状态
                            </td>
                            <td>
                                <label id="lblState" runat="server">
                                </label>
                                &nbsp;
                            </td>
                            <td class="head">
                                性别
                            </td>
                            <td>
                                <label id="lblSex" runat="server" style="line-height: 20px;">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr id="trcompuser" runat="server">
                            <td class="head">
                                国籍
                            </td>
                            <td>
                                <label id="lblNationality" runat="server">
                                </label>
                                &nbsp;
                            </td>
                            <td class="head">
                                电话号码 
                            </td>
                            <td>
                                <label id="lblPhoneNumbe" runat="server">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr id="tr1" runat="server">
                            <td class="head">
                                传真 
                            </td>
                            <td>
                                <label id="lblFax" runat="server">
                                </label>
                                &nbsp;
                            </td>
                            <td class="head">
                                手机 
                            </td>
                            <td >
                                <label id="lblPhone" runat="server">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr id="tr2" runat="server">
                            <td class="head">
                                邮箱 
                            </td>
                            <td>
                                <label id="lblEmail" runat="server">
                                </label>
                                &nbsp;
                            </td>
                            <td class="head">
                                邮政编码 
                            </td>
                            <td>
                                <label id="lblPostCode" runat="server">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr id="tr3" runat="server">
                            <td class="head">
                                代理商地址 
                            </td>
                            <td colspan="3">
                                <label id="lblAccAddress" runat="server">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                        
                        
                    </tbody>
                </table>
            </div>
            <div class="blank20">
            </div>
        </div>
    </div>
    <Footer:Footer ID="Footer" runat="server" />
    </form>
</body>
</html>
