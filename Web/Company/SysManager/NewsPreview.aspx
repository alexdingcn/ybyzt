<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NewsPreview.aspx.cs" Inherits="Company_SysManager_NewsPreview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>信息详情</title>
    <link href="../../Distributor/css/global.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>

    <script>
        $(document).ready(function () {
            $("#lblNewContent").find("a").attr("target", "_blank");
        })
    </script>
</head>
<body class="root3" style=" background:#fff">
    <form id="form1" runat="server">
    <div class="orderNr" style=" padding-top:16px;">
      <table class="orderDet" border="0" cellspacing="0" cellpadding="0" width="100%">
                <tbody>
                    <tr>
                        <td style=" text-align:center;">
                             <label id="lblNewTitle" runat="server" style=" font-size:20px; width:100%; margin:auto; font-weight:bold;"></label><br>
                             <label id="lblCreateDate" runat="server" style=" font-size:17px; margin-top:5px;  float:right;"></label>
                        </td>
                    </tr>
                    <tr>
                        <td style="word-wrap: break-word; padding-left:5px; word-break: break-all;">
                          <label id="lblNewContent" runat="server"></label>
                        </td>
                    </tr>

                </tbody>
            </table>
    </div>
    </form>
</body>
</html>
