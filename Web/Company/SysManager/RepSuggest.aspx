<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RepSuggest.aspx.cs" Inherits="Company_SysManager_RepSuggest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>订单审核</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/odrerstyle.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>

    <script src="../js/order.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function() {
            $(".cancel").click(function() {
                window.parent.Layerclose();
            });
        });
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
            <div class="rightinfo" style="margin-top:0px; margin-left:0px; width:640px;">
                <div class="div_content">
                    <div class="lbtb">
                        <table class="dh">
                            <tr>
                                <td><span >咨询内容</span></td>
                                <td style="text-align:left;">
                                    <label runat="server" id="lblContent"></label>
                                </td>
                            </tr>
                            <tr>
                                <td><span style=" height:62px;">回复内容</span></td>
                                <td>
                                    <textarea id="txtRemark" maxlength="800" class="textarea" style=" width:500px;" placeholder="备注不能大于800个字符" runat="server"></textarea>
                                </td>
                            </tr>
                            <tr>
                                <td style=" width:30%;"><span>回复人</span></td>
                                <td style=" width:70%;">
                                    <label id="lblAuditUserID" runat="server"></label>
                                    <input type="hidden" id="hidAuditUserID" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>

             <div class="tipbtn1">
                 <asp:Button ID="btnSave" CssClass="orangeBtn" runat="server" Text="回复" onclick="btnSave_Click"/>&nbsp;
                <input name="" type="button"  class="cancel" value="取消" />
	         </div>
        </div>
    </form>
</body>
</html>
