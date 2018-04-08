<%@ Page Language="C#" AutoEventWireup="true" CodeFile="updateBill.aspx.cs" Inherits="Company_Order_updateBill" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>发票修改</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/odrerstyle.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../js/order.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $_def.ID = "Save";
            $(".cancel").click(function () {
                window.parent.CloseDialog();
            });

            $("#Save").click(function () {
                $("#btnSave").trigger("click");
            });
        })

    </script>
    <style type="text/css">
        body 
        {
            font-family: "微软雅黑";
            margin: 0 auto;
            min-width: 500px;
        }
        .tipinfo
        {
            width:500px;
            height:auto;
        }
        
        .tipbtn1 {
           margin-bottom: 10px;
           margin-left: 200px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="tipinfo">
            <div class="rightinfo" style=" margin-top:0px; margin-left:0px; width:auto;">
                <div class="div_content">
                    <div class="lbtb">
                        <table class="dh">
                            <tr>
                                <td style=" width:30%;"><span>发票号</span></td>
                                <td style=" width:70%;">
                                    <input type="text" id="txtBillNo" runat="server"  class="textBox" maxlength="50" style="width:250px;" autocomplete="off" />
                                </td>
                            </tr>
                            <tr>
                                <td><span>发票是否收完</span></td>
                                <td style=" padding-left:10px;">
                                    <input type="radio" id="rdoIsBillOk" runat="server" value="1" name="rdoIsbill" />
                                    <label for="rdoIsBillOk" style=" padding-left:0px;">收完</label>

                                    <input type="radio" id="rdoIsBillNo" runat="server" value="0" name="rdoIsbill" />
                                    <label for="rdoIsBillNo" style=" padding-left:0px;">未收完</label>
                                </td>
                            </tr>
                            
                        </table>
                    </div>
                </div>
            </div>

             <div class="tipbtn1">
                 <asp:Button ID="btnSave" CssClass="orangeBtn" style=' display:none;' runat="server" Text="保存" onclick="btnSave_Click" />
                 <input type="button" id='Save' runat="server" class='orangeBtn' value='保存' />
                 &nbsp;
                <input name="" type="button"  class="cancel" value="取消" />
	         </div>
        </div>
    </form>
</body>
</html>
