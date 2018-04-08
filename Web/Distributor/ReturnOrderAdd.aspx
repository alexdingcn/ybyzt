<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReturnOrderAdd.aspx.cs" Inherits="Distributor_ReturnOrderAdd" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>代理商后台</title>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Company/js/jquery-1.11.1.min.js"></script>
    <script src="../js/layer/layer.js" type="text/javascript"></script>
    <script src="../js/layerCommon.js" type="text/javascript"></script>
    <script src="../js/CommonJs.js" type="text/javascript"></script>
    <script type="text/javascript">
        function a_addordreturn() {
            if ($.trim($("#txtremark1").val()) == "") {
                layerCommon.msg("请填写退货理由",IconOption.感叹);
                return false;
            }
            else {
                $("#txtremark").val($.trim($("#txtremark1").val()));
            }
        }

        function clkbtnGrey() {
            window.parent.CloseGoods();
        };
    </script>
</head>
<body class="root3">
    <form id="form1" runat="server">
    <%--<Head:Head ID="Head1" runat="server" />
    <div class="w1200">
    <Left:Left ID="Left1" runat="server" ShowID="ReturnOrderList" />
    <div class="rightCon">
    <div class="info"><span class="homeIcon"></span><a href="">账户中心</a>><a id="navigation1" href="#">基本资料</a>><a id="navigation2" href="#" class="cur">基本信息</a></div>
        <!--功能条件 start-->
        <div class="userFun">
            <div class="left">
                <a href="#" onserverclick="A_AddOrderReturn" runat="server" class="btnOr"><i class="offIcon"></i>保存</a>
                <a href="orderlist.aspx" class="btnBl"><i class="returnIcon"></i>取消</a>
                </div>
        </div>
        <!--功能条件 end-->
        <div class="blank10">
        </div>
        <!--订单管理详细 start-->
        <div class="orderNr">
            <table class="orderDet" border="0" cellspacing="0" cellpadding="0" width="100%">
                <tbody>
                    <tr>
                        <td class="head">
                            订单编号
                        </td>
                        <td>
                            <%=receiptno %>
                        </td>
                        <td class="head">
                            下单用户
                        </td>
                        <td>
                            <%=truename %>
                        </td>
                        <td class="head">
                            订单类型
                        </td>
                        <td>
                            <%=Otype %>
                        </td>
                    </tr>
                    <tr>
                        <td class="head">
                            <i style="color:#ff4e02; font-size:14px">*</i>退货备注
                        </td>
                        <td colspan="5">
                            <textarea id="txtremark" maxlength="500" class="txtBox" runat="server"></textarea>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="orderLiv">
                <table class="PublicList" border="0" cellspacing="0" cellpadding="0">
                    <thead>
                        <tr>
                            <th>产品名称</th>
                            <th>产品代码</th>
                            <th>属 性</th>
                            <th>单 价</th>
                            <th>数 量</th>
                            <th>总 价</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater runat="server" ID="rpDtl">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <a href="../BusinessShop/ProductsView.aspx?goodsId=<%# GetGoodsID(Eval("goodsinfoid").ToString()) %>&comid=<%#  LoginModel.IsLogined(this).CompID %>" class="pic"><img height="50px" width="88px" src='<%# Common.picUrl(Eval("GoodsinfoID").ToString())%>' /></a>
                                        <a href="../BusinessShop/ProductsView.aspx?goodsId=<%# GetGoodsID(Eval("goodsinfoid").ToString()) %>&comid=<%#  LoginModel.IsLogined(this).CompID %>" class="txt"><%# Eval("GoodsName")%></a>
                                    </td>
                                    <td><%# Eval("GoodsCode")%></td>
                                    <td><%# Eval("GoodsInfos")%></td>
                                    <td><%# Convert.ToDecimal(Eval("AuditAmount")).ToString("N")%></td>
                                    <td><%# Convert.ToDecimal(Eval("GoodsNum")).ToString("0.00")%></td>
                                    <td><%# Convert.ToDecimal(Eval("Total")).ToString("N")%></td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                <tr id="tr" runat="server" visible='<%# bool.Parse((rpDtl.Items.Count==0).ToString())%>'>
                                    <td colspan="6" align="center">
                                        无匹配数据
                                    </td>
                                </tr>
                            </FooterTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
        </div>
        <!--订单管理 end-->
    </div>
    <div class="blank20">
    </div>
    </div>
    <Footer:Footer ID="Footer" runat="server" />--%>
    <textarea style="display:none" id="txtremark" runat="server"></textarea>
    <div style="width: 480px; position: fixed; height: 200px; background: #fff">
        <ul style="margin:20px 0 0 50px">
            <li><div class="left" style="margin:0; line-height:100px"><i style="color: #ff4e02; font-size: 14px">*</i>退货原因:</div><textarea id="txtremark1"
                maxlength="500" style="margin: 0 0 20px 5px; width: 300px; height: 100px; font-size:12px; padding:5px; line-height: 22px;"></textarea>
            </li>
            <li style=" text-align:center;"><a id="A1" href="#" onclick="return a_addordreturn();" onserverclick="A_AddOrderReturn" style="color:#fff;" runat="server" class="btnBl">确定</a> <a
                href="#" id="aclose" onclick="clkbtnGrey()"  class="btnGrey">关闭</a> </li>
        </ul>
    </div>
    </form>
</body>
</html>
