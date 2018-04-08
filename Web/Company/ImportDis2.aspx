<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImportDis2.aspx.cs" Inherits="Company_ImportDis2" %>

<%@ Register Src="~/Company/UserControl/CompRemove.ascx" TagPrefix="uc1" TagName="CompRemove" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>代理商导入</title>
    <link href="css/global-2.0.css" rel="stylesheet" type="text/css" />
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <style>
        .t6
        {
            width: 60px;
        }
        .t7
        {
            width: auto;
        }
        .t77
        {
            width: 100px;
        }
        .t8
        {
            width: auto;
        }
        tr th
        {
            line-height: 15px;
        }
    </style>
    <script>
        $(function () {
            //上一步
            $("#Search").click(function () {
                location.href = "ImportDis.aspx";
            })
            //数据检查
            $("#lblbtnToExcel").click(function () {
                $(".liSenior").attr("style", "cursor:pointer");
                // $(".tabLine table th:last").show();
                $(".tabLine table tr").each(function (index, obj) {
                    $(this).find("td").find(".tle").show();
                })
            })
            //确定导入
            $(".liSenior").click(function () {
                if ($(this).css("cursor") != "not-allowed") {
                    $("#btnImport").click();
                } else {
                    layerCommon.msg("请先数据检查", IconOption.错误);
                    return false;
                }
            })
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Button ID="btnImport" runat="server" Text="Button" Style="display: none" OnClick="btnImport_Click" />
    <uc1:Top ID="top1" runat="server" ShowID="nav-4" />
    <uc1:CompRemove runat="server" ID="Remove" ModuleID="4" />
    <div class="rightinfo">
        <!--当前位置 start-->
        <div class="place">
            <ul class="placeul">
                <li><a href="jsc.aspx">我的桌面 </a></li>
                <li>&gt;</li>
                <li><a href="SysManager/DisList.aspx" id="a1">代理商列表</a></li>
                <li>&gt;</li>
                <li><a href="javascript:;" id="atitle">代理商导入</a></li>
            </ul>
        </div>
        <!--当前位置 end-->
        <div class="blank10">
        </div>
        <!--步骤 start-->
        <ul class="imStep">
            <li class="a1">1、上传导入文件<i class="arr1"></i></li>
            <li class="a2 cur">2、导入预览<i class="arr1"></i><i class="arr2"></i></li>
            <li class="a3">3、导入完成<i class="arr1"></i><i class="arr2"></i></li>
        </ul>
        <!--步骤 end-->
        <ul class="toolbar">
            <li id="Search"><span>
                <img src="images/tp3.png"></span>上一步</li>
            <li id="lblbtnToExcel"><span>
                <img src="images/t04.png"></span>数据检查</li>
            <li class="liSenior" style="cursor: not-allowed"><span>
                <img src="images/t15.png"></span>确认导入</li>
        </ul>
        <div class="blank10">
        </div>
        <div class="tabLine ">
            <table border="0" cellspacing="0" cellpadding="0">
                <thead>
                    <tr>
                        <th class="t7" height="35">
                            代理商名称
                        </th>
                        <th class="t7" height="35">
                            管理员姓名
                        </th>
                        <th class="t7" height="35">
                            登录帐号
                        </th>
                        <th class="t6" height="35">
                           手机
                        </th>
                        <th class="t7">
                            所在省
                        </th>
                        <th class="t7">
                            所在市
                        </th>
                        <th class="t7">
                         所在区县
                        </th>
                        <th class="t77">
                           详细地址
                        </th>
                       <%-- <th class="t6">
                            代理商分类
                        </th>
                        <th class="t7">
                            代理商区域
                        </th>--%>
                          <th class="t7">
                            备注
                        </th>
                        <th class="t7">
                            检查结果
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptDis" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <div class="tle">
                                        <%#Eval("disname")%></div>
                                </td>
                                <td>
                                    <ul class="tle">
                                        <li>
                                            <%#Eval("principal")%></li></ul>
                                </td>
                                <td>
                                    <div class="tle">
                                        <%#Eval("username")%></div>
                                </td>
                                <td>
                                    <div class="tc">
                                        <%#Eval("phone")%></div>
                                </td>
                                <td>
                                    <div class="tc">
                                        <%# Eval("pro")%></div>
                                </td>
                                <td>
                                    <div class="tc">
                                        <%#Eval("city")%></div>
                                </td>
                                <td>     <div class="tc">
                                          <%#Eval("quxian")%></div>
                                </td>
                                <td>
                                    <div class="tc" title=' <%#Eval("address") %>'>
                                        <%#Eval("address").ToString().Length > 10 ? Eval("address").ToString().Substring(0, 10) + "..." : Eval("address").ToString()%></div>
                                </td>
                               <%-- <td>
                                    <div class="tc">
                                        <%#Eval("distype")%></div>
                                </td>
                                <td>
  <div class="tc">
                                        <%#Eval("area")%></div>
                                </td>--%>
                                 <td>
 <div class="tc" title=' <%#Eval("remark") %>'>
                                        <%#Eval("remark").ToString().Length > 10 ? Eval("remark").ToString().Substring(0, 10) + "..." : Eval("remark").ToString()%></div>
                                </td>
                                <td>
                                    <ul class="tle" style="display: none;">
                                        <li <%#Eval("chkstr").ToString() != "数据正确！"?"style='color:Red'":"" %>>
                                            <%#Eval("chkstr")%></li></ul>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
