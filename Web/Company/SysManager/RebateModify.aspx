<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RebateModify.aspx.cs" Inherits="Company_SysManager_RebateModify" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<%@ Register TagPrefix="uc2" TagName="DisDemo" Src="~/Company/UserControl/TreeDisName.ascx" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>返利维护</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(document).ready(function() {
            $_def.ID = "btnAdd";
            $('.tb tbody tr td:even').addClass('odd');
            $("#txtDisID_txt_txtDisName").css("width", "300px");
        });

        var t1, t2;
        function DefaultAddr(t1,t2) {
        }

        function CheckReValue() {
            $("#txtRebateAmount").val(formatMoney($.trim($("#txtRebateAmount").val(), 1)));
        }

        function formCheck() {
            var str = "";
            if ($.trim($("#txtDisID_txt_txtDisName").val()) == "") {
                str += "代理商名称不能为空。";
            }
            if ($.trim($("#txtCode").val()) == "") {
                str += "返利单编号不能为空。";
            }
            if ($.trim($("#txtRebateAmount").val()) == "") {
                str += "本次返利金额不能为空。";
            }
            if ($.trim($("#txtStartDate").val()) == "") {
                str += "有效期开始时间不能为空。";
            } else {
                var date1 = getNowFormatDate();
                var a1 = new Date(date1);
                var a2 = new Date($.trim($("#txtStartDate").val()));
                if (a1 > a2) {
                    str += "有效期开始时间不能小于今天的日期。";
                } 
             }
            if ($.trim($("#txtEndDate").val()) == "") {
                str += "有效期结束时间不能为空。";
            }
            else {
                var date1 = getNowFormatDate();
                var a1 = new Date(date1);
                var a2 = new Date($.trim($("#txtStartDate").val()));
                var a3 = new Date($.trim($("#txtEndDate").val()));
                if (a1 > a3) 
                    str += "有效期结束时间不能小于今天的日期。";                
//                if(a2>a3)
//                    str += "有效期结束时间不能小于有效期开始时间。";


            }
//            if ($('input[name="rdotype"]:checked ').length == 0 ) {
//                str += "返利类型不能为空。";
//            }
            if (str != "") {
                layerCommon.msg(str, IconOption.错误);
                return false;
            }
            return true;
        }


        function getNowFormatDate() {
            var date = new Date();
            var seperator1 = "-";
            var seperator2 = ":";
            var year = date.getFullYear();
            var month = date.getMonth() + 1;
            var strDate = date.getDate();
            if (month >= 1 && month <= 9) {
                month = "0" + month;
            }
            if (strDate >= 0 && strDate <= 9) {
                strDate = "0" + strDate;
            }
            var currentdate = year + seperator1 + month + seperator1 + strDate;
           // + " " + date.getHours() + seperator2 + date.getMinutes()
           // + seperator2 + date.getSeconds();
            return currentdate;
        }

        //金额只能输入正数和小数
        function KeyIntPrice(val) {
            val.value = val.value.replace(/[^\d.]/g, '');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-2" />
    <div class="rightinfo">

    <!--当前位置 start-->
    <div class="place">
        <ul class="placeul">
            <li><a href="../jsc.aspx" target="_top">我的桌面 </a></li><li>></li>
            <li><a href="../SysManager/RebateList.aspx">返利查询</a></li><li>></li>
            <li><a href="#">返利单维护</a></li>
        </ul>
    </div>
    <!--当前位置 end-->
        <div class="div_content">
            <table class="tb">
                <tbody>
                    <tr>
                        <td width="120">
                            <span><i class="required">*</i>代理商名称</span>
                        </td>
                        <td>
                            <uc2:DisDemo runat="server" ID="txtDisID" />
                        </td>
                    </tr>
                    <tr>
                        <td width="120">
                            <span><i class="required">*</i>返利单编号</span>
                        </td>
                        <td>
                            <input type="text" id="txtCode" runat="server" class="textBox" maxlength="18"
                                style="width: 160px;" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span><i class="required">*</i>本次返利金额</span>
                        </td>
                        <td class="newspan">
                            <input type="text" id="txtRebateAmount"  onkeyup='KeyIntPrice(this)' runat="server" class="textBox" maxlength="10"
                                style="width: 160px;" onblur="CheckReValue() KeyIntPrice(this)"/>
                        </td>
                    </tr>

                    <%--<tr>
                        <td>
                            <span><i class="required">*</i>返利类型</span>
                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <input type="radio" id="RdType1" name="rdotype" runat="server" value="1" checked="True"/>&nbsp;&nbsp;整单返利&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <input type="radio" id="RdType2" name="rdotype" runat="server" value="2" />&nbsp;&nbsp;分摊返利&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>--%>
                    
                    <tr>
                        <td>
                            <span><i class="required">*</i>有效期</span>
                        </td>
                        <td class="newspan">
                            <input name="txStartDate" runat="server" onclick="var endDate=$dp.$('txtEndDate'); WdatePicker({onpicked:function(){endDate.focus();},maxDate:'#F{$dp.$D(\'txtEndDate\')}'})" id="txtStartDate" readonly="readonly" type="text" class="Wdate" value="" style="margin-left: 5px;width: 150px;" />
                            &nbsp;-&nbsp;
                            <input name="txtEndDate" runat="server" onclick="WdatePicker({minDate:'#F{$dp.$D(\'txtStartDate\')}'})" id="txtEndDate" readonly="readonly" type="text" value="" class="Wdate" style="margin-left: 5px;width: 150px;" />
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <span>备 注</span>
                        </td>
                        <td class="newspan">
                            <textarea id="txtRemark" maxlength="200" rows="3" name="" style="width:700px;height: 70px;border: 1px solid #d1d1d1;text-indent: 5px;margin-top: 2px;margin-bottom: 2px;margin-left: 5px;" runat="server" class="box3"></textarea>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="div_footer">
            <asp:Button ID="btnAdd" CssClass="orangeBtn" runat="server" Text="确定" OnClientClick="return formCheck()"
                OnClick="btnAdd_Click" />&nbsp;
            <input name="" type="button" class="cancel" onclick="javascript:history.go(-1);"
                value="取消" />
        </div>
    </div>
    </form>
</body>
</html>
