<%@ Page Language="C#" AutoEventWireup="true" validateRequest="false" CodeFile="GoodsList.aspx.cs" Inherits="Company_GoodsNew_GoodsList" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/CompRemove.ascx" TagPrefix="uc1" TagName="CompRemove" %>
<%@ Register Src="~/Company/UserControl/TreeDemo.ascx" TagPrefix="uc1" TagName="TreeDemo" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>商品列表</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/ImgAmplify.js" type="text/javascript"></script>
    <script src="../../js/ajaxfileupload.js" type="text/javascript"></script>
    <script src="../../js/MoveLayer.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
</head>
<body>
    <input type="hidden" id="hid_Alert" />
    <form id="form1" runat="server">
    <uc1:Top ID="top1" runat="server" ShowID="nav-3" />
    <uc1:CompRemove runat="server" ID="Remove" ModuleID="3" />
    <div class="rightinfo">
        <!--当前位置 start-->
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li>
                <li>></li>
                <li><a href="../GoodsNew/GoodsInfoList.aspx" runat="server" id="atitle">商品列表</a></li>
            </ul>
        </div>
        <!--当前位置 end-->
        <!--功能按钮 start-->
        <div class="tools">
            <ul class="toolbar left" style="margin-bottom: 5px">
                <li onclick="addGoods()" runat="server" id="btnliAdd"><span>
                    <img src="../images/t01.png" /></span><font>新增商品</font></li>
                <li onclick="Delete(1)" runat="server" id="btnliDel"><span>
                    <img src="../images/t03.png" /></span>删除</li>
                <li runat="server" id="sj" onclick="Delete(2)"><span>
                    <img src="../images/t12.png" /></span>上架</li>
                <li runat="server" id="xj" onclick="Delete(3)"><span>
                    <img src="../images/t11.png" /></span>下架</li>
                <li onclick="addList()" runat="server" id="btnnpoi" style="display: none;"><span>
                    <img src="../images/t14.png" /></span>Excel导入</li>
                
            </ul>
            <ul class="toolbar3 left" style="display: none">
                <li><a href="PicSpaceList.aspx" style="text-decoration: underline; color: Blue;">商品图片库</a></li>
            </ul>
            <div class="right">
                <ul class="toolbar right ">
                    <li onclick="return ChkPage()"><span>
                        <img src="../images/t04.png" /></span>搜索</li>
                    <%--<uc1:ToExcel runat="server" ID="ToExcel" contect="TbList" />--%>
                    <li id="export"><span>
                        <img src="../images/tp3.png" /></span>导出</li>
                    <li class="liSenior"><span>
                        <img src="../images/t07.png" /></span>高级</li>
                </ul>
                <ul class="toolbar3">
                    <li>商品名称:<input name="txtGoodsName" type="text" id="txtGoodsName" runat="server"
                        style="width: 110px" class="textBox txtGoodsName" maxlength="50" /></li>
                    <!--<li>订单金额(元):	<input name="" type="text" class="textBox2"/>-<input name="" type="text" class="textBox2"/></li>-->
                    <li>状态:<asp:DropDownList ID="ddlState" runat="server" Width="70px" CssClass="textBox">
                        <asp:ListItem Value="">全部</asp:ListItem>
                        <asp:ListItem Value="1">上架</asp:ListItem>
                        <asp:ListItem Value="0">下架</asp:ListItem>
                    </asp:DropDownList>
                    </li>
                </ul>
            </div>
        </div>
        <!--功能按钮 end-->
        <div class="hidden">
            <ul style="width: 900px;">
                <li>每页<input name="txtPageSize" type="text" class="textBox txtPageSize" id="txtPageSize"
                    style="width: 40px" runat="server" value="12" onkeyup="KeyInt(this);" onblur="KeyInt(this);" />&nbsp;条</li>
                <li>&nbsp;</li>
                <li>商品分类:<uc1:TreeDemo runat="server" ID="txtCategory" />
                </li>
                <li>&nbsp;</li>
                <%-- <li>是否店铺推荐:<asp:DropDownList ID="ddlRecommend" runat="server" Width="70px" CssClass="textBox">
                    <asp:ListItem Value="">全部</asp:ListItem>
                    <asp:ListItem Value="2">推荐</asp:ListItem>
                    <asp:ListItem Value="1">不推荐</asp:ListItem>
                </asp:DropDownList>
                </li>--%>
            </ul>
        </div>
        <!--信息列表 start-->
        <table class="tablelist" id="TbList">
            <thead>
                <tr>
                    <th class="t7">
                        <input type="checkbox" name="checkbox" id="CB_SelAll" onclick="SelectAll(this)" />
                    </th>
                    <th class="t6">
                        商品名称
                    </th>
                    <th class="t8">
                        图片
                    </th>
                    <th class="t1">
                        商品分类
                    </th>
                    <th align="center" class="t8">
                        单位
                    </th>
                    <th class="t1">
                        销售价格
                    </th>
                    <th align="center" class="t8">
                        状态
                    </th>
                  <% if ((bool)ViewState["IsPower"])
                       { %>
                    <th align="center" class="t8">
                        操作
                    </th>
                    <% } %>
                </tr>
            </thead>
            <tbody>
                  <asp:Repeater ID="rptGoods" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td align="center">
                                <asp:CheckBox ID="CB_SelItem" runat="server"></asp:CheckBox>
                                <asp:HiddenField ID="HF_Id" runat="server" Value='<%# Eval("ID") %>' />
                                <asp:HiddenField ID="HF_GoodsName" runat="server" Value='<%# Eval("Goodsname").ToString()%>' />
                            </td>
                            <td>
                                <div class="tcle">
                                    <a href="GoodsInfo.aspx?nextstep=<%=Request["nextstep"] %>&goodsId=<%# Common.DesEncrypt(Eval("ID").ToString(), Common.EncryptKey)%>" class="link"  tip='<%# Eval("ID") %>'
                                        title='<%# Eval("Goodsname").ToString()%>' >
                                        <%# Eval("Goodsname").ToString()%></a></div>
                            </td>
                            <td>
                                <div class="tc">
                                    <a class="tooltip" href="javascript:;" id='<%# GetPicURL(Eval("Pic2")) %>'
                                        style="display: inline-block;">
                                        <img id='GoodsImg' class='pic' alt="暂无" runat="server" width="42" height="42" src='<%# GetPicURL(Eval("Pic2")) %>' />
                                    </a>
                                </div>
                            </td>
                            <td>
                                <div class="tc">
                                    <%# GoodsCategory(Eval("CategoryID").ToString())%></div>
                            </td>
                            <td>
                                <div class="tc">
                                    <%# Eval("Unit").ToString()%></div>
                            </td>
                            <td>
                                <div class="tc">
                                    ￥<%# decimal.Parse(string.Format("{0:N2}", Eval("salePrice"))).ToString("#,##0.00")%></div>
                            </td>
                            <td>
                                <div class="tc">
                                    <%# Eval("isOffline").ToString()=="1"?"上架":"<span style='color:red;'>下架</span>" %></div>
                            </td>
                            <% if ((bool)ViewState["IsPower"])
                               { %>
                            <td>
                                <div class="tc">
                                    <a href="javascript:;" class="aedit" tip="<%# Eval("ID") %>">编辑</a>
                                </div>
                            </td>
                            <% } %>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <asp:Button ID="btnDel" runat="server" Text="删除" Style="display: none" OnClick="btnDel_Click" />
        <asp:Button ID="btnOffline" runat="server" Text="上架" Style="display: none" OnClick="btnOffline_Click" />
        <asp:Button ID="btnOffline2" runat="server" Text="下架" Style="display: none" OnClick="btnOffline2_Click" />
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
    <div class='Layer'>
    </div>
    <div class="tip" style="display: none;" id="DisImport">
        <div style="position: absolute; top: 0; left: 0; right: 0; bottom: 0; z-index: 999;
            background: #fff;">
            <div class="tiptop">
                <span>商品表格导入</span><a></a></div>
            <div class="tipinfo">
                <div class="lb">
                    <span><b class="hint">1</b> 模版下载： </span><a href="TemplateFile/商品表格导入模版.xls" style="text-decoration: underline"
                        target="_blank">商品表格导入模版.xls</a> <font color="red">（另存到本地电脑进行编辑并保存）</font>
                </div>
                <div class="lb">
                    <span><b class="hint">2</b> 上传文件：</span>
                    <asp:FileUpload ID="FileUpload1" runat="server" Style="width: 150px;" />
                    <font color="red">（选择刚下载并完成编辑的模版）</font>
                </div>
                <%--&nbsp; 即将开通，尽请期待！--%>
                <div class="tipbtn" style="margin-left: 155px">
                    <input type="button" id="btnAddList" class="orangeBtn" runat="server" value="确定"
                        onclick="if(!formChecks(this)){return false;}" onserverclick="btnAddList_Click" />&nbsp;
                    <input name="" type="button" class="cancel" value="取消" />
                </div>
            </div>
        </div>
        <div style="z-index: 11; opacity: 0.3; filter: Alpha(Opacity=30) !important; border-radius: 5px;
            top: -8px; left: -8px; right: -8px; bottom: -8px; background-color: rgb(0, 0, 0);
            position: absolute; top">
        </div>
    </div>
    <style>
        /*鼠标移动：图片预览*/
        .aImg
        {
            width: 45px;
            height: 40px;
        }
        /*border: 1px solid #00adf2;*/
        .pic
        {
            width: 45px;
            height: 40px;
        }
        #tooltip
        {
            position: absolute;
            border: 1px solid #ccc;
            background: #333;
            padding: 2px;
            display: none;
            color: #fff;
            max-width: 452px;
            max-height: 424px;
        }
    </style>
    <script type="text/javascript">
      var div = $('<div   Msg="True"  style="z-index:10; background-color:#000; opacity:0.3; filter:alpha(opacity=50);position: fixed;width:100%;height:100%;top:0px;left:0px;"></div>');
        $('.tablelist tbody tr:odd').addClass('odd');
        $(document).ready(function () {
            if ('<%=Request["nextstep"] %>' == "1") {
                Orderclass("ktxzsp");
                document.getElementById("imgmenu").style.display = "block";
            }
                 $(".liSenior").on("click", function () {
        $("div.hidden").slideToggle(100);
    })
        })
        $(function () {
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("#btnSearch").trigger("click");
                }
            });
            $($(".tiptop")[1]).LockMove({ MoveWindow: "#DLodIMG" });
            //移动图片展示
            $("a.tooltip").ImgAmplify();
            //编辑
            $(".aedit").click(function () {
                var id = $(this).attr("tip");
                if ('<%=Request["nextstep"] %>' == "1") {
                    location.href = "GoodsEdit.aspx?nextstep=1&KeyID=" + id;
                }
                else {
                    location.href = "GoodsEdit.aspx?KeyID=" + id;
                }
            })

              //导出
            $("#export").click(function () {
                 var str = "";
                 if($.trim($("#txtGoodsName").val())!="")
                    str+="and g.GoodsName like '%"+$.trim($("#txtGoodsName").val())+"%'";
                 if($.trim($("#ddlState").val())!="")
                    str+="and info.IsOffline="+$.trim($("#ddlState").val());
                 if($.trim($("#ddlRecommend").val())!="")
                    str+="and g.IsRecommended="+$.trim($("#ddlRecommend").val());
                 if($.trim($("#txtCategory_txt_product_class").val())!="")
                    str+="and c.CategoryName like '%"+$.trim($("#txtCategory_txt_product_class").val())+"%'";

                window.location.href = '../../../ExportExcel.aspx?intype=3&searchValue='+str+'&p=' + $("#txtPageSize").val() + '&c=<%=Pager.CurrentPageIndex %>';
            });

            //关闭导入层
             $("input.cancel,.tiptop a").on("click", function () {
                $(div).remove();
                $("div#DisImport").fadeOut(200);
            });
        });
               //详细商品
        function InfoData(id) {
            location.href = "GoodsInfo.aspx?nextstep=<%=Request["nextstep"] %>&goodsId=" + id;
        }
        //新增商品信息
        function addGoods() {
            if ('<%=Request["nextstep"] %>' == "1") {
                location.href = "GoodsEdit.aspx?nextstep=1";
            }
            else {
                location.href = "GoodsEdit.aspx";
            }
        }
        // 每页显示数量非空验证
        function ChkPage() {
            if ($("#txtPageSize").val() == "") {
                       layerCommon.msg("每页显示数量不能为空", IconOption.错误);
                return false;
            } else if (parseInt($("#txtPageSize").val()) == 0) {
                       layerCommon.msg("每页显示数量必须大于0", IconOption.错误);
                return false;
            } else {
                $("#btnSearch").click();
            }
            return true;
        }
        //删除
        function Delete(type) {
            var bol = false;
            var chklist = $(".tablelist tbody input[type='checkbox']");
            $(chklist).each(function (index, obj) {
                if (obj.checked) {
                    bol = true;
                    return false;
                }
            })
            if (type == 1) {
                if (bol) {
                    layerCommon.confirm('确定要删除商品', function () {
                        $("#btnDel").click();
                    });

                } else {
                       layerCommon.msg("请勾选需要删除的商品", IconOption.错误);
                    return false;
                }
            } else if (type == 2) {
                //上架
                if (bol) {
                     layerCommon.confirm('确定要上架商品', function () {
                        $("#btnOffline").click();
                    });

                } else {
                         layerCommon.msg("请勾选需要上架的商品", IconOption.错误);
                    return false;
                }
            } else if (type == 3) {
                //下架
                if (bol) {
                     layerCommon.confirm('确定要下架商品', function () {
                        $("#btnOffline2").click();
                    });

                } else {
                         layerCommon.msg("请勾选需要下架的商品", IconOption.错误);
                    return false;
                }
            }
        }
           //验证
        function formChecks(obj) {
            var str = $("#FileUpload1").val();
            if (str == "") {
                                layerCommon.msg("请选择要导入商品的Excel文件", IconOption.错误);
                return false;
            }
            var suffix = $.trim(str.substring(str.lastIndexOf(".")));
            if (suffix == ".xlsx" || suffix == ".xls") {
                $(obj).attr("disabled", "disabled");
                return true;
            } else {
                  layerCommon.msg("请选择ExcelL文件", IconOption.错误);
                return false;
            }
        }
        //批量导入
        function addList() {
       location.href="../ImportGoods.aspx";
            //$("body").append(div);
           // $("div#DisImport").css("width", "500px").fadeIn(200);
        }

        function addlis(a,b,c){
            layerCommon.alert("共" + parseInt(parseInt(a) + parseInt(b)) + "条数据,成功导入" + a + "条数据" + c + "", IconOption.正确,5000,function(){ window.location=window.location;});

            //window.location.href='GoodsInfoList.aspx?nextstep=<%=Request["nextstep"] %>';
        }
          //禁用Enter键表单自动提交  
        document.onkeydown = function (event) {
            var target, code, tag;
            if (!event) {
                event = window.event; //针对ie浏览器  
                target = event.srcElement;
                code = event.keyCode;
                if (code == 13) {
                    tag = target.tagName;
                    if (tag == "TEXTAREA") { return true; }
                    else { return false; }
                }
            }
            else {
                target = event.target; //针对遵循w3c标准的浏览器，如Firefox  
                code = event.keyCode;
                if (code == 13) {
                    tag = target.tagName;
                    if (tag == "INPUT") { return false; }
                    else { return true; }
                }
            }
        }
    </script>
    </form>
</body>
</html>
