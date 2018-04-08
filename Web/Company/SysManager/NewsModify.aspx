<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NewsModify.aspx.cs" Inherits="NewsModify" validateRequest="false" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>信息新增</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <link href="../../kindeditor/themes/default/default.css" rel="stylesheet" type="text/css" />
    <link href="../../kindeditor/plugins/code/prettify.css" rel="stylesheet" type="text/css" />
    <script src="../../kindeditor/kindeditor.js" type="text/javascript"></script>
    <script src="../../kindeditor/lang/zh_CN.js" type="text/javascript"></script>
    <script src="../../js/xss.js"></script>
    <script src="../../kindeditor/plugins/code/prettify.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            $_def.ID = "btnAdd";
            $('.tb tbody tr td:even').addClass('odd');
        })

        function formCheck() {
            var str = "";
            if ($.trim($("#txtNewsTitle").val()) == "") {
                str = "新闻标题不能为空";
            }
            if (str != "") {
                layerCommon.msg(str, IconOption.错误);
                return false;
            }
            if ($.trim($("#content7").val()) != "") {
                var html = filterXSS($.trim($("#content7").val()));
                $("#content7").val(html);
            }
            return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-6" />
    <div class="rightinfo">

    <!--当前位置 start-->
    <div class="place">
        <ul class="placeul">
            <li><a href="../jsc.aspx" target="_top">我的桌面 </a></li><li>></li>
            <li><a href="../SysManager/NewsList.aspx">信息发布</a></li><li>></li>
            <li><a href="#">信息新增</a></li>
        </ul>
    </div>
    <!--当前位置 end-->
        <div class="div_content">
            <table class="tb">
                <tbody>
                    <tr>
                        <td width="120">
                            <span><i class="required">*</i>信息标题</span>
                        </td>
                        <td>
                            <input type="text" id="txtNewsTitle" runat="server" class="textBox" maxlength="50"
                                style="width: 500px;" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span><i class="required">*</i>信息类别</span>
                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <input type="radio" id="RdType1" name="rdotype" runat="server" value="1" checked="true" />&nbsp;&nbsp;新闻&nbsp;&nbsp;&nbsp;&nbsp;
                            <input type="radio" id="RdType2" name="rdotype" runat="server" value="2" />&nbsp;&nbsp;通知&nbsp;&nbsp;&nbsp;&nbsp;
                            <input type="radio" id="RdType3" name="rdotype" runat="server" value="3" />&nbsp;&nbsp;公告&nbsp;&nbsp;&nbsp;&nbsp;
                            <input type="radio" id="RdType4" name="rdotype" runat="server" value="4" />&nbsp;&nbsp;促销&nbsp;&nbsp;&nbsp;&nbsp;
                            <input type="radio" id="RdType5" name="rdotype" runat="server" value="5" />&nbsp;&nbsp;企业动态
                        </td>
                    </tr>
                    <tr style="display:;">
                        <td>
                            <span><i class="required">*</i>是否置顶</span>
                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <input type="radio" id="RdTop3" name="rdoindex" runat="server" value="1"  checked="true" />&nbsp;&nbsp;是 
                            &nbsp;&nbsp;&nbsp;&nbsp;<input type="radio" id="RdTop4" name="rdoindex" runat="server" value="0" />&nbsp;&nbsp;否
                            <i class="grayTxt">（最多置顶三条信息）</i>
                        </td>
                    </tr>
                    <tr>
                     <td>
                            <span><i class="required">*</i>同时店铺发布</span>
                        </td>
                        <td>
                             &nbsp;&nbsp;&nbsp;&nbsp;
                            <input type="radio" id="RdIsbled1" name="rdoiIsbled" runat="server" value="1" checked="true" />&nbsp;&nbsp;是 
                            &nbsp;&nbsp;&nbsp;&nbsp;<input type="radio" id="RdIsbled2" name="rdoiIsbled" runat="server" value="0" />&nbsp;&nbsp;否
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span><i class="required">*</i>显示方式</span>
                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:CheckBox ID="CkShowType1" runat="server"    /> New
                             &nbsp; &nbsp;<asp:CheckBox ID="CkShowType2" runat="server"  /> 标红
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>信息内容</span>
                        </td>
                        <td class="newspan">
                            <asp:TextBox ID="content7" runat="server" TextMode="MultiLine" Height="270px" Width="800px"
                                class="textBox"></asp:TextBox>
                            <script>
                                KindEditor.ready(function (K) {
                                    window.editor = K.create('#content7', {
                                        uploadJson: '../../Kindeditor/asp.net/upload_json.ashx',
                                        fileManagerJson: '../../Kindeditor/asp.net/file_manager_json.ashx',
                                        allowFileManager: true
                                    });
                                });
                            </script>
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
