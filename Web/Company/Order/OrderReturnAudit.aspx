<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderReturnAudit.aspx.cs" Inherits="Company_Order_OrderReturnAudit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>订单审核</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/odrerstyle.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>

    <script src="../js/order.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $_def.ID = "btnSave";

            $(".cancel").click(function () {
                window.parent.CloseDialog();
            });

            $("#Save").click(function () {
                $("#btnSave").trigger("click");
            });

           $("#Return").click(function () {
                 $("#btnReturn").trigger("click"); 
            });

            //$(".sure").click(function () { 
                //window.parent.Audit();
            //});
        })

    </script>

    <style type="text/css">
        body 
        {
            font-family: "微软雅黑";
            margin: 0 auto;
            min-width: 650px;
        }
        .tipinfo
        {
            width:650px;
            height:auto;
        }
        
        input[type='text']
        {
            width:170px;
        }
        .tipbtn1 {
            margin-bottom: 10px;
            margin-left: 130px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="tipinfo">
            <div class="rightinfo" style=" margin-top:0px; margin-left:0px;width:auto;">
                <div class="div_content">
                    <div class="lbtb">
                        <table class="dh">
                            <tr>
                                <td style=" width:30%;"><span>审核人</span></td>
                                <td style=" width:70%;">
                                    <label id="lblAuditUser" runat="server"></label>
                                    <input type="hidden" id="hidAuditUserID" runat="server" />&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td><span>审核日期</span></td>
                                <td>
                                    <%--<input name="txtArriveDate" runat="server" onclick="WdatePicker()" id="txtArriveDate" readonly="readonly" type="text" class="Wdate" value="" />--%>
                                    <label id='lblArriveDate' runat="server"></label>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="background: #f6f6f6 none repeat scroll 0 0;"><span style=" height:auto;">备 注</span></td>
                                <td>
                                    <textarea id="txtRemark" maxlength="200" class="textarea" style=" width:500px;" placeholder="备注不能超过200个字" runat="server"></textarea>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>

             <div class="tipbtn1">
                 <asp:Button ID="btnSave" CssClass="orangeBtn" style=' display:none;' runat="server" Text="审核通过" onclick="btnSave_Click" />
                 <input type="button" id='Save' class='orangeBtn' value='审核通过' />
                 &nbsp;
                 <asp:Button ID="btnReturn" CssClass="sure" style=' display:none;'  runat="server" Text="退回订单" onclick="btnReturn_Click" />
                 <input type="button" id='Return' class='sure' value='拒绝退货' />
                 &nbsp;
                <input name="" type="button"  class="cancel" value="取消" />
	         </div>
        </div>
    </form>
</body>
</html>
