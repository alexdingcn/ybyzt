<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompCtinfo.aspx.cs" validateRequest="false" Inherits="Company_SysManager_CompCtinfo" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>自定义公司介绍</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>
    <link href="../../kindeditor/themes/default/default.css" rel="stylesheet" type="text/css" />
    <link href="../../kindeditor/plugins/code/prettify.css" rel="stylesheet" type="text/css" />
    <script src="../../kindeditor/kindeditor.js" type="text/javascript"></script>
    <script src="../../kindeditor/lang/zh_CN.js" type="text/javascript"></script>
    <script src="../../js/xss.js"></script>
    <script src="../../kindeditor/plugins/code/prettify.js" type="text/javascript"></script>
    <script>
        function check() {
            if ($.trim($("#<%=txtCustomInfo.ClientID %>").val()).length > 4000) {
                errMsg("提示", "自定义地址不能超过4000字符", "", "");
                return false;
            }
            if ($.trim($("#txtCustomInfo").val()) != "") {
                var html = filterXSS($.trim($("#txtCustomInfo").val()));
                $("#txtCustomInfo").val(html);
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-6" />
       <div class="rightinfo">
       <div class="place">
	        <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
                <li><a href="#">信息发布</a></li><li>></li>
                <li><a href="#" runat="server" id="Atitle">自定义公司介绍</a></li>
	        </ul>
     </div>
               <div class="div_content">
               <div class="div_title" >
                  企业自定义公司介绍:
               </div>

                 <table  class="tb">
                   <tbody>
                     <tr>
                       <td style=" width:15%;"><span>公司介绍</span> </td>
                       <td  class="newspan"> <div runat="server" style=" margin-left:5px;" id="div_CustomInfo"></div> <asp:TextBox ID="txtCustomInfo"  runat="server" TextMode="MultiLine" Height="270px" Width="800px" style=" display:none;" class="textBox"></asp:TextBox> 
                               <script>
                                   function Load () {
                                       KindEditor.ready(function (K) {
                                           window.editor = K.create('#txtCustomInfo', {
                                               uploadJson: '../../Kindeditor/asp.net/upload_json.ashx',
                                               fileManagerJson: '../../Kindeditor/asp.net/file_manager_json.ashx',
                                               allowFileManager: true
                                           });
                                       });
                                   } 
                            </script>
                       
                       </td>
                     </tr>
                   </tbody>
                 </table>
               </div>

                <div  class="div_footer">
                    <asp:Button ID="btnSave" CssClass="orangeBtn" runat="server" Text="确定"  Visible="false" OnClientClick="return check();"   onclick="btnSave_Click" />&nbsp;
                    <asp:Button ID="btnEdit" CssClass="orangeBtn" runat="server" Text="编辑"   onclick="btnEdit_Click" />&nbsp;
                    <input  id="btnback" type="button" class="cancel" Visible="false" runat="server" onclick="javascript:history.go(-1);" value="返回" />
               </div>
       </div>


    </form>
</body>
</html>
