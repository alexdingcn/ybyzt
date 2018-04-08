<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StockInEdit.aspx.cs" Inherits="Company_GoodsStock_StockInEdit" %>

<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>商品入库新增</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <link href="../../Distributor/newOrder/css/global2.5.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"
        rel="stylesheet" type="text/css" />
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../js/stock.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <style>
        .box
        {
            padding: 0px 10px;
            border: 1px solid #ddd;
            border-left: 0;
            border-top: 0;
            border-right: 0;
            width: 190px;
            height: 27px;
            line-height: 27px;
            color: #999;
            padding-left: 5px;
            font-family: "微软雅黑";
            font-size: 12px;
        }
		.tc .box{border: none;}
        .goods-zs > .tabLine
        {
            position: initial;
        }
        #DropDownList1 {
            border:1px #999 solid;
            width:150px;
            text-align:center;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $(".tabLine tbody tr").each(function (i, index) {
                var remark = $.trim($(".tabLine tbody tr").eq(i).find(".cur").text()); //商品备注
                var index = $.trim($(".tabLine tbody tr").eq(i).attr("trindex2")); //获取当前tr的index
                //判断商品备注是否为空，如果为空则替换商品备注所在的td内容
                if ($.trim(remark) != "") {
                    if (remark.length > 6) {
                        var remark2 = remark.substring(0, 6) + "...";
                        $.trim($(".tabLine tbody tr").eq(i).find(".ramk").replaceWith("<td class=\"ramk\"><div class=\"tc alink\"><a href=\"javascript:;\" class=\"aremark" + index + "\">编辑</a><div class=\"divremark" + index + "\">" + remark2 + "</div><div class=\"cur\">" + remark + "</div></div></td>"))
                    } else {
                        $.trim($(".tabLine tbody tr").eq(i).find(".ramk").replaceWith("<td class=\"ramk\"><div class=\"tc alink\"><a href=\"javascript:;\" class=\"aremark" + index + "\">编辑</a><div class=\"divremark" + index + "\">" + remark + "</div><div class=\"cur\">" + remark + "</div></div></td>"))
                    }
                }
            });

            //添加商品备注
            $(document).on("click", ".alink a", function () {
                var disId = $("#hidDisID").val();
                var compId = $("#hidCompId").val();
                var goodsInfoId = $.trim($(this).parent().parent().parent().attr("tip")); //goodsinfoId
                var indexs = $.trim($(this).parent().parent().parent().attr("trindex2")); //当前行的索引
                var remark = $.trim($(this).parent().find(".cur").text()); //当前行的商品备注
                var index = layerCommon.openWindow("添加备注", "../../Distributor/newOrder/remarkview.aspx?disId=" + disId + "&CompId=" + compId + "&type=2&KeyID=" + goodsInfoId + "&index=" + indexs + "&remark=" + remark, "770px", "450px");  //记录弹出对象
                $("#hid_Alert").val(index); //记录弹出对象
            })
            //返回
            $("#returns").click(function () {
                var Types = $("#hid_Type").val(); //出库 Or 入库
                window.location = "StockInList.aspx?type=" + Types + "";
            })
            //提交
            $("#Tj").click(function () {
                var remark = $("#Remarktex").val(); //单据备注
                var no = $("#Notex").val(); //单号
                var type = $("#DropDownList1").val(); //下拉框类型
                var Types = $("#hid_Type").val(); //出库 Or 入库
                var OrderID = $("#hid_No").val(); //修改的主表ID
                var index = 0; //用来判断是否是第一条有效的商品数据（排除空行）
                var json = "[{\"remark\":\"" + remark + "\",\"no\":\"" + no + "\",\"type\":\"" + type + "\",\"Type\":\"" + Types + "\",\"OrderID\":\"" + OrderID + "\",\"orderdetail\":[";
                $(".tabLine tbody tr").each(function (i, obj) {
                    var tip = $(".tabLine tbody tr").eq(i).attr("tip");
                    index++;
                    if (tip != undefined && tip != "") {
                        var BatchNO = $.trim($(".tabLine tbody tr").eq(i).find(".txtBatchNO ").val()); //商品购买数量
                        var validDate = $.trim($(".tabLine tbody tr").eq(i).find(".txtvalidDate").val()); //商品购买数量

                        var num = $.trim($(".tabLine tbody tr").eq(i).find(".txtGoodsNum ").val()); //商品购买数量
                        var remark = $.trim($(".tabLine tbody tr").eq(i).find(".alink a").text()); //商品备注
                        var id = $.trim($(".tabLine tbody tr").eq(i).attr("id")); //商品id
                        var skuid = $.trim($(".tabLine tbody tr").eq(i).attr("tip")); //商品详情ID
                        if (remark == "添加") {
                            remark = "";
                        } else {
                            remark = $.trim($(".tabLine tbody tr").eq(i).find(".alink .cur").text());
                        }
                        if (index == 1) {
                            json += "{\"id\":\"" + id + "\",\"remark\":\"" + remark + "\",\"skuid\":\"" + skuid + "\",\"goodsnum\":\"" + num + "\",\"BatchNO\":\"" + BatchNO + "\",\"validDate\":\"" + validDate + "\"}";
                        } else {
                            json += ",{\"id\":\"" + id + "\",\"remark\":\"" + remark + "\",\"skuid\":\"" + skuid + "\",\"goodsnum\":\"" + num + "\",\"BatchNO\":\"" + BatchNO + "\",\"validDate\":\"" + validDate + "\"}";
                        }
                    } else {
                        index--;
                    }

                })
                json += "]}]";
                var actiontype = "EditStockInOut";
                $.ajax({
                    type: "post",
                    url: "../../Handler/orderHandle.ashx",
                    data: { ActionType: actiontype, json: json },
                    dataType: "text",
                    success: function (data) {
                        var json = JSON.parse(data);
                        if (json.returns == "True") {
                            layerCommon.msg("操作成功", IconOption.笑脸);
                            window.location = "StockInInfo.aspx?type=" + Types + "&no=" + json.no + "";
                        }
                        else {
                            layerCommon.msg("操作失败：" + json.msg + "", IconOption.哭脸);
                        }

                        return false;
                    }, error: function () {
                        layerCommon.msg("提交失败", IconOption.错误);
                        return false;
                    }
                })

            })

        })
        //备注
        function remarkinfo(type, remark, goodsInfoId, index, disId) {
            remark = stripscript(remark)
            goodsInfoId = stripscript(goodsInfoId)
            index = stripscript(index)
            if (type == 2) {
                if ($.trim(remark) == "") {
                    $(".aremark" + index).text("添加");
                } else {
                    $(".aremark" + index).text("编辑");
                }
                //$(".aremark" + index).nextAll().remove();
                if ($.trim(remark) != "") {
                    if (remark.length > 6) {
                        var remark2 = remark.substring(0, 6) + "...";
                        $(".aremark" + index).parent().replaceWith("<div class=\"tc alink\"><a href=\"javascript:;\" class=\"aremark" + index + "\">编辑</a><div class=\"divremark" + index + "\">" + remark2 + "</div><div class=\"cur\">" + remark + "</div></div>")
                       // $(".aremark" + index).after("<div class=\"divremark" + index + "\">" + remark2 + "</div><div class=\"cur\">" + remark + "</div>");
                    } else {
                        $(".aremark" + index).parent().replaceWith("<div class=\"tc alink\"><a href=\"javascript:;\" class=\"aremark" + index + "\">编辑</a><div class=\"divremark" + index + "\">" + remark + "</div><div class=\"cur\">" + remark + "</div></div>")
                        //$(".aremark" + index).after("<div class=\"divremark" + index + "\">" + remark + "</div><div class=\"cur\">" + remark + "</div>");
                    }
                }
                onchengSum(goodsInfoId, index, 0, disId);
            }
        }
        //显示的商品 strtext=="" focus 事件 !="" keyup事件 strtext文本框输入的内容
        function showGoods(strtext, inindex) {
            var hid = ""; //筛选下拉的商品
            //绑定下拉商品
            //第一次赋值没完成时
            if ($.trim($(".divGoodsName").text()) == "") {
                $.ajax({
                    type: "post",
                    data: { ck: Math.random(), action: "dislist" },
                    dataType: "text",
                    async: false,
                    success: function (data) {
                        hid = eval('(' + data + ')'); //筛选下拉的商品
                    }
                })
            } else {
                hid = eval('(' + $(".divGoodsName").text() + ')'); //筛选下拉的商品
            }
            //先清空下拉商品 再绑定
            $(".tabLine table tbody tr").eq(inindex).find(".search-opt .list").html("");
            //当前行下拉商品显示
            $(".tabLine table tbody tr").eq(inindex).find(".search-opt").show();
            $(hid).each(function (index, obj) {
                //下拉商品最多只显示5条
                if ($(".tabLine table tbody tr").eq(inindex).find(".search-opt .list li").length < 5) {
                    if (strtext != "") {
                        //根据商品名称和商品编码筛选商品
                        if (obj.GoodsName.indexOf(strtext) != -1 || obj.BarCode.toLocaleLowerCase().indexOf(strtext.toLocaleLowerCase()) != -1) {
                            $(".tabLine table tbody tr").eq(inindex).find(".search-opt .list").append("<li tip=\"" + obj.ID + "\"><span><a href=\"javascript:;\"><img src=\"" + ($.trim(obj.Pic) != "" ? '<%=Common.GetWebConfigKey("ImgViewPath")  %>' + "GoodsImg/" + obj.Pic : "../../images/pic.jpg") + "\" width=\"40\" height=\"40\"></a></span><i href=\"javascript:;\" class=\"code\">商品编码：" + obj.BarCode + ($.trim(obj.proTypes) != "" ? obj.Cux : "") + "</i><i href=\"javascript:;\" class=\"name\">" + GetGoodsName(obj.GoodsName, $.trim(obj.ValueInfo), 1) + "<i>" + GetGoodsName(obj.GoodsName, $.trim(obj.ValueInfo), 2) + "</i></i></li>");
                        }
                    } else {
                        $(".tabLine table tbody tr").eq(inindex).find(".search-opt .list").append("<li tip=\"" + obj.ID + "\"><span><a href=\"javascript:;\"><img src=\"" + ($.trim(obj.Pic) != "" ? '<%=Common.GetWebConfigKey("ImgViewPath")  %>' + "GoodsImg/" + obj.Pic : "../../images/pic.jpg") + "\" width=\"40\" height=\"40\"></a></span><i href=\"javascript:;\" class=\"code\">商品编码：" + obj.BarCode + ($.trim(obj.proTypes) != "" ? obj.Cux : "") + "</i><i href=\"javascript:;\" class=\"name\">" + GetGoodsName(obj.GoodsName, $.trim(obj.ValueInfo), 1) + "<i>" + GetGoodsName(obj.GoodsName, $.trim(obj.ValueInfo), 2) + "</i></i></li>");
                    }
                }
            })
        }

        //绑定数据（商品id，无用字段,当前选择行的索引,无用字段）
        function GoodsList(goodsInfoId,str,inindex,str2) {
         
            var Digits = '<%=OrderInfoType.rdoOrderAudit("订单下单数量是否取整", this.CompID) %>'; //小数位数
            var sDigits = Digits.substring(2, Digits.indexOf(".") + 5).length;
            if (goodsInfoId != "") {
                var xyindex = 0; //最大的索引
                //冒泡排序获取最大的索引
                //如果间隔行去选择商品 则对应不到对应的价格 所以采用了下面这个方法
                $(".tabLine table tbody tr").each(function (x, indexy) {
                    if ($(".tabLine table tbody tr").eq(x).attr("trindex") != undefined) {
                        $(".tabLine table tbody tr").each(function (y, indexyz) {
                            y = x + 1;
                            if ($(".tabLine table tbody tr").eq(y).attr("trindex") != undefined) {
                                if (parseInt($(".tabLine table tbody tr").eq(y).attr("trindex")) < parseInt($(".tabLine table tbody tr").eq(x).attr("trindex"))) {
                                    xyindex = $(".tabLine table tbody tr").eq(y).attr("trindex");
                                    $(".tabLine table tbody tr").eq(y).attr("trindex", $(".tabLine table tbody tr").eq(x).attr("trindex"));
                                    $(".tabLine table tbody tr").eq(x).attr("trindex", xyindex);
                                }
                            } else {
                                xyindex = $(".tabLine table tbody tr").eq(x).attr("trindex");
                            }
                        })
                    }
                })
                $.ajax({
                    type: "post",
                    data: { ck: Math.random(), action: "goodsInfo", goodsInfoId: goodsInfoId },
                    dataType: "json",
                    success: function (data) {
                        var html = ""; //绑定的商品数据
                        //var z = 0;
                        var indexxy = 0; //获取选中过来的商品数量 便于相加索引

                        var strr = "<td><div class=\"tc\"><input type=\"text\" class=\"box txtBatchNO\"  maxlength=\"16\" style=\"width:120px\"  value=\"\"></div></td><td><div class=\"tc\"><input type=\"text\" class=\"box txtvalidDate\"  maxlength=\"9\" style=\"width:70px\" readonly=\"readonly\"  onclick=\"WdatePicker()\" value=\"\"></div></td>";

                        $(data).each(function (indexs, obj) {
                            // z = goodsInfoId.split(",").length - 1;
                            indexxy = goodsInfoId.split(",").length; //获取选中过来的商品数量 便于相加索引
                            var index = parseInt(parseInt(xyindex) + parseInt(indexs) + parseInt(indexxy)); //绑定商品数据行的索引
                            html += "<tr trindex=\"" + index + "\" trindex2=\"" + index + "\" id=\"" + obj.GoodsID + "\" tip=\"" + obj.ID + "\"><td class=\"t8\"><div class=\"addg\"><a href=\"javascript:;\" class=\"minus2\"></a><a href=\"javascript:;\" class=\"add2\"></a></div></td><td><div class=\"sPic\"><a class=\"opt-i2\"></a><span><a href=\"javascript:;\"><img src=\"" + ($.trim(obj.Pic) != "" ? '<%=Common.GetWebConfigKey("ImgViewPath")  %>' + "GoodsImg/" + obj.Pic : "../../images/pic.jpg") + "\" width=\"60\" height=\"60\"></a></span><a href=\"javascript:;\" class=\"code\">商品编码：" + obj.BarCode + ($.trim(obj.proTypes) != "" ? obj.Cux : "") + "</a><a href=\"javascript:;\" class=\"name\" style=\"width:250px\">" + GetGoodsName(obj.GoodsName, "", 1) + "<i>" + GetGoodsName(obj.GoodsName, "", 2) + "</i></a></div></td><td class=\"tc\">" + obj.ValueInfo + "</td><td><div class=\"tc\">" + obj.Unit + "</div></td>" + strr + "<td><div class=\"sl divnum\" tip=\"" + goodsInfoId + "\" tip2=\"" + index + "\"><a href=\"javascript:void(0);\"  class=\"minus\">-</a><input type=\"text\" class=\"box txtGoodsNum txtGoodsNum" + index + "\" onfocus=\"InputFocus(this)\"  onkeyup='KeyInt2(this)' maxlength=\"9\" style=\"width:60px\"  value=\"" + (parseFloat(obj.Inventory).toFixed(sDigits) < 1 ? parseFloat(obj.Inventory).toFixed(sDigits) : parseFloat(1).toFixed(sDigits)) + "\"><a href=\"javascript:void(0);\"  class=\"add\">+</a></div></td><td class=\"ramk\"><div class=\"tc alink\"><a href=\"javascript:;\" class=\"aremark" + index + "\">添加</a></div></td></tr>";
                        })
                        $(".tabLine table tbody tr").eq(inindex).replaceWith(html); //替换当前选择时的行
                    }
                })
            }
            }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <input id="hid_Alert" type="hidden" />
        <input id="hid_Type" type="hidden" value="1" runat="server"/>
        <input id="hid_No" type="hidden" value="0" runat="server"/>
    <input type="hidden" id="hidCompId" runat="server" />
    <uc1:Top ID="top1" runat="server" ShowID="nav-3" />
    <div class="rightCon">
        <div class="info">
            <a href="../jsc.aspx">我的桌面</a>>
            <a href="#" runat="server" id="title">新增入库</a></div>
        <!--[if !IE]>商品展示区 start<![endif]-->
        <div class="goods-zs">
            <!--[if !IE]>选择代理商 start<![endif]-->
            <div class="goods-info" style="padding: 0px 20px 20px 0px;">
                <div class="bz">
                    <i class="bt">单号：</i>
                    <input name="" id="Notex" class="box"  runat="server" readonly="readonly"/>
                    <i style="margin-left:100px;"><%=TitleType %>类型：</i>
                    <asp:DropDownList ID="DropDownList1" runat="server">
                        <asp:ListItem Value="采购入库">采购入库</asp:ListItem>
                        <asp:ListItem Value="盘点入库">盘点入库</asp:ListItem>
                        <asp:ListItem Value="其他入库">其他入库</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="bz remark">
                    <i class="bt">备注：</i><div class="txtbox">
                        <input name="" runat="server" id="Remarktex" maxlength="1000" class="box"/></div>
                </div>
            </div>
            <!--[if !IE]>选择代理商 end<![endif]-->
            <!--商品 start -->
            <div class="tabLine">
                <table border="0" cellspacing="0" cellpadding="0">
                    <thead>
                        <tr>
                            <th class="t5">
                            </th>
                            <th class="">
                                商品名称
                            </th>
                            <th class="">
                                规格属性
                            </th>
                            <th class="t5">
                                单位
                            </th>
                            <th>
                                批次号
                            </th>
                            <th>
                                有效期
                            </th>
                            <th style="width: 16%">
                                <%=TitleType %>数量
                            </th>
                            <th class="t3">
                                备注
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr runat="server" id="tbodyTR">
                        <td class="t8"> <div class="addg">
                            <a href="javascript:;" class="minus2"></a><a href="javascript:;" class="add2"></a> </div>
                        </td> 
                        <td> 
                            <div class="search">
                               <input name="" type="text" class="box project2"/><a class="opt-i"></a>
                               <!--[if !IE]>搜索弹窗 start<![endif]-->
                               <div class="search-opt none"> <ul class="list"></ul><div class="opt">
                               <a href="javascript:;"><i class="opt2-i"></i>选择商品</a></div> </div>
                               <!--[if !IE]>搜索弹窗 end<![endif]-->  
                            </div> 
                        </td>
                        <td></td> 
                        <td></td> 
                        <td></td> 
                        <td></td> 
                        <td></td>
                       </tr>
                        <asp:Repeater ID="Repeater1" runat="server">
                            <ItemTemplate>
                           <tr trindex="<%#Eval("ID")%>" trindex2="<%#Eval("ID")%>" id="<%#Eval("GoodsID") %>" tip="<%#Eval("GoodsinfoID") %>">
                               <td class="t8">
                               <div class="addg">
                                 <a href="javascript:;" class="minus2"></a>
                                 <a href="javascript:;" class="add2"></a>
                               </div></td>
                               <td><div class="sPic"><a class="opt-i2"></a>
                                <span><a  target="_blank" href="../../e<%# Eval("GoodsInfoID") %>_<%=this.CompID %>_.html" onclick="return false;">
                                <img src="<%#Eval("pic").ToString()!= "" ?Common.GetWebConfigKey("ImgViewPath")+"GoodsImg/" +Eval("pic").ToString():"../../images/pic.jpg"%>" width="60" height="60"></a>
                                </span><a  target="_blank" href="../../e<%# Eval("GoodsInfoID") %>_<%=this.CompID %>_.html" onclick="return false;" class="code">商品编码：<%#Eval("BarCode") %> </a>
                                   <a  target="_blank" style="width:250px" href="../../e<%# Eval("GoodsInfoID") %>_<%=this.CompID %>_.html" onclick="return false;" class="name" > <%#Eval("GoodsName")%>  <i><%#Eval("GoodsName")%></i></a></div>
                                </td>
                               <td class="tc"><%#Eval("ValueInfo") %></td>
                               <td><div class="tc"><%#Eval("Unit") %>
                                </div></td>
                                <td>
                                    <div class="tc"><input type="text" class="box txtBatchNO "  maxlength="16" style="width:120px"  value="<%#Eval("BatchNO") %>"></div>
                                </td>
                                <td>
                                    <div class="tc"><input type="text" class="box txtvalidDate " onclick="WdatePicker()" maxlength="9" style="width:70px"  value="<%# Eval("validDate","{0:yyyy-MM-dd}") %>"></div>
                                </td>
                                <td><div class="sl divnum" tip="<%#Eval("GoodsinfoID") %>" tip2="<%#Eval("ID")%>"><a href="javascript:void(0);"  class="minus">-</a><input type="text" class="box txtGoodsNum txtGoodsNum<%#Eval("ID")%>" onfocus="InputFocus(this)"  onkeyup='KeyInt2(this)' maxlength="9" style="width:60px"  value="<%#Eval("StockNum") %>"><a href="javascript:void(0);"  class="add">+</a></div></td>
                                <td class="ramk"><div class="tc alink">
                                <a href="javascript:;" class="aremark<%#Eval("ID")%>"><%#Eval("Remark").ToString() != "" ? "<div class=\"tc alink\"><a href=\"javascript:;\" class=\"aremark29\">编辑</a><div class=\"divremark29\">" + (Eval("Remark").ToString().Length <6 ? Eval("Remark").ToString(): Eval("Remark").ToString().Substring(0,6)+"...")+"</div><div class=\"cur\">"+Eval("Remark")+"</div></div>":"添加" %></a></div></td>
                            </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
        </div>
        <!--[if !IE]>商品展示区end<![endif]-->
        <div class="blank20">
        </div>
        <!--[if !IE]>下单信息 start<![endif]-->
        <!--[if !IE]>下单信息 end<![endif]-->
        <div class="blank20">
        </div>
        <div class="btn-box">
            <div class="btn">
                <a href="javascript:;" class="btn-area" id="Tj" runat="server">保存</a><a href="#" id="returns" class="gray-btn">返回</a></div>
            <div class="bg">
            </div>
        </div>
        <div id="divGoodsName" class="divGoodsName" runat="server" style="display: none">
        </div>
    </div>
    </form>
</body>
</html>
