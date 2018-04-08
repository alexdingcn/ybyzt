<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PaybankInfo.aspx.cs" Inherits="Admin_Systems_PaybankInfo" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/CompRemove.ascx" TagPrefix="uc1" TagName="CompRemove" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>平台收款账户</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/odrerstyle.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../Company/js/Pay.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            if ('<%=Request["nextstep"] %>' == "1") {
                Orderclass("ktbdskzh");
                document.getElementById("imgmenu").style.display = "block";
            }
        //返回
        $("#cancel").click(function () {
            //history.go(-1);
            //goInfo(<%# Eval("ID") %>)'

            window.location.href = 'PaybankList.aspx';
            
            //window.location.href = 'PayAccountList.aspx';
        });
        //复核
        $("#Audit").click(function () {
            //window.location.href = 'PAbankEdit.aspx?paid=<%=Paid %>&KeyID=<%=KeyID %>';
            $("#btnAudit").trigger("click");
        });
        //编辑
        $("#Edit").click(function () {
            window.location.href = 'PaybankEdit.aspx?nextstep=<%=Request["nextstep"] %>&paid=<%=Paid %>&KeyID=<%=KeyID %>';
            //$("#btnAudit").trigger("click");
        });
        

    });

    //选择代理商返回
    function SelectMaterialsReturn(materialCodes) {
        if (materialCodes != "") {
            form1.txtMaterialCodes.value = materialCodes;
            document.getElementById("btnSelectMaterialsReturn").click();
            var th = $("#hid_Alert").val();
            this.CloseDialog(th);
        }
    }
    </script>
    <style type="text/css">
        input[type='text']
        {
            width: 170px;
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
            <a href="#" runat="server" id="btitle">平台收款账户</a>
    </div>
    <input id="hid_Alert" type="hidden" />
    <!--当前位置 end-->
    <asp:Button ID="btnAudit" Text="审核" runat="server" OnClick="btnAudit_Click" Style="display: none;" />
     <!--功能按钮 start-->
            <div class="tools">
                <ul class="toolbar left">
                
                    <%-- <li id="Audit" runat="server" ><span><img src="../images/t15.png" /></span>复核</li>--%>
                    <li id="Edit" runat="server" ><span><img src="../../Company/images/t02.png" /></span>编辑</li>
                    <%--<li id="Del" runat="server"><span><img src="../images/t03.png" /></span>删除</li> --%>  
                    <li id="cancel" runat="server"><span><img src="../../Company/images/tp3.png" /></span>返回</li>
                    <li id="libtnNext" runat="server" style="display: none;">
                    <a href="javascript:void(0);" onclick="onlinkOrder('../SysManager/CompEdit.aspx?nextstep=1','ktzxsc')"><span><img src="../images/t07.png" /></span><font color="red">下一步</font></a></li>
                </ul>
            </div>
            <!--功能按钮 end-->
        <div class="div_content">
            <!--销售订单主体 start-->
            <%--<div style="padding-left: 70px; ">--%>
            <div class="lbtb">
                <table class="dh">
                    <tr>
                     <td style="width: 15%;">
                            <span>
                                账户类型</span>
                        </td>
                        <td style="width: 30%;">
                            <label id="lblType" runat="server"></label>
                        </td>
                    
                        <td style="width: 15%;">
                            <span>
                                开户银行</span>
                        </td>
                        <td colspan="3">                            
                            <label id="lblddlbank" runat="server"></label>
                        </td>
                       
                    </tr>
                     <tr id="tbdis" runat="server">
                       <td style="width: 15%;">
                            <span>
                                证件类型</span>
                        </td>
                        <td style="width: 30%;">
                            <label id="lblpesontype" runat="server"></label>
                        </td>
                    
                        <td style="width: 15%;">
                            <span>
                                证件号码</span>
                        </td>
                        <td colspan="3">                            
                            <label id="lblpesoncode" runat="server"></label>
                        </td>
                       
                    </tr>
                    <tr>
                     <td style="width: 15%;">
                            <span>
                                账户名称</span>
                        </td>
                        <td style="width: 30%;">
                            <label id="lblDisUser" runat="server"></label>
                        </td>
                        <td>
                            <span>
                               账户号码</span>
                        </td>
                        <td>
                                <label id="lblbankcode" runat="server"></label>
                        </td>
                       
                    </tr>
                    <tr>
                     <td>
                            <span>
                               开户行地址</span>
                        </td>
                        <td>
                          
                             <label id="lblbankAddress" runat="server"></label>   
                        </td>
                        <td>
                            <span>开户行所在省/市</span>
                        </td>
                        <td>
                           
                            <label id="lblprivateCity" runat="server"></label>   
                        </td>
                       
                    </tr>
                    <tr> <td>
                            <span>是否第一收款账户</span>
                        </td>
                        <td colspan="3">
                            
                                 <label id="lblisno" runat="server"></label>   
                        </td>
                     <%--  <td>
                            <span>复核状态</span>
                        </td>
                        <td>
                            
                                 <label id="lblstart" runat="server"></label>   
                        </td>--%>
                        </tr>
                    <tr>
                        <td>
                            <span style="height: 60px; padding-top: 15px;">备注</span>
                        </td>
                        <td colspan="3" >
                            <label id="lblremake" runat="server"></label>   
                        </td>
                    </tr>
                </table>
            </div>
            <%--</div>--%>
            <!--销售订单主体 start-->
            <!--清除浮动-->
            <div style="clear: none;">
            </div>
            <!--销售订单明细 start-->
            <div style="padding-top: 10px;">
                <!--选择商品按钮 start-->
                <%--<div class="tools">
                    <div class="click" id="btnDis">
                        <ul class="toolbar left">
                            <li><span>
                                <img src="../images/t04.png" /></span>关联代理商 </li>
                        </ul>
                    </div>
                </div>--%>
                <!--选择商品按钮 start-->
             
            </div>
            <!--销售订单明细 start-->
            <div class="footerBtn">
            <input id="txtMaterialCodes" type="hidden" name="txtMaterialCodes" runat="server" />
           <input id="txtGoodsCodes" type="hidden" name="txtGoodsCodes" runat="server" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
