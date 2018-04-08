<%@ Page Language="C#" AutoEventWireup="true" CodeFile="pay.aspx.cs" Inherits="Admin_pay" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <script src="../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#type a").click(function () {
                var id = this.id;
                $("#div1810").hide();
                $("#div1372").hide();
                $("#div1320").hide();
                $("#div1350").hide();
                $("#div" + id.substr(2, 4)).show();
                $("#err" + id.substr(2, 4)).empty();
            });
            $("#a1810").click(function () {
                $("#err1810").empty();
                $.ajax({
                    type: "post",
                    url: "../Handler/Tx1810.ashx",
                    data: { date: $("#date").val() },
                    dataType: "json",
                    success: function (data) {
                        if (data.error == 1) {
                            $("#body1810").empty();
                            $("#err1810").html(data.msg);
                            return false;
                        }
                        var html = "";
                        $.each(data, function (commentIndex, comment) {
                            html += "<tr>";
                            html += "<td>" + comment["TxType"] + "</td>";
                            html += "<td>" + comment["TxSn"] + "</td>";
                            html += "<td>" + comment["TxAmount"] + "</td>";
                            html += "<td>" + comment["InstitutionAmount"] + "</td>";
                            html += "<td>" + comment["PaymentAmount"] + "</td>";
                            html += "<td>" + comment["PayerFee"] + "</td>";
                            html += "<td>" + comment["InstitutionFee"] + "</td>";
                            html += "<td>" + comment["Remark"] + "</td>";
                            html += "<td>" + comment["BankNotificationTime"] + "</td>";
                            html += "</tr>";
                        });
                        if (data.length == 0) {
                            html += "<tr>";
                            html = "<td colspan='9'>无</td>";
                            html += "</tr>";
                        }
                        $("#body1810").html(html);
                    }
                });
            });
            $("#a1810").click();
            $("#a1372").click(function () {
                $("#err1372").empty();
                $.ajax({
                    type: "post",
                    url: "../Handler/Tx1372.ashx",
                    data: { number: $("#number1372").val() },
                    dataType: "json",
                    success: function (data) {
                        if (data.error == 1) {
                            $("#body1372").empty();
                            $("#err1372").html(data.msg);
                            return false;
                        }
                        var html = "";
                        $.each(data, function (commentIndex, comment) {
                            html += "<tr>";
                            html += "<td>" + comment["OrderNo"] + "</td>";
                            html += "<td>" + comment["PaymentNo"] + "</td>";
                            html += "<td>" + comment["Status"] + "</td>";
                            html += "<td>" + comment["BankTxTime"] + "</td>";
                            html += "</tr>";
                        });
                        if (data.length == 0) {
                            html += "<tr>";
                            html = "<td colspan='4'>无</td>";
                            html += "</tr>";
                        }
                        $("#body1372").html(html);
                    }
                });
            });
            $("#a1320").click(function () {
                $("#err1320").empty();
                $.ajax({
                    type: "post",
                    url: "../Handler/Tx1320.ashx",
                    data: { number: $("#number1320").val() },
                    dataType: "json",
                    success: function (data) {
                        if (data.error == 1) {
                            $("#body1320").empty();
                            $("#err1320").html(data.msg);
                            return false;
                        }
                        var html = "";
                        $.each(data, function (commentIndex, comment) {
                            html += "<tr>";
                            html += "<td>" + comment["PaymentNo"] + "</td>";
                            html += "<td>" + comment["Amount"] + "</td>";
                            html += "<td>" + comment["Remark"] + "</td>";
                            html += "<td>" + comment["Status"] + "</td>";
                            html += "<td>" + comment["BankNotificationTime"] + "</td>";
                            html += "</tr>";
                        });
                        if (data.length == 0) {
                            html += "<tr>";
                            html = "<td colspan='5'>无</td>";
                            html += "</tr>";
                        }
                        $("#body1320").html(html);
                    }
                });
            });
            $("#a1350").click(function () {
                $("#err1350").empty();
                $.ajax({
                    type: "post",
                    url: "../Handler/Tx1350.ashx",
                    data: { number: $("#number1350").val() },
                    dataType: "json",
                    success: function (data) {
                        if (data.error == 1) {
                            $("#body1350").empty();
                            $("#err1350").html(data.msg);
                            return false;
                        }
                        var html = "";
                        $.each(data, function (commentIndex, comment) {
                            html += "<tr>";
                            html += "<td>" + comment["SerialNumber"] + "</td>";
                            html += "<td>" + comment["OrderNo"] + "</td>";
                            html += "<td>" + comment["Amount"] + "</td>";
                            html += "<td>" + comment["Remark"] + "</td>";
                            html += "<td>" + comment["AccountType"] + "</td>";
                            html += "<td>" + comment["PaymentAccountName"] + "</td>";
                            html += "<td>" + comment["PaymentAccountNumber"] + "</td>";
                            html += "<td>" + comment["BankID"] + "</td>";
                            html += "<td>" + comment["AccountName"] + "</td>";
                            html += "<td>" + comment["AccountNumber"] + "</td>";
                            html += "<td>" + comment["BranchName"] + "</td>";
                            html += "<td>" + comment["Province"] + "</td>";
                            html += "<td>" + comment["City"] + "</td>";
                            html += "<td>" + comment["Status"] + "</td>";
                            html += "</tr>";
                        });
                        if (data.length == 0) {
                            html += "<tr>";
                            html = "<td colspan='14'>无</td>";
                            html += "</tr>";
                        }
                        $("#body1350").html(html);
                    }
                });
            });
        });
    </script>
    <style type="text/css">
        *
        {
            font-size:15px;
        }
        table
        {
            font-family:"微软雅黑";
            border-collapse:collapse;
            text-align:center;
            margin-top:16px;
            width:100%;
        }
        table, td, th
        {
            border:1px solid gray;
        }
        .err
        {
            color:Red;
        }
        a
        {
            text-decoration:none;
            margin-left:10px;
        }
        .orangeBtn a:hover
        {
            color:#fff;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="type" style="line-height:28px;">
        <ul class="toolbar">
            <li><a href="javascript:void(0)" id="tx1810">交易对账单</a></li>
            <li><a href="javascript:void(0)" id="tx1372">市场订单快捷支付查询</a></li>
            <li><a href="javascript:void(0)" id="tx1320">市场订单支付交易查询</a></li>
            <li><a href="javascript:void(0)" id="tx1350">市场订单结算交易查询</a></li>
        </ul>
        <label style="color:Red;">单位（分）</label>
    </div>
    <br />
    <div id="div1810">
        <div style="height:40px; line-height:40px;">
            <label>查询日期：</label><input type="text" id="date" class="Wdate" readonly="readonly" onclick="WdatePicker()" value="<%= DateTime.Now.ToString("yyyy-MM-dd") %>" />(日期格式：yyyy-MM-dd)<a href="javascript:void(0)" id="a1810" class="orangeBtn" style=" padding:5px 20px;">查询</a>
        </div>
        <table border="0" cellspacing="0" cellpadding="0" class="tablelist">
            <thead>
                <tr>
                    <th>交易类型</th>
                    <th>交易编号</th>
                    <th>交易金额</th>
                    <th>机构应收金额</th>
                    <th>支付平台应收金额</th>
                    <th>付款人手续费</th>
                    <th>机构手续费</th>
                    <th>备注</th>
                    <th>支付平台收到银行通知时间</th>
                </tr>
            </thead>
            <tbody id="body1810">
                
            </tbody>
        </table>
        <label id="err1810" class="err"></label><br />
    </div>
    <div id="div1372" style="display:none;">
        <label>支付交易流水号</label><input type="text" id="number1372" class="textBox" style=" width:300px;" /><a href="javascript:void(0)" id="a1372" class="orangeBtn" style=" padding:5px 20px;">查询</a>
        <table border="0" cellspacing="0" cellpadding="0" class="tablelist">
            <thead>
                <tr>
                    <th>市场订单号</th>
                    <th>支付交易流水号</th>
                    <th>交易状态</th>
                    <th>银行处理时间</th>
                </tr>
            </thead>
            <tbody id="body1372">
                
            </tbody>
        </table>
        <label id="err1372" class="err"></label>
    </div>
    <div id="div1320" style="display:none;">
        <label>支付交易流水号</label><input type="text" id="number1320" class="textBox" style=" width:300px;" /><a href="javascript:void(0)" id="a1320" class="orangeBtn" style=" padding:5px 20px;">查询</a>
        <table border="0" cellspacing="0" cellpadding="0" class="tablelist">
            <thead>
                <tr>
                    <th>支付交易码</th>
                    <th>支付金额</th>
                    <th>备注</th>
                    <th>状态</th>
                    <th>支付平台收到银行通知时间</th>
                </tr>
            </thead>
            <tbody id="body1320">
                
            </tbody>
        </table>
        <label id="err1320" class="err"></label><br />
    </div>
    <div id="div1350" style="display:none;">
        <label>原结算交易流水号：</label><input type="text" id="number1350" class="textBox" style=" width:300px;" /><a href="javascript:void(0)" id="a1350" class="orangeBtn" style=" padding:5px 20px;">查询</a>
        <table border="0" cellspacing="0" cellpadding="0" class="tablelist">
            <thead>
                <tr>
                    <th>原结算交易流水号</th>
                    <th>结算订单号</th>
                    <th>结算金额</th>
                    <th>备注</th>
                    <th>账户类型</th>
                    <th>账户名称（支付平台账户）</th>
                    <th>账户号码（支付平台账户）</th>
                    <th>银行编号</th>
                    <th>账户名称</th>
                    <th>账户号码</th>
                    <th>分支行名称</th>
                    <th>开户账户省份</th>
                    <th>开户账户城市</th>
                    <th>结算状态</th>
                </tr>
            </thead>
            <tbody id="body1350">
                
            </tbody>
        </table>
        <label id="err1350" class="err"></label><br />
    </div>
    <div id="json"></div>
    </form>
</body>
</html>
