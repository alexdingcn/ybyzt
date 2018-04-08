<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayMentSettings.aspx.cs" Inherits="Company_SysManager_PayMentSettings" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>支付手续费设置</title>
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

                var hid_compID = $("#hid_CompID").val();  //企业id
                var pay_sxfsq = $('input:radio[name="rdosxfsq"]:checked').val(); //手续费收取

                //支付方式
                var pay_zffs = -1;
                if ($('#che_xs').is(':checked')) {
                    pay_zffs = 0;
                }
                if ($('#che_xx').is(':checked')) {
                    pay_zffs = 1;
                }
                if ($('#che_xx').is(':checked') && $('#che_xs').is(':checked')) {
                    pay_zffs = 2;
                }

                //手续费比例
                var pay_kjzfbl = $("#txt_kjzfbl").val();
                var pay_kjzfstart = $("#txt_kjzfstart").val();
                var pay_kjzfend = $("#txt_kjzfend").val();

                //var pay_ylzfbl = $("#txt_ylzfbl").val();
                //var pay_ylzfstart = $("#txt_ylzfstart").val();
                // var pay_ylzfend = $("#txt_ylzfend").val();

                var pay_b2cwyzfbl = $("#txt_b2cwyzfbl").val();
                var pay_b2cwyzfstart = $("#txt_b2cwyzfstart").val();

                var pay_b2bwyzf = $("#txt_b2bwyzf").val();
                //免手续费支付次数
                var Pay_mfcs = $("#txt_mfcs").val();

                $.ajax({
                    type: 'post',
                    url: 'PayMentSettings.aspx?action=sett',
                    data: { hid_compID: hid_compID, pay_sxfsq: pay_sxfsq, pay_zffs: pay_zffs, pay_kjzfbl: pay_kjzfbl, pay_kjzfstart: pay_kjzfstart, pay_kjzfend: pay_kjzfend, pay_b2cwyzfbl: pay_b2cwyzfbl,pay_b2cwyzfstart:pay_b2cwyzfstart, pay_b2bwyzf: pay_b2bwyzf, Pay_mfcs: Pay_mfcs },
                    async: false, //true:同步 false:异步
                    success: function (result) {
                        var data = eval('(' + result + ')');
                        alert(data["prompt"].toString())
                    }
                });
            });
        });

        function KeyIntCheck(val) {           
                val.value = val.value.replace(/[^\d]/g, '');
        }

        function Goback() {
            location.href = 'CompInfo.aspx?KeyID=<%=KeyID %>';
         }
         function priceKeyup(obj) {
             //own.value = own.value.replace(/[^\d.]/g, '');
             var reg = /^[\d]+$/g;
             if (!reg.test(obj.value)) {
                 var txt = obj.value;
                 var i = 0;
                 var arr = new Array();
                 txt.replace(/[^\d.]/g, function (char, index, val) {//匹配第一次非数字字符
                     arr[i] = index;
                     i++;
                     obj.value = obj.value.replace(/[^\d.]/g, ""); //将非数字字符替换成""
                     var rtextRange = null;
                     if (obj.setSelectionRange) {
                         obj.setSelectionRange(arr[0], arr[0]);
                     } else {//支持ie
                         rtextRange = obj.createTextRange();
                         rtextRange.moveStart('character', arr[0]);
                         rtextRange.collapse(true);
                         rtextRange.select();
                     }
                 });
             }
         }

         function priceBlur(own) {
             own.value = own.value == "" ? "0" : own.value;
             own.value = parseFloat(own.value).toFixed(2);
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
            <a href="#" runat="server" id="atitle">企业管理</a><i>></i>
            <a href="#" runat="server" id="btitle">支付设置</a>
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
                            <span style="width:150px;"><i class="required">*</i>手续费收取</span>
                        </td>
                        <td>
                            <input type="radio" id="rdo_jxs" style=" margin-left:10px;" class="rdo" name="rdosxfsq" value="1" runat="server"/>代理商
                              <input type="radio" id="rdo_hxqy" style=" margin-left:10px;" class="rdo" name="rdosxfsq" value="2" runat="server"/>企业
                            <input type="radio" id="rdo_pt" style=" margin-left:10px;" class="rdo" name="rdosxfsq" value="0"  runat="server"/>平台
                            <i id="IOrderAdd" class="grayTxt">（默认平台）</i>
                            <i class="grayTxt" style=" padding-left:30px;">手续费将有选中方承担</i>
                        </td>
                    </tr>  
                    
                    <tr  style=" display:none;">
                        <td style="width:200px;" align="right">
                            <span style="width:150px;"><i class="required">*</i>支付方式</span>
                        </td>
                        <td>
                             <input type="checkbox" id="che_xs" style=" margin-left:10px;" class="rdo" name="rdoDigits"  runat="server"/>线上支付
                             <input type="checkbox" id="che_xx" style=" margin-left:10px;" class="rdo" name="rdoDigits"  runat="server"/>线下支付
                             <i id="Ipaytype" class="grayTxt">（默认都支持）</i>
                            <i class="grayTxt" style=" padding-left:25px;">  （根据选择的方式，在支付界面显示是否可用）</i>
                        </td>
                    </tr>
                                  
                    <tr>
                        <td style="width:200px;" align="right" >
                            <span><i class="required">*</i>手续费比例</span>
                        </td>
                        <td>
                         &nbsp;   B2C-快捷支付  <input type="text" id="txt_kjzfbl" onkeyup="KeyIntCheck(this);"   onblur="KeyIntCheck(this);" class="textBox" maxlength="5" runat="server"/>‰
                        <%-- &nbsp;  封底 <input type="text" id="txt_kjzfstart" onkeyup="KeyIntCheck(this);"  onblur="KeyIntCheck(this);" class="textBox" maxlength="5" runat="server"/>
                          &nbsp;  封顶 <input type="text" id="txt_kjzfend" onkeyup="KeyIntCheck(this);"   onblur="KeyIntCheck(this);" class="textBox" maxlength="5" runat="server"/>
                            <i id="ComSinceSign" class="grayTxt">（默认2‰ ，2元封底，20元封顶）</i> --%> <br />

                       <%--  &nbsp;   B2C-银联支付 <input type="text" id="txt_ylzfbl" onkeyup="KeyIntCheck(this);"   onblur="KeyIntCheck(this);" class="textBox" maxlength="5" runat="server"/>‰
                           &nbsp;  封底 <input type="text" id="txt_ylzfstart" onkeyup="KeyIntCheck(this);"   onblur="KeyIntCheck(this);" class="textBox" maxlength="5" runat="server"/>
                          &nbsp;  封顶 <input type="text" id="txt_ylzfend" onkeyup="KeyIntCheck(this);"  onblur="KeyIntCheck(this);" class="textBox" maxlength="5" runat="server"/>
                         
                            <i id="I1" class="grayTxt">（默认3‰ ，10元封底、50元封顶）</i><br />--%>
                         &nbsp;  B2C-网银支付  <input type="text" id="txt_b2cwyzfbl" onkeyup="KeyIntCheck(this);"  onblur="KeyIntCheck(this);" class="textBox" maxlength="5" runat="server"/>‰
                        <%--  &nbsp;  封底 <input type="text" id="txt_b2cwyzfstart" onkeyup="priceKeyup(this);"   onblur="priceBlur(this);" class="textBox" maxlength="5" runat="server"/>
                               <i id="I2" class="grayTxt">（ 默认1.2‰ 、0.5元封底）</i>--%> <br /> 
                         &nbsp;   B2B-网银支付  <input type="text" id="txt_b2bwyzf" onkeyup="KeyIntCheck(this);"   onblur="KeyIntCheck(this);" class="textBox" maxlength="5" runat="server"/>
                            <i id="I3" class="grayTxt">（默认10元/笔）</i>
                        </td>
                    </tr>
                    <%--
                     <tr>
                        <td style="width:200px;" align="right">
                            <span><i class="required">*</i>免手续费支付次数</span>
                        </td>
                        <td>
                            <input type="text" id="txt_mfcs" onkeyup="KeyIntCheck(this);"   onblur="KeyIntCheck(this);" class="textBox" maxlength="5" runat="server"/>
                            <i id="I4" class="grayTxt">（默认0次）</i>
                        
                        </td>
                    </tr> --%>

               
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
