<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NavTopControl.ascx.cs"
    Inherits="UserControl_NavTopControl" %>
 <script src="js/MYCommon.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
<script type="text/javascript">

    $(document).ready(function () {

        $(".navmenu").on({
            "mouseenter": function () {
                $(".leftsideBar").show();
            },
            "mouseleave": function () {
                <% if (!IsIndex) {%>
                 $(".leftsideBar").hide();
               <% }%>
            }
        });


        //鼠标移动分类查询二级/三级分类数据
        $(".submenu").hover(function () {
            var indid = $(this).children("h3").attr("indid"), Parent = this;
            MYCommon.IsWait = false;
            MYCommon.dataType = "text";
            $(".cur", this).html("");
            MYCommon.AjaxRequest("/Handler/BusImgHandler.ashx", { PageAction: "GetClassChild", indid: indid }, function (result) {
                if (result) {
                    //var Html = "";
                    //$.each(result, function (index,item) {
                    //    Html += ' <h2 class="title"><a href="/goodslist_' + item.id + '.html">' + item.name + '</a></h2>';
                    //    Html += ' <ul class="list">';
                    //    $.each(item.child, function (index, item1) {
                    //        Html += '<li><a href="/goodslist_' + item1.id + '.html">' + item1.name + '</a></li>';
                    //    })
                    //    Html += ' </ul>';
                    //})
                    $(".cur", Parent).append(result);
                }
            }, true, function () {

            }, function () {

            })
        }, function () {
            $(".cur", this).html("");
        });


        //店铺/商品 选项切换
        $(document).on("click",".header .search .opt .cur i", function () {
            var text = $(this).text(), prevtext = $(".header .search .opt").text().replace(text, ""), opt = $(".header .search .opt");
            opt.html('' + text + '<i class="down-i"></i><div class="cur"><i>' + prevtext + '</i></div>').attr("type", text == "店铺" ? 0 : 1);
            $("#<%=txtcontent.ClientID %>").val(text == "店铺" ? "请输入厂商名称" : "请输入商品名称").attr("value", text == "店铺" ? "请输入厂商名称" : "请输入商品名称");
        })

    });

    $(function () {

        $("#<%=txtcontent.ClientID %>").blur(function () {
            setTimeout(function () {
                $("#selectul").hide();
            }, 1000);
            
        })

        $("#<%=txtcontent.ClientID %>").click(function () {
            var type = "";
            type = $(".header .search .opt").attr("type") == "0" ? "comp" : "goods";
            $.ajax({
                type: 'POST',
                url: "../Handler/orderHandle.ashx",
                data: { ActionType: "Select", type: type },
                dataType: "text",
                cache: false,
                success: function (Data) {
                    if (Data != "") {
                        $("#selectul").show();
                        $("#selectul").html(Data);
                    }
                    else {
                        $("#selectul").hide();
                    }
                }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                    layerCommon.msg("删除失败", IconOption.错误);
                }
            });

        })
        $(document).on("click", "#selectul li p", function () {
            $("#<%=txtcontent.ClientID %>").val($.trim($(this).html()));
            $("#selectul").hide();
            $("#btn_SearCh").click();

        })
    })

    function checkpro() {

        var proname = $("#<%=txtcontent.ClientID %>").val();

        if (proname == '请输入厂商名称' || proname == '请输入商品名称') {
            proname = '';
        }

        var str = new RegExp("[()（）]")
        //var pattern = new RegExp("[\\\\\/%--`~!@#$^\\?=|{}'_:;',\\[\\].<>~！@#￥……——| {}【】‘；：”“'。，、？\"]")        //格式 RegExp("[在中间定义特殊过滤字符]")  
        var pattern = new RegExp("[\\\\\/%`~!@#$^\\?|';',<>~！@#￥|‘；”“'。，、？\"]")        //格式 RegExp("[在中间定义特殊过滤字符]")  

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
        proname = rs;

        if ($(".header .search .opt").attr("type") == "0") {
            location.href = "/comlist_0_" + proname + ".html";
        } else {
            location.href = "/goodslist_0_" + proname + ".html";
        }
    }

    //Enter键表单自动提交
    document.onkeydown = function (event) {
        var $_Serach = $("#btn_SearCh");
        var target, code, tag;
        if ($("#<%=txtcontent.ClientID %>", ".s-box").is(":focus")) {
            if (!event) {
                event = window.event; //针对ie浏览器  
                target = event.srcElement;
                code = event.keyCode;
                if (code == 13) {
                    $_Serach.trigger("click");
                    return false;
                }
            }
            else {
                target = event.target; //针对遵循w3c标准的浏览器，如Firefox  
                code = event.keyCode;
                if (code == 13) {
                    $_Serach.trigger("click");
                    return false;
                }
            }
        }
    }
</script>
<style>
    .leftsideBar .submenu .cur h2 a {
        color: #008be3;
    }
</style>
<%--<!--logo搜素 start-->
<div class="header">
  <div class="m-logo"> <a href='<%= ConfigurationManager.AppSettings["WebDomainName"].ToString() %>'> <img alt="www.my1818.com"  src="../images/logo2.0.png" width="247" height="42" /></a></div>

  <div class="search">
  	<ul class="title"><li class="hover" id="eshops">店铺</li><li id="shop">商品</li></ul>
    <div class="s-box">
      <input type="text" id="txtcontent" runat="server" autocomplete="off" class="box"  value="请输入厂商名称" onfocus="if(value==defaultValue){value=''}" onblur="if(!value){value=defaultValue}" maxlength="40" />
       <ul  id="selectul">
          
       </ul>
      <a id="btn_SearCh" href="#" class="btn sCompN" onclick="checkpro()">搜索</a> </div>
      <div class="hot">
		<a href="/subject/June/index.aspx" class="red" target="_blank">六月,年中惠报</a>
        <a href="/subject/apr/index.aspx" target="_blank">“愚”你同行</a>
        <a href="/subject/march/index.html" target="_blank">万千好货三月来袭</a>
      	<a href="/subject/february/index.html" target="_blank">2月初春上新</a>

      </div>
  </div>
  
  <div class="or-app"> <span> <img alt="下载医站通"  src="../images/app-qr.png" width="80" height="80" /></span><i>下载医站通</i></div>
</div>
<!--logo搜素 end--> --%>

<!--页头 start-->
<div class="header">
  <div class="m-logo"> <a href='<%= ConfigurationManager.AppSettings["WebDomainName"].ToString() %>'> <img alt="https://www.yibanmed.com/" src="/images/logo2.0.png"></a></div>
  <div class="search">
    <div class="s-box">
      <input type="text" id="txtcontent" runat="server" autocomplete="off" class="box"  value="请输入厂商名称" onfocus="if(value==defaultValue){value=''}" maxlength="40">
        <ul class="search-li" id="selectul">
       </ul>
        <a id="btn_SearCh" href="#" class="btn sCompN" onclick="checkpro()">搜索</a> 
      <div class="opt" type="0" id="opt" runat="server">店铺<i class="down-i"></i><div class="cur"><i>商品</i></div></div>
    </div>
    <div class="hot clear" runat="server" id="SelectValue">
    		
    </div>
  </div>
	
  <div class="or-app"> <span> <img alt="医站通公众号" src="images/wx-qr.png" width="80" height="80"></span><i>医站通公众号</i></div>
</div>
<!--页头 end-->


<!--主导航 start-->
<div class="nav">
  <div class="n">
    <div class="navmenu fl"> <i class="hy-i"></i>行业市场
     
      <!--左侧菜单 start-->
      <div  class="leftsideBar " style="color: #000;"  runat="server"  id="leftNav">
        <asp:Repeater runat="server" ID="rpt_Gtype">
            <ItemTemplate>
              <div class="submenu">
                <h3 class="t" indid="<%# Eval("TypeCode") %>"><a href="javascript:void(0);" class="bt"><i class="menu-i6"></i><%# Eval("TypeName")%></a></h3>
                <div class="cur">
                </div>
              </div>
          </ItemTemplate>
        </asp:Repeater>
          <%--<div class="submenu">
            <h3 class="t" indid="13"><a href="/comlist_4.html" class="bt"><i class="menu-i14"></i>消毒灭菌/冷用</a></h3>
            <div class="cur">
            </div>
          </div>
          <div class="submenu">
            <h3 class="t" indid="14"><a href="/comlist_5.html" class="bt"><i class="menu-i13"></i>体外循环/血液处理</a></h3>
            <div class="cur">
            </div>
          </div>
          <div class="submenu">
            <h3 class="t" indid="15"><a href="/comlist_6.html" class="bt"><i class="menu-i12"></i>康复理疗</a></h3>
            <div class="cur">
            </div>
          </div>
          <div class="submenu">
            <h3 class="t" indid="16"><a href="/comlist_7.html" class="bt"><i class="menu-i15"></i>口腔设备器具</a></h3>
            <div class="cur">
            </div>
          </div>
          <div class="submenu">
            <h3 class="t" indid="17"><a href="/comlist_8.html" class="bt"><i class="menu-i4"></i>手术急救/诊疗护理</a></h3>
           	<div class="cur">
            </div>
          </div>
          <div class="submenu">
            <h3 class="t" indid="18"><a href="/comlist_9.html" class="bt"><i class="menu-i1"></i>科室手术器械</a></h3>
           <div class="cur">
            </div>
          </div>
          <div class="submenu">
            <h3 class="t" indid="19"><a href="/comlist_10.html" class="bt"><i class="menu-i4"></i>普通诊察/注射穿刺</a></h3>
           	<div class="cur">
            </div>
          </div>
          <div class="submenu">
            <h3 class="t" indid="20"><a href="/comlist_11.html" class="bt"><i class="menu-i5"></i>中医器械</a></h3>
            <div class="cur">
            </div>
          </div>
          <div class="submenu">
            <h3 class="t" indid="21"><a href="/comlist_260.html" class="bt"><i class="menu-i5"></i>材料敷料/相关制品</a></h3>
            <div class="cur">
            </div>
          </div>
          <div class="submenu">
            <h3 class="t" indid="22"><a href="/comlist_261.html" class="bt"><i class="menu-i5"></i>软件/系统/其他</a></h3>
            <div class="cur">
            </div>
          </div>--%>
          <div class="sortbox_bg"></div>   
        </div>
      </div>
      <!--左侧菜单 end--> 
      
      <ul class="navitems fl">
          <li><a href='<%= ConfigurationManager.AppSettings["WebDomainName"].ToString() %>'>首页</a></li>
          <li><a href="/zslist.html">招商信息</a></li>
          <li><a href="/compnew.html">厂家列表</a></li>
          <li><a href="/platform.html">平台介绍</a></li>
          <li><a href="/news_1.html">新闻资讯</a></li>
		  <li><a href="/financial.html">金融服务</a></li>
          <li><a href="/help/help.html">帮助中心</a></li>
   	 </ul> 
    </div>
    <div class=" clear"></div>
</div>
<!--主导航 end-->


<!--主导航 end-->
<input type="hidden" id="showID" runat="server" />
