<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GoodsPriceList.aspx.cs" Inherits="Company_Goods_GoodsPriceList" %>

<%@ Register Src="../UserControl/TreeDisName.ascx" TagPrefix="uc2" TagName="DisDemo" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>代理商调价新增</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script>
        $(function () {
            $(".showDiv6 .ifrClass").css("width", "355px");
            $(".showDiv6").css("width", "350px");
            $(".txt_txtDisname").css("width", "350px");

            $(".txt_product_class").css("width", "350px");
            $('iframe').load(function () {
                $('iframe').contents().find('.pullDown').css("width", "350px");
            });
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("#btnSearch").trigger("click");
                }
            })
            //返回
            $(".cancel").click(function () {
                location.href = "DisPriceList.aspx";
            })
            //选择加盟商
            $("#txtDisID").click(function () {
                var Id = $("#hidCompId").val();
                var height = document.body.clientHeight; //计算高度
                var layerOffsetY = (height - 450) / 2; //计算宽度
               // var index = showDialog('选择代理商', '/Company/UserControl/SelectDisList.aspx?compid=' + Id, '880px', '450px', layerOffsetY); //记录弹出对象
                var index = layerCommon.openWindow('选择代理商', '/Company/UserControl/SelectDisList.aspx?compid=' + Id, '880px', '450px'); 
           
                $("#hid_Alert").val(index); //记录弹出对象
            });
            //确定并生效
            $(".btnInsert").click(function () {
                var z = 0;
                var x = 0;
                $("input[type='checkbox']").each(function (index, obj) {
                    if (obj.checked) {
                        z++;
                        var id = $(this).next().val();
                        if (id != undefined) {
                            if ($.trim($(".txtPrice" + id).val()) == "") {
                                x++;
                            }
                        }
                    }

                })
                if (z != 0) {
                    if (x != 0) {
                        layerCommon.msg("商品所调价格不能为空，请检查", IconOption.错误);
                        return false;
                    } else {
                        return true;
                    }
                } else {
                    layerCommon.msg("请选择商品", IconOption.错误);
                    return false;
                }
            })

        })
        //关闭选择代理商区域
        function selectDis(id, name) {
            if (id.toString() != "") {
                $(".txt_txtDisname").focus(); //解决 IE11 弹出层后文本框不能输入
                layerCommon.layerClose("hid_Alert");
                //代理商默认地址
                // DefaultAddr(id, 0);
                $(".txt_txtDisname").val(name); //区域名称
                $(".hid_DisId").val(id); //区域id
            } else {
                $(".txt_txtDisname").focus(); //解决 IE11 弹出层后文本框不能输入
                layerCommon.layerClose("hid_Alert");
                $(".txt_txtDisname").val(name); //区域名称
                $(".hid_DisId").val(id); //区域id
            }
        }
        //关闭选择商品区域
        function GbGoods() {
            layerCommon.layerClose("hid_Alert");
            selectPrice();
        }
        //选的商品
        function selectPrice() {
            var disid = $(".hid_DisId").val();
            $.ajax({
                type: "post",
                url: "GoodsPriceList.aspx",
                data: { ck: Math.random(), action: "selectPrice", disId: disid },
                dataType: "text",
                success: function (data) {
                    if (data != "") {
                        $("#lblGoodsPrice").html(data);
                    }
                }
            })
        }
        //新增商品
        function addData() {
            var title = $(".txtDisTitle").val(); //标题
            //            var disName = $(".txtDisID").val(); //代理商
            //            var disId = $("#hidDisId").val(); //代理商ID
            if ($.trim(title) == "") {
                layerCommon.msg("请输入调价标题", IconOption.错误);
                return false;
            }
            //            if ($.trim(disName) == "" || $.trim(disId) == "") {
            //                alert("请选择代理商");
            //                return false;
            //            }
            var disId = $(".hid_DisId").val();
            if (disId.toString() == "") {
                layerCommon.msg("请选择代理商", IconOption.错误);
                return false;
            }
            var trlength = $(".tablelist tr").length;
            var height = document.body.clientHeight; //计算高度
            var layerOffsetY = (height - 450) / 2; //计算宽度
           // var index = showDialog('选择商品', 'GoodsPriceEdit.aspx?keyId=' + trlength + "&disId=" + disId, '880px', '450px', layerOffsetY); //记录弹出对象
            var index = layerCommon.openWindow('选择商品', 'GoodsPriceEdit.aspx?keyId=' + trlength + "&disId=" + disId, '880px', '450px'); 
            $("#hid_Alert").val(index); //记录弹出对象
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
        function Delete(id) {
            layerCommon.confirm('确定要删除商品', function () {
                $.ajax({
                    type: "post",
                    url: "GoodsPriceList.aspx",
                    data: { ck: Math.random(), action: "delGoods", id: id },
                    dataType: "text",
                    success: function (data) {
                        if (data == "cg") {
                            $(".tablelist tr").remove(".tr" + id);
                        }
                    }, error: function () { }
                })

            });
        }
        function DefaultAddr(DisId, AddrId) {
        }
    </script>
    <style>
        .tb2
        {
            width: 100%;
            height: auto;
            border-bottom: medium none;
        }
        .tb2 table
        {
            border-collapse: collapse;
            border-spacing: 0;
        }
        .tb2 .span
        {
            background: none repeat scroll 0 0 #f6f6f6;
            display: block;
            padding-right: 10px;
            text-align: right;
            white-space: nowrap;
        }
        .tb2 label
        {
            padding-left: 5px;
        }
        .tb2 td
        {
            border: 1px solid #dedede;
            font-size: 13px;
            line-height: 30px;
            text-align: left;
        }
        .tb2 span i
        {
            color: red;
            margin-right: 5px;
        }
        .dh3 td
        {
            border: 0px solid #dedede;
            font-size: 13px;
            line-height: 35px;
            text-align: left;
        }
        .textarea
        {
            /* background: rgba(0, 0, 0, 0) url("../images/inputbg.gif") repeat-x scroll 0 0;*/
            line-height: 25px;
            margin-left: 5px;
            text-indent: 5px;
            margin: 5px;
            width: 585px;
            height: 50px;
            border: 1px solid #d1d1d1;
        }
        .footerBtn
        {
            text-align: center;
            margin-top: 15px;
            padding-bottom: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-3" />
    <input id="hid_Alert" type="hidden" />
    <input type="hidden" id="hidCompId" runat="server" />
    <div class="rightinfo">
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
                <li><a href="../GoodsNew/DisPriceList.aspx" runat="server" id="btitle">代理商价格维护</a></li><li>></li>
                <li><a href="javascript:;" runat="server" id="A1">代理商调价新增</a></li>
            </ul>
        </div>
        <div class="div_content">
            <div class="div_title">
            </div>
            <table class="tb2">
                <tbody>
                    <tr class="trClass">
                        <td style="width: 20%;">
                            <span class="span"><i class="required">*</i>调价标题</span>
                        </td>
                        <td colspan="3">
                            <input name="txtDisTitle" maxlength="50" type="text" style="width: 350px" class="textBox txtDisTitle"
                                id="txtDisTitle" runat="server" />
                        </td>
                    </tr>
                    <tr class="trClass">
                        <td style="width: 20%;">
                            <span class="span"><i class="required">*</i>选择代理商</span>
                        </td>
                        <td colspan="3">
                            <%--         <input name="txtDisID" type="text" style="width: 402px; cursor: pointer; margin-left: 5px;"
                                class="textBox txtDisID" id="txtDisID" runat="server" readonly="readonly" /><input
                                    type="hidden" id="hidDisId" runat="server" />--%>
                            <uc2:DisDemo runat="server" ID="txtDisID1" />
                            <a href="javascript:void(0)" class="linkbule" id="txtDisID">高级搜索</a>
                        </td>
                    </tr>
                    <% if (1 == 1)
                       { %>
                    <tr class="trClass">
                        <td style="width: 20%;">
                            <span class="span"><i class="required">*</i>调价有效期</span>
                        </td>
                        <td colspan="3">
                            <input name="txtArriveDate" runat="server" onclick="WdatePicker({minDate:'%y-%M-%d'})"
                                id="txtArriveDate" readonly="readonly" type="text" class="Wdate" value="" style="width: 150px;
                                margin-left: 5px;" />-
                            <input name="txtArriveDate1" runat="server" onclick="WdatePicker({minDate:'%y-%M-%d'})"
                                id="txtArriveDate1" readonly="readonly" type="text" class="Wdate" value="" style="width: 150px;
                                margin-left: 5px;" />
                        </td>
                    </tr>
                    <%} %>
                    <tr class="trClass">
                        <td style="background: #f6f6f6 none repeat scroll 0 0; width: 20%;">
                            <span class="span">备注</span>
                        </td>
                        <td>
                            <textarea id="txtRemark" maxlength="400" class="textarea" placeholder="备注不能大于400个字符"
                                runat="server"></textarea>
                        </td>
                    </tr>
                </tbody>
            </table>
            <!--功能按钮 start-->
            <div class="tools" style="padding-top: 15px">
                <ul class="toolbar left">
                    <li onclick="addData()"><span>
                        <img src="../images/t01.png" /></span>选择商品</li>
                </ul>
            </div>
            <!--功能按钮 end-->
            <!--信息列表 start-->
            <asp:Label ID="lblGoodsPrice" runat="server" Text=""></asp:Label>
            <!--信息列表 end-->
            <div class="footerBtn">
                <asp:Button ID="btnInsert" runat="server" Text="确定并生效" CssClass="orangeBtn btnInsert"
                    OnClick="btnInsert_Click" />&nbsp;
                <input name="" type="button" class="cancel" value="返回" />
            </div>
        </div>
    </div>
    </form>
    <script type="text/javascript">
        $('.tablelist tbody tr:odd').addClass('odd');
    </script>
</body>
</html>
