<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CMerchantsAdd.aspx.cs" Inherits="Company_CMerchants_CMerchantsAdd" %>

<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>招商信息</title>
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/CitysLine/JQuery-Citys-online-min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/OpenJs.js"></script>
    <script src="../../js/UploadJs.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../js/classifyview.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../js/autoTextarea.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
        <uc1:Top ID="top1" runat="server" ShowID="nav-2" />
        <input type="hidden" id="hid_Alert" />
        <input type="hidden" id="hidCompID" runat="server"/>
        <div class="rightinfo">
            <div class="info">
                <a href="../jsc.aspx">我的桌面</a>><a href="javascript:;">招商信息编辑</a>
            </div>
            <ul class="coreInfo">
                <li class="lb fl">
                    <i class="name">招商编码</i>
                    <%--<input type="text" class="box1" placeholder="" runat="server" maxlength="50" id="txtCMCode"/>--%>
                    <i>（自动生成）</i>
                </li>
                <li class="lb fl">
                    <i class="name"><i class="red">*</i>招商名称</i>
                    <input type="text" maxlength="50" style="margin-left:2px" class="box1" id="txtCMName" name="txtCMName" runat="server" />
                </li>
                <li class="lb fl">
                    <i class="name"><i class="red">*</i>商品编码</i>
                    <input type="text" class="box1" placeholder="" runat="server" maxlength="50" id="txtGoodsCode"  readonly="readonly"/>
                    <input type="hidden" runat="server" id="hidGoodsID"/>
                    <a class="opt-i addgoods"></a>
                </li>
                <li class="lb fl">
                    <i class="name">商品分类</i>
                    <input type="text" maxlength="50" style="margin-left:2px" class="box1" id="txtCategoryID" name="txtCategoryID" readonly="readonly" runat="server" />
                    <input type="hidden" runat="server" id="hidCategoryID"/>
                </li>
                <li class="lb fl">
                    <i class="name">商品名称</i>
                    <input type="text" class="box1" placeholder="" runat="server" maxlength="50" readonly="readonly" id="txtGoodsName"/>
                </li>
                <li class="lb fl">
                    <i class="name">规格型号</i>
                    <input type="text" maxlength="50" style="margin-left:2px" class="box1" readonly="readonly" id="txtValueInfo" name="txtValueInfo" runat="server" />
                </li>
                <li class="lb fl">
                    <i class="name"><i class="red">*</i>生效日期</i>
                    <input type="text" class="box1" placeholder="" runat="server" maxlength="50" id="txtForceDate" readonly="readonly" onclick="WdatePicker()"/>
                </li>
                <li class="lb fl">
                    <i class="name"><i class="red">*</i>失效日期</i>
                    <input type="text" maxlength="50" style="margin-left:2px" class="box1" id="txtInvalidDate" runat="server" readonly="readonly" onclick="WdatePicker()"/>
                </li>
                <li class="lb fl">
                    <i class="name">类型</i>
                    <select id="ddrtype"  style=" width:380px;" class="box1 p-box"  runat="server">
                        <option value="1">公开</option>
                        <option value="2">指定区域</option>
                        <option value="3">指定代理商</option>
                    </select>
                </li>
                <li class="lb fl none" id="selType" runat="server">
                    <i class="name">指定选择</i>
                    <input type="text" class="box1" readonly="readonly" runat="server" maxlength="50" id="txtFC"/>
                    <input type="hidden" runat="server" id="hidfc" />
                    <a class="opt-i addFc"></a>
                </li>
                <li class="lb fl">
                    <i class="name">需提供资料</i>
                    <input name="chk" value="1" id="chk1" runat="server" type="checkbox"/><label for="chk1">营业执照</label>
                    <input name="chk" value="2" id="chk2" runat="server" type="checkbox"/><label for="chk2">医疗器械经营许可证</label>
                    <input name="chk" value="3" id="chk3" runat="server" type="checkbox"/><label for="chk3">开户许可证</label>
                    <input name="chk" value="4" id="chk4" runat="server" type="checkbox"/><label for="chk4">医疗器械备案</label>
                </li>
                <li class="lb fl">
                    <i class="name">备 注</i>
                    <%--<input type="text" style="margin-left:3px"  class="box1" id="txtRemark" maxlength="20" runat="server" />--%>
                    <textarea id="OrderNote" runat="server" name="OrderNote" maxlength="200" class="box1" style="width:650px; height:80px;" placeholder="招商介绍等信息，不能超过200个字"></textarea>
                </li>
            </ul>

            <div class="btn-box">
	            <div class="btn">
                <a href="javascript:;" class="btn-area" id="btnAdds">提交</a>
                <a href="javascript:;" class="gray-btn" onclick="javascript:history.go(-1);">取消</a></div>
                 <asp:Button ID="btnAdd" CssClass="" runat="server"  OnClientClick="return formCheck()" OnClick="btnAdd_Click" />
	            <div class="bg"></div>
            </div>
        </div>
    </form>
    <script type="text/javascript">
        $(function () {
            //提交按钮单机事件
            $("#btnAdds").click(function () {
                $("#<%=btnAdd.ClientID%>").click();
            });

            $(document).on("click", ".addgoods", function () {
                var url = 'SelectGoods.aspx?type=1';
                var index = layerCommon.openWindow("选择商品", url, '880px', '600px'); //记录弹出对象
                $("#hid_Alert").val(index); //记录弹出对象
            });

            $(document).on("change", "#ddrtype", function () {
                var PageAction = $(this).val();
                var compid = $("#hidCompID").val();
                $("#txtFC").val("");
                $("#hidfc").val("");
                if (PageAction === "2" || PageAction === "3") {
                    $("#selType").removeClass("none");
                } else {
                    $("#selType").addClass("none");

                }
            });

            $(document).on("click", ".addFc", function () {
                var PageAction = $("#ddrtype").val();
                var compid = $("#hidCompID").val();
                var url = "";
                if (PageAction === "3")
                    url = 'SelectFC.aspx?PageAction=' + PageAction;
                else if (PageAction === "2")
                    url = 'SelectArea.aspx?PageAction=' + PageAction;

                var index = layerCommon.openWindow("指定选择", url, '880px', '600px'); //记录弹出对象
                $("#hid_Alert").val(index); //记录弹出对象
            });
        });

        function Goods(id, GoodsName, Barcode, ValueInfo, CategoryID, CategoryName) {
            $("#txtGoodsCode").val(Barcode);
            $("#hidGoodsID").val(id);
            $("#txtGoodsName").val(GoodsName);
            $("#txtCategoryID").val(CategoryName);
            $("#hidCategoryID").val(CategoryID);
            $("#txtValueInfo").val(ValueInfo);
        }

        function Fcs(Province, City, Area) {
            var item = $.trim($("#txtFC").val());
            var str = Province + "|" + City + "|" + Area;
            item += item === "" ? str : "," + str;
            $("#txtFC").val(item);
            $("#hidfc").val(item);
        }

        function Fc(ids) {
            var PageAction = $("#ddrtype").val();
            var compid = $("#hidCompID").val();
            if (ids !== "") {
                $.ajax({
                    type: 'post',
                    url: '../../Handler/CMHandler.ashx',
                    data: { ck: Math.random(), PageAction: PageAction, compid: compid, ids: ids },
                    dataType: 'json',
                    success: function (data) {
                        if (data.length > 0) {
                            console.log(data);
                            $("#hidfc").val(ids);
                            var item = "";
                            $.each(data, function (index, obj) {
                                if (PageAction === "2")
                                    item += item === "" ? obj.AreaName : "," + obj.AreaName;
                                else
                                    item += item === "" ? obj.DisName : "," + obj.DisName;
                            });
                            $("#txtFC").val(item);
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
                    }
                });
            }
        }

        function formCheck() {
            var str = "";
            if ($.trim($("#txtCMName").val()) === "") {
                str = "招商名称不能为空";
            } else if ($.trim($("#txtGoodsCode").val()) === "") {
                str = "请选择商品信息";
            }
            else if ($.trim($("#txtForceDate").val()) === "") {
                str = "请选择生效日期";
            }
            else if ($.trim($("#txtInvalidDate").val()) === "") {
                str = "请选择失效日期";
            } else if ($.trim($("#OrderNote").val()) === "") {
                tr = "备注不能为空";
            }

            if (str !== "") {
                layerCommon.msg(str, IconOption.错误);
                return false;
            } else {
                return true;
            }
        }
    </script>
</body>
</html>
