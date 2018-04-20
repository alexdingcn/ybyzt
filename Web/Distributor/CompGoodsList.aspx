<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompGoodsList.aspx.cs" Inherits="Distributor_CompGoodsList" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/TreeDemo.ascx" TagPrefix="uc1" TagName="TreeDemo" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>商品列表信息</title>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link href="../Company/css/orderGoods.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Company/js/jquery-1.11.1.min.js"></script>
    <script type="text/javascript" src="../js/CommonJs.js"></script>
    <script src="../js/layer/layer.js" type="text/javascript"></script>
    <script src="../js/layerCommon.js" type="text/javascript"></script>
    <script src="../js/shopcart.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $(".txt_product_class").addClass("box");
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("input[type='text']").blur();
                    location.href = $("#A1").attr("href");
                }
            });

            //商品促销提示
            $(document).on({
                "mouseover":function(e){
                    var proID=$(this).attr("tip");
                    var pro_type=$(this).attr("tip_type");
                    var th=this;
                    $.ajax({
                        type: 'post',
                        url: '../../Handler/GoodsInfoPrice.ashx?action=Goodspro',
                        data: { GoodsID: proID,CompId:<%=this.CompID %> },
                        async: false, //true:同步 false:异步
                        success: function (result) {
                            var data = eval('(' + result + ')');
                            var falg=data["falg"];
                            if(falg=="True"){
                                var lb=data["TheLabel"];
                                $(th).append(lb);
                                $(th).find("i").css({"top":"60px"});
                            }
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            layerCommon.msg("服务器异常，请稍后再试",IconOption.哭脸);
                        }
                    });
                },
                "mouseout":function(e){
                    $(this).find("i").remove();
                } 
            },".sale");

            $(".list tbody tr").each(function () {
                var Id = $(this).find("td:eq(2)").attr("id");
                var ht = $(this).find("td:eq(2)").find("div[class!=\"attr\"]").text();
                if (ht != "") {
                    if (ht.length > 30) {
                        $(this).find("td:eq(2)").find("div[class!=\"attr\"]").attr("title", ht);
                        ht = ht.substr(0, 30) + "...";
                        $(this).find("td:eq(2)").find("div[class!=\"attr\"]").html(ht);
                    }
                } else {
                    $(this).find("td:eq(2)").find("div[class=\"attr\"]").css({ "line-height": "20px", "cursor": "pointer" });
                }
            });
        })

        //根据商品基础ID和描述信息查询GoodsInfoID和商品价格
        function onAttrGoods(id, attr, attrvalue,i) {
            var falg="";   //商品是否存在
            var goodsinfoid;  //商品信息Id
            var TinkerPrice;  //代理商 商品价格
            var DisID=<%=DisID %>;
            var CompID=<%=CompID %>;
            var IsInve=<%=IsInve %>; //是否启用商品库存，默认0、启用库存

            $("#" + id + "_" + attr).find("a").removeClass("a");
            $("#" + id + "_" + i).addClass("a");

            var val = '';
            var val1='';
            $("#" + id + " a").each(function (index, data) {
                if ($(this).attr("class") == "a") {
                    var aa = $(this).parent().find("span").text();
                    val += aa + $(this).text() + "；";
                    val1=aa + $(this).text() + "；"+val1;
                }
            });

            $.ajax({
                type: 'post',
                url: '../Handler/GoodsInfoPrice.ashx?action=attr',
                data: { Id: id, attrvalue: val,attrvalue1: val1,DisID:DisID,CompID:CompID },
                async: false, //true:同步 false:异步
                success: function (result) {
                    var data = eval('(' + result + ')');
                    falg=data["falg"];
                    goodsinfoid = data["goodsinfoid"];
                    TinkerPrice = data["TinkerPrice"];
                    if(goodsinfoid!=0){
                          $("#tr_"+id).find("td:first").find("a[class=\"view\"]").attr("href","../e"+goodsinfoid+"_"+CompID+"_.html");
                        if($("#tr_"+id).find("td:first").find("a[class=\"view\"]").siblings("i:not(.sale)").length>0)
                            $("#tr_"+id).find("td:first").find("a[class=\"view\"]").siblings("i:not(.sale)").remove();
                    }else{
                        if($("#tr_"+id).find("td:first").find("a[class=\"view\"]").siblings("i:not(.sale)").length<=0)
                            $("#tr_"+id).find("td:first").find("a[class=\"view\"]").after("&nbsp;<i style=\"color:red;\">已下架</i>");
                    }

                    $("#tr_"+id).find("td:first").find("input[type=hidden]").val(goodsinfoid);
                    $("#"+id+"_TinkerPrice").text(TinkerPrice);

                    if(IsInve==0){
                        if(parseFloat(data["Inventory"])<=0){
                                $("#tr_Inve_"+id).text("0.00");  //商品库存
                            $("#tr_"+id).find("td:first").find("input[type=checkbox]").attr("disabled","disabled");
                        }else{
                                $("#tr_Inve_"+id).text(parseFloat(data["Inventory"]).toFixed(2));  //商品库存
                                $("#tr_"+id).find("td:first").find("input[type=checkbox]").removeAttr("disabled");
                        }
                    }

                    if(data["BarCode"].toString()!="")
                        $("#tr_"+id).find("td").find("label[class=\"pCode\"]").text(data["BarCode"]);

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    layerCommon.msg("服务器异常，请稍后再试",IconOption.哭脸);
                }
            });
        }
    </script>
</head>
<body class="root3">
    <form id="form1" runat="server">
    <Head:Head ID="Head2" runat="server" />
    <div class="w1200">
        <Left:Left ID="Left1" runat="server" ShowID="nav-2" />
        <div class="rightCon">
            <div class="info">
                <a id="navigation1" href="UserIndex.aspx">我的桌面</a>> <a id="navigation2" href="CompGoodsList.aspx"
                    class="cur">商品列表</a>
            </div>
            <div class="userFun2">
                <div class="right">
                    <ul class="term">
                        <li>
                            <label class="head">
                                商品名称：</label>
                            <input name="txtGoodsName" type="text" id="txtGoodsName" runat="server" class="box txtGoodsName"
                                style="width: 110px;" /></li>
                        <li>
                            <label class="head">
                                商品分类：</label>
                            <uc1:TreeDemo runat="server" ID="txtCategory" />
                        </li>
                        <li>
                            <!--<label class="head">
                                每页</label><input name="txtPager" type="text" class="box3 txtPageSize" id="txtPager"
                                    runat="server" value="12" onkeyup="KeyInt(this);" onblur="KeyInt(this);" /><label
                                        class="head">条</label>-->
                        </li>
                    </ul>
                    <a id="A1" href="" onserverclick="A_Seek" runat="server" class="btnBl"><i class="searchIcon">
                    </i>搜索</a><a href="javascript:;" onclick="javascript:location.href='CompGoodsList.aspx'"
                        class="btnBl"><i class="resetIcon"></i>重置</a>
                </div>
            </div>
            <div class="blank10">
            </div>
            <div class="orderNr">
                <table class="PublicList list" id="orderBg" border="0" cellspacing="0" cellpadding="0">
                    <thead>
                        <tr>
                            <th class="t6">
                                商品名称
                            </th>
                            <th>
                                商品描述
                            </th>
                            <th style='<%= IsInve==0?"": "display:none"  %>'>
                                商品库存
                            </th>
                            <th class="t2">
                                价格（元）
                            </th>
                            <th class="t5">
                                操作
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptProList" runat="server">
                            <ItemTemplate>
                                <tr id="tr_<%# Eval("ID") %>">
                                    <td class="imgtd" style="height: 60px;" goodsname="<%# Eval("GoodsName") %>">
                                        <asp:HiddenField ID="Hd_Id" runat="server" Value='<%# Eval("ViewInfoID") %>' />
                                        <div class="pxxc left">
                                            <a target="_blank" href="../e<%# Eval("id") %>_<%=  this.CompID %>.html" class="pic">
                                                <img width="70px" height="70px" src="<%# Common.GetPicURL(Eval("pic").ToString()) %>" />
                                            </a>
                                        </div>
                                        <div class="tc tcle " style="padding: 10px 10px 0 0;">
                                            <a target="_blank" class="view" href="../e<%# Eval("id") %>_<%=  this.CompID %>.html">
                                                <%# Common.GetName(Eval("GoodsName").ToString())%></a><br />
                                            <label class="pCode">
                                                <%# Eval("BarCode") %></label>
                                            <%# Eval("proGoodsID").ToString() != "" ? "<i class=\"sale\" tip=\"" + Eval("proGoodsID") + "\">促销</i>" : ""%>
                                            <%# GoodsinfoModel(Eval("ViewInfoID").ToString()) == "" ? "&nbsp;<i style=\"color:red;\">已下架</i>" : "&nbsp;"%>
                                            <br />
                                            <label class="hideInfo1">
                                                <%# Common.GetName(Eval("hideInfo1").ToString())%>
                                            </label>
                                            <br />
                                            <label class="hideInfo2">
                                                <%# Common.GetName(Eval("hideInfo2").ToString())%>
                                            </label>
                                        </div>
                                    </td>
                                    <td style="text-align: left;" id='<%# Eval("Id") %>'>
                                        <%# Eval("ViewInfos") %>
                                    </td>
                                    <td style='<%# IsInve==0?"": "display:none"  %>'>
                                        <!--商品库存-->
                                        <span id="tr_Inve_<%# Eval("Id") %>">
                                            <%# Eval("infoInve", "{0:f2}")%>
                                        </span>
                                    </td>
                                    <td>
                                        <span id='<%# Eval("Id") %>_TinkerPrice'>
                                            <%# TPrice(Eval("ViewInfoID").ToString(), (Eval("SalePrice").ToString()).ToDecimal(0))%>
                                        </span>
                                        <input type="hidden" value="<%# Eval("SalePrice") %>" />
                                    </td>
                                    <td>
                                        <span class="linkBtn">
                                            <%--<a onserverclick="A_Collect"  collect='<%# Eval("ID") %>' runat="server" style="cursor: pointer;"></a>--%>
                                            <a href='javascript:void(0);' class="btnAddCart" tip_id="<%# Eval("Id") %>">加入购物车</a>
                                            <%# CheckGoodsCollect(Eval("CompID").ToString(), Eval("ID").ToString())%></span>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
            <div class="pagin">
                <webdiyer:AspNetPager ID="Pagers" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                    NextPageText=">" PageSize="12" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
                    ShowPageIndexBox="Always" TextAfterPageIndexBox="<span class='pagiPage'>页</span>"
                    TextBeforePageIndexBox="<span class='jump'>跳转到:</span>" CustomInfoSectionWidth="20%"
                    CustomInfoStyle="padding:5px 0 0 20px;cursor: default;color: #737373;" CustomInfoClass="message"
                    ShowCustomInfoSection="Left" CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                    CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5"
                    OnPageChanged="Pager_PageChanged">
                </webdiyer:AspNetPager>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
