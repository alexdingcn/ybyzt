<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="Admin_login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title><%= ConfigurationManager.AppSettings["TitleName"].ToString() %>-欢迎进入总后台管理登录页面</title>
    <link href="css/login.css" rel="stylesheet" type="text/css" />
    <script src="../js/CommonSha.js"  type="text/javascript"></script>
    <script src="../Company/js/jquery.js" type="text/javascript"></script>
    <script type="text/javascript">

        //JS监听整个页面的回车事件
        document.onkeydown = keyDownSearch;
        function keyDownSearch(e) {
            // 兼容FF和IE和Opera    
            var theEvent = e || window.event;
            var code = theEvent.keyCode || theEvent.which || theEvent.charCode;
            if (code == 13) {
                $("#<%=submit.ClientID %>").trigger("click"); //具体处理函数    
                return false;
            }
            return true;
        }  

        function SubmitCheck(obj) {
            var username = document.getElementById("txtLoginId").value;
            var userpwd = document.getElementById("txtPwd").value;
            //var yanzhengcode = document.getElementById("txtCode").value;
            var usercode = document.getElementById("txtcode").value;
            if (username.toString() == "" || username.toString() == null) {
                alert("请输入用户名！");
                return false;
            }
            else if (userpwd.toString() == "" || userpwd.toString() == null) {
                alert("请输入密码！");
                return false;
            }
            if (usercode.toString() == "" || usercode.toString() == null) {
                alert("请输入验证码！");
                return false;
            }
            //return true;
            $("#txtPwd").val(hex_one(hex_one(hex_two(userpwd))));

            //$("#txtPwd").val(hex_two(userpwd));
            window.location.href = $(obj).attr("href");
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="login">
	    <div class="title"></div>
        <ul class="list">
    	    <li><label><i>用户名</i></label><input type="text" class="box" runat="server" id="txtLoginId" name="txtLoginId" maxlength="50" style="color: #464646"/></li>
            <li><label><i>密&nbsp;&nbsp;&nbsp;码</i></label><input type="password" class="box" runat="server" id="txtPwd" name="txtPwd" style="color: #464646"/></li>
            <li>
                <label style="float:left;width:78px;"><i>验证码</i></label>
                <div style="float:left;width:242px;height:38px;">
                    <input type="text" class="box" runat="server" id="txtcode" name="txtcode" style="color: #464646;width:142px;float:left;"/>
                    <img id="ckcode" style="cursor: pointer;float:left;" alt="看不清？点击更换" onclick="this.src='/UserControl/CheckCode.aspx?t='+new Date().getTime()"
                        src="../UserControl/CheckCode.aspx" width="80" title="看不清?点击更换" height="39" />
                </div>
            </li>
        </ul>
        <div class="btn"><a id="submit" runat="server" href="javascript:;" onclick="return SubmitCheck(this);" onserverclick="btnLogin_Click">登 录</a></div>
        <br />
        <div  align="center"><a href="<%= ConfigurationManager.AppSettings["WebDomainName"].ToString() %>" align="center">>>返回首页</a></div>
    </div>
    <img src="../Company/images/loginBg.jpg" width="100%" class="loginBg"/>
    </form>

    <script type="text/javascript">
        document.getElementById("txtLoginId").focus();
    </script>
</body>
</html>
