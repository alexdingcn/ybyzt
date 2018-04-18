<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SettlementInfo.aspx.cs" Inherits="Financing_SettlementInfo" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>结算账户</title>
    <link href="../Distributor/css/global.css" rel="stylesheet" type="text/css" />
    <script src="../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/CommonJs.js" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            $("#editIcon").on("click", function () {
                location.href = 'SettlementAdd.aspx';

            })
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <Head:Head ID="Head1" runat="server" />
    <div class="w1200">
        <Left:Left ID="Left1" runat="server" ShowID="SettlementInfo" />
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
                                开销户帐号
                            </td>
                            <td style="width: 23%">
                                <label id="lblAccNumver" runat="server">
                                </label>
                                &nbsp;
                            </td>
                            <td class="head" style="width: 10%">
                                银行帐号
                            </td>
                            <td style="width: 23%">
                                <label id="lblAccNo" runat="server">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="head">
                                开户名称
                            </td>
                            <td>
                                <label id="lblAccNm" runat="server">
                                </label>
                                &nbsp;
                            </td>
                              <td class="head" >
                                状态
                            </td>
                            <td style="width: 23%">
                                <label id="lblState" runat="server">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="head">
                                账户类别
                            </td>
                            <td>
                                <label id="lblAccTp" runat="server">
                                </label>
                                &nbsp;
                            </td>
                            <td class="head" >
                                跨行标示
                            </td>
                            <td >
                                <label id="lblCrsMk" runat="server">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="head">
                                分支行清算行号
                            </td>
                            <td>
                                <label id="lblOpenBkCd" runat="server">
                                </label>
                                &nbsp;
                            </td>
                            <td class="head">
                                分支行省份
                            </td>
                            <td>
                                <label id="lblPrcCd" runat="server">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="head">
                                分支行城市
                            </td>
                            <td>
                                <label id="lblCityCd" runat="server">
                                </label>
                            </td>
                            <td class="head">
                                开户行网点名称
                            </td>
                            <td>
                                <label id="lblOpenBkNm" runat="server" >
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="head">
                                开户行网点地址
                            </td>
                            <td>
                                <label id="lblOpenBkAddr" runat="server">
                                </label>
                            </td>
                            <td class="head">
                                币种
                            </td>
                            <td>
                                <label id="lblCcyCd" runat="server" >
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
