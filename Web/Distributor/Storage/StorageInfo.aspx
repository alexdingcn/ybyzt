<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StorageInfo.aspx.cs" Inherits="Distributor_Storage_StorageAdd" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>入库详情</title>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <link href="../newOrder/css/global2.5.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"
        rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/autoTextarea.js" type="text/javascript"></script>
    <link href="../../Company/css/style.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .search {
            width: 90%;
        }

        .xl {
            width: 160px;
            border: 1px solid #d1d1d1;
            color: #494949;
            font-size: 12px;
            height: 27px;
            line-height: 27px;
            float: left;
            overflow: hidden;
            border-radius: 5px;
            margin-right: 10px;
        }

        tr input {
            text-align: center;
            font-size: 12px;
            color: #494949;
        }
         tr input {
            text-align: center;
            font-size: 12px;
            color: #494949;
        }
         .coreInfo .box1 {
            width: 360px;
        }

        .box1, .box2 {
            border: 1px solid #ddd;
            border-radius: 5px;
            height: 34px;
            line-height: 34px;
            padding: 0px 10px;
            color: #555;
            font-size: 12px;
        }

        .coreInfo .lb {
            padding-top: 10px;
            min-width: 482px;
            color: #666;
            position: relative;
            margin-right: 30px;
            min-height: 36px;
        }

        .coreInfo .name {
            width: 90px;
            text-align: right;
            display: inline-block;
            padding-right: 10px;
            line-height: 35px;
            color: #666;
        }

        .search {
            width: 90%;
        }
         .fl {
            float: left;
        }

        li {
            list-style-type: none;
        }

        input {
            outline: none;
            star: expression(this.onFocus=this.blur());
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <Head:Head ID="Head2" runat="server" />
        <div class="w1200">
            <input type="hidden" runat="server" id="hid_Alert" value="" />
            <input type="hidden" runat="server" id="hidStorageType" value="" />
            <input type="hidden" id="StorageID" value="" runat="server" />
            <Left:Left ID="Left1" runat="server" ShowID="nav-4" />
            <div class="rightCon">
                <div class="info">
                    <a id="navigation1" href="/Distributor/UserIndex.aspx">我的桌面</a>>
                    <a id="navigation2" href="StorageList.aspx" class="cur">入库单列表</a>>
                    <a href="StorageInfo.aspx" class="cur">入库单详情</a>
                </div>

                <div id="DisBoot" class="tools">
                    <ul class="toolbar left">
                        <li id="btnUpdate" runat="server" onclick="btnUpdate()"><span>
                            <img src="../../Company/images/t02.png"></span>修改</li>
                        <li id="auditBtn" runat="server"><span>
                            <img src="../../Company/images/t06.png"></span>审核</li>
                    </ul>
                </div>



                <div class="blank10"></div>
                <!--[if !IE]>商品展示区 start<![endif]-->
                <!--[if !IE]>选择厂商 start<![endif]-->
                <%--<div class="jxs-box left">
                    <div class="bt">厂商：</div>
                    <div class="s left">
                        <select id="ddrComp" name="" runat="server" class="xl" disabled="disabled"></select>
                    </div>

                </div>--%>
                <!--[if !IE]>选择厂商 end<![endif]-->

                                <div class="blank10"></div>
                <ul class="coreInfo">
                    <li class="lb fl"><i class="name"><i class="red">*</i>入库单号</i>
                        <input type="text" class="box1" id="StorageNO" readonly="readonly" runat="server"  maxlength="20" />
                    </li>
                    <li class="lb fl"><i class="name"><i class="red">*</i>入库日期</i>
                        <input type="text" class=" box1" id="txtStorageDate" runat="server" readonly="readonly"
                            />
                    </li>
                    <li class="lb fl">
                         <i class="name ">厂 商</i>
                         <select id="ddrComp" name="" runat="server" disabled="disabled" class="box1" style="width: 382px;"></select>
                    </li>
                    <li class="lb fl">
                        <i class="name ">入库类型</i>
                        <select name="CState" runat="server" id="lblStorageType1" style="width: 382px;" class="box1" disabled="disabled">
                            <option value="1">采购入库</option>
                            <option value="2">其它入库</option>
                        </select>
                    </li>
                </ul>

                <div class="blank10"></div>

                <div class="goods-zs">
                    <!--商品 start -->
                    <div class="tabLine">
                        <table border="0" cellspacing="0" cellpadding="0">
                            <thead>
                                <tr>
                                    <th class="t5"></th>
                                    <th class="">商品名称
                                    </th>
                                    <th class="t3">规格属性
                                    </th>
                                    <th class="t3">单位
                                    </th>
                                    <th>批次
                                    </th>
                                    <th>有效期
                                    </th>
                                    <th class="t3">入库数量
                                    </th>
                                    <th class="t3">备注
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="Rep_StorageDetail" runat="server">
                                    <ItemTemplate>
                                        <tr trindex="1" tip="0">
                                            <td class="t8">
                                                <div class="addg">
                                                    <a href="javascript:;" class="minus2"></a>
                                                    <a href="javascript:;" class="add2" tip="alast"></a>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="search">
                                                    <input type="text" class="box" readonly="readonly" value="<%# Eval("GoodsName") %>" />

                                                </div>
                                            </td>
                                            <td class="t2">
                                                <div class="search">
                                                    <input type="text" class="box" readonly="readonly" value="<%# Eval("ValueInfo") %>" />
                                                </div>
                                            </td>
                                            <td>
                                                <div class="search">
                                                    <input type="text" class="box" readonly="readonly" value="<%# Eval("Unit") %>" />
                                                </div>
                                            </td>
                                            <td class="t4">
                                                <div class="search">
                                                    <input type="text" class="box" readonly="readonly" value="<%# Eval("BatchNO") %>" />
                                                </div>
                                            </td>
                                            <td>
                                                <div class="search">
                                                    <input type="text" class="box" readonly="readonly" value="<%# Convert.ToDateTime( Eval("validDate")).ToString("yyyy-MM-dd")%>" />
                                                </div>
                                            </td>
                                            <td>
                                                <div class="search">
                                                    <input type="text" class="box" readonly="readonly" value="<%# Convert.ToDecimal( Eval("StorageNum")).ToString("#0.00") %>" />
                                                </div>
                                            </td>
                                            <td>
                                                <div class="search">
                                                    <input type="text" class="box" readonly="readonly" value="<%# Eval("Remark") %>" />
                                                </div>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>

                            </tbody>
                        </table>
                    </div>
                    <!--商品 end -->
                </div>
                <!--[if !IE]>商品展示区end<![endif]-->
                <div class="blank20"></div>
                <!--[if !IE]>下单信息 start<![endif]-->
                <div class="goods-info">

                    <div class="bz remark">
                        <i class="bt">备 注：</i><div class="txt_box">
                            <textarea id="OrderNote" runat="server" name="OrderNote" disabled="disabled" maxlength="200" class="box"
                                placeholder="备注不能超过200个字"></textarea>
                        </div>
                    </div>
                </div>
                <!--[if !IE]>下单信息 end<![endif]-->
                <div class="blank20"></div>

            </div>

        </div>
        <div class="po-bg2 none" style="z-index: 999999; background: #fffff">
        </div>
        <div id="p-delete" class="popup2 p-delete2 none" style="z-index: 9999999">
            <img src="../../js/layer/skin/default/loading-0.gif" />
        </div>

        <script>
            $(function () {
                //配送方式
                $(".carry .menu a").click(function () {
                    var type = $.trim($("#lblStorageType1").text());
                    if (type == "采购入库") {
                        $("#hidStorageType").val("2");
                        $("#lblStorageType1").text("其他入库");
                        $("#lblStorageType2").text("采购入库");
                    } else {
                        $("#hidStorageType").val("1");
                        $("#lblStorageType2").text("其他入库");
                        $("#lblStorageType1").text("采购入库");
                    }
                });

                ///取消
                $(document).on("click", ".btnCancel", function () {
                    window.location.href = 'StorageList.aspx';
                });

                //审核
                $(document).on("click", "#auditBtn", function () {
                    layerCommon.confirm("确定审核入库单？", function () {
                        var StorageID = $("#StorageID").val() + "";
                        $.ajax({
                            type: "post",
                            url: "../../Handler/orderHandle.ashx",
                            data: { ActionType: "auditStorage", StorageID: StorageID },
                            dataType: "text",
                            success: function (data) {
                                var json = JSON.parse(data);
                                if (json.returns == "true") {
                                    layerCommon.msg("操作成功", IconOption.笑脸);
                                    window.location = "StorageInfo.aspx?KeyID=" + json.StorageID + "";
                                }
                                else {
                                    layerCommon.msg("操作失败：" + json.msg + "", IconOption.哭脸);
                                }

                                return false;
                            }, error: function () {
                                layerCommon.msg("提交失败", IconOption.错误);
                                return false;
                            }
                        })
                    }, "审核")
                });


            })

            function btnUpdate() {
                var StorageID = $("#StorageID").val();
                window.location.href = 'StorageAdd.aspx?KeyID=' + StorageID;
            }
        </script>
    </form>
</body>
</html>
