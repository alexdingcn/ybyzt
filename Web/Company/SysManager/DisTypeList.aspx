<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DisTypeList.aspx.cs" Inherits="Company_SysManager_DisTypeList" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>代理商分类</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../js/TreeViewOpen.js" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            if('<%=Request["type"]+"" %>'=="1"){
                $(".rightinfo").css("width", "auto");
            }
            var div = $('<div   Msg="True"  style="z-index:1000; background-color:#000; opacity:0.3; filter:alpha(opacity=30);position: fixed;width:100%;height:100%;top:0px;left:0px;"></div>');
            $(".tools li.click").on("click", function () {
                $(".AddWindow div.trParent").hide();
                $("body").append(div);
                $(".AddWindow #<%=hideTypeId.ClientID %>").val("0");
                $(".AddWindow").css("height", "240px").fadeIn(200);
            })

            $(".AddWindow .tiptop a,.EditWindow .tiptop a,.tipbtn .cancel").on("click", function () {
                $(div).remove();
                $(".EditWindow,.AddWindow").fadeOut(100);
            })

            $(".tablelist .TypeChildAdd").on("click", function () {
                $(".AddWindow #<%=lblTypeName.ClientID %>").val($(this).attr("Pname"));
                $(".AddWindow #<%=hideTypeId.ClientID %>").val($(this).attr("tip"));
                $(".AddWindow div.trParent").show();
                $("body").append(div);
                $(".AddWindow").css("height", "280px").fadeIn(200);
            })

            $(".tablelist .TypeEdit").on("click", function () {
                $(".EditWindow #<%=hideTypeIds.ClientID %>").val($(this).attr("tip"));
                $(".EditWindow #<%=txtTypeNames.ClientID %>").val($(this).attr("Pname"));
                $("body").append(div);
                $(".EditWindow").css("height", "240px").fadeIn(200);
            })

            $(".tablelist .TypeDel").on("click", function () {
                var obj = $(this);
               layerCommon.confirm("确认删除", function () {
                    var id = obj.attr("tip");
                    $.ajax({
                        type: "post",
                        url: "DisTypeList.aspx",
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
                                layerCommon.msg(json.code, IconOption.错误);
                            }
                        }, error: function (XMLHttpRequest, textStatus, errorThrown) {

                        }
                    });
                }, "提示");

            })
        })

        function ChkPage() {
            if ($("#txtPageSize").val() == "") {
                layerCommon.msg("每页显示数量不能为空", IconOption.错误);
                return false;
            }
            else {
                $("#btnSearch").click();
            }
            return true;
        }
        function formCheck() {
            var Name = $.trim($(".txtTypeName").val());
            var Names = $.trim($(".txtTypeNames").val());
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

        //Enter键表单自动提交
        document.onkeydown = function (event) {
            var target, code, tag;
            var temp = $(".AddWindow").is(":visible"); //新增是否显示
            var temp1 = $(".EditWindow").is(":visible"); //修改是否显示

            var cla = "";
            if (temp)
                cla = ".AddWindow";
            else if (temp1)
                cla = ".EditWindow";
            else
                cla = "";
            var ID = $(cla + " .tipbtn input[type=\"submit\"]").attr("ID");

            if (!event) {
                event = window.event; //针对ie浏览器  
                target = event.srcElement;
                code = event.keyCode;
                if (code == 13) {
                    if (cla != "") {
                        $("input[type=\"text\"]").blur();
                        $("#" + ID).trigger("click");
                    } else {
                        ChkPage();
                    }
                } else {
                    return true;
                }
            }
            else {
                target = event.target; //针对遵循w3c标准的浏览器，如Firefox  
                code = event.keyCode;
                if (code == 13) {
                    if (cla != "") {
                        $("input[type=\"text\"]").blur();
                        $("#" + ID).trigger("click");
                    } else {
                        ChkPage();
                    }
                } else {
                    return true;
                }
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
    <div>
    <uc1:top ID="top1" runat="server" ShowID="nav-4" />
        <div class="rightinfo">
             <!--当前位置 start-->
                <div class="place">
                    <ul class="placeul">
                        <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
                        <li><a href="../SysManager/DisTypeList.aspx">代理商分类</a></li>
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
                        <li>每页<input name="txtPageSize" type="text" style=" width:40px;" class="textBox txtPageSize" id="txtPageSize"
                            runat="server" value="12" onkeyup="KeyInt(this);" onblur="KeyInt(this);" />条</li>
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
                        <th>
                            操作
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptDisTypeList" runat="server" >
                        <ItemTemplate>
                            <tr id='<%#Eval("ID") %>' parentid='<%# Eval("ParentId")%>' bgcolor='#fcfeff' style='height: 26px;
                                width: 100%;'>
                                <td>
                                   <div class="tcle"> <img id="Openimg" height='9' src='<%# Simage(Eval("Id")) %>' width='9' border='0' />&nbsp;<span
                                        class="span">
                                        <%# Eval("TypeName")%></span></div>
                                </td>
                                <td>
                                    <div class="tcle"> 　<a href="javascript:;" tip='<%# Eval("Id") %>' Pname='<%#Eval("TypeName") %>' class="TypeChildAdd">添加下级</a> | 
                                    <a class="TypeEdit" href="javascript:;" tip='<%# Eval("Id") %>' sortid='<%#Eval("SortIndex") %>'   Pname='<%#Eval("TypeName") %>' 
                                    >编辑</a> | 
                                    <a href="javascript:;"  tip='<%# Eval("Id") %>'  class="TypeDel">移除</a></div>
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
        <!--新增 start-->
        <div class="AddWindow tip" style="display: none;">
            <div style=" position:absolute;top:0; left:0; right:0; bottom:0;z-index:999; background:#fff;">
            <div class="tiptop">
                <span>新增</span><a></a></div>
            <div class="tipinfo">
                <div class="lb trParent" id="trParent" runat="server">
                    <span><i class="required">*</i>父类名称：</span><input name="lblTypeName" disabled="disabled" id="lblTypeName" type="text" runat="server" class="textBox lblTypeName" />
                    <asp:HiddenField ID="hideTypeId" runat="server" />
                </div>
                <div class="lb">
                    <span><i class="required">*</i>分类名称：</span>
                    <input name="txtTypeName" id="txtTypeName" style=" margin-left:2px;"  type="text" runat="server" class="textBox txtTypeName" maxlength="50" /></div>
<%--                  <div class="lb">
                    <span>分类编码：</span><input name="txtTypecode" id="txtTypecode" type="text" runat="server" class="textBox txtTypecode" /></div>--%>
                <div class="tipbtn">
                    <asp:Button ID="btnAdd" CssClass="orangeBtn" runat="server" Text="确定" OnClientClick="return  formCheck();"
                        OnClick="btnAdd_Click" />&nbsp;
                    <input name="" type="button" class="cancel" value="取消" />
                </div>
            </div>
            </div>
            <div id="xubox_border1" class="xubox_border" style="z-index:11; opacity: 0.3;filter: Alpha(Opacity=30) !important; border-radius:5px;top: -8px; left: -8px; right: -8px; bottom: -8px;  background-color: rgb(0, 0, 0); position:absolute; top"></div>
        </div>
        <!--新增 end-->
        <!--编辑 start-->
        <div class="EditWindow tip" style="display: none;">
            <div style=" position:absolute;top:0; left:0; right:0; bottom:0;z-index:999; background:#fff;">
            <div class="tiptop">
                <span>编辑</span><a></a></div>
            <div class="tipinfo">
                <asp:HiddenField ID="hideTypeIds" runat="server" />
                <div class="lb">
                    <span>*分类名称：</span><input name="txtTypeNames"  id="txtTypeNames" runat="server"
                        type="text" class="textBox txtTypeNames" maxlength="50" />
                    
                        </div>
               <%--           <div class="lb">  <span>分类编码：</span><input name="txtTypecodes" id="txtTypecodes" type="text"
                        runat="server" class="textBox txtTypecodes" /></div>--%>
                <div class="tipbtn">
                    <asp:Button ID="btnEdit" CssClass="sure" runat="server" Text="确定" OnClientClick="return formCheck();"
                        OnClick="btnEdit_Click" />&nbsp;
                    <input name="" type="button" class="cancel" value="取消" />
                </div>
            </div>
            </div>
            <div id="xubox_border" class="xubox_border" style="z-index:11; opacity: 0.3;filter: Alpha(Opacity=30) !important; border-radius:5px;top: -8px; left: -8px; right: -8px; bottom: -8px;  background-color: rgb(0, 0, 0); position:absolute; top"></div>
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
