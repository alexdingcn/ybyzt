<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PaymentInfo.aspx.cs" Inherits="Distributor_Payment_PaymentAdd" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>收款单详情</title>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <link href="../newOrder/css/global2.5.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"
        rel="stylesheet" type="text/css" />

    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../newOrder/js/order.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
    <script src="../../js/UploadJs.js" type="text/javascript"></script>
    <style type="text/css">
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

        .jxs-box .bt {
            width: 80px;
            border: 1px solid blue;
        }

        .jxs-box .s {
            width: 430px;
            border: 1px solid blue;
        }

        .opt-i2 {
            margin-top: -20px;
        }

        ul, li {
            margin: 0;
            padding: 0;
            border: 0;
            font-style: normal;
            font-family: '微软雅黑', 'microsoft yahei', 宋体, Tahoma, Verdana, Arial, Helvetica, sans-serif;
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

        .tools {
            clear: both;
            overflow: hidden;
            margin-bottom: 8px;
        }

        .left {
            float: left;
        }

        .toolbar li {
            background: url(../../Company/images/toolbg.gif) repeat-x;
            line-height: 28px;
            height: 28px;
            border: solid 1px #c5d2d8;
            float: left;
            padding-right: 10px;
            margin-right: 5px;
            border-radius: 3px;
            behavior: url(js/pie.htc);
            cursor: pointer;
        }

            .toolbar li span {
                float: left;
                margin-left: 10px;
                margin-right: 5px;
                margin-top: 5px;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <Head:Head ID="Head2" runat="server" />
        <div class="w1200">
            <input type="hidden" runat="server" id="hid_Alert" value="" />
            <input type="hidden" runat="server" id="hidStorageType" value="" />
            <input type="hidden" id="PaymentID" value="" runat="server" />
            <Left:Left ID="Left1" runat="server" ShowID="nav-3" />
            <div class="rightCon">
                <div class="info">
                    <a id="navigation1" href="/Distributor/UserIndex.aspx">我的桌面</a>>
                    <a id="navigation2" href="PaymentList.aspx" class="cur">收款单详情</a>
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
                <ul class="coreInfo">
                    <li class="lb fl"><i class="name"><i class="red">*</i>收款单号</i>
                        <input type="text" class="box1 PaymentNO" id="PaymentNO" runat="server" name="PaymentNO" maxlength="20"  readonly="readonly" />
                    </li>
                    <li class="lb fl"><i class="name"><i class="red">*</i>收款日期</i>
                        <input name="PaymentDate" runat="server"
                            id="PaymentDate" readonly="readonly" type="text" class="box1" value="" />
                    </li>
                    <li class="lb fl"><i class="name"><i class="red">*</i>医院</i>
                          <input type="text" class="box1 PaymentNO" id="HtDrop" runat="server" name="PaymentNO" maxlength="20"  readonly="readonly" />
                    </li>


                    <li class="lb fl">
                        <i class="name ">状态</i>
                        <select name="CState" runat="server" id="IState" style="width: 382px;" class="box1" disabled="disabled">
                            <option value="0">新增</option>
                            <option value="1">审核</option>
                        </select>
                    </li>


                    <li class="lb fl"><i class="name">金额</i>
                        <input type="text" class="box1 " id="PaymentAmount" readonly="readonly" runat="server" maxlength="20" />
                    </li>
                    <li class="lb fl"><i class="name">备注</i>
                        <input type="text" class="box1 Remark" id="Remark" runat="server" name="Salesman" maxlength="20" readonly="readonly"/>
                    </li>

                </ul>

                <div class="blank20"></div>

                <div class="blank20"></div>
                <ul class="coreInfo" style="margin-left: 30px;">
                    <li class="lb clear">
                        <div class="con upload" style="margin-top: 8px;">
                            <asp:Panel runat="server" ID="DFile" Style="margin: 5px 5px;">
                            </asp:Panel>
                        </div>

                        <div id="UpFileText" style="margin-left: 70px;">
                        </div>
                        <input runat="server" id="HidFfileName" type="hidden" />
                    </li>
                </ul>
                <div class="blank10"></div>


                <div class="goods-zs">
                    <!--商品 start -->
                    <div class="tabLine">
                        <table border="0" cellspacing="0" cellpadding="0">
                            <thead>
                                <tr>
                                    <th class="t5" style="text-align: center"></th>
                                    <th class="t2">商品名称
                                    </th>
                                    <th class="t4">规格属性
                                    </th>
                                    <th class="t4">单位
                                    </th>
                                    <th class="t4">批次
                                    </th>
                                    <th class="t5">有效期
                                    </th>
                                    <th class="t4">数量
                                    </th>
                                    <th class="t4">单价
                                    </th>
                                    <th class="t4">金额
                                    </th>
                                    <th class="t4">备注
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="Rep_StorageDetail" runat="server">
                                    <ItemTemplate>
                                        <tr trindex="1" tip="<%#Eval("id") %>">
                                            <td class="t3">
                                                <div class="addg">
                                                    <a href="javascript:;" class="minus2"></a>
                                                    <a href="javascript:;" class="add2" tip="alast"></a>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="search">
                                                    <a class="opt-i2"></a>
                                                    <input name="GoodsName" value="<%#Eval("GoodsName") %>" type="text" class="box GoodsName" maxlength="15" readonly="readonly" />
                                                    <input type="hidden" value="<%#Eval("GoodsCode") %>" class="GoodsCode" value="0" />
                                                </div>
                                            </td>
                                            <td class="t2">
                                                <div class="search">
                                                    <input name="ValueInfo" value="<%#Eval("ValueInfo") %>" type="text" class="box ValueInfo" maxlength="15" readonly="readonly" />
                                                </div>
                                            </td>
                                            <td>
                                                <div class="search">
                                                    <input name="Unit" value="<%#Eval("Unit") %>" type="text" class="box Unit" maxlength="4" readonly="readonly" />
                                                </div>
                                            </td>
                                            <td class="t4">
                                                <div class="search">
                                                    <input name="BatchNO" value="<%#Eval("BatchNO") %>" type="text" class="box BatchNO" maxlength="15" readonly="readonly" />
                                                </div>
                                            </td>
                                            <td>
                                                <div class="search">
                                                    <input type="text" class="Wdate validDate" readonly="readonly"
                                                        value="<%#Convert.ToDateTime( Eval("validDate")).ToString("yyyy-MM-dd") %>" />
                                                </div>
                                            </td>
                                            <td>
                                                <div class="search">
                                                    <input value="<%#Convert.ToDecimal(Eval("Num")).ToString("#0") %>" type="text" class="box OutNum" maxlength="7" readonly="readonly"/>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="search">
                                                    <input  readonly="readonly" id="AuditAmount<%#Eval("id")%>" value="<%#Convert.ToDecimal( Eval("AuditAmount")).ToString("#0.00") %>"  type="text" class="box AuditAmount" maxlength="7" />
                                                </div>
                                            </td>
                                            <td>
                                                <div class="search">
                                                    <input readonly="readonly"  id="sumAmount<%#Eval("id")%>" value="<%#Convert.ToDecimal(Eval("sumAmount")).ToString("#0.00") %>" type="text" class="box sumAmount" maxlength="7" />
                                                </div>
                                            </td>
                                            <td>
                                                <div class="search">
                                                    <input readonly="readonly"  value="<%#Eval("Remark") %>" type="text" class="box Remark" maxlength="20" />
                                                </div>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <tr trindex="1" tip="0" runat="server" id="oneTR">
                                    <td>
                                        <div class="addg">
                                            <a href="javascript:;" class="minus2"></a>
                                            <a href="javascript:;" class="add2" tip="alast"></a>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="search">
                                            <a class="opt-i2"></a>
                                            <input name="GoodsName" type="text" class="box GoodsName" maxlength="15" readonly="readonly" />

                                            <input type="hidden" class="GoodsCode" value="0" />
                                        </div>
                                    </td>
                                    <td class="">
                                        <div class="search">
                                            <input name="ValueInfo" type="text" class="box ValueInfo" maxlength="15" readonly="readonly" />
                                        </div>
                                    </td>
                                    <td class="">
                                        <div class="search">
                                            <input name="Unit" type="text" class="box Unit" maxlength="4" readonly="readonly" />
                                        </div>
                                    </td>
                                    <td class="">
                                        <div class="search">
                                            <input name="BatchNO" type="text" class="box BatchNO" maxlength="15" readonly="readonly" />
                                        </div>
                                    </td>
                                    <td class="">
                                        <div class="search">
                                            <input type="text" class="Wdate validDate" readonly="readonly" />
                                        </div>
                                    </td>
                                    <td class="">
                                        <div class="search">
                                            <input name="OutNum" type="text" class="box OutNum" onblur="KeyInt(this,0)" onkeyup="KeyInt(this,0)" maxlength="7" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="search">
                                            <input type="text" id="AuditAmount0" class="box AuditAmount" maxlength="7" onblur="MoneyYZ(this)" onkeyup="MoneyYZ(this)" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="search">
                                            <input type="text" id="sumAmount0" class="box sumAmount" maxlength="7" readonly="readonly" />
                                        </div>
                                    </td>
                                    <td class="">
                                        <div class="search">
                                            <input name="Remark" type="text" class="box Remark" maxlength="20" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <!--商品 end -->
                </div>
                <!--[if !IE]>商品展示区end<![endif]-->
                <div class="blank20"></div>



            </div>
            <div class="po-bg2 none" style="z-index: 999999; background: #fffff">
            </div>
            <div id="p-delete" class="popup2 p-delete2 none" style="z-index: 9999999">
                <img src="../../js/layer/skin/default/loading-0.gif" />
            </div>

            <script>
                $(function () {
                    //审核
                    $(document).on("click", "#auditBtn", function () {
                        layerCommon.confirm("确定收款单？", function () {
                            var PaymentID = $("#PaymentID").val() + "";
                            $.ajax({
                                type: "post",
                                url: "../../Handler/orderHandle.ashx",
                                data: { ActionType: "auditPayment", PaymentID: PaymentID },
                                dataType: "text",
                                success: function (data) {
                                    var json = JSON.parse(data);
                                    if (json.returns == "true") {
                                        layerCommon.msg("操作成功", IconOption.笑脸);
                                        window.location = "PaymentInfo.aspx?KeyID=" + json.PaymentID + "";
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
                    var PaymentID = $("#PaymentID").val();
                    window.location.href = 'PaymentAdd.aspx?KeyID=' + PaymentID;
                }
            </script>
    </form>
</body>
</html>
