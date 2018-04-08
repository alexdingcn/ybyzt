<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GType.aspx.cs" Inherits="Admin_Systems_IndustryList" %>



<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>商品分类管理</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="/Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="/js/layer/layer.js" type="text/javascript"></script>
    <script src="/js/layerCommon.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
    <link href="/css/layer.css" rel="stylesheet" type="text/css" />
    <script src="/Company/js/js.js" type="text/javascript"></script>
    <script>
        $(function () {
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    ChkPage();
                }
            })

            //一级分类展开事件
            $(".Openimg").click(function () {
                var img = $(this);
                var ParentId = $(this).parents("tr").attr("id");
                var str = $(this).attr("src")
                if ($(".tr" + ParentId + "").attr("id") == undefined) {
                    $.ajax({
                        type: "POST",
                        data: { action: "one", ParentId: ParentId },
                        dataType: "text",
                        success: function (html) {
                            $("#" + ParentId + "").after(html)
                            img.attr('src', '../../Company/images/menu_minus.gif');
                        },
                        error: function () { }
                    })
                } else {
                    if (str == "../../Company/images/menu_minus.gif") {
                        $(".tr" + ParentId + "").addClass("none")
                        img.attr('src', '../../Company/images/menu_plus.gif');
                        if ($(".tr3").attr("id") != undefined) {
                            $(".tr3").addClass("none")
                            $(".tr" + ParentId + " .Openimg2").attr('src', '../../Company/images/menu_plus.gif');
                        }
                    }
                    else {
                        $(".tr" + ParentId + "").removeClass("none")
                        img.attr('src', '../../Company/images/menu_minus.gif');
                    }
                }
            })
            //二级分类展开
            $(document).on("click", ".Openimg2", function () {
                var img = $(this);
                var ParentId = $(this).parents("tr").attr("id");
                var str = $(this).attr("src")
                if ($(".tr" + ParentId + "").attr("id") == undefined) {
                    $.ajax({
                        type: "POST",
                        data: { action: "two", ParentId: ParentId },
                        dataType: "text",
                        success: function (html) {
                            $("#" + ParentId + "").after(html)
                            img.attr('src', '../../Company/images/menu_minus.gif');
                        },
                        error: function () { }
                    })
                } else {
                    if (str == "../../Company/images/menu_minus.gif") {
                        $(".tr" + ParentId + "").addClass("none")
                        img.attr('src', '../../Company/images/menu_plus.gif');
                    }
                    else {
                        $(".tr" + ParentId + "").removeClass("none")
                        img.attr('src', '../../Company/images/menu_minus.gif');
                    }
                }
            })

        })

        
        $(document).ready(function () {

            //table 行样式
            $('.tablelist tbody tr:odd').addClass('odd');

            $(".liSenior").on("click", function () {
                $("div.hidden").slideToggle(100);
            })

        })

    </script>
    <style>
        .none {
            display:none;
        }
        .tablelist td {
            padding-left:10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <!--当前位置 start-->
    <uc1:Top ID="top1" runat="server" ShowID="nav-3" />

    <div class="rightinfo">
        <!--当前位置 start-->
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li>
                <li>></li>
                <li><a href="../GoodsNew/GType.aspx" runat="server" id="atitle">商品分类</a></li>
            </ul>
        </div>
    <!--当前位置 end-->
              <!--功能按钮 start-->
             <div class="tools">

            </div>
            <!--功能按钮 end-->
        
  <!--信息列表 start-->
  <table class="tablelist" id="tablelist">
                <thead>
                    <tr>
                        <th>
                            类别名称
                        </th>
                        <th>
                            状态
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptGTypeList" runat="server">
                        <ItemTemplate>
                            <tr id='<%# Eval("ID") %>' parentid='<%# Eval("ParentId")%>' style='height: 26px;width: 100%;'>
                             <td style="width:650px"> <img class="Openimg" height='9' src='../../Company/images/<%#Eval("SVdef3") == null?"menu_minus":"menu_plus"%>.gif' width='9'
                                   border='0' />&nbsp;   <%# Eval("TypeName")%> 

                             </td>
                            <td>
                                <div class="tcle" >
                                    <%# Eval("IsEnabled").ToString()=="True"?"启用":"禁用" %>
                                </div>
                            </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
  <!--信息列表 end-->

    </div>
    </form>
</body>
</html>
