<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Service.aspx.cs" Inherits="Company_SysManager_Service" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <%--<meta http-equiv="X-UA-Compatible" content="IE=edge" >--%>
    <meta http-equiv = "X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>购买服务</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />

    <link href="../../Distributor/newOrder/css/global2.5.css" rel="stylesheet" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            //选择服务
            $(".goods-title li").click(function () {
                $(".goods-title li").removeClass("hover");
                $(this).addClass("hover");
                if ($(this).attr("tip")=="1") {
                    $(".purchase-service").removeClass("none")
                    $("#servicelist").addClass("none")
                }
                else {
                    $("#servicelist").removeClass("none")
                    $(".purchase-service").addClass("none")
                }
            })

            //版本对比
            $(".contrast").click(function () {
                var index = layerCommon.openWindow('版本对比', 'contrast.aspx', '1016px', '600px');
                $("#hid_Alert").val(index);

            })

            //立即下单
            $("#Buy").click(function () {
                var hover = $("#service_1 .hover .txt").text();//选择的服务种类
                var day = 0;//天数
                var type = 0;
                type = 12;

                $.ajax({
                    type: "post",
                    data: { action: "Pay", type: type },
                    success: function (data) {
                        var json = JSON.parse(data);
                        if (json.rel == "OK") {
                            //window.open( "../Pay/Pay.aspx?isDBPay=" + json.compID + "&KeyID=" + json.Orderid + "");
                            window.open("../Pay/Pay.aspx?Orderid=" + json.Orderid + "");
                        }
                        else {
                            layerCommon.msg("网络异常 ！", IconOption.错误);
                        }
                    }, error: function () {
                        layerCommon.msg("网络异常 ！", IconOption.错误);
                    }
                })

            })
        })
        function Layerclose() {
            layerCommon.layerClose($("#hid_Alert").val());
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-6" />
    <input  type="hidden" id="Moneys" runat="server"/>
    <input type="hidden" id="hid_Alert" />
     <div class="rightinfo">
        <div class="place">
	        <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面</a></li><li>></li>
                <li><a href="../SysManager/Service.aspx">购买服务</a></li>
	        </ul>
     </div>
        
<!--购买服务 start-->
<ul class="goods-title">
	<li class="hover" tip="1"><a href="javascript:;">购买服务</a></li>
	<li tip="2"><a href="javascript:;">付款记录</a></li>
</ul>

<div class="purchase-service ">
	<div class="box left"><i class="size">免费版：5用户</i><b>永久免费</b></div>
    <ul class="ri left"><li>到期时间：永久有效</li><li>用户数：5用户</li><li><a href="#" id="Buy" class="bule-btn">购买标准版</a><i class="contrast">版本对比</i></li></ul>
</div>


<div class="goods-zs record none" id="servicelist">
<div class=" tabLine">
  <table border="0" cellspacing="0" cellpadding="0" style="text-align:center">
    <thead>
      <tr>
        <th>
                        厂商名称
                    </th>
                    <th>
                        服务类型
                    </th>
                    <th>
                        服务金额
                    </th>
                    <th>
                        到期时间
                    </th>
                    <th>
                        开通日期
                    </th>
                    <th>
                        处理状态
                    </th>
      </tr>
    </thead>
    <tbody>
       <asp:Repeater ID="Services" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                
                                    <%# Eval("CompName").ToString()%>
                               
                            </td>
                            <td>
                                <%# Eval("ServiceType").ToString()=="1"?"年费":"月费"%>
                            </td>
                            <td>
                                <%# Eval("Price")%>
                            </td>
                            <td>
                                <%#Eval("OutData","{0:yyyy-MM-dd}")%>
                            </td>
                            <td>
                                <%# Eval("CreateDate", "{0:yyyy-MM-dd}")%>
                            </td>
                            <td>
                                已支付
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
    </tbody>
  </table>
</div>
</div>

<!--购买服务 end-->

    
     </div>

     
    </form>
</body>
</html>
