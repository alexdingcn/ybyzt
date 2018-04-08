<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="Company_index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title><%= ConfigurationManager.AppSettings["TitleName"].ToString() %>-企业管理后台</title>
<link href="css/style.css" rel="stylesheet" type="text/css" />
<script src="js/jquery-1.11.1.min.js" type="text/javascript"></script>
<script src="../js/CommonJs.js" type="text/javascript"></script>
<script>
    $(document).ready(function () {
        //add by hgh 
        $(window.parent.leftFrame.document).find('.menuson li.active').removeClass('active');
        $(window.parent.leftFrame.document).find('.menuson').css('display', 'none');
    })
</script>
</head>

<body>
<div class="place"><span>位置：</span><ul class="placeul"><li><a href="jsc.aspx">我的桌面</a></li></ul></div>
    
<div class="mainindex">
 <img src="images/index.jpg" width="100%" height="100%" />   
   
</div>
     
</body>
</html>
