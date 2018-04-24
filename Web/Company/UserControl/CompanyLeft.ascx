<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CompanyLeft.ascx.cs" Inherits="Company_UserControl_CompanyLeft" %>
<!--左侧导航 start-->
<div class="sidebar responsive" data-sidebar="true" data-sidebar-scroll="true" data-sidebar-hover="true" id="sidebar">
    <ul class="nav nav-list">
         <li class="active">
            <a href="#" class="dropdown-toggle">
				<i class="menu-icon fa fa-truck"></i>
				<span class="menu-text">招商</span>
				<b class="arrow fa fa-angle-down"></b>
			</a>

			<b class="arrow"></b>
            <ul class="submenu nav-hide" style="display: none;">
				<li runat="server" class="" id="zslb" visible="false">
					<a href="<%=ResolveUrl("../CMerchants/CMerchantsList.aspx")%>">
						<i class="menu-icon fa fa-caret-right"></i>
						招商列表
					</a>
                    <b class="arrow"></b>
				</li>
                <li runat="server" id="sylb" visible="false">
                    <a href="<%=ResolveUrl("../CMerchants/FirstCampList.aspx")%>">
                        <i class="menu-icon fa fa-caret-right"></i>
                        首营列表
                    </a>
                </li>
                <li runat="server" id="dlssy" visible="false">
                    <a href="<%=ResolveUrl("../SysManager/DisFCmaterialsList.aspx")%>">
                        <i class="menu-icon fa fa-caret-right"></i>
                        代理商首营
                    </a>
                </li>
                <li runat="server" id="wdsy" visible="false">
                    <a href="<%=ResolveUrl("../SysManager/FCmaterialsInfo.aspx")%>">
                        <i class="menu-icon fa fa-caret-right"></i>
                        我的首营
                    </a>
                </li>
            </ul>
        </li>

        <li id="htHref" runat="server">
            <a href="#" class="dropdown-toggle">
				<i class="menu-icon fa fa-list-alt"></i>
				<span class="menu-text">合同</span>
				<b class="arrow fa fa-angle-down"></b>
			</a>

			<b class="arrow"></b>
            <ul class="submenu nav-hide" style="display: none;">
				<li runat="server" class="" id="htlb" visible="false">
					<a href="<%=ResolveUrl("../Contract/ContractList.aspx")%>">
						<i class="menu-icon fa fa-caret-right"></i>
						合同列表
					</a>
                    <b class="arrow"></b>
				</li>
            </ul>
        </li>


        <li id="goodsHref" runat="server">
            <a href="#" class="dropdown-toggle">
				<i class="menu-icon fa fa-shopping-bag"></i>
				<span class="menu-text">商品</span>
				<b class="arrow fa fa-angle-down"></b>
			</a>
            <b class="arrow"></b>
            <ul class="submenu nav-hide" style="display: none;">
				<li class="" runat="server" id="spxx">
                    <a href="#" class="dropdown-toggle">
						<i class="menu-icon fa fa-caret-right"></i>
				        <span class="menu-text">商品信息</span>
				        <b class="arrow fa fa-angle-down"></b>
			        </a>
                    <b class="arrow"></b>
                    <ul class="submenu nav-hide" style="display: none;">
                        <li class="" runat="server" visible="false" id="splb" >
                            <a href="<%=ResolveUrl("../GoodsNew/GoodsList.aspx")%>">
                                <i class="menu-icon fa fa-list green"></i>
                                商品列表
                            </a>
                            <b class="arrow"></b>
                        </li>
                        <li class="" runat="server" visible="false" id="spfl" >
                            <a href="<%=ResolveUrl("../GoodsNew/GType.aspx?type=2")%>">
                                <i class="menu-icon fa fa-book green"></i>
                                商品分类
                            </a>
                            <b class="arrow"></b>
                        </li>
                        <li class="" runat="server" visible="false" id="spsxwh">
                            <a href="<%=ResolveUrl("../GoodsNew/GoodsAttributelist.aspx")%>">
                                <i class="menu-icon fa fa-server green"></i>
                                商品属性维护
                            </a>
                            <b class="arrow"></b>
                        </li>
                        <li class="" runat="server" visible="false" id="spggmb">
                            <a href="<%=ResolveUrl("../GoodsNew/GoodsTemplatelist.aspx")%>">
                                <i class="menu-icon fa fa-leaf green"></i>
                                商品规格模板
                            </a>
                            <b class="arrow"></b>
                        </li>
                    </ul>
				</li>

                <li class="" runat="server" id="spwhgl">
                    <a href="#" class="dropdown-toggle">
				        <i class="menu-icon fa fa-caret-right"></i>
				        <span class="menu-text">库存管理</span>
				        <b class="arrow fa fa-angle-down"></b>
			        </a>
                    <b class="arrow"></b>
                    <ul class="submenu nav-hide" style="display: none;">
                        <li class="" runat="server" visible="false" id="spkc">
                            <a href="<%=ResolveUrl("../GoodsNew/GoodsInfoList.aspx")%>">
                                <i class="menu-icon fa fa-database green"></i>
                                商品库存
                            </a>
                            <b class="arrow"></b>
                        </li>
                        <li class="" runat="server" visible="false" id="sprk">
                            <a href="<%=ResolveUrl("../GoodsStock/StockInList.aspx?type=1")%>">
                                <i class="menu-icon fa fa-download green"></i>
                                商品入库
                            </a>
                            <b class="arrow"></b>
                        </li>
                        <li class="" runat="server" visible="false" id="spck">
                            <a href="<%=ResolveUrl("../GoodsStock/StockInList.aspx?type=2")%>">
                                <i class="menu-icon fa fa-upload green"></i>
                                商品出库
                            </a>
                            <b class="arrow"></b>
                        </li>
                        <li class="" runat="server" visible="false" id="sppd">
                            <a href="<%=ResolveUrl("../GoodsStock/InventoryList.aspx")%>">
                                <i class="menu-icon fa fa-keyboard green"></i>
                                库存盘点
                            </a>
                            <b class="arrow"></b>
                        </li>
                        <li class="" runat="server" visible="false" id="Dd3">
                            <a href="<%=ResolveUrl("../GoodsStock/OutStorageList.aspx")%>">
                                <i class="menu-icon fa fa-leaf green"></i>
                                出入库明细
                            </a>
                            <b class="arrow"></b>
                        </li>
                    </ul>
				</li>
                
                    <%--<dd runat="server" visible="false" id="jxsjg">
                        <a href="<%=ResolveUrl("../GoodsNew/DisPriceList.aspx")%>">代理商价格</a></dd>--%>
                    <%--<dd runat="server" visible="false" id="spbks">
                        <a href="<%=ResolveUrl("../GoodsNew/GoodsAreaList.aspx")%>">不可售设置</a></dd>--%>
                    <%--暂时屏蔽该功能 add by hgh  160921--%>
                    <%--<dd runat="server" visible="false"  id="djcx"><a href="<%=ResolveUrl("../PmtManager/PromotionList.aspx?type=0")%>">特价促销</a></dd>--%>

                <li class="" runat="server" id="spcxgl">
                    <a href="#" class="dropdown-toggle">
				        <i class="menu-icon fa fa-caret-right"></i>
				        <span class="menu-text">价格及促销</span>
				        <b class="arrow fa fa-angle-down"></b>
			        </a>
                    <b class="arrow"></b>
                    <ul class="submenu nav-hide" style="display: none;">
                        <li class="" runat="server" visible="false" id="spcx">
                            <a href="<%=ResolveUrl("../PmtManager/PromotionList.aspx?type=1")%>">
                                <i class="menu-icon fa fa-calculator green"></i>
                                商品促销
                            </a>
                            <b class="arrow"></b>
                        </li>
                        <li class="" runat="server" visible="false" id="ddcx">
                            <a href="<%=ResolveUrl("../PmtManager/PromotionList.aspx?type=2")%>">
                                <i class="menu-icon fa fa-cut green"></i>
                                订单促销
                            </a>
                            <b class="arrow"></b>
                        </li>
                    </ul>
				</li>
            </ul>
        </li>

        <li id="disHref" runat="server">
            <a href="#" class="dropdown-toggle">
				<i class="menu-icon fa fa-exchange"></i>
				<span class="menu-text">渠道</span>
				<b class="arrow fa fa-angle-down"></b>
			</a>

			<b class="arrow"></b>
            <ul class="submenu nav-hide" style="display: none;">
				<li runat="server" class="" id="jxsxx" visible="false">
					<a href="#" class="dropdown-toggle">
						<i class="menu-icon fa fa-caret-right"></i>
						代理商信息
                       <b class="arrow fa fa-angle-down"></b>
					</a>
                    <b class="arrow"></b>
                    <ul class="submenu nav-hide" style="display: none;">
				        <li runat="server" class="" id="jxslb" visible="false">
					        <a href="<%=ResolveUrl("../SysManager/DisList.aspx")%>">
						        代理商列表
					        </a>
                            <b class="arrow"></b>
				        </li>

                         <li runat="server" class="" id="jxssh" visible="false">
					        <a href="<%=ResolveUrl("../SysManager/DisAuditList.aspx")%>">
						        代理商审核
					        </a>
                            <b class="arrow"></b>
				        </li>

                         <li runat="server" class="" id="jxsgly" visible="false">
					        <a href="<%=ResolveUrl("../SysManager/DisUserList.aspx")%>">
						        管理员列表
					        </a>
                            <b class="arrow"></b>
				        </li>
                    </ul>
				</li>

                 <%--<dd runat="server" visible="false" id="dlssyzl">
                        <a href="<%=ResolveUrl("../SysManager/DisFCmaterialsList.aspx")%>">代理商首营资料</a></dd>--%>
                <li runat="server" class="" id="jsxsz" visible="false">
					<a href="#"  class="dropdown-toggle">
						<i class="menu-icon fa fa-caret-right"></i>
						代理商设置
                        <b class="arrow fa fa-angle-down"></b>
					</a>
                    <b class="arrow"></b>
                    <ul class="submenu nav-hide" style="display: none;">
				        <li runat="server" class="" id="jxsfl" visible="false">
					        <a href="<%=ResolveUrl("../SysManager/DisTypeList.aspx")%>">
						        代理商分类
					        </a>
                            <b class="arrow"></b>
				        </li>

                         <li runat="server" class="" id="jxsqy" visible="false">
					        <a href="<%=ResolveUrl("../SysManager/DisAreaList.aspx")%>">
						        代理商区域
					        </a>
                            <b class="arrow"></b>
				        </li>
                    </ul>
				</li>
            </ul>
        </li>

        <li id="ddglHref" runat="server">
            <a href="#" class="dropdown-toggle">
				<i class="menu-icon fa fa-shopping-cart"></i>
				<span class="menu-text">销售</span>
				<b class="arrow fa fa-angle-down"></b>
			</a>

			<b class="arrow"></b>
            <ul class="submenu nav-hide" style="display: none;">
				<li runat="server" class="" id="ddlb" visible="false">
					<a href="<%=ResolveUrl("../Order/OrderCreateList.aspx")%>">
						<i class="menu-icon fa fa-caret-right"></i>
						订单列表
					</a>
                    <b class="arrow"></b>
                </li>

                <li runat="server" class="" id="ddskmx" visible="false">
					<a href="<%=ResolveUrl("../Report/CompCollection.aspx")%>">
						<i class="menu-icon fa fa-caret-right"></i>
						订单收款明细
					</a>
                    <b class="arrow"></b>
                </li>
            </ul>
        </li>

        <li id="reportHref" runat="server">
            <a href="#" class="dropdown-toggle">
				<i class="menu-icon fa fa-file"></i>
				<span class="menu-text">报表</span>
				<b class="arrow fa fa-angle-down"></b>
			</a>

			<b class="arrow"></b>
            <ul class="submenu nav-hide" style="display: none;">
				<li runat="server" class="" id="Dd7">
					<a href="<%=ResolveUrl("../Report/RepSaleList.aspx")%>">
						<i class="menu-icon fa fa-caret-right"></i>
						销售报表
					</a>
                    <b class="arrow"></b>
                </li>

                <li runat="server" class="" id="Dd8">
					<a href="<%=ResolveUrl("../Report/Product_trace.aspx")%>">
						<i class="menu-icon fa fa-caret-right"></i>
						产品追溯图
					</a>
                    <b class="arrow"></b>
                </li>

                 <li runat="server" class="" id="Dd9">
					<a href="<%=ResolveUrl("../Report/RepQDList.aspx")%>">
						<i class="menu-icon fa fa-caret-right"></i>
						渠道报表
					</a>
                    <b class="arrow"></b>
                </li>

                <li runat="server" class="" id="ddtj" visible="false">
					<a href="<%=ResolveUrl("../Report/OrderRpt.aspx")%>">
						<i class="menu-icon fa fa-caret-right"></i>
						订单统计
					</a>
                    <b class="arrow"></b>
                </li>                

                <li runat="server" class="" id="jxstj" visible="false">
					<a href="<%=ResolveUrl("../Report/DisRpt.aspx")%>">
						<i class="menu-icon fa fa-caret-right"></i>
						代理商统计
					</a>
                    <b class="arrow"></b>
                </li>

                <li runat="server" class="" id="spxsmx" visible="false">
					<a href="<%=ResolveUrl("../Report/GodRet.aspx")%>">
						<i class="menu-icon fa fa-caret-right"></i>
						商品销售明细
					</a>
                    <b class="arrow"></b>
                </li>

                <li runat="server" class="" id="sptj" visible="false">
					<a href="<%=ResolveUrl("../Report/GoodsRpt.aspx")%>">
						<i class="menu-icon fa fa-caret-right"></i>
						商品统计
					</a>
                    <b class="arrow"></b>
                </li>

                <li runat="server" class="" id="zltj" visible="false">
					<a href="<%=ResolveUrl("../Report/AgingRpt.aspx")%>">
						<i class="menu-icon fa fa-caret-right"></i>
						账龄统计
					</a>
                    <b class="arrow"></b>
                </li>

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
            </ul>
        </li>

        <li id="sysHref" runat="server">
            <a href="#" class="dropdown-toggle">
				<i class="menu-icon fa fa-cog"></i>
				<span class="menu-text">设置</span>
				<b class="arrow fa fa-angle-down"></b>
			</a>

			<b class="arrow"></b>
            <ul class="submenu nav-hide" style="display: none;">
				<li runat="server" class="" id="jbsz" visible="false">
					<a href="#" class="dropdown-toggle">
						<i class="menu-icon fa fa-caret-right"></i>
						基本设置
                        <b class="arrow fa fa-angle-down"></b>
					</a>
                    <b class="arrow"></b>
                    <ul class="submenu nav-hide" style="display: none;">
				        <li runat="server" class="" id="skzhgl" visible="false">
					        <a href="<%=ResolveUrl("../Pay/PayAccountList.aspx")%>">
						        <i class="menu-icon fa fa-caret-right"></i>
						        收款帐号管理
					        </a>
                            <b class="arrow"></b>
				        </li>

                        <li runat="server" class="" id="gwqxwh" visible="false">
					        <a href="<%=ResolveUrl("../SysManager/RoleList.aspx")%>">
						        <i class="menu-icon fa fa-caret-right"></i>
						        岗位权限维护
					        </a>
                            <b class="arrow"></b>
				        </li>

                        <li runat="server" class="" id="ygzhwh" visible="false">
					        <a href="<%=ResolveUrl("../SysManager/UserList.aspx")%>">
						        <i class="menu-icon fa fa-caret-right"></i>
						        员工帐号维护
					        </a>
                            <b class="arrow"></b>
				        </li>

                        <li runat="server" class="" id="xgdlmm" visible="false">
					        <a href="<%=ResolveUrl("../ChangePwd.aspx")%>">
						        <i class="menu-icon fa fa-caret-right"></i>
						        修改登录密码
					        </a>
                            <b class="arrow"></b>
				        </li>

                        <li runat="server" class="" id="xtsz" visible="false">
					        <a href="<%=ResolveUrl("../SysManager/SystemSettings.aspx")%>">
						        <i class="menu-icon fa fa-caret-right"></i>
						        系统设置
					        </a>
                            <b class="arrow"></b>
				        </li>
                        <%--<dd runat="server" visible="false" id="gmfw">
                        <a href="<%=ResolveUrl("../SysManager/Service.aspx")%>">购买服务</a></dd>--%>
                    </ul>
				</li>

                <li runat="server" class="" id="xxwh" visible="false">
					<a href="#" class="dropdown-toggle">
						<i class="menu-icon fa fa-caret-right"></i>
						信息维护
                        <b class="arrow fa fa-angle-down"></b>
					</a>
                    <b class="arrow"></b>
                    <ul class="submenu nav-hide" style="display: none;">
				        <li runat="server" class="" id="xxfb" visible="false">
					        <a href="<%=ResolveUrl("../SysManager/NewsList.aspx")%>">
						        <i class="menu-icon fa fa-caret-right"></i>
						        信息发布
					        </a>
                            <b class="arrow"></b>
				        </li>

                        <li runat="server" class="" id="lyhf" visible="false">
					        <a href="<%=ResolveUrl("../SysManager/RepSuggestList.aspx")%>">
						        <i class="menu-icon fa fa-caret-right"></i>
						        留言回复
					        </a>
                            <b class="arrow"></b>
				        </li>

                        <li runat="server" class="" id="qyxx" visible="false">
					        <a href="<%=ResolveUrl("../SysManager/CompServiceEdit.aspx")%>">
						        <i class="menu-icon fa fa-caret-right"></i>
						        企业信息
					        </a>
                            <b class="arrow"></b>
				        </li>

                        <li runat="server" class="" id="xsywh" visible="false">
					        <a href="<%=ResolveUrl("../PmtManager/SaleManList.aspx")%>">
						        <i class="menu-icon fa fa-caret-right"></i>
						        销售员维护
					        </a>
                            <b class="arrow"></b>
				        </li>
                    </ul>
				</li>

                <li runat="server" class="" id="wddp" visible="false">
					<a href="#" class="dropdown-toggle">
						<i class="menu-icon fa fa-caret-right"></i>
						我的店铺
                        <b class="arrow fa fa-angle-down"></b>
					</a>
                    <b class="arrow"></b>
                    <ul class="submenu nav-hide" style="display: none;">
				        <li runat="server" class="" id="dpxx" visible="false">
					        <a href="<%=ResolveUrl("../SysManager/CompEdit.aspx?back=0")%>">
						        <i class="menu-icon fa fa-caret-right"></i>
						        店铺信息
					        </a>
                            <b class="arrow"></b>
				        </li>
                    <%--  <dd runat="server" visible="false"  id="ztsp"><a href="<%=ResolveUrl("../ShopManager/RecommendGoodsList.aspx")%>">主推商品</a></dd>
                    <dd runat="server" visible="false"  id="zttpwh"><a href="<%=ResolveUrl("../ShopManager/RecommendGoodsPic.aspx")%>">主推图片维护</a></dd>--%>
                        <li runat="server" class="" id="dpwh" visible="false">
					        <a href="<%=ResolveUrl("../ShopManager/ShopManager.aspx")%>" target="_blank">
						        <i class="menu-icon fa fa-caret-right"></i>
						        店铺装修
					        </a>
                            <b class="arrow"></b>
				        </li>

                        <li runat="server" class="" id="dply" visible="false">
					        <a href="<%=ResolveUrl("../ShopManager/ShopMessage.aspx")%>">
						        <i class="menu-icon fa fa-caret-right"></i>
						        店铺留言
					        </a>
                            <b class="arrow"></b>
				        </li>
                    </ul>
				</li>
            </ul>
        </li>

    </ul>
    <div class="sidebar-toggle sidebar-collapse" id="sidebar-collapse">
		<i id="sidebar-toggle-icon" class="ace-save-state ace-icon fa fa-angle-double-left" data-icon1="ace-icon fa fa-angle-double-left" data-icon2="ace-icon fa fa-angle-double-right"></i>
	</div>
</div>
<!--左侧导航 end-->

