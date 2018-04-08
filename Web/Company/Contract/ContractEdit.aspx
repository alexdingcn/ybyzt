<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ContractEdit.aspx.cs" Inherits="Company_Contract_ContractEdit" %>

<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>合同编辑</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />

    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <link href="../../Company/css/global-2.0.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"
        rel="stylesheet" type="text/css" />
    <link href="../../Distributor/newOrder/css/global2.5.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css"/>

    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../js/contract.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../js/UploadJs.js" type="text/javascript"></script>

    <style type="text/css">
       .rightinfo{margin-left: 130px;margin-top: 60px;width: 1060px;padding: 8px;}
        .toolbar li span{float: left;margin-left: 10px;margin-right: 5px;margin-top: 5px;}
        dl, dt, dd, span{margin: 0;padding: 0;display: inline-block;}
        .tools{clear: both;overflow: hidden;margin-bottom: 8px;}
        .left{float: left;}
        .toolbar li{background: url(../images/toolbg.gif) repeat-x;line-height: 28px;height: 28px;border: solid 1px #c5d2d8;float: left;padding-right: 10px;margin-right: 5px;border-radius: 3px;behavior: url(js/pie.htc);cursor: pointer;}

        /*公共-标题*/
        .c-n-title{line-height: 36px;height: 36px;font-size: 13px;padding-left: 13px;border-radius: 5px;margin: 10px 0 5px 0;}
        .c-n-title .set{font-size: 12px;margin-left: 15px;padding-top: 1px;}
        .c-n-title .set .r-check + label{top: 5px;margin-right: 5px;}
        /*公共-文本框*/
        .coreInfo .box1{width: 360px;}
        .box1, .box2{border: 1px solid #ddd;border-radius: 5px;height: 34px;line-height: 34px;padding: 0px 10px;color: #555;font-size: 12px;}
        .box2{width: 120px;}
        .coreInfo .name{width: 90px;text-align: right;display: inline-block;padding-right: 10px;line-height: 35px;color: #666;}
        i{font-style: normal;}
        .coreInfo .lb{/*padding-top: 10px;*/min-width: 482px;color: #666;position: relative;margin-right: 30px;min-height: 36px;}
        .fl{float: left;}
        li, dl{list-style-type: none;}
        .box{padding: 0px 10px;border: 1px solid #ddd;border-left: 0;border-top: 0;border-right: 0;width: 190px;height: 27px;line-height: 27px;color: #999;padding-left: 5px;font-family: "微软雅黑";font-size: 12px;}
        .goods-zs > .tabLine{position: initial;}
        #DropDownList1{border: 1px #999 solid;width: 150px;text-align: center;}
        .rightinfo{margin-left: 130px;margin-top: 60px;width: 1060px;padding: 8px; }
		.coreInfo .con a{ color:#528fe9}
		
    </style>
	
    <script type="text/javascript">
        $(function () {
            $("#uploadFile").AjaxUploadFile({ Src: "UploadFile/", ShowDiv: "UpFileText", ResultId: "HidFfileName", AjaxSrc: "../../Controller/Fileup.ashx", maxlength: 5, DownSrc: "../../" });
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

            //选择医院
            $(document).on("click", ".areHt", function () {
                var TrId = $.trim($(this).parents("tr").attr("id")); //父元素  tr  的 ID
                var indexs = $.trim($(this).parents("tr").attr("trindex2")); //当前行的索引
                var htId = $(this).parents("tr").find(".hid_Htid").val(); //当前医院ID
                var index = layerCommon.openWindow("选择医院", "HtAlert.aspx?TrId=" + TrId + "&indexs=" + indexs + "&htId=" + htId, "670px", "550px");  //记录弹出对象
                $("#hid_Alert").val(index); //记录弹出对象
            })


            //选择区域
            $(document).on("click", ".areArea", function () {
                var TrId = $.trim($(this).parents("tr").attr("id")); //父元素  tr  的 ID
                var indexs = $.trim($(this).parents("tr").attr("trindex2")); //当前行的索引
                var AreaID = $(this).parents("tr").find(".hidAreaID").val(); //当前医院ID
                var index = layerCommon.openWindow("选择区域", "AreaMvg.aspx?TrId=" + TrId + "&indexs=" + indexs + "&AreaID=" + AreaID, "670px", "550px");  //记录弹出对象
                $("#hid_Alert").val(index); //记录弹出对象
            })
            //返回
            $("#returns").click(function () {
                var Types = $("#hid_Type").val(); //出库 Or 入库
                window.location = "ContractList.aspx";
            })

            $("#DropDis").change(function () {
                $(".tabLine table tbody").html("");
                var html = "<tr><td class=\"t8\"><div class=\"addg\"><a href=\"javascript:;\" class=\"minus2\"></a><a href=\"javascript:;\" class=\"add2\"></a></div></td><td><div class=\"search\"><input name=\"\" type=\"text\" class=\"box project2\" readonly=\"readonly\" /><a class=\"opt-i\"></a><div class=\"search-opt none\"><ul class=\"list\"></ul><div class=\"opt\"><a href=\"javascript:;\"><i class=\"opt2-i\"></i>选择商品</a></div></div></div></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>";
                $(".tabLine table tbody").append(html);
            });

            //提交
            $("#Tj").click(function () {
                var txtcontractNO = $("#txtcontractNO").val(); //合同号
                var txtcontractDate = $("#txtcontractDate").val(); //日期
                var DropDis = $("#DropDis").val(); //代理商
                var CState = $("#CState").val(); //状态
                var txtForceDate = $("#txtForceDate").val(); //生效日期
                var txtInvalidDate = $("#txtInvalidDate").val(); //失效日期
                var txtRemark = $("#txtRemark").val(); //备注
                var Cid = $("#Cid").val();
                var HidFfileName = $("#HidFfileName").val();

                if (txtcontractNO == "" || txtcontractNO == undefined) { layerCommon.msg("合同号不能为空", IconOption.哭脸); return false; }
                if (txtcontractDate == "" || txtcontractDate == undefined) { layerCommon.msg("日期不能为空", IconOption.哭脸); return false; }
                if (DropDis == "" || DropDis == undefined) { layerCommon.msg("代理商不能为空", IconOption.哭脸); return false; }
                if (CState == "" || CState == undefined) { layerCommon.msg("状态不能为空", IconOption.哭脸); return false; }
                if (txtForceDate == "" || txtForceDate == undefined) { layerCommon.msg("生效日期不能为空", IconOption.哭脸); return false; }
                if (txtInvalidDate == "" || txtInvalidDate == undefined) { layerCommon.msg("失效日期不能为空", IconOption.哭脸); return false; }

                var msg = "";
                var index = 0; //用来判断是否是第一条有效的商品数据（排除空行）
                var json = "[{\"txtcontractNO\":\"" + txtcontractNO + "\",\"txtcontractDate\":\"" + txtcontractDate + "\",\"DropDis\":\"" + DropDis + "\",\"CState\":\"" + CState + "\",\"txtForceDate\":\"" + txtForceDate + "\",\"txtInvalidDate\":\"" + txtInvalidDate + "\",\"txtRemark\":\"" + txtRemark + "\",\"Cid\":\"" + Cid + "\",\"HidFfileName\":\"" + HidFfileName + "\",\"orderdetail\":[";
                $(".tabLine tbody tr").each(function (i, obj) {
                    var tip = $(".tabLine tbody tr").eq(i).attr("tip");
                    index++;
                    if (tip != undefined && tip != "") {

                        var GoodsName = $.trim($(".tabLine tbody tr").eq(i).find(".hid_GoodsName ").val()); //商品名称
                        var GoodsCode = $.trim($(".tabLine tbody tr").eq(i).find(".hid_GoodsCode ").val()); //商品编码
                        var ValueInfo = $.trim($(".tabLine tbody tr").eq(i).find(".ValueInfo ").html()); //规格型号
                        var hid_Htid = $.trim($(".tabLine tbody tr").eq(i).find(".hid_Htid ").val()); //医院名称
                        var hidAreaID = $.trim($(".tabLine tbody tr").eq(i).find(".hidAreaID ").val()); //区域ID
                        var SalePrice = $.trim($(".tabLine tbody tr").eq(i).find(".SalePrice ").html()); //零售价
                        var txtTinkerPrice = $.trim($(".tabLine tbody tr").eq(i).find(".txtTinkerPrice ").val()); //价格
                        var discount = $.trim($(".tabLine tbody tr").eq(i).find(".discount ").val()); //折扣
                        var target = $.trim($(".tabLine tbody tr").eq(i).find(".target ").val()); //指标
                        var remark = $.trim($(".tabLine tbody tr").eq(i).find(".alink a").text()); //商品备注
                        var id = $.trim($(".tabLine tbody tr").eq(i).attr("id").split('_')[0]); //商品id
                        var skuid = $.trim($(".tabLine tbody tr").eq(i).attr("tip")); //商品详情ID


                        if (hid_Htid == "" || hid_Htid == undefined) { msg = "请选择医院"; return false; }
                        if (hidAreaID == "" || hidAreaID == undefined) { msg = "请选择区域"; return false; }
                        if (txtTinkerPrice == "" || txtTinkerPrice == undefined) { msg = "请输入折扣"; return false; }
                        if (discount == "" || discount == undefined) { msg = "请输入价格"; return false; }
                        if (target == "" || target == undefined) { target = 0; }

                        var FirstCampID = $.trim($(".tabLine tbody tr").eq(i).find(".hid_FirstCampID ").val()); //首营ID
                        if (FirstCampID == "" || FirstCampID == undefined) FirstCampID = 0;

                        if (remark == "添加") {
                            remark = "";
                        } else {
                            remark = $.trim($(".tabLine tbody tr").eq(i).find(".alink .cur").text());
                        }
                        if (index == 1) {
                            json += "{\"id\":\"" + id + "\",\"remark\":\"" + remark + "\",\"skuid\":\"" + skuid + "\",\"GoodsName\":\"" + GoodsName + "\",\"GoodsCode\":\"" + GoodsCode + "\",\"ValueInfo\":\"" + ValueInfo + "\",\"hid_Htid\":\"" + hid_Htid + "\",\"AreaID\":\"" + hidAreaID + "\",\"SalePrice\":\"" + SalePrice + "\",\"txtTinkerPrice\":\"" + txtTinkerPrice + "\",\"discount\":\"" + discount + "\",\"target\":\"" + target + "\",\"FirstCampID\":\"" + FirstCampID + "\"}";
                        } else {
                            json += ",{\"id\":\"" + id + "\",\"remark\":\"" + remark + "\",\"skuid\":\"" + skuid + "\",\"GoodsName\":\"" + GoodsName + "\",\"GoodsCode\":\"" + GoodsCode + "\",\"ValueInfo\":\"" + ValueInfo + "\",\"hid_Htid\":\"" + hid_Htid + "\",\"AreaID\":\"" + hidAreaID + "\",\"SalePrice\":\"" + SalePrice + "\",\"txtTinkerPrice\":\"" + txtTinkerPrice + "\",\"discount\":\"" + discount + "\",\"target\":\"" + target + "\",\"FirstCampID\":\"" + FirstCampID + "\"}";
                        }
                    } else {
                        index--;
                    }

                })
                json += "]}]";
                if (msg != "" && msg != undefined) { layerCommon.msg(msg, IconOption.哭脸); return false; }
                if (index == 0) { layerCommon.msg("请最少选择一行商品", IconOption.哭脸) }
                else {

                    $.ajax({
                        type: "post",
                        url: "../../Handler/orderHandle.ashx",
                        data: { ActionType: "ContractCheck", json: json },
                        dataType: "text",
                        success: function (data) {
                            var json1 = JSON.parse(data);
                            if (json1.returns == "true") {
                                //layerCommon.msg("操作成功", IconOption.笑脸);
                                //window.location = "ContractInfo.aspx?cid=" + json.ContrctID + "";

                                layerCommon.confirm(json1.msg, function () {

                                }, "提示", function () {
                                    ContractEdit(json);
                                });

                            }
                            else {
                                ContractEdit(json);
                            }

                            return false;
                        }, error: function () {
                            layerCommon.msg("提交失败", IconOption.错误);
                            return false;
                        }
                    })
                }

            });


            function ContractEdit(json) {
                var actiontype = "ContractEdit";
                $.ajax({
                    type: "post",
                    url: "../../Handler/orderHandle.ashx",
                    data: { ActionType: actiontype, json: json },
                    dataType: "text",
                    success: function (data) {
                        var json1 = JSON.parse(data);
                        if (json1.returns == "true") {
                            layerCommon.msg("操作成功", IconOption.笑脸);
                            window.location = "ContractInfo.aspx?cid=" + json1.ContrctID + "";
                        }
                        else {
                            layerCommon.msg("操作失败：" + json1.msg + "", IconOption.哭脸);
                        }

                        return false;
                    }, error: function () {
                        layerCommon.msg("提交失败", IconOption.错误);
                        return false;
                    }
                })
            }
        });

        //生单
        function btnCMerchants() {
            var disid = $("#DropDis").val();//代理商
            if (disid == "" || disid == undefined) {
                layerCommon.msg("请先选择代理商", IconOption.错误);
                return false;
            }

            var index = layerCommon.openWindow("选择招商", "selectCMerchantsList.aspx?disid=" + disid + "", "985px", "630px");  //记录弹出对象
            $("#hid_Alert").val(index); //记录弹出对象
        }

        //关闭弹窗
        function closeAll() {
            layer.closeAll();
        }


        //选择医院回调事件
        function Htinfo(HtName, Htid, hidTrId) {
            HtName = stripscript(HtName)
            hidTrId = stripscript(hidTrId)
            $("#" + hidTrId + "").find(".areHt").html(HtName);
            $("#" + hidTrId + "").find(".hid_Htid").val(Htid)
            layer.closeAll();
        }

        //选择区域回调事件
        function Areainfo(Name, Areaid, hidTrId) {
            Name = stripscript(Name)
            hidTrId = stripscript(hidTrId)
            $("#" + hidTrId + "").find(".areArea").html(Name);
            $("#" + hidTrId + "").find(".hidAreaID").val(Areaid)
            layer.closeAll();
        }

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
        }

        //绑定数据（商品id，无用字段,当前选择行的索引,无用字段）
        function GoodsList(goodsInfoId, str, inindex, str2) {

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
                var disid = $("#DropDis").val(); //代理商

                $.ajax({
                    type: "post",
                    data: { ck: Math.random(), action: "goodsInfo", goodsInfoId: goodsInfoId, disid: disid, str: str },
                    dataType: "json",
                    success: function (data) {
                        var html = ""; //绑定的商品数据
                        //var z = 0;
                        var indexxy = 0; //获取选中过来的商品数量 便于相加索引

                        $(".tabLine table tbody tr").eq(inindex).append(html); //替换当前选择时的行
                       
                            $(data).each(function (indexs, obj) {
                            indexxy = goodsInfoId.split(",").length; //获取选中过来的商品数量 便于相加索引
                            var FCID = 0;
                            var index = parseInt(parseInt(xyindex) + parseInt(indexs) + parseInt(indexxy)); //绑定商品数据行的索引
                            if ((str == "CM")) FCID = obj.FirstCampID;
                            var HospitalName = "";
                            if (obj.HospitalName == undefined || obj.HospitalName == "") HospitalName = "请选择医院"; else HospitalName = obj.HospitalName;
                            var AreaName = "";
                            if (obj.AreaName == undefined || obj.AreaName == "") AreaName = "请选择区域"; else AreaName = obj.AreaName;

                            html += "<tr  trindex=\"" + index + "\" trindex2=\"" + index + "\" id=\"" + obj.GoodsID + "_" + index + "\" tip=\"" + obj.ID + "\" ><td class=\"t5\"><div class=\"addg\"><a href=\"javascript:;\" class=\"minus2\"></a><a href=\"javascript:;\" class=\"add2\"></a></div></td><td class=\"t1\"><div class=\"sPic\"><a class=\"opt-i2\"></a><span><a href=\"javascript:;\"><img src=\"" + ($.trim(obj.Pic) != "" ? '<%=Common.GetWebConfigKey("ImgViewPath")  %>' + "GoodsImg/" + obj.Pic : "../../images/havenopicsmallest.gif") + "\" width=\"60\" height=\"60\"></a></span><a href=\"javascript:;\" class=\"color:blue\">商品编码：" + obj.BarCode + ($.trim(obj.proTypes) != "" ? obj.Cux : "") + "</a><input class=\"hid_GoodsName\" type=\"hidden\" value=\"" + obj.GoodsName + "\" /><input class=\"hid_FirstCampID\" type=\"hidden\" value=\"" + FCID + "\" /><input class=\"hid_GoodsCode\" value=\"" + obj.BarCode + "\" type=\"hidden\" /><a href=\"javascript:;\" class=\"name\" >" + GetGoodsName(obj.GoodsName, "", 1) + "<i>" + GetGoodsName(obj.GoodsName, "", 2) + "</i></a></div></td><td class=\"t3 center ValueInfo\">" + obj.ValueInfo + "</td><td class=\" t3 center\"><a href=\"javascript:;\" class=\"areHt\" style=\"color:blue;\">" + HospitalName + "</a><input class=\"hid_Htid\" type=\"hidden\" value=\"" + obj.htid + "\"/></td><td class=\" t3 center\"><a href=\"javascript:;\" class=\"areArea\" style=\"color:blue;\">" + AreaName + "</a><input class=\"hidAreaID\" type=\"hidden\" value=\"" + obj.AreaID + "\"/></td><td class=\" t5 center\"><span class=\"SalePrice\">" + obj.SalePrice + "</span></td><td class=\"t3 center\"><input type=\"text\" class=\"box1 discount\" onfocus=\"InputFocus(this)\"  onkeyup='KeyInt2(this,1)' maxlength=\"9\" style=\"width:40px\"  value=\"100.00\"></td><td class=\"t3 center\"><input type=\"text\" class=\"box1 txtTinkerPrice \" onfocus=\"InputFocus(this)\"  onkeyup='KeyInt2(this,2)' maxlength=\"9\" style=\"width:40px\"  value=\"" + obj.SalePrice + "\"></td><td class=\"t5 center\"><input type=\"text\" class=\"box1 target\" onfocus=\"InputFocus(this)\"  onkeyup='KeyInt2(this,3)' maxlength=\"9\" style=\"width:40px\"  value=\"0\"></td><td class=\"t3 center ramk\"><div class=\"tc alink\"><a href=\"javascript:;\" class=\"aremark" + index + "\">添加</a></div></td></tr>";
                            })

                        if (str == "CM")
                        {
                            if (xyindex == "0")
                                $(".tabLine table tbody ").prepend(html);
                            else
                            $(".tabLine table tbody ").append(html);
                            //layer.closeAll();
                        }
                        else
                        $(".tabLine table tbody tr").eq(inindex).replaceWith(html); //替换当前选择时的行
                       

                        
                    }
                })
            }
        }

        function KeyInt2(val, type) {
            var r = /^[0-9]\d*?\.?\d*?$/;
            if (!val.value.match(r)) {
                val.value = '';
            }
            var SalePrice = $(val).parents("tr").find(".SalePrice").html() * 1; //零售价
            var itemValue = 0; //折扣  或者  价格（根据type  区分 1、 折扣 2、价格）
            if (val.value != '' && val.value != undefined)
                itemValue = val.value * 1;

            if (type == 1)//折扣文本框值改变事件
            {

                var price = (SalePrice * itemValue) / 100
                if (itemValue == 0 || itemValue == undefined || itemValue == '')
                    price = SalePrice;
                if (price.toString().indexOf(".") != -1) {
                    var index = price.toString().indexOf(".");
                    price = price.toString().substring(0, index + 3)
                }

                $(val).parents("tr").find(".txtTinkerPrice").val(parseFloat(price));
            }
            else if (type == 2) {
                if (parseFloat(SalePrice) != 0) {
                    var discount = (itemValue / SalePrice) * 100;
                    if (discount.toString().indexOf(".") != -1) {
                        var index = discount.toString().indexOf(".");
                        discount = discount.toString().substring(0, index + 3)
                    }
                } else {
                    discount = "100.00";
                }
                $(val).parents("tr").find(".discount").val(parseFloat(discount));
            }


        }


    </script>
    <style type="text/css">
        .center {text-align: center;}
        .coreInfo .con {line-height: 36px;display: inline-block;position: relative;top:-50px;}
        .upload { padding-left: 65px;}
		.teamList{ top:-50px;position: relative;}
        .teamList dd span,.red {margin-top:3px; }
		.teamList .speed{ margin-right: 20px;}
        .teamList dd {display:block;width:300px;height:30px;border:0px solid red;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <input id="hid_Alert" type="hidden" />
        <input id="hid_Type" type="hidden" value="1" runat="server" />
        <input id="hid_No" type="hidden" value="0" runat="server" />
        <input type="hidden" id="hidCompId" runat="server" />
        <input type="hidden" id="Cid" runat="server" />
        <uc1:Top ID="top1" runat="server" ShowID="nav-8" />
        <div class="rightinfo">
            <div class="info">
                <a href="../jsc.aspx">我的桌面</a>>
                <a href="ContractList.aspx" runat="server" id="A1">合同列表</a>>
                <a href="#" runat="server" id="title">合同编辑</a>
            </div>
            <!--[if !IE]>商品展示区 start<![endif]-->
            <div class="goods-zs">

                <div id="DisBoot" class="tools">
                    <ul class="toolbar left">
                        <li runat="server" onclick="btnCMerchants()"><span>
                            <img src="../images/t02.png" /></span><font>招商信息</font></li>
                    </ul>
                </div>

                <!--[if !IE]>选择代理商 start<![endif]-->
                <div class="c-n-title">基本信息</div>
                <ul class="coreInfo">
                    <li class="lb fl"><i class="name"><i class="red">*</i>合同号</i>
                        <input type="text" class="box1 contractNO" id="txtcontractNO" runat="server" name="txtcontractNO" maxlength="100" />
                    </li>
                    <li class="lb fl"><i class="name"><i class="red">*</i>日期</i>
                        <input name="contractDate" runat="server" onclick="WdatePicker()"
                            id="txtcontractDate" readonly="readonly" type="text" class="box1" value="" />
                    </li>



                    <li class="lb fl">
                        <i class="name "><i class="red">*</i>代理商</i>


                        <asp:DropDownList ID="DropDis" runat="server" CssClass="box1" Width="382"></asp:DropDownList>

                    </li>
                    <li class="lb fl"><i class="name"><i class="red">*</i>状态</i>
                        <select name="CState" runat="server" id="CState" style="width: 382px;" class="box1">
                            <option value="1">启用</option>
                            <option value="2">停用</option>
                        </select>
                    </li>

                    <li class="lb fl"><i class="name"><i class="red">*</i>生效日期</i>
                        <input name="txtForceDate" runat="server" onclick="WdatePicker()"
                            id="txtForceDate" readonly="readonly" type="text" class="box1" value="" />
                    </li>
                    <li class="lb fl"><i class="name"><i class="red">*</i>失效日期</i>
                        <input name="txtInvalidDate" runat="server" onclick="WdatePicker()"
                            id="txtInvalidDate" readonly="readonly" type="text" class="box1" value="" />
                    </li>

                    <li class="lb fl"><div   style="width:90px;float:left;height:82px;line-height:82px;text-align:right">备注</div>
                        
                             <textarea style="width: 890px; height: 80px;margin-left:13px;" rows="3" runat="server" id="txtRemark" class="box1"></textarea>
                        
                       
                    </li>

                </ul>

                <div class="clear"></div>


                
                <div class="c-n-title">商品</div>
                <!--[if !IE]>选择代理商 end<![endif]-->
                <!--商品 start -->
                <div class="tabLine">

                    <table border="0" cellspacing="0" cellpadding="0">
                        <thead>
                            <tr>
                                <th class="t5"></th>
                                <th class="t1">商品名称
                                </th>
                                <th class="t3">规格属性
                                </th>
                                <th class="">医院
                                </th>
                                <th class="">区域
                                </th>
                                <th class="t3">零售价
                                </th>
                                <th class="t5">折扣(%)
                                </th>
                                <th class="t5">价格
                                </th>
                                <th class="t5">年指标(元)
                                </th>
                                <th class="t3">备注
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr runat="server" id="tbodyTR">
                                <td class="t8">
                                    <div class="addg">
                                        <a href="javascript:;" class="minus2"></a><a href="javascript:;" class="add2"></a>
                                    </div>
                                </td>
                                <td>
                                    <div class="search">
                                        <input name="" type="text" class="box project2" readonly="readonly" /><a class="opt-i"></a>
                                        <!--[if !IE]>搜索弹窗 start<![endif]-->
                                        <div class="search-opt none">
                                            <ul class="list"></ul>
                                            <div class="opt">
                                                <a href="javascript:;"><i class="opt2-i"></i>选择商品</a>
                                            </div>
                                        </div>
                                        <!--[if !IE]>搜索弹窗 end<![endif]-->
                                    </div>
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <asp:Repeater ID="RepContractDetail" runat="server">
                                <ItemTemplate>
                                    <tr trindex="<%#Eval("ID")%>" trindex2="<%#Eval("ID")%>" id="<%#Eval("GoodsID")+"_"+Eval("ID")%>" tip="<%#Eval("GoodsInfoID")%>">
                                        <td class="t5">
                                            <div class="addg">
                                                <a href="javascript:;" class="minus2"></a>
                                                <a href="javascript:;" class="add2"></a>
                                            </div>
                                        </td>
                                        <td class="t1">
                                            <div class="sPic" style="width:270px;">
                                                <a class="opt-i2"></a>
                                                <span><a target="_blank" href="../../e<%# Eval("GoodsInfoID") %>_<%=this.CompID %>_.html" onclick="return false;">
                                                    <img src="<%#Eval("pic").ToString()!= "" ?Common.GetWebConfigKey("ImgViewPath")+"GoodsImg/" +Eval("pic").ToString():"../../images/havenopicsmallest.gif"%>" width="60" height="60"></a>
                                                </span><a target="_blank" href="../../e<%# Eval("GoodsInfoID") %>_<%=this.CompID %>_.html" onclick="return false;" class="code">商品编码：<%#Eval("GoodsCode") %> </a>
                                                <a target="_blank" style="width: 200px" href="../../e<%# Eval("GoodsInfoID") %>_<%=this.CompID %>_.html" onclick="return false;" class="name"><%#Eval("GoodsName")%>  <i><%#Eval("GoodsName")%></i></a>
                                            </div>
                                            <input class="hid_GoodsName" type="hidden" value="<%#Eval("GoodsName")%>" />
                                            <input class="hid_GoodsCode" value="<%#Eval("GoodsCode")%>" type="hidden" />
                                            <input class="hid_FirstCampID" type="hidden" value="<%#Eval("FCID")%>">
                                        </td>
                                        <td class="t3 center ValueInfo"><%# Eval("ValueInfo") %></td>
                                        <td class="t3 center">
                                            <a href="javascript:;" class="areHt" style="color: blue;"><%# getHtName(Eval("HtID").ToString()) %></a>
                                            <input class="hid_Htid" type="hidden" value="<%# Eval("HtID") %>" />
                                        </td>
                                        <td class="t3 center">
                                            <a href="javascript:;" class="areArea" style="color: blue;"><%# Eval("AreaID").ToString() == "" ? "选择区域" : Common.GetDisAreaNameById(Eval("AreaID").ToString().ToInt(0))%></a>
                                            <input class="hidAreaID" type="hidden" value="<%# Eval("AreaID") %>" />
                                        </td>
                                        <td class="t5 center">
                                            <span class="SalePrice"><%#Convert.ToDecimal( Eval("SalePrice")).ToString("#.00")%></span>
                                        </td>
                                        <td class="t3 center">
                                            <input type="text" class="box1 discount" onfocus="InputFocus(this)" onkeyup='KeyInt2(this,1)' maxlength="9" style="width: 40px" value="<%# Convert.ToDecimal(Eval("discount")).ToString("#.00")%>">
                                        </td>
                                        <td class="t3 center">
                                            <input type="text" class="box1 txtTinkerPrice" onfocus="InputFocus(this)" onkeyup='KeyInt2(this,2)' maxlength="9" style="width: 60px" value="<%# Eval("TinkerPrice").ToString()=="0.0000"?"0":Convert.ToDecimal(Eval("TinkerPrice")).ToString("#.00")%>">
                                        </td>
                                        <td class="t5 center">
                                            <input type="text" class="box1 target" onfocus="InputFocus(this)" onkeyup='KeyInt2(this,3)' maxlength="9" style="width: 40px" value="<%#  Eval("target").ToString()=="0.0000"?"0": Convert.ToDecimal(Eval("target")).ToString("#.00") %>"></td>
                                        <td class="t3 center ramk">
                                            <div class="tc alink"><a href="javascript:;" class="aremark_<%#Eval("ID") %>">添加</a><%#Eval("Remark") %></div>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
				 <div class="clear"></div>
				<div class="c-n-title">附件</div>
                <ul class="coreInfo">
                    <li class="lb clear">
                       
                        <div class="con upload" >
                            <a href="javascript:;" class="a-upload bclor left">
                                <input id="uploadFile" runat="server" type="file" name="fileAttachment" class="AddBanner" />上传附件</a>
                            <i class="gclor9">（附件最大20M，支持格式：PDF、Word、Excel、Txt、JPG、PNG、BMP、GIF、RAR、ZIP）</i>
                            <asp:Panel runat="server" ID="DFile" Style="margin: 5px 5px;">
                            </asp:Panel>
                        </div>

                        <div id="UpFileText" style="margin-left: 70px;">
                        </div>
                        <input runat="server" id="HidFfileName" type="hidden" />
                    </li>
                </ul>
                <div class="clear"></div>	
            </div>
            <div class="blank20">
            </div>
            <div class="blank20">
            </div>
            <div class="blank20">
            </div>
            <div class="blank20">
            </div>
            <div class="blank20">
            </div>
            <div class="blank20">
            </div>
            <div class="btn-box">

                <div class="btn">
                    <a href="javascript:;" class="btn-area" id="Tj" runat="server">保存</a><a href="#" id="returns" class="gray-btn">返回</a>
                </div>
                <div class="bg">
                </div>
            </div>
            <div id="divGoodsName" class="divGoodsName" runat="server" style="display: none">
            </div>
        </div>

    </form>
</body>
</html>
