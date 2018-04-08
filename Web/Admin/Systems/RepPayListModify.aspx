<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RepPayListModify.aspx.cs" Inherits="Admin_Systems_RepPayListModify" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>销售管理</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>   
   <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    
    <script src="../../js/CommonJs.js" type="text/javascript"></script>

     <%--附件上传  start--%>
    <script src="../../js/UploadJs.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //上传附件用到方法
            $("#uploadFile").AjaxUploadFile({ Src: "UploadFile/", ShowDiv: "UpFileText", ResultId: "HidFfileName", AjaxSrc: "../../Controller/Fileup.ashx", maxlength: 5 });

        })
    </script>
    <%--附件上传   end--%>

    <script type="text/javascript">
        $(document).ready(function () {
            $('.tb tbody tr td:even').addClass('odd');
        })



        function formCheck() {

            var str = "";
            if ($("#ddrcljg").val() == "-1") {
                str = "请选择处理结果！";
            } 

            if (str != "") {
                errMsg("提示", str, "", "");
                return false;
            }
            return true;
        }
    </script>
    
</head>
<body>
    <form id="form1" runat="server">
    <!--当前位置 start-->
    <uc1:Header ID="Header1" runat="server" />
    <uc2:Left ID="Left1" runat="server" />
    <div class="m-con">
    <div class="m-place">
	        <i>位置：</i>
            <a href="../index.aspx" target="_top">我的桌面</a><i>></i>
            <a href="#">报表查询</a><i>></i>
            <a href="UserList.aspx">账单处理</a>
    </div>
    <!--当前位置 end-->
        <div class="div_content">
            <table class="tb">
                <tbody>

                    <tr>
                        <td width="120">
                            <span><i class="required">*</i>交易流水号</span>
                        </td>
                        <td>                              <label runat="server" id="lblzflsh">
                            </label>
                        </td>
                        <td width="120">
                            <span>支付时间</span>
                        </td>
                        <td>
                            <label runat="server" id="lblzfsj">
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span><i class="required">*</i>支付类型</span>
                        </td>
                        <td>
                           <label runat="server" id="lblzflx">
                         </label> </td>
                        <td>
                            <span><i class="required">*</i>对应单号</span>
                        </td>
                        <td>
                               <label runat="server" id="lbldydh">
                         </label>  </td>
                    </tr>
                    <tr>
                        <td >
                            <span>付款方</span>
                        </td>
                        <td>
                               <label runat="server" id="lblfkf">
                         </label>
                        </td>
                        <td>
                            <span>收款方</span>
                        </td>
                        <td>
                               <label runat="server" id="lblskf">
                         </label>
                        </td>
                    </tr>
                    <tr>
                        <td width="120">
                            <span><i class="required">*</i>付款银行</span>
                        </td>
                        <td>
                                <label runat="server" id="lblfkyh">
                         </label>
                        </td>
                        <td width="120">
                            <span>收款银行</span>
                        </td>
                        <td>
                               <label runat="server" id="lblskyh">
                         </label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span><i class="required">*</i>付款账户</span>
                        </td>
                        <td>
                              <label runat="server" id="lblfkzh">
                         </label> </td>
                        <td>
                            <span><i class="required">*</i>收款账户</span>
                        </td>
                        <td>
                               <label runat="server" id="lblskzh">
                         </label> </td>
                    </tr>
                    <tr>
                        <td >
                            <span>支付金额</span>
                        </td>
                        <td>
                              <label runat="server" id="lblzfje">
                         </label>
                        </td>
                        <td>
                            <span>手续费</span>
                        </td>
                        <td>
                                <label runat="server" id="lblsxf">
                         </label>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            <span>清算结果</span>
                        </td>
                        <td>
                                <label runat="server" id="lblqsjg">
                         </label>
                        </td>
                        <td>
                            <span>清算时间</span>
                        </td>
                        <td>
                            <label runat="server" id="lblqssj">
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>清算失败原因</span>
                        </td>
                        <td colspan="3">
                            <textarea style="height: auto; width: 500px;" rows="3"  class="textBox" runat="server" maxlength="1000"
                                id="txtqssbyy"></textarea>
                        </td>
                    </tr> 
                     <tr>
                        <td >
                            <span>处理结果</span>
                        </td>
                        <td>
                            <select name="PayState" runat="server" id="ddrcljg" class="downBox">
                                <option value="1">成功</option>
                                <option value="0">失败</option>
                                
                            </select>&nbsp;&nbsp;
                        </td>
                        <td>
                            <span>处理时间</span>
                        </td>
                        <td>
                         <input name="txtArriveDate"  runat="server" onclick="WdatePicker({minDate:'%y-%M-%d'})" id="txtArriveDate"
                            readonly="readonly" type="text" class="textBox" />
                        </td>
                    </tr>
                      <tr>
                        <td>
                            <span>说明</span>
                        </td>
                        <td colspan="3">
                            <textarea style="height: auto; width: 500px;" rows="3" class="textBox" runat="server" maxlength="1000"
                                id="txtsm"></textarea>
                        </td>
                    </tr>
                     <tr>
                        <td>
                            <span>附件</span>
                        </td>
                        <td colspan="3">
                             <div class="teamR" id="editdiv" runat="server" style=" margin-left:5px;">
                                <div class="verFile" style="margin: 0px; width: 100%;">
                                    <span class="verFileCon" style="min-width: 60px; text-align:left;">
                                         <input id="uploadFile"   runat="server" type="file" name="fileAttachment" /></span>
                               <%--  <a class="btn1" id="A1" style="margin-left: 5px; text-decoration: NONE;" href="javascript:void(0)">
                                        <b class="L"></b><b class="R"></b>上传凭证</a>--%>
                                </div>
                                <asp:Panel runat="server" ID="DFile" Style="margin: 5px 5px;">
                                </asp:Panel>
                            </div>
                            <div id="UpFileText" style="margin: 5px 5px;">
                            </div>
                            <input runat="server" id="HidFfileName" type="hidden" />

                            <asp:Panel runat="server" id="DFileinfo"  style=" margin-left:5px;"></asp:Panel>
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
