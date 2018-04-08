<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ERPDataTransfer.aspx.cs" Inherits="Company_SysManager_ERPDataTransfer" %>

<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ERP代理商/产品同步</title>
     <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
   <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs1.js" type="text/javascript"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script>
        function check(name, obj) {
            if (confirm(name)) {
                $(obj).val("数据正在同步请等待同步结果...");
               $("input.orangeBtn").prop("disabled", "disabled").css({ "background": "rgb(129,129,129)", "border": "rgb(129,129,129)" });
            } else {
                return false;
            }
            return true;
        }

        $(document).ready(function () {
            if ($("table.tablelist").length > 0) {
                if ($("table.tablelist1")[0].clientHeight > $("table.tablelist1").parent()[0].clientHeight) {
                    $("table.tablelist1 tr:eq(0)").find("td:eq(0)").attr("width", "15.25%");
                    $("table.tablelist1 tr:eq(0)").find("td:eq(1)").attr("width", "84.75%");
                }
            }
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">

    <uc1:top ID="top1" runat="server" ShowID="nav-4" />

       <div class="rightinfo">
        
  <div class="place">
	        <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li>
                <li><a href="#" runat="server" id="btitle">我要维护</a></li><li>></li>
                <li><a href="#" runat="server" id="ctitle">我的代理商</a></li><li>></li>
                <li><a href="#" runat="server" id="Atitle">ERP代理商/产品同步</a></li>
	        </ul>
     </div>

          <div class="div_content">
                  <table class="tb" >
                   <tbody>
                      <tr>
                      <td width="10%"><span>代理商同步(包括分类、地址)</span> </td>
                      <td width="20%"><input type="button" id="btnDisTransfer" class="orangeBtn" style=" margin-left:20px;" runat="server" value="代理商数据同步" onclick="if(!check('确认同步？',this)){return false}" onserverclick="btnDisTransfer_Click" />&nbsp;</td>
                      <td width="10%"><span>同步结果</span> </td>
                      <td width="60%"><label runat="server" style=" font-size:14px;"  id="lblDisIpResult"></label></td>
                      </tr>
                      <tr>
                      <td><span>商品同步(包括分类、价格)</span> </td>
                      <td><input type="button" id="btnGoodsTransfer" class="orangeBtn" style=" margin-left:20px;" runat="server" value="商品数据同步" onclick="if(!check('确认同步？',this)){return false}" onserverclick="btnGoodsTransfer_Click" />&nbsp;</td>
                      <td><span>同步结果</span> </td>
                      <td><label runat="server" id="lblGoodsIpResult"></label></td>
                      </tr>
                      <tr runat="server" visible="true">
                      <td><span>清除数据</span> </td>
                      <td><input type="button" id="btnDelete" class="orangeBtn" style=" margin-left:20px;" runat="server" value="清除数据" onclick="if(!check('确认删除？',this)){return false}" onserverclick="btnDelete_Click" />&nbsp;</td>
                      <td><span>同步结果</span> </td>
                      <td><label runat="server" id="Label1"></label></td>
                      </tr>
                      </tbody>
                   </table>

              <div runat="server" id="DivError"  visible="false">
                 <div class="div_title" >
                 导入/同步错误信息：<h1 runat="server" id="HERCount" style=" color:Red; display:inline-block;">160</h1>条
                 </div>
                 <table class="tablelist" style="width:100%;">
                    <tr>
                        <th width="15%">导入/同步类型</th>
                        <th width="85%">导入/同步错误原因</th>
                    </tr>
                   </table>
                 <div style=" height:450px; width:100%; overflow-y:visible;overflow-x:hidden;">
                    <table class="tablelist tablelist1">
                       <tbody>
                         <asp:Repeater ID="Rpt_Error" runat="server">
                         <ItemTemplate>
                           <tr>
                         <td width="15%"><%# Eval("Etype")%></td>
                         <td width="85%"><%# Eval("Ename")%></td>
                           </tr>
                         </ItemTemplate>
                         </asp:Repeater>
                      </tbody>
                    </table>
                </div>
            </div>
        </div>
       </div>
    </form>
</body>
</html>
