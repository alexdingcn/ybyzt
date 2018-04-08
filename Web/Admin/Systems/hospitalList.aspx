<%@ Page Language="C#" AutoEventWireup="true" CodeFile="hospitalList.aspx.cs" Inherits="Admin_Systems_PaybankList" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="SelectComp" Src="~/Admin/UserControl/TextCompList.ascx" %>
<%@ Register Src="~/Company/UserControl/CompRemove.ascx" TagPrefix="uc1" TagName="CompRemove" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>医院档案</title>

    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../Company/js/Pay.js" type="text/javascript"></script>

    <script type="text/javascript">
        //回车事件
        $(function () {
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("#btnSearch").trigger("click");
                }
            })
        })
        $(document).ready(function () {
            //table 行样式
            $('.tablelist tbody tr:odd').addClass('odd');

            //新增按钮事件
            $("#btnAdd").click(function () {
                window.location.href = 'hospitalEdit.aspx';
            });

            // 搜索
            $("#Search").on("click", function () {
                if ($("#txtPager").val() == "" || $("#txtPager").val() == 0) {
                    alert("- 每页显示数量不能为空");
                    return false;
                }
                else {
                    $("#btnSearch").trigger("click");
                }
            });


            
        });

        //详细页面
        function goInfo(Id) {
            window.location.href = 'hospitalInfo.aspx?hid=' + Id;
        }
                     
        //只能输入数字验证
        function KeyInt(val) {
            val.value = val.value.replace(/[^\d]/g, '');
        }        
    </script>
    <style type="text/css">
      
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:CompRemove runat="server" ID="Remove" ModuleID="4" />
        <!--当前位置 start-->
    <uc1:Header ID="Header1" runat="server" />
    <uc2:Left ID="Left1" runat="server" />
    <div class="m-con">
    <div class="m-place">
	        <i>位置：</i>
            <a href="../index.aspx" target="_top">我的桌面</a><i>></i>
            <a href="#" runat="server" id="atitle">系统管理</a><i>></i>
            <a href="#" runat="server" id="btitle">医院档案</a>
        </div>
        <!--当前位置 end--> 
        <!--代理商搜索 Begin-->
        <input id="hid_Alert" type="hidden" />
        <asp:Button ID="btnSearch" Text="搜索" runat="server" onclick="btnSearch_Click" style=" display:none"  />
        <!--代理商搜索 End-->
            <!--功能按钮 start-->
            <div class="tools">
             <ul class="toolbar left">
                    <li class="click" id="btnAdd"><span><img src="../../Company/images/t01.png" /></span><font color="red">新增</font></li>
                </ul>
            <div class="right">
                    <ul class="toolbar right">
                        <li id="Search"><span><img src="../../Company/images/t04.png" alt="" /></span>搜索</li>
                    </ul>
                    <ul class="toolbar3">
                        <li>医院名称:
                         <input name="txtHospitalName" type="text" id="txtHospitalName" runat="server" class="textBox" />                        
                        </li>
                        <li>
                            每页<input type="text" onkeyup='KeyInt(this)' id="txtPager" name="txtPager" runat="server" class="textBox" style=" width:40px;" />行
                        </li>
                    </ul>
                </div>
            </div>
            <!--功能按钮 end-->

            <!--信息列表 start-->
            <table class="tablelist" id="TbList">
            <asp:Repeater runat="server" ID="rptPAcount" >
            <HeaderTemplate>
                <thead>
                    <tr>
                        <th>医院编码</th>
                        <th>医院全称</th>
                        <th>医院级别</th>
                        <th>创建日期</th>
                    </tr>
                </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                    <tr id='tr_<%# Eval("ID") %>' >
                        <td><a href="hospitalInfo.aspx?hid=<%#Eval("ID") %>"><%# Eval("HospitalCode")%></a></td>
                        <td><a href="hospitalInfo.aspx?hid=<%#Eval("ID") %>"><%# Eval("HospitalName")%></a></td>
                        <td><%# Eval("HospitalLevel")%></td>
                        <td><%#Convert.ToDateTime( Eval("CreateDate")).ToString("yyyy-MM-dd")%></td>
                       
                    </tr>
               
                </ItemTemplate>
             </asp:Repeater>
            </table>
            <!--信息列表 end-->

            <!--列表分页 start--> 
            <div class="pagin">
                           <webdiyer:AspNetPager ID="Pager" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                     NextPageText=">"  PageSize="12" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
          ShowPageIndexBox="Always" TextAfterPageIndexBox="<span class='pagiPage'>页</span>"
                       TextBeforePageIndexBox="<span class='jump'>跳转到:</span>"
                   CustomInfoSectionWidth="20%"  CustomInfoStyle="padding:5px 0 0 20px;cursor: default;color: #737373;"  CustomInfoClass="message" ShowCustomInfoSection="Left"
                  CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                     CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5"
                    OnPageChanged="Pager_PageChanged" >
                </webdiyer:AspNetPager>
            </div>
            <!--列表分页 end--> 
        </div>
       

    </form>
</body>
</html>
