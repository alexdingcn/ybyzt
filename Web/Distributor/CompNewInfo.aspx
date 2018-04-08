<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompNewInfo.aspx.cs" Inherits="Distributor_CompNewInfo" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>公告信息详情</title>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link href="../css/layer.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Company/js/jquery-1.11.1.min.js"></script>
    <script type="text/javascript" src="../js/CommonJs.js"></script>
    <script src="../js/layer.js" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            $("#lblNewContent").find("a").attr("target", "_blank");
        })
    </script>
</head>
<body class="root3">
    <form id="form1" runat="server">
      <Head:Head ID="Head1" runat="server" />
       <div class="w1200">
         <Left:Left ID="Left1" runat="server" ShowID="nav-6" />
           <div class="rightCon">
            <div class="info"> 
                <a id="navigation1" href="UserIndex.aspx">我的桌面</a>>
                <a id="navigation2" href="CompNewList.aspx" class="cur">公告信息</a>>
                <a id="navigation3" href="#" class="cur"></a>公告信息详情</div>
            <div class="userFun">
            <div id="A_btn" runat="server" class="left">
                <a href="#" class="btnBl" onclick="location.href='CompNewList.aspx'" id="returnIcon" runat="server">
                <i class="returnIcon"></i>返回</a>
              </div>
              </div>
               <!--功能条件 end-->
           <div class="blank10">
           </div>
               <div class="orderNr">
            <table class="orderDet" border="0" cellspacing="0" cellpadding="0" width="100%">
                <tbody>
                    <tr>
                        <td  style=" text-align:center;">
                             <label id="lblNewTitle" style=" font-size:20px; width:100%; margin:auto; font-weight:bold;" runat="server">
                            </label></br>
                             <label id="lblCreateDate" style=" font-size:17px; margin-top:5px;  float:right;" runat="server">
                            </label>
                        </td>
                    </tr>
                       <tr>
                        <td style="word-wrap: break-word; padding-left:5px; word-break: break-all;">
                          <label id="lblNewContent" runat="server">
                            </label>
                        </td>
                    </tr>

                </tbody>
            </table>

             </div>
           </div>
       </div>
    </form>
</body>
</html>
