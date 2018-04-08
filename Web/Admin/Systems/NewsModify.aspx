<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NewsModify.aspx.cs" Inherits="Admin_Systems_NewsModify" validateRequest="false" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>公告</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>
    <link href="../../kindeditor/themes/default/default.css" rel="stylesheet" type="text/css" />
    <link href="../../kindeditor/plugins/code/prettify.css" rel="stylesheet" type="text/css" />
    <script src="../../kindeditor/kindeditor.js" type="text/javascript"></script>
    <script src="../../kindeditor/lang/zh_CN.js" type="text/javascript"></script>
    <script src="../../kindeditor/plugins/code/prettify.js" type="text/javascript"></script>
    <script src="../../js/xss.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            $('.tb tbody tr td:even').addClass('odd');
            $("#txtNewsTitle").keyup(function () {
                Filtration(this)
            })
           
        })

        //过滤非法字符 <
        function Filtration(val)//val=this
        {
            var id = $(val).attr("id");
            var el = $("#" + id + "").get(0);
            var pos = 0;
            if ('selectionStart' in el) {
                pos = el.selectionStart;
            } else if ('selection' in document) {
                el.focus();
                var Sel = document.selection.createRange();
                var SelLength = document.selection.createRange().text.length;
                Sel.moveStart('character', -el.value.length);
                pos = Sel.text.length - SelLength;
            }
            var str = new RegExp("[^<]")//需要被过滤掉的非法字符[]
            var s = $("#" + id + "").val();
            var rs = "";
            for (var i = 0; i < s.length; i++) {
                if (str.test(s.substr(i, 1))) {
                    rs = rs + s.substr(i, 1);
                }
                else {

                }
            }
            if (s != rs) {
                $("#" + id + "").val(rs);
                if (val.setSelectionRange) {
                    val.focus();
                    val.setSelectionRange(pos - 1, pos - 1);
                }
                else if (input.createTextRange) {
                    var range = val.createTextRange();
                    range.collapse(true);
                    range.moveEnd('character', pos - 1);
                    range.moveStart('character', pos - 1);
                    range.select();
                }
            }

        }
        function formCheck() {
            var str = "";
            if ($.trim($("#txtNewsTitle").val()) == "") {
                str = "新闻标题不能为空";
            }

            if (str != "") {
                errMsg("提示", str, "", "");
                return false;
            }
            if ($.trim($("#content7").val())!="") {
                var html = filterXSS($.trim($("#content7").val()));
                $("#content7").val(html);
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
            <a href="#">系统管理</a><i>></i>
            <a href="NewsList.aspx">新闻发布</a>
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
                            <input type="radio" id="Radio1" name="rdotype" runat="server" value="1" checked="true" />&nbsp;&nbsp;新
                            闻&nbsp;&nbsp;&nbsp;&nbsp;
                            <input type="radio" id="Radio2" name="rdotype" runat="server" value="2" />&nbsp;&nbsp;公
                            告&nbsp;&nbsp;&nbsp;&nbsp;
                            <input type="radio" id="Radio5" name="rdotype" runat="server" value="3" />&nbsp;&nbsp;资
                            讯&nbsp;&nbsp;&nbsp;&nbsp;
                            <input type="radio" id="Radio6" name="rdotype" runat="server" value="4" />&nbsp;&nbsp;生意经
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span><i class="required">*</i>是否发布</span>
                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <input type="radio" id="rdoStatus0" name="rdoStatus" runat="server" value="1" checked="true" />&nbsp;&nbsp;是
                            &nbsp;&nbsp;&nbsp;&nbsp;<input type="radio" id="rdoStatus1" name="rdoStatus" runat="server"
                                value="0" />&nbsp;&nbsp;否
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span><i class="required">*</i>是否置顶</span>
                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <input type="radio" id="Radio3" name="rdoindex" runat="server" value="1" checked="true" />&nbsp;&nbsp;是&nbsp;&nbsp;&nbsp;&nbsp;<input
                                type="radio" id="Radio4" name="rdoindex" runat="server" value="0" />&nbsp;&nbsp;否
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>信息内容</span>
                        </td>
                        <td class="newspan">
                            <asp:TextBox ID="content7" runat="server" TextMode="MultiLine" Height="400px" Width="800px"
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
                    <tr>
                        <td width="120">
                            <span>Keywords</span>
                        </td>
                        <td>
                            <input type="text" id="textKeywords" runat="server" class="textBox" maxlength="50"
                                style="width: 500px;" />
                        </td>
                    </tr>
                    <tr>
                        <td width="120">
                            <span>信息摘要</span>
                        </td>
                        <td>
                            <asp:TextBox ID="textNewsInfo" runat="server" TextMode="MultiLine" 
                                style=" border:1px solid #808080;text-indent:5px;margin:5px 5px 5px 5px" 
                                Width="500px" Rows="3"></asp:TextBox>
                            <%--（如信息类别为公告，请填写显示类型：升级、上线、告知 等）--%>
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
