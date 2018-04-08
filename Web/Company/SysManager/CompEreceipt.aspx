<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompEreceipt.aspx.cs" Inherits="Company_SysManager_CompEreceipt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>仓单信息</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script>
        $(function ($) {
            $("#libtnSave").on("click", function () {
                $("#btnSave").trigger("click");
            });

            $("#li1").on("click", function () {
                window.parent.layerClose();
            });
        });

        function formCheck() {
            
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="rightinfo" style=" margin-top:0px; margin-left:0px;">
            <div class="tools" id="DisBoot" runat="server">
                <ul class="toolbar left">
                    <li id="libtnSave" runat="server" ><span><img src="../images/t15.png" /></span><font>确定</font></li>

                    <li id="li1" runat="server" ><span><img src="../images/t03.png" /></span>关闭</li>
                </ul>
            </div>
            <asp:Button ID="btnSave" runat="server" Text="确定"  OnClientClick="return formCheck()" onclick="btn_Save" style="display:none;" />
            <div class="div_content">
                <table  class="tb">
                    <tbody>
                        <tr>
                            <td><span><i class="required">*</i>仓库编号</span></td>
                            <td><input type="text" id="txtereceipt_whno" runat="server"  class="textBox" maxlength="200"/></td>

                            <td><span><i class="required">*</i>仓库名称</span></td>
                            <td><input type="text" id="txtereceipt_whnm" runat="server"  class="textBox" maxlength="200"/></td>
                        </tr>

                        <tr>
                            <td><span><i class="required">*</i>批次号</span></td>
                            <td><input type="text" id="txtereceipt_batchno" runat="server"   class="textBox" maxlength="200"/></td>

                            <td><span><i class="required">*</i>生产厂家</span></td>
                            <td><input type="text" id="txtereceipt_mfters" runat="server"  class="textBox" maxlength="200"/></td>
                        </tr>
                        <tr>
                            <td><span><i class="required">*</i>规格</span></td>
                            <td><input type="text" id="txtereceipt_std" runat="server"  class="textBox" maxlength="20"/></td>

                            <td><span><i class="required">*</i>失效日期</span></td>
                            <td><input type="text" id="txtereceipt_duedate" runat="server"  class="Wdate" onclick="WdatePicker()" maxlength="200" /></td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
