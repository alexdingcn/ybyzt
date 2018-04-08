<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompInfo.aspx.cs" Inherits="Company_SysManager_CompInfo" %>
<%@ Register TagPrefix="uc1" TagName="IndusTry" Src="~/Admin/UserControl/Tree_Industry.ascx" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>商城详情</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../js/OpenJs.js" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            $("#btnEdit").on("click", function () {
                location.href = "CompEdit.aspx?KeyID=<%=CompID %>";
            })
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-6" />
        <div class="rightinfo">
            <div class="place">
	            <ul class="placeul">
                    <li><a href="../jsc.aspx">我的桌面</a></li><li>></li>
                    <li><a href="../SysManager/CompEdit.aspx?back=0" runat="server" id="btitle">店铺信息</a></li><li>></li>
                    <li><a href="#" runat="server" id="Atitle">商城详情</a></li>
	            </ul>
         </div>
                  <div class="tools">
          <ul class="toolbar left">
             <input type="button" id="btnSelect"  onclick="javascript:OpenCustomWindow('/<%=CompID %>.html','e店铺',1000,600)" class="sure"  value="商城预览" />&nbsp;&nbsp;
             <input type="button" id="btnEdit"  class="sure"  value="装修" />&nbsp;&nbsp;
          </ul>
          </div>

                 <div class="div_content">

                    <table  class="tb">
                       <tbody>

                          <tr>
                            <td><span>联系人</span> </td>
                          <td><label runat="server" id="lblPrincipal"></label> </td>
                          <td ><span>固定电话</span> </td>
                          <td > <label runat="server" id="lblTel"></label>  </td>
                         </tr>

                         <tr>
                         <td ><span>联系人</span> </td>
                         <td>
                          <label runat="server" id="lblPhone"></label>
                         </td>
                         <td><span>传真</span> </td>
                          <td><label runat="server" id="lblFax"></label> </td>
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
                       <td colspan="3">  <p><img style=" margin:5px 0 5px 5px;width:185px;height:105px;" src="" id="ImgCompLogo" runat="server" class="imgWrap"  /></p><i style="color: #999; margin:0 5px;">企业LOGO默认大小：185*105</i></td>
                       </tr>
                            <tr>
                       <td><span>店铺搜索关键字</span> </td>
                       <td colspan="3"> <label runat="server" id="BrandInfo"></label>   </td>
                       </tr>
                        <tr>
                       <td><span>企业详细介绍</span> </td>
                       <td colspan="3"> <div runat="server" style=" margin-left:5px; width:600px;" id="div_CustomInfo"></div>  </td>
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
