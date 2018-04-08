<%@ Page Language="C#" AutoEventWireup="true" CodeFile="hospitalInfo.aspx.cs" Inherits="Admin_Systems_PaybankInfo" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<%@ Register Src="~/Company/UserControl/DisAreaTreeBox.ascx" TagPrefix="uc1" TagName="DisArea" %>
<%@ Register Src="~/Company/UserControl/CompRemove.ascx" TagPrefix="uc1" TagName="CompRemove" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>医院档案详情</title>
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


            //编辑按钮单机事件
            $(document).on("click", "#Edit", function () {
                window.location = "hospitalEdit.aspx?hid=<%= hid%>"
            })

            //启用停用按钮
            $(document).on("click", "#IsEnabled", function () {
                $("#IsEnabledbtn").trigger("click");
            })

            //返回按钮
            $(document).on("click", "#cancel", function () {
              window.location = "hospitalList.aspx"
            })


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
            <a href="#" runat="server" id="btitle">医院档案详情</a>
         
    </div>
   <!--功能按钮 start-->
            <div class="tools">
                <ul class="toolbar left">
                    <li id="Edit" runat="server" ><span><img src="../../Company/images/t02.png" /></span>编辑</li>
                    <li id="IsEnabled" runat="server" ><span><img src="../../Company/images/t06.png" /></span>启用</li>

                    <%--<li id="Del" runat="server"><span><img src="../images/t03.png" /></span>删除</li> --%>  
                    <li id="cancel" runat="server"><span><img src="../../Company/images/tp3.png" /></span>返回</li>
                </ul>
            </div>
   <!--功能按钮 end-->
 
        <div class="div_content">
            <div class="lbtb layoutRegist">
                <table class="dh">
                    <tr>
                        
                        <td style="width: 15%;">
                            <span>
                                <label style="color: Red; display: inline-block;" class="required">
                                    *</label>&nbsp;医院编码</span>
                        </td>
                        <td style="width:35%">
                            <label  runat="server" id="hospitalCode" style="width: 200px;"></label>
                        </td>
                         <td style="width: 15%;">
                            <span>
                                <label style="color: Red; display: inline-block;" class="required">
                                    *</label>&nbsp;医院全称</span>
                        </td>
                        <td>
                            <label  runat="server" id="hospitalName" name="hospitalName" style="width: 200px;"> </label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>
                                <label style="color: Red; display: inline-block;" >
                                   </label>&nbsp;医院级别</span>
                        </td>
                        <td>
                            <label  runat="server" id="hospitalLevel" style="width: 200px;"></label>
                        </td>
                        <td>
                            <span>
                                <label style="color: Red; display: inline-block;">
                                    </label>&nbsp;医院所在省/市/区</span>
                        </td>
                        <td>
                            
                                <label  runat="server" id="hidProvince" style="width: 200px;"></label>/
                                <label  runat="server" id="hidCity" style="width: 200px;"></label>/
                                <label  runat="server" id="hidArea" style="width: 200px;"></label>
                            
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <span>
                                <label style="color: Red; display: inline-block;" >
                                   </label>&nbsp;医院地址</span>
                        </td>
                        <td colspan="3">
                            <label  runat="server" id="address" style="width: 200px;"></label>
                        </td>
                    </tr>
                   
                </table>
            </div>
            <%--</div>--%>
            
            <div style="clear: none;">
            </div>
                    <asp:Button ID="IsEnabledbtn" runat="server" OnClick="IsEnabledbtn_Click"/>
           
        </div>
    </div>
    </form>
</body>
</html>
