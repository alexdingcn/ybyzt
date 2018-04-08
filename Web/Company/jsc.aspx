<%@ Page Language="C#" AutoEventWireup="true" CodeFile="jsc.aspx.cs" Inherits="Company_jsc" %>

<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>我的桌面（厂商后台）</title>
    <link href="css/style.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <link href="css/global-2.0.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/layer/layer.js" type="text/javascript"></script>
    <script src="../js/layerCommon.js" type="text/javascript"></script>
</head>
<body> 
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="" />    
    <div class="rightinfo">
        <div class="place" style="display:none;">
            <ul class="placeul">
                <li><a href="jsc.aspx">我的桌面</a></li>
            </ul>
        </div>
        <div class="blank10"></div>
        <!--简报 start-->
        <div class="briefing fl">
	        <div class="mh-title">简报</div>
            <div class="goods-ml">
		        <table border="0" cellspacing="0" cellpadding="0" class="tab-m">
                    <thead>
                        <tr>
                            <th class="t1">时间</th>
                            <th class="t2">订单</th>
                            <th class="t3">订单金额</th>
                            <th class="t3">订单收款</th>
                            <th class="t2">招商</th>
                        </tr>
                    </thead>
                    <tbody>
                    <tr>
                        <td><div class="tc">今天</div></td>
                        <td><div class="tc"><label id="DayOrderCount">0</label>笔</div></td>
                        <td><div class="tc">￥<label id="DaySum">0</label></div> </td>
                        <td><div class="tc">￥<label id="dayPaggerSum">0</label></div></td>
                        <td><div class="tc"><label id="DayCMCount">0</label>家</div></td>
                    </tr>
                    <tr>
                        <td><div class="tc">本周</div></td>
                        <td><div class="tc"><label id="WeekOrderCount">0</label>笔</div></td>
                        <td><div class="tc">￥<label id="WeekSum">0</label></div></td>
                        <td><div class="tc">￥<label id="WeekPaggerSum">0</label></div></td>
                        <td><div class="tc"><label id="WeekCMCount">0</label>家</div></td>
                    </tr>
                    <tr>
                        <td><div class="tc">本月</div></td>
                        <td><div class="tc"><label id="OrderCount">0</label>笔</div></td>
                        <td><div class="tc">￥<label id="MonthSum">0</label></div> </td>
                        <td><div class="tc">￥<label id="paggerSum">0</label></div></td>
                        <td><div class="tc"><label id="MonthCMCount">0</label>家</div></td>
                    </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <!--简报 end-->

        <div class="fr">
            <!--待处理 start-->
            <div class="wait-handle">
	            <div class="mh-title">待处理</div>
                <ul class="list">
                    <li class="a3"><a href="CMerchants/FirstCampList.aspx?type=1"><i class="thd-i"></i></a><i>合作待审：<a href="javascript:void(0)" style=" color:Red;" id="ReturnCount">0</a>笔</i></li>
                    <li class="a1"><a href="Order/OrderCreateList.aspx?type=1"><i class="dcl-i"></i></a><i>订单待审：<a href="javascript:void(0)" style=" color:Red;" id="NotCount">0</a>笔</i></li>
                    <li class="a2"><a href="Order/OrderCreateList.aspx?type=3"><i class="dfh-i"></i></a><i>待发货：<a href="javascript:void(0)" style=" color:Red;" id="DeliveryCount">0</a>笔</i></li>
                    <li class="a4"><a href="SysManager/DisFCmaterialsList.aspx"><i class="jm-i"></i></a><i>证件到期：<a href="javascript:void(0)" style=" color:Red;" id="disCount">0</a>家</i></li>
                </ul>
            </div>
            <!--待处理 end-->
            <div class="blank10"></div>
            <!--商品信息 start-->
            <div class="goods-mxx">
	            <div class="mh-title">商品信息<div style="float:right;margin-right:5px;">店铺未读留言：<a href="javascript:void(0)" id="shopmsgCount" style="text-decoration:underline; margin-right:3px;color:Red;">0</a>条</div></div>
                <div class="list" style=" height:53px;">
    	            <li><i class="ysj-i"></i>已上架商品：<a href="javascript:void(0)" style=" color:Red;" id="IsOffLineOk">0</a>件</li>
                    <li><i class="yxj-i"></i>已下架商品：<a href="javascript:void(0)" style=" color:Red;" id="IsOffLineNO">0</a>件</li>
                    <%--<li><i class="dsj-i"></i>促销商品数：<a href="javascript:void(0)" id="proCount">0</a>件</li>--%>
                </div>
            </div>
            <!--商品信息 end-->
        </div>

        <div class="blank10"></div>
        <!--分析 start-->
        <div class="analysis">
	        <div class="mh-title">
                分析
                <ul class="xx">
                    <li class="hover" tip="Month" style="cursor:pointer;"><i class="fx"></i>当月</li>
                    <li tip="YM" style="cursor:pointer;"><i class="fx"></i>半年</li>
                    <li tip="Y" style="cursor:pointer;"><i class="fx"></i>全年</li>
                </ul>
            </div>
            <div class="analysis-p" id="myChart">
    	        <%--<img src="images/analysis-p.jpg" />--%>
            </div>
        </div>
        <!--分析 end-->
    </div>
    <script src="js/echarts.min.js" type="text/javascript"></script>
    <script src="js/OrderCharts.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
    </form>
</body>
</html>
