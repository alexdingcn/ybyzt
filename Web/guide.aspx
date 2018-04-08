<%@ Page Language="C#" AutoEventWireup="true" CodeFile="guide.aspx.cs" EnableViewState="false" Inherits="guide" %>

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
	    <div class="menu"><a href="guide.html" class="hover">卖家版</a><a href="guide2.html">买家版</a></div>
    </div></div>
    <div class="guide-ad"><img src="images/guide-ad.jpg" alt="暂无图片"/></div>
    
    <!--步骤流程图 start-->
    <div class="guidePic">
	    <ul class="list">
    	    <li class="hover"><a href="javascript:;" class="bg"><i class="gp-i1"></i></a><a href="" class="t">1.注 册</a></li>
            <li><a href="javascript:;" class="bg"><i class="gp-i2"></i></a><a href="" class="t">2.认 证</a></li>
            <li><a href="javascript:;" class="bg"><i class="gp-i3"></i></a><a href="" class="t">3.发布公司介绍</a></li>
            <li><a href="javascript:;" class="bg"><i class="gp-i4"></i></a><a href="" class="t">4.开通旺铺</a></li>
            <li><a href="javascript:;" class="bg"><i class="gp-i5"></i></a><a href="" class="t">5.发布供应信息</a></li>
            <li><a href="javascript:;" class="bg"><i class="gp-i6"></i></a><a href="" class="t">6.查看订单</a></li>
            <li><a href="javascript:;" class="bg"><i class="gp-i7"></i></a><a href="" class="t">7.发 货</a></li>
            <li><a href="javascript:;" class="bg"><i class="gp-i8"></i></a><a href="" class="t">8.收 款</a></li>
        </ul>
        <div class="line"><i class="dot"></i><i class="dot2"></i></div>
    </div>
    <!--步骤流程图 end-->

    <div class="wrap wid">
	    <div class="guidenr1 ">
            <div class="guideCur ">注 册<div class="line"></div></div>
            <div class="guidecont">
                <p>
                    第一步：注册厂商用户<br>
                    <img width="931" height="405" src="help/image/厂商专区/clip_image002.png" alt="暂无图片">
                </p>
                <p>
                    <br>
                    第二步：根据提示完成第一部分注册<br>
                    <img width="930" height="402" src="help/image/厂商专区/clip_image004.png" alt="暂无图片">
                </p>
                <p>
                    <br>
                    第三步：进行第二部分注册<br>
                    <img width="944" height="400" src="help/image/厂商专区/clip_image006.png" alt="暂无图片">
                </p>
                <p>
                    <br>
                    第四步：注册成功后，等待系统管理员审核，通过后会有短信告知；审核通过后，输入注册的帐号密码进入厂商界面
                </p>
            </div>	
        </div>
    
        <div class="guidenr2 none">
            <div class="guideCur">认 证<div class="line"></div></div>
            <div class="guidecont">
                <p>
                    第一步：注册成功后，等待系统管理员审核，通过后会有短信告知；审核通过后，输入注册的帐号密码进入厂商界面<br>
                    <img width="930" height="401" src="help/image/厂商专区/clip_image002_0000.png" alt="暂无图片">
                </p>
            </div>	
        </div>
    
         <div class="guidenr3 none">
            <div class="guideCur">发布公司介绍<div class="line"></div></div>
            <div class="guidecont">
                <p>第一步：打开设置-店铺信息</p>
                <p>
                    <img width="931" height="417" src="help/image/厂商专区/13_开通旺铺_clip_image002.png" alt="暂无图片"><br>
                    第二步：输入店铺信息
                </p>
                <img width="931" height="415" src="help/image/厂商专区/clip_6.png" alt="暂无图片">
                <p><img width="930" height="418" src="help/image/厂商专区/clip_7.png" alt="暂无图片"> </p>
                <p>第三步：确认店铺信息 </p>
                <p><img width="930" height="322" src="help/image/厂商专区/clip_8.png" alt="暂无图片">
                    <img width="930" height="218" src="help/image/厂商专区/clip_9.png" alt="暂无图片"></p>
                <p>&nbsp;</p>
            </div>	
        </div>
    
         <div class="guidenr4 none">
            <div class="guideCur">开通旺铺<div class="line"></div></div>
            <div class="guidecont">
                <p>第一步：打开设置-店铺装修</p>
                <p><img width="931" height="415" src="help/image/厂商专区/13_开通旺铺_clip_image001.png" alt="暂无图片"> </p>
                <p>&nbsp;</p>
                <p>第二步：装修广告页区域</p>
                <p><img width="930" height="397" src="help/image/厂商专区/13_开通旺铺_clip_image004.png" alt="暂无图片"></p>
                <p><img width="930" height="414" src="help/image/厂商专区/13_开通旺铺_clip_image006.png" alt="暂无图片"></p>
                <p><img width="930" height="392" src="help/image/厂商专区/13_开通旺铺_clip_image008.png" alt="暂无图片"> </p>
                <p><img width="930" height="390" src="help/image/厂商专区/13_开通旺铺_clip_image010.png" alt="暂无图片"></p>
                <p>
                    <br>
                    第三步：店铺推荐<br>
                    <img width="930" height="416" src="help/image/厂商专区/13_开通旺铺_clip_image012.png" alt="暂无图片">
                </p>
                <p><img width="930" height="420" src="help/image/厂商专区/13_开通旺铺_clip_image014.png" alt="暂无图片"></p>
                <p>
                    <br>
                    第四步：商品区装修<br>
                    <img width="930" height="416" src="help/image/厂商专区/13_开通旺铺_clip_image016.png" alt="暂无图片">
                </p>
                <p><img width="930" height="418" src="help/image/厂商专区/13_开通旺铺_clip_image018.png" alt="暂无图片"></p>
                <p>
                    <br>
                    第五步：联系方式<br>
                    <img width="930" height="416" src="help/image/厂商专区/13_开通旺铺_clip_image020.png" alt="暂无图片">
                </p>
                <p><img width="930" height="415" src="help/image/厂商专区/13_开通旺铺_clip_image022.png" alt="暂无图片"></p>
            </div>	
        </div>
    
         <div class="guidenr5 none">
            <div class="guideCur">发布供应信息<div class="line"></div></div>
            <div class="guidecont">
                <p>第一步：打开商品-商品列表</p>
                <p><img width="931" height="415" src="help/image/厂商专区/clip_image002_0003.png" alt="暂无图片"></p>
                <p>
                    <br>
                    第二步：新增商品分类
                </p>
                <p><img width="931" height="414" src="help/image/厂商专区/clip_image004_0002.png" alt="暂无图片"></p>
                <p>
                    <br>
                    <img width="930" height="421" src="help/image/厂商专区/clip_image006_0002.png" alt="暂无图片">
                </p>
                <p>
                    <br>
                    第三步：新增商品
                </p>
                <p><img width="930" height="413" src="help/image/厂商专区/clip_image008_0001.png" alt="暂无图片"></p>
                <p>
                    <br>
                    <img width="930" height="414" src="help/image/厂商专区/clip_image010_0001.png" alt="暂无图片">
                </p>
                <p><img width="930" height="420" src="help/image/厂商专区/clip_image012_0000.png" alt="暂无图片"></p>
            </div>	
        </div>
    
         <div class="guidenr6 none">
            <div class="guideCur">查看订单<div class="line"></div></div>
            <div class="guidecont">
                <p>第一步：打开订单-订单列表</p>
                <p>
                    <br>
                    <img width="930" height="416" src="help/image/厂商专区/clip_image00215.png" alt="暂无图片">
                </p>
                <p>
                    <br>
                    <img width="931" height="414" src="help/image/厂商专区/clip_image00415.png" alt="暂无图片">
                </p>
                <p>&nbsp;</p>
                <p><img width="931" height="415" src="help/image/厂商专区/clip_image00615.png" alt="暂无图片"></p>
                <p>&nbsp;</p>
            </div>	
        </div>
    
         <div class="guidenr7 none">
            <div class="guideCur">发 货<div class="line"></div></div>
            <div class="guidecont">
                <p>第一步：打开订单-订单发货</p>
                <p><img width="930" height="419" src="help/image/厂商专区/clip_image002_000016.png" alt="暂无图片"></p>
                <p>
                    <br>
                    第二步：发货
                </p>
                <p>
                    <br>
                    <img width="930" height="416" src="help/image/厂商专区/clip_image006_000016.png" alt="暂无图片">
                </p>
                <p>
                    <br>
                    第三步：维护物流信息
                </p>
                <p>
                    <br>
                    <img width="930" height="416" src="help/image/厂商专区/clip_image008_000016.png" alt="暂无图片">
                </p>
                <p><img width="930" height="658" src="help/image/厂商专区/clip_image010_000016.png" alt="暂无图片">  </p>
                <p>&nbsp;</p>
                <p>第四步：确认发货信息 </p>
                <p><img width="931" height="416" src="help/image/厂商专区/clip_image012_000016.png" alt="暂无图片"></p>
                <p>&nbsp;</p>
            </div>	
        </div>
    
         <div class="guidenr8 none">
            <div class="guideCur">收 款<div class="line"></div></div>
            <div class="guidecont">
            	<p>普通支付：<br>
                  代理商付款后T+1天到达厂商账户<br><br>
                  担保支付：<br>
                  代理商付款并且确认收货以后，T+1天到达厂商账户</p>
            </div>	
        </div>
    </div>





   <uc1:bottom runat="server" ID="bottom" />
   </form>
</body>
</html>
