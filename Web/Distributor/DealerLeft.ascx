<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DealerLeft.ascx.cs" Inherits="Distributor_DealerLeft" %>
<script type="text/javascript">
    function skip() {
        location.href = '<%=ResolveUrl("UserIndex.aspx")%>';
    }
    $(document).ready(function () {
        true && function () {
            var url = window.location.href;
            var lastIndex = url.lastIndexOf("/");
            url = url.substr(lastIndex + 1, url.length - lastIndex);
            url = url.split("?")[0];
            var FindControl = $("li a[href*='" + url + "']", ".leftsideBar #moveNav");
            FindControl.length > 0 ? FindControl.parents("li:eq(0)").addClass("cur") : function () {
                $.trim($("#<%=Showid.ClientID %>").val()) != "" && $("li." + $.trim($("#<%=Showid.ClientID %>").val()) + "", ".leftsideBar #moveNav").addClass("cur");
            } ();


            $.each($("#moveNav"), function (index, PrentControl) {
                $.each($("li .submenu", PrentControl), function (index, item) {
                    //菜单显示控制 start
                    item = $(item);
                    var ChildDD = item.find("dd");
                    var ChildDl = item.children("dl");
                    ChildDD.length == 0 ?
                        function () {
                            if ('<%=master %>' == '0') {
                                item.parent("").addClass("stopmenu");
                                item.remove();
                            }
                        } () : 
                        function () {
                        $.each(ChildDl, function (index, item1) {
                            item1 = $(item1);
                            item1.children("dd").length == 0 && item1.remove();
                        });
                        var Width = 10;
                        $.each(item.children("dl"), function (index, item2) {
                            item2 = $(item2);
                            Width += item2.outerWidth();
                        });
                        if (Width == 20)
                            Width = 98; //取不到值时
                        item.css("width", Width + "px");
                    } ();
                    //菜单显示控制 end

                });
            });

        } ();
    })
   

</script>


<div class="leftsideBar">
    <ul class="nav" id="moveNav">
        
        <li class="nav-7">
            <a href="javascript:;" id="Dis26" runat="server" class="a1"><span
            class="st-icon"></span><i class="bt">招商</i> </a>
            <div class="submenu wd">
                <dl>
                    <dd runat="server" id="Dis2610" visible="false">
                        <a href="<%=ResolveUrl("CMerchants/CMerchantsList.aspx")%>">招商列表</a></dd>
                    <dd runat="server" id="Dis2612" visible="false">
                        <a href="<%=ResolveUrl("CMerchants/FirstCampList.aspx")%>">首营列表</a></dd>
                    <dd runat="server" id="Dis2615" visible="false">
                        <a href="<%=ResolveUrl("FCmaterials/FCmaterialsInfo.aspx")%>">我的首营资料</a></dd>
                    <dd runat="server" id="Dis2617" visible="false">
                        <a href="<%=ResolveUrl("FCmaterials/CompFCmateriarialsList.aspx")%>">厂商首营资料</a></dd>
                </dl>
            </div>
        </li>
        
    	<li class="nav-8">
    	    <a href='javascript:;' class="a1" id="Dis27" runat="server">
    	        <span class="dd-icon"></span><i class="bt">合同</i>
    	    </a>
             <div class="submenu" style="visibility: hidden;"><dl><dt></dt></dl></div>
            <%--<div class="submenu" style="visibility: hidden;">
                <dl>
        	       <dd runat="server" id="Dd3">
                        <a href="<%=ResolveUrl("Contract/ContractList.aspx")%>">合同列表</a></dd>
                    
                </dl>
            </div>--%>
    	</li>

    	<li class="nav-1">
    	    <a href='javascript:;' class="a1" id="Dis20" runat="server">
    	        <span class="sz-icon"></span><i class="bt">采购</i>
    	    </a>
            <div class="submenu" style="visibility: hidden;"><dl><dt></dt></dl></div>
    	</li>

    	<li class="nav-2">
    	    <a class="a1" id="Dis21" href="javascript:;" runat="server"> 
    	        <span class="sp-icon"></span><i class="bt">商品</i>
    	    </a>
            <div class="submenu">
                <dl><dt></dt>
        	        <dd id="Dis2110" visible="false" runat="server"><a href='<%=ResolveUrl("GoodsList.aspx")%>'>商品列表</a></dd>
                    <dd id="Dis2113" visible="false" runat="server"><a href='<%=ResolveUrl("GoodsList.aspx?sc=sc")%>'>收藏商品</a></dd>
                </dl>
            </div>
    	</li>
       
        <li class="nav-3" >
            <a href="javascript:;" class="a1" id="Dis22" runat="server">
                <span class="zf-icon"></span><i class="bt">支付</i>
            </a>
            <div class="submenu wd">
        	    <dl><dt>订单支付</dt>
                    <dd id="Dis2210" visible="false" runat="server"><a href='<%=ResolveUrl("pay/orderPayList.aspx")%>' >待支付订单</a></dd>
                    <dd id="Dis2211" visible="false" runat="server"><a href='<%=ResolveUrl("Rep/RepDetailsList.aspx")%>'>订单支付明细</a></dd>
                </dl>

            </div>
        </li>

        <li class="nav-4">
            <a href="javascript:;" class="a1" id="Dis28" runat="server">
                <span class="st-icon"></span><i class="bt">库存</i>
            </a>
            <div class="submenu wid">
        	<dl><dt></dt>
                <dd id="Dis2810" visible="false" runat="server"><a href='<%=ResolveUrl("Storage/GoodsStorageList.aspx")%>' >商品库存</a></dd>
                <dd id="Dis2812" visible="false" runat="server"><a href='<%=ResolveUrl("Storage/GoodsStockList.aspx")%>' >商品批次库存</a></dd>
                <dd id="Dis2811" visible="false" runat="server"><a href='<%=ResolveUrl("Storage/StorageList.aspx")%>' >入库单</a></dd>
                <dd id="Dis2814" visible="false" runat="server"><a href='<%=ResolveUrl("Storage/LibraryList.aspx")%>' >出库单</a></dd>
                <dd id="Dis2817" visible="false"  runat="server"><a href='<%=ResolveUrl("Payment/PaymentList.aspx")%>'>收款单</a></dd>
            </dl>
            </div>
        </li>

        <li class="nav-5">
            <a href="javascript:;" class="a1" id="Dis24" runat="server">
                <span class="bb-icon"></span><i class="bt">报表</i>
            </a><div class="submenu">
        	<dl><dt></dt>
            
                <%--<dd id="Dd8" visible="false"  runat="server"><a href='<%=ResolveUrl("Rep/RepStorageList.aspx")%>'>采购报表</a></dd>
                <dd id="Dd7" visible="false" runat="server"><a href='<%=ResolveUrl("Rep/RepLibraryList.aspx")%>'>销售报表</a></dd>
                <dd id="Dd9" visible="false" runat="server"><a href='<%=ResolveUrl("Rep/RepPaymentList.aspx")%>'>应收账款</a></dd>--%>
                
               <%-- <dd id="Dis2410" visible="false" runat="server"><a href='<%=ResolveUrl("Rep/RepOrderList.aspx")%>'>进货订单明细</a></dd>
                <dd id="Dis2411" visible="false" runat="server"><a href='<%=ResolveUrl("Rep/RepGoodsList.aspx")%>'>商品进货数据</a></dd>
                <dd id="Dis2412" visible="false" runat="server"><a href='<%=ResolveUrl("Rep/RepMonthList.aspx")%>'>进货数据月份变动</a></dd>--%>
                <dd id="Dis2413" visible="true" runat="server"><a href='<%=ResolveUrl("Rep/CMerchantsOrder.aspx")%>'>采购报表</a></dd>
                <%--<dd id="Dis2414" visible="true" runat="server"><a href='<%=ResolveUrl("Rep/RepGoodsStockList.aspx")%>'>库存报表</a></dd>--%>
                <dd id="Dis2415" visible="true" runat="server"><a href='<%=ResolveUrl("Rep/RepLibraryList.aspx")%>'>销售报表</a></dd>
                <dd id="Dis2416" visible="true" runat="server"><a href='<%=ResolveUrl("Rep/RepPaymentList.aspx")%>'>应收账款</a></dd>
            </dl>
        </div>
        </li>

        <li class="nav-6">
            <a href="javascript:;" class="a1" id="Dis25" runat="server">
               <span class="sz-icon"></span><i class="bt">设置</i>
            </a>
            <div class="submenu">
        	<dl><dt></dt>
                <dd id="Dis2511" visible="false" runat="server"><a href='<%=ResolveUrl("DeliveryList.aspx")%>'>收货地址维护</a></dd>
                <dd id="Dis2512" visible="false" runat="server"><a href='<%=ResolveUrl("pay/PayQuickly.aspx")%>'>修改快捷银行卡</a></dd>
                <dd id="Dis2513" visible="false" runat="server"><a href='<%=ResolveUrl("PhoneEdit.aspx")%>'>修改绑定手机</a></dd>
                <dd id="Dis2514" visible="false" runat="server"><a href='<%=ResolveUrl("RoleList.aspx")%>'>设置岗位权限</a></dd>
                <dd id="Dis2515" visible="false" runat="server"><a href='<%=ResolveUrl("UsersList.aspx") %>'>员工帐号设置</a></dd>
                <dd id="Dis2516" visible="false" runat="server"><a href='<%=ResolveUrl("PayPWDEdit.aspx") %>'>修改支付密码</a></dd>
                <dd id="Dis2517"  runat="server"><a href='<%=ResolveUrl("UserEdit.aspx") %>'>基本信息维护</a></dd>
                <dd id="Dis9999"  runat="server"><a href='<%=ResolveUrl("FCmaterials/FCmaterialsInfo.aspx") %>'>我的首营信息</a></dd>
            </dl>
            </div>
        </li>
        
    </ul>
</div><script type="text/javascript">
// 左侧导航-鼠标移上去添加class
var tool = {
    $: function (id) {
        return typeof id === "object" ? id : document.getElementById(id)
    },
    $$: function (tagName, oParent) {
        return (oParent || document).getElementsByTagName(tagName)
    },
    $$$: function (className, elem, tagName) {
        var i = 0,
			  aClass = [],
			  reClass = new RegExp("(^|\\s)" + className + "(\\s|$)"),
			  aElement = tool.$$(tagName || "*", elem || document);
        for (i = 0; i < aElement.length; i++)
            reClass.test(aElement[i].className) && aClass.push(aElement[i]);
        return aClass
    },
    addClass: function (elem, value) {
        !elem.className ? elem.className = value : elem.className += " " + value
    },
    removeClass: function (elem, className) {
        var reg = new RegExp('(\\s|^)' + className + '(\\s|$)');
        var ematch = elem.className.match(reg);
        if (ematch) elem.className = elem.className.replace(reg, ' ')
    }
};
(function () {
    var moveNav = tool.$("moveNav");
    var li = tool.$$("li", moveNav);

    for (var i = 0; i < li.length; i++) {
        li[i].onmouseover = function () {
//            tool.addClass(this, "hover");
            $(this).addClass("hover");
        };
        li[i].onmouseout = function () {
//            tool.removeClass(this, "hover");
            $(this).removeClass("hover");
        };
    }
})();
	
</script>
<input type="hidden" id="Showid" runat="server"  />