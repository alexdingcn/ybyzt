<%@ Page Language="C#" AutoEventWireup="true" CodeFile="logistadd.aspx.cs" Inherits="Company_newOrder_logistadd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>物流信息</title>
    <link href="../../Distributor/newOrder/css/global2.5.css?v=201507261156" rel="stylesheet"
        type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../Distributor/newOrder/js/order.js" type="text/javascript"></script>
    <style>
        .logistOk  .aa
        {
            text-align:left;
            float:right;
            line-height: 36px;
            padding-right:60px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <!--发布物流信息 start-->
    <div class="popup po-logistOk">
        <%--<div class="po-title">物流信息<a href="" class="close"></a></div>--%>
        <ul class="logistOk">
            <li>
                <div class="bt">
                    物流公司：</div>
                <div class="address">
                    <ul>
                        <li>
                            <input type="button" autocomplete="off" id="txtLogistics" class="text" onclick="beginSelect(this);"
                                value="" runat="server" />
                            <label style="color: Red;" id="lblLogistics">
                            </label>
                            <span class="arrow" onmousedown="beginSelect(this)"></span>
                        </li>
                        <li class="select">
                            <%--<p>顺丰快递</p>--%>
                            <asp:Repeater runat="server" ID="rptlogista">
                                <ItemTemplate>
                                    <p id="logistics"><%# Eval("LogisticsName") %></p>
                                </ItemTemplate>
                            </asp:Repeater>
                        </li>
                    </ul>
                    <div class="aa">
                        <a href="javascript:;" style="color: Blue" class="_logistics">维护物流公司</a>
                    </div>
                </div>
            </li>
            <li>
                <div class="bt">
                    物流单号：</div>
                <input name="" id="txtLogisticsNo" maxlength="50" autocomplete="off"
                    type="text" class="box" runat="server" />
                <label style="color: Red;" id="lblLogisticsNo">
                </label>
            </li>
            <li>
                <div class="bt">
                    司机姓名：</div>
                <input name="" id="txtCarUser" maxlength="50" autocomplete="off" type="text" class="box"
                    runat="server" />
                <label style="color: Red;" id="lblCarUser">
                </label>
            </li>
            <li>
                <div class="bt">
                    司机手机：</div>
                <input name="" id="txtCarNo" maxlength="50" autocomplete="off" onkeyup="KeyInt(this)"
                    type="text" class="box" runat="server" />
                <label style="color: Red;" id="lblcarNo">
                </label>
            </li>
            <li>
                <div class="bt">
                    车牌号：</div>
                <input name="" id="txtCar" maxlength="50" autocomplete="off"
                    type="text" class="box" runat="server" />
                <label style="color: Red;" id="lblCar">
                </label>
            </li>
        </ul>
        <div class="po-btn">
            <a href="javascript:;" class="gray-btn" id="btnCancel">取消</a> 
            <a href="javascript:;" runat="server" class="btn-area" id="btnConfirm">确定</a>
        </div>
    </div>
    <!--发布物流信息 end-->
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
        <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../Distributor/newOrder/js/ordercommon.js?v=201608170930" type="text/javascript"></script>

    <script>
        
        //返回维护的物流信息
        function ComLogisticsAdd(json) {
            location.href = location.href;
        }

        $(function () {

            //维护物流信息
            $("._logistics").click(function () {
                var url = '../SysManager/ComLogisticsAdd.aspx';                             //转向网页的地址; 
                var name = '维护物流';                     //网页名称，可为空; 
                var iWidth = 980;                          //弹出窗口的宽度; 
                var iHeight = 724;                         //弹出窗口的高度; 
                //获得窗口的垂直位置 
                var iTop = (window.screen.availHeight - 30 - iHeight) / 2;
                //获得窗口的水平位置 
                var iLeft = (window.screen.availWidth - 10 - iWidth) / 2;
                window.open(url, name, 'height=' + iHeight + ', width=' + iWidth + ',top=' + iTop + ',left=' + iLeft + ',toolbar=no,menubar=no,location=no,resizable=no,titlebar=no,scrollbars=yes');
            });

            //取消
            $(document).on("click", "#btnCancel", function () {
                //layerCommon.layerClose();
                window.parent.CloseGoods();
            });

            //确定
            $("#btnConfirm").click(function () {
                //物流信息、验证
                var str = ""
                var KeyID = '<%=Request["KeyID"]%>';

                var Logistics =stripscript( $.trim($("#txtLogistics").val()));
                var LogisticsNo =stripscript( $.trim($("#txtLogisticsNo").val()));
                var CarUser =stripscript( $.trim($("#txtCarUser").val()));
                var CarNo =stripscript( $.trim($("#txtCarNo").val()));
                var Car =stripscript( $.trim($("#txtCar").val()));

                //                    if (Logistics == "") {
                //                        str += "-物流公司不能为空";
                //                        $("#lblLogistics").html("-物流公司不能为空");
                //                    }
                //                    if (LogisticsNo == "") {
                //                        str += "-物流单号不能为空";
                //                        $("#lblLogisticsNo").html("-物流单号不能为空");
                //                    }
                //                    if (CarUser == "") {
                //                        str += "-姓名不能为空";
                //                        $("#lblCarUser").html("-姓名不能为空");
                //                    }
                //                    if (CarNo == "") {
                //                        str += "- 手机不能为空。<br/>";
                //                        $("#lblcarNo").html("- 手机不能为空");
                //                    } else {
                //                        var isMobile = /^0?(13[0-9]|15[012356789]|18[0-9]|14[57]|17[7])[0-9]{8}$/;
                //                        if (!isMobile.test(CarNo)) {
                //                            $("#lblcarNo").html("- 手机号码格式不正确");
                //                            str += "- 手机号码格式不正确。<br/>";
                //                        }
                //                    }
                //                    if (Car == "") {
                //                        str += "-车牌号不能为空";
                //                        $("#lblCar").html("-车牌号不能为空");
                //                    }
                //                    if (str != "") {
                //                        return false;
                //                    }


                if (KeyID != "") {
                    $.ajax({
                        type: "Post",
                        url: "logistadd.aspx/Edit",
                        data: "{'KeyID':'" + KeyID + "','logistics':'" + Logistics + "','logisticsNo':'" + LogisticsNo + "','carUser':'" + CarUser + "','carNo':'" + CarNo + "','car':'" + Car + "'}",
                        dataType: "json",
                        timeout: 5000,
                        contentType: "application/json; charset=utf-8",
                        success: function (ReturnData) {
                            var Json = eval('(' + ReturnData.d + ')');
                            if (Json.result) {
                                //修改物流信息
                                window.parent.logista_info(<%=KeyID %>, Logistics, LogisticsNo, CarUser, CarNo, Car);
                                window.parent.CloseGoods();
                            } else {
                                layerCommon.msg(Json.code, IconOption.错误);
                            }
                        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                            var a;

                        }

                    })
                }
                else {
                    window.parent.logista_info("", Logistics, LogisticsNo, CarUser, CarNo, Car);
                    window.parent.CloseGoods();
                }

            });

        });
    </script>
    </form>
</body>
</html>
