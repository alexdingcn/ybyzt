<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CMerchantsInfo.aspx.cs" Inherits="Distributor_CMerchants_CMerchantsInfo" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">    
    <title>招商信息</title>
     <link href="../css/global.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script> 
    <script src="../../Company/js/order.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $("#returnIcon").on("click", function () {
                window.location.href = 'CMerchantsList.aspx';
            })

            //申请合作
            $(document).on("click", ".btnCo", function () {
                
                //转向网页的地址; 
                var url = 'FirstCampAdd.aspx?KeyID=' + <%= this.KeyID %>;
                var index = layerCommon.openWindow("申请合作", url, '950px', '615px'); //记录弹出对象
                $("#hid_Alert").val(index); //记录弹出对象
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <Head:Head ID="Head2" runat="server" />
        <div class="w1200">
            <input type="hidden" runat="server" id="hid_Alert" value="" />

            <Left:Left ID="Left1" runat="server" ShowID="nav-7" />
            <div class="rightCon">
                <div class="info">
                    <a id="navigation1" href="/Distributor/UserIndex.aspx">我的桌面</a>>
                    <a id="navigation2" href="/CMerchants/CMerchantsList.aspx" class="cur">招商信息</a>
                </div>
                <!--功能条件 start-->
                <div class="userFun">
                    <div class="left">
                        <a href="javascript:void(0)" class="btnOr btnCo" id="btnCo" runat="server">
                            <i class="prnIcon"></i>申请合作</a>
                        <a href="#" class="btnBl" id="returnIcon" runat="server">
                            <i class="returnIcon"></i>返回</a>
                    </div>
                </div>
                <!--功能条件 end-->
                <div class="blank10"></div>
                <div class="orderNr">
                    <table class="orderDet" border="0" cellspacing="0" cellpadding="0" width="100%">
                        <tbody>
                            <tr>
                                <td class="head" style="width: 13%">招商编码</td>
                                <td style="width: 37%">
                                    <label id="lblCMCode" runat="server"></label>&nbsp;
                                </td>
                                 <td class="head" style="width: 13%">招商名称</td>
                                <td style="width: 37%">
                                    <label id="lblCMName" runat="server"></label>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="head" style="width: 13%">厂商名称</td>
                                <td style="width: 37%">
                                    <label id="lblCompName" runat="server"></label>&nbsp;
                                </td>
                                 <td class="head" style="width: 13%">商品编码</td>
                                <td style="width: 37%">
                                    <label id="lblGoodsCode" runat="server"></label>&nbsp;
                                </td>
                            </tr>
                             <tr>
                                <td class="head" style="width: 13%">商品名称</td>
                                <td style="width: 37%">
                                    <label id="lblGoodsName" runat="server"></label>&nbsp;
                                </td>
                                 <td class="head" style="width: 13%">规格型号</td>
                                <td style="width: 37%">
                                    <label id="lblValueInfo" runat="server"></label>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="head" style="width: 13%">生效日期</td>
                                <td style="width: 37%">
                                    <label id="lblForceDate" runat="server"></label>&nbsp;
                                </td>
                                 <td class="head" style="width: 13%">失效日期</td>
                                <td style="width: 37%">
                                    <label id="lblInvalidDate" runat="server"></label>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="head" style="width: 13%">需提供资料</td>
                                <td colspan="3">
                                    <label id="lblchk1" runat="server" visible="false">
                                    <input name="chk" value="1" disabled="disabled" runat="server" id="chk1" type="checkbox"/>
                                    <label for="chk1">营业执照</label></label>

                                     <label id="lblchk2" runat="server" visible="false">
                                    <input name="chk" value="2" disabled="disabled" runat="server" id="chk2" type="checkbox"/>
                                    <label for="chk2">医疗器械经营许可证</label>
                                    </label>

                                     <label id="lblchk3" runat="server" visible="false">
                                    <input name="chk" value="3" disabled="disabled" runat="server" id="chk3" type="checkbox"/>
                                    <label for="chk3">开户许可证</label>
                                    </label>

                                     <label id="lblchk4" runat="server" visible="false">
                                    <input name="chk" value="4" disabled="disabled" runat="server" id="chk4" type="checkbox"/>
                                    <label for="chk4">医疗器械备案</label>
                                    </label>
                                </td>
                            </tr>
                            <tr> 
                                <td class="head" style="width: 13%">备注</td>
                                <td colspan="3">
                                    <label id="lblRemark" runat="server"></label>&nbsp;
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <div class="opacity" style=" display:none;"></div>
    </form>
</body>
</html>
