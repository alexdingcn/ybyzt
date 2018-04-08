<%@ Page Language="C#" AutoEventWireup="true" CodeFile="main.aspx.cs" Inherits="Company_main" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title><%= ConfigurationManager.AppSettings["TitleName"].ToString() %>-企业管理后台</title>
</head>
<frameset rows="60,*" cols="*" frameborder="no" border="0" framespacing="0">
  <frame src="top.aspx" name="topFrame" scrolling="No" noresize="noresize" id="topFrame" title="topFrame" />
  <frameset cols="148,*" frameborder="no" border="0" framespacing="0">
    <frame src="left.aspx?type=<%= Request.QueryString["type"] %>" name="leftFrame" scrolling="No" noresize="noresize" id="leftFrame" title="leftFrame" />
     <frame  name="rightFrame" id="rightFrame" runat="server" title="rightFrame" />
  </frameset>
</frameset>
<noframes>
    <body>
    </body>
</noframes>
</html>
