<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayWxSettings.aspx.cs" Inherits="Company_SysManager_PayWxSettings" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>微信账户设置</title>
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
                var appid = $("#appid").val(); //appid
                var appsecret = $("#appsecret").val(); //appsecret
                var mchid = $("#mchid").val(); //mchid
                var key = $("#key").val(); //key
                var hid_compID = $("#hid_CompID").val();


                var str = "";
                if (appid == "") {
                    str += "请填写AppID！";
                }
                if (mchid == "") {
                    str += "请填写MCHID！";
                }
                if (key == "") {
                    str += "请填写KEY！";
                }
                if (str != "") {
                    errMsg("提示", str, "", "");
                    return false;

                }




                $.ajax({
                    type: 'post',
                    url: 'PayWxSettings.aspx?action=sett',
                    data: { hid_compID: hid_compID, chkisno: chkisno, appid: appid, appsecret: appsecret, mchid: mchid, key: key },
                    async: false, //true:同步 false:异步
                    success: function (result) {
                        var data = eval('(' + result + ')');
                        alert(data["prompt"].toString())
                    }
                });
            });
        });

        function KeyIntCheck(val) {
            val.value = val.value.replace(/([^\u0000-\u00FF])/g, '');//输入汉字自动替换为空
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
            <a href="#" runat="server" id="btitle">微信收款帐号设置</a>
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
                            <span style="width:150px;"><i class="required">*</i>微信帐号</span>
                        </td>
                        <td colspan="3">
                            <input type="checkbox" id="chisno" style=" margin-left:10px; width:20px; height:20px; margin-top:10px;"  name="rdosxfsq" class="box"  runat="server"/>是否启用
                        </td>
                    </tr>  
                    
                    <tr>
                        <td style="width:200px;" align="right">
                            <span style="width:150px;"><i class="required">*</i>AppID（应用ID）</span>
                        </td>
                        <td>
                             <input type="text" id="appid" onkeyup="KeyIntCheck(this);" style="width:230px;"   onblur="KeyIntCheck(this);" class="textBox" maxlength="40" runat="server"/>
                             <i id="Ipaytype" class="grayTxt"></i>
                        </td>
                          <td style="width:200px;" align="right">
                            <span style="width:150px;"><%--<i class="required">*</i>--%>APPSECRET</span>
                        </td>
                        <td>
                             <input type="text" id="appsecret" onkeyup="KeyIntCheck(this);"  style="width:240px;"  onblur="KeyIntCheck(this);" class="textBox" maxlength="60" runat="server"/>
                             <i id="I5" class="grayTxt">（公众帐号secert）</i>
                        </td>
                    </tr>
                       <tr>
                        <td style="width:200px;" align="right">
                            <span style="width:150px;"><i class="required">*</i> MCHID</span>
                        </td>
                        <td>
                             <input type="text" id="mchid" onkeyup="KeyIntCheck(this);"   onblur="KeyIntCheck(this);" class="textBox" maxlength="40" runat="server"/>
                             <i id="I1" class="grayTxt">（商户号）</i>
                        </td>
                          <td style="width:200px;" align="right">
                            <span style="width:150px;"><i class="required">*</i>KEY</span>
                        </td>
                        <td>
                             <input type="text" id="key" onkeyup="KeyIntCheck(this);" style="width:240px;"  onblur="KeyIntCheck(this);" class="textBox" maxlength="60" runat="server"/>
                             <i id="I2" class="grayTxt">（商户支付密钥，参考开户邮件设置）</i>
                        </td>
                    </tr>           
                   

               
                </tbody>
            </table>
        </div>
        <div class="div_footer">
            <asp:Button ID="btnAdd" CssClass="orangeBtn" runat="server" Text="确定" />&nbsp;
            <input name="" type="button" class="cancel" onclick="Goback()" value="返回"/>
            <%--<input name="" id="Log" type="button" class="cancel"  value="日志"/>--%>
        </div>
    </div>

     <input type="hidden" id="hid_CompID"  runat="server"/>
    </form>
</body>
</html>
