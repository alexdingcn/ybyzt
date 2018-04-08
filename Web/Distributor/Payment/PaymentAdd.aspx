<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PaymentAdd.aspx.cs" Inherits="Distributor_Payment_PaymentAdd" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>收款单编辑</title>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <link href="../newOrder/css/global2.5.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"
        rel="stylesheet" type="text/css" />

    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../newOrder/js/order.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
    <script src="../../js/UploadJs.js" type="text/javascript"></script>

    <style type="text/css">
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

        .jxs-box .bt {
            width: 80px;
            border: 1px solid blue;
        }

        .jxs-box .s {
            width: 430px;
            border: 1px solid blue;
        }

        .opt-i2 {
            margin-top: -20px;
        }

        ul, li {
            margin: 0;
            padding: 0;
            border: 0;
            font-style: normal;
            font-family: '微软雅黑', 'microsoft yahei', 宋体, Tahoma, Verdana, Arial, Helvetica, sans-serif;
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
        
        .tools {
            clear: both;
            overflow: hidden;
            margin-bottom: 8px;
        }

        .left {
            float: left;
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
        }        .toolbar li span {
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
            <input type="hidden" id="PaymentID" value="" runat="server" />
            <Left:Left ID="Left1" runat="server" ShowID="nav-4" />
            <div class="rightCon">
                <div class="info">
                    <a id="navigation1" href="/Distributor/UserIndex.aspx">我的桌面</a>>
                    <a id="navigation2" href="PaymentList.aspx" class="cur">收款单编辑</a>
                </div>
                 <div id="DisBoot" class="tools">
                    <ul class="toolbar left">
                        <li runat="server" onclick="btnCMerchants()"><span>
                            <img src="../../Company/images/t02.png"  /></span><font>生单</font></li>
                    </ul>
                </div>
                <div class="blank10"></div>
                <ul class="coreInfo">
                    <li class="lb fl"><i class="name"><i class="red">*</i>收款单号</i>
                        <input type="text" class="box1 PaymentNO" id="PaymentNO" runat="server" name="PaymentNO" maxlength="20" />
                    </li>
                    <li class="lb fl"><i class="name"><i class="red">*</i>收款日期</i>
                        <input name="PaymentDate" runat="server" onclick="WdatePicker()"
                            id="PaymentDate" readonly="readonly" type="text" class="box1" value="" />
                    </li>
                    <li class="lb fl"><i class="name"><i class="red">*</i>医院</i>
                        <asp:DropDownList ID="HtDrop" runat="server" CssClass="box1 ddrht"  Width="382"></asp:DropDownList>
                    </li>


                    <li class="lb fl">
                        <i class="name ">状态</i>
                       <select name="CState" runat="server" id="IState" style="width: 382px;" class="box1" disabled="disabled">
                            <option value="0">新增</option>
                            <option value="1">审核</option>
                        </select>
                    </li>


                    <li class="lb fl"><i class="name">金额</i>
                        <input type="text" class="box1 "  readonly="readonly" id="PaymentAmount" onblur="MoneyYZ(this)" onkeyup="MoneyYZ(this)" runat="server" maxlength="20" />
                    </li>
                    <li class="lb fl"><i class="name">备注</i>
                        <input type="text" class="box1 Remark" id="Remark" runat="server" name="Salesman" maxlength="20" />
                    </li>

                </ul>

                <div class="blank20"></div>

                                <div class="blank20"></div>
                
                <div class="blank10"></div>


                <div class="goods-zs">
                    <!--商品 start -->
                    <div class="tabLine">
                        <table border="0" cellspacing="0" cellpadding="0">
                            <thead>
                                <tr>
                                    <th class="t5" style="text-align: center"></th>
                                    <th class="t2">商品名称
                                    </th>
                                    <th class="t4">规格属性
                                    </th>
                                    <th class="t4">单位
                                    </th>
                                    <th class="t4">批次
                                    </th>
                                    <th class="t5">有效期
                                    </th>
                                    <th class="t4">数量
                                    </th>
                                    <th class="t4">单价
                                    </th>
                                    <th class="t4">金额
                                    </th>
                                    <th class="t4">备注
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="Rep_StorageDetail" runat="server">
                                    <ItemTemplate>
                                        <tr trindex="1" tip="<%#Eval("id") %>">
                                            <td class="t3">
                                                <div class="addg">
                                                    <a href="javascript:;" class="minus2"></a>
                                                    <a href="javascript:;" class="add2" tip="alast"></a>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="search">
                                                    <a class="opt-i2"></a>
                                                    <input name="GoodsName" value="<%#Eval("GoodsName") %>" type="text" class="box GoodsName" maxlength="15" readonly="readonly" />
                                                    <input type="hidden" value="<%#Eval("GoodsCode") %>" class="GoodsCode" value="0" />
                                                </div>
                                            </td>
                                            <td class="t2">
                                                <div class="search">
                                                    <input name="ValueInfo" value="<%#Eval("ValueInfo") %>" type="text" class="box ValueInfo" maxlength="15" readonly="readonly" />
                                                </div>
                                            </td>
                                            <td>
                                                <div class="search">
                                                    <input name="Unit" value="<%#Eval("Unit") %>" type="text" class="box Unit" maxlength="4" readonly="readonly" />
                                                </div>
                                            </td>
                                            <td class="t4">
                                                <div class="search">
                                                    <input name="BatchNO" value="<%#Eval("BatchNO") %>" type="text" class="box BatchNO" maxlength="15" readonly="readonly" />
                                                </div>
                                            </td>
                                            <td>
                                                <div class="search">
                                                    <input type="text" class="Wdate validDate" readonly="readonly"
                                                        value="<%#Convert.ToDateTime( Eval("validDate")).ToString("yyyy-MM-dd") %>" />
                                                </div>
                                            </td>
                                            <td>
                                                <div class="search">
                                                    <input name="OutNum" value="<%#Convert.ToDecimal(Eval("Num")).ToString("#0") %>" onblur="KeyInt(this,0)" onkeyup="KeyInt(this,0)" type="text" class="box OutNum" maxlength="7" />
                                                </div>
                                            </td>
                                            <td>
                                                <div class="search">
                                                    <input name="OutNum" id="AuditAmount<%#Eval("id")%>" value="<%#Convert.ToDecimal( Eval("AuditAmount")).ToString("#0.00") %>" onblur="MoneyYZ(this)" onkeyup="MoneyYZ(this)" type="text" class="box AuditAmount" maxlength="7" />
                                                </div>
                                            </td>
                                            <td>
                                                <div class="search">
                                                    <input name="OutNum" id="sumAmount<%#Eval("id")%>" value="<%#Convert.ToDecimal(Eval("sumAmount")).ToString("#0.00") %>" type="text" class="box sumAmount" maxlength="7" />
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
                                    <td>
                                        <div class="addg">
                                            <a href="javascript:;" class="minus2"></a>
                                            <a href="javascript:;" class="add2" tip="alast"></a>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="search">
                                            <a class="opt-i2"></a>
                                            <input name="GoodsName" type="text" class="box GoodsName" maxlength="15" readonly="readonly" />

                                            <input type="hidden" class="GoodsCode" value="0" />
                                        </div>
                                    </td>
                                    <td class="">
                                        <div class="search">
                                            <input name="ValueInfo" type="text" class="box ValueInfo" maxlength="15" readonly="readonly" />
                                        </div>
                                    </td>
                                    <td class="">
                                        <div class="search">
                                            <input name="Unit" type="text" class="box Unit" maxlength="4" readonly="readonly" />
                                        </div>
                                    </td>
                                    <td class="">
                                        <div class="search">
                                            <input name="BatchNO" type="text" class="box BatchNO" maxlength="15" readonly="readonly" />
                                        </div>
                                    </td>
                                    <td class="">
                                        <div class="search">
                                            <input type="text" class="Wdate validDate" readonly="readonly" />
                                        </div>
                                    </td>
                                    <td class="">
                                        <div class="search">
                                            <input name="OutNum" type="text" class="box OutNum" onblur="KeyInt(this,0)" onkeyup="KeyInt(this,0)" maxlength="7" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="search">
                                            <input type="text" id="AuditAmount0" class="box AuditAmount" maxlength="7" onblur="MoneyYZ(this)" onkeyup="MoneyYZ(this)" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="search">
                                            <input type="text" id="sumAmount0" class="box sumAmount" maxlength="7" readonly="readonly" />
                                        </div>
                                    </td>
                                    <td class="">
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
                      <div class="blank20"></div>

                      <ul class="coreInfo" style="margin-left: 30px;">
                    <li class="lb clear">

                        <div class="con upload" style="margin-top: 8px;">
                            <a href="javascript:;" class="a-upload bclor le">
                                <input id="uploadFile" runat="server" type="file" name="fileAttachment" class="AddBanner"  style="height:40px;"/>上传附件</a>
                            <i class="gclor9">（附件最大20M，支持格式：PDF、Word、Excel、Txt、JPG、PNG、BMP、GIF、RAR、ZIP）</i>
                            <asp:Panel runat="server" ID="DFile" Style="margin: 5px 5px;">
                            </asp:Panel>
                        </div>

                        <div id="UpFileText" style="margin-left: 70px;">
                        </div>
                        <input runat="server" id="HidFfileName" type="hidden" />
                    </li>
                </ul>
                           <div class="blank20"></div>

                <div class="blank20">
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
                    $("#uploadFile").AjaxUploadFile({ Src: "UploadFile/", ShowDiv: "UpFileText", ResultId: "HidFfileName", AjaxSrc: "../../Controller/Fileup.ashx", maxlength: 5, DownSrc: "../../" });
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
                        window.location.href = 'PaymentList.aspx';
                    });

                    $(document).on("change", ".ddrht", function () {
                        $(".tabLine table tbody").html("");
                        var trIndex = 0;
                        var html = '<tr trindex="' + trIndex + '"  tip="0"><td><div class="addg"><a href="javascript:;" class="minus2"></a><a href="javascript:;" class="add2" tip="alast"></a></div></td><td><div class="search"><a class="opt-i2"></a><input name="GoodsName" type="text" class="box GoodsName"  maxlength="15" readonly="readonly"/><input  type="hidden" class="GoodsCode" value="0"/></div></td><td><div class="search"><input name="ValueInfo" type="text" class="box ValueInfo" maxlength="15" readonly="readonly"/></td><td><div class="search"><input name="Unit" type="text" class="box Unit" maxlength="4" readonly="readonly"/></div></td><td><div class="search"><input name="BatchNO" type="text" class="box BatchNO" maxlength="15" readonly="readonly"/></div></td><td><div class="search"><input type="text" class="Wdate validDate" readonly="readonly" /></div></td><td><div class="search"><input name="OutNum" type="text" class="box OutNum" onblur="KeyInt(this,0)"  onkeyup="KeyInt(this,0)" maxlength="7"/></div></td><td><div class="search"><input type="text" id="AuditAmount' + trIndex + '" class="box AuditAmount" maxlength="7"  onblur="MoneyYZ(this)"  onkeyup="MoneyYZ(this)"/> </div></td> <td ><div class="search"><input type="text" id="sumAmount' + trIndex + '"  class="box sumAmount" maxlength="7" readonly="readonly"/></div></td><td><div class="search"><input name="Remark" type="text" class="box Remark" maxlength="20"/></div></td></tr>';
                        $(".tabLine table tbody").html(html);
                    });

                    //加行
                    $(document).on("click", ".minus2", function () {
                        var trIndex = getTrindex();
                        var html = '<tr trindex="' + trIndex + '"  tip="0"><td><div class="addg"><a href="javascript:;" class="minus2"></a><a href="javascript:;" class="add2" tip="alast"></a></div></td><td><div class="search"><a class="opt-i2"></a><input name="GoodsName" type="text" class="box GoodsName"  maxlength="15" readonly="readonly"/><input  type="hidden" class="GoodsCode" value="0"/></div></td><td><div class="search"><input name="ValueInfo" type="text" class="box ValueInfo" maxlength="15" readonly="readonly"/></td><td><div class="search"><input name="Unit" type="text" class="box Unit" maxlength="4" readonly="readonly"/></div></td><td><div class="search"><input name="BatchNO" type="text" class="box BatchNO" maxlength="15" readonly="readonly"/></div></td><td><div class="search"><input type="text" class="Wdate validDate" readonly="readonly" /></div></td><td><div class="search"><input name="OutNum" type="text" class="box OutNum" onblur="KeyInt(this,0)"  onkeyup="KeyInt(this,0)" maxlength="7"/></div></td><td><div class="search"><input type="text" id="AuditAmount' + trIndex + '" class="box AuditAmount" maxlength="7"  onblur="MoneyYZ(this)"  onkeyup="MoneyYZ(this)"/> </div></td> <td ><div class="search"><input type="text" id="sumAmount' + trIndex + '"  class="box sumAmount" maxlength="7" readonly="readonly"/></div></td><td><div class="search"><input name="Remark" type="text" class="box Remark" maxlength="20"/></div></td></tr>';
                        $(".tabLine table tbody").append(html);
                    });
                    //减行
                    $(document).on("click", ".add2", function () {
                        if ($(".tabLine table tbody tr").length > 1) {
                            $(this).parent().parent().parent().remove(); //大于1行时直接删除
                        } else {
                            var trIndex = getTrindex();
                            var html = '<tr trindex="' + trIndex + '"  tip="0"><td><div class="addg"><a href="javascript:;" class="minus2"></a><a href="javascript:;" class="add2" tip="alast"></a></div></td><td><div class="search"><a class="opt-i2"></a><input name="GoodsName" type="text" class="box GoodsName"  maxlength="15" readonly="readonly"/><input  type="hidden" class="GoodsCode" value="0"/></div></td><td><div class="search"><input name="ValueInfo" type="text" class="box ValueInfo" maxlength="15" readonly="readonly"/></td><td><div class="search"><input name="Unit" type="text" class="box Unit" maxlength="4" readonly="readonly"/></div></td><td><div class="search"><input name="BatchNO" type="text" class="box BatchNO" maxlength="15" readonly="readonly"/></div></td><td><div class="search"><input type="text" class="Wdate validDate" readonly="readonly" /></div></td><td><div class="search"><input name="OutNum" type="text" class="box OutNum" onblur="KeyInt(this,0)"  onkeyup="KeyInt(this,0)" maxlength="7"/></div></td><td><div class="search"><input type="text" id="AuditAmount' + trIndex + '" class="box AuditAmount" maxlength="7"  onblur="MoneyYZ(this)"  onkeyup="MoneyYZ(this)"/> </div></td> <td ><div class="search"><input type="text" id="sumAmount' + trIndex + '"  class="box sumAmount" maxlength="7" readonly="readonly"/></div></td><td><div class="search"><input name="Remark" type="text" class="box Remark" maxlength="20"/></div></td></tr>';
                            $(this).parent().parent().parent().remove(); //小于等于1时 先删除 再添加一个空的html
                            $(".tabLine table tbody").append(html);
                        }
                        getPaymentAmount();
                    });

                    //选择商品
                    $(document).on("click", ".opt-i2", function () {
                        var inindex = $(this).parents("tr").index(); //当前行索引
                        var htid = $.trim($("#HtDrop").val());
                        var url = 'SelectStorageList.aspx?index=' + inindex + "&id=" + htid;
                        var index = layerCommon.openWindow("选择商品", url, "1100px", "630px");  //记录弹出对象
                        $("#hid_Alert").val(index); //记录弹出对象
                    });

                    //提交
                    $("#Tj").click(function () {
                        var PaymentNO = $("#PaymentNO").val(); //收款单号
                        var PaymentDate = $("#PaymentDate").val(); //日期
                        var IState = $("#IState").val(); //状态
                        var HtDrop = $("#HtDrop").val(); //医院
                        var PaymentAmount = $("#PaymentAmount").val(); //收款金额
                        var Remark = $("#Remark").val(); //备注
                        var PaymentID = $("#PaymentID").val(); //ID

                        var HidFfileName = $("#HidFfileName").val(); //附件 

                        if (PaymentNO == "" || PaymentNO == undefined) { layerCommon.msg("单号不能为空", IconOption.哭脸); return false; }
                        if (PaymentDate == "" || PaymentDate == undefined) { layerCommon.msg("日期不能为空", IconOption.哭脸); return false; }
                        if (PaymentAmount == "" || PaymentAmount == undefined) { layerCommon.msg("收款金额不能为空", IconOption.哭脸); return false; }
                        if (HtDrop == "" || HtDrop == undefined) { layerCommon.msg("请选择医院", IconOption.哭脸); return false; }


                        var msg = "";
                        var index = 0; //用来判断是否是第一条有效的商品数据（排除空行）
                        var json = "[{\"PaymentNO\":\"" + PaymentNO + "\",\"PaymentDate\":\"" + PaymentDate + "\",\"IState\":\"" + IState + "\",\"HtDrop\":\"" + HtDrop + "\",\"PaymentAmount\":\"" + PaymentAmount + "\",\"Remark\":\"" + Remark + "\",\"PaymentID\":\"" + PaymentID + "\",\"HidFfileName\":\"" + HidFfileName + "\",\"orderdetail\":[";
                        $(".tabLine tbody tr").each(function (i, obj) {
                            var tip = $(".tabLine tbody tr").eq(i).attr("tip");

                            index++;
                            if (tip != undefined && tip != "" && tip != "0") {
                                var GoodsName = $.trim($(".tabLine tbody tr").eq(i).find(".GoodsName").val()); //商品名称
                                var GoodsCode = $.trim($(".tabLine tbody tr").eq(i).find(".GoodsCode").val()); //商品编码
                                var ValueInfo = $.trim($(".tabLine tbody tr").eq(i).find(".ValueInfo").val()); //规格型号
                                var Unit = $.trim($(".tabLine tbody tr").eq(i).find(".Unit").val()); //单位
                                var BatchNO = $.trim($(".tabLine tbody tr").eq(i).find(".BatchNO").val()); //批次号
                                var validDate = $.trim($(".tabLine tbody tr").eq(i).find(".validDate").val()); //有效期
                                var OutNum = $.trim($(".tabLine tbody tr").eq(i).find(".OutNum").val()); //入库数量
                                var AuditAmount = $.trim($(".tabLine tbody tr").eq(i).find(".AuditAmount").val()); //单价
                                var sumAmount = $.trim($(".tabLine tbody tr").eq(i).find(".sumAmount").val()); //金额
                                var Remark = $.trim($(".tabLine tbody tr").eq(i).find(".Remark").val()); //备注
                                //if (ValueInfo == "" || ValueInfo == undefined) { msg = "请输入规格型号"; return false; }
                                if (OutNum == "" || OutNum == undefined) { msg = "请输入出库数量"; return false; }
                                if (index == 1) {
                                    json += "{\"tip\":\"" + tip + "\",\"GoodsName\":\"" + GoodsName + "\",\"GoodsCode\":\"" + GoodsCode + "\",\"ValueInfo\":\"" + ValueInfo + "\",\"Unit\":\"" + Unit + "\",\"BatchNO\":\"" + BatchNO + "\",\"validDate\":\"" + validDate + "\",\"OutNum\":\"" + OutNum + "\",\"Remark\":\"" + Remark + "\",\"AuditAmount\":\"" + AuditAmount + "\",\"sumAmount\":\"" + sumAmount + "\"}";
                                } else {
                                    json += ",{\"tip\":\"" + tip + "\",\"GoodsName\":\"" + GoodsName + "\",\"GoodsCode\":\"" + GoodsCode + "\",\"ValueInfo\":\"" + ValueInfo + "\",\"Unit\":\"" + Unit + "\",\"BatchNO\":\"" + BatchNO + "\",\"validDate\":\"" + validDate + "\",\"OutNum\":\"" + OutNum + "\",\"Remark\":\"" + Remark + "\",\"AuditAmount\":\"" + AuditAmount + "\",\"sumAmount\":\"" + sumAmount + "\"}";
                                }
                            } else {
                                index--;
                            }
                        })
                        json += "]}]";
                        if (msg != "" && msg != undefined) { layerCommon.msg(msg, IconOption.哭脸); return false; }
                        if (index == 0) { layerCommon.msg("请最少选择一行商品", IconOption.哭脸) }
                        else {
                            var actiontype = "disPaymentEdit";
                            $.ajax({
                                type: "post",
                                url: "../../Handler/orderHandle.ashx",
                                data: { ActionType: actiontype, json: json },
                                dataType: "text",
                                success: function (data) {
                                    var json = JSON.parse(data);
                                    if (json.returns == "true") {
                                        layerCommon.msg("操作成功", IconOption.笑脸);
                                        window.location = "PaymentInfo.aspx?KeyID=" + json.PaymentID + "";
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
                function GoodsList(OrderOutDetailID, inindex) {
                    if (OrderOutDetailID != "") {
                        var xyindex = 0; //最大的索引
                        //冒泡排序获取最大的索引
                        //如果间隔行去选择商品 则对应不到对应的价格 所以采用了下面这个方法
                        $.ajax({
                            type: "post",
                            data: { ck: Math.random(), action: "goodsInfo", OrderOutDetailID: OrderOutDetailID, inindex: inindex },
                            dataType: "json",
                            success: function (data) {
                                var html = ""; //绑定的商品数据
                                var trIndex = getTrindex(); //获取选中过来的商品数量 便于相加索引
                                $(data).each(function (indexs, obj) {
                                    var validDatestr = new Date(obj.validDate);
                                    var year = validDatestr.getFullYear();  //获取年
                                    var month = validDatestr.getMonth() + 1;    //获取月
                                    var day = validDatestr.getDate(); //获取日
                                    var validDate = year + "-" + month + "-" + day
                                    
                                    var OutNum = 0;//数量
                                    var AuditAmount = "";//单价
                                    var sumAmount = "";//金额
                                    if (inindex == "-1") {
                                        OutNum = obj.OutNum
                                        AuditAmount = obj.AuditAmount
                                        sumAmount = obj.sumAmount
                                    } else {
                                        OutNum = obj.StockNum;
                                    }

                                    html += '<tr trindex="' + trIndex + '" tip="' + obj.ID + '"><td><div class="addg"><a href="javascript:;" class="minus2"></a><a href="javascript:;" class="add2" tip="alast"></a></div></td><td><div class="search"><a class="opt-i2"></a><input name="GoodsName"  type="text" class="box GoodsName" value="' + obj.GoodsName + '"  maxlength="15" readonly="readonly"/><input  type="hidden" class="GoodsCode" value="' + obj.GoodsCode + '"/></div></td><td ><div class="search"><input name="ValueInfo" type="text" value="' + obj.ValueInfo + '" class="box ValueInfo" maxlength="15" readonly="readonly"/></td><td><div class="search"><input name="Unit" value="' + obj.Unit + '" type="text" class="box Unit" maxlength="4" readonly="readonly"/></div></td><td ><div class="search"><input name="BatchNO" value="' + obj.BatchNO + '" type="text" class="box BatchNO" maxlength="15" readonly="readonly"/></div></td><td><div class="search"><input type="text" class="Wdate validDate" value="' + validDate + '" readonly="readonly" /></div></td><td><div class="search"><input name="OutNum" value="' + OutNum + '"  onblur="KeyInt(this,0)"  onkeyup="KeyInt(this,0)" type="text" class="box OutNum" maxlength="7"/></div></td><td><div class="search"><input type="text" id="AuditAmount' + trIndex + '" class="box AuditAmount" maxlength="7" value="' + AuditAmount + '"  onblur="MoneyYZ(this)"  onkeyup="MoneyYZ(this)"/> </div></td> <td ><div class="search"><input type="text" id="sumAmount' + trIndex + '" value="' + sumAmount + '"  class="box sumAmount" maxlength="7" readonly="readonly"/></div></td><td><div class="search"><input name="Remark" value="" type="text" class="box Remark" maxlength="20"/></div></td></tr>';
                                    trIndex++;
                                })
                                if (inindex == "-1")
                                    $(".tabLine table tbody ").prepend(html);
                                else
                                    $(".tabLine table tbody tr").eq(inindex).replaceWith(html); //替换当前选择时的行

                                getPaymentAmount();
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
                    return xyindex * 1 + 1;
                }

                //关闭弹窗
                function closeAll() {
                    layer.closeAll();
                }

                function MoneyYZ(val)//val=this
                {
                    var id = $(val).attr("id");
                    var el = $("#" + id + "").get(0);
                    var pos = 0;
                    if ('selectionStart' in el) {
                        pos = el.selectionStart;
                    } else if ('selection' in document) {
                        el.focus();
                        var Sel = document.selection.createRange();
                        var SelLength = document.selection.createRange().text.length;
                        Sel.moveStart('character', -el.value.length);
                        pos = Sel.text.length - SelLength;
                    }
                    var str = new RegExp("[1234567890.]")
                    var d = new RegExp("[.]")
                    var s = $("#" + id + "").val();
                    var rs = "";
                    for (var i = 0; i < s.length; i++) {
                        if (str.test(s.substr(i, 1))) {
                            if (d.test(s.substr(i, 1))) {
                                if (rs.indexOf('.') < 0 && rs.length > 0) {
                                    rs = rs + s.substr(i, 1);
                                }
                            }
                            else {
                                var index = rs.indexOf('.');
                                if (index > 0) {
                                    var strs = rs.substring(index, rs.length)
                                    if (strs.length < 3) {
                                        rs = rs + s.substr(i, 1);
                                    }
                                }
                                else {
                                    rs = rs + s.substr(i, 1)
                                }
                            }
                        }
                    }
                    if (s != rs) {
                        $("#" + id + "").val(rs);
                        if (val.setSelectionRange) {
                            val.focus();
                            val.setSelectionRange(pos - 1, pos - 1);
                        }
                        else if (input.createTextRange) {
                            var range = val.createTextRange();
                            range.collapse(true);
                            range.moveEnd('character', pos - 1);
                            range.moveStart('character', pos - 1);
                            range.select();
                        }
                    }

                    if (id != "PaymentAmount") {
                        //计算金额
                        var AuditAmount = $("#" + id + "").val() * 1;//单价
                        var OutNum = $("#" + id + "").parents("tr").find(".OutNum").val() * 1;//出库数量
                        var sumAmount = AuditAmount * OutNum
                        $("#" + id + "").parents("tr").find(".sumAmount").val(sumAmount);//金额
                        getPaymentAmount();
                    }
                    

                }
                function KeyInt(val, defaultValue) {

                    if (val.value == "0")
                        val.value = (defaultValue == undefined ? "" : defaultValue);
                    else
                        val.value = val.value.replace(/[^\d]/g, '');

                    if (defaultValue == "0")
                    { 
                    //计算金额
                    var OutNum = $(val).val() * 1;//出库数量
                    var AuditAmount = $(val).parents("tr").find(".AuditAmount").val() * 1;//单价
                    var sumAmount = AuditAmount * OutNum
                    $(val).parents("tr").find(".sumAmount").val(sumAmount);//金额
                    getPaymentAmount();
                }
                }

                //生单
                function btnCMerchants() {

                    var HtDropId = $("#HtDrop").val();
                    if (HtDropId === "" || HtDropId === null || typeof (HtDropId) === undefined) {
                        layerCommon.msg("请选择医院", IconOption.哭脸);
                        return false;
                    }
                    var index = layerCommon.openWindow("选择招商", "SelectLibraryList.aspx?index=-1&id=" + HtDropId, "1100px", "530px");  //记录弹出对象
                    $("#hid_Alert").val(index); //记录弹出对象
                }

                function getPaymentAmount() {
                    var PaymentAmount = 0;
                    $(".tabLine table tbody tr").each(function (x, indexy) {
                        if ($(".tabLine table tbody tr").eq(x).attr("tip") != undefined && $(".tabLine table tbody tr").eq(x).attr("tip") != 0) {
                            var PaymentAmountTR = $(".tabLine table tbody tr").eq(x).find(".sumAmount").val() * 1;
                            PaymentAmount += PaymentAmountTR;
                        }
                    })
                    $("#PaymentAmount").val(PaymentAmount);
                }

            </script>
    </form>
</body>
</html>
