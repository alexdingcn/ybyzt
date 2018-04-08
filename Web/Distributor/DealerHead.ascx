<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DealerHead.ascx.cs" Inherits="Distributor_DealerHead" %>
<script>
    $(document).ready(function () {
        //初始化高度  
        $(".wlik").height($(window).height() - 65);
        //当文档窗口发生改变时 触发  
        $(window).resize(function () {
            $(".wlik").height($(window).height() - 65);
        });

        //商品Info页面
        $(document).on("click", ".GoGoodsInfo", function () {
            var GoodsID = $(this).attr("goods_tip");
            var GoodsInfoID=$(this).attr("tip");
            $(".aGoodsInfo").attr("target", "_blank");
            $(".aGoodsInfo").attr("href", "/Distributor/GoodsInfo.aspx?goodsId=" + GoodsID + "&goodsInfoId=" + GoodsInfoID); //edit by hgh
            $(".aGoodsInfo").html("<span>商品信息</span>")
            $(".aGoodsInfo").find("span").click();
        });
    });
</script>
<div class="topBar">
<div class="nr">
	<div class="logo">
        <span><img src='<%= bol==true?logo:"/Distributor/images/al-logo.png" %>' width="94"/></span>
        <i id="disName"><a target="_blank" href='javascript:;'><%=dis == null ? "" : dis.DisName %></a></i>
    </div>
    <ul class="right topNav">
        <li><a href="<%=ResolveUrl("../Distributor/Shop.aspx") %>"><i class="shop-icon" id="Top_CartImg"></i>购物车</a><b class="number" id="Top_CartNum" runat="server">0</b>
            <a class="aGoodsInfo" style=" display:none;"></a>
            <div class="tgnCart" id="tgnCar" runat="server">
            </div>
        </li>
        <li class="name"><a href="/Distributor/UserIndex.aspx"><i class="me-icon"></i><%=ShowName%><i class="triangle"></i></a>
            <span class="tgn">
                <a href="/Distributor/UserEdit.aspx">基本信息</a>
                <a href="/Distributor/UserPWDEdit.aspx">修改密码</a>
                <a href="/Distributor/MessAgeList.aspx">我要咨询</a>
                <a href="javascript:void(0);" runat="server" onserverclick="QuitLogin_Click">退出</a>
            </span>
        </li>
        <li style='<%= bol==true?"display:none": ""%>'><a href="/Distributor/CompNewList.aspx"><i class="xx-icon"></i>消息</a></li>
        <li id="eDis"  style='<%= bol==true?"display:none": ""%>' ><a target="_blank" href="/<%=logUser.CompID%>.html"><i class="dp-icon"></i>店铺</a></li>
        <li style='<%= bol==true?"display:none": ""%>'><a target="_blank" href='<%= ConfigurationManager.AppSettings["WebDomainName"].ToString() %>'><i class="home-icon"></i>首页</a></li>
        <li><a href="javascript:void(0);" runat="server" onserverclick="QuitLogin_Click">退出</a></li>
    </ul>
</div>

</div>
<script type="text/javascript" src="<%=ResolveUrl("../Distributor/js/resolutionDis.js") %>"></script>

