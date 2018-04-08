<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MessAgeInfo.aspx.cs" Inherits="Distributor_MessAgeInfo" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>咨询详细</title>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Company/js/jquery-1.11.1.min.js"></script>
</head>
<body class="root3">
    <form id="form1" runat="server">
    <Head:Head ID="Head2" runat="server" />
    <div class="w1200">
    <Left:Left ID="Left1" runat="server" ShowID="nav-6" />
    <div class="rightCon">
    <div class="info"> <a id="navigation1" href="UserIndex.aspx">我的桌面</a>>
                <a id="navigation2" href="MessAgeList.aspx" class="cur">我要咨询</a>>
                <a id="navigation3" href="#" class="cur">咨询详细</a></div>
        <!--功能条件 start-->
        <div class="userFun">
            <div class="left">
                <a href="MessAgeList.aspx" class="btnBl"><i class="returnIcon"></i>返回</a></div>
        </div>
        <!--功能条件 end-->
        <div class="blank10">
        </div>
        <div class="userTrend">
            <div class="uTitle">
            <b>咨询</b></div>
            <div class="ModifyData">
                <h3 class="title"><%=suggest.Title %><i class="time">咨询时间：<%=suggest.CreateDate %></i></h3>
                <div class="txt">
                    <p><%=suggest.Remark %></p>
                </div>
            </div>
            <div id="reply" runat="server" class="ModifyData">
                <h3 class="title">回复人:<%=compuser==null?"":compuser.TrueName %><i class="time">回复时间：<%=suggest.ReplyDate %></i></h3>
                <div class="txt">
                    <p><%=suggest.CompRemark %></p>
                    
                </div>
            </div>
            <div class="blank10">
            </div>
        </div>
    </div>
    </div>
    </form>
</body>
</html>
