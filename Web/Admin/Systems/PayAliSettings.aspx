<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayAliSettings.aspx.cs" Inherits="Company_SysManager_PayAliSettings" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>支付宝账户设置</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>

    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/layer.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('.tb tbody tr td').each(function (index, obj) {
                if (index % 2 == 0) {
                    $(obj).addClass('odd');
                }
            });

            //确定按钮
            $("#btnAdd").click(function () {

                var chkisno = 0;
                //是否启用
                if (document.getElementById("chisno").checked) {
                    chkisno = 1;
                }
                //手续费比例
                var seller_email = $("#seller_email").val(); //appid
                var partner = $("#partner").val(); //appsecret
                var PayKey = $("#PayKey").val(); //mchid
                var hid_compID = $("#hid_CompID").val();
                var alirsa = $("#alirsa").val();

                var str = "";
                if (seller_email == "") {
                    str += "请填写支付宝企业账户！";
                }
                if (partner == "") {
                    str += "请填写合作者身份！";
                }
                if (PayKey == "") {
                    str += "请填写安全验证码！";
                }
                if (alirsa == "") {
                    str += "请填写RSA秘钥！";
                }
                if (str != "") {
                    errMsg("提示", str, "", "");
                    return false;

                }


                $.ajax({
                    type: 'post',
                    url: 'PayAliSettings.aspx?action=sett',
                    data: { hid_compID: hid_compID, chkisno: chkisno, seller_email: seller_email, partner: partner, PayKey: PayKey, alirsa: alirsa },
                    async: false, //true:同步 false:异步
                    success: function (result) {
                        var data = eval('(' + result + ')');
                        alert(data["prompt"].toString())
                    }
                });
            });
        });

        function KeyIntCheck(val) {
            val.value = val.value.replace(/([^\u0000-\u00FF])/g, ''); //输入汉字自动替换为空
        }

        function Goback() {
            location.href = 'CompInfo.aspx?KeyID=<%=KeyID %>';
         }
     
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <uc1:Header ID="Header1" runat="server" />
    <uc2:Left ID="Left1" runat="server" />
    <div class="m-con">
    <div class="m-place">
	        <i>位置：</i>
            <a href="../index.aspx" target="_top">我的桌面</a><i>></i>
            <a href="#" runat="server" id="atitle">厂商管理</a><i>></i>
            <a href="#" runat="server" id="btitle">支付宝收款帐号设置</a>
    </div>
        <div class="tools">
            <ul class="toolbar left">
                <input type="hidden" id="hid_Alert"/>
               
            </ul>
        </div>
        <div class="div_content">
            <table class="tb">
                <tbody>   
                 <tr>
                        <td style="width:200px;" align="right">
                            <span style="width:150px;"><i class="required">*</i>支付宝帐号</span>
                        </td>
                        <td colspan="3">
                            <input type="checkbox" id="chisno" style=" margin-left:10px; width:20px; height:20px; margin-top:10px;"  name="rdosxfsq" class="box"  runat="server"/>是否启用
                        </td>
                    </tr>  
                    
                    <tr>
                        <td style="width:200px;" align="right">
                            <span style="width:150px;"><i class="required">*</i>支付宝企业账户</span>
                        </td>
                        <td>
                             <input type="text" id="seller_email" onkeyup="KeyIntCheck(this);"  style="width:240px;"  onblur="KeyIntCheck(this);" class="textBox" maxlength="40" runat="server"/>
                             <i id="Ipaytype" class="grayTxt">（绑定支付的APPID）</i>
                        </td>
                          <td style="width:200px;" align="right">
                            <span style="width:150px;"><i class="required">*</i>合作者身份</span>
                        </td>
                        <td>
                             <input type="text" id="partner" onkeyup="KeyIntCheck(this);"   onblur="KeyIntCheck(this);" class="textBox" maxlength="40" runat="server"/>
                             <i id="I5" class="grayTxt">（PartnerId）</i>
                        </td>
                    </tr>
                       <tr>
                        <td style="width:200px;" align="right">
                            <span style="width:150px;"><i class="required">*</i> 安全验证码（Key）</span>
                        </td>
                        <td >
                             <input type="text" id="PayKey" onkeyup="KeyIntCheck(this);"  style="width:240px;"  onblur="KeyIntCheck(this);" class="textBox" maxlength="40" runat="server"/>
                             <i id="I1" class="grayTxt"></i>
                        </td>
                        <td style="width:200px;" align="right">
                            <span style="width:150px;"><i class="required">*</i> RSA秘钥</span>
                        </td>
                        <td>
                             <input type="text" id="alirsa" onkeyup="KeyIntCheck(this);"  style="width:240px;"  onblur="KeyIntCheck(this);" class="textBox" maxlength="400" runat="server"/>
                             <i id="I2" class="grayTxt"></i>
                        </td>
                         
                    </tr>           
                   

               
                </tbody>
            </table>
        </div>
        <div class="div_footer">
            <asp:Button ID="btnAdd" CssClass="orangeBtn" runat="server" Text="确定" />&nbsp;
            <input name="" type="button" class="cancel" onclick="Goback()" value="返回"/>
        </div>
    </div>

     <input type="hidden" id="hid_CompID"  runat="server"/>
    </form>
</body>
</html>
