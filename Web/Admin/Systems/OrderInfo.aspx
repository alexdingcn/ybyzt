<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderInfo.aspx.cs" Inherits="Admin_Systems_OrderInfo" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <%--  日期控件js--%>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/shop.css" rel="stylesheet" type="text/css" />
    <link href="../../Distributor/newOrder/css/global2.5.css?v=201507261156" rel="stylesheet" type="text/css" />
    <%--附件上传  start--%>
    <script src="../../js/FileUpLoad.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#AddOrderfile").AjaxUploadFile({ Src: "TempFile/", ShowDiv: "payulfile", ResultId: "HDFileNames", AjaxSrc: "/Controller/Fileup.ashx", maxlength: 200, DownSrc: "../" });
                //日志
            $("#Log").on("click", function () {

                var height = document.body.clientHeight; //计算高度
                var layerOffsetY = (height - 450) / 2; //计算宽度
                var index = layerCommon.openWindow('订单日志', '../../BusinessLog.aspx?LogClass=Order&ApplicationId=' + <%=KeyID%> + '&CompId=' + <%=CompID%>, '750px', '450px'); //记录弹出对象
                $("#hid_Alert").val(index); //记录弹出对象
            });
            function CloseDialog() 
            {
                var showedDialog = $("#hid_Alert").val(); //获取弹出对象
                layerCommon.layerClose(showedDialog); //关闭弹出对象
            }
            //彻底删除弹框出现
            $("#orderDelete").click(function(){
                $(".po-bg").attr("class", "po-bg");
                $("#p-deletes").show();

            })
            $(".deletes_true").click(function(){
               
                var oID = $.trim($("#hidOrderID").val());
               
                $.ajax({
                    type: 'post',
                    url: '../../Handler/orderHandle.ashx',
                    data: { ck: Math.random(),ActionType: "OrderDelete",oID: oID },
                    dataType: 'json',
                    success: function (data) {
                        if(data.Result)
                        {
                            layerCommon.msg("订单彻底删除成功", IconOption.笑脸);
                            $("#p-deletes").hide();
                            window.location.href ="OrderList.aspx";
                        }
                        else
                        {
                            layerCommon.msg("订单彻底删除失败", IconOption.笑脸);
                        }
                            
                       
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
                    }
                });
                $(".po-bg").attr("class", "po-bg none");
                $("#p-delete").attr("class", "popup p-delete none");
                $(".btn-area").attr("class", "btn-area");
               

            })
        })
        //彻底删除取消事件
        function cancel()
        {
            $(".po-bg").attr("class", "po-bg none");
            $("#p-deletes").hide();
        }
        //支付流水，跳转到详情页面
        function PayItem(orderid, paymentid, PreType) {
            window.location.href = '../Report/CompCollection.aspx?RepDetailsList&PreType=' + PreType + '&orderid=' + orderid + '&Paymnetid=' + paymentid + '&KeyID=' + paymentid;
        }
    </script>
    <%--附件上传   end--%>
    <style type="text/css">
	body{ background:#fff;}
        .orderDiv {
       margin-left:15px;
        padding:0px;
        }
        .place{height:30px; color:#fff;z-index:999; background:#1d4a8d; border-left:1px solid #103a78; position:fixed;top:0; left:0; right:0;}
      .place a{ color:#fff;  cursor:default;}
      .place span{line-height:30px; font-weight:bold;float:left; margin-left:12px;}
      .placeul li{float:left; line-height:30px; padding-left:7px; padding-right:12px; background:url(../../Company/images/rlist.gif) no-repeat right;}
      .placeul li:last-child{background:none;}
	 
    .header{ padding:0; padding-left:150px; height:60px; background:#f5f5f5; border-bottom:1px solid #ddd; width:auto;}
        .cancel:hover {
              background: inherit;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <!--头部 start-->
    <!--头部 end-->
   
    <input type="hidden" runat="server" id="hidOrderID" value="" />
    <input type="hidden" runat="server" id="hid_Alert" value="" />
    <input type="hidden" runat="server" id="hidOstate" value="" />
    <input type="hidden" runat="server" id="hidIsOutstate" value="0" />
    <input type="hidden" runat="server" id="hidpaystate" value="" />
    <input type="hidden" runat="server" id="hidDigits" value="0" />
    <input type="hidden" runat="server" id="hidDisID" value="" />
    <input type="hidden" runat="server" id="hidUserType" value="" />
    <input type="hidden" runat="server" id="hidCompID" value="0" />
    <input type="hidden" runat="server" id="hidPicpath" value="" />
   <uc1:Header ID="Header1" runat="server" />
    <uc2:Left ID="Left1" runat="server" />
    <div class="m-con">
        <div class="m-place">
	        <i>位置：</i>
            <a href="../index.aspx" target="_top">我的桌面</a><i>></i>
             <a href="#">订单查询</a><i>></i>
             <a href="OrderList.aspx">订单详细</a>
        </div>
    <div class="rightCon orderDiv" >
         <input type="hidden" id="hid_Alerts" />
         <!--当前位置 start-->
    
    
        <!--当前位置 end--> 
        <!--[if !IE]>商品展示区 start<![endif]-->
        <div class="goods-zs" style="margin-top:25px;">
            <!--标题 start -->
            <ul class="goods-title">
                <li class="hover" tip="1"><a href="javascript:;">订单</a></li>
                <li tip="2"><a href="javascript:;">发货</a></li>
                <li tip="3"><a href="javascript:;">收款</a></li>
            </ul>
            <div class="blank10">
            </div>
            <!--标题 end -->
            <!--订单 start -->
            <div class="order">
                <!--订单编号 start -->
                <div class="goods-if">
                    <div class="title">
                        <i>订单编号：<label id="lblReceiptNo" runat="server"></label></i> 
                        <i>订单日期：<label id="lblCreateDate" runat="server"></label></i> 
                        <i>状态：<label id="lblOstate" runat="server">待订单审核</label></i>
                        <i>代理商：<label id="lblDisName" runat="server"></label></i>
                    </div>
                    <div class="alink">
                       <%-- <label id="orderaudit" runat="server">
                            <a href="javascript:;" class="bule">订单审核</a>|</label>
                        <label id="buyagain" runat="server">
                            <a href="javascript:;" class="bule">再次购买</a>|</label>
                        <label id="modifyorder" runat="server">
                            <a href="orderBuy.aspx?KeyID=<%=Common.DesEncrypt(KeyID.ToString(),Common.EncryptKey) %>"
                                class="bule">订单修改</a>|</label>--%>
                        <%--<label id="ordervoid" runat="server">
                            <a href="javascript:;" class="bule">订单作废</a>|</label>--%>
                         <label id="orderDelete" runat="server">
                            <a href="javascript:;" class="bule" >彻底删除</a>|</label>
                         <label id="Log" runat="server">
                            <a href="javascript:;" class="bule">日志</a>|</label>
                        <a id="orderprint" href="javascript:;" class="bule">打印</a>
                    </div>
     
                    <div class="cancel ordercancel" style="display:none; border:none;"></div>
                </div>
                <!--订单编号 start -->
                <!--流程 start -->
                <div class="blank10">
                </div>
                <ul class="goods-flow">
                    <li tip="1">
                        <i class="name">提交订单</i>
                        <i class="da"><%=CreateDate %></i>
                        <i class="size">1</i>
                        <i class="line"></i>
                    </li>
                    <li tip="2">
                        <i class="name">订单审核</i>
                        <i class="da"><%=AuditDate%></i>
                        <i class="size">2</i>
                        <i class="line"></i>
                    </li>
                    <li tip="4">
                        <i class="name">订单发货</i>
                        <i class="da sendde"><%=sendde %></i>
                        <i class="size">3</i>
                        <i class="line"></i>
                    </li>
                    <li tip="43">
                        <i class="name">收货确认</i>
                        <i class="da signde"><%=signde %></i>
                        <i class="size">4</i>
                        <i class="line"></i>
                    </li>
                    <li tip="5">
                        <i class="name">完成</i>
                        <i class="da fulfil"><%=fulfil %></i>
                        <i class="size">5</i>
                        <i class="line"></i>
                    </li>
                </ul>
                <div class="blank10">
                </div>
                <!--流程 end -->
                <!--商品 start -->
                <div class="tabLine">
                    <table border="0" cellspacing="0" cellpadding="0">
                        <thead>
                            <tr>
                                <%--<th class="t5">
                                </th>--%>
                                <th class="">
                                    商品名称
                                </th>
                                <th class="t2">
                                    规格属性
                                </th>
                                <th class="t5">
                                    单位
                                </th>
                                <th class="t3">
                                    单价
                                </th>
                                <th class="t4">
                                    数量
                                </th>
                                <th class="t3">
                                    小计
                                </th>
                                <th class="t3">
                                    备注
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater runat="server" ID="rptOrderD">
                                <ItemTemplate>
                                    <tr trd="<%# Eval("ID") %>">
                                        <%--<td class="t8">
                                            <div class="addg">
                                                <a href="javascript:;" class="minus2"></a><a href="javascript:;" class="add2"></a>
                                            </div>
                                        </td>--%>
                                        <td>
                                            <div class="sPic">
                                                <span><a target="_blank" href="../../e<%# Eval("GoodsID") %>_<%# CompID %>.html">
                                                    <img src="<%# SelectGoodsInfo.GetGoodsPic(Convert.ToString(Eval("Pic"))) %>" width="60"
                                                        height="60"></a></span> <a target="_blank" href="../../e<%# Eval("GoodsID") %>_<%# CompID %>.html" class="code">商品编码：<%# Eval("GoodsCode")%>
                                                         <%# SelectGoodsInfo.protitle(Convert.ToString(Eval("ProID")), Convert.ToString(Eval("Protype")),Convert.ToString(Eval("Unit")),CompID) %>
                                                        </a><a target="_blank" href="../../e<%# Eval("GoodsID") %>_<%# CompID %>.html" class="name">
                                                            <%# Common.MySubstring(Convert.ToString(Eval("GoodsName")),20,"...")%><i><%# Eval("GoodsName")%></i></a>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="tc">
                                                <%# SelectGoodsInfo.GetGoodsInfos(Eval("GoodsInfos").ToString()) %>&nbsp;</div>
                                        </td>
                                        <td>
                                            <div class="tc">
                                                <%# Eval("Unit")%>&nbsp;</div>
                                        </td>
                                        <td>
                                            <div class="tc">
                                                ￥<%# Eval("AuditAmount", "{0:N2}")%>&nbsp;</div>
                                        </td>
                                        <td>
                                            <div class="tc">
                                                <%# SelectGoodsInfo.GetNum(Eval("GoodsNum").ToString(), Digits)%>&nbsp;
                                            </div>
                                        </td>
                                        <td>
                                            <div class="tc">
                                                ￥<%# Eval("sumAmount","{0:N2}")%>&nbsp;</div>
                                        </td>
                                        <%--<td>
                                            <div class="tc alink">
                                                <%# goodsRemark(Eval("ID").ToString(), Eval("Remark").ToString()) %>&nbsp;
                                            </div>
                                        </td>--%>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
                <!--商品 end -->
                <!--[if !IE]>订单金额 start<![endif]-->
                <div class="options-box price-box">
                    <div class="right">
                        <table>
                            <tr>
                                <td class="li">
                                    商品总额：
                                </td>
                                <td>
                                    ¥<label id="lblTotalAmount" runat="server">0.00</label>
                                </td>
                            </tr>
                            <tr>
                                <td class="li">
                                    促销优惠：
                                </td>
                                <td>
                                    ¥<label id="lblProAmount" runat="server">0.00</label>
                                </td>
                            </tr>
                            <tr runat="server" id="trbate">
                                <td class="li">
                                    返利抵扣：
                                </td>
                                <td>
                                    ¥<label id="lblbateAmount" runat="server">0.00</label>
                                </td>
                            </tr>
                            <tr>
                                <td class="li">
                                    运费：
                                </td>
                                <td>
                                    <a href="javascript:;" style="display: none;" class="edit-i postfee"></a>¥<label
                                        id="lblPostFee" runat="server">0.00</label>
                                </td>
                            </tr>
                            <tr>
                                <td class="li">
                                    <i class="rz-icon contract"></i>应付总额：
                                </td>
                                <td>
                                    <div class="price-sum li">
                                        <i class="price"><a href="javascript:;" style="display: none;" class="edit-i payamount">
                                        </a>￥<label id="lblAuditAmount" runat="server">0.00</label>
                                        </i>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="rebate" runat="server" id="rebate">
                        <i class="bt2 left">使用返利：</i>
                        <div class="edit-ok left none">
                            <input name="" type="text" class="box" value="500" />
                            <i class="txt">可用返利￥5000<i class="sus-i"></i></i>
                        </div>
                        <i class="sum ok ">￥<label id="lblbate" runat="server">500</label><i class="sus-i seebate"></i></i>
                    </div>
                </div>
                <!--[if !IE]>订单金额 end<![endif]-->
            </div>
            <!--订单 end -->
            <!--发货 start -->
            <div class="goodstakn" style="display: none;">
                <div class="deli-title">
                    <i class="wait-i"></i>待发货清单</div>
                <!--商品 start -->
                <div class="tabLine">
                    <table border="0" cellspacing="0" cellpadding="0" class="noOut">
                        <thead>
                            <tr>
                                <th class="">
                                    商品名称
                                </th>
                                <th class="t1">
                                    规格属性
                                </th>
                                <th class="t5">
                                    单位
                                </th>
                                <th class="t3">
                                    订购数量
                                </th>
                                <th class="t3">
                                    已发货数量
                                </th>
                                <th class="t3">
                                    本次发货
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater runat="server" ID="rptIsout">
                                <ItemTemplate>
                                    <tr tld="<%# Eval("ID") %>">
                                        <td>
                                            <div class="sPic">
                                                <span><a target="_blank" href="../../e<%# Eval("GoodsID") %>_<%# CompID %>.html">
                                                    <img src="<%# SelectGoodsInfo.GetGoodsPic(Convert.ToString(Eval("Pic"))) %>" width="60"
                                                        height="60"></a></span> <a target="_blank" href="../../e<%# Eval("GoodsID") %>_<%# CompID %>.html" class="code">商品编码：<%# Eval("GoodsCode")%>
                                                        <%# SelectGoodsInfo.protitle(Convert.ToString(Eval("ProID")), Convert.ToString(Eval("Protype")),Convert.ToString(Eval("Unit")),CompID) %>
                                                        </a><a target="_blank" href="../../e<%# Eval("GoodsID") %>_<%# CompID %>.html" class="name">
                                                            <%# Common.MySubstring(Convert.ToString(Eval("GoodsName")),20,"...")%><i><%# Eval("GoodsName")%></i></a>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="tc">
                                                <%# SelectGoodsInfo.GetGoodsInfos(Eval("GoodsInfos").ToString()) %>&nbsp;</div>
                                        </td>
                                        <td>
                                            <div class="tc">
                                                <%# Eval("Unit")%>&nbsp;</div>
                                        </td>
                                        <td>
                                            <div class="tc">
                                                <%# SelectGoodsInfo.GetNum(Eval("GoodsNum").ToString(), Digits) %>&nbsp;</div>
                                        </td>
                                        <td>
                                            <div class="tc">
                                                <%# SelectGoodsInfo.GetNum(Eval("OutNum").ToString(), Digits)%>&nbsp;</div>
                                        </td>
                                        <td>
                                            <div class="sl">
                                                <input type="hidden" id="Notshipnum" class="Notshipnum" value="<%# Convert.ToDecimal(Eval("GoodsNum")) - Convert.ToDecimal(Eval("OutNum")) %>" />
                                                <a href="javascript:;" class="minus nominus">-</a>
                                                <input type="text" class="box txtGoodsNum" onchange="outOrderNum(this,0);"
                                                    onkeyup='KeyInt2(this)' value="<%# SelectGoodsInfo.GetNum((Convert.ToDecimal(Eval("GoodsNum")) - Convert.ToDecimal(Eval("OutNum"))).ToString(), Digits) %>" />
                                                <a href="javascript:;" class="add noadd">+</a>
                                            </div>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
                <!--商品 end -->
                <div class="goods-info deliver" runat="server" id="deliver">
                    <div class="bz remark">
                        <i class="bt">发货日期：</i>
                        <input type="text" class="Wdate" id="txtDate" runat="server" readonly="readonly"
                            onclick="WdatePicker({ minDate: '%y-%M-%d' })" />
                    </div>
                    <div class="bz invoice">
                        <i class="bt">物流信息：</i>
                        <div class="txtbox">
                            <a href="javascript:;" class="edit-i addlogistics"></a><i class="in-if">无物流信息</i>
                            <input type="hidden" id="hidLogistics" />
                            <input type="hidden" id="hidLogisticsNo" />
                            <input type="hidden" id="hidCarUser" />
                            <input type="hidden" id="hidCarNo" />
                            <input type="hidden" id="hidCar" />
                        </div>
                    </div>
                    <div class="btn">
                        <%--<a href="javascript:;" class="bule-btn outOg">发货</a>--%></div>
                </div>
                <div class="blank20">
                </div>
                <div class="deli-title">
                    <i class="send-i"></i>发货记录</div>
                <div id="outGoods" runat="server">
                </div>
            </div>
            <!--发货 end -->
            <!--收款 start -->
            <div class="payment" style="display: none">
                <input type="hidden" id="desKeyID" runat="server" />
                <!--金额 start -->
                <div class="goods-sum">
                    <div class="t">
                        订单金额：<b class="red">￥<%=TotalAmount %></b></div>
                    <div class="t">
                        已付款金额：<b class="red">￥<%=PayedAmount %></b></div>
                    <div class="t">
                        未付款金额：<b class="red">￥<%=paymoney%></b></div>
                    <div class="btn" runat="server" id="btn_pay">
                        <%-- <a href="#" onclick="payDB('<%=Common.DesEncrypt("0",Common.EncryptKey) %>','<%=Common.DesEncrypt(KeyID.ToString(),Common.EncryptKey) %>')"
                            class="bule-btn">在线支付</a>--%><%--<a href="#" id="btn_pay_xx" class="bule-btn">线下收款</a>--%></div>
                </div>
                <!--金额 end -->
                <!--线下支付 start -->
                <div class="offLine " style="display: none;">
                    <div class="title">
                        线下支付<a href="#" id="a_pay" class="bule">收起</a></div>
                    <%--记录未付款金额--%>
                    <input type="hidden" id="hidden_pay" value="<%=paymoney%>" />
                    <div class="li liw">
                        <div class="bt">
                            付款金额：</div>
                        <input name="paymoney" onkeyup="KeyIntPrice(this)" onblur="KeyIntPrice(this)" id="paymoney"
                            type="text" value="<%=paymoney%>" class="box price" />元</div>
                    <div class="li">
                        <div class="bt">
                            账户名称：</div>
                        <input name="bankname" id="bankname" runat="server" type="text" class="box" /></div>
                    <div class="li">
                        <div class="bt">
                            收款银行：</div>
                        <input name="bank" id="bank" type="text" runat="server" class="box" /></div>
                    <div class="li">
                        <div class="bt">
                            收款卡号：</div>
                        <input name="bankcode" id="bankcode" runat="server" type="text" class="box" /></div>
                    <div class="li">
                        <div class="bt">
                            付款日期：</div>
                        <input name="txtArriveDate" onclick="WdatePicker({minDate:'%y-%M-%d'})" runat="server"
                            id="txtArriveDate" readonly="readonly" type="text" class="box" />
                    </div>
                    <div class="li liw">
                        <div class="bt">
                            备 注：</div>
                        <textarea name="remark" id="remark" cols="" rows="" class="t-box"></textarea></div>
                    <%-- 附件凭证上传  start--%>
                    <div class="li liw">
                        <div class="bt2">
                            上传附件：</div>
                        <ul class="list" id="payulfile" runat="server">
                        </ul>
                        <div class="addDoc">
                            <a href="javascript:;" class="a-upload bule">
                                <input type="file" name="AddOrderfile" id="AddOrderfile" class="AddBanner" />+新增附件
                            </a>
                            <input type="hidden" id="HDFileNames" runat="server" />
                            <i class="txt">（附件最大20M，支持格式：PDF、Word、Excel、Txt、JPG、PNG、BMP、GIF、RAR、ZIP）</i>
                        </div>
                    </div>
                    <%-- 附件凭证上传  start--%>
                    <div class="payBtn">
                        <a href="#" id="pay_sub" class="bule-btn">确认支付</a></div>
                    <div class="blank10">
                    </div>
                </div>
                <!--线下支付 end -->
                <!--线下支付 end -->
                <div class="blank10">
                </div>
                <!--支付记录 start -->
                <div class="tabLine">
                    <table border="0" cellspacing="0" cellpadding="0" class="payTable">
                        <asp:Repeater ID="rptmessage" runat="server">
                            <HeaderTemplate>
                                <thead>
                                    <tr>
                                        <th class="">
                                            支付流水号
                                        </th>
                                        <th class="t3">
                                            支付时间
                                        </th>
                                        <th class="t3">
                                            支付金额
                                        </th>
                                        <th class="t3">
                                            手续费
                                        </th>
                                        <th class="t3">
                                            支付方式
                                        </th>
                                        <th class="t5">
                                            状态
                                        </th>
                                        <th class="t3">
                                            收款账户
                                        </th>
                                        <th class="">
                                            收款帐号
                                        </th>
                                        <th class="">
                                            操作
                                        </th>
                                    </tr>
                                </thead>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tbody>
                                    <tr>
                                        <td>
                                            <div class="tc">
                                                <a href="javascript:void(0)" onclick='Go_compPayInfo("<%# Common.DesEncrypt(Eval("paymentID").ToString(), Common.EncryptKey) %>","<%# Common.DesEncrypt(Eval("pretype").ToString(), Common.EncryptKey) %>")'>
                                                    <%# Eval("guids")%></a>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="tc">
                                                <%# Eval("Paytime").ToString()==""?"":Convert.ToDateTime(Eval("Paytime")).ToString("yyyy-MM-dd")%>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="tc">
                                                <%#Convert.ToDecimal(Eval("PayPrice")).ToString("0.00")%></div>
                                        </td>
                                        <td>
                                            <div class="tc">
                                                <%#Convert.ToString(Eval("sxf")) == "" ? "0.00" : Convert.ToString(Eval("sxf"))%></div>
                                        </td>
                                        <td>
                                            <div class="tc">
                                                <%# Eval("PayType")%></div>
                                        </td>
                                        <td>
                                            <div class="tc">
                                                <%# Convert.ToString(Eval("status"))=="1"?"成功":"作废"%></div>
                                        </td>
                                        <td>
                                            <div class="tc">
                                                <%# Eval("payName")%><i class="<%#  Eval("payName")==""?"":"car-i"%>"></i></div>
                                        </td>
                                        <td>
                                            <div class="tc">
                                                <%# Eval("paycode")%></div>
                                        </td>
                                        <td>
                                            <div class="tc alink">
                                                <a href="#" onclick='Go_compPayInfo("<%# Common.DesEncrypt(Eval("paymentID").ToString(), Common.EncryptKey) %>","<%# Common.DesEncrypt(Eval("pretype").ToString(), Common.EncryptKey) %>")'>
                                                    详情</a>
                                               <%-- <%# Pretype(Convert.ToString(Eval("PayType")), Eval("paymentID").ToString())%>--%>
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
                <!--支付记录 end -->
            </div>
            <!--收款 end -->
        </div>
        <!--[if !IE]>商品展示区 end<![endif]-->
        <div class="blank20">
        </div>
        <!--[if !IE]>下单信息 start<![endif]-->
        <div class="goods-info order">
            <div class="bh">
                <div class="left deli">
                    <i class="bt2">交货日期：</i>
                    <%--<div class="date left none"><a href="javascript:;">2015-05-07<i class="rl-icon"></i></a></div>--%>
                    <i class="ok "><%--<a href="javascript:;" class="edit-i po_deli" tip="0"></a>--%>
                        <label id="lblArriveDate" runat="server">
                        </label>
                    </i>
                </div>
                <div class="left carry">
                    <i class="bt2">配送方式：</i>
                    <%--<div class="ca-box left none"><i class="dx">自提<i class="arrow"></i></i><div class="menu"><a href="">送货</a></div></div>--%>
                    <i class="ok "><%--<a href="javascript:;" class="edit-i po_deli" tip="1"></a>--%>
                        <label id="lblGiveMode" runat="server">
                        </label>
                    </i>
                </div>
                <div class="clear">
                </div>
            </div>
            <div class="bz remark">
                <i class="bt">订单备注：</i>
                <div class="txtbox" style="height: auto;" id="divRemark">
                   <%-- <a href="javascript:;" class="edit-i orderRemark"></a>--%><i class="ok" id="iRemark"
                        runat="server"></i>&nbsp;
                </div>
            </div>
            <div class="bz site">
                <i class="bt">收货信息：</i>
                <div class="txtbox">
                   <%-- <a href="javascript:;" class="edit-i addr">--%></a><i class="ok">
                        <input id="hidAddrID" runat="server" type="hidden" />
                        收货人：<label id="lblPrincipal" runat="server"></label>
                        ，联系电话：<label id="lblPhone" runat="server"></label>
                        ，收货地址：<label id="lblAddress" runat="server"></label>
                    </i>
                </div>
            </div>
            <div class="bz invoice">
                <i class="bt">开票信息：</i>
                <div class="txtbox">
                    <input type="hidden" id="hidval" runat="server" value="0" />
                    <input type="hidden" id="hidDisAccID" runat="server" />
                    <%--<a href="javascript:;" class="edit-i Invoi"></a>--%><i class="ok iInvoice" id="iInvoice"
                        runat="server">
                        <%--发票抬头：<label id="lblRise" runat="server"></label>
                        ，发票内容：<label id="lblContent" runat="server"></label>
                        ，开户银行：<label id="lblOBank" runat="server"></label>
                        ，开户账户：<label id="lblOAccount" runat="server"></label>
                        ，纳税人登记号：<label id="lblTRNumber" runat="server"></label>--%>
                    </i>
                </div>
            </div>
            <div class="bz invoice">
                <i class="bt">发票信息：</i>
                <div class="txtbox">
                <%--   <a href="javascript:;" class="edit-i addbill"></a>--%><i class="ok">发票号：<label id="lblBillNo"
                        runat="server"></label>
                        ，是否已开完：<label id="lblIsBill" runat="server"></label>
                    </i>
                    <input id="hidisBill" runat="server" value="0"  type="hidden"/>
                </div>
            </div>
            <div class="bz attach">
                <i class="bt">附件：</i>
                <ul class="list" id="ulAtta" runat="server">
                    <%--<li><a href="javascript:;" class="name">新建 Microsoft Word 文档.docx（大小：255.95KB）</a> <a
                        href="javascript:;" class="bule del" orderid="">删除</a> <a href="javascript:;"
                            class="bule">下载</a> </li>--%>
                </ul>
                <div class="add">
                    <a href="javascript:;" class="a-upload bule">
                        <input type="file" name="AddBanner" id="AddBanner" class="AddBanner" onchange="uploadAvatar(this,'<%= Common.GetWebConfigKey("ImgViewPath") %>','<%=KeyID %>');" />+新增附件
                    </a><i class="txt">（附件最大20M，支持格式：PDF、Word、Excel、Txt、JPG、PNG、BMP、GIF、RAR、ZIP）</i>
                    <asp:HiddenField ID="hrOrderFj" runat="server" />
                </div>
            </div>
        </div>
        <!--[if !IE]>下单信息 end<![endif]-->
        <div class="blank20">
        </div>
        <div class="btn-box none">
            <div class="btn">
                <a href="javascript:;" class="btn-area">保存</a> <a href="javascript:;" class="gray-btn">
                    返回</a>
            </div>
            <div class="bg">
            </div>
        </div>
    </div>

    <div class="po-bg none"></div>
    <!--订单作废提示 start-->
   <div id="p-delete" class="popup p-delete none">
	    <div class="po-title">作废<a href="javascript:;" class="close canOrder"></a></div>
        <div class="nr"><i class="delete-i"></i>作废后，不可恢复，是否确认作废？</div>
	    <div class="po-btn">
            <a href="javascript:;" class="gray-btn canOrder">取消</a>
            <a href="javascript:;" class="btn-area canOrderSave">确定</a>
        </div>
    </div>
   
    <!--订单作废提示 end-->
      <%--订单彻底删除提示 start--%>
          <div id="p-deletes" class="popup p-delete none">
	    <div class="po-title">彻底删除<a href="javascript:;" class="close " onclick="cancel()"></a></div>
        <div class="nr"><i class="delete-i"></i>彻底删除后，不可恢复，是否确认删除？</div>
	    <div class="po-btn">
            <a href="javascript:;" class="gray-btn" onclick="cancel()">取消</a>
            <a href="javascript:;" class="btn-area deletes_true">确定</a>
        </div>
    </div>
       <%-- 订单彻底删除提示 end--%>
    <!--订单审核提示 start-->
    <%--<div id="divAdiut" class="popup p-delete none">
	    <div class="po-title">审核<a href="javascript:;" class="close canNoAudit"></a></div>
        <div class="nr"><i class="delete-i"></i>审核后，不可恢复，是否确认审核？</div>
	    <div class="po-btn">
            <a href="javascript:;" class="gray-btn canNoAudit">取消</a>
            <a href="javascript:;" class="btn-area canAudit">确定</a>
        </div>
    </div>--%>
    <!--订单审核提示 end-->

    <script src="/js/layer/layer.js" type="text/javascript"></script>
    <script src="/js/layerCommon.js" type="text/javascript"></script>
    <script src="/Distributor/newOrder/js/order.js?v=201608041653" type="text/javascript"></script>
    <script src="/js/ajaxfileupload.js" type="text/javascript"></script>
    <script src="/Company/js/js.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
        </div>
    </form>
</body>
</html>
