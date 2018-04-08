<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompFCmateriarialsList.aspx.cs" Inherits="Distributor_FCmaterials_CompFCmateriarialsList" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>厂商首营资料</title>
    <%--<link href="../../Company/css/style.css" rel="stylesheet" type="text/css" />--%>
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <link href="../css/global.css" rel="stylesheet" type="text/css" />
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("input[type='text']").blur();
                    $("#btnSearch").trigger("click");
                }
            })

            //搜索单机
            $(document).on("click", "#Search", function () {
                $("#btnSearch").trigger("click");
            })

            //table 行样式
            $('.PublicList tbody tr:odd').addClass('odd');
        })

      
        //企业钱包详细页面
        function goInfo(Id, CompID) {
            window.location.href = 'CompFCmaterialsInfo.aspx?fid=' + Id + "&CompID="+CompID;
        }


    </script>
    <style>
        td {
            border-bottom:1px solid #eaeaea;
        }
    </style>


</head>
<body class="root3">
    <form id="form1" runat="server">
    <Head:Head ID="Head2" runat="server" />
    <div class="w1200">
        <Left:Left ID="Left1" runat="server" ShowID="nav-7" />
        <div class="rightCon">
            <div class="info">
                <a id="navigation1" href="/Distributor/UserIndex.aspx">我的桌面</a>>
            <a id="navigation2" href="#" class="cur">厂商首营资料</a></div>
            <!--功能条件 start-->
            <div class="userFun">
                <div class="right">
                    <ul class="term">
                        <li>
                             <label class="head">
                                厂商名称:</label><input type="text"  id="textName" name="textName"
                                    runat="server" class="box" style="width: 130px;" /><label class="head"></label>
                        </li>
                        <li>
                            <label class="head">
                                每页</label><input type="text" onkeyup='KeyInt(this)' id="txtPager" name="txtPager"
                                    runat="server" class="box" style="width: 30px;" /><label class="head">条</label></li>
                    </ul>
                    <a href="javascript:void(0)" id="Search" class="btnBl"><i class="searchIcon"></i>搜索</a>
                </div>
            </div>

            <!--功能条件 end-->
            <asp:Button ID="btnSearch"  runat="server" OnClick="btnSearch_Click" />
            <div class="blank10">
            </div>
            <!--订单管理 start-->
            <div class="orderNr">
                <!--信息列表 start-->
                <asp:Repeater runat="server" ID="rptOrder" OnItemCommand="rptOrder_ItemCommand">
                    <HeaderTemplate>
                        <table class="PublicList list">
                            <thead>
                                <tr>
                                    <th>
                                        厂商编码
                                    </th>
                                      <th>
                                        厂商名称
                                    </th>
                                    <th style="text-align: center; width: 110px;">
                                        操作
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr id='tr_<%# Eval("ID") %>'>
                       
                            <td>
                                <%#Eval("CompCode")%>
                            </td>
                            <td>
                               <%#Eval("CompName")%>
                            </td>
                            <td style="width: 110px" align="center">
                            <a href="javascript:void(0)" onclick='goInfo(<%# Eval("ID") %>,<%# Eval("CompID") %>)' class="tablelinkQx"
                                id="clickMx">详情</a>
                        </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </tbody> </table>
                    </FooterTemplate>
                </asp:Repeater>
                <!--信息列表 end-->
                <!--列表分页 start-->
                <div class="pagin">
                    <webdiyer:AspNetPager ID="Pager" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                        NextPageText=">" PageSize="12" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
                        ShowPageIndexBox="Always" TextAfterPageIndexBox="<span style='margin-left:5px;'>页</span>"
                        TextBeforePageIndexBox="<span>跳转到: </span>" ShowCustomInfoSection="Left" CustomInfoClass="message"
                        CustomInfoStyle="padding-left:20px;" CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                        CustomInfoSectionWidth="35%" CssClass="paginList" CurrentPageButtonClass="paginItem"
                        NumericButtonCount="5" Width="100%" OnPageChanged="Pager_PageChanged">
                    </webdiyer:AspNetPager>
                </div>
                <!--列表分页 end-->
                <!--分页 start-->
                <%-- 
                 <div class="page"><ul class="list">
        	        <li><a href="">&lt;</a></li><li class="cur"><a href="">1</a></li><li><a href="">2</a></li><li><a href="">3</a></li>
                    <li><a href="">4</a></li><li><a>...</a></li><li><a href="">10</a></li><li><a href="">&gt;</a></li>
                </ul></div>
                --%>
                <!--分页 end-->
            </div>
            <!--订单管理 end-->
        </div>
    </div>

    </form>
</body>
</html>
