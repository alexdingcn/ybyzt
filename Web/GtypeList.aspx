<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GtypeList.aspx.cs" Inherits="GtypeList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="Admin/css/style.css" rel="stylesheet" />
    <script src="Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="Company/js/js.js" type="text/javascript"></script>
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
                        dataTyoe: "text",
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
                        dataTyoe: "text",
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

            $("li#libtnAdd").on("click", function () {
                location.href = "IndustryModify.aspx";
            })
        })

        // 每页显示数量非空验证
        function ChkPage() {
            if ($("#txtPageSize").val() == "") {
                alert("- 每页显示数量不能为空");
                return false;
            } else {
                $("#btnSearch").click();
            }
            return true;
        }
        //重载
        function Reset() {
            location.href = "IndustryList.aspx";
        }
    </script>
    <style>
        .none {
            display:none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
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
                             <td style="width:650px"> <img class="Openimg" height='9' src='../../Company/images/menu_plus.gif' width='9'
                                   border='0' />&nbsp;   <%# Eval("TypeName")%> --- <%# Eval("ID")%></td>
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
    </form>
</body>
</html>
