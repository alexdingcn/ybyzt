<%@ Page Language="C#" AutoEventWireup="true" CodeFile="statement.aspx.cs" Inherits="statement" %>

<%@ Register Src="~/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<%@ Register Src="~/UserControl/SecTopControl.ascx" TagPrefix="uc1" TagName="TopSec" %>
<%@ Register Src="~/UserControl/BottomControl.ascx" TagPrefix="uc1" TagName="Bottom" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>网站申明 医站通</title>
    <meta name="keywords" content="我的医站通网上分销_电子商务_B2B_批发_采购" />
    <meta name="description" content="我的医站通网,中小企业网上分销平台,为您寻找产品批发分销渠道,提供订单管理、代理商管理、资金对账、数据分析等功能,建立企业网上分销管理平台" />
    <link href="css/global-2.0.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Company/js/jquery-1.11.1.min.js"></script>

    <script>
        $(function () {
            var url = '<%=Request["type"] %>';
            
            if (url == "1") {
                $("#liRight").attr("class", "flbt hover");
                $("#liLia").attr("class", "flbt");
                $("#liSe").attr("class", "flbt");
                $("#lilink").attr("class", "flbt");

                $("#divRight").attr("class", "about");
                $("#divLia").attr("class", "about none");
                $("#divSe").attr("class", "about none");
                $("#divlink").attr("class", "about none");
            } else if (url == "2") {
                $("#liRight").attr("class", "flbt");
                $("#liLia").attr("class", "flbt hover");
                $("#liSe").attr("class", "flbt");
                $("#lilink").attr("class", "flbt");

                $("#divRight").attr("class", "about none");
                $("#divLia").attr("class", "about");
                $("#divSe").attr("class", "about none");
                $("#divlink").attr("class", "about none");
            } else if (url == "3") {
                $("#liRight").attr("class", "flbt");
                $("#liLia").attr("class", "flbt");
                $("#liSe").attr("class", "flbt hover");
                $("#lilink").attr("class", "flbt");

                $("#divRight").attr("class", "about none");
                $("#divLia").attr("class", "about none");
                $("#divSe").attr("class", "about");
                $("#divlink").attr("class", "about none");
            } else if (url=="4") {
                $("#liRight").attr("class", "flbt");
                $("#liLia").attr("class", "flbt");
                $("#liSe").attr("class", "flbt");
                $("#lilink").attr("class", "flbt hover");

                $("#divRight").attr("class", "about none");
                $("#divLia").attr("class", "about none");
                $("#divSe").attr("class", "about none");
                $("#divlink").attr("class", "about");
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
<body>
    <form id="form1" runat="server">
    <uc1:Top ID="top1" runat="server" />
    <uc1:TopSec ID="top2" runat="server" />

    <div class="guideTitle">
        <div class="nr">
            <div class="menu">
                <a href="javascript:void(0);" class="hover">网站申明</a></div>
        </div>
    </div>
    <div class="help-ad">
        <img src="images/statement-ad.jpg" /></div>
    <div class="blank20">
    </div>

    <div class="wrap">
        <!--网站申明左侧菜单 start-->
        <div class="help-sideBar fl">
            <div class="title">
                <h2>
                    网站申明</h2>
            </div>
            <ul class="menu new-menu">
                <li id="liRight" class="flbt hover"><a href="/statement_1.html" class="t"><i class="arrw-i"></i>权利申明</a></li>
                <li id="liLia" class="flbt"><a href="/statement_2.html" class="t"><i class="arrw-i"></i>免责申明</a></li>
                <li id="liSe" class="flbt"><a href="/statement_3.html" class="t"><i class="arrw-i"></i>保密申明</a></li>
                <li id="lilink" class="flbt"><a href="/statement_4.html" class="t"><i class="arrw-i"></i>链接申明</a></li>
            </ul>
        </div>
        <!--网站申明左侧菜单 end-->
        <div class="help-ricon fr about-ri">
            <!--权利申明 start-->
            <div id="divRight" class="about">
                <div class="place">
                    <i href="" class="t">网站申明</i>><i href="" class="t">权利申明</i></div>
                <div class="news-info">
                    <h1>
                        权利申明</h1>
                   
                    <p style="margin-left: 18.0pt; text-indent: 0cm;">
                       <%-- 权利声明：<br />--%>
                        1、除特别指明外，本网站的设计思路、整体结构、网页设计、文字、图片、图表、软件、视音频文件、广告和其它信息等所有内容，其著作权均属我公司所有。任何他人或他方不得复制或在非我公司网站所属的服务器上做镜像或者以其它方式进行非法使用。</p>
                    <p style="margin-left: 18.0pt; text-indent: 0em;">
                        2、本网站著作权归属于我公司，他人或他方如需引用、摘录、转载或以其它方式使用，必须取得我公司的书面许可，并在使用时注明来源和著作权方。对于任何违反国家有关法律法规，不遵守本网站声明，未经本网站同意，擅自使用本网站内容并不注明出处的行为，我公司保留追究其法律责任的权利。</p>
                </div>
                <div class="blank20">
                </div>
            </div>
            <!--权利申明 end-->

            <!--免责申明 start-->
            <div id="divLia" class="about none">
                <div class="place">
                    <i href="" class="t">网站申明</i>><i href="" class="t">免责申明</i></div>
                <div class="news-info">
                    <h1>
                        免责声明</h1>
                    <p style="margin-left: 18.0pt; text-indent: 2em;">
                        1、任何在本网站上出现的信息（包括但不限于评论、预测、图表、指标、理论、直接的或暗示的指示），均只作为参考，您须对您自主决定的行为负责。医站通企业信息及其产品/服务（包括但不限于公司名称、联系人及联络信息，产品/服务的描述和说明，相关图片等）的信息均由企业自行提供，企业依法应对其提供的任何信息承担全部责任。我公司不对因本平台资料全部或部分内容产生的或因依赖该资料而引致的任何损失承担任何责任，也不对任何因本网站提供的资料不充分、不完整或未能提供特定资料产生的任何损失承担任何责任。</p>
                    <p style="margin-left: 18.0pt; text-indent: 2em;">
                        2、互联网传输可能会受到干扰，中断、延迟或数据错误。对于非我公司能控制的通讯设施故障可能引致的数据及交易的不准确性或不及时性，我公司不承担任何责任。</p>
                    <p style="margin-left: 18.0pt; text-indent: 2em;">
                        3、凡通过本平台与其它网站的链接而获得的其它网站提供的网上资料及内容，仅供您浏览和参考之用，请您对相关内容自行辨别及判断，我公司不承担任何责任。
                    </p>
                    <p style="margin-left: 18.0pt; text-indent: 2em;">
                        4、当地法律对责任限制及免除可能有强行性的规定，此种情况下，应以此类强行性的法律为准。</p>
                    <p style="margin-left: 18.0pt; text-indent: 2em;">
                        5、医站通仅为交易双方提供交易推介，并提供平台系统技术、支付、融资、咨询服务，本平台不是交易双方交易关系的参与者，无商品所有权，不承担商品所有权对应的质量保证、售后服务，不提供物流配送。</p>
                </div>
                <div class="blank20">
                </div>
            </div>
            <!--免责申明 end-->
            <!--免责申明 start-->
            <div id="divSe" class="about none">
                <div class="place">
                    <i href="" class="t">网站申明</i>><i href="" class="t">保密申明</i></div>
                <div class="news-info">
                    <h1>
                        保密申明</h1>
                    <p style="margin-left: 18.0pt; text-indent: 2em; padding-top: 10px;">
                        1、我公司会根据业务需要向您收集相关信息，以便为您提供更优质的产品和服务。</p>
                    <p style="margin-left: 18.0pt; text-indent: 2em; padding-top: 10px;">
                        2、我公司将会依照国家有关法律法规，采取各种严格的措施确保您的信息安全。</p>
                    <p style="margin-left: 18.0pt; text-indent: 2em; padding-top: 10px;">
                        3、我公司可能会把您的相关信息提供给与我公司联合提供业务的合作伙伴，但该合作伙伴须对您的相关信息进行保密。</p>
                    <p style="margin-left: 18.0pt; text-indent: 2em; padding-top: 10px;">
                        4、在法律规定的情况下，如司法机关、监管机构及其他相关机构提出要求，我公司有可能提供您的相关信息。</p>
                    <p style="margin-left: 18.0pt; text-indent: 2em; padding-top: 10px;">
                        5、您在我公司网站留下邮件地址信息时，即视为您同意我公司及我公司的合作伙伴向您留下的电子邮箱发送与我公司金融服务有关的电子邮件。</p>
                </div>
                <div class="blank20">
                </div>
            </div>
            <!--免责申明 end-->

            <!--链接申明 start-->
            <div id="divlink" class="about none">
                <div class="place">
                    <i href="" class="t">网站申明</i>><i href="" class="t">链接声明</i></div>
                <div class="news-info">
                    <h1>
                        链接声明</h1>
                    <p style="margin-left: 18.0pt; text-indent: 2em;">
                        1、从本平台链接至我公司以外的网站：某些情况下，本平台会提供跳转至国际互联网上的其它页面或网站的链接。此链接将会引您至第三方发行或经营的网站，而该第三方并非我公司的合作机构或与我公司有任何联系。我公司将该链接列入平台内，仅为协助用户浏览和参考之用。我公司致力于挑选声誉良好的网站和资料来源，以方便用户。</p>
                    <p style="margin-left: 18.0pt; text-indent: 2em;">
                        然而，除非我公司已经明确声明与该第三方有合作关系，提供链接至此第三方网站或网页，并不视为我公司同意、推荐、认可、保证或推介任何第三方或在第三方网站上所提供的任何服务、产品，亦不可视为我公司与该第三方及其网站有任何形式的合作。</p>
                    <p style="margin-left: 18.0pt; text-indent: 2em;">
                        任何由于甲方合理控制范围以外的原因而产生的交易或服务系统问题，造成资料、行情等信息传输或储存上的错误，导致资料泄露、丢失、被盗用或被篡改等，致使甲方平台响应延迟或未能履约的，甲方不承担对此可能造成的任何责任。</p>
                    <p style="margin-left: 18.0pt; text-indent: 2em;">
                        如果链接的网站含有可供下载的软件，则此链接仅为方便您的使用而设。若您在下载软件时遇到任何困难或问题、或因而造成任何影响，我公司概不负责。请谨记，使用任何从国际互联网下载的软件可能受版权条款的管制与约束，若您不遵守该版权同意条款，可能导致侵犯该软件开发者的知识产权，我公司不承担任何责任。</p>
                    <p style="margin-left: 18.0pt; text-indent: 2em;">
                        当您按下某个链接，即会离开本平台并进入另一个网站，您必须遵守该网站的使用条款。</p>
                    <p style="margin-left: 18.0pt; text-indent: 2em;">
                        2. 从本平台链接至我公司其它网站：本网站亦包含链接至我公司其它网站，以方便您的使用。该平台所提供的产品和服务只提供给身处或居所属于该司法管辖地区的人士使用。我公司其他网站各自制定使用条款，条款可能互有差异，您应先仔细查阅适用的使用条款，然后才使用相关的网站。</p>
                    <p style="margin-left: 18.0pt; text-indent: 2em;">
                        3. 从其他网站链接至本平台：若您想在第三方的网站以任何形式建立链接至本平台，必须先取得我公司的书面同意。我公司有权决定是否核准建立此链接。一般而言，我公司只会允许建立纯粹以“医站通”显示的链接，在特殊情况下才会准予将本平台其他相关标志、名称、商标用于或显示于链接，而使用本平台相关标志、名称或商标是否支付费用以及费用金额由我公司全权决定。</p>
                    <p style="margin-left: 18.0pt; text-indent: 2em;">
                        凡从第三方网站建立任何链接至本平台，我公司不负责该链接的建立与设置。依此建立的链接，并不构成我公司与该第三方网站有任何形式的合作，亦不构成我公司对该第三方网站的认同与背书。任何将使用者导向本平台的链接，均需主动且直接的连接至本平台，且只可直接进入本平台的主页或首页；这些链接不可对本平台的各网页或内容予以“分框切割”处理，也不可直接链接至本平台内页。</p>
                    <p style="margin-left: 18.0pt; text-indent: 2em;">
                        如因该链接而产生或导致的任何需由您或第三方承担或蒙受的损失或损害，我公司不承担任何责任。对于经由我公司核准以纯文字格式或任何形式建立的链接，我公司保留随时撤销核准的权利，并有权要求清除任何指向本平台的链接。</p>
                </div>
                <div class="blank20">
                </div>
            </div>
            <!--链接申明 end-->
        </div>
    </div>

    <div class="blank20">
    </div>

    <uc1:Bottom ID="Bottom1" runat="server" />
    <script src="js/layer/layer.js" type="text/javascript"></script>
    <script src="js/layerCommon.js" type="text/javascript"></script>
    </form>
</body>
</html>
