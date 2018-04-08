<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectValue.aspx.cs" Inherits="Admin_Systems_SelectValue" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style>
      .textBox{width:500px; height:23px; line-height:23px; border:1px solid #d1d1d1; text-indent:5px;margin-left:5px;}
      .orangeBtn{ padding:0px 20px; height:28px; color:#fff; background:#3f97c9; border:1px solid #287fb1; font-size:14px;border-radius: 3px; cursor:pointer;}
      .orangeBtn{background:#ff4e02;font-weight:normal; border:1px solid #ea5211;}
      .orangeBtn:hover{background:#f14900; border:1px solid #dd4a0a;}

    </style>
</head>
<body>
    <form id="form1" runat="server" >
     
        <span style="font-size:14px;color:#808080;">热搜词:</span> <input runat="server" type="text" class="textBox" autocomplete="off" id="txt_SelectValue"  maxlength="50" />
        <input type="button" runat="server" id="btnSubMit" class="orangeBtn"  value="确定"   onserverclick="btnSubMit_Click" />
        <p style="font-size:13px;color:#ff4e02;"> 每个热词以,分割 (列： 汽车用品,母婴玩具,礼品园艺) 最多六个 </p>
   
    </form>
</body>
</html>
