<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IndustryList.aspx.cs" Inherits="Admin_Systems_IndustryList" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>商品分类管理</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
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

            $("li#libtnAdd").on("click", function () {
                location.href = "IndustryModify.aspx";
            })

            var div = $('<div   Msg="True"  style="z-index:1000; background-color:#000; opacity:0.3; filter:alpha(opacity=30);position: fixed;width:100%;height:100%;top:0px;left:0px;"></div>');
           
            //新增一级分类
            $(document).on("click", ".tools li.click", function () {
                $("#MsgT").hide();
                $(".AddWindow div.trParent").hide();
                $("body").append(div);
                $(".AddWindow #<%=hidePTypeId.ClientID %>").val("0");
                $(".AddWindow").css("height", "240px").fadeIn(200);
            });

           //新增子分类
            $(document).on("click", ".tablelist .TypeChildAdd", function () {
                $("#MsgT").hide();
                $(".AddWindow div.trParent").show();
                $("body").append(div);
               $(".AddWindow #<%=lblTypeName.ClientID %>").val($(this).attr("Pname"));
                $(".AddWindow #<%=hidePTypeId.ClientID %>").val($(this).attr("tip"));
                $(".AddWindow").css("height", "280px").fadeIn(200);
            });

            //编辑分类
            $(document).on("click", ".tablelist .TypeEdit", function () {
                $("#MsgT").hide();
                $(".EditWindow div.trParent").show();
                $("body").append(div);
                $(".EditWindow #<%=txtTypeNames.ClientID %>").val($(this).attr("Pname"));
                $(".EditWindow #<%=hideTypeId.ClientID %>").val($(this).attr("tip"));
                $(".EditWindow").css("height", "240px").fadeIn(200);
            });

            //取消
            $(document).on("click", ".AddWindow .tiptop a,.EditWindow .tiptop a,.tipbtn .cancel", function () {
                $(div).remove();
                $(".EditWindow,.AddWindow").fadeOut(100);

                //清除商品分类的数据，包括商品大类，分类名称，隐藏空间的值
                $("#hidePTypeId").val("");
                $("#hideTypeId").val("");
            });

            //删除分类
            $(document).on("click", ".tablelist .TypeDel", function () {
                var _this = this;
                layerCommon.confirm("确认移除？<span style='color:red'>（如有子分类，子分类将同时删除）</span>", function () {
                    $.ajax({
                        type: "POST",
                        data: { action: "del", typeId: $(_this).attr("tip") },
                        dataType: "text",
                        success: function (msg) {
                            if (msg == "1") {
                                layerCommon.msg("操作成功", IconOption.笑脸, 1500, function () {
                                    location.reload();
                                });
                            } else {
                                layerCommon.msg(msg,IconOption.错误);
                            }
                        },
                        error: function () {
                            layerCommon.msg("删除失败，网络或服务器异常", IconOption.错误);
                        }
                    })
                }, "提示")
            })


        })

    </script>
    <style>
        .none {
            display:none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <!--当前位置 start-->
    <uc1:Header ID="Header1" runat="server" />
    <uc2:Left ID="Left1" runat="server" />
    <div class="m-con">
    <div class="m-place">
	        <i>位置：</i>
            <a href="../index.aspx" target="_top">我的桌面</a><i>></i>
            <a href="#">系统管理</a><i>></i>
            <a href="IndustryList.aspx">商品分类管理</a>
    </div>
    <!--当前位置 end-->
              <!--功能按钮 start-->
            <div class="tools">
                <ul class="toolbar left">
                    <li class="click"><span>
                        <img src="/Company/images/t01.png" /></span><font>新增一级类别</font></li>
                </ul>
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
                         <th>
                            操作
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptGTypeList" runat="server">
                        <ItemTemplate>
                            <tr id='<%# Eval("ID") %>' parentid='<%# Eval("ParentId")%>' style='height: 26px;width: 100%;'>
                             <td style="width:650px"> 
                                <img class="Openimg" height='9' src='../../Company/images/<%#Eval("SVdef3") == null?"menu_minus":"menu_plus"%>.gif' width='9'
                                   border='0' />&nbsp;   <%# Eval("TypeName")%> 

                             </td>
                            <td>
                                <div class="tcle" >
                                    <%# Eval("IsEnabled").ToString()=="True"?"启用":"禁用" %>
                                </div>
                            </td>
                            <td>
                                     <div class="tcle">
                                        <a href="javascript:;" tip='<%# Eval("Id") %>' pname='<%#Eval("TypeName") %>' class="TypeChildAdd">添加下级 |</a> 
                                        <a href="javascript:;" tip='<%# Eval("Id") %>' class="TypeEdit"   pname='<%#Eval("TypeName") %>'>编辑 | </a>
                                        <a href="javascript:;" tip='<%# Eval("Id") %>' class="TypeDel">移除</a>
                                    </div>
                            </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
  <!--信息列表 end-->

                <!--新增 start-->
        <div class="AddWindow tip" style="display: none;top:30%;">
            <div style="position: absolute; top: 0; left: 0; right: 0; bottom: 0; z-index: 16;
                background: #fff;">
                <div class="tiptop">
                    <span>新增</span><a></a>
                </div>
                <div class="tipinfo" style="z-index:99">
                    <div class="lb trParent" id="trParent" runat="server">
                        <span><i class="required">*</i>父类名称：</span>
                        <input name="lblTypeName" disabled="disabled" id="lblTypeName" type="text" runat="server"
                            class="textBox lblTypeName" maxlength="20" />
                        <input type="hidden" runat="server" id="hidePTypeId" />
                    </div>
                    <div class="lb">
                        <span><i class="required">*</i>分类名称：</span>
                        <input name="txtTypeName" id="txtTypeName" style="margin-left: 2px;" type="text"
                            runat="server" class="textBox txtTypeName" maxlength="20" />
                    </div>
                    <div class="tipbtn">
                        <asp:Button ID="btnAdd" CssClass="orangeBtn" runat="server" Text="确定" OnClientClick="return  formCheck();"
                            OnClick="btnAdd_Click" />&nbsp;
                        <input name="" type="button" class="cancel" value="取消" />
                    </div>
                    <div id='MsgT'>
                        <span><i class='required' style='margin-left: 20px; font-size: 12px; margin-top: 10px;'>
                            该分类下已有商品，添加子类会将其商品移动至新的分类下面，请确认！</i></span></div>
                </div>
            </div>
            <div id="xubox_border1" class="xubox_border" style="z-index: 11; opacity: 0.3; filter: Alpha(Opacity=30) !important;
                border-radius: 5px; top: -8px; left: -8px; right: -8px; bottom: -8px; background-color: rgb(0, 0, 0);
                position: absolute; top">
            </div>
        </div>
        <!--新增 end-->


                <!--编辑 start-->
        <div class="EditWindow tip" style="display: none;top:30%;">
            <div style="position: absolute; top: 0; left: 0; right: 0; bottom: 0; z-index: 999;
                background: #fff;">
                <div class="tiptop">
                    <span>编辑</span><a></a></div>
                <div class="tipinfo">
                    <asp:HiddenField ID="hideTypeId" runat="server" />
                    <div class="lb">
                        <span>*分类名称：</span>
                        <input name="txtTypeNames" id="txtTypeNames" runat="server" type="text" class="textBox txtTypeNames" maxlength="20" />
                    </div>
                    <div class="tipbtn">
                        <asp:Button ID="btnEdit" CssClass="orangeBtn" runat="server" Text="确定" OnClientClick="return formCheck();"
                            OnClick="btnEdit_Click" />&nbsp;
                        <input  type="button" class="cancel" value="取消" />
                    </div>
                </div>
            </div>
            <div id="xubox_border" class="xubox_border" style="z-index: 11; opacity: 0.3; filter: Alpha(Opacity=30) !important;
                border-radius: 5px; top: -8px; left: -8px; right: -8px; bottom: -8px; background-color: rgb(0, 0, 0);
                position: absolute; top">
            </div>
        </div>
        <!--编辑 end-->

    </div>
    </form>
</body>
</html>
