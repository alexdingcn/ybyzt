<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StorageAdd.aspx.cs" Inherits="Distributor_Storage_StorageAdd" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>入库编辑</title>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <link href="../newOrder/css/global2.5.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"
        rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/autoTextarea.js" type="text/javascript"></script>
    <style type="text/css">
        .search {
            width: 90%;
        }

        .xl {
            width: 160px;
            border: 1px solid #d1d1d1;
            color: #494949;
            font-size: 12px;
            height: 27px;
            line-height: 27px;
            float: left;
            overflow: hidden;
            border-radius: 5px;
            margin-right: 10px;
        }

        tr input {
            text-align: center;
            font-size: 12px;
            color: #494949;
        }
         .coreInfo .box1 {
            width: 360px;
        }

        .box1, .box2 {
            border: 1px solid #ddd;
            border-radius: 5px;
            height: 34px;
            line-height: 34px;
            padding: 0px 10px;
            color: #555;
            font-size: 12px;
        }

        .coreInfo .lb {
            padding-top: 10px;
            min-width: 482px;
            color: #666;
            position: relative;
            margin-right: 30px;
            min-height: 36px;
        }

        .coreInfo .name {
            width: 90px;
            text-align: right;
            display: inline-block;
            padding-right: 10px;
            line-height: 35px;
            color: #666;
        }

        .search {
            width: 90%;
        }
         .fl {
            float: left;
        }

        li {
            list-style-type: none;
        }

        input {
            outline: none;
            star: expression(this.onFocus=this.blur());
        }
         .toolbar li {
            background: url(../../Company/images/toolbg.gif) repeat-x;
            line-height: 28px;
            height: 28px;
            border: solid 1px #c5d2d8;
            float: left;
            padding-right: 10px;
            margin-right: 5px;
            border-radius: 3px;
            behavior: url(js/pie.htc);
            cursor: pointer;
        }       
         .toolbar li span {
            float: left;
            margin-left: 10px;
            margin-right: 5px;
            margin-top: 5px;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <Head:Head ID="Head2" runat="server" />
        <div class="w1200">
            <input type="hidden" runat="server" id="hid_Alert" value="" />
            <input type="hidden" runat="server" id="hidStorageType" value="" />
            <input type="hidden" id="StorageID" value="" runat="server" />
            <Left:Left ID="Left1" runat="server" ShowID="nav-4" />
            <div class="rightCon">
                <div class="info">
                    <a id="navigation1" href="/Distributor/UserIndex.aspx">我的桌面</a>>
                    <a id="navigation2" href="StorageList.aspx" class="cur">入库单列表</a>
                </div>
                <div class="blank10"></div>
                <!--[if !IE]>商品展示区 start<![endif]-->
                <!--[if !IE]>选择厂商 start<![endif]-->
                <div class="jxs-box left">
                    <ul class="toolbar left">
                        <li class="btnSingle"><span>
                            <img src="../../Company/images/t02.png"  /></span><font>生单</font></li>
                    </ul>
                </div>
                 <div class="blank10"></div>
                <ul class="coreInfo">
                    <li class="lb fl"><i class="name"><i class="red">*</i>入库单号</i>
                        <input type="text" class="box1 " id="StorageNO" runat="server"  maxlength="20" />
                    </li>
                    <li class="lb fl"><i class="name"><i class="red">*</i>入库日期</i>
                         <input type="text" class=" box1" id="txtStorageDate" runat="server" readonly="readonly"
                                onclick="WdatePicker({minDate:'%y-%M-%d'})"  />
                    </li>
                    <li class="lb fl">
                         <i class="name ">厂 商</i>
                         <select id="ddrComp" name="" runat="server" class="box1" style="width: 382px;"></select>
                    </li>
                    <li class="lb fl">
                        <i class="name ">入库类型</i>
                        <select name="CState" runat="server" id="lblStorageType1" style="width: 382px;" class="box1">
                            <option value="1">采购入库</option>
                            <option value="2">其它入库</option>
                        </select>
                    </li>
                   
                </ul>

                <div class="blank10"></div>




                <!--[if !IE]>选择厂商 end<![endif]-->
                <div class="goods-zs">
                    <!--商品 start -->
                    <div class="tabLine">
                        <table border="0" cellspacing="0" cellpadding="0">
                            <thead>
                                <tr>
                                    <th class="t5"></th>
                                    <th class="">商品名称
                                    </th>
                                    <th class="t3">规格属性
                                    </th>
                                    <th class="t3">单位
                                    </th>
                                    <th>批次
                                    </th>
                                    <th>有效期
                                    </th>
                                    <th class="t3">入库数量
                                    </th>
                                    <th class="t3">备注
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="Rep_StorageDetail" runat="server">
                                    <ItemTemplate>
                                        <tr trindex="1" tip="0">
                                            <td class="t8">
                                                <div class="addg">
                                                    <a href="javascript:;" class="minus2"></a>
                                                    <a href="javascript:;" class="add2" tip="alast"></a>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="search">
                                                    <input name="GoodsName" value="<%#Eval("GoodsName") %>" type="text" class="box GoodsName" maxlength="15" />
                                                    <input type="hidden" value="<%#Eval("GoodsCode") %>" class="GoodsCode" value="0" />
                                                </div>
                                            </td>
                                            <td class="t2">
                                                <div class="search">
                                                    <input name="ValueInfo" value="<%#Eval("ValueInfo") %>" type="text" class="box ValueInfo" maxlength="15" />
                                                </div>
                                            </td>
                                            <td>
                                                <div class="search">
                                                    <input name="Unit" value="<%#Eval("Unit") %>" type="text" class="box Unit" maxlength="4" />
                                                </div>
                                            </td>
                                            <td class="t4">
                                                <div class="search">
                                                    <input name="BatchNO" value="<%#Eval("BatchNO") %>" type="text" class="box BatchNO" maxlength="15" />
                                                </div>
                                            </td>
                                            <td>
                                                <div class="search">
                                                    <input type="text" class="Wdate validDate" readonly="readonly"
                                                        onclick="WdatePicker({minDate:'%y-%M-%d'})" value="<%#Convert.ToDateTime( Eval("validDate")).ToString("yyyy-MM-dd") %>" />
                                                </div>
                                            </td>
                                            <td>
                                                <div class="search">
                                                    <input name="StorageNum" value="<%#Convert.ToDecimal( Eval("StorageNum")).ToString("#0.00") %>" onblur="KeyInt(this,1)" onkeyup="KeyInt(this,1)" type="text" class="box StorageNum" maxlength="7" />
                                                </div>
                                            </td>
                                            <td>
                                                <div class="search">
                                                    <input name="Remark" value="<%#Eval("Remark") %>" type="text" class="box Remark" maxlength="20" />
                                                </div>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <tr trindex="1" tip="0" runat="server" id="oneTR">
                                    <td class="t8">
                                        <div class="addg">
                                            <a href="javascript:;" class="minus2"></a>
                                            <a href="javascript:;" class="add2" tip="alast"></a>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="search">
                                            <input name="GoodsName" type="text" class="box GoodsName" maxlength="15" />
                                            <input type="hidden" class="GoodsCode" value="0" />
                                        </div>
                                    </td>
                                    <td class="t2">
                                        <div class="search">
                                            <input name="ValueInfo" type="text" class="box ValueInfo" maxlength="15" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="search">
                                            <input name="Unit" type="text" class="box Unit" maxlength="4" />
                                        </div>
                                    </td>
                                    <td class="t4">
                                        <div class="search">
                                            <input name="BatchNO" type="text" class="box BatchNO" maxlength="15" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="search">
                                            <input type="text" class="Wdate validDate" readonly="readonly"
                                                onclick="WdatePicker({minDate:'%y-%M-%d'})" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="search">
                                            <input name="StorageNum" type="text" class="box StorageNum" maxlength="7" onblur="KeyInt(this,1)" onkeyup="KeyInt(this,1)"/>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="search">
                                            <input name="Remark" type="text" class="box Remark" maxlength="20" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <!--商品 end -->
                </div>
                <!--[if !IE]>商品展示区end<![endif]-->
                <div class="blank20"></div>
                <!--[if !IE]>下单信息 start<![endif]-->
                <div class="goods-info">
                   
                    <div class="bz remark">
                        <i class="bt">备 注：</i><div class="txt_box">
                            <textarea id="OrderNote" runat="server" name="OrderNote" maxlength="200" class="box"
                                placeholder="备注不能超过200个字"></textarea>
                        </div>
                    </div>
                </div>
                <!--[if !IE]>下单信息 end<![endif]-->
                <div class="blank20"></div>
                <div class="btn-box">
                    <div class="btn" runat="server" id="Btn">
                        <a href="javascript:;" class="btn-area" id="Tj" tip="2">提交</a>
                        <a href="javascript:;" class="gray-btn2 btnCancel">取消</a>
                    </div>
                    <div class="bg">
                    </div>
                </div>
            </div>

        </div>
        <div class="po-bg2 none" style="z-index: 999999; background: #fffff">
        </div>
        <div id="p-delete" class="popup2 p-delete2 none" style="z-index: 9999999">
            <img src="../../js/layer/skin/default/loading-0.gif" />
        </div>

        <script>
            $(function () {
                //配送方式
                $(".carry .menu a").click(function () {
                    var type = $.trim($("#lblStorageType1").text());
                    if (type == "采购入库") {
                        $("#hidStorageType").val("2");
                        $("#lblStorageType1").text("其他入库");
                        $("#lblStorageType2").text("采购入库");
                    } else {
                        $("#hidStorageType").val("1");
                        $("#lblStorageType2").text("其他入库");
                        $("#lblStorageType1").text("采购入库");
                    }
                });

                ///取消
                $(document).on("click", ".btnCancel", function () {
                    window.location.href = 'StorageList.aspx';
                });

               
                //加行
                $(document).on("click", ".minus2", function () {
                    var trIndex = getTrindex();
                    var html = '<tr trindex="' + trIndex + '"  tip="0"><td class="t8"><div class="addg"><a href="javascript:;" class="minus2"></a><a href="javascript:;" class="add2" tip="alast"></a></div></td><td><div class="search"><input name="GoodsName" type="text" class="box GoodsName"  maxlength="15"/><input  type="hidden" class="GoodsCode" value="0"/></div></td><td class="t2"><div class="search"><input name="ValueInfo" type="text" class="box ValueInfo" maxlength="15"/></td><td><div class="search"><input name="Unit" type="text" class="box Unit" maxlength="4"/></div></td><td class="t4"><div class="search"><input name="BatchNO" type="text" class="box BatchNO" maxlength="15"/></div></td><td><div class="search"><input type="text" class="Wdate validDate" readonly="readonly"onclick="WdatePicker({minDate:\'%y-%M-%d\'})" /></div></td><td><div class="search"><input name="StorageNum" onblur="KeyInt(this,1)" onkeyup="KeyInt(this,1)" type="text" class="box StorageNum" maxlength="7"/></div></td><td><div class="search"><input name="Remark" type="text" class="box Remark" maxlength="20"/></div></td></tr>';
                    $(".tabLine table tbody").append(html);
                });
                //减行
                $(document).on("click", ".add2", function () {
                    if ($(".tabLine table tbody tr").length > 1) {
                        $(this).parent().parent().parent().remove(); //大于1行时直接删除
                    } else {
                        var trIndex = getTrindex();
                        var html = '<tr trindex="' + trIndex + '"  tip="0"><td class="t8"><div class="addg"><a href="javascript:;" class="minus2"></a><a href="javascript:;" class="add2" tip="alast"></a></div></td><td><div class="search"><input name="GoodsName" type="text" class="box GoodsName"  maxlength="15"/><input  type="hidden" class="GoodsCode" value="0"/></div></td><td class="t2"><div class="search"><input name="ValueInfo" type="text" class="box ValueInfo" maxlength="15"/></td><td><div class="search"><input name="Unit" type="text" class="box Unit" maxlength="4"/></div></td><td class="t4"><div class="search"><input name="BatchNO" type="text" class="box BatchNO" maxlength="15"/></div></td><td><div class="search"><input type="text" class="Wdate validDate" readonly="readonly"onclick="WdatePicker({minDate:\'%y-%M-%d\'})" /></div></td><td><div class="search"><input name="StorageNum" onblur="KeyInt(this,1)" onkeyup="KeyInt(this,1)" type="text" class="box StorageNum" maxlength="7"/></div></td><td><div class="search"><input name="Remark" type="text" class="box Remark" maxlength="20"/></div></td></tr>';
                        $(this).parent().parent().parent().remove(); //小于等于1时 先删除 再添加一个空的html
                        $(".tabLine table tbody").append(html);
                    }
                });

                //生单
                $(document).on("click", ".btnSingle", function () {
                    var compId = $("#ddrComp").val();
                    if (compId === "" || compId == undefined) {
                        layerCommon.msg("请选择厂商", IconOption.哭脸);
                        return false;
                    }
                    var url = 'SelectOrderOut.aspx?cid=' + compId;
                    var index = layerCommon.openWindow("选择商品", url, "1100px", "680px");  //记录弹出对象
                    $("#hid_Alert").val(index); //记录弹出对象
                });

                //提交
                $("#Tj").click(function () {
                    var ddrCompid = $("#ddrComp").val();//厂商ID
                    var StorageType = $("#lblStorageType1").val();//入库类型
                    var txtStorageDate = $("#txtStorageDate").val();//入库日期
                    var StorageNO = $("#StorageNO").val();//入库单号
                    var OrderNote = $("#OrderNote").val();//备注
                    var StorageID = $("#StorageID").val();//ID
                    if (StorageNO == "" || StorageNO == undefined) { layerCommon.msg("入库单号不能为空", IconOption.哭脸); return false; }
                    if (txtStorageDate == "" || txtStorageDate == undefined) { layerCommon.msg("入库日期不能为空", IconOption.哭脸); return false; }
                  
                    var msg = "";
                    var index = 0;//用来判断是否是第一条有效的商品数据（排除空行）
                    var json = "[{\"StorageNO\":\"" + StorageNO + "\",\"ddrCompid\":\"" + ddrCompid + "\",\"StorageType\":\"" + StorageType + "\",\"txtStorageDate\":\"" + txtStorageDate + "\",\"StorageID\":\"" + StorageID + "\",\"OrderNote\":\"" + OrderNote + "\",\"orderdetail\":[";
                    $(".tabLine tbody tr").each(function (i, obj) {
                        var tip = $(".tabLine tbody tr").eq(i).attr("tip");
                        index++;
                        var GoodsName = $.trim($(".tabLine tbody tr").eq(i).find(".GoodsName").val()); //商品名称
                        if (GoodsName != undefined && GoodsName != "") {

                            var GoodsCode = $.trim($(".tabLine tbody tr").eq(i).find(".GoodsCode").val()); //商品编码
                            var ValueInfo = $.trim($(".tabLine tbody tr").eq(i).find(".ValueInfo").val()); //规格型号
                            var Unit = $.trim($(".tabLine tbody tr").eq(i).find(".Unit").val()); //单位
                            var BatchNO = $.trim($(".tabLine tbody tr").eq(i).find(".BatchNO").val()); //批次号
                            var validDate = $.trim($(".tabLine tbody tr").eq(i).find(".validDate").val()); //有效期
                            var StorageNum = $.trim($(".tabLine tbody tr").eq(i).find(".StorageNum").val()); //入库数量
                            var Remark = $.trim($(".tabLine tbody tr").eq(i).find(".Remark").val()); //备注
                            if (validDate == "" || validDate == undefined) { msg = "有效期不能为空"; return false; }
                           // if (ValueInfo == "" || ValueInfo == undefined) { msg = "请输入规格型号"; return false; }
                            if (StorageNum == "" || StorageNum == undefined) { msg = "请输入入库数量"; return false; }
                            if (index == 1) {
                                json += "{\"tip\":\"" + tip + "\",\"GoodsName\":\"" + GoodsName + "\",\"GoodsCode\":\"" + GoodsCode + "\",\"ValueInfo\":\"" + ValueInfo + "\",\"Unit\":\"" + Unit + "\",\"BatchNO\":\"" + BatchNO + "\",\"validDate\":\"" + validDate + "\",\"StorageNum\":\"" + StorageNum + "\",\"Remark\":\"" + Remark + "\"}";
                            } else {
                                json += ",{\"tip\":\"" + tip + "\",\"GoodsName\":\"" + GoodsName + "\",\"GoodsCode\":\"" + GoodsCode + "\",\"ValueInfo\":\"" + ValueInfo + "\",\"Unit\":\"" + Unit + "\",\"BatchNO\":\"" + BatchNO + "\",\"validDate\":\"" + validDate + "\",\"StorageNum\":\"" + StorageNum + "\",\"Remark\":\"" + Remark + "\"}";
                            }
                        } else {
                            index--;
                        }
                    })
                    json += "]}]";
                    if (msg != "" && msg != undefined) { layerCommon.msg(msg, IconOption.哭脸); return false; }
                    if (index == 0) { layerCommon.msg("请最少选择一行商品", IconOption.哭脸) }
                    else {
                        var actiontype = "disStorageEdit";
                        $.ajax({
                            type: "post",
                            url: "../../Handler/orderHandle.ashx",
                            data: { ActionType: actiontype, json: json },
                            dataType: "text",
                            success: function (data) {
                                var json = JSON.parse(data);
                                if (json.returns == "true") {
                                    layerCommon.msg("操作成功", IconOption.笑脸);
                                    window.location = "StorageInfo.aspx?KeyID=" + json.StorageID + "";
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

                    }

                });
            })




            //绑定数据（商品id，无用字段,当前选择行的索引,无用字段）
            function GoodsList(OrderOutDetailID) {
                if (OrderOutDetailID != "") {
                var xyindex = 0; //最大的索引
                //冒泡排序获取最大的索引
                //如果间隔行去选择商品 则对应不到对应的价格 所以采用了下面这个方法
                $.ajax({
                    type: "post",
                    data: { ck: Math.random(), action: "goodsInfo", OrderOutDetailID: OrderOutDetailID },
                    dataType: "json",
                    success: function (data) {
                        var html = ""; //绑定的商品数据
                        var trIndex = getTrindex(); //获取选中过来的商品数量 便于相加索引
                            $(data).each(function (indexs, obj) {
                                html += '<tr trindex="' + trIndex + '" tip="' + obj.id + '"><td class="t8"><div class="addg"><a href="javascript:;" class="minus2"></a><a href="javascript:;" class="add2" tip="alast"></a></div></td><td><div class="search"><input name="GoodsName"  type="text" class="box GoodsName" value="' + obj.GoodsName + '"  maxlength="15"/><input  type="hidden" class="GoodsCode" value="' + obj.GoodsCode + '"/></div></td><td class="t2"><div class="search"><input name="ValueInfo" type="text" value="' + obj.ValueInfo + '" class="box ValueInfo" maxlength="15"/></td><td><div class="search"><input name="Unit" value="' + obj.Unit + '" type="text" class="box Unit" maxlength="4"/></div></td><td class="t4"><div class="search"><input name="BatchNO" value="' + obj.BatchNO + '" type="text" class="box BatchNO" maxlength="15"/></div></td><td><div class="search"><input type="text" class="Wdate validDate" value="' + obj.validDate + '" readonly="readonly"onclick="WdatePicker({minDate:\'%y-%M-%d\'})" /></div></td><td><div class="search"><input name="StorageNum" onblur="KeyInt(this,1)" onkeyup="KeyInt(this,1)" value="' + obj.SignNum + '" type="text" class="box StorageNum" maxlength="7"/></div></td><td><div class="search"><input name="Remark" value="' + obj.Remark + '" type="text" class="box Remark" maxlength="20"/></div></td></tr>';
                                trIndex + 1;
                            })
                            $(".tabLine table tbody ").prepend(html);
                    }
                })
            }
        }

            function getTrindex() {
                var xyindex = 0;
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
                return xyindex*1+1;
            }

            //关闭弹窗
            function closeAll() {
                layer.closeAll();
            }
        </script>
    </form>
</body>
</html>
