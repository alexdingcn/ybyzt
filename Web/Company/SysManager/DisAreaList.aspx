<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DisAreaList.aspx.cs" Inherits="Company_SysManager_DisAreaList" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>代理商区域</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../js/TreeViewOpen.js" type="text/javascript"></script>
    <script>
        $(function () {
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    ChkPage();
                }
            })
        })
        $(document).ready(function () {
            if('<%=Request["type"]+"" %>'=="1"){
                $(".rightinfo").css("width","auto");
            }
            var div = $('<div   Msg="True"  style="z-index:1000; background-color:#000; opacity:0.3; filter:alpha(opacity=30);position: fixed;width:100%;height:100%;top:0px;left:0px;"></div>');
            $(".tools li.click").on("click", function () {
                $(".AddWindow div.trParent").hide();
                $("body").append(div);
                $(".AddWindow #<%=hideAreaId.ClientID %>").val("0");
                $(".AddWindow").css("height", "200px").fadeIn(200);
            })

            $(".AddWindow .tiptop a,.EditWindow .tiptop a,.tipbtn .cancel").on("click", function () {
                $(div).remove();
                $(".EditWindow,.AddWindow").fadeOut(100);
            })

            $(".tablelist .AreaChildAdd").on("click", function () {
                $(".AddWindow #<%=lblAreaName.ClientID %>").val($(this).attr("Pname"));
                $(".AddWindow #<%=hideAreaId.ClientID %>").val($(this).attr("tip"));
                $(".AddWindow div.trParent").show();
                $("body").append(div);
                $(".AddWindow").css("height", "240px").fadeIn(200);
            })

            $(".tablelist .AreaEdit").on("click", function () {
                $(".EditWindow #<%=hideAreaIds.ClientID %>").val($(this).attr("tip"));
                $(".EditWindow #<%=txtAreaNames.ClientID %>").val($(this).attr("Pname"));
                $(".EditWindow #<%=txtSortIndexs.ClientID %>").val($(this).attr("sortid"));
                $("body").append(div);
                $(".EditWindow").css("height", "200px").fadeIn(200);
            })

            $(".tablelist .AreaDel").on("click", function () {
                var obj = $(this);
               layerCommon.confirm("确认删除？", function () {
                    var id = obj.attr("tip");
                    $.ajax({
                        type: "post",
                        url: "DisAreaList.aspx",
                        dataType: "text",
                        async: false,
                        data: { Action: "Del", Id: id },
                        success: function (data) {
                            var json = eval('(' + data + ')');
                            if (json.result != undefined) {
                                if (json.result == true) {
                                    obj.parents("tr:eq(0)").remove();
                                }
                                else {
                                    if (json.code != "操作成功") {
                                        layerCommon.msg(json.code, IconOption.笑脸);
                                    }
                                }
                            } else {
                                layerCommon.msg("操作失败", IconOption.哭脸);
                            }
                        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                            var a;
                        }
                    })
                }, "提示");

            })


        })

        function ChkPage() {
            if ($("#txtPageSize").val() == "") {
                layerCommon.msg(" 每页显示数量不能为空", IconOption.错误);
                return false;
            }
            else {
                $("#btnSearch").click();
            }
            return true;
        }
        function formCheck() {
            var Name = $.trim($(".txtAreaName").val());
            var Names = $.trim($(".txtAreaNames").val());
            var str = "";
            if (Name == "" && Names == "") {
                str += "类别不能为空";
            }
            if (str == "") {
                $(".EditWindow,.AddWindow").fadeOut(100);
                return true;
            }
            else {
                layerCommon.msg(str, IconOption.错误);
                return false;
            }
        }
    
    </script>
    <style>
        .span
        {
            margin: 0;
            padding: 0;
            display: inline;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-4" />
    <div>
        <div class="rightinfo">
            <!--当前位置 start-->
            <div class="place">
                <ul class="placeul">
                    <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
                    <li><a href="../SysManager/DisAreaList.aspx">代理商区域</a></li>
                </ul>
            </div>
            <!--当前位置 end-->

            <!--功能按钮 start-->
            <div class="tools">
                <ul class="toolbar left">
                    <li class="click"><span>
                        <img src="../images/t01.png" /></span><font>新增一级类别</font></li>
                </ul>
                <div class="right">
                    <ul class="toolbar right ">
                        <li onclick="return ChkPage()"><span>
                            <img src="../images/t04.png" /></span>搜索</li>
                    </ul>
                    <ul class="toolbar3">
                        <li>每页<input name="txtPageSize" type="text" style="width: 40px;" class="textBox txtPageSize"
                            id="txtPageSize" runat="server" value="12" onkeyup="KeyInt(this);" onblur="KeyInt(this);" />条</li>
                    </ul>
                </div>
            </div>
            <!--功能按钮 end-->
            <!--信息列表 start-->
            <table class="tablelist" id="tablelist">
                <thead>
                    <tr>
                        <th>
                            类别名称
                        </th>
                        <%--                     <th>
                            编码
                        </th>--%>
                        <th>
                            排序
                        </th>
                        <th>
                            操作
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptDisAreaList" runat="server">
                        <ItemTemplate>
                            <tr id='<%#Eval("ID") %>' parentid='<%# Eval("ParentId")%>' bgcolor='#fcfeff' style='height: 26px;
                                width: 100%;'>
                                <td>
                                <div class="tcle"> 
                                    <img id="Openimg" height='9' src='<%# Simage(Eval("Id")) %>' width='9' border='0' />&nbsp;<span
                                        class="span">
                                        <%# Eval("AreaName")%></span></div>
                                </td>
                                <%--                             <td>
                                 <%# Eval("Areacode")%></span>
                                </td>--%>
                                <td>
                                   <div class="tc tcle"> <%# Eval("Sortindex")%></div>
                                </td>
                                <td>
                                <div class="tcle"> 
                                   　 <a href="javascript:;" tip='<%# Eval("Id") %>' pname='<%#Eval("AreaName") %>' class="AreaChildAdd">
                                        添加下级</a> | <a class="AreaEdit" href="javascript:;" tip='<%# Eval("Id") %>' sortid='<%#Eval("sortindex") %>'
                                            pname='<%#Eval("AreaName") %>'>编辑</a> | <a href="javascript:;" tip='<%# Eval("Id") %>'
                                                class="AreaDel">移除</a></div>
                                </td>
                            </tr>
                            <%# FindChild(Eval("Id").ToString())%>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
            <asp:Button ID="btnSearch" runat="server" Style="display: none" Text="搜索" OnClick="btnSearch_Click" />
            <!--信息列表 end-->
            <!--列表分页 start-->
            <div class="pagin">
                <webdiyer:AspNetPager ID="Pager" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                    NextPageText=">" PageSize="12" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
                    ShowPageIndexBox="Always" TextAfterPageIndexBox="<span class='pagiPage'>页</span>"
                    TextBeforePageIndexBox="<span class='jump'>跳转到:</span>" CustomInfoSectionWidth="20%"
                    CustomInfoStyle="padding:5px 0 0 20px;cursor: default;color: #737373;" CustomInfoClass="message"
                    ShowCustomInfoSection="Left" CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                    CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5"
                    OnPageChanged="Pager_PageChanged">
                </webdiyer:AspNetPager>
            </div>
            <!--列表分页 end-->
        </div>
        <!--新增 start-->
        <div class="AddWindow tip" style="display: none;">
            <div style=" position:absolute;top:0; left:0; right:0; bottom:0;z-index:999; background:#fff;">
            <div class="tiptop">
                <span>新增</span><a></a></div>
            <div class="tipinfo">
                <div class="lb trParent" id="trParent" runat="server">
                    <span><i class="required">*</i>父类名称：</span><input name="lblAreaName" disabled="disabled"
                        id="lblAreaName" type="text" runat="server" class="textBox lblAreaName" />
                    <asp:HiddenField ID="hideAreaId" runat="server" />
                </div>
                <div class="lb">
                    <span><i class="required">*</i>区域名称：</span>
                    <input name="txtAreaName" id="txtAreaName" style="margin-left: 2px;" type="text"
                        runat="server" maxlength="50" class="textBox txtAreaName" />
                </div>
                <%--                          <div class="lb">
                    <span>区域编码：</span><input name="txtAreacode" id="txtAreacode" type="text"
                        runat="server" class="textBox txtAreacode" /></div>--%>
                <div class="lb">
                    <span>排序：</span><input name="txtSortIndex" onkeyup="KeyInt(this);" onblur="KeyInt(this);"
                        runat="server" id="txtSortIndex" type="text" class="textBox" /></div>
                <div class="tipbtn">
                    <asp:Button ID="btnAdd" CssClass="orangeBtn" runat="server" Text="确定" OnClientClick="return  formCheck();"
                        OnClick="btnAdd_Click" />&nbsp;
                    <input name="" type="button" class="cancel" value="取消" />
                </div>
            </div>
            </div>
            <div id="xubox_border" class="xubox_border" style="z-index:11; opacity: 0.3;filter: Alpha(Opacity=30) !important; border-radius:5px;top: -8px; left: -8px; right: -8px; bottom: -8px;  background-color: rgb(0, 0, 0); position:absolute; top"></div>
        </div>
        <!--新增 end-->
        <!--编辑 start-->
        <div class="EditWindow tip" style="display: none;">
            <div style=" position:absolute;top:0; left:0; right:0; bottom:0;z-index:999; background:#fff;">
            <div class="tiptop">
                <span>编辑</span><a></a></div>
            <div class="tipinfo">
                <asp:HiddenField ID="hideAreaIds" runat="server" />
                <div class="lb">
                    <span>*区域名称：</span><input name="txtAreaNames" id="txtAreaNames" runat="server" type="text"
                        class="textBox txtAreaNames" maxlength="50" /></div>
                <%--                          <div class="lb">  <span>区域编码：</span><input name="txtAreacodes" id="txtAreacodes" type="text"
                        runat="server" class="textBox txtAreacodes" /></div>--%>
                <div class="lb">
                    <span>排序：</span><input name="txtSortIndexs" id="txtSortIndexs" runat="server" type="text"
                        class="textBox" onkeyup="KeyInt(this);" onblur="KeyInt(this);" /></div>
                <div class="tipbtn">
                    <asp:Button ID="btnEdit" CssClass="orangeBtn" runat="server" Text="确定" OnClientClick="return formCheck();"
                        OnClick="btnEdit_Click" />&nbsp;
                    <input name="" type="button" class="cancel" value="取消" />
                </div>
            </div>
            </div>
            <div id="xubox_border1" class="xubox_border" style="z-index:11; opacity: 0.3;filter: Alpha(Opacity=30) !important; border-radius:5px;top: -8px; left: -8px; right: -8px; bottom: -8px;  background-color: rgb(0, 0, 0); position:absolute; top"></div>
        </div>
        <!--编辑 end-->
        <script type="text/javascript">
            //收缩插件
            $(document).ready(function () {
                $("#tablelist").TreeViewOpen({ imgID: "Openimg", UpSrc: "../images/menu_plus.gif", DownSrc: "../images/menu_minus.gif" });
            })
            $('.tablelist tbody tr:odd').addClass('odd');

        </script>
    </div>
    </form>
</body>
</html>
