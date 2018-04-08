<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompServiceInfo.aspx.cs" Inherits="Company_SysManager_CompServiceInfo" %>
<%@ Register TagPrefix="uc1" TagName="IndusTry" Src="~/Admin/UserControl/Tree_Industry.ascx" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>企业信息详情</title>
    <link href="../../Company/css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery.idTabs.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            $("li#libtnEdit").on("click", function () {
                location.href = 'CompServiceEdit.aspx';
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
                    <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
                    <li><a href="../SysManager/CompServiceEdit.aspx" runat="server" id="a1">企业信息维护</a></li><li>></li>
                    <li><a href="#" runat="server" id="Atitle">企业信息详情</a></li>
	            </ul>
            </div>
                <div class="tools">
                  <ul class="toolbar left">
                   <li id="libtnEdit" runat="server"><span><img src="../../Company/images/t02.png" /></span>编辑</li>       
                  </ul>               
                 </div>
             <div class="div_content">
       

                     <table  id="tab1" class="tb">
                       <tbody>
                          <tr>
                          <td style=" width:15%;"><span>厂商名称</span> </td>
                          <td style=" width:25%;"><label runat="server" id="lblCompName"></label></td>
                          <td style=" width:15%;"><span>行业类别</span> </td>
                          <td style=" width:25%;"><uc1:IndusTry runat="server"  UType="label" Id="lblIndusName" />  </td>
                         </tr>
                         <tr>
                          <td><span>企业简称</span></td>
                          <td><label runat="server" id="lblShortName"></label> </td>
                          <td><span>是否允许代理商加盟</span></td>
                          <td><label runat="server" id="lblIsHot"></label></td>
                         </tr>
                         <tr>
                          <td><span>企业编号</span></td>
                          <td><label runat="server" id="lblCompCode"></label></td>
                          <td ><span>法人</span> </td>
                          <td ><label runat="server" id="lblLegal"></label> </td>
                         </tr>
                         <tr>                         
                          <td ><span>法人身份证</span> </td>
                          <td > <label runat="server" id="lblIdentitys"></label>  </td>
                          <td ><span>法人手机</span> </td>
                          <td ><label runat="server" id="lblLegalTel"></label></td>
                         </tr>
                         <tr>                          
                          <td ><span>营业执照号码</span> </td>
                          <td ><label runat="server" id="lblLicence"></label></td>
                          <td><span>组织机构代码证号码</span> </td>
                         <td ><label runat="server" id="lblOrCode"></label></td>
                         </tr>
                         <tr>
                         <td><span>税务登记证号码</span> </td>
                         <td ><label runat="server" id="lblAccount"></label></td>
                          <td><span>联系人</span> </td>
                          <td><label runat="server" id="lblPrincipal"></label> </td>
                         </tr>
                         <tr>
                         <td ><span>固定电话</span> </td>
                          <td ><label runat="server" id="lblTel"></label></td>
                          <td ><span>联系人手机</span> </td>
                          <td > <label runat="server" id="lblPhone"></label>  </td>                         
                        </tr>
                          <tr>
                          <td><span>传真</span> </td>
                          <td><label runat="server" id="lblFax"></label> </td>
                          <td ><span>邮箱</span> </td>
                          <td ><label runat="server" id="lblZip"></label> </td>
                         </tr>

                         <%--<tr>
                          <td><span>融资账户号</span> </td>
                          <td><label runat="server" id="lblFinanceCode"></label> </td>
                          <td ><span>融资账户名称</span> </td>
                          <td ><label runat="server" id="lblFinanceName"></label> </td>
                         </tr>--%>

                         <tr>
                         <td><span>下载查看附件</span> </td>
                         <td>
                         <asp:Panel runat="server" id="DFile" style=" margin-left:5px;"></asp:Panel>
                          </td>
                          <td><span>店主QQ</span></td>
                          <td><label runat="server" id="QQ"></label></td>
                        </tr>
                        <tr>
                           <td><span>主要经营范围</span> </td>
                          <td colspan="3"> <label runat="server" id="lblInfo"></label>  </td>
                        </tr>
                          <tr>
                          <td><span>详细地址</span> </td>
                          <td colspan="3"> <label runat="server" id="lblAddress"></label>   </td>
                         </tr>
                          <tr>
                          <td><span>管理员登录帐号</span></td >
                          <td><label runat="server" id="lblUsername"></label></td>
                          <td><span>管理员手机号</span></td>
                          <td><label runat="server" id="lblUserPhone"></label></td>
                         </tr>    
                         <tr>
                          <td><span>管理员姓名</span></td >
                          <td colspan="3"><label runat="server" id="lblUserTrueName"></label></td>
                         </tr>                     
                       </tbody>
                      </table>                                               
               </div>
             </div>

    </form>
</body>
</html>