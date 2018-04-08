<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" CodeFile="guide2.aspx.cs" Inherits="guide2" %>

<%@ Register Src="~/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<%@ Register Src="~/UserControl/BottomControl.ascx" TagPrefix="uc1" TagName="bottom" %>
<%@ Register Src="~/UserControl/SecTopControl.ascx" TagPrefix="uc1" TagName="TopSec" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="X-UA-Compatible" content="IE=edge">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>入市指南-卖家版、买家版-医站通</title>
    <meta name="keywords" content="B2B电子商务,在线订货,手机订货,订货app,电商平台,分销系统,代理商管理,订货系统,管理系统,在线订单管理,网上订货系统,在线订货平台,订货软件,代理商系统" />
    <meta name="description" content="医站通-B2B电子商务平台是为贸易或生产企业开发的网上订单管理系统,实现厂商与代理商之间实时订货,付款,发货,收货,库存管理,收付款对帐管理,物流信息查询,安全的在线支付,在线客服等全面高效的订货流程管理,提升企业管理竞争力,分销、批发就上医站通.咨询热线:40077-40088" />
    <link href="css/global-2.0.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <script src="Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" >
        $(document).ready(function () {
            $("li a", ".guidePic").on("click", function () {
                $("li", ".guidePic").removeAttr("class");
                $(this).parent().addClass("hover");
                var index = $(this).parent().index();
                $("div.wrap").children().addClass("none").eq(index).removeClass("none");
            })
        })
    </script>
    <%--<script>
        var _hmt = _hmt || [];
        (function () {
            var hm = document.createElement("script");
            hm.src = "https://hm.baidu.com/hm.js?779e9b3d086d94ec0ead28ec3dd99190";
            var s = document.getElementsByTagName("script")[0];
            s.parentNode.insertBefore(hm, s);
        })();
    </script>--%>
</head>
<body>
    <form id="form1" runat="server">
      <!--顶部导航栏 start-->
        <uc1:Top ID="top1" runat="server" />
    <!--顶部导航栏 end-->

       <!--页头 start-->
       <uc1:TopSec ID="top2" runat="server" />
        <!--页头 end-->
   
        <div class="guideTitle"><div class="nr">
	        <div class="menu"><a href="guide.html">卖家版</a><a href="guide2.html" class="hover">买家版</a></div>
        </div></div>
		<div class="guide-ad"><img src="images/guide-ad.jpg" alt="暂无图片"/></div>

        <!--步骤流程图 start-->
        <div class="guidePic hxli">
	        <ul class="list">
				<li class="hover"><a href="javascript:;" class="bg"><i class="gp-i1"></i></a><a href="" class="t">1.注 册</a></li>
    	        <li><a href="javascript:;" class="bg"><i class="gp-i9"></i></a><a href="" class="t">2.找商机</a></li>
                <li><a href="javascript:;" class="bg"><i class="gp-i10"></i></a><a href="" class="t">3.申请合作</a></li>
                <li><a href="javascript:;" class="bg"><i class="gp-i2"></i></a><a href="" class="t">4.认 证</a></li>
                <li><a href="javascript:;" class="bg"><i class="gp-i11"></i></a><a href="" class="t">5.查看商品</a></li>
                <li><a href="javascript:;" class="bg"><i class="gp-i12"></i></a><a href="" class="t">6.下 单</a></li>
                <li><a href="javascript:;" class="bg"><i class="gp-i13"></i></a><a href="" class="t">7.付 款</a></li>
                <li><a href="javascript:;" class="bg"><i class="gp-i14"></i></a><a href="" class="t">8.收 货</a></li>
            </ul>
            <div class="line"><i class="dot"></i><i class="dot2"></i></div>
        </div>
        <!--步骤流程图 end-->


        <div class="wrap">
			<div class="guidenr1 ">
                <div class="guideCur ">注册<div class="line"></div></div>
                <div class="guidecont">
                    <p>第一步：输入想要查找的货源名称或者关键字</p><br />
                    <p>第二步：查看商品详细介绍</p><br />
                    <p>第三步：加盟该商品的厂商</p><br />
                    <p>第四步：购买商品</p>
                </div>
            </div>
            <div class="guidenr1 ">
                <div class="guideCur ">找货源<div class="line"></div></div>
                <div class="guidecont">
                    <p>第一步：输入想要查找的货源名称或者关键字</p><br />
                    <p>第二步：查看商品详细介绍</p><br />
                    <p>第三步：加盟该商品的厂商</p><br />
                    <p>第四步：购买商品</p>
                </div>
            </div>

            <div class="guidenr2 none">
                <div class="guideCur">加 盟<div class="line"></div></div>
                <div class="guidecont">
                    <p>
                        第一步：选择需要加盟的商户
                    </p>
                    <p>
                        <img src="help/image/%e7%bb%8f%e9%94%80%e5%95%86%e4%b8%93%e5%8c%ba/no1.png" width="930" height="415" alt="暂无图片" />
                    </p>
                    <p style="margin-top:10px;">
                        <img src="help/image/%e7%bb%8f%e9%94%80%e5%95%86%e4%b8%93%e5%8c%ba/no1_2.png" width="930" height="415" alt="暂无图片" />
                    </p>

                    <p>
                        第二步：根据提示完成第一部分注册
                    </p>
                    <p>
                        <img src="help/image/%e7%bb%8f%e9%94%80%e5%95%86%e4%b8%93%e5%8c%ba/no2.png" width="930" height="415" alt="暂无图片" />
                    </p>
                    <p style="margin-top:10px;">

                        <img src="help/image/%e7%bb%8f%e9%94%80%e5%95%86%e4%b8%93%e5%8c%ba/no2_1.png" width="930" height="415" alt="暂无图片" />
                    </p>

                    <p>
                        第三步：进行第二部分注册
                    </p>
                    <img src="help/image/%e7%bb%8f%e9%94%80%e5%95%86%e4%b8%93%e5%8c%ba/no3.png" width="930" height="415" alt="暂无图片" />
                    <p>

                    </p>
                    <p style="margin-top:10px;">

                        <img src="help/image/%e7%bb%8f%e9%94%80%e5%95%86%e4%b8%93%e5%8c%ba/no3.png" width="930" height="415" alt="暂无图片" />
                    </p>
                </div>
            </div>

            <div class="guidenr3 none">
                <div class="guideCur">认 证<div class="line"></div></div>
                <div class="guidecont">
                    <p>
                        第一步：注册成功后，等待系统管理员审核，通过后会有短信告知；审核通过后，输入注册的帐号密码进入商户界面
                    </p>
                    <p>
                        <img src="help/image/%e7%bb%8f%e9%94%80%e5%95%86%e4%b8%93%e5%8c%ba/no22_1_1.png" width="930" height="415" alt="暂无图片" />
                    </p>
                </div>
            </div>

            <div class="guidenr4 none">
                <div class="guideCur">查看商品<div class="line"></div></div>
                <div class="guidecont">
                    <p>
                        第一步：打开设置-商品列表
                    </p>
                    <p>
                        <img src="help/image/%e7%bb%8f%e9%94%80%e5%95%86%e4%b8%93%e5%8c%ba/no23_1_1.png" width="930" height="415" alt="暂无图片" />
                    </p>
                    <p>
                        <img src="help/image/%e7%bb%8f%e9%94%80%e5%95%86%e4%b8%93%e5%8c%ba/no23_1_2.png" width="930" height="415" alt="暂无图片" />
                    </p>
                    <p>
                        第二步：点击进入商品详细页面
                    </p>
                    <p>
                        <img src="help/image/%e7%bb%8f%e9%94%80%e5%95%86%e4%b8%93%e5%8c%ba/no23_2_1.png" width="930" height="415" alt="暂无图片" />

                    </p>
                    <p>
                        <img src="help/image/%e7%bb%8f%e9%94%80%e5%95%86%e4%b8%93%e5%8c%ba/no23_2_2.png" width="930" height="415" alt="暂无图片" />
                    </p>
                    <p>
                        <img src="help/image/%e7%bb%8f%e9%94%80%e5%95%86%e4%b8%93%e5%8c%ba/no23_2_3.png" width="930" height="415" alt="暂无图片" />
                    </p>
                </div>
            </div>

            <div class="guidenr5 none">
                <div class="guideCur">下 单<div class="line"></div></div>
                <div class="guidecont">
                    <p>
                        第一步：打开订货，点击新建订单
                    </p>
                    <p>
                        <img src="help/image/%e7%bb%8f%e9%94%80%e5%95%86%e4%b8%93%e5%8c%ba/no25_1.png" width="930" height="415" alt="暂无图片" />
                    </p>
                    <p>
                        第二步：输入订单信息
                    </p>
                    <p>
                        <img src="help/image/%e7%bb%8f%e9%94%80%e5%95%86%e4%b8%93%e5%8c%ba/no25_2.png" width="930" height="415" alt="暂无图片" />
                    </p><p>
                        第三步：选购商品
                    </p>
                    <p>
                        <img src="help/image/%e7%bb%8f%e9%94%80%e5%95%86%e4%b8%93%e5%8c%ba/no25_3.png" width="930" height="415" alt="暂无图片" />
                    </p><p>
                        第四步：新增地址界面
                    </p>
                    <p>
                        <img src="help/image/%e7%bb%8f%e9%94%80%e5%95%86%e4%b8%93%e5%8c%ba/no25_4.png" width="930" height="415" alt="暂无图片" />
                    </p><p>
                        第五步：提交订单
                    </p>
                    <p>
                        <img src="help/image/%e7%bb%8f%e9%94%80%e5%95%86%e4%b8%93%e5%8c%ba/no25_5.png" width="930" height="415" alt="暂无图片" />
                    </p>
                </div>
            </div>

            <div class="guidenr6 none">
                <div class="guideCur">付 款<div class="line"></div></div>
                <div class="guidecont">
                    <p>
                        第一步：打开支付-待支付订单
                    </p>
                    <p>
                        <img src="help/image/%e7%bb%8f%e9%94%80%e5%95%86%e4%b8%93%e5%8c%ba/no25_1_1.png" width="930" height="415" alt="暂无图片" />
                    </p>
                    <p>
                        第二步：查找需要支付的订单
                    </p>
                    <p>

                        <img src="help/image/%e7%bb%8f%e9%94%80%e5%95%86%e4%b8%93%e5%8c%ba/no25_2_1.png" width="930" height="415" alt="暂无图片" />
                    </p>
                    <p>
                        <img src="help/image/%e7%bb%8f%e9%94%80%e5%95%86%e4%b8%93%e5%8c%ba/no25_2_2.png" width="930" height="415" alt="暂无图片" />

                    </p>

                    <p>
                        第三步：订单支付
                    </p>
                    <p>
                        <img src="help/image/%e7%bb%8f%e9%94%80%e5%95%86%e4%b8%93%e5%8c%ba/no25_3_1.png" width="930" height="415" alt="暂无图片" />

                    </p>
                    <p>
                        第四步：选择支付方式（支付方式：企业钱包支付、快捷支付、网银支付、企业钱包+网银组合支付）
                    </p>
                    <p>
                        <img src="help/image/%e7%bb%8f%e9%94%80%e5%95%86%e4%b8%93%e5%8c%ba/no25_4_1.png" width="1000" alt="暂无图片" />


                    </p>
                    <p>
                        第五步：确认支付结果
                    </p>
                    <p>
                        <img src="help/image/%e7%bb%8f%e9%94%80%e5%95%86%e4%b8%93%e5%8c%ba/no25_5_1.png" width="1000" alt="暂无图片" />
                    </p>
                </div>
            </div>

            <div class="guidenr7 none">
                <div class="guideCur">收 货<div class="line"></div></div>
                <div class="guidecont">
                    <p>
                        第一步：打开收退货-我要收货
                    </p>
                    <p>
                        <img src="help/image/%e7%bb%8f%e9%94%80%e5%95%86%e4%b8%93%e5%8c%ba/no26_1_1.png" width="930" height="415" alt="暂无图片" />
                    </p>

                    <p>
                        第二步：查找需要收货的订单
                    </p>
                    <p>
                        <img src="help/image/%e7%bb%8f%e9%94%80%e5%95%86%e4%b8%93%e5%8c%ba/no26_2_1.png" width="930" height="415" alt="暂无图片" />
                    </p>
                    <p>
                        第三：进入订单详情中确认信息
                    </p>
                    <p>
                        <img src="help/image/%e7%bb%8f%e9%94%80%e5%95%86%e4%b8%93%e5%8c%ba/no26_3_1.png" width="930" height="415" alt="暂无图片" />
                    </p>
                    <p>
                        第四步：确认收货
                    </p>
                    <p>
                        <img src="help/image/%e7%bb%8f%e9%94%80%e5%95%86%e4%b8%93%e5%8c%ba/no26_4_1.png" width="930" height="415" alt="暂无图片" />
                    </p>
                    <p>
                        第五步：在订单列表中确认收货信息
                    </p>
                    <p>
                        <img src="help/image/%e7%bb%8f%e9%94%80%e5%95%86%e4%b8%93%e5%8c%ba/no26_5_1.png" width="930" height="415" alt="暂无图片" />
                    </p>
                </div>
            </div>


        </div>

       <uc1:bottom runat="server" ID="bottom" />
   </form>
</body>
</html>
