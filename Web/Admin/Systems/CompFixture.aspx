<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompFixture.aspx.cs" Inherits="Admin_Systems_CompFixture" validateRequest="false" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="IndusTry" Src="~/Admin/UserControl/Tree_Industry.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>厂商查询</title>
        <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
   <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script src="../../Company/js/OpenJs.js" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            $("#btnEdit").on("click", function () {
                location.href = "CompFixEdit.aspx?KeyID=<%=KeyID %>";
            })
            $("#btnBack").on("click", function () {
                location.href = "CompInfo.aspx?KeyID=<%=KeyID %>&type=2";
            })
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
       <uc1:Header ID="Header1" runat="server" />
    <uc2:Left ID="Left1" runat="server" />
    <div class="m-con">
    <div class="m-place">
	        <i>位置：</i>
            <a href="../index.aspx" target="_top">我的桌面</a><i>></i>
            <a href="#">厂商管理</a><i>></i>
            <a href="#" runat="server" id="Atitle">厂商查询</a>
     </div>
      <div class="tools">
          <ul class="toolbar left">
             <input type="button" id="btnSelect"  onclick="javascript:OpenCustomWindow('/<%=KeyID %>.html','e店铺',1000,600)" class="sure"  value="商城预览" />&nbsp;&nbsp;
             <input type="button" id="btnEdit"  class="sure"  value="装修" /> &nbsp;&nbsp;
             <input type="button" id="btnBack"  class="sure"  value="返回" /> 
          </ul>
          </div>

                 <div class="div_content">

                    <table  class="tb">
                       <tbody>
                          <tr>
                          <td><span>联系人</span> </td>
                          <td><label runat="server" id="lblPrincipal"></label> </td>
                          <td ><span>固定电话</span> </td>
                          <td> <label runat="server" id="lblTel"></label>  </td>
                          </tr>

                          <tr>
                          <td ><span>邮编</span> </td>
                          <td ><label runat="server" id="lblZip"></label> </td>
                          <td><span>传真</span> </td>
                          <td><label runat="server" id="lblFax"></label> </td>
                          </tr>
                          <tr>
                          <td ><span>联系人手机</span> </td>
                          <td colspan="3" > <label runat="server" id="lblPhone"></label>  </td>
                          </tr>
                          <tr>
                          <td><span>详细地址</span> </td>
                          <td colspan="3"> <label runat="server" id="lblAddress"></label>   </td>
                          </tr>


                        <%--<tr>
                       <td><span>企业简介</span> </td>
                       <td colspan="3"> <label runat="server" id="lblCompInfo"></label>   </td>
                       </tr>--%>

                      
                        <tr>
                       <td><span>企业LOGO</span> </td>
                       <td colspan="3">  <p><img style=" margin:5px 0 5px 5px;width:185px;height:105px;"  id="ImgCompLogo" runat="server" class="imgWrap"  /></p><i style="color: #999; margin:0 5px;">企业LOGO默认大小：185*105</i></td>
                       </tr>

                        <tr>
                       <td><span>首页显示LOGO</span> </td>
                       <td colspan="3">  <p><img style=" margin:5px 0 5px 5px;width:140px;height:75px;"  id="ImgShopLogo" runat="server" class="imgWrap"  /></p><i style="color: #999; margin:0 5px;">企业LOGO默认大小：140*75</i></td>
                       </tr>

                       <tr style=" display:none;">
                       <td width="15%"><span>首页显示（New）LOGO</span> </td>
                       <td colspan="3">  <p><img style=" margin:5px 0 5px 5px;width:140px;height:75px;"  id="ImgNewShopLogo" runat="server" class="imgWrap"  /></p><i style="color: #999; margin:0 5px;">企业LOGO默认大小：140*75</i></td>
                       </tr>

                       <tr>
                       <td><span>企业详细介绍</span> </td>
                       <td colspan="3"><%=contents%></td>
                       </tr>

                       </tbody>
                      </table>


                 </div>

                    <div  class="div_footer">
                
               </div>


            </div>


    </form>
</body>
</html>
