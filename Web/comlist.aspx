<%@ Page Language="C#" AutoEventWireup="true" CodeFile="comlist.aspx.cs" Inherits="ComList"  %>

<%@ Register Src="~/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<%@ Register Src="~/UserControl/NavTopControl.ascx" TagPrefix="uc1" TagName="TopNav" %>
<%@ Register Src="~/UserControl/BottomControl.ascx" TagPrefix="uc1" TagName="Bottom" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title><%= GetTiTle() %></title>
    <meta name="keywords" content="<%= keywords%>" />
    <meta name="description" content="<%=description %>" />
    <link href="css/global-2.0.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="js/CitysLine/json-array-of-province.js"></script>
    <script type="text/javascript" src="js/CitysLine/json-array-of-city2.js"></script>
    <script type="text/javascript" src="js/CitysLine/Provinces-select.js"></script>
    <script>
        $(function () {
            Province()
            $("#Select").click(function () {
                $("#GoodsName_").val($("#Select_Text").val())
            })
            $(".hy-value li").each(function (index, obj) {
                if (index != 0) {
                    if ($(".hy-value li").eq(index).attr("class") == "hover") {
                        $(".hy-value li").eq(0).removeClass("hover");
                        return false;
                    } else {
                        $(".hy-value li").eq(0).addClass("hover");
                    }
                }
            })
            $(".sort a").each(function (index, obj) {
                if (index != 0) {
                    if ($(".sort a").eq(index).attr("class") == "hover") {
                        $(".sort a").eq(0).removeClass("hover");
                        return false;
                    } else {
                        $(".sort a").eq(0).addClass("hover");
                    }
                }
            })
            $("#<%=TopNav1.gettxtClientID%>").attr("value", '<%=GetTxtcontent%>');
            if ($(".logo-mall .list li").length == 0) {
                $(".logo-mall .list").html("<li>暂无数据</li>");
            }
            $(".s-prev").click(function () {
                $("#Pager_List").children("a").eq(1)[0].click();
            })
            $(".s-next").click(function () {
                $("#Pager_List").children("a").each(function (index, item) {
                    if ($(item).html() == "下一页") {
                        $(item)[0].click();
                    }
                })
            })

            $("#Select_Text").focus(function () {
                $("#Select_Text").val("")
            }).blur(function () {
                
                if ($("#Select_Text").val() == "")
                {
                    $("#Select_Text").val("请输入店铺名称")
                }
            });
            $("body").delegate("#Top_Select", "click", function () {

                if (confirm("删除该条件 其他条件也会失效，是否继续")) {
                    window.location.href = "comlist_0_.html";
                }
               
            })

         //行业分类单击事件
            $(document).on("click", ".GtypeA", function () {
                var ID = $(this).attr("id");
                $("#GtypeHid").val(ID);
                $("#<%=GtypeBtn.ClientID%>").click();
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
    <form runat="server">
        <uc1:Top ID="top1" runat="server" />
        <!--顶部导航栏 end-->
        <uc1:TopNav ID="TopNav1" runat="server" />
        <div class="line2"></div>
       <div class="wrap">
   <!--条件导航  start-->
<a id="Titlt_close" runat="server" style="display:none;"  onserverclick="Titlt_close_ServerClick"></a>
<a id="Shengshi" runat="server" style="display:none;"  onserverclick="Unnamed_ServerClick"></a>
   <input type="hidden" runat="server" id="CategoryID_" />
   <input type="hidden" runat="server" id="AddName_" />
   <input type="hidden"  runat="server" id="GoodsName_" />
   <input type="hidden"  runat="server" id="Top_Select" />
   <input  type="hidden" id="A_Type" runat="server"   value="1"/>
   <input  type="hidden" id="CategoryID_type" runat="server"   value="1"/>
<div class="m-crumbs" runat="server" id="TitleDiv" ><i class="bt">全部结果</i>>
</div>
   <!--条件导航  end--><!--行业分类 start-->
<div class="m_selector">
	<div class="hy-key"><i>行业分类：</i></div>
	<ul class="hy-value">
    	<li class="hover"><a href="#" class="GtypeA" id="0">全部</a></li>
        <asp:Repeater ID="RepList" runat="server">
                    <ItemTemplate>
                        <li class="<%#Eval("ID").ToString() == indid?"hover":""%>"><a class="GtypeA" id="<%#Eval("ID")%>" href="#">
                            <%#Eval("TypeName")%></a></li>
                    </ItemTemplate>
                </asp:Repeater>
    </ul>
</div>
<!--行业分类 end-->

<!--筛选条件 start-->
 
<div class="m-filter">
	<div class="sort fl">
        <a id="A_sort" href="javascript:;" runat="server" onserverclick="A_sort_ServerClick">精品</a>
        <a id="A_Nes" runat="server" href="javascript:;" onserverclick="A_Nes_ServerClick">最新</a>
       </div>
    <div class="area fl">
    	所在地<div class="text text_select"  onclick="textselect()"><%= AddNameWeb %><i class="arrow" ></i></div>
        <input  id="text_select" runat="server" type="hidden"/>
    	<div id="SelectDiv" class="select" style="display:none">
        	<a href="javascript:;" class="close" onclick="textselect()"></a>
        	<ul class="title">
             <li class="Province_btn hover" onclick="Province()"><%= AddNameWeb %><i class="arrow"></i></li>
            <%--<li class="Citys_btn " onclick="Citys_()">请选择<i class="arrow"></i></li>--%></ul>
            <ul class="list Province_Citys_list"> </ul>
        </div>
    </div>
    <div class="search fl"><input name="Select_Text" id="Select_Text"  type="text" class="box" value="<%=GoodsNameWeb %>"/><a href="javascript:;" id="Select" runat="server" onserverclick="Select_ServerClick"  class="btn">确定</a></div>

    <div class="pager fr">
    	<span class="text"><b runat="server" id="Page_index">1</b>/<b id="Page_Count" runat="server"></b></span>
        <a class="s-prev" href="javascript:;"><</a><a class="s-next" href="javascript:;">></a>
    </div>
    <div class="result fr"><b runat="server" id="Count_goods"></b>家店铺</div>
</div>
<!--筛选条件 end-->

<!--商品列表 start-->
<ul class="m-compa">
    <asp:Repeater ID="RepComp" runat="server" OnItemDataBound="RepComp_ItemDataBound">
        <ItemTemplate>  
      
            <li class="li-compa"  runat="server" >
    	<div class="logo fl">
        <a href="/<%#Eval("ID")%>.html" class="pic" target="_blank" title='<%#Eval("CompName") %>'>
       <%# ShowComImg(Eval("ShopLogo").ToString(), Eval("CompLogo").ToString(), Eval("CompName").ToString(), Eval("ShortName").ToString())%></a>
       <a href="/<%#Eval("ID")%>.html"target="_blank" class="t"><%#Eval("CompName")%></a></div>
        <ul class="text fl">
        	<li><i class="bt">主营范围：</i><%# ManageInfoSubstring(Eval("ManageInfo").ToString()) %></li>
        	<li><i class="bt">地　　址：</i><%# islogUser == true ? Eval("Address") : "&nbsp;"%></li>
            <li><i class="bt">联 系 人：</i><%# islogUser == true ? Eval("Principal") : "&nbsp;"%></li>
            <li><i class="bt">联系电话：</i><%# islogUser == true ? Eval("Phone") : "&nbsp;"%></li>
        </ul>
        <div class="goods fr">
        	<div class="qrCode"><div class="pic">
                <a href="/<%#Eval("ID")%>.html"target="_blank">
        <img width="140" height="140" alt="店铺二维码" src="<%# ConfigurationManager.AppSettings["ImgViewPath"] + "CompImg/c" + Eval("ID") + ".png" %>" onerror="this.src='/images/Goods400x400.jpg'"/>
                </a>扫描转发，分享店铺<i class="fh"></i></div></div>
            <div class="line"></div>
        	<ul class="list">         
               <asp:Repeater ID="GoodsList" runat="server">
               <ItemTemplate>
               <li><a target="_blank" href="/e<%#Eval("ID")%>_<%#Eval("CompID")%>.html">
               <img alt="暂无图片" width="110px" height="110px" src="<%# ResolveUrl(Common.GetPicURL(Eval("Pic2").ToString(),"3")) %>"  onerror="this.src='/images/Goods400x400.jpg'"/></a></span>
               <a href="/e<%#Eval("ID")%>_<%#Eval("CompID")%>.html" target="_blank"><%# GoodsNameSubstring(Eval("GoodsName").ToString()) %></a></li>
               </ItemTemplate>              
               </asp:Repeater>
            </ul>
        </div>
    </li>    
        </ItemTemplate>
    </asp:Repeater>
	
	
</ul>
<!--商品列表 end-->
<!--[if !IE]>分页 start<![endif]-->
             <div class="blank10"></div>
        <webdiyer:AspNetPager ID="Pager_List" runat="server" EnableTheming="true"  
            FirstLastButtonClass="tf" FirstPageText="首页" LastPageText="尾页" NextPageText="下一页"
            NextPrevButtonClass="tf" PageSize="7" PrevPageText="上一页" AlwaysShow="True" UrlPaging="false"
            MoreButtonClass="tf" PagingButtonClass="tf" CssClass="page" CurrentPageButtonClass="cur" NumericButtonCount="3"
            OnPageChanged="PagerList_PageChanged"  TextBeforePageIndexBox="跳转到"
            ShowInputBox="Always" ShowBoxThreshold="1"   SubmitButtonText="跳转" SubmitButtonClass="tf">
        </webdiyer:AspNetPager>

        <div class="blank10">
        </div>
        <!--[if !IE]>分页 end<![endif]-->
   
</div>

        <!--页尾 start-->
        <uc1:Bottom ID="Bottom1" runat="server" />
        <script src="js/layer/layer.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
        <script src="js/layerCommon.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
        <!--页尾 end-->

      <input  type="hidden" id="GtypeHid" runat="server" value="0"/>
     <asp:Button ID="GtypeBtn" runat="server" Text="Button" style="display:none;" OnClick="GtypeBtn_Click"/>
    </form>
</body>
</html>
