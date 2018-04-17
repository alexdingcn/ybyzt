<%@ Page Language="C#" AutoEventWireup="true" CodeFile="m_productsview.aspx.cs" Inherits="productsview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Cache-Control" content="no-transform" />
    <meta http-equiv="Cache-Control" content="no-siteapp" />
    <meta charset="UTF-8" />
    <!-- Sets initial viewport load and disables zooming  -->
    <meta name="viewport" content="initial-scale=1, maximum-scale=1" />

    <!-- Makes your prototype chrome-less once bookmarked to your phone's home screen -->
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />

    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="renderer" content="webkit" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title><%= GetTitle() %></title>
    <meta name="keywords" runat="server" id="mKeyword" />

    <!-- Include the compiled Ratchet CSS -->
    <link href="/eshop/ratchet/css/ratchet.min.css" type="text/css" rel="stylesheet" />
    <script src="/eshop/ratchet/js/ratchet.min.js" type="text/javascript"></script>
    <style type="text/css">
        .content {
            padding: 5px;
        }
        button {outline: 0;}
        img {
            width: 100%;
        }
        .bar-tab a.front {background-color: #648bc6; color: white;}
        .bar-tab a.allpro {background-color: #ed505b; color: white; }
        .bar-tab a.company {background-color: #ff6a00; color: white; }
    </style>
</head>

<body class="root">

    <header class="bar bar-nav">
      <button class="btn btn-link btn-nav pull-left" onclick="history.go(-1); return false;">
        <span class="icon icon-left-nav"></span>
        返回
      </button>
        <!--
      <button class="btn btn-link pull-right">
        关闭
      </button>
        -->
      <h1 class="title"><%= GetShortTitle() %></h1>
    </header>

    <!-- Wrap all non-bar HTML in the .content div (this is actually what scrolls) -->
    <div class="content">
        <form id="form2" runat="server">

            <input type="hidden" runat="server" id="hidycId" />
            <input type="hidden" runat="server" id="hidCompId" />
            <input type="hidden" id="hid_Alert" />
            <input type="hidden" id="hidGoodsInfoId" runat="server" />

            <div class="slider" id="mySlider">
                <div class="slide-group">
                    <div id="DefaultProductImg" runat="server" class="slide">
                        <img src="../images/Goods400x400.jpg" runat="server" alt="暂无图片" id="defaultImg" />
                    </div>
                    <asp:Repeater ID="rptImg" runat="server">
                        <ItemTemplate>
                            <div class="slide">
                                <img src="<%# Eval("pic2").ToString()=="X" || Eval("pic2").ToString()==""?"../images/Goods400x400.jpg": Common.GetWebConfigKey("ImgViewPath") + "/GoodsImg/"+ Eval("pic2") %>" alt="">
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>

            <div class="content-padded">
                <h4 id="lblGoodsName" runat="server"></h4>
                <h4 id="lblGoodsTitle" runat="server"></h4>
                <h3><%= hideInfo1 %></h3>
                <h3><%= hideInfo2 %></h3>
            </div>

            <div class="product-desc">

                    <p>
                        <div id="litAttrVaue" runat="server"></div>
                    </p>
                    <p >
                        <div class="control-input label fl" id="DivLabel" runat="server"></div>
                    </p>

                    <p >
                        <span>单位: </span><span id="lblunit" runat="server"></span>
                    </p>
                    <p style='<%= OrderInfoType.rdoOrderAudit("商品是否启用库存",compId)=="0"?"": "display:none" %>'>
                        <span>库存: </span><span id="lblinventory" runat="server"></span>
                    </p>
                    <p >
                        <span>价格: </span><b class="red" id="lblPrice" runat="server"></b>
                    </p>
                    <p class="table-view-cell">
                        <span id="YuanPrice" runat="server"></span>
                    </p>
    
            </div>


            <div class="segmented-control">
                <a class="control-item active" href="#productDesc">商品介绍</a>
                <a class="control-item" href="#productCert">注册证</a>
            </div>
            <div class="card">
                <div id="productDesc" class="control-content active" runat="server">
                    <p style="padding-top: 20px; line-height: 40px; padding-left: 20px">
                        暂无数据
                    </p>
                </div>
                <div id="productCert" class="control-content" runat="server"></div>
            </div>
            <div id="litZiDingYi" runat="server"></div>
        </form>
        <p style="margin-bottom:4rem"></p>
    </div>
 
    <nav class="bar bar-tab">
          <a class="tab-item front" href="/<%=Request["Comid"] %>.html">
            店铺首页
          </a>
          <a class="tab-item allpro" href="/<%=Request["Comid"] %>.html#allPro">
            全部商品
          </a>
          <a class="tab-item company" href="/eshop/about_<%=Request["Comid"] %>.html">
            公司介绍
          </a>
    </nav>

</body>

</html>
