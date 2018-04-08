<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DisList.aspx.cs" Inherits="Company_SysManager_DisList" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/TreeDisArea.ascx" TagPrefix="uc1" TagName="TreeDisArea" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<%@ Register Src="~/Company/UserControl/CompRemove.ascx" TagPrefix="uc1" TagName="CompRemove" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>代理商列表</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/MoveLayer.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script>
        $(function () {
            $(".showDiv2 .ifrClass").css("width", "155px");
            $(".showDiv2").css("width", "150px");

            $('iframe').load(function () {
                $('iframe').contents().find('.pullDown').css("width", "150px");
            });
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("#btn_Search").trigger("click");
                }
            })
        })
        var div = $('<div   Msg="True"  style="z-index:10; background-color:#000; opacity:0.3; filter:alpha(opacity=50);position: fixed;width:100%;height:100%;top:0px;left:0px;"></div>');

        $(document).ready(function () {

            //table 行样式
            $('.tablelist tbody tr:odd').addClass('odd');

            if ('<%=Request["nextstep"] %>' == "1") {
                Orderclass("ktxzjxs");
                document.getElementById("imgmenu").style.display = "block";
            }

            //导出
            $("#ToExcel").click(function () {
                var str = "";

                //if ($.trim($(".hid_AreaId").val()) != "")
                //    str += " and areaid='" + $.trim($(".hid_AreaId").val()) + "'";
                if ($.trim($("#txtDisName").val()) != "")
                    str += "and dis.DisName like '%" + $.trim($("#txtDisName").val()) + "%'";
                if ($.trim($("#txtPrincipal").val()) != "")
                    str += "and dis.Principal like '%" + $.trim($("#txtPrincipal").val()) + "%'";
                if ($.trim($("#txtPhone").val()) != "")
                    str += " and dis.Phone like '%" + $.trim($("#txtPhone").val()) + "%'";
                if ($.trim($("#ddlAUState").val()) != "-1")
                    str += " and dis.AuditState='" + $.trim($("#ddlAUState").val()) + "'";
                if ($.trim($("#ddlState").val()) != "-1")
                    str += " and dis.IsEnabled='" + $.trim($("#ddlState").val()) + "'";
                if ($.trim($("#txtCreateDate").val()) != "")
                    str += " and dis.createdate>='" + $.trim($("#txtCreateDate").val()) + "'";
                if ($.trim($("#txtEndCreateDate").val()) != "") {
                    var dtime = $("#txtEndCreateDate").val()
                    // 转换日期格式
                    dtime = dtime.replace(/-/g, '/'); // "2010/08/01";
                    // 创建日期对象
                    var date = new Date(dtime);
                    // 加一天
                    date.setDate(date.getDate() + 1);
                    var dateTime = date.getFullYear() + "-" + (date.getMonth() + 1).toString() + "-" + (date.getDate()).toString();
                    str += " and dis.createdate>='" + dateTime + "' ";
                }

                window.location.href = '../../../ExportExcel.aspx?intype=4&searchValue=' + str + '&p=' + $("#txtPageSize").val() + '&c=<%=Pager.CurrentPageIndex %>';
            });

            $("li#liSearch").on("click", function () {
                $("#btn_Search").trigger("click");
            })

            //            $("li#libtnEdit").on("click", function () {
            //                if ($(this).data("isEnble") == false) {
            //                    location.href = "DistributorAddEdit.aspx?id=" + $(".tablelist tbody input[type=checkbox]:checked").val();
            //                }
            //            })

            //            $(".tablelist  input:checkbox").on("click", function (e) {
            //                var len = $(".tablelist tbody input[type=checkbox]:checked").length;
            //                if (len == 1) {
            //                    $("li#libtnEdit").removeClass("Enbled");
            //                    $("li#libtnEdit").data("isEnble", false);
            //                }
            //                else {
            //                    $("li#libtnEdit").addClass("Enbled");
            //                    $("li#libtnEdit").data("isEnble", true);
            //                }
            //            })

            //        function Isenble() {
            //            var len = $(".tablelist tbody input[type=checkbox]:checked").length;
            //            if (len == 1) {
            //                $("li#libtnEdit").removeClass("Enbled");
            //                $("li#libtnEdit").data("isEnble", false);
            //            }
            //            else {
            //                $("li#libtnEdit").addClass("Enbled");
            //                $("li#libtnEdit").data("isEnble", true);
            //            }
            //        }

            $("a.bulk").on("click", function () {
                $("body").append(div);
                $("div#DisImport").css("width", "500px").fadeIn(200);
            })
            $("input.cancel,.tiptop a").bind("click", function (event) {
                $(div).remove();
                $("div#DisImport").fadeOut(200);
            })
            $("div.tiptop").LockMove({ MoveWindow: "#DisImport" });
            //add by hgh 
            if ('<%=Request["nextstep"] %>' == "1") {
                Orderclass("ktxzjxs");
                document.getElementById("imgmenu").style.display = "block";
            }
        });

        function formChecks(obj) {
            var str = $("#FileUpload1").val();
            if (str == "") {
                layerCommon.msg("请选择要导入代理商Excel的文件", IconOption.错误);
                return false;
            }
            var suffix = $.trim(str.substring(str.lastIndexOf(".")));
            if (suffix == ".xlsx" || suffix == ".xls") {
                $(obj).attr("disabled", "disabled");
                return true;
            } else {
                layerCommon.msg("请选择ExcelL文件", IconOption.错误)
                return false;
            }
        }

        function addList() {
            // $("body").append(div);
            //  $("div#DisImport").css("width", "500px").fadeIn(200);
            location.href = "../ImportDis.aspx";
        }

        function addDis() {
            window.location.href = '../SysManager/DisEdit.aspx';
        }
    </script>
    <style>
        i[error]
        {
            color: Red;
            font-weight: bold;
            font-family: '微软雅黑';
            font-size: 16px;
        }
        .batch i
        {
            font-style: normal;
        }
        .batch
        {
            margin: 0px auto;
            width: 1000px;
            height: 275px;
            position: absolute;
            top: 50%;
            left: 50%;
            margin: -120px 0 0 -500px;
            overflow: hidden;
        }
        .batch a
        {
            width: 490px;
            height: 275px;
            background: url(../images/batchImages1.jpg) no-repeat 0 0;
            position: relative;
            cursor: pointer;
            display: block;
            float: left;
        }
        .bulk i
        {
            position: absolute;
            top: 133px;
            color: #999;
            left: 215px;
        }
        .batch .alone
        {
            background: url(../images/batchImages2.jpg) no-repeat 0 0;
            float: right;
        }
        .alone i
        {
            position: absolute;
            top: 133px;
            color: #999;
            left: 227px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:CompRemove runat="server" ID="Remove" ModuleID="2" />
    <uc1:Top ID="top1" runat="server" ShowID="nav-4" />
    <div class="rightinfo">
        <!--当前位置 start-->
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li>
                <li>></li>
                <li><a href="../SysManager/DisList.aspx" runat="server" id="atitle">代理商列表</a></li>
            </ul>
        </div>
        <!--当前位置 end-->
        <!--功能按钮 start-->
        <div class="tools">
            <ul class="toolbar left">
                <li onclick="addDis()" runat="server" id="btnAdd"><span>
                    <img src="../images/t01.png"></span><font>新增</font></li>
                <li onclick="addList()" runat="server" id="btnnpoi"><span>
                    <img src="../images/t14.png"></span>Excel导入</li>
                <li id="libtnNext" runat="server" style="display: none;"><a href="javascript:void(0);"
                    onclick="onlinkOrder('../GoodsNew/GoodsEdit.aspx?nextstep=1','spxz')"><span>
                        <img src="../images/t07.png" /></span><font color="red">下一步</font></a></li>
            </ul>
            <div class="right">
                <ul class="toolbar right">
                    <li id="liSearch"><span>
                        <img src="../images/t04.png" /></span>搜索</li>
                    <%--<uc1:ToExcel runat="server" ID="ToExcel" contect="TbDisList"/>--%>
                    <li runat="server" id="ToExcel"><span>
                        <img src="../images/tp3.png" /></span>导出</li>
                    <li class="liSenior"><span>
                        <img src="../images/t07.png" /></span>高级</li>
                </ul>
                <input type="button" runat="server" id="btn_Search" style="display: none;" onserverclick="btn_SearchClick" />
                <ul class="toolbar3">
                    <li>代理商名称:<input runat="server" id="txtDisName" type="text" class="textBox" />&nbsp;&nbsp;</li>
                    <li>联系人:<input runat="server" id="txtPrincipal" type="text" class="textBox" />&nbsp;&nbsp;</li>
                    <li>审核状态:<asp:DropDownList runat="server" ID="ddlAUState" CssClass="downBox">
                        <asp:ListItem Value="-1" Text="全部"></asp:ListItem>
                        <asp:ListItem Value="2" Text="已审"></asp:ListItem>
                        <asp:ListItem Value="0" Text="未审"></asp:ListItem>
                    </asp:DropDownList>
                        &nbsp;</li>
                </ul>
            </div>
        </div>
        <!--功能按钮 end-->
        <div class="hidden">
            <ul style="">
                <li>每页<input name="txtPageSize" type="text" style="width: 40px;" class="textBox txtPageSize"
                    id="txtPageSize" runat="server" value="12" onkeyup="KeyInt(this);" onblur="KeyInt(this);" />
                    &nbsp;条</li>
                <li>启用:<asp:DropDownList runat="server" ID="ddlState" CssClass="downBox">
                    <asp:ListItem Value="-1" Text="全部"></asp:ListItem>
                    <asp:ListItem Value="1" Text="启用"></asp:ListItem>
                    <asp:ListItem Value="0" Text="禁用"></asp:ListItem>
                </asp:DropDownList>
                    &nbsp;&nbsp;</li>
                <li>入驻时间:<input name="txtCreateDate" runat="server" onclick="WdatePicker()" style="width: 115px;"
                    id="txtCreateDate" readonly="readonly" type="text" class="Wdate" value="" />&nbsp;-&nbsp;
                    <input name="txtEndCreateDate" runat="server" onclick="WdatePicker()" style="width: 115px;"
                        id="txtEndCreateDate" readonly="readonly" type="text" class="Wdate" value="" />&nbsp;&nbsp;
                </li>
                <%--<li>区域:<uc1:TreeDisArea runat="server" ID="txtDisArea" />
                    &nbsp;&nbsp;</li>--%>
                <li>联系手机:<input runat="server" id="txtPhone" type="text" class="textBox" />&nbsp;&nbsp;</li>
            </ul>
        </div>
        <!--信息列表 start-->
        <table class="tablelist" id="TbDisList">
            <thead runat="server">
                <tr>
                    <th class="">
                        代理商名称
                    </th>
                    <th class="t5">
                        代理商分类
                    </th>
                    <th class="t5">
                        代理商区域
                    </th>
                    <th class="t1">
                        审核状态
                    </th>
                    <th class="t5">
                        入驻时间
                    </th>
                    <th class="t1">
                        是否启用
                    </th>
                    <th class="t5">
                        联系人
                    </th>
                    <th class="t5">
                        联系手机
                    </th>
                </tr>
            </thead>
            <tbody runat="server">
                <asp:Repeater ID="Rpt_Distribute" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <div class="tcle">
                                    <a style="text-decoration: underline;" href='DisInfo.aspx?nextstep=<%=Request["nextstep"] %>&KeyID=<%# Common.DesEncrypt(Eval("Id").ToString(), Common.EncryptKey)%>&type=2'>
                                        <%# Eval("DisName") %></a></div>
                            </td>
                            <td>
                                <div class="tc">
                                    <%# Common.GetDisTypeNameById(Eval("ID").ToString().ToInt(0),Eval("compID"))%></div>
                            </td>
                            <td>
                                <div class="tc">
                                    <%# Common.GetDisAreaNameById(Eval("ID").ToString().ToInt(0), Eval("compID"))%></div>
                            </td>
                            <td>
                                <div class="tc">
                                    <%# Eval("AuditState").ToString() == "2" ? "已审" : "<span style='color:red'>未审</span>"%></div>
                            </td>
                            <td>
                                <div class="tc">
                                    <%# Common.GetDateTime(Eval("CreateDate").ToString()==""?new DateTime():Eval("CreateDate").ToString().ToDateTime(), "yyyy-MM-dd")%></div>
                            </td>
                            <td>
                                <div class="tc">
                                    <%# Eval("IsEnabled").ToString() == "1" ? "启用" : "<span style='color:red'>禁用</span>"%></div>
                            </td>
                            <td>
                                <div class="tc">
                                    <%# Eval("Principal")%>
                            </td>
                            <td>
                                <div class="tc">
                                    <%# Eval("phone")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
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
    <div class="tip" style="display: none;" id="DisImport">
        <div style="position: absolute; top: 0; left: 0; right: 0; bottom: 0; z-index: 999;
            background: #fff;">
            <div class="tiptop">
                <span>代理商表格导入</span><a></a></div>
            <div class="tipinfo">
                <div class="lb">
                    <span><b class="hint">1</b> 模版下载： </span><a href="TemplateFile/代理商表格导入模版.xls" style="text-decoration: underline"
                        target="_blank">代理商表格导入模版.xls</a> <font color="red">（另存到本地电脑进行编辑并保存）</font>
                </div>
                <div class="lb">
                    <span><b class="hint">2</b> 上传文件：</span>
                    <asp:FileUpload ID="FileUpload1" runat="server" Style="width: 150px;" />
                    <font color="red">（选择刚下载并完成编辑的模版）</font>
                </div>
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
    </form>
</body>
</html>
