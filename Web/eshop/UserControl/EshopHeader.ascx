<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EshopHeader.ascx.cs" Inherits="EShop_UserControl_EshopHeader" %>
<%@ Register Src="~/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
    <script type="text/javascript">
        
        $.fn.extend({ defaultValue: function () {
            var c = function () {
                var a = document.createElement("input");
                return "placeholder" in a
            } ();
            return c ? $(this) : $(this).each(function (index, control) {
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
        });
        $(document).ready(function () {

            $("#<%=txt_GoosName.ClientID %>").defaultValue();
            $("#btn_SearCh").on("click", function () {
                var ZControl = $(".ztitle:eq(1)")[0] || $(".ztitle:eq(0)")[0];
                $("#Top_Scro").val((document.documentElement.clientHeight + ZControl.offsetTop));
            });

            $(".AllGoods").on("click", function () {
                $(this).find("a").attr("href", '/<%=Request["Comid"] %>.html#allPro');
                $(".HNav").attr("href", '/<%=Request["Comid"] %>.html');
                $(this).siblings("li[class=\"hover\"]").attr("class", "");
                $(this).attr("class", "hover");
            });

            document.onkeydown = function (event) {
                var $_Serach = $("#btn_SearCh");
                var target, code, tag;
                if ($("#<%=txt_GoosName.ClientID %>").is(":focus")) {
                    if (!event) {
                        event = window.event; //针对ie浏览器  
                        target = event.srcElement;
                        code = event.keyCode;
                        if (code == 13) {
                            $_Serach[0].click();
                            return false;
                        }
                    }
                    else {
                        target = event.target; //针对遵循w3c标准的浏览器，如Firefox  
                        code = event.keyCode;
                        if (code == 13) {
                            $_Serach[0].click();
                            return false;
                        }
                    }
                }
            }

        });
        function checkpro() {

            var proname = $("#<%=txt_GoosName.ClientID %>").val();

            if (proname == '欢迎进入')
                proname = '';
            var str = new RegExp("[()（）]")
            var pattern = new RegExp("[\\\\\/%--`~!@#$^\\?=|{}'_:;',\\[\\].<>~！@#￥……——| {}【】‘；：”“'。，、？\"]")        //格式 RegExp("[在中间定义特殊过滤字符]")  
            var s = proname;
            var rs = "";
            for (var i = 0; i < s.length; i++) {
                if (!str.test(s.substr(i, 1))) {
                    rs = rs + s.substr(i, 1).replace(pattern, '');
                }
                else {
                    rs = rs + s.substr(i, 1);
                }

            }
            $("#<%=txt_GoosName.ClientID %>").val(rs); 
        }
    </script>
  <uc1:Top runat="server" ID="HeaderTop" />
      <!--header start-->
        <div class="header">
            <div class="nr">
                <% if (!string.IsNullOrWhiteSpace(ComList[0].CompLogo))
                    { %>
                    <div class="logo left">
                       <img  src="<%= ConfigurationManager.AppSettings["ImgViewPath"] + "CompImage/" + ComList[0].CompLogo %>"/>
	                </div>
                <% } %>
	            <div class="logoTxt left"><%= ComList[0].CompName %></div>
                
                <div class="or-app right">
                    <span><img runat="server" id="ImgSmCompQRCode" src="../images/app-qr.png" width="60" height="60" alt="店铺二维码" ></span>
                    <i class="t">店铺二维码</i>
                    <div class="hover">
                        <span><img runat="server" id="ImgBigCompQRCode" src="/images/app-qr.png" width="100" height="100" alt="扫描转发·分享店铺" ></span>
                        <i class="t2">扫描转发·分享店铺</i>
                    </div>
                </div>
                <div class="search right">
                    <i class="fdj_icon"></i><input id="txt_GoosName" runat="server" type="text" class="box" placeholder="欢迎进入"/><a id="btn_SearCh" class="btn" onclick="checkpro()"  href="javascript:location.href='/'+'<%=Request["Comid"] %>_'+$.trim($('#<%=txt_GoosName.ClientID %>').val())+'_'+$.trim($('#Top_Scro').val())+'.html'">搜索商品</a>
                </div>
            </div>
        </div>
    <!--header end-->
    <!--nav start-->
<div class="nav enav">
    <div class="nr">
	    <ul class="menu">
            <li runat="server" id="HoverGoods"><a runat="server" class="HNav" id="Nav_AllGoods" href="javascript:;">店铺首页</a></li>
            <li runat="server" id="HoverAllGoods" class="AllGoods"><a href='/<%=Request["Comid"] %>.html#allPro'>全部商品</a></li>
            <li runat="server" id="HoverCompNew"><a href="/eshop/about_<%=Request["Comid"]%>.html">公司介绍</a></li>
            <li style="display:none;"><a href="/CompRegister_<%=Request["Comid"] %>.html">加盟 · 购买</a></li>
            
        </ul>
        <div runat="server"  visible="false" id="RidrctCart" class="shop right"><a href="javascript:;"><i class="txt">进入管理中心</i><i class="jt-icon"></i></a></div>
    </div>
</div>
<input type="hidden"  id="Top_Scro" />
<!--nav end-->