<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayCorrectAdd.aspx.cs" Inherits="Company_Pay_PayCorrectAdd" %>
<%@ Register Src="~/Company/UserControl/DisAreaTreeBox.ascx" TagPrefix="uc1" TagName="DisArea" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %><%--
<%@ Register Src="~/Company/UserControl/TextDisList.ascx" TagPrefix="uc1" TagName="DisList" %>--%>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>钱包冲正</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/odrerstyle.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../js/Pay.js" type="text/javascript"></script>
    <script type="text/javascript">
        /*企业钱包冲正 start */
        $(document).ready(function () {

            $_def.ID = "btnSave";

            //选择加盟商
            $("#txtDisID").click(function () {

                ///获取光标位置
                var x = $("#txtDisID").offset().left;
                var y = $("#txtDisID").offset().top;
                var Id = $("#hidCompId").val();
               
                ChoseProductClass('Tree_Dis.aspx?Id=' + Id, x, y);

            });




        });
        //验证用
        function formCheck() {
            var str = "";
            var txtcomp = $("#hidDisUserID").val();
            var txtprice = $("#txtPayCorrectPrice").val();
            var txtremark = $("#txtRemark").val();

            if (txtcomp == "") {
                str += "-请选择代理商；\r\n";
            }
            if (txtprice == "") {
                str += "-请填写冲正金额；\r\n";
               
            } else if (parseFloat(txtprice) <= 0)
                str += "-冲正金额不能小于或等于零；\r\n";
//            if (txtremark=="") {
//                str += "-请填写备注；\r\n";
//            }
            if (txtremark.length > 200) {
                str += "-备注字数不能大于200个字符；\r\n";
            }

            if (str != "") {
                layerCommon.msg(str, IconOption.错误);
                return false;
            } else {
                return true;
            }

        }

        //金额只能输入正数和小数
        function KeyIntPrice(val) {
            val.value = val.value.replace(/[^\d.]/g, '');
        }
    </script>

    <style type="text/css">
        input[type='text']
        {
            width:170px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-2" />

    <div class="rightinfo">
        <!--当前位置 start-->
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面</a></li><li>></li>
                <li><a href="../pay/PayDisList.aspx">钱包查询</a></li><li>></li>
                <li><a href="#">钱包冲正</a></li>
           
            </ul>
        </div>
        <input id="hid_Alert" type="hidden" />
        <input type="hidden" id="hidCompId" runat="server"/>
        <!--当前位置 end-->
  
        <div class="div_content">
            <!--收款帐号管理新增 start-->
            
                <div class="lbtb">
                    <table class="dh">
                        <tr>
                            <td style=" width:110px;">
                                <span><label style=" color:Red; display:inline-block;"><i class="required">*</i></label>&nbsp;代理商名称</span>
                            </td>
                            <td><%--
                                 <uc1:DisList runat="server" ID="DisListID" />--%>
                                  <label id="txtDisUser" runat="server"> </label>
                                 <input type="hidden" id="hidDisUserID" runat="server" />
                                </td>
                             <td style=" width:110px;">
                                <span><label style=" color:Red; display:inline-block;"><i class="required">*</i></label>&nbsp;冲正金额</span>
                            </td>
                            <td>
                                <input id="txtPayCorrectPrice" onkeyup='KeyIntPrice(this)' type="text" runat="server" class="textBox" />               
                             </td>
                        </tr>
                      
                        <tr>
                           <td>
                                 <span>&nbsp;款项来源</span>
                            </td>
                            <td>
                                     <label>预收款</label>
                            </td>                           
                        </tr>
                        
                        <tr>
                            <td><span style=" height:60px;padding-top: 15px;""><span>&nbsp;备注</span></td>
                            <td colspan="3">
                                <textarea id="txtRemark" maxlength="200" class="textarea" placeholder="订单备注不能大于200个字符" runat="server"></textarea>
                            </td>
                        </tr>
                    </table>
                </div>
            <%--</div>--%>

            <!--销售订单主体 start-->

          
            <div class="footerBtn">
                <asp:Button ID="btnSave" runat="server" Text="确定" CssClass="orangeBtn" OnClick="btnSave_Click" OnClientClick="return formCheck()"/>&nbsp;
                <input name="" type="button" onclick="javascript:history.go(-1);" class="cancel" value="返回" />
            </div>
        </div>
    </div>
    
    </form>
</body>
</html>
