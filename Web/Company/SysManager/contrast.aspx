<%@ Page Language="C#" AutoEventWireup="true" CodeFile="contrast.aspx.cs" Inherits="Company_SysManager_contrast" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="../../Distributor/newOrder/css/global2.5.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="layui-layer layui-anim layui-layer-iframe" style="width: 985px; height: 830px; margin:0 auto">
  <div class="layui-layer-title">版本对比功能区分</div>
  
  <div class="goods-zs record ">
    <div class="tabLine over" style="height:950px">
      <table border="0" cellspacing="0" cellpadding="0">
        <thead><tr><th class="t3">功能</th><th class="t3">订单内容</th><th class="t3">免费版</th><th class="t3">标准版</th></tr></thead>
        <tbody>
          <tr><td></td><td></td><td><div class="tc red">0</div></td><td><div class="tc red">4999/年</div></td></tr>
          <tr><td></td><td></td><td><div class="tc">5用户数</div></td><td><div class="tc">不限用户数</div></td></tr>
          <tr><td rowspan="6"><div class="tc">功能模板</div></td><td><div class="tle">订单管理</div></td><td><div class="tc f14 bule">√</div></td><td><div class="tc f14 bule">√</div></td></tr>
          <tr><td><div class="tle">收款管理</div></td><td><div class="tc f14 bule">√</div></td><td><div class="tc f14 bule">√</div></td></tr>
          <tr><td><div class="tle">商品信息</div></td><td><div class="tc f14 bule">√</div></td><td><div class="tc f14 bule">√</div></td></tr>
          <tr><td><div class="tle">库存管理</div></td><td><div class="tc f14 bule">√</div></td><td><div class="tc f14 bule">√</div></td></tr>
          <tr><td><div class="tle">代理商管理</div></td><td><div class="tc f14 bule">√</div></td><td><div class="tc f14 bule">√</div></td></tr>
          <tr><td><div class="tle">报表</div></td><td><div class="tc f14 bule">√</div></td><td><div class="tc f14 bule">√</div></td></tr>
          <tr><td rowspan="4"><div class="tc">订单扩展功能</div></td><td><div class="tle">代客下单</div></td><td><div class="tc f14 bule">√</div></td><td><div class="tc f14 bule">√</div></td></tr>
          <tr><td><div class="tle">订单核准</div></td><td><div class="tc f14 bule">√</div></td><td><div class="tc f14 bule">√</div></td></tr>
          <tr><td><div class="tle">订单发货</div></td><td><div class="tc f14 bule">√</div></td><td><div class="tc f14 bule">√</div></td></tr>
          <tr><td><div class="tle">在线支付</div></td><td><div class="tc f14 bule">√</div></td><td><div class="tc f14 bule">√</div></td></tr>
          <tr><td><div class="tc">供应商扩展功能</div></td><td><div class="tle">Excel导入</div></td><td><div class="tc f14 red">×</div></td><td><div class="tc f14 bule">√</div></td></tr>
          <tr><td rowspan="3"><div class="tc">收款扩展功能</div></td><td><div class="tle">在线支付</div></td><td><div class="tc f14 bule">√</div></td><td><div class="tc f14 bule">√</div></td></tr>
          <tr><td><div class="tle">企业钱包</div></td><td><div class="tc f14 red">×</div></td><td><div class="tc f14 bule">√</div></td></tr>
          <tr><td><div class="tle">账单收款</div></td><td><div class="tc f14 red">×</div></td><td><div class="tc f14 bule">√</div></td></tr>
          
          <tr><td rowspan="2"><div class="tc">商品扩展功能</div></td><td><div class="tle">Excel导入</div></td><td><div class="tc f14 red">×</div></td><td><div class="tc f14 bule">√</div></td></tr>
          <tr><td><div class="tle">价格及促销</div></td><td><div class="tc f14 red">×</div></td><td><div class="tc f14 bule">√</div></td></tr>
          <%--<tr><td><div class="tle">账单收款</div></td><td><div class="tc f14 red">×</div></td><td><div class="tc f14 bule">√</div></td></tr>--%>
          
          <tr><td rowspan="2"><div class="tc">报表扩展功能</div></td><td><div class="tle">统计报表</div></td><td><div class="tc f14 bule">√</div></td><td><div class="tc f14 bule">√</div></td></tr>
          <tr><td><div class="tle">数据分析</div></td><td><div class="tc f14 red">×</div></td><td><div class="tc f14 bule">√</div></td></tr>
        </tbody>
      </table>
    </div>
 </div>


</div>
    </form>
</body>
</html>
