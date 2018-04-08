<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectCompList.aspx.cs" Inherits="Admin_UserControl_SelectCompList" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            
            $(".tablelist tbody tr").on("click", function () {
                var id = $(this).find("a#A_CompSelect").attr("compid");
                var name = $(this).find("a#A_CompSelect").siblings("#Hid_CompName").val();
                window.parent.selectComp(id, name);
            });

            $("#close").on("click", function () {
                window.parent.CloseDialogTo();
            })

            $("#inpqk").on("click", function () {
                var id = "";
                var name = "";
                window.parent.selectComp(id, name);
            })

            $(document).ready(function () {
                $('.tablelist tbody tr:odd').addClass('odd');
                $("li#liSearch").on("click", function () {
                    $("#btn_Search").trigger("click");
                })

            })

        })
    </script>
</head>
<body style=" min-width:800px;">
    <form id="form1" runat="server">
      <div class="rightinfo">
        <div class="tools">
        <input name="" type="button" class="sure" id="inpqk" value="清空" />&nbsp;&nbsp;
        <input name="" type="button" class="sure" id="close" value="关闭" />
           <div class="right">
              <ul class="toolbar right">
                <li id="liSearch"><span><img src="../../Company/images/t04.png" /></span>搜索</li>
              </ul>
                <input type="button" runat="server" id="btn_Search" style="display:none;" onserverclick="btn_SearchClick" /> 
                <ul class="toolbar3">
                  <li>厂商名称:<input  runat="server" id="txtCompName" type="text" class="textBox"/>&nbsp;&nbsp;</li>
                  <li>联系人:<input  runat="server" id="txtPerson" type="text" class="textBox"/>&nbsp;&nbsp;</li>
                  <li>联系电话:<input  runat="server" id="txtPerPhone" type="text" class="textBox"/>&nbsp;&nbsp;</li>
                </ul>
            </div>
        </div>

                   <table class="tablelist">
                <thead>
                    <tr>
                        <th>厂商名称</th>
                        <th>联系人</th>
                        <th>联系电话</th>
                        <th>行业</th>
                        <th>经营范围</th>
                    </tr>
                </thead>
                <tbody>
                  <asp:Repeater ID="Rpt_Comp" runat="server">
                   <ItemTemplate>
                         <td style=" cursor:pointer;"><%# Eval("CompName")%> <a id="A_CompSelect" class="choiceBtn" style=" cursor:pointer; display:none; text-decoration:underline;" compid="<%#Eval("ID")%>"></a> <input type="hidden" value="<%#Eval("CompName")%>" id="Hid_CompName" /></td>
                         <td style=" cursor:pointer;" ><%# Eval("Principal")%></td>
                         <td style=" cursor:pointer;" ><%# Eval("Phone")%></td>
                         <td style=" cursor:pointer;"> <%# Eval("Trade")%></td>
                         <td style=" cursor:pointer;"><%# Common.MySubstring(Convert.ToString(Eval("ManageInfo")), 34, "...")%></td>
                    </tr>
                    </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>

                <div class="pagin" style=" height:30px;">
                          <webdiyer:AspNetPager ID="Pager" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                     NextPageText=">"  PageSize="12" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
             ShowPageIndexBox="Always" TextAfterPageIndexBox="<span class='pagiPage'>页</span>"
                       TextBeforePageIndexBox="<span class='jump'>跳转到:</span>"
                                    CustomInfoSectionWidth="30%"  CustomInfoStyle="padding:5px 0 0 20px;cursor: default;color: #737373;"  CustomInfoClass="message" ShowCustomInfoSection="Left"
                  CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                     CssClass="paginList" CurrentPageButtonClass="paginItem"
                    OnPageChanged="Pager_PageChanged" >
                </webdiyer:AspNetPager>
            </div>

              

      </div>
    </form>
</body>
</html>
