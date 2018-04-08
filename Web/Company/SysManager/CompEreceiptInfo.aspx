<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompEreceiptInfo.aspx.cs" Inherits="Company_SysManager_CompEreceiptInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>仓单详情</title>
    <link href="../../Company/css/style.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery.idTabs.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
        <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            $("li#libtnEdit").on("click", function () {
                location.href = 'CompEreceipt.aspx';
            });

            $("#li1").on("click", function () {
                window.parent.CloseDialog();
            });
        })   
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div class="rightinfo" style=" margin-top:0px; margin-left:0px;">
            <div class="tools">
                <ul class="toolbar left">
                    <li id="libtnEdit" runat="server"><span><img src="../../Company/images/t02.png" /></span>编辑</li> 
                    
                     <li id="li1" runat="server" ><span><img src="../images/t03.png" /></span>关闭</li>      
                </ul>               
            </div>
            <div class="div_content">
                <table  id="tab1" class="tb">
                   <tbody>
                       <tr>
                            <td><span>仓库编号</span></td>
                            <td><label id="lblereceipt_whno" runat="server"   ></label></td>

                            <td><span>仓库名称</span></td>
                            <td><label id="lblereceipt_whnm" runat="server"  ></label></td>
                        </tr>

                        <tr>
                            <td><span>批次号</span></td>
                            <td><label id="lblereceipt_batchno" runat="server"    ></label></td>

                            <td><span>生产厂家</span></td>
                            <td><label id="lblereceipt_mfters" runat="server"   ></label></td>
                        </tr>

                        <tr>
                            <td><span>规 格</span></td>
                            <td><label  id="lblereceipt_std" runat="server"   ></label></td>

                            <td><span>失效日期</span></td>
                            <td><label id="lblereceipt_duedate" runat="server"  ></label></td>
                        </tr>
                   </tbody>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
