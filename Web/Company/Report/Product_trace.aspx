<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Product_trace.aspx.cs" Inherits="Company_Product_trace" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager, Version=7.2.0.0, Culture=neutral, PublicKeyToken=fb0a0fe055d40fd4" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>销售报表</title>
    <link href="../../Company/css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>

    <style>
        .timeClass a
        {
            height: 23px;
            line-height: 23px;
            display: inline-block;
            border: 1px solid #ddd;
            padding: 0px 10px;
            margin: 0 0 0 5px;
        }
        a.hover
        {
            color: #fff;
            border: 1px solid #5e89c9;
            background: #779bd1;
        }
    </style>
    
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Top ID="top1" runat="server" ShowID="nav-5" />
    
    <div class="rightinfo">
        <!--当前位置 start-->
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li>
                <li>></li>
                <li><a href="Product_trace.aspx">产品追溯图</a></li>
            </ul>
        </div>
        <!--当前位置 end-->
        <!--功能按钮 start-->
        <div class="tools">
            <ul class="toolbar left">
            </ul>
            <div class="right">
                <ul class="toolbar right">
                    <li id="Search"><span>
                        <img src="../../Company/images/t04.png" /></span>搜索</li>
                   
                </ul>
                <asp:Button ID="btnSearch" Text="搜索" runat="server" OnClick="btnSearch_Click" Style="display: none" />
                <ul class="toolbar3">
                    <li>商品:<input name="txtGoodsName" type="text" id="txtGoodsName" runat="server" class="textBox" />
                    </li>
                    <li>代理商:<input name="txtDisName" type="text" id="txtDisName" runat="server" class="textBox" />
                    </li>
                     <li>医院:<input name="txtHtName" type="text" id="txtHtName" runat="server" class="textBox" />
                    </li>

                </ul>
            </div>
        </div>
        <!--功能按钮 end-->
        <!--信息列表 start-->

			<div class="fl tablelist-pro">
                <table class="tablelist ">
                    <thead><tr><th class="t6">商品</th></tr></thead>
					<tbody>
                           <asp:Repeater ID="rpt_Goods" runat="server">
                                <ItemTemplate>
                                     <tr><td><div tip="<%# Eval("ID") %>" class="goodstcle tcle <%# goodsid==Eval("ID").ToString()?"cur":"" %>"><a href="javascript:;"><%# Eval("GoodsName") %></a></div></td></tr>
                                </ItemTemplate>
                           </asp:Repeater>
					</tbody>
				</table>
				</div>
				<div class="tablelist-agent fr">
                <table class="tablelist ">
                    <thead><tr><th class="t6">代理商</th><th class="t6">医院</th><th class="t6">数量</th></tr></thead>
                    <tbody id="tbodyT"><tr><td><div class="tcle">代理商A</div></td><td><div class="tcle">上海六院</div></td><td><div class="tcle">3</div></td></tr>
						   <tr><td><div class="tcle">代理商B</div></td><td><div class="tcle">上海八院</div></td><td><div class="tcle">7</div></td></tr>
						   <tr><td><div class="tcle">代理商C</div></td><td><div class="tcle">武汉人民医院</div></td><td><div class="tcle">4</div></td></tr>
						<tr><td><div class="tcle">代理商D</div></td><td><div class="tcle">苏州人民医院</div></td><td><div class="tcle">10</div></td></tr>
						<tr><td><div class="tcle">代理商E</div></td><td><div class="tcle">杭州人民医院</div></td><td><div class="tcle">3</div></td></tr>
					</tbody>
				</table>
				</div>
        <!--信息列表 end-->
        <!--列表分页 start-->
       
        <!--列表分页 end-->
    </div>
        <div style="margin-left: 120px;">
        </div>

        <script>
            "use strict"
            $(document).ready(function () {
                GetDisHt(<%=goodsid %>);
                $(document).on("click", ".goodstcle", function () {
                    var goodsid = $.trim($(this).attr("tip"));
                    GetDisHt(goodsid);
                });

                $(document).on("click","#Search",function(){
                   var  GoodsName=$.trim($("#txtGoodsName").val());

                   if(GoodsName!=""){
                      $("#btnSearch").trigger("click");
                   }else{
                        var goodsid=$(".tablelist-pro").find(".goodstcle[class*='cur']").attr("tip");                
                        GetDisHt(goodsid);
                   }
                });
            });

            function GetDisHt(goodsid) {
                var DisName=$.trim($("#txtDisName").val());
                var HtName=$.trim($("#txtHtName").val());
                 var tr = "";

                 if(typeof(goodsid)=="undefined"||goodsid==""||goodsid==null){
                    $("#tbodyT").html(tr);
                    return ;
                 }

                $.ajax({
                    type: 'post',
                    url: 'Product_trace.aspx',
                    data: { ck: Math.random(), ActionType: "GetDisHT", goodsid: goodsid,DisName:DisName,HtName:HtName },
                    success: function (data) {
                        //console.log(data);
                       

                        if(data!=""){
                            $($.parseJSON(data)).each(function (index, item) {
                                tr += "<tr><td><div class=\"tcle\">" + item.DisName + "</div></td><td><div class=\"tcle\">" + item.HtName + "</div></td><td><div class=\"tcle\">" + item.GoodsNum + "</div></td></tr>";
                            });
                        }

                        $(".tablelist-pro").find(".goodstcle").removeClass("cur");
                        $(".tablelist-pro").find(".goodstcle[tip='"+goodsid+"']").addClass("cur");

                        $("#tbodyT").html(tr);
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
                    }
                });
                
            }
        </script>
    </form>
</body>
</html>
