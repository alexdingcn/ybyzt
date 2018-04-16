<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TopControl.ascx.cs" Inherits="UserControl_TopControl" %>
<script>

    $(document).ready(function () {
        //账户切换
        $(document).on("click", ".swUser", function () {
            var cuID = $(this).closest(".role-cur").find("div.list").find("a").attr("tip");
            $.ajax({
                type: 'post',
                url: '../Handler/UserSwitch.ashx',
                async: true, //false:同步 true: 异步
                dataType: 'json',
                data: { action: "switch", ID: cuID },
                success: function (data) {
                    if (data != "") {
                        if (data["type"] == true) {
                            //layerCommon.msg(data["str"], IconOption.笑脸);
                            //location.reload();
                            //window.location.href = "/eshop/index.aspx?comid=" + data["CompID"].toString();
                            window.location.href = "/"+data["CompID"].toString()+".html";
                        } else {
                            layerCommon.msg(data["str"], IconOption.哭脸);
                        }
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    layerCommon.msg("服务器异常，请稍后再试", IconOption.哭脸);
                }
            });
        });
        $(document).on("click", ".topnav .role-cur .out", function () {
            $("#<%=A_QuitLogin.ClientID%>")[0].click();
        })


    });
    function GeAccountUserList(CallBack) {
        $.ajax({
            type: 'post',
            url: '../Handler/UserSwitch.ashx',
            dataType: 'json',
            data: { action: "UserList" },
            success: function (data) {
                $_UserSwicth(data);
                if (typeof CallBack == "function") {
                    CallBack();
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                if (typeof CallBack == "function") {
                    CallBack();
                }
            }
        });
    }

    //加载角色Html
    $_UserSwicth = function (data) {
        var coma = 3;
        var disa = 3;
        var userHtml = "";
        userHtml = "<div class=\"role-cur\">";
        if (data.length > 0) {
            var compHtml = "<div class=\"title\"><i class=\"swUser\"><i class=\"hx-i\"></i>厂商</i><a href='javascript:void(0)' class='out'>退出</a></div><div class=\"list\">";
            var disHtmltop = "<div class=\"title\"><i class=\"swUser\"><i class=\"jx-i\"></i>代理商</i></div><div class=\"list\">";
            var disHtml = "";

            for (var i in data) {
                var Audit = parseInt(data[i].IsAudit) == 2 ? "已审" : "未审";
                if (parseInt(data[i].Ctype) == 1) {
                    compHtml += "<a tip=\"" + data[i].CUID + "\" href=\"javascript:void(0)\">" + "</a>";
                    coma++;
                }
                else if (parseInt(data[i].Ctype) == 2) {
                    disHtml += "<a tip=\"" + data[i].CUID + "\" href=\"javascript:void(0)\">" + "</a>";
                    disa++;
                }
            }
            compHtml += "</div>";
            disHtml += "</div>";

            if (parseInt(coma) > 3)
                userHtml += compHtml;
            if (parseInt(disa) > 3) {
                if (parseInt(coma) <= 3) {
                    disHtmltop = "<div class=\"title\"><i class=\"swUser\"><i class=\"jx-i\"></i>代理商</i><a href='javascript:void(0)' class='out'>退出</a></div><div class=\"list\">";
                }
                userHtml += disHtmltop + disHtml;
            }
        } else {
            userHtml = '<div class="role-cur" style="right:0;padding-left:30px;"><a href="javascript:void(0)" class="out" style="color:#008be3">退出</a>';
        }
        userHtml += "</div>";
        $(".topnav .fl").append(userHtml);
    }
    function eshop() {
        window.location.href = "/"+<%=this.Comp %>+".html";
    }
</script>
<!--顶部导航栏 start-->
<div class="topnav">
    <a  id="A_QuitLogin" runat="server"  onserverclick="QuitLogin_Click" class="out" style="display:none;">退出</a> 
    <div class="n">
        <div class="fl mt-login" runat="server" id="mlogin">
           <a href="<%=ResolveUrl("../login.html") %>">登录</a>| <a href="<%=ResolveUrl("../compOrdisRegister.html") %>">
                注册</a>
        </div>
        <div class="fl role" id="userSw" runat="server">
          <a href="javascript:void(0);" style="cursor: hand;" class="user">
                <i runat="server" id="Headjx" style="cursor: hand;" class="jxGray-i">
                </i>
                <label id="username" style="cursor: hand;" runat="server">
                    </label>，
                <label id="compname" style="cursor: hand;" runat="server">
                    </label>
            </a>
        </div>
        <%--        代理商头部--%>
        <ul class="fr" id="disTop" runat="server">
            <li class="fore1"><a href="javascript:;" class="a1">我的医站通</a> <i class="down-i"></i>
                <div class="cur">
                    <a href="/Distributor/OrderList.aspx">我的订单</a> <a href="/Distributor/pay/orderPayList.aspx">
                        待支付订单</a> <a href="/Distributor/GoodsList.aspx?sc=sc">
                            我的收藏</a>
                </div>
            </li>
            <li class="line"><em>|</em></li>
            <li class="fore2"><a href="<%=ResolveUrl("../Distributor/Shop.aspx") %>" class="a2">
                <i class="shop-i"></i>购物车<i class="red">(<i runat="server" id="TopE_CartNum" class="TopE_CartNum">0</i>)</i>
            </a><i class="down-i"></i>
                <div class="cur" id="TopE_StoreCart" runat="server">
                </div>
            </li>
            <li class="line"><em>|</em></li>
<%--            <li class="fore3"><a href="/Distributor/UserIndex.aspx">管理中心</a></li>--%>
<%--            <li class="line"><em>|</em></li> --%>
            <li class="fore3"><a href="/AppDown.aspx?type=Comp">APP下载</a><i class="down-i"></i><div class="cur"><img alt="关注官方微信" src="images/app-qr.png" width="110" height="110"></div></li>
            <li class="line"><em>|</em></li> 
            <li class="fore1" style="display:none;"><a href="javascript:void(0)">网站导航</a><i class="down-i"></i>
				<div class="cur2">
                	<div class="title">行业市场</div>
                    <ul class="name">
                        <asp:Repeater ID="Gtype" runat="server">
                            <ItemTemplate>
                                <li><a href="/comlist_<%#Eval("ID") %>.html"><%#Eval("TypeName") %></a></li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                    <div class="title">新手指南</div>
                    <ul class="name">
                    	<li><a href="/CompRegister.html">卖家入驻</a></li><li><a href="/goodslist.html">买家加盟</a></li><li><a href="/guide.html">入市指南</a></li><li><a href="/help/help.html">帮助中心</a></li>
                    </ul>
                    
                    <div class="title">网站申明</div>
                    <ul class="name">
                    	<li><a href="/statement_1.html">权利申明</a></li><li><a href="/statement_2.html">免责申明</a></li><li><a href="/statement_3.html">保密申明</a></li><li><a href="/statement_4.html">链接申明</a></li>
                    </ul>
                    <div class="title">关于我们</div>
                    <ul class="name">
                    	<li><a href="/about_1.html">医伴金服</a></li>
                        <li><a href="/about_2.html">企业文化</a></li>
                        <li><a href="/about_1.html">公司简介</a></li>
                        <li><a href="/about_3.html">联系我们</a></li>
                    </ul>
				</div>
                <li class="line" style="display:none;"><em>|</em></li> 
            	<li class="fore1"><a href="<%=Common.GetWebConfigKey("WebDomainName")  %>">医站通首页</a></i>
               
            </li>
        </ul>
        
<%--        厂商头部--%>
        <ul class="fr" id="compTop" runat="server">
            <li class="fore1"><a href="javascript:;" class="a1">我的医站通</a> <i class="down-i"></i>
                <div class="cur">
                    <a href="../Company/Order/OrderCreateList.aspx">我的订单</a> <a href="../Company/Report/ArrearageRpt.aspx">
                        我的收款</a> <a href="../Company/Order/OrderCreateList.aspx?type=3">我要发货</a> <a href="../Company/Report/CustSaleRpt.aspx">
                            我的报表</a>
                </div>
            </li>
            <li class="line"><em>|</em></li>
<%--            <li class="fore3"><a href="../Company/jsc.aspx">管理中心</a></li>--%>
<%--            <li class="line"><em>|</em></li> --%>
            <li class="fore3"><a href="/AppDown.aspx?type=Comp">APP下载</a><i class="down-i"></i><div class="cur"><img alt="关注官方微信" src="images/app-qr.png" width="110" height="110"></div></li>
            <li class="line"><em>|</em></li> 
            <li class="fore1" style="display:none;"><a href="javascript:void(0)">网站导航</a><i class="down-i"></i>
				<div class="cur2">
                	<div class="title">行业市场</div>
                    <ul class="name">
                    	<li><a href="/comlist_21.html">食品酒水</a></li><li><a href="/comlist_22.html">建材家居</a></li><li><a href="/comlist_23.html">礼品百货</a></li><li><a href="/comlist_24.html">化工建材</a></li>
                        <li><a href="/comlist_25.html">机械五金</a></li><li><a href="/comlist_26.html">电工安防</a></li><li><a href="/comlist_27.html">纺织皮革</a></li><li><a href="/comlist_28.html">服饰配饰</a></li>
                        <li><a href="/comlist_29.html">智能电子</a></li><li><a href="/comlist_30.html">智能电子</a></li><li><a href="/comlist_31.html">汽车用品</a></li><li><a href="/comlist_32.html">个护化妆</a></li>
                        <li><a href="/comlist_33.html">母婴玩具</a></li><li><a href="/comlist_34.html">运动户外</a></li><li><a href="/comlist_35.html">营养保健</a></li>
                    </ul>
                    <div class="title">主题频道</div>
                    <ul class="name">
                    	<li><a href="/compnew.html">最新入驻</a></li><li><a href="/comphot.html">精品品牌</a></li><li><a href="/goodshotlist.html">优质货源</a></li><li><a href="/news_1.html">新闻资讯</a></li>
                    </ul>
                    <div class="title">热门专题</div>
                    <ul class="name">
                    	<li><a href="/subject/June/index.aspx" target="_blank" class="red">六月,年中惠报</a></li>
                        <li><a href="/subject/apr/index.aspx" target="_blank">“愚”你同行</a></li>
                        <li><a href="/subject/January/index.html" target="_blank">聚好商供好货</a></li>
                        <li><a href="/subject/november/index.html" target="_blank" >保暖季御寒新风尚</a></li>
                    	<li><a href="/subject/oct/index.html" target="_blank">备战双十一</a></li>
                    	<%--<li><a href="/subject/september/index.html" target="_blank">金秋9月好货来袭</a></li>--%>
                    </ul>
                    <div class="title">帮助中心</div>
                    <ul class="name">
                    	<li><a href="/guide.html">入市指南</a></li><li><a href="/help/help_6.html">卖家入门</a></li><li><a href="/help/help_7.html">卖家进阶</a></li><li><a href="/help/help_8.html">买家入门</a></li><li><a href="/help/help_9.html">买家进阶</a></li>
                        <li><a href="/help/help_10.html">常见问题</a></li>
                    </ul>
                    <div class="title">更多热点</div>
                    <ul class="name">
                    	<li><a href="http://www.moreyou.cn/" target="_blank">医伴金服</a></li>
                        <li><a href="http://www.yibanmed.com/" target="_blank">医站通</a></li>
                        <li><a href="http://www.b2b1818.com/" target="_blank">陌远易家</a></li>
                        <li><a href="http://www.fi1818.com/" target="_blank">陌远易融</a></li>
                    </ul>
				</div>
            </li>
			<li class="line" style="display:none;"><em>|</em></li> 
            	<li class="fore1"><a href="<%=Common.GetWebConfigKey("WebDomainName")  %>">医站通首页</a></i>
               
            </li>
        </ul>
    </div>
</div>
<!--顶部导航栏 end-->
