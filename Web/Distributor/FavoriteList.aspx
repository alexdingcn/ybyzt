<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FavoriteList.aspx.cs" EnableEventValidation="false" Inherits="Distributor_FavoriteList" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>收藏商品</title>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link href="../Company/css/orderGoods.css" rel="stylesheet" type="text/css" />
    <script src="../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/layer/layer.js" type="text/javascript"></script>
    <script src="../js/layerCommon.js" type="text/javascript"></script>
    <script src="../js/shopcart.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#orderBg tr:even').addClass("bg");
        
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
                                $(th).find("i").css({"top":"50px"});
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
       
       });
        //根据商品基础ID和描述信息查询GoodsInfoID和商品价格
        function onAttrGoods(id, attr, attrvalue,i) {
            var falg="";   //商品是否存在
            var goodsinfoid;  //商品信息Id
            var TinkerPrice;  //代理商 商品价格
            var DisID=<%=this.DisID %>;
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
                async: true, //true:异步 false:同步
                success: function (result) {
                    var data = eval('(' + result + ')');
                    falg=data["falg"];
                    goodsinfoid = data["goodsinfoid"];
                    TinkerPrice = data["TinkerPrice"];
                        if(goodsinfoid!=0){
                          
                        if($("#tr_"+id).find("td:first").find("a[class=\"view\"]").siblings("i:not(.sale)").length>0)
                            $("#tr_"+id).find("td:first").find("a[class=\"view\"]").siblings("i:not(.sale)").remove();
                    }else{
                        if($("#tr_"+id).find("td:first").find("a[class=\"view\"]").siblings("i:not(.sale)").length<=0)
                            $("#tr_"+id).find("td:first").find("a[class=\"view\"]").after("&nbsp;<i style=\"color:red;\">已下架</i>");
                    }

                    if(IsInve==0){
                        if(parseFloat(data["Inventory"])<=0){
                                $("#tr_Inve_"+id).text(0);  //商品库
                            $("#tr_"+id).find("td:first").find("input[type=checkbox]").attr("disabled","disabled");
                        }else{
                                $("#tr_Inve_"+id).text(parseFloat(data["Inventory"]));  //商品库存
                                $("#tr_"+id).find("td:first").find("input[type=checkbox]").removeAttr("disabled");
                        }
                    }

                    $("#tr_"+id).find("td:first").find("input[type=hidden]").val(goodsinfoid);
                    $("#"+id+"_TinkerPrice").text(TinkerPrice);
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
    <Head:Head ID="Head1" runat="server" />
    <div class="w1200">
    <Left:Left ID="Left1" runat="server" ShowID="nav-2" />
    <div class="rightCon">
        <!--订单管理 start-->
        <div class="info"> <a id="navigation1" href="UserIndex.aspx">我的桌面</a>>
                <a id="navigation2" href="#" class="cur">收藏商品</a></div>
        <div class="orderNr">
            <table class="PublicList list" id="orderBg" border="0" cellspacing="0" cellpadding="0">
            <asp:Repeater ID="rptfavorite" runat="server">
            <HeaderTemplate>
                <thead>
                    <tr>
                        <th class="t6">
                            商品名称
                        </th>
                        <th>
                            商品描述
                        </th>
                        <th style='<%= IsInve==0?"":"display:none"  %>'>
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
                </HeaderTemplate>
                <ItemTemplate>
                    <tbody>
                        <tr id="tr_<%# Eval("ID") %>">
                            <td class="imgtd" style="height:60px;" GoodsName="<%# Eval("GoodsName") %>">
                                <asp:HiddenField ID="Hd_Id" runat="server" Value='<%# Eval("ViewInfoID") %>' />
                                <div class="pxxc left">
                                <a target="_blank" href="/e<%# Eval("id") %>_<%=this.CompID %>.html" class="pic">
                                    <img width="80px" height="75px" src="<%# GetGoodsPic(Eval("pic").ToString()) %>" />
                                </a>
                                </div>
                                <div class="tc tcle " style="margin:10px 0 0 10px; text-align:left; line-height:22px;"><a target="_blank" class="view" href="/e<%# Eval("id") %>_<%= this.CompID %>.html">
                                <%# Common.GetName(Eval("GoodsName").ToString())%></a><br />
                                <label class="pCode"><%# Eval("BarCode") %></label>
                               <%# Eval("proGoodsID").ToString() != "" ? "<i class=\"sale\" tip=\"" + Eval("proGoodsID") + "\">促销</i>" : ""%>
                                <%# Eval("ViewInfoID").ToString() == "0" ? "&nbsp;<i style=\"color:red;\">已下架</i>" : "&nbsp;"%></div>
                            </td>
                               
                            <td style="text-align:left;" id='<%# Eval("Id") %>'>
                                    <%# Eval("ViewInfos") %>
                            </td>
                            <td style='<%# IsInve==0?"":"display:none"  %>'>
                                <!--商品库存-->
                                <span id="tr_Inve_<%# Eval("Id") %>">
                                    <%# Eval("infoInve", "{0:F2}")%>
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
                                    <a href='javascript:void(0);' class="btnAddCart" tip_ID="<%# Eval("Id") %>">加入购物车</a>
                                    <a  href="javascript:void(0);" TipGoods='<%#Eval("ID") %>' runat="server" class="txtn">取消收藏</a>
                                </span>
                            </td>
                        </tr>
                     </tbody>
                </ItemTemplate>
                </asp:Repeater>
            </table>
            
        </div>
        <!--分页 start-->
            <div class="pagin">
                <webdiyer:AspNetPager ID="Pager" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                NextPageText=">" PageSize="12" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
                ShowPageIndexBox="Always" TextAfterPageIndexBox="<span class='pagiPage'>页</span>"
                TextBeforePageIndexBox="<span class='jump'>跳转到:</span>" CustomInfoSectionWidth="20%"
                CustomInfoStyle="padding:5px 0 0 20px;cursor: default;color: #737373;" CustomInfoClass="message"
                ShowCustomInfoSection="Left" CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5" OnPageChanged="Pager_PageChanged">
            </webdiyer:AspNetPager>
            </div>
            <!--分页 end-->
        <!--订单管理 end-->
    </div>
    </div>
    </form>
</body>
</html>
