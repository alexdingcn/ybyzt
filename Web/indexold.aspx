<%@ Page Language="C#" AutoEventWireup="true" CodeFile="indexold.aspx.cs" Inherits="indexold" %>
<%@ Register Src="~/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<%@ Register Src="~/UserControl/ContentControl.ascx" TagPrefix="uc1" TagName="content" %>
<%@ Register Src="~/UserControl/NavTopControl.ascx" TagPrefix="uc1" TagName="TopNav" %>
<%@ Register Src="~/UserControl/BottomControl.ascx" TagPrefix="uc1" TagName="bottom" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <script>

        function browserRedirect() {
            var sUserAgent = navigator.userAgent.toLowerCase();
            var bIsIpad = sUserAgent.match(/ipad/i) == "ipad";
            var bIsIphoneOs = sUserAgent.match(/iphone os/i) == "iphone os";
            var bIsMidp = sUserAgent.match(/midp/i) == "midp";
            var bIsUc7 = sUserAgent.match(/rv:1.2.3.4/i) == "rv:1.2.3.4";
            var bIsUc = sUserAgent.match(/ucweb/i) == "ucweb";
            var bIsAndroid = sUserAgent.match(/android/i) == "android";
            var bIsCE = sUserAgent.match(/windows ce/i) == "windows ce";
            var bIsWM = sUserAgent.match(/windows mobile/i) == "windows mobile";
            if (bIsIpad || bIsIphoneOs || bIsMidp || bIsUc7 || bIsUc || bIsAndroid || bIsCE || bIsWM) {
                //app
                window.location.href = "https://www.yibanmed.com/";
            } else {
                //pc
                //window.location.href="https://www.yibanmed.com/";
            }
        }
        if ('<%=Request["index"] %>' != '1') {
            browserRedirect();
        }
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
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta name="baidu-site-verification" content="ymHVMhGTi6" />
    <meta name="baidu-site-verification" content="cksf9cvilc" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>医站通</title>
    <meta name="keywords" content="我的1818_我的1818网_my1818_网上分销_电子商务_B2B_批发_采购" />
    <meta name="description" content="我的1818网（my1818.com）,中小企业网上分销平台,为您寻找产品批发分销渠道,提供订单管理、代理商管理、资金对账、数据分析等功能,建立企业网上分销管理平台" />
    <link href="css/global-2.0.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <link rel="apple-touch-icon" href="images/havenopicmin.gif" />
    <script type="text/javascript" src="js/jquery-1.9.1.min.js"></script>
    <script src="js/menu.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
    <script type="text/javascript" src="js/weiboscroll.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <!--顶部导航栏 start-->
    <uc1:Top ID="top1" runat="server" />
    <!--顶部导航栏 end-->
    <uc1:TopNav ID="TopNav1" runat="server" ShowID="index" />

<div class="bgf5">
<div class="wrap">
<!--第一屏-中间 start-->
<div class="first-screen-c fl">
	<!--大图广告 start-->
	<div class="main-slider-ad">
		<div class="bd">
		<ul style="position: relative; width: 780px; height: 360px;">
            <li><a target="_blank" href='<%=ConfigCommon.GetBanner("IndexBanner.xml", "1", "url")%>'>
                <img src='../Config/image/<%=ConfigCommon.GetBanner("IndexBanner.xml", "1", "imgurl")%>'
                    width="780" height="330" alt='<%=ConfigCommon.GetBanner("IndexBanner.xml", "1", "title")%>' /></a></li>
            <li><a target="_blank" href='<%=ConfigCommon.GetBanner("IndexBanner.xml", "2", "url")%>'>
                <img src='../Config/image/<%=ConfigCommon.GetBanner("IndexBanner.xml", "2", "imgurl")%>'
                    width="780" height="330" alt='<%=ConfigCommon.GetBanner("IndexBanner.xml", "2", "title")%>' /></a></li>
            <li><a target="_blank" href='<%=ConfigCommon.GetBanner("IndexBanner.xml", "3", "url")%>'>
                <img src='../Config/image/<%=ConfigCommon.GetBanner("IndexBanner.xml", "3", "imgurl")%>'
                    width="780" height="330" alt='<%=ConfigCommon.GetBanner("IndexBanner.xml", "3", "title")%>' /></a></li>
            <li><a target="_blank" href='<%=ConfigCommon.GetBanner("IndexBanner.xml", "4", "url")%>'>
                <img src='../Config/image/<%=ConfigCommon.GetBanner("IndexBanner.xml", "4", "imgurl")%>'
                    width="780" height="330" alt='<%=ConfigCommon.GetBanner("IndexBanner.xml", "4", "title")%>' /></a></li>
          <li><a target="_blank" href='<%=ConfigCommon.GetBanner("IndexBanner.xml", "5", "url")%>'>
                <img src='../Config/image/<%=ConfigCommon.GetBanner("IndexBanner.xml", "5", "imgurl")%>'
                    width="780" height="330" alt='<%=ConfigCommon.GetBanner("IndexBanner.xml", "5", "title")%>' /></a></li>
		</ul>
		</div>
		<div class="banner-btn">
			<a class="prev" href="javascript:void(0);"></a><a class="next" href="javascript:void(0);" style="opacity: 0.4;"></a>
			<div class="hd"><ul><li class="">1</li><li class="">2</li><li class="">3</li><li class="on">4</li><li class="">5</li></ul></div>
		</div>
	</div>
	<!--大图广告end-->   
    <!--快捷入口 start-->  
    <ul class="first-inlet">
    	<li><a href="http://www.my1818.com/e100503_1545.html"><img src="Config/image/ad250-1.jpg" alt=""/></a></li>
        <li><a href="http://www.my1818.com/e100860_1621.html"><img src="Config/image/ad250-2.jpg" alt="" /></a></li>
        <li><a href="http://www.my1818.com/e120390_3410.html"><img src="Config/image/ad250-3.jpg" alt="" /></a></li>
    </ul>
    <!--快捷入口 start-->       
</div>
<!--第一屏-中间 end-->
<!--第一屏右侧 start-->
<div class="fr me-fr">
  <div class="me-box"> 
    <i class="me-i"></i><div class="t1 time"></div>
    <div class="t2"> 欢迎来到my1818.com</div>
  </div>
  <div class="m-login" runat="server" id="index_Dvlogin"> <a href="login.aspx" class="dl">登录</a> <a href="CompRegister.aspx" class="rz">卖家注册</a> </div>

  <div class="m-notice" runat="server" id="index_mNotice">
    <ul class="title">
      <li class="hover" id="a1" onmouseover="setTab2('a',1,3)"><a href="/news_2.html">公告</a></li>  
      <li class="" id="a2" onmouseover="setTab2('a',2,3)"><a href="/help/help.html?K=0">买家入门</a></li>  
      <li class="" id="a3" onmouseover="setTab2('a',3,3)"><a href="/help/help.html?K=1">卖家入门</a></li>  
    </ul>
    <ul class="list hover" id="con_a_1" >
        <asp:Repeater ID="Rpt_News" runat="server">
            <ItemTemplate>
             <li><a target="_blank" href="/newsinfo_<%# Eval("ID") %>.html"title="<%# Eval("NewsTitle") %>"><%# Eval("NewsTitle") %></a></li>
            </ItemTemplate>
        </asp:Repeater>  
    </ul>
    <ul class="list2" id="con_a_2" style="display:none;" >
      <li class="a1"><a href="/help/help.html?K=0&T=0">找货源</a></li><li class="a1"><a href="/help/help.html?K=0&T=1">申请<br />加盟</a></li><li><a href="/help/help.html?K=0&T=2">卖家<br />通过</a></li>
      <li class="a1"><a href="/help/help.html?K=0&T=4">下单<br />购买</a></li><li class="a1"><a href="/help/help.html?K=0&T=5">付款</a></li><li><a href="/help/help.html?K=0&T=6">收货</a></li>
    </ul>
    <ul class="list2" id="con_a_3" style="display:none;" >
      <li class="a1"><a href="/help/help.html?K=1&T=0">卖家<br />注册</a></li><li class="a1"><a href="/help/help.html?K=1&T=1">认证<br />审核</a></li><li><a href="/help/help.html?K=1&T=3">开通<br />旺铺</a></li>
      <li class="a1"><a href="/help/help.html?K=1&T=4">发布<br />商品</a></li><li class="a1"><a href="/help/help.html?K=1&T=5">确认<br />订单</a></li><li><a href="/help/help.html?K=1&T=6">发货</a></li>
    </ul>
  </div>
  
  <div class="blank15"></div>
  <div class="m-notice new">
    <ul class="title"><li class="hover" ><a href="/newslist.aspx">最新入驻</a></li>  </ul>
     <div id="m-dynamic-p" style=" height:103px; overflow:hidden;">
        <div class="m-dynamic">
          <ul>
             <asp:Repeater ID="Rpt_comps" runat="server">
                 <ItemTemplate>
                    <li><i class="map-i"></i><%# Eval("CompName").ToString().Length > 12 ? Eval("ShortName").ToString() : Eval("CompName").ToString()%>入驻</li>
                 </ItemTemplate>
             </asp:Repeater>
          </ul>
        </div>
    </div>
  </div>
</div>
<div class="blank15"></div>
</div>
</div>
<!--第一屏右侧 end--> 



<div class="wrap">
    <!--首页中间商品分类内容-->
    <uc1:content runat="server" ID="uc_Content" />
        
<!--新闻资讯 start-->
<div class="blank25"></div>
    <div class="m-news fl">
    	<div class="title">行业新闻<a href="/news_1.html" class="more">></a></div>
    	<div class="pic"><a href="/news_1.html" target="_blank"><img src="Config/image/m-news-p1.jpg" /></a></div>
        <ul class="list">
            <asp:Repeater ID="Rpt_News_1" runat="server">
                <ItemTemplate>
                    <li><i class="icon">></i><a target="_blank" href="/newsinfo_<%#Eval("id")%>.html"><%#Eval("NewsTitle") %></a><i class="time"><%#Eval("CreateDate").ToString().ToDateTime().ToString("yyyy-MM-dd") %></i></li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
    </div>            
    <div class="m-news fl male40">
    	<div class="title">my1818资讯<a href="/news_3.html" class="more">></a></div>
    	<div class="pic"><a href="/news_3.html" target="_blank"><img src="Config/image/m-news-p2.jpg" /></a></div>
        <ul class="list">
            <asp:Repeater ID="Rpt_News_2" runat="server">
                <ItemTemplate>
                    <li><i class="icon">></i><a target="_blank" href="/newsinfo_<%#Eval("id")%>.html"><%#Eval("NewsTitle") %></a><i class="time"><%#Eval("CreateDate").ToString().ToDateTime().ToString("yyyy-MM-dd") %></i></li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
    </div>           
    <div class="m-news fl">
    	<div class="title">生意经<a href="/news_4.html" class="more">></a></div>
    	<div class="pic"><a href="/news_4.html" target="_blank"><img src="Config/image/m-news-p3.jpg" /></a></div>
        <ul class="list">
            <asp:Repeater ID="Rpt_News_4" runat="server">
                <ItemTemplate>
                    <li><i class="icon">></i><a target="_blank" href="/newsinfo_<%#Eval("id")%>.html"><%#Eval("NewsTitle") %></a><i class="time"><%#Eval("CreateDate").ToString().ToDateTime().ToString("yyyy-MM-dd") %></i></li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
    </div>
<!--新闻资讯 end-->
 </div>
  <div class="blank10"></div>
 <!--友情链接 start-->
 <div class="w1200">
    	<div class="m-news"><div class="title">友情链接</div></div>
    	<div class="lianjie"><a href="http://www.naico.com.cn/" target="_blank">耐柯工业吸尘器</a>
        <a href="http://www.seazheng.cn/" target="_blank">海郑实业</a>
        <a href="http://www.bizrobot.com/" target="_blank">B2B信息网</a>
        <a href="http://www.cnpowdertech.com/" target="_blank">中国粉碎技术网</a>
        <a href="http://www.lvduep.com/" target="_blank">车载空气净化器</a>
        <a href="http://www.benben77.com/" target="_blank">儿童游乐设备</a>
        <a href="http://www.runbaijia.com/" target="_blank">售楼部软装设计 </a>
        </div>
    </div>
 <!--友情链接end--> 
  <div class="blank25"></div>
    <!--页尾 start-->

   <uc1:bottom runat="server" ID="bottom" />

    <!--页尾 end-->
    </form>
</body>

<script src="js/layer/layer.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
<script src="js/layerCommon.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
<script type="text/javascript" src="js/jquery.SuperSlide.2.1.1.js"></script>
<script type="text/javascript" src="js/jscarousel.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $('#jsCarousel').jsCarousel({ onthumbnailclick: function (src) {
            $("#overlay_pic").attr('src', src);
            $(".overlay").show();
        }, autoscroll: true
        });

        $(".overlay").click(function () {
            $(this).hide();
        });
        $("#ulcomp li,#uldis li").click(function () {
            e.stopPropagation();
            //  location.href = $(this).find('a').attr('href');
            window.open($(this).find('a').attr('href'), "_blank");
        })
        //时间提示
        var now = new Date();
        var hour = now.getHours()
        if (hour < 6) { $(".me-box .time").html("Hi,凌晨好！") }
        else if (hour < 9) { $(".me-box .time").html("Hi,早上好！") }
        else if (hour < 12) { $(".me-box .time").html("Hi,上午好！") }
        else if (hour < 14) { $(".me-box .time").html("Hi,中午好！") }
        else if (hour < 17) { $(".me-box .time").html("Hi,下午好！") }
        else if (hour < 19) { $(".me-box .time").html("Hi,傍晚好！") }
        else if (hour < 22) { $(".me-box .time").html("Hi,晚上好！") }
        else { $(".me-box .time").html(" Hi,晚上好！") }
    })

    function ToPag(id) {

    }
</script>
<script type="text/javascript">
    $(document).ready(function () {

        function b() {
            var b = {
                clientW: document.body.clientWidth,
                clientH: document.body.clientHeight,
                scrollTop: $(window).scrollTop()
            }
              , c = {
                  left: b.clientW / 2 - ($("#ly_content").width() / 2),
                  top: b.scrollTop + 50
              };
            $("#ly_content").css({
                left: c.left
            }).animate({
                top: c.top
            })
        };
        $.fn.extend({ defaultValue: function () {
            var c = function () {
                var a = document.createElement("input");
                return "placeholder" in a
            } ();
            c ? !1 : this.each(function (index, control) {
                $(control).data("defaultValued", $(control).attr("placeholder")),
                $(control).val($(control).data("defaultValued")),
                $(control).undelegate("focus").delegate("", "focus", function () {
                    if ($.trim($(this).val()) == $(this).data("defaultValued")) {
                        $(this).val("");
                    }
                }),
                $(control).undelegate("blur").delegate("", "blur", function () {
                    if ($.trim($(this).val()) == "") {
                        $(this).val($(this).data("defaultValued"));
                    }
                })
            })
        }
        }),
        $("#float_service").css({ position: "fixed", right: "10px", bottom: "-442px", width: "182px", overflow: "hidden", zIndex: "2007" }).on({
            "mouseenter": function () {
                $("#float_service").stop().animate({
                    bottom: 0,
                    height: "426px"
                }),
                    $(".float_start").css({
                        opacity: 0,
                        visibility: "hidden"
                    });
            },
            "mouseleave": function () {
                $("#float_service").stop().animate({
                    bottom: "-392px",
                    height: "426px"
                }, function () {
                    $(".float_start").css({
                        visibility: "visible"
                    }).animate({
                        opacity: 1
                    })
                });
            }
        }),
        $("#liuyan").css({
            "transition-duration": "0.5s",
            width: "95px",
            height: "18px",
            padding: "11px 20px",
            display: "block",
            backgroundColor: "#77b36c",
            color: "#FFFFFF"
        }).on({
            mouseenter: function () {
                $(this).css({
                    "transition-duration": "0.5s",
                    width: "95px",
                    height: "18px",
                    padding: "11px 20px",
                    display: "block",
                    backgroundColor: "#508945",
                    color: "#FFFFFF"
                })
            },
            mouseleave: function () {
                $(this).css({
                    "transition-duration": "0.5s",
                    width: "95px",
                    height: "18px",
                    padding: "11px 20px",
                    display: "block",
                    backgroundColor: "#77b36c",
                    color: "#FFFFFF"
                })
            }
        }).on("click", function () {
            $(".float_close").trigger("click");
            var e = '<div id="ly_lockmask" style="position: fixed;filter:alpha(opacity=40); left: 0px; top: 0px; width: 100%; height: 100%; overflow: hidden; z-index: 2008;background: #000000;opacity: .4;"></div>'
                  , f = '<div id="ly_content" style="left: 581px; top: -400px; visibility: visible; position: absolute; width: 410px;height:560px;background-color:#FFFFFF; z-index: 2028;" class=""><div style="overflow:hidden;line-height:40px;"><a id="closeContent" href="javascript:void(0)" style="float:right;font-size:16px;font-family:arial;margin-right:10px;color:#555555;text-decoration:none;">X</a></div><div style="width:320px;margin:0 auto;"><div style="width:320px;height:47px;background:url(../images/floatservice-bg-5.png) 0 -529px no-repeat;"></div><div style="width:320px;height:42px;margin-top:10px;"><input id="name" maxlength="20" placeholder="怎么称呼您？" style="color:#555555;border:1px solid #cccccc;line-height:18px;height:20px;width:298px;padding:10px;" type="text" /></div><div style="width:320px;height:42px;margin-top:10px;"><input id="phone" maxlength="11" placeholder="请留下您的手机号码" style="color:#555555;border:1px solid #cccccc;line-height:18px;height:20px;width:298px;padding:10px;" type="text" /></div><div style="width:320px;height:42px;margin-top:10px;"><input id="qq" maxlength="20" placeholder="请留下您的邮箱或QQ号码" style="color:#555555;border:1px solid #cccccc;line-height:18px;height:20px;width:298px;padding:10px;" type="text" /></div><div style="width:320px;height:42px;margin-top:10px;"><img width="80" height="42" style="float:right;" id="verifyCodeImg" src="" alt="留言" /><input id="verifyCode" maxlength="4" placeholder="验证码" style="color:#555555;border:1px solid #cccccc;line-height:18px;height:20px;width:218px;padding:10px;" type="text" /></div><div style="width:320px;height:150px;margin-top:10px;"><textarea id="content" maxlength="2000" placeholder="如有产品咨询请您留言；如咨询合作代理请务必同时留下您的qq和手机" style="color:#555555;border:1px solid #cccccc;line-height:24px;height:128px;width:298px;padding:10px;" name="" id="" cols="30" rows="10"></textarea></div><div style="width:320px;height:42px;margin-top:20px;"><a id="send" style="width:320px;height:40px;line-height:40px;color:#FFFFFF;text-align:center;background-color#008be3;display:block;font-size:16px;letter-spacing:1px;" href="javascript:void(0)">提交您的留言</a></div></div></div>';
            0 == $("#ly_content").length && ($("body").append(e),
                $("body").append(f),
                $("#verifyCodeImg", "#ly_content").attr("src", "UserControl/CheckCode.aspx"),
                $("#verifyCodeImg", "#ly_content").undelegate("click").delegate("", "click", function () {
                    $(this).attr("src", '/UserControl/CheckCode.aspx?t=' + new Date().getTime());
                },
                $("#name", "#ly_content").defaultValue(),
                $("#phone", "#ly_content").defaultValue(),
                $("#qq", "#ly_content").defaultValue(),
                $("#verifyCode", "#ly_content").defaultValue(),
                $("#content", "#ly_content").defaultValue()
                ), b(),
              $("#closeContent", "#ly_content").undelegate("click").delegate("", "click", function () {
                  $("#ly_lockmask").animate({
                      opacity: 0
                  }, 300, function () {
                      $(this).remove()
                  }),
                    $("#ly_content").animate({
                        opacity: 0
                    }, 300, function () {
                        $(this).remove()
                    })
              }),
              $("#send", "#ly_content").undelegate("click").delegate("", "click", function () {
                  var Msg =
                  {
                      name: $.trim($("#name", "#ly_content").val()),
                      Phone: $.trim($("#phone", "#ly_content").val()),
                      MailQQ: $.trim($("#qq", "#ly_content").val()),
                      Code: $.trim($("#verifyCode", "#ly_content").val()),
                      Msg: $.trim($("#content", "#ly_content").val())
                  };
                  return (Msg.name == "" || Msg.name == $("#name", "#ly_content").data("defaultValued")) || (Msg.Phone == "" || Msg.Phone == $("#phone", "#ly_content").data("defaultValued")) || (Msg.Code == "" || Msg.Code == $("#verifyCode", "#ly_content").data("defaultValued")) || (Msg.Msg == "" || Msg.Msg == $("#content", "#ly_content").data("defaultValued")) ? (layerCommon.msg("请输入完整的留言信息", IconOption.哭脸)) : (void $.ajax({
                      url: "../Handler/SubmitUserMsg.ashx",
                      type: "get",
                      data: Msg,
                      dataType: 'json',
                      timeout: 4000,
                      success: function (ReturnData) {
                          if (ReturnData.Result) {
                              layerCommon.msg("留言成功，我们会尽快联系您！", IconOption.笑脸, 2500);
                              $("#ly_lockmask").animate({
                                  opacity: 0
                              }, 300, function () {
                                  $(this).remove()
                              }),
                             $("#ly_content").animate({
                                 opacity: 0
                             }, 300, function () {
                                 $(this).remove()
                             });
                          }
                          else {
                              layerCommon.msg(ReturnData.Msg, IconOption.哭脸);
                          }
                      }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                          if (XMLHttpRequest.statusText == "timeout") {
                              layerCommon.msg("提交留言超时，请稍候再试！", IconOption.哭脸);
                          } else {
                              layerCommon.msg("提交留言超时，请稍候再试！", IconOption.哭脸);
                          }
                      }
                  }));
              })
          );
        });


        $(".float_close").on("click", function () {
            $("#float_service").trigger("mouseleave");
        })
    });
</script>
</html>
