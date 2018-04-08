<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GoodsList.aspx.cs" Inherits="Distributor_GoodsList" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv = "X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>商品列表信息</title>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link href="../eshop/css/goods.css" rel="stylesheet" type="text/css" />
    <link href="../Company/css/orderGoods.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.9.1.min.js" type="text/javascript"></script>
                    <script>
        $(function(){
         var type='<%=cxtype%>';
            if(type=="cx"){
                //判断是否选中
                if ($(".back1 ul li").find("a").eq(0).attr("class") == "k-icon") {
                    $(".back1 ul li").find("a").eq(0).attr("class", "k-icon2"); //选中
                } else {
                    $(".back1 ul li").find("a").eq(0).attr("class", "k-icon"); //取消选中
                }
                //SaleFunction.GetSalePaginData();
            }
        })
    </script>

    <script src="../js/layer/layer.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"
        type="text/javascript"></script>
    <script src="../js/layerCommon.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"
        type="text/javascript"></script>
    <script src="../js/MyClassPaging.js" type="text/javascript"></script>
    <script src="js/goodslist.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"
        type="text/javascript"></script>
    <style>
        .none
        {
            display: none;
        }
        .po-bg2
        {
            bottom: 0;
            left: 0;
            opacity: 0.5;
            position: fixed;
            right: 0;
            top: 0;
            z-index: 998;
        }
        .p-delete2
        {
            height: 250px;
            left: 50%;
            position: fixed;
            top: 50%;
            width: 400px;
        }
        .popup2
        {
            border-radius: 5px;
            margin: 0 auto;
            z-index: 999;
        }
    </style>
</head>
<body class="root3">
    <form id="form1" runat="server">
    <input type="hidden" id="hidCompId" class="hidCompId" runat="server" />
    <input type="hidden" id="hidDisId" class="hidDisId" runat="server" />
    <input type="hidden" id="hidsDigits" class="hidsDigits" runat="server" />
    <input type="hidden" id="hidFlie" class="hidFlie" runat="server" />
    <input type="hidden" id="hidIsInve" class="hidIsInve" runat="server" />
    <input type="hidden" id="hidShouc" class="hidShouc" runat="server" />
    <input type="hidden" id="iskeep" class="iskeep" runat="server" value="0" />
    <input type="hidden" id="isaddCart" class="isaddCart" runat="server" value="0" />

    <Head:Head ID="Head2" runat="server" />
    <div class="rightCon" style="overflow: visible;">
        <Left:Left ID="Left1" runat="server" ShowID="nav-2" />
        <div class="info">
            <a id="navigation1" href="UserIndex.aspx">我的桌面</a>> <a id="navigation2" href="javascript:;"
                class="cur">商品列表</a>
        </div>
        <!--[if !IE]>商品功能区 start<![endif]-->
        <div class="goods-gn back1">
            <div class="userFun left">
                <label class="head">选择厂商：</label>
                <select id="ddrComp" name="" style=" width:150px; margin-right: 10px;" runat="server" class="xl"></select>
            </div>
            <div class="s left">
                <input name="" type="text" placeholder="商品编码/名称搜索" class="box txtGoods" /><a href="javascript:;"
                    class="searchBtn"></a></div>
            <ul class="fn left">
                <li><a href="javascript:;" class="k-icon"></a><a href="javascript:;">促销商品</a></li>
            </ul>
            <div class="fy right">
                <a href="javascript:;" class="s-icon xy-icon"></a><a href="javascript:;" class="x-icon yy-icon">
                </a>
            </div>
            <div class="bl right">
                <a href="javascript:;" class="dt-icon hover" title="大图"></a><a href="javascript:;"
                    class="lb-icon" title="列表"></a>
            </div>
            <div class=" clear">
            </div>
        </div>
        <!--[if !IE]>商品功能区 end<![endif]-->
        <!--[if !IE]>大图商品展示区 start<![endif]-->
        <div class="goods-zs back">
            <!--[if !IE]>商品展示 start<![endif]-->
            <ul class="goods-li">
            </ul>
            <!--[if !IE]>商品展示 end<![endif]-->
        </div>
        <!--[if !IE]>大图商品展示区 end<![endif]-->
        <!--[if !IE]>列表商品展示区 start<![endif]-->
        <div class="goods-zs back3" style="display: none;">
            <div class="tabNr-box">
                <table border="0" cellspacing="0" cellpadding="0" class="tabNr">
                    <thead>
                        <tr>
                            <th class="t7">
                                商品
                            </th>
                            <th class="">
                                商品规格
                            </th>
                            <th class="t3" style='<%= IsInve==0?"": "display:none"  %>'>
                                商品库存
                            </th>
                            <th class="t3">
                                价格（元）
                            </th>
                            <th class="t3">
                                操作
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
        </div>
        <!--[if !IE]>列表商品展示区 end<![endif]-->
        <div class="blank10">
        </div>
        <!--[if !IE]>分页 start<![endif]-->
        <div class="page1 paging">
        </div>
        <!--[if !IE]>分页 end<![endif]-->
    </div>
    <div class="po-bg2 none" style="z-index: 999999; background: #fffff">
    </div>
    <div id="p-delete" class="popup2 p-delete2 none" style="z-index: 9999999">
        <img src="/js/layer/skin/default/loading-0.gif" alt="" />
    </div>
    </form>
</body>
</html>
