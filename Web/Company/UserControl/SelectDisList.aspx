<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectDisList.aspx.cs" Inherits="Company_UserControl_SelectDisList" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/TreeDisArea.ascx" TagPrefix="uc1" TagName="DisArea" %>
<%@ Register Src="~/Company/UserControl/TreeDisType.ascx" TagPrefix="uc2" TagName="DisType" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <link href="../../Company/css/style.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
    <script src="../../js/layerCommon.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
    <script src="../../js/CommonJs.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
    <script src="../js/js.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            $_def.ID = "btn_Search";
            $(".showDiv3 .ifrClass,.showDiv2 .ifrClass").css("width", "135px");
            $(".showDiv3,.showDiv2").css("width", "130px");
            $('iframe').load(function () {
                $('iframe').contents().find('.pullDown').css("width", "130px");
            });
            $(".tablelist tbody a#A_DisSelect").on("click", function () {
                //                var $radio = $(".tablelist tr td input[type=radio]:checked");
                //                if ($radio.length == 0) {
                //                    errMsg("提示", "请选择企业", "", "");
                //                    return;
                //                }
                var id = $(this).attr("disid");
                var name = $(this).siblings("#Hid_DisName").val();
                window.parent.selectDis(id, name);
            });

            $(".tablelist tbody tr").on("click", function () {
                var id = $(this).find("a#A_DisSelect").attr("disid");
                var name = $(this).find("a#A_DisSelect").siblings("#Hid_DisName").val();
                window.parent.selectDis(id, name);
            });

            $("#inpqx").on("click", function () {
                window.parent.CloseDialogTo();
            })

            $("#inpqk").on("click", function () {
                var id = "";
                var name = "";
                window.parent.selectDis(id, name);
            })

            ///重置
            $("#li_Reset").click(function () {
                //window.location.href = 'OrderCreateList.aspx';
                location = location;
            });

            $(document).ready(function () {
                $('.tablelist tbody tr:odd').addClass('odd');
                $("li#liSearch").on("click", function () {
                    $("#btn_Search").trigger("click");
                })

            })

        })

        //        function onClickDis(id, Name) {
        //            //alert(id + " :" + Name);
        //            window.parent.selectDis(id, Name);
        //        }
    </script>
    <style>
        .clears
        {
            background: url(../images/btnbg2.png) repeat-x;
            color: #000;
            font-weight: normal;
        }
        .rightinfo
        {
            margin-top:0px;  
        }
    </style>
</head>
<body style="min-width: 800px;">
    <form id="form1" runat="server">
    <div class="rightinfo" style=" margin-left:0px; width:820px;">
        <div class="tools">
            <input name="" type="button" class="sure" id="inpqk" value="清空选择" />
            <div class="right">
                <ul class="toolbar right">
                    <li id="liSearch"><span>
                        <img src="../../Company/images/t04.png" /></span>搜索</li>
                    
                </ul>
                <input type="button" runat="server" id="btn_Search" style="display: none;" onserverclick="btn_SearchClick" />
                <ul class="toolbar3">
                    <li>代理商名称:<input runat="server" id="txtDisName" type="text" class="textBox" />&nbsp;&nbsp;</li>
                    <%--<li>代理商简称:<input runat="server" id="txtDisSname" type="text" class="textBox" />&nbsp;&nbsp;</li>--%>
                    <li>代理商分类:<uc2:DisType runat="server" ID="txtDisType" />
                    </li>
                    <li>代理商区域:<uc1:DisArea runat="server" ID="txtDisAreaBox" />
                    </li>
                </ul>
            </div>
        </div>
        <table class="tablelist">
            <thead>
                <tr>
                    <%--<th width="60px">
                        请选择
                    </th>--%>
                    <th>
                        代理商名称
                    </th>
                    <%--<th>
                        代理商简称
                    </th>--%>
                    <th>
                        代理商分类
                    </th>
                    <th>
                        代理商区域
                    </th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="Rpt_Dis" runat="server">
                    <ItemTemplate>
                        <tr style="cursor: pointer;">
                            <%--<td>
                               
                            </td>--%>
                            <td>
                               <div class="tcle"> <a id="A_DisSelect" style="cursor: pointer; text-decoration: underline; display: block"
                                    disid="<%#Eval("ID")%>"></a>
                                <%# Eval("DisName")%>
                                <input type="hidden" value="<%#Eval("DisName")%>" id="Hid_DisName" /></div>
                            </td>
                            <%--<td>
                                <%# Eval("ShortName")%>
                            </td>--%>
                            <td>
                              <div class="tc">  <%# Common.GetDisTypeNameById(Eval("DisTypeID").ToString().ToInt(0))%></div>
                            </td>
                            <td>
                               <div class="tc">  <%# Common.GetDisAreaNameById(Eval("AreaID").ToString().ToInt(0))%></div>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <div class="pagin" style="height: 30px;">
            <webdiyer:AspNetPager ID="Pager" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                NextPageText=">" PageSize="12" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
                ShowPageIndexBox="Always" TextAfterPageIndexBox="<span class='pagiPage'>页</span>"
                TextBeforePageIndexBox="<span class='jump'>跳转到:</span>" CustomInfoSectionWidth="30%"
                CustomInfoStyle="padding:5px 0 0 20px;cursor: default;color: #737373;" CustomInfoClass="message"
                ShowCustomInfoSection="Left" CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5"
                OnPageChanged="Pager_PageChanged">
            </webdiyer:AspNetPager>
        </div>
        <%--<div class="div_footer">
            <input name="" type="button" class="cancel" id="inpqx" value="取消" />
            <input name="" type="button" class="cancel" id="inpqk" value="清空" />
        </div>--%>
    </div>
    </form>
</body>
</html>
