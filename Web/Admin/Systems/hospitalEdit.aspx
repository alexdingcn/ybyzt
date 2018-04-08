<%@ Page Language="C#" AutoEventWireup="true" CodeFile="hospitalEdit.aspx.cs" Inherits="Admin_Systems_PaybankEdit" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<%@ Register Src="~/Company/UserControl/DisAreaTreeBox.ascx" TagPrefix="uc1" TagName="DisArea" %>
<%@ Register Src="~/Company/UserControl/CompRemove.ascx" TagPrefix="uc1" TagName="CompRemove" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>医院档案编辑</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/odrerstyle.css" rel="stylesheet" type="text/css" />
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript">
    </script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script src="../../js/CommonJs1.js" type="text/javascript"></script>
    <script src="../js/CitysLine/JQuery-Citys-online-min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txtbankcode").keyup(function () {
                $(this).val($(this).val().replace(/\D/g, '').replace(/....(?!$)/g, '$& '));
            });
        });

      
        //验证用
        function formCheck() {
            var hospitalCode = $("#hospitalCode").val();//医院编码
            var hospitalName = $("#hospitalName").val();//医院全称
            var msg = "";//错误提示
            if (hospitalCode == "") {
                msg += "-医院编码不能为空！；\r\n";
            }
            else if (hospitalName == "") {
                msg += "-医院编码不能为空！；\r\n";
            }

            if (msg == "") {
                return true;
            }
            else {
                alert(msg);
                return false;
            }
           
        }

      
    </script>
    <style type="text/css">
        input[type='text']
        {
            width: 170px;
        }
        .layoutRegist select
        {
            width: 100px;
            display: inline-block;
            height: 23px;
            border: 2px;
            line-height: 23px;
            border: 1px solid #ddd;
        }
        .layoutRegist p
        {
            color: #1b2b3b;
            font-size: 9px;
            border: 2px;
            padding-left: 30px;
            position: relative;
        }
        .layoutRegist p span
        {
            width: 60px;
            text-align: left;
            position: absolute;
            left: 50px;
            top: 10px;
        }
        .select
        {
            line-height: 23px;
            display: inline-block;
            height: 23px;
            width: 85px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:CompRemove runat="server" ID="Remove" ModuleID="4" />
    <!--当前位置 start-->
    <uc1:Header ID="Header1" runat="server" />
    <uc2:Left ID="Left1" runat="server" />
    <div class="m-con">
    <div class="m-place">
	        <i>位置：</i>
            <a href="../index.aspx" target="_top">我的桌面</a><i>></i>
            <a href="#" runat="server" id="atitle">系统管理</a><i>></i>
            <a href="#" runat="server" id="btitle">医院档案编辑</a>
         
    </div>
   
 
        <div class="div_content">
            <div class="lbtb layoutRegist">
                <table class="dh">
                    <tr>
                        
                        <td>
                            <span>
                                <label style="color: Red; display: inline-block;" class="required">
                                    *</label>&nbsp;医院编码</span>
                        </td>
                        <td>
                            <input name="hospitalCode" maxlength="18" runat="server" id="hospitalCode" type="text"
                                class="textBox"  style="width: 200px;" />&nbsp;
                        </td>
                         <td style="width: 15%;">
                            <span>
                                <label style="color: Red; display: inline-block;" class="required">
                                    *</label>&nbsp;医院全称</span>
                        </td>
                        <td style="width: 30%;">
                            <input name="hospitalName" maxlength="50" runat="server" id="hospitalName" type="text"
                                class="textBox" value="" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>
                                <label style="color: Red; display: inline-block;" >
                                   </label>&nbsp;医院级别</span>
                        </td>
                        <td>
                            <input name="hospitalLevel" maxlength="50"  style="width: 200px;"  runat="server" 
                                id="hospitalLevel" type="text" class="textBox" />
                        </td>
                        <td>
                            <span>
                                <label style="color: Red; display: inline-block;">
                                    </label>&nbsp;医院所在省/市/区</span>
                        </td>
                        <td>
                            <p>
                                <select runat="server" id="ddlProvince" class="prov select1 l" onchange="Change()"
                                    style="margin-left: -24px;">
                                </select>
                                <input type="hidden" id="hidProvince" runat="server" value="请选择省份" />
                                <select runat="server" id="ddlCity" class="city select" onchange="Change()">
                                </select>
                                <input type="hidden" id="hidCity" runat="server" value="请选择市" />
                                <select runat="server" id="ddlArea" class="dist select" onchange="Change()">
                                </select>
                                <input type="hidden" id="hidArea" runat="server" value="请选择区" />
                                <input type="hidden" id="hidCode" runat="server" />
                                <span id="spandiqu" style="color: #da5132; margin: 0 0 0 350px; width: 80px;"></span> 
                                </p>
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <span>
                                <label style="color: Red; display: inline-block;" >
                                   </label>&nbsp;医院地址</span>
                        </td>
                        <td colspan="3">
                            <input name="address" maxlength="80" runat="server" id="address" type="text"
                                class="textBox" value="" style="width: 450px;" />
                        </td>
                    </tr>
                   
                </table>
            </div>
            <%--</div>--%>
            
            <div style="clear: none;">
            </div>
          
            <div class="footerBtn">
                <asp:Button ID="btnSave" runat="server" Text="确定" CssClass="orangeBtn" OnClick="btnSave_Click"
                    OnClientClick="return formCheck()" />&nbsp;
            </div>
        </div>
    </div>
    </form>
</body>
</html>
