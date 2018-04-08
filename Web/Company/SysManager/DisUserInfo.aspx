<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DisUserInfo.aspx.cs" Inherits="Company_SysManager_DisUserInfo" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>代理商管理员详情</title>
         <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
        <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
       <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            $("li#libtnDel").on("click", function () {
               layerCommon.confirm("确认删除？", function () { $("#btnDel").trigger("click"); }, "提示");
            })

//            $("li#libtnEdit").on("click", function () {
//                window.location.href = "DisUserEdit.aspx?KeyID=<%= KeyID%>";
//            })

            $("li#liEnabled").on("click", function () {
                $("#btnEnabled").trigger("click");
            })

            $("li#liNEnabled").on("click", function () {
                $("#btnNEnabled").trigger("click");
            })
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-4" />
         <div class="rightinfo"><div class="place">
	        <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面</a></li><li>></li>
                <li><a href="../SysManager/DisUserList.aspx">代理商管理员查询</a></li><li>></li>
                <li><a href="#">代理商管理员详情</a></li>
	        </ul>
     </div>

   
      <div class="tools">
          <ul class="toolbar left">
<%--              <li id="libtnEdit"><span><img src="../images/t02.png" /></span>编辑</li>--%>
              <li id="liEnabled" runat="server"><span><img src="../images/Nlock.png" /></span><label>启用</label></li>
              <li id="liNEnabled" runat="server"><span><img src="../images/lock.png" /></span><label>禁用</label></li>
    <%--          <li id="libtnDel"><span><img src="../images/t03.png" /></span>删除</li>--%>
               <li id="lblbtnback" onclick="javascript:window.location.href='DisUserList.aspx';"><span><img src="../images/tp3.png" /></span>返回</li>
          </ul>
          <input type="button" runat="server" id="btnEnabled" onserverclick="btn_Enabled"  style="display:none;"  />
          <input type="button" runat="server" id="btnNEnabled" onserverclick="btn_NEnabled"  style="display:none;"  />
     <%--     <input type="button" runat="server" id="btnDel" onserverclick="btn_Del" style="display:none;"  />--%>
    </div>

        <div class="div_content">
        <table class="tb" >
        <tbody>
            <tr>
            <td width="140px"><span>代理商名称</span> </td>
            <td><label runat="server" id="lblDisName"></label></td>
            <td  width="140px"><span>登录帐号</span> </td>
            <td><label runat="server" id="lblUname"></label></td>
            </tr>

            <tr>
            <td><span>姓名</span> </td>
            <td><label runat="server" id="lblTrueName"></label></td>
            <td><span>微信帐号</span> </td>
            <td><label runat="server" id="lblOpenID"></label></td>
            </tr>

            <tr>
            <td><span>性别</span> </td>
            <td><label runat="server" id="lblSex"></label></td>
            <td><span>手机号码</span> </td>
            <td><label runat="server" id="lblPhone"></label></td>
            </tr>

            <tr>
            <td><span>电话</span> </td>
            <td><label runat="server" id="lblTel"></label></td>
            <td><span>身份证</span> </td>
            <td><label runat="server" id="lblIdentitys"></label></td>
            </tr>

            <tr>
            <td><span>是否启用</span> </td>
            <td><label runat="server" id="lblIsEnabled"></label></td>
            <td><span>邮箱</span> </td>
            <td><label runat="server" id="lblEmail"></label>&nbsp;</td>
            </tr>

        </tbody>
        </table>
    </div>

    </div>
    </form>
</body>
</html>
