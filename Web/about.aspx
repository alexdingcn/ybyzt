<%@ Page Language="C#" AutoEventWireup="true" CodeFile="about.aspx.cs" Inherits="about" EnableViewState="false" %>

<%@ Register Src="~/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<%@ Register Src="~/UserControl/SecTopControl.ascx" TagPrefix="uc1" TagName="TopSec" %>
<%@ Register Src="~/UserControl/BottomControl.ascx" TagPrefix="uc1" TagName="Bottom" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>关于我们 医站通</title>
    <meta name="keywords" content="医站通" />
    <meta name="description" content="我的医站通,中小企业网上分销平台,为您寻找产品批发分销渠道,提供订单管理、代理商管理、资金对账、数据分析等功能,建立企业网上分销管理平台" />
    <link href="css/global-2.0.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Company/js/jquery-1.11.1.min.js"></script>
    <style>
        .aboutNr
        {
            border-bottom: 1px dashed #ccc;
            border-left: 1px dashed #ccc;
            font-size: 14px;
            line-height: 30px;
            margin-left: 140px;
            padding-bottom: 30px;
            padding-left: 20px;
            position: relative;
        }
        .mianDiv
        {
            margin: 0 auto;
            width: 1200px;
        }
        
        .aboutNr .sidebar
        {
            background: #ddd none repeat scroll 0 0;
            color: #666;
            font-size: 14px;
            height: 26px;
            left: -140px;
            line-height: 26px;
            position: absolute;
            text-align: center;
            top: 30px;
            width: 110px;
        }
        .aboutNr dt
        {
            color: #494949;
            font-weight: bold;
            padding-top: 30px;
            position: relative;
        }
        .aboutNr dd
        {
            color: #797979;
        }
    </style>

    <script>
        $(function () {
            var url = '<%=Request["type"] %>';
            
            if (url == "1") {
                $("#licom").attr("class", "flbt hover");
                $("#licul").attr("class", "flbt");
                $("#liwe").attr("class", "flbt");

                $("#compIn").attr("class", "about");
                $("#cul").attr("class", "about none");
                $("#we").attr("class", "about none");
            } else if (url == "2") {
                $("#licom").attr("class", "flbt");
                $("#licul").attr("class", "flbt hover");
                $("#liwe").attr("class", "flbt");

                $("#compIn").attr("class", "about none");
                $("#cul").attr("class", "about");
                $("#we").attr("class", "about none");
            } else if (url == "3") {
                $("#licom").attr("class", "flbt");
                $("#licul").attr("class", "flbt");
                $("#liwe").attr("class", "flbt hover");

                $("#compIn").attr("class", "about none");
                $("#cul").attr("class", "about none");
                $("#we").attr("class", "about");
            }
        });
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
<body class="root">
    <form id="form1" runat="server">
    <uc1:Top ID="top1" runat="server" />
    <uc1:TopSec ID="top2" runat="server" />

    <div class="guideTitle">
        <div class="nr">
            <div class="menu">
                <a href="guide.htm" class="hover">关于我们</a></div>
        </div>
    </div>
    <div class="help-ad">
        <img src="images/about-ad.jpg" alt="关于我们" /></div>
    <div class="blank20"></div>

    <div class="wrap">
        <!--新闻左侧菜单 start-->
        <div class="help-sideBar fl">
            <div class="title">
                <h2>
                    关于我们</h2>
            </div>
            <ul class="menu new-menu">
                <div class="flbt"><a href="http://www.moreyou.cn/" target="_blank" class="t"><i class="arrw-i"></i>医伴金服</a></div>
                <li id="licom" class="flbt hover"><a href="about_1.html" class="t"><i class="arrw-i"></i>公司简介</a></li>
                <li id="licul" class="flbt"><a href="about_2.html" class="t"><i class="arrw-i"></i>企业文化</a></li>
                <li id="liwe" class="flbt"><a href="about_3.html" class="t"><i class="arrw-i"></i>联系我们</a></li>
            </ul>
        </div>
        <!--新闻左侧菜单 end-->
        <div class="help-ricon fr about-ri">
            <div class="about1 none">
                <div class="guideCur ">
                    医伴金服<div class="line">
                    </div>
                </div>
            </div>
            <div id="compIn" class="about">
                <div class="place">
                    <i href="" class="t">关于我们</i>><i href="" class="t">公司简介</i></div>
                <div class="nr">
                    <div class="fl" style="width: 600px;">
                        <p>
                            “医伴金服”是中国领先的B2B线上交易平台、供应链金融服务运营商，为入驻的厂商、供应商、代理商及终端用户提供供应链管理、B2B电子商务、物流、支付、金融等相关服务。</p>
                        <br />
                        <p>
                            “医伴金服”还将依托旗下陌远批发交易平台（医站通.com）、陌远大宗交易平台（b2b1818.com）、陌远供应链金融平台（fi1818.com）三大平台，发展B2B生态链，通过B2B生态链为平台上的企业提供优质物流、金融等相关服务，同时，为生态链上的伙伴创造共同服务客户的巨大商机。</p>
                    </div>
                    <div class="fr" style="margin-top: 25px;">
                        <img src="images/about-img.png" alt="暂无图片" /></div>
                </div>
            </div>
            <div id="cul" class="about none">
                <div class="place">
                    <i href="" class="t">关于我们</i>><i href="" class="t">企业文化</i></div>
                <div class="nr" style="padding: 50px 30px 100px 30px;">
                    <div class="fl" style="width: 380px;">
                        <img src="images/about-t.gif" alt="暂无图片" /></div>
                    <div class="fl">
                        <img src="images/about-img2.jpg" alt="暂无图片" /></div>
                </div>
            </div>
            <div id="we" class="about none">
                <div class="place">
                    <i href="" class="t">关于我们</i>><i href="" class="t">联系我们</i></div>
                <div class="nr" style="padding: 50px 30px 100px 30px;">
                    <div class="fl" style="width: 380px; background: url(images/mian2.5-i.png) no-repeat -192px -890px;
                        padding-left: 35px; line-height: 39px;">
                        <i>地址：上海市浦东新区耀华路488号信建大厦8楼、9楼</i><br />
                        <i>服务热线：40077-40088 </i>
                        <br />
                        <i>传真：021-50672081</i>
                    </div>
                    <div class="fl">
                        <img src="images/about-img3.jpg" alt="暂无图片" /></div>
                </div>
            </div>
        </div>
    </div>

    <div class="blank20"></div>

    <uc1:Bottom ID="Bottom1" runat="server" />
    <script src="js/layer/layer.js" type="text/javascript"></script>
    <script src="js/layerCommon.js" type="text/javascript"></script>
    </form>
</body>
</html>
