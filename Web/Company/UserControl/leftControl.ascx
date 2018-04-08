<%@ Control Language="C#" AutoEventWireup="true" CodeFile="leftControl.ascx.cs" Inherits="Company_UserControl_leftControl" %>
<script type="text/javascript">
    $(document).ready(function () {
        true && function () {
            var url = ".." + window.location.pathname;
            //var lastIndex = url.lastIndexOf("/");
            //url = url.substr(lastIndex + 1, url.length - lastIndex);
            //url = url.split("?")[0];
            var FindControl = $("li a[href='" + url + "']", ".leftsideBar #moveNav");
            FindControl.length > 0 ? FindControl.parents("li:eq(0)").addClass("cur") : function () {
                $.trim($("#<%=ShowidLeft.ClientID %>").val()) != "" && $("li." + $.trim($("#<%=ShowidLeft.ClientID %>").val()) + "", ".leftsideBar #moveNav").addClass("cur");
            } ();

            $.each($("#moveNav"), function (index, PrentControl) {
                $.each($("li .submenu", PrentControl), function (index, item) {
                    //菜单显示控制 start
                    item = $(item);
                    var ChildDD = item.find("dd");
                    var ChildDl = item.children("dl");
                    var length = ChildDD.length;
                    if (ChildDD.length == 0)
                    {
                        item.parent().addClass("n-lock"); item.remove()
                    }
                     else
                     {
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
                            Width = 98;//取不到值时
                        item.css("width", Width + "px");
                     }
                   //ChildDD.length == 0 ? function () { item.parent("li").addClass("stopmenu"); item.remove() }() : function () {
                   //     $.each(ChildDl, function (index, item1) {
                   //         item1 = $(item1);
                   //         item1.children("dd").length == 0 && item1.remove();
                   //     });
                   //     var Width = 10;
                   //     $.each(item.children("dl"), function (index, item2) {
                   //         item2 = $(item2);
                   //         Width += item2.outerWidth();
                   //     });
                   //     if (Width == 20)
                   //         Width = 98;//取不到值时
                   //     item.css("width", Width + "px");
                   // }();
                    //菜单显示控制 end

                });
            });
        } ();
    })
	
</script>
<script type="text/javascript" src="<%=ResolveUrl("../js/resolutionCom.js") %>"></script>
<!--左侧导航 start-->
<div class="leftsideBar ">
    <ul class="nav" id="moveNav">

        <li class="nav-2">
            <a href="javascript:;" id="zsHref" runat="server" class="a1"><span
            class="st-icon"></span><i class="bt">招商</i> </a>
            <div class="submenu wd">
                <dl>
                    <dd runat="server" id="zslb" visible="false">
                        <a href="<%=ResolveUrl("../CMerchants/CMerchantsList.aspx")%>">招商列表</a></dd>
                    <dd runat="server" id="sylb" visible="false">
                        <a href="<%=ResolveUrl("../CMerchants/FirstCampList.aspx")%>">首营列表</a></dd>
                    <dd runat="server" id="dlssy" visible="false">
                        <a href="<%=ResolveUrl("../SysManager/DisFCmaterialsList.aspx")%>">代理商首营</a></dd>
                    <dd runat="server" id="wdsy" visible="false">
                        <a href="<%=ResolveUrl("../SysManager/FCmaterialsInfo.aspx")%>">我的首营</a></dd>
                </dl>
            </div>
        </li>
        <li class="nav-8">
            <a href="javascript:;" id="htHref" runat="server" class="a1"><span
            class="dd-icon"></span><i class="bt">合同</i> </a>
            <div class="submenu wd">
                <dl>
                    <dd runat="server" id="htlb" visible="false">
                        <a href="<%=ResolveUrl("../Contract/ContractList.aspx")%>">合同列表</a></dd>
                     
                </dl>
            </div>
        </li>

        <li class="nav-3"><a href="javascript:;" id="goodsHref" runat="server" class="a1"><span
            class="sp-icon"></span><i class="bt">商品</i> </a>
            <div class="submenu wd">
                <dl>
                    <dt runat="server" id="spxx">商品信息</dt>
                    <dd runat="server" visible="false" id="splb">
                        <a href="<%=ResolveUrl("../GoodsNew/GoodsList.aspx")%>">商品列表</a></dd>
                    <dd runat="server" visible="false" id="spfl">
                        <a href="<%=ResolveUrl("../GoodsNew/GType.aspx?type=2")%>">商品分类</a></dd>
                    <dd runat="server" visible="false" id="spsxwh">
                        <a href="<%=ResolveUrl("../GoodsNew/GoodsAttributelist.aspx")%>">商品属性维护</a></dd>
                    <dd runat="server" visible="false" id="spggmb">
                        <a href="<%=ResolveUrl("../GoodsNew/GoodsTemplatelist.aspx")%>">商品规格模板</a></dd>
                </dl>
                <dl runat="server" visible="false" id="spwhgl">
                    <dt runat="server" id="spwh">库存管理</dt>
                    <dd runat="server" visible="false" id="spkc">
                        <a href="<%=ResolveUrl("../GoodsNew/GoodsInfoList.aspx")%>">商品库存</a></dd>
                    <dd runat="server" visible="false" id="sprk">
                        <a href="<%=ResolveUrl("../GoodsStock/StockInList.aspx?type=1")%>">商品入库</a></dd>
                    <dd runat="server" visible="false" id="spck">
                        <a href="<%=ResolveUrl("../GoodsStock/StockInList.aspx?type=2")%>">商品出库</a></dd>
                    <dd runat="server" visible="false" id="sppd">
                        <a href="<%=ResolveUrl("../GoodsStock/InventoryList.aspx")%>">库存盘点</a></dd>
                    <dd runat="server" visible="false" id="Dd3">
                        <a href="<%=ResolveUrl("../GoodsStock/OutStorageList.aspx")%>">出入库明细</a></dd>
                </dl>
                <dl>
                    <dt runat="server" id="spcxgl">价格及促销</dt>

                    <%--<dd runat="server" visible="false" id="jxsjg">
                        <a href="<%=ResolveUrl("../GoodsNew/DisPriceList.aspx")%>">代理商价格</a></dd>--%>
                    <%--<dd runat="server" visible="false" id="spbks">
                        <a href="<%=ResolveUrl("../GoodsNew/GoodsAreaList.aspx")%>">不可售设置</a></dd>--%>
                    <%--暂时屏蔽该功能 add by hgh  160921--%>
                    <%--<dd runat="server" visible="false"  id="djcx"><a href="<%=ResolveUrl("../PmtManager/PromotionList.aspx?type=0")%>">特价促销</a></dd>--%>
                    <dd runat="server" visible="false" id="spcx">
                        <a href="<%=ResolveUrl("../PmtManager/PromotionList.aspx?type=1")%>">商品促销</a></dd>
                    <dd runat="server" visible="false" id="ddcx">
                        <a href="<%=ResolveUrl("../PmtManager/PromotionList.aspx?type=2")%>">订单促销</a></dd>
                </dl>
            </div>
        </li>
        
        <li class="nav-4"><a href="javascript:;" id="disHref" runat="server" class="a1"><span
            class="kh-icon"></span><i class="bt">渠道</i> </a>
            <div class="submenu wd">
                <dl>
                    <dt runat="server" id="jxsxx">代理商信息</dt>
                    <dd runat="server" visible="false" id="jxslb">
                        <a href="<%=ResolveUrl("../SysManager/DisList.aspx")%>">代理商列表</a></dd>
                    <dd runat="server" visible="false" id="jxssh">
                        <a href="<%=ResolveUrl("../SysManager/DisAuditList.aspx")%>">代理商审核</a></dd>
                    <dd runat="server" visible="false" id="jxsgly">
                        <a href="<%=ResolveUrl("../SysManager/DisUserList.aspx")%>">管理员列表</a></dd>
                </dl>
                <dl>
                    <dt runat="server" id="jsxsz">代理商设置</dt>
                    <dd runat="server" visible="false" id="jxsfl">
                        <a href="<%=ResolveUrl("../SysManager/DisTypeList.aspx")%>">代理商分类</a></dd>
                    <dd runat="server" visible="false" id="jxsqy">
                        <a href="<%=ResolveUrl("../SysManager/DisAreaList.aspx")%>">代理商区域</a></dd>
                    <%--<dd runat="server" visible="false" id="dlssyzl">
                        <a href="<%=ResolveUrl("../SysManager/DisFCmaterialsList.aspx")%>">代理商首营资料</a></dd>--%>
                </dl>
            </div>
        </li>

        <li class="nav-1"><a href="javascript:;" id="ddglHref" runat="server" class="a1"><span
            class="dd-icon"></span><i class="bt">销售</i> </a>
            <div class="submenu">
                <dl id="ceshi">

                    <dd runat="server" visible="false" id="ddlb">
                        <a href="<%=ResolveUrl("../Order/OrderCreateList.aspx")%>">订单列表</a></dd>
                    <dd runat="server" visible="false" id="ddskmx">
                        <a href="<%=ResolveUrl("../Report/CompCollection.aspx")%>">订单收款明细</a></dd>
                    
                </dl>
            </div>
        </li>
        
        <li class="nav-5"><a href="javascript:;" id="reportHref" runat="server" class="a1"><span
            class="bb-icon"></span><i class="bt">报表</i> </a>
            <div class="submenu wd">
                <dl>
                    <dt runat="server" id="tjbb">统计报表</dt>

                    <dd runat="server" id="Dd7">
                        <a href="<%=ResolveUrl("../Report/RepSaleList.aspx")%>">销售报表</a></dd>
                    <dd runat="server" id="Dd8">
                        <a href="<%=ResolveUrl("../Report/Product_trace.aspx")%>">产品追溯图</a></dd>
                    <dd runat="server" id="Dd9">
                        <a href="<%=ResolveUrl("../Report/RepQDList.aspx")%>">渠道报表</a></dd>


                    <dd runat="server" visible="false" id="ddtj">
                        <a href="<%=ResolveUrl("../Report/OrderRpt.aspx")%>">订单统计</a></dd>
                    <dd runat="server" visible="false" id="jxstj">
                        <a href="<%=ResolveUrl("../Report/DisRpt.aspx")%>">代理商统计</a></dd>
                    <dd runat="server" visible="false" id="spxsmx">
                        <a href="<%=ResolveUrl("../Report/GodRet.aspx")%>">商品销售明细</a></dd>
                    <dd runat="server" visible="false" id="sptj">
                        <a href="<%=ResolveUrl("../Report/GoodsRpt.aspx")%>">商品统计</a></dd>
                    <dd runat="server" visible="false" id="zltj">
                        <a href="<%=ResolveUrl("../Report/AgingRpt.aspx")%>">账龄统计</a></dd>

                    <%--<dd runat="server" visible="false" id="jxsxssj" style="display:none">
                        <a href="<%=ResolveUrl("../Report/CustSaleRpt.aspx")%>">代理商销售数据</a></dd>
                    <dd runat="server" visible="false" id="spxssj" style="display:none">
                        <a href="<%=ResolveUrl("../Report/GoodsSaleRpt.aspx")%>">商品销售数据</a></dd>
                    <dd runat="server" visible="false" id="xssjbd" style="display:none">
                        <a href="<%=ResolveUrl("../Report/MonthSaleRpt.aspx")%>">销售数据月份变动</a></dd>
                    <dd runat="server" visible="false" id="jxsddys" style="display:none">
                        <a href="<%=ResolveUrl("../Report/ArrearageRpt.aspx")%>">代理商订单应收</a></dd>
                    <dd runat="server" visible="false" id="jxszdys" style="display:none">
                        <a href="<%=ResolveUrl("../Report/ArrearageRpt_ZD.aspx")%>">代理商账单应收</a></dd>
                    <dd runat="server" visible="false" id="sjfxbb" style="display:none">
                        <a href="<%=ResolveUrl("../Report/Reporttest.aspx")%>">数据分析报表</a></dd>--%>
                </dl>
                <%--<dl>
                    <dt runat="server" id="sjfx">数据分析</dt>
                    <dd runat="server" visible="false" id="zhfx">
                        <a href="<%=ResolveUrl("../Report/Report1.aspx")%>">综合分析</a></dd>
                              <dd runat="server" visible="false" id="ddfx">
                        <a href="<%=ResolveUrl("../Report/Report2.aspx")%>">订单分析</a></dd>
                              <dd runat="server" visible="false" id="jxsfx">
                        <a href="<%=ResolveUrl("../Report/Report3.aspx")%>">代理商分析</a></dd>
                              <dd runat="server" visible="false" id="cpfx">
                        <a href="<%=ResolveUrl("../Report/Report4.aspx")%>">产品分析</a></dd>
                              <dd runat="server" visible="false" id="cpabcfx">
                        <a href="<%=ResolveUrl("../Report/Report5.aspx")%>">产品ABC分析</a></dd>
                              <dd runat="server" visible="false" id="ysfx">
                        <a href="<%=ResolveUrl("../Report/Report6.aspx")%>">应收分析</a></dd>
                </dl>--%>
            </div>
        </li>
        <li class="nav-6"><a href="javascript:;" id="sysHref" runat="server" class="a1"><span
            class="sz-icon"></span><i class="bt">设置</i> </a>
            <div class="submenu wd">
                <dl>
                    <dt runat="server" id="jbsz">基本设置</dt>
                    <dd runat="server" visible="false" id="skzhgl">
                        <a href="<%=ResolveUrl("../Pay/PayAccountList.aspx")%>">收款帐号管理</a></dd>
                    <dd runat="server" visible="false" id="gwqxwh">
                        <a href="<%=ResolveUrl("../SysManager/RoleList.aspx")%>">岗位权限维护</a></dd>
                    <dd runat="server" visible="false" id="ygzhwh">
                        <a href="<%=ResolveUrl("../SysManager/UserList.aspx")%>">员工帐号维护</a></dd>
                    <dd runat="server" visible="false" id="xgdlmm">
                        <a href="<%=ResolveUrl("../ChangePwd.aspx")%>">修改登录密码</a></dd>
                    <dd runat="server" visible="false" id="xtsz">
                        <a href="<%=ResolveUrl("../SysManager/SystemSettings.aspx")%>">系统设置</a></dd>
                    <%--<dd runat="server" visible="false" id="gmfw">
                        <a href="<%=ResolveUrl("../SysManager/Service.aspx")%>">购买服务</a></dd>--%>
                </dl>
                <dl>
                    <dt runat="server" id="xxwh">信息维护</dt>
                    <dd runat="server" visible="false" id="xxfb">
                        <a href="<%=ResolveUrl("../SysManager/NewsList.aspx")%>">信息发布</a></dd>
                    <dd runat="server" visible="false" id="lyhf">
                        <a href="<%=ResolveUrl("../SysManager/RepSuggestList.aspx")%>">留言回复</a></dd>
                    <dd runat="server" visible="false" id="qyxx">
                        <a href="<%=ResolveUrl("../SysManager/CompServiceEdit.aspx")%>">企业信息</a></dd>
                    <dd runat="server" visible="false" id="xsywh">
                        <a href="<%=ResolveUrl("../PmtManager/SaleManList.aspx")%>">销售员维护</a></dd>
                    
                </dl>
                <dl>
                    <dt runat="server" id="wddp">我的店铺</dt>
                    <dd runat="server" visible="false" id="dpxx">
                        <a href="<%=ResolveUrl("../SysManager/CompEdit.aspx?back=0")%>">店铺信息</a></dd>
                    <%--                    <dd runat="server" visible="false"  id="ztsp"><a href="<%=ResolveUrl("../ShopManager/RecommendGoodsList.aspx")%>">主推商品</a></dd>
                    <dd runat="server" visible="false"  id="zttpwh"><a href="<%=ResolveUrl("../ShopManager/RecommendGoodsPic.aspx")%>">主推图片维护</a></dd>--%>
                    <dd runat="server" visible="false" id="dpwh">
                        <a href="<%=ResolveUrl("../ShopManager/ShopManager.aspx")%>" target="_blank">店铺装修</a></dd>
                           <dd runat="server" visible="false" id="dply">
                        <a href="<%=ResolveUrl("../ShopManager/ShopMessage.aspx")%>">店铺留言</a></dd>
                </dl>
            </div>
        </li>
        
    </ul>
</div>
<script type="text/javascript">
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

                tool.addClass(this, "hover");
            };
            li[i].onmouseout = function () {
                tool.removeClass(this, "hover");
            };
        }
    })();
	
</script>
<!--左侧导航 end-->
<input type="hidden" id="ShowidLeft" runat="server" />
