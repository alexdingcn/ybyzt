<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserIndex.aspx.cs" Inherits="Distributor_UserIndex" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>代理商管理后台</title>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Company/js/jquery-1.11.1.min.js"></script>
    <script src="../js/CommonJs.js" type="text/javascript"></script>
    <script src="../js/layer/layer.js" type="text/javascript"></script>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link href="css/shop.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        
        function btnupdate() {
            if ($.trim($("#txtpwd1").val()) == "") {
                $("#spanpwd").text("-原密码不能为空").css("display", "inline-block");
                return false;
            }
            if ($.trim($("#txtpwd2").val()) == "123456") {
                $("#spanpwd").text("-不能使用系统默认密码作为新密码").css("display", "inline-block");
                return false;
            }
            if ($.trim($("#txtpwd2").val()) == "") {
                $("#spanpwd").text("-新密码不能为空").css("display", "inline-block");
                return false;
            }
            if ($.trim($("#txtpwd3").val()) == "") {
                $("#spanpwd").text("-确认密码不能为空").css("display", "inline-block");
                return false;
            }
            if ($.trim($("#txtpwd3").val()) != $.trim($("#txtpwd2").val())) {
                $("#spanpwd").text("-两次输入的密码不一致").css("display", "inline-block");
                return false;
            }

            if ($("#txtpwd2").val().length < 6 || $("#txtpwd2").val().length > 16) {
                $("#spanpwd").text("-新密码长度必须大于6位，小于16位").css("display", "inline-block");
                return false;
            }
            return true;

        }

        $(function () {
            $("#DisImport").css({
                'top': ($(window).height() - $("#DisImport").height()) / 2,
                'left': ($(window).width() - $("#DisImport").width()) / 2
            });
            $(window).scroll(function () {
                $("#DisImport").css({
                    'top': ($(window).height() - $("#DisImport").height()) / 2 + $(window).scrollTop(),
                    'left': (($(window).width() - $("#DisImport").width()) / 2) + $(window).scrollLeft()
                });
            });
            $(window).resize(function () {
                $("#DisImport").css({
                    'top': ($(window).height() - $("#DisImport").height()) / 2,
                    'left': ($(window).width() - $("#DisImport").width()) / 2
                });
            });
        });
        function pay(Id) {
            //window.location.href = 'Pay.aspx?KeyID=' + Id;
            window.open('pay/Pay.aspx?isDBPay=<%=Common.DesEncrypt("0",Common.EncryptKey) %>&KeyID=' + Id);
        }
        function payDB(Id) {
            //window.location.href = 'PayDB.aspx?KeyID=' + Id;
            window.open('pay/Pay.aspx?isDBPay=<%=Common.DesEncrypt("1",Common.EncryptKey) %>&KeyID=' + Id);
        }
    </script>
</head>
<body class="root3">
    <form id="form1" runat="server">
    <Head:Head ID="Head1" runat="server" />
    <div class="w1200">
        <Left:Left ID="Left1" runat="server" ShowID="" />
        
        <div class="rightCon">
            <div class="info">
                <a id="navigation1" href="#">我的桌面</a></div>
            <!--快捷入口 start-->
            <ul class="quick-btn">
                <li class="money-q"><a class="a1" href="CMerchants/CMerchantsList.aspx"><i class="icon-mo"></i>
                    <h3 class="t">
                        招商列表</h3>
                    </a></li>
                <li class="order-q"><a class="a1" href="newOrder/orderBuy.aspx"><i class="icon-or"></i>
                    <h3 class="t">
                        立即下单</h3>
                    </a></li>
                <li class="sale-q"><a class="a1" href="GoodsList.aspx?cx=cx"><i class="icon-sa"></i>
                    <h3 class="t">
                        最新促销<i class="size"><%=Cx_Sum%></i></h3>
                    </a></li>
                <li class="collect-q"><a class="a1" href="GoodsList.aspx?sc=sc"><i class="icon-co"></i>
                    <h3 class="t">
                        收藏商品</h3>
                    </a></li>
				<li class="overdue-q"><a class="a1" href="/Distributor/Storage/GoodsStockList.aspx?type=1"><i class="icon-ov"></i>
                    <h3 class="t">
                        快过期商品<i class="size"><%=goods_Sum%></i></h3>
                    </a></li>
            </ul>
            <!--快捷入口 end-->
            <div class="blank10">
            </div>
            <!--简报 start-->
            <div class="briefing left">
                <div class="mh-title">
                    简报</div>
                <ul class="list">
                    <li>今日订购额：<i class="size" style=" color:Red;"><%=DaySum %></i>元</li>
                    <li>今日订单数：<i class="size" style=" color:Red;"><%=dayOrderCount %></i>笔</li>
                    <li>本月订购额：<i class="size" style=" color:Red;"><%=MonthSum%></i>元</li>
                    <li>本月订单数：<i class="size" style=" color:Red;"><%=orderCount%></i>笔</li>
                    <li>本月付款额：<i class="size" style=" color:Red;"><%=PaymentSum %></i>元</li>
                    <li>本月未付额：<i class="size" style=" color:Red;"><%=PayableSum %></i>元</li>
                </ul>
            </div>
            <!--简报 end-->
            <!--最新公告 start-->
            <div class="new-sale right">
                <div class="mh-title">
                    最新公告<a href="CompNewList.aspx" class="more"></a></div>
                <ul class="list" runat="server" id="ULNewList">
                </ul>
            </div>
            <!--最新公告 end-->
            <div class="blank10">
            </div>
            <!--订单 start-->
            <div class="goods-jxs">
                <div class="mh-title">
                    订单<a href="OrderList.aspx" class="more"></a></div>
                <div class="goods-list">
                    <table border="0" cellspacing="0" cellpadding="0" class="tab-m">
                        <%  if (orderl.Count > 0)
                            {%>
                        <thead>
                            <tr>
                                <th class="t4">
                                    订单编号
                                </th>
                                <th class="t1">
                                    下单时间
                                </th>
                                <th class="t2">
                                    订单金额
                                </th>
                                <th class="t2">
                                    已支付金额
                                </th>
                                <th class="t5">
                                    订单状态
                                </th>
                                <th class="t5">
                                    支付状态
                                </th>
                                <th class="t3">
                                    操作
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <%  
                                    int i = 0;
                                    foreach (Hi.Model.DIS_Order orderl_mode in orderll)
                                    {
                                %>
                                <tr>
                                    <td>
                                        <div class="tc-or">
                                            <a href="neworder/orderdetail.aspx?KeyID=<%=orderl_mode.ID %>">
                                                <%=orderl_mode.ReceiptNo%></a></div>
                                    </td>
                                    <td>
                                        <div class="tc">
                                            <%=orderl_mode.CreateDate.ToString("yyyy-MM-dd HH:mm:ss")%></div>
                                    </td>
                                    <td>
                                        <div class="tc">
                                            ￥<%=Math.Round(orderl_mode.AuditAmount)%></div>
                                    </td>
                                    <td>
                                        <div class="tc">
                                            ￥<%=Math.Round(orderl_mode.PayedAmount)%></div>
                                    </td>
                                    <td>
                                        <div class="tc">
                                            <%=OrderInfoType.OState(orderl_mode.ID)%>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="tc">
                                            <%=OrderInfoType.PayState(orderl_mode.PayState)%>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="tc alink">
                                            <%=Getmessage(orderl_mode.OState, orderl_mode.PayState, orderl_mode.ReturnState, orderl_mode.ID)%>
                                            <a href="neworder/orderdetail.aspx?KeyID=<%=orderl_mode.ID %>">订单详情</a></div>
                                    </td>
                                </tr>
                                <%
               
                                i++;
                                if (i >= 3)
                                    break;
                                }
                                %>
                        </tbody>
                        <%  }
                            else
                            {%>
                        <!-- 无订单数据时显示内容及图片  start-->
                        <div class="noOr-box"><i class="noOr-i"></i>无订单，快去下单吧！</div>
                        <!-- 无订单数据时显示内容及图片  end-->
                        <%} %>
                    </table>
                </div>
            </div>
            <!--订单 end-->
        </div>
    </div>
    <div class="tip" style="display: none; z-index: 999;" id="DisImport" runat="server">
        <div style="position: absolute; top: 0; left: 0; right: 0; bottom: 0; z-index: 999;
            background: #fff;">
            <div class="tiptop">
                <span>修改登录密码</span></div>
            <div class="tipinfo">
                <ul class="ModifyData">
                    <li><i class="head" style="width: 100px;"><i class="required">*</i>原密码：</i><input
                        id="txtpwd1" name="" type="password" runat="server" class="box" value="" /></li>
                    <li><i class="head" style="width: 100px;"><i class="required">*</i>新密码：</i><input
                        id="txtpwd2" name="" type="password" runat="server" class="box" value="" /></li>
                    <li><i class="head" style="width: 100px;"><i class="required">*</i>确认密码：</i><input
                        id="txtpwd3" name="" type="password" runat="server" class="box" value="" /></li>
                    <li style="text-align: center;"><span id="spanpwd" runat="server" style="color: Red;
                        height: 40px; width: 100%; text-align: center; display: block;"></span></li>
                </ul>
                <div class="mdBtn" style="text-align: center;">
                    <a id="A1" href="#" onclick="if(!btnupdate()){return false;}" onserverclick="Btn_Update"
                        style="margin: 0" runat="server" class="btnYe">确定修改</a></div>
            </div>
        </div>
        <div style="z-index: 998; opacity: 0.3; filter: Alpha(Opacity=30) !important; border-radius: 5px;
            top: -8px; left: -8px; right: -8px; bottom: -8px; background-color: rgb(0, 0, 0);
            position: absolute; top">
        </div>
    </div>
    
    <div id="zzc" class="zzc" runat="server" style="display: none;">
    </div>
    </form>
</body>
</html>
     <%-- 新闻标红样式--%>
       <style>
        .red a {
            color:red;
        }
    </style>