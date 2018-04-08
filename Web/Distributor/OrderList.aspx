<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderList.aspx.cs" Inherits="Distributor_OrderList" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>订单列表</title>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Company/js/jquery-1.11.1.min.js"></script>
<%--    <link href="../css/layer.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/CommonJs.js"></script>
    <script src="../js/layer.js" type="text/javascript"></script>--%>
    <script type="text/javascript" src="../js/CommonJs.js"></script>
    <script src="../js/layer/layer.js" type="text/javascript"></script>
    <script src="../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("input[type='text']").blur();
                    $("#btnSearch").trigger("click");
                }
            })
        })
        $(document).ready(function () {
            $('#orderBg tr:even').addClass("bg");

            //订单生成 搜索
            $("#Search").on("click", function () {
                $("#btnSearch").trigger("click");
            });

            //设置cookie  
            function setCookie(name, value, seconds) {
                seconds = seconds || 0;   //seconds有值就直接赋值，没有为0，这个根php不一样。  
                var expires = "";
                if (seconds != 0) {      //设置cookie生存时间  
                    var date = new Date();
                    date.setTime(date.getTime() + (seconds * 1000));
                    expires = "; expires=" + date.toGMTString();
                }
                document.cookie = name + "=" + value + expires + "; path=/";   //转码并赋值  
            }

            //订单详情导出
            $("#liexcel").on("click", function () {
                setCookie("oID", "<%=oId %>", 1800);
                window.location.href = '../../../ExportExcel.aspx';
                return false;
            });

        });

        function showPayDetail(orderid) {
            var height = document.body.clientHeight; //计算高度
            var layerOffsetY = (height - 450) / 2; //计算宽度
            var index = layerCommon.openWindow('订单支付明细', 'Pay/PayDetail.aspx?KeyID=' + orderid, '750px', '380px'); //记录弹出对象
            $("#hid_Alert").val(index); //记录弹出对象
        }

        function CloseDialog() {
            var showedDialog = $("#hid_Alert").val(); //获取弹出对象
             layerCommon.layerClose(showedDialog); //关闭弹出对象
        }

        function reset() {
            $("#orderid").val("");
            $("#ddrOState").val(-2);
            $("#ddrPayState").val(-1);
            $("#ddrReturnState").val(-1);
        }

        function present(ReceiptNo) {
            $.ajax({
                url: "OrderList.aspx?type=present",
                data: { ReceiptNo: ReceiptNo },
                dataType: 'json',
                success: function (img) {
                    if (img.str) {
                        location.href = "OrderList.aspx";
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    layerCommon.msg("服务器异常，请稍后再试", IconOption.哭脸);
                }
            });
        }

        function affirm(ReceiptNo) {
            $.ajax({
                url: "OrderList.aspx?type=affirm",
                data: { ReceiptNo: ReceiptNo },
                dataType: 'json',
                success: function (img) {
                    if (img.str) {
                        location.href = "OrderList.aspx";
                    }
                    else {
                        layerCommon.msg("到货状态异常", IconOption.哭脸);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    layerCommon.msg("服务器异常，请稍后再试", IconOption.哭脸);
                }
            });
        }

        function Del(oid) {
            layerCommon.confirm("是否作废当前订单?", function () {
                $.ajax({
                    url: "OrderList.aspx?type=Del",
                    data: { oid: oid },
                    dataType: 'json',
                    success: function (img) {
                        if (img.str) {
                            location.href = "OrderList.aspx";
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        layerCommon.msg("服务器异常，请稍后再试", IconOption.哭脸);
                    }
                });
            });
        }

        function Goinfo(id) {
            window.location.href = 'neworder/orderdetail.aspx?KeyID=' + id + '&type1=OrderList';
        }

        function pay(Id) {
            //window.location.href = 'Pay.aspx?KeyID=' + Id;
            window.open('pay/Pay.aspx?isDBPay=<%=Common.PaySetingsValue(this.CompID) %>&KeyID=' + Id);
        }
       
    </script>
</head>
<body class="root3">
    <form id="form1" runat="server" method="post">
    <Head:Head ID="Head1" runat="server" />
    <div class="w1200">
    <Left:Left ID="Left1" runat="server" ShowID="OrderList" />
    <asp:Button ID="btnSearch" Text="搜索" runat="server" onclick="btnSearch_Click" style=" display:none"  />
    <div class="rightCon">
    <div class="info">
         <a id="navigation1" href="UserIndex.aspx">我的桌面</a>>
         <a id="navigation2" href="OrderList.aspx" class="cur">订单列表</a></div>
        <!--功能条件 start-->
        <div class="userFun">
            <div class="left">
                <a href="neworder/orderbuy.aspx" class="btnOr" id="btnAdd" runat="server"><i class="addIcon"></i>新建订单</a></div>
            <div class="right">
                <ul class="term">
                     <li><label class="head">选择厂商：</label>
                        <select id="ddrComp" name="" style=" width:120px;" runat="server" class="xl">
                        </select>
                    </li>
                    <li><label class="head">订单状态：</label>
                        <select id="ddrOState" name="" style=" width:90px;" runat="server" class="xl">
                            <option value="-2">全部订单</option>
                            <option value="0">待处理订单</option>
                            <option value="1">已完成订单</option>
                            <option value="2">已作废订单</option>
                        </select>
                    </li>
                    <li>
                        <label class="head">
                            订单编号：</label><input id="orderid" runat="server" type="text" class="box" style="width:110px;" /></li>
                    
                </ul>
                <a href="javascript:void(0)" class="btnBl" id="Search"><i class="searchIcon"></i>搜索</a>
                <a href="javascript:void(0)"  class="btnBl liSenior"><i class="resetIcon "></i>高级</a>
                <%--<a href="javascript:void(0)"  class="btnBl liRest"><i class="resetIcon "></i>重置</a>--%>
            </div>
        </div>
           <div class="hidden userFun" style=" text-align:right; <%--padding-right:160px;--%> padding-top:10px; display:none;">
             <div class="right">
                <ul class="term">
                      <li>
                        <label class="head">下单日期：</label>
                        <input name="txtArriveDate" runat="server" onclick="var endDate=$dp.$('txtArriveDate1'); WdatePicker({onpicked:function(){endDate.focus();},maxDate:'#F{$dp.$D(\'txtArriveDate1\')}'})" id="txtArriveDate"
                            readonly="readonly" type="text" class="Wdate box" style=" width:100px;" value="" />
                        <i class="txt">—</i>
                        <input name="txtArriveDate" runat="server" onclick="WdatePicker({minDate:'#F{$dp.$D(\'txtArriveDate\')}'})" id="txtArriveDate1"
                            readonly="readonly" type="text" class="Wdate box" style=" width:100px;" value="" />
                    </li>
                     <li>
                        <label class="head">
                            支付状态：</label><select id="ddrPayState" runat="server" name="" class="xl">
                                <option value="-1">全部</option>
                                <option value="0">未支付</option>
                                <option value="1">部分支付</option>
                                <option value="2">已支付</option>
                         
                            </select></li>   
                    <li>
                        <label class="head">总价区间：</label>
                        <input name="txtTotalAmount1" id="txtTotalAmount1" onkeyup="KeyInt2(this)" runat="server"
                            type="text" class="box" style=" width:60px;" maxlength="50"/>
                        <i class="txt">—</i>
                        <input name="txtTotalAmount2" id="txtTotalAmount2" onkeyup="KeyInt2(this)" runat="server"
                            type="text" class="box" style=" width:60px;" maxlength="50"/>
                    </li>
                    <li> <label class="head">每页</label><input type="text" onkeyup='KeyInt(this)' onblur="KeyInt(this);" id="txtPager" name="txtPager" runat="server"
                class="box3" /><label class="head">行</label></li>
                        <li> <a href="javascript:void(0)" class="btnBl" id="liexcel" ><i class="searchIcon"></i>导出订单</a></li>
                </ul>
                </div>
                    </div>
        <!--功能条件 end-->
        <div class="blank10">
        </div>
        <!--订单管理 start-->
        <div class="orderNr">
            <table class="PublicList list" id="orderBg" border="0" cellspacing="0" cellpadding="0">
                <asp:Repeater ID="rptOrder" runat="server">
                    <HeaderTemplate>
                        <thead>
                            <tr>
                                <th >
                                    订单编号
                                </th>
                                <th >
                                   下单时间
                                </th>
                                <th class="t2">
                                    订单金额
                                </th>
                                <th class="t2">
                                    已支付金额
                                </th>
                                <th class="t2">
                                    订单状态
                                </th>
                                <th class="t2">
                                    支付状态
                                </th>
                                  <th>订单来源</th>
                                 <th class="t2">操 作</th>
                            </tr>
                        </thead>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <a href='/Distributor/newOrder/orderdetail.aspx?top=1&KeyID=<%# Common.DesEncrypt(Eval("Id").ToString(), Common.EncryptKey) %>' /><%# Eval("ReceiptNo") %></a>
                        
                                <%--<a href='javascript:void(0);' onclick='Goinfo("<%# Common.DesEncrypt(Eval("ID").ToString(), Common.EncryptKey) %>")'><%#Eval("ReceiptNo")%></a>--%>
                            </td>
                            <td>
                                <%# Eval("CreateDate","{0:yyyy-MM-dd HH:mm}") %>
                            </td>
                            <td>
                                <%# Math.Round((decimal)Eval("AuditAmount"),2)%>&nbsp;
                            </td>
                            <td>
                                <%# Math.Round((decimal)Eval("PayedAmount"), 2)%>&nbsp;
                            </td>
                            <td>
                                <%# OrderType.GetOState(Eval("OState").ToString(), Eval("IsOutState").ToString())%>&nbsp;
                            </td>
                            <td style="width:100px;">
                                <%# OrderInfoType.PayState((int)Eval("paystate")) == "已支付" ? "<a title='查看支付清单' class='ger' href='javascript:void(0)' onclick='showPayDetail(\"" + Common.DesEncrypt(Eval("ID").ToString(), Common.EncryptKey) + "\")'>已支付</a>" : OrderInfoType.PayState((int)Eval("paystate")) == "部分支付" ? "<a title='查看支付清单' class='ger' href='javascript:void(0)' onclick='showPayDetail(\"" + Common.DesEncrypt(Eval("ID").ToString(), Common.EncryptKey) + "\")'>部分支付</a>" : OrderInfoType.PayState((int)Eval("paystate"))%>&nbsp;
                            </td>
                            <td><%# OrderInfoType.AddType(int.Parse(Eval("AddType").ToString()))%></td>
                            <td>
                             	<span class="linkBtn">
                                 <a class='a-gray' href='/Distributor/newOrder/orderdetail.aspx?top=1&KeyID=<%# Common.DesEncrypt(Eval("Id").ToString(), Common.EncryptKey) %>' />详情</a>
                                	 <%--<%# Getmessage(Eval("OState"), Convert.ToInt32(Eval("PayState")), Eval("ReturnState"), Convert.ToInt32(Eval("ID")))%>--%>
                              		<%--<a class='a-gray'  href='javascript:void(0);' onclick='Goinfo("<%# Common.DesEncrypt(Eval("ID").ToString(), Common.EncryptKey) %>")'>订单详情</a>--%>
                                </span>
                      <style>                     	
						
                      </style>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
        <!--订单管理 end-->

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
    </div>
    </div>
    
    </form>
</body>
</html>
