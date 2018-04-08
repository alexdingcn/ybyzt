<%@ Page Language="C#" AutoEventWireup="true" CodeFile="goodslist.aspx.cs" Inherits="goodslist" %>

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
    <meta name="description" content="<%= description%>" />
    <link href="css/global-2.0.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="js/CitysLine/json-array-of-city2.js"></script>
    <script type="text/javascript" src="js/CitysLine/json-array-of-province.js"></script>
    <script type="text/javascript" src="js/CitysLine/Provinces-select.js"></script>
    <%--<script>
        var _hmt = _hmt || [];
        (function () {
            var hm = document.createElement("script");
            hm.src = "https://hm.baidu.com/hm.js?779e9b3d086d94ec0ead28ec3dd99190";
            var s = document.getElementsByTagName("script")[0];
            s.parentNode.insertBefore(hm, s);
        })();
    </script>--%>
    <script type="text/javascript">
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
            $(".sort a").each(function (index,obj) {
                if (index != 0) {
                    if ($(".sort a").eq(index).attr("class") == "hover") {
                        $(".sort a").eq(0).removeClass("hover");
                        return false;
                    } else {
                        $(".sort a").eq(0).addClass("hover");
                    }
                }
            })
            if ($(".logo-mall .list li").length == 0) {
                $(".logo-mall .list").html("<li>暂无数据</li>");
            }
           
            $("#eshops").attr("class", ""); $("#shop").attr("class", "hover");
            

            $("#<%=TopNav1.gettxtClientID%>").attr("value", '<%=GetTxtcontent%>');
       
            $(".fl a").on("click", function () {
                $(".fl a").removeClass("hover");
                $(this).addClass("hover");
            })
            $(".s-prev").click(function () {
                $("#Pager_List").children("a").eq(1)[0].click();
            })
            $(".s-next").click(function () {
                $("#Pager_List").children("a").each(function (index, item) {
                    if ($(item).html() == "下一页")
                    {
                        $(item)[0].click();
                    }
                })
            })
            $("#Select_Text").focus(function () {
                $("#Select_Text").val("")
            }).blur(function () {

                if ($("#Select_Text").val() == "") {
                    $("#Select_Text").val("请输入商品名称")
                }
            });

            $("body").delegate("#Top_Select", "click", function () {

                if (confirm("删除该条件 其他条件也会失效，是否继续")) {
                    window.location.href = "goodslist.html";
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
</head>
<body>
 <form id="form1" runat="server">
    <uc1:Top ID="top1" runat="server" />
    <!--顶部导航栏 end-->
    <uc1:TopNav ID="TopNav1" runat="server"  />
    
<div class="line2"></div>
<div class="wrap">
   <!--条件导航  start-->
<a id="Titlt_close" runat="server" style="display:none;"  onserverclick="Titlt_close_ServerClick"></a>
<a id="Shengshi" runat="server" style="display:none;"  onserverclick="Unnamed_ServerClick"></a>
   <input type="hidden" runat="server" id="CategoryID_" />
   <input type="hidden" runat="server" id="AddName_" />
   <input type="hidden"  runat="server" id="GoodsName_" />
   <input  type="hidden" id="A_Type" runat="server"   value="0"/>
   <input  type="hidden" id="CategoryID_type" runat="server"   value="0"/>
<div class="m-crumbs" runat="server" id="TitleDiv" ><i class="bt">全部结果</i>>
</div>
   <!--条件导航  end-->
<!--行业分类 start-->
<div class="m_selector">
	<div class="hy-key"><i>行业分类</i></div>
	<ul class="hy-value">
    	<li class="<%= CategoryID=="0"?"hover":""%>"><a class="GtypeA" id="0" href="javascript:void(0)" title="全部商品">全部</a></li>
        <asp:Repeater ID="RepList" runat="server">
                <ItemTemplate>
                    <li class="<%#Eval("ID").ToString() == CategoryID?"hover":CategoryID%>"><a class="GtypeA" id="<%#Eval("ID")%>" href="javascript:void(0)">
                        <%#Eval("TypeName")%></a></li>
                </ItemTemplate>
       </asp:Repeater>
    </ul>
</div>
<div class="m_selector mi-sele" runat="server" id="Gtypediv2"> 
	<div class="hy-key" runat="server" id="GtypeName2"><i></i></div>
	<ul class="hy-value">
    	<li class="<%= CategoryID2=="0"?"hover":""%>"><a class="GtypeA" id="<%=CategoryID %>" href="javascript:void(0)" title="全部商品">全部</a></li>
        <asp:Repeater ID="Gtype2" runat="server">
                <ItemTemplate>
                    <li class="<%#Eval("ID").ToString() == CategoryID2?"hover":""%>"><a class="GtypeA" id="<%#Eval("ID")%>" href="javascript:void(0)">
                        <%#Eval("TypeName")%></a></li>
                </ItemTemplate>
       </asp:Repeater>
    </ul>
</div>
    <div class="m_selector mi-sele" runat="server" id="Gtypediv3">
	<div class="hy-key" runat="server" id="GtypeName3"><i></i></div>
	<ul class="hy-value">
    	<li class="<%= CategoryID3=="0"?"hover":""%>"><a class="GtypeA" id="<%=CategoryID2 %>" href="javascript:void(0)" title="全部商品">全部</a></li>
        <asp:Repeater ID="Gtype3" runat="server">
                <ItemTemplate>
                    <li class="<%#Eval("ID").ToString() == CategoryID3?"hover":""%>"><a class="GtypeA" id="<%#Eval("ID")%>" href="javascript:void(0)">
                        <%#Eval("TypeName")%></a></li>
                </ItemTemplate>
       </asp:Repeater>
    </ul>
</div>
<!--行业分类 end-->

<!--筛选条件 start-->
<div class="m-filter">
	<div class="sort fl">
        <a id="A_sort" href="javascript:;" runat="server" onserverclick="A_sort_ServerClick">综合排序</a>
        <a id="A_ShopCount" runat="server" href="javascript:;" onserverclick="A_ShopCount_ServerClick" >销量</a>
        <a id="A_Nes" runat="server" href="javascript:;" onserverclick="A_Nes_ServerClick">最新</a>
        <a id="A_cshop" runat="server" href="javascript:;" onserverclick="A_cshop_ServerClick">促销</a></div>
    <div class="area fl">
    	所在地<div class="text text_select"  onclick="textselect()"><%= AddNameWeb %><i class="arrow" ></i></div>
        <input  id="text_select" runat="server" type="hidden"/>
    	<div id="SelectDiv" class="select" style="display:none">
        	<a href="javascript:;" class="close" onclick="textselect()"></a>
        	<ul class="title">
             <li class="Province_btn hover" onclick="Province()">北京市<i class="arrow"></i></li>
            <li class="Citys_btn " onclick="Citys_()">请选择<i class="arrow"></i></li></ul>
            <ul class="list Province_Citys_list"> </ul>
        </div>
    </div>
    <div class="search fl"><input name="Select_Text" id="Select_Text"  type="text" class="box" value="<%= GoodsNameWeb %>"/><a href="javascript:;" id="Select" runat="server" onserverclick="Select_ServerClick"  class="btn">确定</a></div>

    <div class="pager fr">
    	<span class="text"><b runat="server" id="Page_index">1</b>/<b id="Page_Count" runat="server"></b></span>
        <a class="s-prev" href="javascript:;"><</a><a class="s-next" href="javascript:;">></a>
    </div>
    <div class="result fr"><b runat="server" id="Count_goods"></b>件商品</div>
</div>
<!--筛选条件 end-->

<!--商品列表 start-->
<ul class="m-goods">
       <asp:Repeater ID="Rpt_Goods" runat="server">
         <ItemTemplate>
         <li class="g-li">
		<div class="pic"><a target="_blank" href="/e<%#Eval("ID")%>_<%#Eval("CompID")%>.html">
        <img alt="暂无图片" width="190px" height="190px" src="<%# ResolveUrl(Common.GetPicURL(Eval("Pic2").ToString(),"3")) %>" onerror="this.src='/images/Goods400x400.jpg'"/></a></div>
		<div class="title"><a target="_blank" title='<%#Eval("GoodsName") %>' href='/e<%#Eval("ID")%>_<%#Eval("CompID")%>.html'> <%#Eval("GoodsName") %></a></div>
		<div class="name"><a target="_blank" href="/<%#Eval("CompID")%>.html"><i class="hxBule"></i><%# Eval("CompName") %></a></div>
        <a class="exp" target="_blank" title='<%#Eval("GoodsName") %>' href='/e<%#Eval("ID")%>_<%#Eval("CompID")%>.html'>
           <div class="bg"></div>
           <ul><li>主营范围：<%# ManageInfoSubstring(Eval("ManageInfo").ToString()) %></li><li>所在地区：<%# Eval("Address") %></li><li>联 系 人 ：<%# Eval("Principal") %></li><li>联系电话：<%# Eval("Phone") %></li></ul>
        </a>
        </li>
       </ItemTemplate>
        </asp:Repeater>
</ul>
 <!--[if !IE]>分页 start<![endif]-->
             <div class="blank10"></div>
        <webdiyer:AspNetPager ID="Pager_List" runat="server" EnableTheming="true"  
            FirstLastButtonClass="tf" FirstPageText="首页" LastPageText="尾页" NextPageText="下一页"
            NextPrevButtonClass="tf" PageSize="20" PrevPageText="上一页" AlwaysShow="true" UrlPaging="false"
            MoreButtonClass="tf" PagingButtonClass="tf" CssClass="page" CurrentPageButtonClass="cur" NumericButtonCount="3"
            OnPageChanged="PagerList_PageChanged" TextBeforePageIndexBox="跳转到"
            ShowInputBox="Always" ShowBoxThreshold="1"   SubmitButtonText="跳转" SubmitButtonClass="tf">
        </webdiyer:AspNetPager>
                 
        <div class="blank10">
        </div>
        <!--[if !IE]>分页 end<![endif]-->
</div>
    <%--页尾 start--%>
    <uc1:Bottom ID="Bottom1" runat="server" />
<script src="js/layer/layer.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
<script src="js/layerCommon.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
    <!--页尾 end-->
     <input  type="hidden" id="GtypeHid" runat="server" value="0"/>
     <asp:Button ID="GtypeBtn" runat="server" Text="Button" style="display:none;" OnClick="GtypeBtn_Click"/>
     </form>
</body>
</html>
