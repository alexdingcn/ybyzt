<%@ Page Language="C#" AutoEventWireup="true" CodeFile="selectHtList.aspx.cs" Inherits="Distributor_Storage_selectHtList" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>选择医院</title>

    <link href="../../Company/css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../Company/css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../Company/js/order.js" type="text/javascript"></script>
    <link href="../newOrder/css/global2.5.css?v=2015001311899" rel="stylesheet"
        type="text/css" />

    <script type="text/javascript">
        $(function () {
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("#btnSearch").trigger("click");
                }
            });

            //订单生成 搜索
            $("#Search").on("click", function () {
                $("#btnSearch").trigger("click");
            });

            $("#btnCancel").click(function () {
                window.parent.closeAll();
            })

            $(".htname").click(function () {
                window.parent.$("#HtDrop").val($(this).text());
                window.parent.closeAll();
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">

        <input id="hid_Alert" type="hidden" />
        <input type="hidden" id="hidCompId" runat="server" />
        <input type="hidden" id="index" runat="server" />
        <div class="rightinfo" style="width: 1000px; margin: 0 auto; margin-top: 10px;">
            <!--功能按钮 start-->
            <div class="tools">

                <div class="right">
                    <ul class="toolbar right">
                        <li id="Search"><span>
                            <img src="../../Company/images/t04.png" /></span>搜索</li>
                        
                    </ul>
                    <ul class="toolbar3" style=" margin-top:-1px;">
                         <li><label class="head">医院名称:</label><input name="txtHtName" type="text" id="txtHtName" runat="server" class="box1" maxlength="50" /></li>

                    </ul>
                </div>
            </div>
            <!--功能按钮 end-->
            <table class="tablelist" id="TbList">
                <thead>
                    <tr>
                        <th>医院名称</th>
                        <th>医院编码</th>
                        <th>医院等级</th>
                    </tr>
                </thead>
                <tbody>
                    <!--信息列表 start-->
                    <asp:Repeater runat="server" ID="rptOrder">
                        <ItemTemplate>
                            <tr id='tr_<%# Eval("Id") %>'>
                                <td><div class="tc">
                                  <a style=" cursor:pointer;" href="javascript:void(0);" class="htname"><%# Eval("HospitalName")%></a></div>
                                </td>
                                
                                <td><div class="tc">
                                    <%# Eval("HospitalCode")%>    </div>
                                </td>
                                <td>
                                    <div class="tc">
                                       <%# Eval("HospitalLevel")%>
                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
            <!--信息列表 end-->
            <asp:Button ID="btnSearch" Text="搜索" runat="server" OnClick="btnSearch_Click" Style="display: none" />
            
            <!--列表分页 start-->
            <div class="pagin">
                <webdiyer:AspNetPager ID="Pager" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                    NextPageText=">" PageSize="10" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
                    ShowPageIndexBox="Always" TextAfterPageIndexBox="<span class='pagiPage'>页</span>"
                    TextBeforePageIndexBox="<span class='jump'>跳转到:</span>" CustomInfoSectionWidth="20%"
                    CustomInfoStyle="padding:5px 0 0 20px;cursor: default;color: #737373;" CustomInfoClass="message"
                    ShowCustomInfoSection="Left" CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                    CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5"
                    OnPageChanged="Pager_PageChanged">
                </webdiyer:AspNetPager>
            </div>
            <!--列表分页 end-->
            <!--商品 end-->
            <div class="po-btn">
                <a href="javascript:void(0);" class="gray-btn" id="btnCancel">取消</a>
            </div>
        </div>
    </form>
</body>
</html>
