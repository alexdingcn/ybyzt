<%@ Page Language="C#" AutoEventWireup="true" CodeFile="law.aspx.cs" Inherits="law" EnableViewState="false" %>
<%@ Register Src="~/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<%@ Register Src="~/UserControl/NavTopControl.ascx" TagPrefix="uc1" TagName="TopNav" %>
<%@ Register Src="~/UserControl/BottomControl.ascx" TagPrefix="uc1" TagName="Bottom" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>网站声明 医站通</title>
    <meta name="keywords" content="我的1818_我的1818网_医站通_网上分销_电子商务_B2B_批发_采购" />
    <meta name="description" content="我的1818网（医站通.com）,中小企业网上分销平台,为您寻找产品批发分销渠道,提供订单管理、代理商管理、资金对账、数据分析等功能,建立企业网上分销管理平台" />
    <link href="css/global-2.0.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <script src="Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        var c = $("#b3").offset().top;
        var d1 = $("#a1").offset().top;
        var d2 = $("#a2").offset().top;
        var d3 = $("#a3").offset().top;
        var d4 = $("#a4").offset().top;
        var d5 = $("#a5").offset().top;
        $(window).scroll(function () {
            var e1 = d1 - $(window).scrollTop();
            var e2 = d2 - $(window).scrollTop();
            var e3 = d3 - $(window).scrollTop();
            var e4 = d4 - $(window).scrollTop();
            var e5 = d5 - $(window).scrollTop();
            if (e2 < c&&e1!=0) {
                if (e3 > c || e2 == 0) {
                    $(".sidebar").find("li").removeClass("hover");
                    $("#b2").addClass("hover");
                }
                else {
                    if (e4 > c || e3 == 0) {
                        $(".sidebar").find("li").removeClass("hover");
                        $("#b3").addClass("hover");
                    }
                    else {
                        if (e5 > c||e4==0) {
                            $(".sidebar").find("li").removeClass("hover");
                            $("#b4").addClass("hover");
                        }
                        else {
                            $(".sidebar").find("li").removeClass("hover");
                            $("#b5").addClass("hover");
                        }
                    }
                }
            }
            else {
                $(".sidebar").find("li").removeClass("hover");
                $("#b1").addClass("hover");
            }
        });
    });
</script>
<style>.mianDiv {
    margin: 0 auto;
    width: 1200px;
}
/*用户协议*/
.agreement{ position:relative; padding-top:30px;}
.agreement .sidebar{ position:absolute; top:50%; left:0; text-align:right; width:150px; padding-top:30px; }
.agreement .sidebar li{ margin-top:5px; position:relative; cursor:pointer;}
.agreement .sidebar a{ display:inline-block; background:#dbe9ff; height:22px; line-height:22px; color:#3c6eb9; padding:0px 10px; border-radius:5px; font-size:13px;}
.agreement .sidebar .trigon{border-color: transparent #eeeeee transparent transparent; position:absolute; top:5px; right:-10px;}
.agreement .sidebar .circle{ background:#e4e4e4; width:7px; height:7px; display:inline-block; border-radius:7px; position:absolute; top:-30px; right:-14px;}
.agreement .sidebar .hover a,.agreement .sidebar a:hover{ background:#fcaf00; color:#fff; font-weight:bold; text-decoration:none;}
.agreement .sidebar .hover .trigon,.agreement .sidebar li:hover .trigon{border-color: transparent #fcaf00 transparent transparent;}

.agreement .txtNr{ margin-left:160px; border-left:1px solid #eee; padding:0px 35px;}
.agreement .txtNr{ line-height:35px; color:#666; font-size:14px;}
.agreement .txtNr h1{ text-align:center; font-size:22px; font-weight:normal; padding:20px 0; color:#494949;}
.agreement .txtNr p{ text-indent:2em; padding-top:5px;}
.agreement .txtNr h3{ font-size:14px; color:#333; padding-top:30px;}

.float{ background:#fcaf00; width:30px; height:28px; position:fixed; top:70%; right:20px; border-radius:5px;}
.float .trigon{border-color: transparent transparent  #ffffff transparent; position:absolute; top:5px; right:9px;}
.trigon {
    border-color: #d1d1d1 transparent transparent;
    border-style: solid;
    border-width: 6px;
    display: inline-table;
    height: 0;
    line-height: 0;
    width: 0;
}
</style>
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
    <uc1:TopNav ID="TopNav1" runat="server" ShowID="" />
<a href="#" class="float"><i class="trigon"></i></a>

<div class="mianDiv agreement">
	<ul class="sidebar" style="position:fixed; top:50%; margin-top:-150px;left:auto;">
    	<li class="hover" id="b1"><a href="#a1">网站声明</a><i class="trigon"></i><i class="circle"></i></li>
        <li id="b2"><a href="#a2">权利声明</a><i class="trigon"></i></li>
        <li id="b3"><a href="#a3">免责声明</a><i class="trigon"></i></li>
        <li id="b4"><a href="#a4">保密声明</a><i class="trigon"></i></li>
        <li id="b5"><a href="#a5">链接声明</a><i class="trigon"></i></li>
    </ul>
	
    <div class="txtNr">
    	<h1 id="a1">医站通网站声明</h1>
        <h3  style="padding-top:0px;">网站声明：</h3>
    	<p>本声明为深圳前海医伴金服信息技术有限公司医站通B2B电子商务平台（以下简称“医站通”或“平台”）使用的条款，凡浏览或以其它方式使用（以下统称“使用”）本平台及相关网页的用户，均表示接受本声明所有条款。深圳前海医伴金服信息技术有限公司（以下简称“我公司”）保留对本声明包含的条款、条件和说明变更的权利，变更自公布时生效。您应当经常浏览本网站声明，以了解本网站相关规则的变化。</p>
        <!--<p>您通过其它网站的链接进入本平台可能存在登录假冒医伴金服的风险，建议您采用安全的方式（如在浏览器地址栏直接键入医伴金服网址，域名为http://www.shangyijiu.com，或点击收藏夹内记录的正确网址）访问本平台。</p>-->
        <p>当您使用医站通服务时，您不可以：粘贴或传播具有威胁性、诽谤性、贬损性、报复性、亵渎性、涉隐私或隐密性的信息及其它法律法规禁止传播的信息，以及可能侵害其他单位或个人的人身、财产权利（包括但不限于著作权、商标权、专利权等）的文字、数据、图形、程序等其他信息。</p>
        
        <h3 id="a2">权利声明：</h3>
        <p>1、除特别指明外，本网站的设计思路、整体结构、网页设计、文字、图片、图表、软件、视音频文件、广告和其它信息等所有内容，其著作权均属我公司所有。任何他人或他方不得复制或在非我公司网站所属的服务器上做镜像或者以其它方式进行非法使用。</p>
        <p>2、本网站著作权归属于我公司，他人或他方如需引用、摘录、转载或以其它方式使用，必须取得我公司的书面许可，并在使用时注明来源和著作权方。对于任何违反国家有关法律法规，不遵守本网站声明，未经本网站同意，擅自使用本网站内容并不注明出处的行为，我公司保留追究其法律责任的权利。</p>
        <p>3、我公司特别指出：他人或他方未经许可不得修改、复制或以其它方式使用医站通相关的商标和标识；任何试图淡化或丑化上述商标和标识的行为都是违法的，我公司保留追究其法律责任的权利。</p>
        <p>4、我公司对于与他人/他方共同开发的所有内容和服务拥有或与合作者共同拥有全部知识产权，受有关著作权、商标权、专利权等知识产权法律的保护。</p>
        <p>5、遵从知识产权法律法规和本网站内容的声明，您可在非商业用途的情况下浏览、下载本网站的内容。如出于商业用途的使用，则必须经过我公司的书面许可，并在使用时注明来源和著作权方。对于不经本网站同意，擅自使用本网站内容并不注明出处的行为，我公司保留追究其法律责任的权力。</p>
        <p>6、本网站部分内容由其它组织、团体、机构或个人提供，这些内容的著作权属于相应的提供者。本网站引用、摘录或转载这些内容时均已获得著作权持有人的许可，并按约定注明。任何人需要使用本网站包含的由其它组织、团体、机构或个人提供的内容，请直接与著作权人联系，与之相关的任何事务以及法律责任均与本网站无关。</p>
        <p>7、本网站引用、摘录或转载来自第三方内容，仅供访问者交流或参考，文中观点或信息与本网站无关。</p>
        <p>8、本网站摘录或转载来自第三方内容时，均严格按照我国网络著作权相关法律、法规和司法解释确立的原则进行。任何人浏览本网站时如发现有关文章存在侵权事宜，请立即通知网络管理员，如果属侵权信息，本网站将立即在职责范围内予以清除。</p>
        
        <h3 id="a3">免责声明</h3>
        <p>1、任何在本网站上出现的信息（包括但不限于评论、预测、图表、指标、理论、直接的或暗示的指示），均只作为参考，您须对您自主决定的行为负责。医站通企业信息及其产品/服务（包括但不限于公司名称、联系人及联络信息，产品/服务的描述和说明，相关图片等）的信息均由企业自行提供，企业依法应对其提供的任何信息承担全部责任。我公司不对因本平台资料全部或部分内容产生的或因依赖该资料而引致的任何损失承担任何责任，也不对任何因本网站提供的资料不充分、不完整或未能提供特定资料产生的任何损失承担任何责任。</p>
        <p>2、互联网传输可能会受到干扰，中断、延迟或数据错误。对于非我公司能控制的通讯设施故障可能引致的数据及交易的不准确性或不及时性，我公司不承担任何责任。</p>
        <p>3、凡通过本平台与其它网站的链接而获得的其它网站提供的网上资料及内容，仅供您浏览和参考之用，请您对相关内容自行辨别及判断，我公司不承担任何责任。</p>
        <p>4、当地法律对责任限制及免除可能有强行性的规定，此种情况下，应以此类强行性的法律为准。</p>
        <p>5、医站通仅为交易双方提供交易推介，并提供平台系统技术、支付、融资、咨询服务，本平台不是交易双方交易关系的参与者，无商品所有权，不承担商品所有权对应的质量保证、售后服务，不提供物流配送。</p>
        
        <h3 id="a4">保密声明</h3>
        <p>1、我公司会根据业务需要向您收集相关信息，以便为您提供更优质的产品和服务。</p>
        <p>2、我公司将会依照国家有关法律法规，采取各种严格的措施确保您的信息安全。</p>
        <p>3、我公司可能会把您的相关信息提供给与我公司联合提供业务的合作伙伴，但该合作伙伴须对您的相关信息进行保密。</p>
        <p>4、在法律规定的情况下，如司法机关、监管机构及其他相关机构提出要求，我公司有可能提供您的相关信息。</p>
        <p>5、您在我公司网站留下邮件地址信息时，即视为您同意我公司及我公司的合作伙伴向您留下的电子邮箱发送与我公司金融服务有关的电子邮件。</p>
        
        <h3 id="a5">链接声明</h3>
        <p>1、从本平台链接至我公司以外的网站：某些情况下，本平台会提供跳转至国际互联网上的其它页面或网站的链接。此链接将会引您至第三方发行或经营的网站，而该第三方并非我公司的合作机构或与我公司有任何联系。我公司将该链接列入平台内，仅为协助用户浏览和参考之用。我公司致力于挑选声誉良好的网站和资料来源，以方便用户。</p>
        <p>然而，除非我公司已经明确声明与该第三方有合作关系，提供链接至此第三方网站或网页，并不视为我公司同意、推荐、认可、保证或推介任何第三方或在第三方网站上所提供的任何服务、产品，亦不可视为我公司与该第三方及其网站有任何形式的合作。</p>
        <p>任何由于甲方合理控制范围以外的原因而产生的交易或服务系统问题，造成资料、行情等信息传输或储存上的错误，导致资料泄露、丢失、被盗用或被篡改等，致使甲方平台响应延迟或未能履约的，甲方不承担对此可能造成的任何责任。</p>
        <p>如果链接的网站含有可供下载的软件，则此链接仅为方便您的使用而设。若您在下载软件时遇到任何困难或问题、或因而造成任何影响，我公司概不负责。请谨记，使用任何从国际互联网下载的软件可能受版权条款的管制与约束，若您不遵守该版权同意条款，可能导致侵犯该软件开发者的知识产权，我公司不承担任何责任。</p>
        <p>当您按下某个链接，即会离开本平台并进入另一个网站，您必须遵守该网站的使用条款。</p>
        <p>2. 从本平台链接至我公司其它网站：本网站亦包含链接至我公司其它网站，以方便您的使用。该平台所提供的产品和服务只提供给身处或居所属于该司法管辖地区的人士使用。我公司其他网站各自制定使用条款，条款可能互有差异，您应先仔细查阅适用的使用条款，然后才使用相关的网站。</p>
        <p>3. 从其他网站链接至本平台：若您想在第三方的网站以任何形式建立链接至本平台，必须先取得我公司的书面同意。我公司有权决定是否核准建立此链接。一般而言，我公司只会允许建立纯粹以“医站通”显示的链接，在特殊情况下才会准予将本平台其他相关标志、名称、商标用于或显示于链接，而使用本平台相关标志、名称或商标是否支付费用以及费用金额由我公司全权决定。</p>
        <p>凡从第三方网站建立任何链接至本平台，我公司不负责该链接的建立与设置。依此建立的链接，并不构成我公司与该第三方网站有任何形式的合作，亦不构成我公司对该第三方网站的认同与背书。任何将使用者导向本平台的链接，均需主动且直接的连接至本平台，且只可直接进入本平台的主页或首页；这些链接不可对本平台的各网页或内容予以“分框切割”处理，也不可直接链接至本平台内页。</p>
        <p>如因该链接而产生或导致的任何需由您或第三方承担或蒙受的损失或损害，我公司不承担任何责任。对于经由我公司核准以纯文字格式或任何形式建立的链接，我公司保留随时撤销核准的权利，并有权要求清除任何指向本平台的链接。</p>
        <div class="blank20"></div><div class="blank20"></div><div class="blank20"></div>
    </div>

</div>

    <!--footer start-->
    <uc1:Bottom ID="Bottom1" runat="server" />
<script src="js/layer/layer.js" type="text/javascript"></script>
<script src="js/layerCommon.js" type="text/javascript"></script>
    <!--footer end-->
    <link href="css/root.css" rel="stylesheet" type="text/css" />
    </form>
</body>
</html>
