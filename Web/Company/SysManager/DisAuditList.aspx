<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DisAuditList.aspx.cs" Inherits="Company_SysManager_DisAuditList" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/TreeDisArea.ascx" TagPrefix="uc1" TagName="TreeDisArea" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>代理商审核</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script>
        $(function () {
            $(".showDiv2 .ifrClass").css("width", "155px");
            $(".showDiv2").css("width", "150px");

            $('iframe').load(function () {
                $('iframe').contents().find('.pullDown').css("width", "150px");
            });
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("#btn_Search").trigger("click");
                }
            })
        })
        $(document).ready(function () {
            var div = $('<div   Msg="True"  style="z-index:111110; background-color:#000; opacity:0.5; filter:alpha(opacity=50);position: fixed;width:100%;height:100%;top:0px;left:0px;"></div>');
            //table 行样式
            $('.tablelist tbody tr:odd').addClass('odd');

//            $("li.liDisImport").on("click", function () {
//                $("body").append(div);
//                $("div.tip").css("width", "500px").fadeIn(200); 
//            })

//            $("input.cancel,.tiptop a").on("click", function () {
//                $(div).remove();
//                $("div.tip").fadeOut(200);
//            })

            $("li#liSearch").on("click", function () {
                $("#btn_Search").trigger("click");
            })

        })
        function formCheck(obj) {
            var str = $("#FileUpload1").val();
            if (str == "") {
                layerCommon.msg("请选择要导入代理商Excel的文件", IconOption.错误);
                return false;
            }
            var suffix = $.trim(str.substring(str.lastIndexOf(".")));
            if (suffix == ".xlsx" || suffix == ".xls") {
                $(obj).attr("disabled", "disabled");
                return true;
            } else {
                layerCommon.msg("请选择Excel文件", IconOption.错误);
                return false;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <uc1:top ID="top1" runat="server" ShowID="nav-4" />
        <div class="rightinfo">
            <!--当前位置 start-->
            <div class="place">
	            <ul class="placeul">
                    <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
                    <li><a href="../SysManager/DisAuditList.aspx">代理商审核</a></li>
	            </ul>
            </div>
            <!--当前位置 end--> 
            <!--功能按钮 start--><div class="tools">
                <div class="right">
                    <ul class="toolbar right">
                        <li id="liSearch"><span><img src="../images/t04.png" /></span>搜索</li>
                        <%--<uc1:ToExcel runat="server" ID="ToExcel" contect="TbList" />--%>
                        <li class="liSenior"><span><img src="../images/t07.png" /></span>高级</li>
                    </ul>
                    <input type="button" runat="server" id="btn_Search" style="display:none;" onserverclick="btn_SearchClick" /> 
                    <ul class="toolbar3">
                        <li>代理商名称:<input  runat="server" id="txtDisName" type="text" class="textBox"/></li>
                        <li>联系人:<input  runat="server" id="txtPrincipal" type="text" class="textBox"/>&nbsp;&nbsp;</li>
                        <li>联系手机:<input  runat="server" id="txtPhone" type="text" class="textBox"/>&nbsp;&nbsp;</li>
                    </ul>
                </div>
            </div>
            <!--功能按钮 end-->
                <div class="hidden">
               <ul>
                      <li>每页<input name="txtPageSize" type="text" style=" width:40px;" class="textBox txtPageSize" id="txtPageSize"
                       runat="server" value="12" onkeyup="KeyInt(this);" onblur="KeyInt(this);" /> &nbsp;条&nbsp;&nbsp;</li>
                       <%--<li>区域:<uc1:TreeDisArea runat="server" Id="BoxArea" />&nbsp;&nbsp;</li>
                       <li>等级:<asp:DropDownList runat="server" CssClass="textBox"  ID="ddlDisLevel"></asp:DropDownList>&nbsp;&nbsp;</li>--%>
               </ul>
             </div>

            <!--信息列表 start-->
            <table class="tablelist" id="TbList">
                <thead>
                    <tr>
                        <th>代理商名称</th>
                        <th class="t2">联系人</th>
                        <th class="t2">联系手机</th>
                        <th class="t2">审核状态</th>
                        <th class="t2">申请时间</th>
                    </tr>
                </thead>
                <tbody>
                  <asp:Repeater ID="Rpt_Distribute" runat="server">
                   <ItemTemplate>
                    <tr>
                         <td><div class="tcle"><a style=" text-decoration:underline; " href='DisInfo.aspx?KeyID=<%#Common.DesEncrypt(Eval("Id").ToString(), Common.EncryptKey)%>&type=0' ><%# Eval("DisName") %></a></div></td>
                         <td><div class="tc"> <%# Eval("Principal")%></div></td>
                         <td><div class="tc"> <%# Eval("phone")%></div></td>
                          <td><div class="tc"> <%# Eval("AuditState").ToString() == "2" ? "已审" : "<span style='color:red'>未审</span>"%></div></td>
                         <td><div class="tc"> <%# Common.GetDateTime(Eval("CreateDate").ToString().ToDateTime(),"yyyy-MM-dd")%></div></td>
                    </tr>
                    </ItemTemplate>
                    </asp:Repeater>
                </tbody>
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
