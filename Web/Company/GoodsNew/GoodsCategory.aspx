<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GoodsCategory.aspx.cs" Inherits="Company_SysManager_GoodsCategory" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>商品分类</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="TreeViewOpen.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
    <script src="../js/classifyview.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
    <script type="text/javascript">
        //enter搜索
        $(function () {
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
                } else {
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
            };

                        //绑定商品分类
            $("#txtGtype").click(function () {
                var dataviews=<%=GoodsType%>
               handleChange(this,dataviews);
            });

             $("#txtGtype2").click(function () {
                var dataviews=<%=GoodsType%>
               handleChange(this,dataviews);
            });
        });

        $(document).ready(function () {
            if ($("tr").length <= 50) {
                $("a[class='TypeChildAdd'][pname='三级']").each(function () {
                    $(this).css("visibility", "hidden");
                });
            }

            var div = $('<div   Msg="True"  style="z-index:1000; background-color:#000; opacity:0.3; filter:alpha(opacity=30);position: fixed;width:100%;height:100%;top:0px;left:0px;"></div>');

            //新增一级分类
            $(document).on("click", ".tools li.click", function () {
                $("#trCate").show();
                $("#MsgT").hide();
                $(".AddWindow div.trParent").hide();
                $("body").append(div);
                $(".AddWindow #<%=hideTypeId.ClientID %>").val("0");
                $(".AddWindow").css("height", "240px").fadeIn(200);
            });

            //添加下级分类：
            $(document).on("click", ".tablelist .TypeChildAdd", function () {
                $("html,body").animate({ scrollTop: 0 }, 700);
                $("#ddlGoodsType").attr("Enabled", "false");
                $("#trCate").css("display", "none");

                $(".AddWindow #<%=lblTypeName.ClientID %>").val($(this).attr("Pname"));
                $(".AddWindow #<%=hideTypeId.ClientID %>").val($(this).attr("tip"));
                $(".AddWindow div.trParent").show();
                $("body").append(div);
                $(".AddWindow").css("height", "280px").fadeIn(200);

                $.ajax({
                    type: "post",
                    url: "GoodsCategory.aspx",
                    dataType: "text",
                    async: false,
                    data: { Action: "Msg", Id: $(this).attr("tip") },
                    success: function (data) {
                        var json = eval('(' + data + ')');
                        if (json == "0") {
                            $("#MsgT").css("display", "none");
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                    }
                });
            });

            //取消
            $(document).on("click", ".AddWindow .tiptop a,.EditWindow .tiptop a,.tipbtn .cancel", function () {
                $(div).remove();
                $(".EditWindow,.AddWindow").fadeOut(100);

                //清除商品分类的数据，包括商品大类，分类名称，隐藏空间的值
                $("#ddlGoodsType").val("");
                $("#hideGoodsType").val("");
                $("#txtTypeName").val("");
                $("#lblTypeName").val("");
                $("#hideTypeId").val("");
            });

            //编辑
            $(document).on("click", ".tablelist .TypeEdit", function () {
                $("html,body").animate({ scrollTop: 0 }, 700);
                var pid = $(this).parents("tr").attr("parentid")
                $("#trCate2").css("display", "none");
                var gid = $(this).attr("gid");
                $("#hid_txtGtype2").val("0")
                if (pid=="0")
                {
                    $("#txtGtype2").val("请选择")
                    $.ajax({
                        type: "post",
                        dataType: "text",
                        data: { Action: "Gtype", Gid: gid },
                        success: function (data) {
                            var jsonstr = JSON.parse(data)
                            $("#hid_txtGtype2").val(jsonstr.ID)
                            $("#txtGtype2").val(jsonstr.TypeName)
                        }
                    
                    })
                    $("#trCate2").css("display", "block");
                }
               
                $(".EditWindow #<%=hideTypeIds.ClientID %>").val($(this).attr("tip"));
                $(".EditWindow #<%=txtTypeNames.ClientID %>").val($(this).attr("Pname"));
                $(".EditWindow #<%=txtSortIndexs.ClientID %>").val($(this).attr("sortid"));
                $("body").append(div);
                $(".EditWindow").css("height", "240px").fadeIn(200);

            });

        });

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
                layerCommon.msg(str, IconOption.错误, 2000);
                return false;
            }
        }
    
    </script>
    <script src="TreeViewAspx.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
    <style type="text/css">
        .span
        {
            margin: 0;
            padding: 0;
            display: inline;
        }
        @media (min-width:1px) and (max-width: 500px)
        {
            .tablelist
            {
                display: none;
                
            }
        }
        .pop-menu {
            z-index:1000000000;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <uc1:Top ID="top1" runat="server" ShowID="nav-3" />
        <div class="rightinfo" id="isright" runat="server" >
            <!--当前位置 start-->
            <div class="place" id="isShow" runat="server">
                <ul class="placeul">
                    <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
                    <li><a href="../GoodsNew/GoodsCategory.aspx?type=2">商品分类</a></li>
                </ul>
            </div>
            <!--当前位置 end-->
            <!--功能按钮 start-->
            <div class="tools">
                <ul class="toolbar left">
                    <li class="click"><span>
                        <img src="../images/t01.png" /></span><font>新增一级类别</font></li>
                </ul>
                <div class="right" style="display: none;">
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
                        <th>
                            操作
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptDisTypeList" runat="server">
                        <ItemTemplate>
                            <tr id='<%# Eval("ID") %>' parentid='<%# Eval("ParentId")%>' style='height: 26px; width: 100%;' isopen="1" num="<%# Count%>" level="<%# Eval("Deep")%>">
                                <td>
                                    <div class="tcle"> 
                                        <img id="Openimg" height='9' src='<%# Simage(Eval("ID").ToString())%>' width='9' border='0' />&nbsp;
                                        <span class="span"> <%# Eval("CategoryName")%>
                                          <font style="color:red;"><%#Eval("ParentId").ToString()=="0"?GetGtypeName(Eval("GoodsTypeID").ToString()):"" %></font>
                                        </span>
                                    </div>
                                </td>
                                <td>
                                    <div class="tcle">
                                        <a href="javascript:;" tip='<%# Eval("Id") %>' pname='<%#Eval("CategoryName") %>' class="TypeChildAdd">添加下级 |</a> 
                                        <a href="javascript:;" tip='<%# Eval("Id") %>' sortid='<%#Eval("SortIndex") %>'   class="TypeIndex">上移</a> | 
                                        <a href="javascript:;" tip='<%# Eval("Id") %>' sortid='<%#Eval("SortIndex") %>'   class="TypeIndexDown">下移</a> | 
                                        <a href="javascript:;" tip='<%# Eval("Id") %>' sortid='<%#Eval("SortIndex") %>'   class="TypeEdit"  gid="<%#Eval("GoodsTypeID")%>""  pname='<%#Eval("CategoryName") %>'>编辑</a> | 
                                        <a href="javascript:;" tip='<%# Eval("Id") %>' class="TypeDel">移除</a>
                                    </div>
                                </td>
                           </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
            <!--信息列表 end-->
        </div>
        <!--新增 start-->
        <div class="AddWindow tip" style="display: none;">
            <div style="position: absolute; top: 0; left: 0; right: 0; bottom: 0; z-index: 16;
                background: #fff;">
                <div class="tiptop">
                    <span>新增</span><a></a>
                </div>
                <div class="tipinfo" style="z-index:99">
                    <div class="lb" id="trCate" style="z-index:99">
                        <span><i class="required">*</i>商品系统大类：</span>
                        <input type="hidden" id="hid_txtGtype" class="hid_txt_product_class" runat="server" />
                         <input name="txtGtype" readonly="readonly"  id="txtGtype" type="text" runat="server" class="textBox txtGtype" maxlength="20" />
    	                 <div class="pop-menu" style="width:605px; display:none;z-index:100">
    	</div>
                        <input type="hidden" runat="server" class="hideGoodsType" id="hideGoodsType" />
                    </div>
                    <div class="lb trParent" id="trParent" runat="server">
                        <span><i class="required">*</i>父类名称：</span>
                        <input name="lblTypeName" disabled="disabled" id="lblTypeName" type="text" runat="server"
                            class="textBox lblTypeName" maxlength="20" />
                        <%--<asp:HiddenField ID="hideTypeId" runat="server" />--%>
                        <input type="hidden" runat="server" id="hideTypeId" />
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
        <div class="EditWindow tip" style="display: none;">
            <div style="position: absolute; top: 0; left: 0; right: 0; bottom: 0; z-index: 999;
                background: #fff;">
                <div class="tiptop">
                    <span>编辑</span><a></a></div>
                <div class="tipinfo">
                    <asp:HiddenField ID="hideTypeIds" runat="server" />

                   <div class="lb" id="trCate2" style="z-index:99;display:none">
                        <span><i class="required">*</i>商品大类：</span>
                        <input type="hidden" id="hid_txtGtype2" class="hid_txt_product_class" runat="server" />
                         <input name="txtGtype" readonly="readonly"  id="txtGtype2" type="text" runat="server" class="textBox txtGtype" maxlength="20" />
    	                 <div class="pop-menu" style="width:605px; display:none;z-index:100">
    	               </div>
                        <input type="hidden" runat="server" class="hideGoodsType" id="Hidden2" />
                   </div>

                    <div class="lb">
                        <span>*分类名称：</span>
                        <input name="txtTypeNames" id="txtTypeNames" runat="server" type="text" class="textBox txtTypeNames" maxlength="20" />
                    </div>
                    <div class="lb" style="display: none">
                        <span>分类编码：</span>
                        <input name="txtTypecodes" id="txtTypecodes" type="text" runat="server" class="textBox txtTypecodes" maxlength="20" />
                    </div>
                    <div class="lb" style="display: none">
                        <span>*排序：</span>
                        <input name="txtSortIndexs" id="txtSortIndexs" runat="server" type="text" class="textBox"
                            onkeyup="KeyInt(this);" onblur="KeyInt(this);" maxlength="5" /></div>
                    <div class="tipbtn">
                        <asp:Button ID="btnEdit" CssClass="orangeBtn" runat="server" Text="确定" OnClientClick="return formCheck();"
                            OnClick="btnEdit_Click" />&nbsp;
                        <input name="" type="button" class="cancel" value="取消" />
                    </div>
                </div>
            </div>
            <div id="xubox_border" class="xubox_border" style="z-index: 11; opacity: 0.3; filter: Alpha(Opacity=30) !important;
                border-radius: 5px; top: -8px; left: -8px; right: -8px; bottom: -8px; background-color: rgb(0, 0, 0);
                position: absolute; top">
            </div>
        </div>
        <!--编辑 end-->
        <asp:HiddenField runat="server" ID="HiddenUp" />
        <asp:HiddenField runat="server" ID="HiddenDown" />
        <script type="text/javascript">
            //收缩插件
            $(document).ready(function () {
                $("#tablelist").TreeViewOpen(
                    {
                        imgID: "Openimg",
                        UpSrc: "../images/menu_plus.gif",
                        DownSrc: "../images/menu_minus.gif"
                    });
            });
            $('.tablelist tbody tr:odd').addClass('odd');
        </script>
    </div>
    </form>
</body>
</html>
