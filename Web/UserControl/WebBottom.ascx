<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WebBottom.ascx.cs" Inherits="UserControl_WebBottom" %>
<%@ OutputCache Duration="180" VaryByParam="none" %>
<script src="/js/layer/layer.js" type="text/javascript"></script>
<script src="/js/layerCommon.js" type="text/javascript"></script>
<!--footer start-->    
<div class="footer">
	<div class="link"><a href="/platform.html">平台介绍</a>|<a href="/about.html">关于我们</a>|<a href="/help.html">帮助中心</a>|<a href="/about.html#contact">联系我们</a>|<a href="/law.html">网站声明</a>|<a href="http://www.aikuaidi.cn/" target="_blank">快递单号查询</a></div>
    <div class="txt">粤ICP备17130448号-2　Copyright © 2016-2017 深圳前海医伴金服信息技术有限公司版权所有　
    <script type="text/javascript">            var cnzz_protocol = (("https:" == document.location.protocol) ? " https://" : " http://"); document.write(unescape("%3Cspan id='cnzz_stat_icon_1258987782'%3E%3C/span%3E%3Cscript src='" + cnzz_protocol + "s11.cnzz.com/z_stat.php%3Fid%3D1258987782%26show%3Dpic' type='text/javascript'%3E%3C/script%3E"));</script>

    </div>
    <div class="pic">
    <a href="http://www.moreyou.cn/"  target="_blank"title="医伴金服"></a>
    <a href="http://icp.alexa.cn/index.php?q=yibanmed.com&code=&icp_host=lncainfo" target="_blank"></a>
    <a href="http://www.police.sh.cn/shga/index.html" target="_blank"></a>
    <a href="http://www.12377.cn/" target="_blank"></a>
    <a href="http://www.12377.cn/node_548446.htm" target="_blank"></a>
    </div>
</div>
<!--footer end-->
    
<!-- QQ客服 region -->
    <div id="float_service">
            <div class="float_start" style="opacity: 0.7; letter-spacing: 0.5em; border-radius: 10px 10px 0px 0px; font-size: 14px; width: 180px; height: 35px; line-height: 35px; cursor: pointer; color: rgb(255, 255, 255); text-indent: 2em; font-family: 宋体, simSun; font-weight: bold; bottom: 0px; visibility: visible; background-color:#2059ae">在线服务
                <em class="float_open" style="position: absolute; top: 10px; right: 30px; width: 20px; height: 20px; background: url(../images/floatservice-bg-5.png) -20px -460px no-repeat;"></em>
            </div>
            <div class="float_contact" style="width: 192px; height: 442px; float: left; background-image: url(../images/floatservice-bg-5.png);">
                <em class="float_close" style="position: absolute; top: 50px; right: 30px; width: 20px; height: 20px; border-radius: 3px; cursor: pointer; opacity: 0.7; background: url(../images/floatservice-bg-5.png) 0px -460px no-repeat;"></em>
                <ul style="margin-top:105px;padding-left:25px;list-style:none;font-size:14px;">
                    <li>
                        <a target="_blank" id="service_qq1" href="http://crm2.qq.com/page/portalpage/wpa.php?uin=4009619099&f=1&ty=1&aty=0&a=&from=6" style="transition-duration: 0.5s; width: 95px; height: 18px; padding: 11px 20px; display: block; color: rgb(255, 255, 255); background-color: rgb(32, 89, 174);">
                            <em style="display:inline-block;background:url(../images/floatservice-bg-5.png) 0 -509px no-repeat;width:20px;height:20px;float:left;margin-right:5px;"></em>
                            <span style="float:left;line-height:18px;">在线客服</span>
                        </a>
                    </li>
                    <li style="margin-top:12px;">
                        <a id="liuyan" href="javascript:void(0)" style="transition-duration: 0.5s; width: 95px; height: 18px; padding: 11px 20px; display: block; color: rgb(255, 255, 255); background-color: rgb(119, 179, 108);">
                            <em style="display:inline-block;background:url(../images/floatservice-bg-5.png) -20px -509px no-repeat;width:20px;height:20px;float:left;margin-right:5px;"></em>
                            <span style="float:left;line-height:18px;">留言反馈</span>
                        </a>
                    </li>
                </ul>
            </div>
        </div>
<!-- QQ客服 end -->

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
                        opacity: 0.7
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
                  , f = '<div id="ly_content" style="left: 581px; top: -400px; visibility: visible; position: absolute; width: 410px;height:560px;background-color:#FFFFFF; z-index: 2028;" class=""><div style="overflow:hidden;line-height:40px;"><a id="closeContent" href="javascript:void(0)" style="float:right;font-size:16px;font-family:arial;margin-right:10px;color:#555555;text-decoration:none;">X</a></div><div style="width:320px;margin:0 auto;"><div style="width:320px;height:47px;background:url(../images/floatservice-bg-5.png) 0 -529px no-repeat;"></div><div style="width:320px;height:42px;margin-top:10px;"><input id="name" maxlength="20" placeholder="怎么称呼您？" style="color:#555555;border:1px solid #cccccc;line-height:18px;height:20px;width:298px;padding:10px;" type="text" /></div><div style="width:320px;height:42px;margin-top:10px;"><input id="phone" maxlength="11" placeholder="请留下您的手机号码" style="color:#555555;border:1px solid #cccccc;line-height:18px;height:20px;width:298px;padding:10px;" type="text" /></div><div style="width:320px;height:42px;margin-top:10px;"><input id="qq" maxlength="20" placeholder="请留下您的邮箱或QQ号码" style="color:#555555;border:1px solid #cccccc;line-height:18px;height:20px;width:298px;padding:10px;" type="text" /></div><div style="width:320px;height:42px;margin-top:10px;"><img style="width:80px;height:42px;float:right;" id="verifyCodeImg" src="" alt="暂无图片" /><input id="verifyCode" maxlength="4" placeholder="验证码" style="color:#555555;border:1px solid #cccccc;line-height:18px;height:20px;width:218px;padding:10px;" type="text" /></div><div style="width:320px;height:150px;margin-top:10px;"><textarea id="content" maxlength="2000" placeholder="如有产品咨询请您留言；如咨询合作代理请务必同时留下您的qq和手机" style="color:#555555;border:1px solid #cccccc;line-height:24px;height:128px;width:298px;padding:10px;" name="" id="" cols="30" rows="10"></textarea></div><div style="width:320px;height:42px;margin-top:20px;"><a id="send" style="width:320px;height:40px;line-height:40px;color:#FFFFFF;text-align:center;background-color:rgb(32, 89, 174);display:block;font-size:16px;letter-spacing:1px;" href="javascript:void(0)">提交您的留言</a></div></div></div>';
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
                              layerCommon.msg("留言成功，我们会尽快联系您！", IconOption.笑脸,2500);
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
