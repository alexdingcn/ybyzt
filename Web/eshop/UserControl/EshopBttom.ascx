<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EshopBttom.ascx.cs" Inherits="EShop_UserControl_EshopBttom" %>
<!--footer start-->
<div class="footer">
    <div class="link" style="display: none">
        <a href="/platform.html">平台介绍</a>|<a href="/about.html">关于我们</a>|<a href="/help.html">帮助中心</a>|<a
            href="/about.html#contact">联系我们</a>|<a href="/law.html">网站声明</a></div>
    <div class="txt">
        © 2017-2018 深圳前海医伴金服信息技术有限公司版权所有 
        <br />
        <a href="http://www.miitbeian.gov.cn" target="_blank" rel="noopener noreferrer">粤ICP备17130448号-2</a>
    </div>
</div>
<!--右侧漂浮 start-->
<div class="fr-outer">
    <div class="outer-mask">
    </div>
    <div class="outer-box">
        <div class="adds" style="display:none;">
            <a style="color: #fff" href="/CompRegister_<%= Request["Comid"]%>.html">
                加<br />
                盟<br />
                </a>
            </div>
            <div class="ou mes">
            <a id="MessageOnline" href="javascript:void(0);"><i class="mes-icon"></i>
                <div class="hover ser-h">
                    店铺在线留言</div>
            </a>
        </div>
        
        <div class="ou tel">
            <i class="tel-icon"></i>
            <div class="hover tel-h">
            <%=phone %>
            </div>
        </div>
        <div class="ou or">
            <i class="or-icon"></i>
            <div class="hover or-h">
                <i class="or-p"></i><i class="or-img">
                    <img src="../images/app-qr.png" height="96" width="96" id="storeCode" runat="server"
                        alt="暂无图片" /></i><i class="or-txt">店铺二维码<br />
                            扫描 分享 转发</i></div>
        </div>
       
        <div class="ou up">
            <a href="javascript:void(0);" id="ReturnTop"><i class="up-icon"></i>
                <div class="hover  ser-h">
                    返回顶部</div>
            </a>
        </div>
    </div>
</div>
<!--右侧漂浮 end-->

     <%--店铺QQ客服 --%>
        <div style="border-radius: 10px 10px 10px 10px;position:fixed;right: 20px;bottom: 77px;z-index:1000;
             font-size: 20px; width: 150px; height: 60px; line-height: 60px;
             text-indent: 2em; font-family: 微软雅黑; background-color: #2059ae;display:<%=qqtrueOrfalse%>">
             <a target="_blank" href="http://wpa.qq.com/msgrd?v=3&uin=<%=qq%>&site=qq&menu=yes" style="color:white">
             <img border="0" width="35" height="35" src="/eshop/images/qq.png" alt="联系店主" title="联系店主" style="margin-top:-5px;margin-left:-30px; vertical-align:middle;" />
                 联系店主</a>
        </div>
       <%--店铺QQ客服 end --%>
<script type="text/javascript">
    
    $(document).ready(function () {
        $("#ReturnTop").on("click", function () {
            $('body,html').animate({ scrollTop: 0 }, 400);
            return false;
        });
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
        $("#MessageOnline").on("click", function () {
            $(".float_close").trigger("click");
            var e = '<div id="ly_lockmask" style="position: fixed;filter:alpha(opacity=40); left: 0px; top: 0px; width: 100%; height: 100%; overflow: hidden; z-index: 2008;background: #000000;opacity: .4;"></div>'
                  , f = '<div id="ly_content" style="left: 581px; top: -400px; visibility: visible; position: absolute; width: 410px;height:460px;background-color:#FFFFFF; z-index: 2028;" class=""><div style="overflow:hidden;line-height:40px;"><a id="closeContent" href="javascript:void(0)" style="float:right;font-size:16px;font-family:arial;margin-right:10px;color:#555555;text-decoration:none;">X</a></div><div style="width:320px;margin:0 auto;"><div style="width:320px;height:47px;background:url(../images/floatservice-bg-5.png) 0 -529px no-repeat;"></div><div style="width:320px;height:42px;margin-top:10px;"><input id="name" maxlength="20" placeholder="怎么称呼您？" style="color:#555555;border:1px solid #cccccc;line-height:18px;height:20px;width:298px;padding:10px;" type="text" /></div><div style="width:320px;height:42px;margin-top:10px;"><input id="phone" maxlength="11" placeholder="请留下您的手机号码" style="color:#555555;border:1px solid #cccccc;line-height:18px;height:20px;width:298px;padding:10px;" type="text" /></div><div style="width:320px;height:150px;margin-top:10px;"><textarea id="content" maxlength="200" placeholder="如有产品咨询请您留言；如咨询合作代理请务必同时留下您的手机" style="color:#555555;border:1px solid #cccccc;line-height:24px;height:128px;width:298px;padding:10px;" name="" id="" cols="30" rows="10"></textarea></div><div style="width:320px;height:42px;margin-top:20px;"><a id="send" style="width:320px;height:40px;line-height:40px;color:#FFFFFF;text-align:center;background-color:rgb(32, 89, 174);display:block;font-size:16px;letter-spacing:1px;" href="javascript:void(0)">提交您的留言</a></div></div></div>';
            0 == $("#ly_content").length && ($("body").append(e),
                $("body").append(f),
            //                $("#verifyCodeImg", "#ly_content").attr("src", "/UserControl/CheckCode.aspx"),
            //                $("#verifyCodeImg", "#ly_content").undelegate("click").delegate("", "click", function () {
            //                    $(this).attr("src", $(this).attr("src") + "?");
            //                },
                $("#name", "#ly_content").defaultValue(),
                $("#phone", "#ly_content").defaultValue(),
            //$("#qq", "#ly_content").defaultValue(),
            //$("#verifyCode", "#ly_content").defaultValue(),
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
                      //MailQQ: $.trim($("#qq", "#ly_content").val()),
                      //Code: $.trim($("#verifyCode", "#ly_content").val()),
                      Msg: $.trim($("#content", "#ly_content").val()),
                      Compid:<%=Compid %>
                  };
                  return Msg.Compid==0 ||(Msg.name == "" || Msg.name == $("#name", "#ly_content").data("defaultValued")) || (Msg.Phone == "" || Msg.Phone == $("#phone", "#ly_content").data("defaultValued")) || (Msg.Msg == "" || Msg.Msg == $("#content", "#ly_content").data("defaultValued")) ? (layerCommon.msg("请输入完整的留言信息", IconOption.哭脸)) : (void $.ajax({
                      url: "../Handler/ShopMessage.ashx",
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
        });
        $(".float_close").on("click", function () {
            $("#float_service").trigger("mouseleave");
        })
    });
</script>
<script type="text/javascript" src="/eshop/js/resolutionEshop.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"></script>
<!--footer end-->
