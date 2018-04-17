<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DisEdit.aspx.cs" Inherits="Company_SysManager_DisEdit" %>

<%@ Register Src="~/Company/UserControl/TreeDisArea.ascx" TagPrefix="uc1" TagName="TreeDisArea" %>
<%@ Register Src="~/Company/UserControl/TreeDisType.ascx" TagPrefix="uc1" TagName="TreeDisType" %>
<%@ Register Src="~/Company/UserControl/CompRemove.ascx" TagPrefix="uc1" TagName="CompRemove" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>代理商新增</title>

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
    <style type="text/css">
        .pullDown2 {
            margin-left: 100px;
            border: 1px solid #e5e5e5;
            width: 170px;
            background: #fff;
            position: relative;
            z-index: 10000;
        }

            .pullDown2 .list {
                overflow-y: scroll;
            }

                .pullDown2 .list a {
                    padding-left: 10px;
                    line-height: 26px;
                    height: 26px;
                    display: block;
                    color: #444;
                }

                    .pullDown2 .list a:hover {
                        background: #d1d1d2;
                        color: #444;
                    }

            .pullDown2 .addBtn {
                background: #f5f5f5;
                border-top: 1px solid #ddd;
                height: 30px;
                line-height: 30px;
                position: relative;
                display: block;
                padding-left: 25px;
                color: #555;
            }

            .pullDown2 .addIcon {
                width: 12px;
                height: 14px;
                background: url(../images/t05.png) no-repeat 0 0;
                display: inline-block;
                position: absolute;
                top: 8px;
                left: 8px;
            }

        .xy {
            z-index: 999999px;
            position: absolute;
        }

        select {
            width: 200px;
            height: 50px;
        }

        #More {
            cursor: pointer;
        }

        .tip, .tip2, .tip3, .tip4 {
            background: #fff;
            /* overflow:hidden;*/
            /* -moz-box-shadow:0 2px 3px rgba(0,0,0,0.5);
   -webkit-box-shadow:0 2px 3px rgba(0,0,0,0.5);*/
            width: 450px;
            height: 205px;
            position: absolute;
            top: 20%;
            left: 30%; /*background:#fcfdfd;box-shadow:1px 8px 10px 1px #9b9b9b; border:8px solid rgba(0, 0, 0, .5); border-radius:5px;behavior:url(js/PIE.htc); display:none;*/
            z-index: 111111;
        }

        .tiptop {
            position: absolute;
            background: #ebebeb url("default/xubox_title0.png") repeat-x scroll 0 0;
            border-bottom: 1px solid #d5d5d5;
            color: #333;
            cursor: move;
            font-size: 14px;
            height: 35px;
            line-height: 35px;
            width: 100%;
        }

            .tiptop span {
                position: absolute;
                display: block;
                font-style: normal;
                height: 20px;
                left: 10px;
                line-height: 20px;
                overflow: hidden;
                top: 9px;
                width: 80%;
            }
            /*.tiptop a{display:block; background:url(../images/close.png) no-repeat; width:22px; height:22px;float:right;margin-right:7px; margin-top:10px; cursor:pointer;}
.tiptop a:hover{background:url(../images/close1.png) no-repeat;}
.tipinfo{padding-top:10px;margin-left:0; height:95px;}*/
            .tiptop a {
                position: absolute;
                background: rgba(0, 0, 0, 0) url("../../Distributor/images/fx.png") no-repeat scroll 0 -52px;
                cursor: pointer;
                height: 14px;
                overflow: hidden;
                right: 10px;
                top: 10px;
                width: 14px;
            }

                .tiptop a:hover {
                    color: #00a4ac;
                    text-decoration: none;
                }

        .tipinfo {
            padding-top: 40px;
            margin-left: 0;
            height: 95px;
        }

            .tipinfo .lb {
                height: 30px;
                line-height: 30px;
                padding-bottom: 10px;
                overflow: hidden;
            }

                .tipinfo .lb span {
                    display: inline-block;
                    text-align: right;
                    width: 150px;
                }

                .tipinfo .lb .textBox {
                    width: 150px;
                }

        .tipbtn {
            margin-top: 10px;
            margin-left: 155px;
        }

        .sure, .cancel, .orangeBtn {
            padding: 0px 20px;
            height: 28px;
            color: #fff;
            background: #537fc4;
            border: 1px solid #537fc4;
            font-size: 14px;
            border-radius: 3px;
            cursor: pointer;
        }

        .cancel {
            background: #efefef;
            color: #000;
            font-weight: normal;
            border: 1px solid #e0e0e0;
        }

        .sure:hover {
            background: #3b6dbb;
            border: 1px solid #3b6dbb;
        }

        .cancel:hover {
            background: #e0e0e0;
        }

        .orangeBtn {
            background: #537fc4;
            font-weight: normal;
            border: 1px solid #537fc4;
        }

            .orangeBtn:hover {
                background: #3b6dbb;
                border: 1px solid #3b6dbb;
            }
    </style>
    <script>
        $(function () {

            //绑定代理商分类
            $("#txtDisType").click(function () {
                handleChange(this, "<%=DisType%>");
            });
            //绑定代理商区域
            $("#txtDisArea").click(function () {
                handleChange(this, "<%=DisArea%>");
            });

            //绑定代理商区域
            $("#txtunit").click(function () {
                handleChange(this, "<%=DisDj%>");
            });

            //绑定现有登录用户确认事件
            $("#PhoneUserBtn").click(function () {
                $("#userid").val($("#useridtext").val())
            })
            //绑定Enter事件
            if ($("#DisLel").is(":hidden"))
                $_def.ID = "btnAdd";
            else
                $_def.ID = "btnAddUnit";

            $(".showDiv3 .ifrClass,.showDiv2 .ifrClass").css("width", "175px");
            $(".showDiv3,.showDiv2").css("width", "170px");

            $('iframe').load(function () {
                $('iframe').contents().find('.pullDown').css("width", "170px");
            })
            //新增代理商等级
            $(".addBtn").click(function () {
                $("#DisLel").fadeIn(200);
                $(".Layer").fadeIn(200);
                $_def.ID = "btnAddUnit";//绑定Enter事件
            });

            //取消
            $(".cancel,.tiptop a").click(function () {
                $("#DisLel").fadeOut(100);
                //$("#SetUser").fadeOut(100);
                //$(".Layer").fadeOut(100);
                $("#useridtext").val("0")
                $("#userid").val("0")
                $_def.ID = "btnAdd";//绑定Enter事件
            });
            if ($(".pullDown2 .list li").length > 6) {//超过6个单位计量，则出现滚轴
                $(".pullDown2 .list").css("height", "156px");
            }
            //$(document.body).click(function (e) {
            //    var xx = e.pageX; //鼠标X轴
            //    var yy = e.pageY; //鼠标Y轴
            //    var x = parseInt($(".txtunit").offset().left) - 5; //文本框坐标
            //    var y = parseInt($(".txtunit").offset().top) + 25; //文本框坐标

            //    if (xx > x && xx < x + 170 && yy + 25 > y && yy < y) {
            //        //  alert(x + "," + xx + ";;" + y + "," + yy)
            //    } else {
            //        if ($(".pullDown2").is(":visible")) {
            //            $(".pullDown2").hide();
            //            $(".pullDown2").removeClass("xy");
            //        }
            //    }
            //});
            //分类管理
            $(document).on("click", "#adisType", function () {
                var index = layerCommon.openWindow('分类管理', 'DisTypeList.aspx?type=1&lefttype=3', '850px', '500px');
                $("#hid_Alert").val(index);
            });

            //区域管理
            $(document).on("click", "#aDisArea", function () {
                var index = layerCommon.openWindow('区域管理', 'DisAreaList.aspx?type=1&lefttype=3', '850px', '500px');
                $("#hid_Alert").val(index);
            });

            //代理商等级
            $(".txtunit").click(function () {
                if ($(".pullDown2").is(":visible")) {
                    $(".pullDown2").hide();
                    $(".pullDown2").removeClass("xy");
                } else {
                    $(".pullDown2").show();
                    $(".pullDown2").addClass("xy");
                    $(".pullDown2").css("display", "block");
                }
            })
            $(".pullDown2 .list li").click(function () {
                $(".txtunit").val($.trim($(this).text()));
                $(".pullDown2").hide();
                $(".pullDown2").removeClass("xy");
            })


            //提交按钮单机事件
            $("#btnAdds").click(function () {
                $("#<%=btnAdd.ClientID%>").click();
            });
            //更多功能 按钮单机事件
            $("#More").click(function () {
                var text = $(this).html();
                if (text == "更多功能") {
                    $(".gd").removeClass("none");
                    $(this).html("收起");
                }
                else {
                    $(".gd").addClass("none");
                    $(this).html("更多功能");
                }
            })

            //add by hgh 
            if ('<%=Request["nextstep"] %>' == "1") {
                //window.parent.leftFrame.document.getElementById("ktszgwqx").className = "";
                //window.parent.leftFrame.document.getElementById("ktxzjxs").className = "active";
                document.getElementById("imgmenu").style.display = "block";
            }

        })
        //验证新增代理商等级
        function formChecks1() {
            var str = "";
            if ($.trim($(".txtunits").val()) == "") {
                str = "代理商等级不能为空"
            }
            if (str == "") {
                str = ExisDisLevel($.trim($(".txtunits").val()));
            }
            if (str != "") {
                layerCommon.msg(str, IconOption.错误);
                return false;
            }
            return true;
        }
        function ExisDisLevel(name) {
            var str = "";
            $.ajax({
                type: "post",
                data: { Action: "GetDis", Value: name },
                dataType: 'json',
                async: false,
                timeout: 4000,
                success: function (data) {
                    if (data.result) {
                        str = "该代理商等级已存在";
                    }
                }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                    CheckTitle(obj, false, "校验失败，服务器或网络异常");
                }
            })
            return str;
        }
        function etitle() {
            $("i[error]").css("color", "red");
        }
        function getFileName(o) {
            var pos = o.lastIndexOf("\\");
            return o.substring(pos + 1);
        }

        function change(obj, showid) {
            var FileName = getFileName(obj.value.toString());
            $("#" + showid + "").text(FileName);
        }

        function formCheck() {
            var str = "";
            if ($.trim($("#txtDisName").val()) == "") {
                str = "代理商名称不能为空";
            }
            else if ($.trim($("#txtDisName").val()) != "" && ($.trim($("#txtDisName").val()).length < 2 || $.trim($("#txtDisName").val()).length > 20)) {
                str = "代理商名称只能为2-20个汉字或字母";
            }
            else if ($.trim($("#txtLicence").val()) != "" && !validateIdCard($.trim($("#txtLicence").val()))) {
                str = "负责人身份证号码不正确";
            }
            else if ($.trim($("#txtLeadingPhone").val()) != "" && !IsMobile($.trim($("#txtLeadingPhone").val()))) {
                str = "负责人手机不正确";
            }
            else if ($.trim($("#txtPhone").val()) != "" && !IsMobile($.trim($("#txtPhone").val()))) {
                str = "联系人手机不正确";
            }
            else if ($.trim($("#ddlProvince").val()) == "") {
                str = "请选择省";
            }
            else if ($.trim($("#ddlCity").val()) == "") {
                str = "请选择市";
            }
            else if ($.trim($("#ddlArea").val()) == "") {
                str = "请选择区";
            }
            else if ($.trim($("#txtAddress").val()) == "") {
                str = "详细地址不能为空";
            }
            else if ($.trim($("#txtUsername").val()) == "") {
                str = "登录帐号不能为空";
            }
            else if ($.trim($("#txtUsername").val()).length < 2 || $.trim($("#txtUsername").val()).length > 20) {
                str = "登录帐号必须在2-20字符之间。";
            }
            else if ($.trim($("#txtUserPhone").val()) == "") {
                str = "请输入管理员手机号";
            }
            else if (!IsMobile($.trim($("#txtUserPhone").val()))) {
                str = "管理员手机号不正确。";
            }
            else if ($.trim($("#txtUpwd").val()).length < 6) {
                str = "用户名登录密码不能少于6位";
            }
            else if ($.trim($("#txtUpwd").val()) != $.trim($("#txtUpwds").val())) {
                str = "确认密码不一致";
            }
            else if ($.trim($("#txtCreditAmount").val()) != "") {
                var CreditAmount = $.trim($("#txtCreditAmount").val());
                var Credit = $('input[name="Credit"]:checked').val();
                if (parseInt(Credit) == 1) {
                    if (parseInt(CreditAmount) == 0) {
                        str = "企业授信不能等于0";
                    } else {
                   <%--var GetSumAmount=<%=OrderInfoType.GetSumAmount(KeyID) %>;
                    if(parseFloat(CreditAmount)<parseFloat(GetSumAmount))
                    {
                          str="正在赊销的订单金额大于企业授信，订单赊销金额："+GetSumAmount;
                      }--%>
                   }
               }
        }
        else if (parseInt($.trim($("#txtFinancingRatio").val())) < 0 || parseInt($.trim($("#txtFinancingRatio").val())) > 100) {
            str = "融资申请比例要大于等于0小于等于100";
        }
        else if ($.trim($("#txtUserTrueName").val()) == "") {
            str = "管理员姓名不能为空";
        }
            <% if (KeyID == 0)
        { %>
            else if ($.trim($("#txtDisName").val()) !== "") {
                str = ExistsDis($.trim($("#txtDisName").val()));
            }
            else if (IsMobile($.trim($("#txtUserPhone").val()))) {
                if ($("#userid").val() == "0") {
                    str = ExisPhone($.trim($("#txtUserPhone").val()));
                }
            }
            <% } %>
            if (str != "") {
                if (str === "该代理商名称已存在") {
                    //                    $("#SetUser").fadeIn(200);
                    //                    $(".Layer").fadeIn(200);
                    //                    return false;
                    layerCommon.confirm("平台已存在代理商，是否直接添加为渠道商", function () {
                        $("#<%=btnAddDis.ClientID%>").click();
                       }, "提示", function () {

                       });
                   } else {
                       layerCommon.msg(str, IconOption.错误);
                   }
                   return false;
               }
               return true;
           }


           function phoneValue(obj) {
               if (!IsMobile($.trim($("#txtUserPhone").val()))) {
                   $("#txtUserPhone").val(obj.value);
               }
           }

           $(document).ready(function () {
               //$("#uploadFile").AjaxUploadFile({ Src: "UploadFile/", ShowDiv: "UpFileText", ResultId: "HidFfileName", AjaxSrc: "../../Controller/Fileup.ashx", maxlength: 5, DownSrc: "../../" });
               var div = $('<div   Msg="True"  style="z-index:1000; background-color:#000; opacity:0.3; filter:alpha(opacity=50);position: fixed;width:100%;height:100%;top:0px;left:0px;"></div>');


               $("li#liSearch").on("click", function () {
                   $("#btn_Search").trigger("click");
               })

               //是否赊销
               $("input[name='Credit']").click(function () {
                   var $this = $(this);
                   $this.val() == 1 ? function () {

                       $(".tdCredit").show();
                   }() : function () {
                       $(".tdCredit").hide();
                   }();
               });

               $("#rdCreditNo:checked").length > 0 && function () { $(".tdCredit").hide(); $("#rdCreditNo:checked").closest("td").attr("colspan", "3"); }();


           });

           //判断当前代理商信息是否已存在
           function ExistsDis(name) {
               var str = "false";
               $.ajax({
                   type: "post",
                   data: { Action: "DisExists", Value: name },
                   dataType: 'json',
                   async: false,
                   timeout: 4000,
                   success: function (data) {
                       if (data.result) {
                           str = "该代理商名称已存在";
                       } else {
                           str = data.msg;
                       }
                   }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                       CheckTitle(obj, false, "校验失败，服务器或网络异常");
                   }
               })
               return str;
           }

           //判断当前手机号是否已存在
           function ExisPhone(name) {
               var str = "";
               $.ajax({
                   type: "post",
                   data: { Action: "GetPhone", Value: name },
                   dataType: 'json',
                   async: false,
                   timeout: 4000,
                   success: function (data) {
                       if (data.result) {
                           str = "该手机已被注册请重新填写";
                           $("#UserName").html(data.userNO)
                           $("#useridtext").val(data.userid)
                       }
                       else {
                           $("#useridtext").val("0")
                           $("#userid").val("0")
                       }
                   }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                       CheckTitle(obj, false, "校验失败，服务器或网络异常");
                   }
               })
               return str;
           }

           function formChecks(obj) {
               var str = $("#FileUpload1").val();
               if (str == "") {
                   errMsg("提示", "- 请选择要导入代理商Excel的文件", "", "");
                   return false;
               }
               var suffix = $.trim(str.substring(str.lastIndexOf(".")));
               if (suffix == ".xlsx" || suffix == ".xls") {
                   $(obj).attr("disabled", "disabled");
                   return true;
               } else {
                   errMsg("提示", "- 请选择ExcelL文件", "", "");
                   return false;
               }
           }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <uc1:Top ID="top1" runat="server" ShowID="nav-4" />
        <uc1:CompRemove runat="server" ID="Remove" ModuleID="2" />
        <input type="hidden" id="hid_Alert" />
        <div class="rightinfo">
            <!--当前位置 start-->
            <div class="info">
                <a href="../jsc.aspx">我的桌面 </a>>
   
                <a href="../SysManager/DisList.aspx" runat="server" id="atitle">代理商查询</a>>
   
                <a href="#" runat="server" id="btitle">代理商新增</a>
            </div>
            <!--当前位置 end-->
            <!--代理商信息 start-->
            <div class="c-n-title">代理商信息</div>
            <p class="c-n-subtitle">代理商名称一般为公司全名，如果代理商已在系统注册账号，则会自动根据名称进行添加</p>
            <ul class="coreInfo">
                <li class="lb fl"><i class="name"><i class="red">*</i>代理商名称</i>
                    <input type="text" class="box1" placeholder="2-20个汉字或字母，代理商公司名称" runat="server" onkeypress="KeyPress(event)" maxlength="50" id="txtDisName" /></li>
                <li class="lb fl"><i class="name">代理商编码</i><input type="text" style="margin-left: 2px" class="box1" id="DisCode" name="DisCode" placeholder="6-20以内字母或数字" runat="server" /></li>
                <li class="lb fl">
                    <i class="name fl"><i class="red">*</i>详细地址</i>
                    <div class="pullDown fl" style="margin-left: 5px;">
                        <%--<ul><li><input type="button" class="box1 p-box" onclick="beginSelect(this);" value="上海市　"><span class="arrow"></span></li>
			<li class="select"><p class="">上海市</p><p class=" ">采购</p><p class=" ">外协</p><p class=" ">临时</p></li></ul>--%>
                        <input type="hidden" id="hidProvince" runat="server" value="选择省" />
                        <select runat="server" id="ddlProvince" class="box1 p-box prov" onchange="Change()"></select>
                    </div>
                    <div class="pullDown fl">
                        <select runat="server" id="ddlCity" class="box1 p-box city" onchange="Change()"></select>
                        <input type="hidden" id="hidCity" runat="server" value="选择市" />
                    </div>
                    <div class="pullDown fl">
                        <select runat="server" id="ddlArea" class="box1 p-box dist" onchange="Change()"></select>
                        <input type="hidden" id="hidArea" runat="server" value="选择区" />
                    </div>

                </li>

                <li class="lb fl">
                    <i class="name" style="margin-left: -10px">代理商分类</i>
                    <input type="text" class="box1" id="txtDisType" readonly="readonly" runat="server" /><span class="arrow"></span>
                    <input type="hidden" class="box1" id="hid_txtDisType" readonly="readonly" runat="server" />
                    <div class="pop-menu" style="width: 605px;">
                    </div>
                </li>
                <li class="lb fl">
                    <i class="name"></i>
                    <input type="text" runat="server" maxlength="100" id="txtAddress" class="box1" placeholder="填写具体的详细地址" />
                </li>
                <%--<li class="lb fl"><i class="name">代理商等级</i>
        <input type="text" class="box1" id="txtunit" readonly="readonly" runat="server"/><span class="arrow"></span>
        <input type="hidden" class="box1" id="hid_txtunit" readonly="readonly" runat="server"/>                 

	</li>--%>
                <li class="lb fl">
                    <i class="name">代理商区域</i>

                    <input type="text" class="box1" id="txtDisArea" readonly="readonly" runat="server" /><span class="arrow"></span>
                    <input type="hidden" class="box1" id="hid_txtDisArea" readonly="readonly" runat="server" />
                    <a id="aDisArea" style="color: #1a8fc2" href="javascript:void(0);"></a>
                    <div class="pop-menu" style="width: 605px; display: none;">
                    </div>
                </li>
                <li class="lb fl gd none"><i class="name">联系人</i><input type="text" style="margin-left: 3px" class="box1" onkeypress="KeyPress(event)" maxlength="20" placeholder="代理商联系人" id="txtPerson" onblur="javascript:if($.trim($('#txtUserTrueName').val())==''){$('#txtUserTrueName').val(this.value);}" runat="server" /></li>
                <li class="lb fl gd none"><i class="name">联系人手机</i><input type="text" style="margin-left: 3px" class="box1" id="txtPhone" maxlength="11" onblur="phoneValue(this)" runat="server" /></li>
                <li class="lb fl gd none"><i class="name">邮编</i><input type="text" style="margin-left: 3px" class="box1" id="txtZip" maxlength="50" runat="server" /></li>
                <li class="lb fl gd none"><i class="name">传真</i><input type="text" style="margin-left: 3px" class="box1" id="txtFax" maxlength="20" runat="server" /></li>
                <li class="lb fl gd none"><i class="name">备注</i><input type="text" style="margin-left: 3px" class="box1" id="txtRemark" maxlength="20" runat="server" /></li>
                <li class="lb fl"><i class="name"></i><i class="more" id="More">更多功能</i></li>
            </ul>
            <!--代理商信息 end-->

            <div class="blank10"></div>
            <!--账户信息 start-->
            <div class="c-n-title">账户信息</div>
            <ul class="coreInfo">
                <li class="lb fl"><i class="name"><i class="red">*</i>帐号</i>
                    <input runat="server" type="text" maxlength="50" class="box1" id="txtUsername" placeholder="2-20个文字、字母、数字，可以录入代理商姓名、简称等" /></li>
                <li class="lb fl"><i class="name"><i class="red">*</i>姓名</i><input runat="server" maxlength="50" type="text" class="box1" id="txtUserTrueName" placeholder="请填写真实姓名" /></li>
                <li class="lb fl"><i class="name"><i class="red">*</i>密码</i>
                    <asp:TextBox ID="txtUpwd" TextMode="Password" MaxLength="50" runat="server" CssClass="box1"></asp:TextBox></li>
                <li class="lb fl"><i class="name"><i class="red">*</i>确认密码</i><asp:TextBox ID="txtUpwds" TextMode="Password" MaxLength="50" runat="server" CssClass="box1"></asp:TextBox></li>
                <li class="lb fl"><i class="name"><i class="red">*</i>手机号码</i><input runat="server" type="text" maxlength="11" class="box1" id="txtUserPhone" style="margin-left: 3px" placeholder="登录、发送验证短信使用" /></li>
            </ul>
            <!--账户信息 end-->

            <div class="blank10"></div>
            <!--参数设置　start-->
            <div class="c-n-title">参数设置</div>
            <ul class="coreInfo">
                
                <li class="lb fl">
                    <i class="name"><i class="red">*</i>启用帐号</i>
                    <div class="single">
                        <input type="radio" id="rdEnabledYes" value="1" checked="true" runat="server" name="IsEnabled" class="regular-check" /><label for="rdEnabledYes"></label><i class="t">启用</i>
                        <input type="radio" id="rdEnabledNo" class="regular-check" runat="server" name="IsEnabled" value="0" /><label for="rdEnabledNo"></label><i class="t">禁用</i>
                    </div>
                </li>
                
                <li class="lb fl">
                    <i class="name">订单是否审核</i>
                    <div class="single">
                        <input type="radio" id="rdAuditYes" class="regular-check" runat="server" name="audit" value="1" /><label for="rdAuditYes"></label><i class="t">需要审核</i>
                        <input type="radio" id="rdAuditNo" class="regular-check" runat="server" name="audit" value="0" checked="true" /><label for="rdAuditNo"></label><i class="t">无需审核</i>
                    </div>
                </li>
                <li class="lb fl">
                    <i class="name">启用授信</i>
                    <div class="single">
                        <input type="radio" runat="server" name="Credit" value="1" id="rdCreditYes" class="regular-check" /><label for="rdCreditYes"></label><i class="t">启用</i>
                        <input type="radio" runat="server" id="rdCreditNo" name="Credit" checked="true" value="0" class="regular-check" /><label for="rdCreditNo"></label><i class="t">禁用</i>
                    </div>
                </li>
                <li class="lb fl tdCredit">
                    <i class="name">授信额度</i>
                    <div class="single">
                        <input type="text" class="box2" id="txtCreditAmount" maxlength="18" runat="server" onkeyup="KeyInt(this)" name="txtCreditAmount" />
                    </div>
                </li>
                <%--<li class="lb clear">
    	<i class="name fl">上传三证</i>
    	 <div class="con upload">
           <a href="javascript:;" class="a-upload bclor le"> <input id="uploadFile" runat="server" type="file" name="fileAttachment"  class="AddBanner"/>上传附件</a>
             <i class="gclor9">（附件最大20M，支持格式：PDF、Word、Excel、Txt、JPG、PNG、BMP、GIF、RAR、ZIP）</i>
             <asp:Panel runat="server" ID="DFile" Style="margin: 5px 5px;">
              </asp:Panel>
              </div>
         
         <div id="UpFileText" style="margin-left:100px;">
          </div>
            <input runat="server" id="HidFfileName" type="hidden" />
     </li>--%>
            </ul>

            <!--参数设置 end-->

            <div class="blank60"></div>
            <div class="btn-box">
                <div class="btn">
                    <a href="javascript:void(0);" class="btn-area" id="btnAdds">提交</a>
                    <a href="javascript:void(0);" class="gray-btn" onclick="javascript:history.go(-1);">取消</a>
                </div>
                <asp:Button ID="btnAdd" CssClass="" runat="server" OnClientClick="return formCheck()" OnClick="btnAdd_Click" />

                <asp:Button ID="btnAddDis" CssClass="" runat="server" OnClick="btnAddDis_Click" />

                <div class="bg"></div>
            </div>

        </div>

        <!--遮照层-->
        <div class='Layer'>
        </div>
        <!--确定用户 start-->
        <div class="tip" style="display: none; margin-top: 250px" id="SetUser">
            <div style="position: absolute; top: 0; left: 0; right: 0; bottom: 0; z-index: 999; background: #fff;">
                <div class="tiptop">
                    <span>绑定现有用户</span><a></a>
                </div>
                <div class="tipinfo">
                    <div style="padding-left: 50px">
                        <span class="lb">该手机号已经绑定登录用户(绑定登录用户名:<label style="color: red" id="UserName"></label>)</span><br />
                        <span class="lb">你只能用此登录用户作为该代理商的登录用户,或更换手机号,请确认!</span>
                    </div>
                    <div class="tipbtn">
                        <asp:Button ID="PhoneUserBtn" CssClass="orangeBtn" runat="server" Text="确定" OnClick="btnAdd_Click" />&nbsp;
               
                        <input name="" type="button" class="cancel" value="取消" />
                    </div>
                </div>
            </div>
            <div style="z-index: 11; opacity: 0.3; filter: Alpha(Opacity=30) !important; border-radius: 5px; top: 242px; left: -8px; right: -8px; bottom: -8px; background-color: rgb(0, 0, 0); position: absolute; top"></div>
        </div>
        <input type="hidden" id="useridtext" value="0" />
        <input type="hidden" id="userid" value="0" runat="server" />

        <!--确定用户 end-->



    </form>
</body>
</html>
