<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DisInfo.aspx.cs" Inherits="Company_SysManager_DisInfo" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/CompRemove.ascx" TagPrefix="uc1" TagName="CompRemove" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>代理商详情</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/UploadJs.js" type="text/javascript"></script>
    <script src="../../js/InputSearchData.js" type="text/javascript"></script>

    <script src="../../js/MoveLayer.js" type="text/javascript"></script>
    <style type="text/css">
        .pullDown2 {
            border: 1px solid #e5e5e5;
            width: 152px;
            background: #fff;
            position: absolute;
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

        #More {
            cursor: pointer;
        }
    </style>
    <script>
        $(function () {
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

            if ('<%=Request["type"] %>' == "1") {
                $(".rightinfo").css("width", "auto");
                $(".tools").css("display", "none");
            }
            $("#uploadFile").AjaxUploadFile({ Src: "UploadFile/", ShowDiv: "UpFileText", ResultId: "HidFfileName", AjaxSrc: "../../Controller/Fileup.ashx", maxlength: 5, DownSrc: "../../" });
            //新增代理商等级
            $(".addBtn").click(function () {
                var index = layerCommon.openWindow('业务员新增', '../PmtManager/SaleManEdit.aspx?lefttype=3&posttype=1', '900px', '400px');
                $("#hid_Alert").val(index);
            });
            $(".txtSaleMan").InputSearchData({
                callBackData: function (value, callback) {
                    $.ajax({
                        type: "post",
                        data: { action: "GetSaleMan", Value: value },
                        dataType: 'json',
                        cache: false,
                        success: function (data) {
                            callback(data);
                        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                            return;
                        }
                    })
                },
                SetValueID: "HidSM",
                JsonName: "name",
                JsonID: "id",
                JsonType: "type",
                IsReset: true,
                ShowMaxHeight: "150px",
                AddShowHeight: 2
            })
            //查询积分记录
            $("#Integral").click(function () {

                var index = layerCommon.openWindow('代理商积分', 'DisIntegral.aspx?DisId=<%=KeyID %>', '850px', '450px');  //记录弹出对象
                $("#hid_Alert").val(index); //记录弹出对象
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

            //合同
            $("#liContract").click(function () {
                window.location.href = '../Contract/ContractEdit.aspx?disid=<%= DisID%>';

                //var index = layerCommon.openWindow("上传合同", "../Contract/UplodeContract.aspx", "770px", "250px");  //记录弹出对象
                //$("#hid_Alert").val(index); //记录弹出对象
            })

            //查看合同附件
            $(document).on("click", ".selectContractFile", function () {
                var cid = $(this).parent().find(".ContractID").val();
                var index = layerCommon.openWindow("查看附件", "selecttContractFile.aspx?cid="+cid+"", "770px", "250px");  //记录弹出对象
                $("#hid_Alert").val(index); //记录弹出对象
            })

        });
        $(document).ready(function () {

            var div = $('<div   Msg="True"  style="z-index:1000; background-color:#000; opacity:0.3; filter:alpha(opacity=30);position: fixed;width:100%;height:100%;top:0px;left:0px;"></div>');
            $("li#libtnDel").on("click", function () {
                layerCommon.confirm("确认删除？", function () { $("#btnDel").trigger("click"); }, "提示");
            });
            $("#liSMBind").on("click", function () {
                $("body").append(div);
                $("div.AddWindow").css("height", "120px").fadeIn(200).find(".tiptop").LockMove({ MoveWindow: "#SaleWindow" });
            })

            $(".AddWindow .cancel,.AddWindow .tiptop a").bind("click", function (event) {
                $(div).remove();
                $("div.AddWindow").fadeOut(200);
            })

            $("li#libtnNUse").on("click", function () {
                layerCommon.confirm("确认禁用？", function () { $("#btnNUse").trigger("click"); }, "提示");
            })
            $("li#libtnUse").on("click", function () {
                layerCommon.confirm("确认启用？", function () { $("#btnUse").trigger("click"); }, "提示");
            })

            if ('<%=Request["nextstep"] %>' == "1") {
                Orderclass("ktxzjxs");
                document.getElementById("imgmenu").style.display = "block";
            }

            $("li#libtnAudit").on("click", function () {
                location.href = 'DisEdit.aspx?KeyID=<%= DisID %>&type=<%=Request["type"] %>';
            })
            $("li#libtnEdit").on("click", function () {
                location.href = 'DisEdit.aspx?nextstep=<%=Request["nextstep"] %>&KeyID=<%= DisID %>&type=<%=Request["type"] %>';
            })

        });


        function ssave() {
            if ('<%=Request["nextstep"] %>' == "1") {
                window.location.href = 'DisList.aspx?nextstep=1';
            }
            else {
                window.location.href = 'DisEdit.aspx';
            }
        }

        function formCheck() {
            var str = "";
            if ($.trim($("#txtSaleMan").val()) == "") {
                str = "请选择业务员";
            }
            if (str != "") {
                layerCommon.msg(str, IconOption.错误);
                return false;
            }
        }
        function save(name, id) {
            LayerClose();
            $(".txtunit").val(name);
            $("#HidSM").val(id);
            $.ajax({
                type: "post",
                url: "DisInfo.aspx",
                data: { ck: Math.random(), action: "addUnit" },
                dataType: "json",
                success: function (data) {
                    $(".pullDown2 .list").html(data);
                }, error: function () { }
            })
        }

        function LayerClose() {
            layerCommon.layerClose($("#hid_Alert").val());
        }
        //  重复上传判断
        function uploadFileClick() {
            if ($("#HidFfileName").val() != "" && $("#HidFfileName").val() != undefined) {
                layerCommon.msg("请勿重复上传！", IconOption.哭脸);
                return false;
            }
            else
                return true;
        }
        //长传合同回调方法
        function insertContract(FileName, txtForceDate, txtInvalidDate) {
            $.ajax({
                type: "post",
                data: { action: "insertContract", FileName: FileName, txtForceDate: txtForceDate, txtInvalidDate: txtInvalidDate },
                dataType: 'json',
                cache: false,
                success: function (data) {
                    if (data.ret == "ok")
                        layerCommon.msg("上传合同成功", IconOption.笑脸);
                    else
                        layerCommon.msg(data.ret, IconOption.哭脸);
                }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                    return;
                }
            })

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <uc1:CompRemove runat="server" ID="Remove" ModuleID="2" />
        <input type="hidden" id="HidSM" runat="server" />
        <input type="hidden" id="hid_Alert" />
        <uc1:Top ID="top1" runat="server" ShowID="nav-4" />


      <div class="rightinfo">
         <div class="place">
	        <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
                <li><a href="../SysManager/DisList.aspx" runat="server" id="btitle">代理商查询</a></li><li>></li>
                <li><a href="#" runat="server" id="Atitle">代理商详情</a></li>
	        </ul>
     </div>

            <div class="tools">
                <ul class="toolbar left">
                    <li id="libtnAudit" runat="server"><span>
                        <img src="../images/t15.png" /></span><label>审核</label></li>
                    <li id="liContract" runat="server"><span>
                        <img src="../images/t02.png" /></span>合同</li>
                    <li id="libtnEdit" runat="server"><span>
                        <img src="../images/t02.png" /></span>编辑</li>
                    <li id="libtnDel" runat="server"><span>
                        <img src="../images/t03.png" /></span>删除</li>
                    <li id="liSMBind" runat="server"><span>
                        <img src="../../Company/images/t17.png" /></span>销售人员绑定</li>
                    <li id="libtnUse" runat="server"><span>
                        <img src="../images/nlock.png" /></span>启用</li>
                    <li id="libtnNUse" runat="server"><span>
                        <img src="../images/lock.png" /></span>禁用</li>
                    <li id="lblbtnback" runat="server" onclick="ssave()"><span>
                        <img src="../images/tp3.png" /></span>返回</li>
                    <li id="libtnNext" runat="server" style="display: none;">
                        <a href="javascript:void(0);" onclick="onlinkOrder('../GoodsNew/GoodsEdit.aspx?nextstep=1','spxz')"><span>
                            <img src="../images/t07.png" /></span><font color="red">下一步</font></a></li>
                </ul>
                <%--<input type="button" runat="server" id="btnAudit" style="display:none;" onserverclick="btn_AuditClick" />--%>
                <input type="button" runat="server" id="btnUse" onserverclick="btn_Use" style="display: none;" />
                <input type="button" runat="server" id="btnNUse" onserverclick="btn_NUse" style="display: none;" />
                <input type="button" runat="server" id="btnDel" onserverclick="btn_Del" style="display: none;" />
            </div>



<!--代理商信息 start-->
<div class="c-n-title">代理商信息</div>
<ul class="coreInfo">
	<li class="lb fl"><i class="name"><i class="red">*</i>代理商名称</i>
    <input type="text" class="box1" id="lblDisName" readonly="readonly" runat="server" /></li>
	<li class="lb fl"><i class="name">代理商编码</i><input type="text" class="box1" id="DisCode" readonly="readonly" runat="server" /></li>
    <li class="lb clear">
    	<i class="name fl"><i class="red">*</i>详细地址</i>
    	
        <input type="text" class="box1" id="lblAddress" readonly="readonly" runat="server" style="margin-left:5px" />
    </li>

    <li class="lb fl">
    	<i class="name">代理商分类</i>
       
        <input type="text" class="box1" id="lblTyoeName" readonly="readonly" runat="server" />
    </li>
	<%--<li class="lb fl"><i class="name">代理商等级</i>
       
        <input type="text" class="box1" id="lblDisLevel" readonly="readonly" runat="server" />
	</li>--%>
    <li class="lb fl">
    	<i class="name">代理商区域</i>
         <input type="text" class="box1" id="lblAreaName" readonly="readonly" runat="server" />
    </li>
    <li class="lb fl gd none"><i class="name">联系人</i><input type="text" class="box1" id="lblPerson" readonly="readonly" runat="server" /></li>
    <li class="lb fl gd none"><i class="name">联系人手机</i><input type="text" class="box1" id="lblPhone" readonly="readonly" runat="server" /></li>
    <li class="lb fl gd none"><i class="name">邮编</i><input type="text" class="box1" id="lblZip" readonly="readonly" runat="server" /></li>
    <li class="lb fl gd none"><i class="name">传真</i><input type="text" class="box1" id="lblFax" readonly="readonly" runat="server" /></li>
    <li class="lb fl gd none"><i class="name">备注</i><input type="text" class="box1" id="lblRemark" readonly="readonly" runat="server" /> </li>
    <li class="lb fl"><i class="name"></i><i class="more" id="More">更多功能</i></li>
</ul>
<!--代理商信息 end-->
<div class="blank10"></div>
<!--参数设置　start-->
<div class="c-n-title">参数设置</div>
<ul class="coreInfo">
	<li class="lb fl">
    	<i class="name"><i class="red">*</i>启用帐号</i>
         <input type="radio"  checked="true"  class="regular-check" /><label for="rdEnabledYes"></label>
    	<i><label runat="server" id="lblIsEnabled"></label></i>
     </li>
     <li class="lb fl">
    	<i class="name">订单是否审核</i>
          <input type="radio"  checked="true"  class="regular-check" /><label for="rdEnabledYes"></label>
    	<i><label runat="server" id="rdAuditYes"></label></i>
        
     </li>
     <li class="lb fl">
    	<i class="name">启用授信</i>
         <input type="radio" checked="true"   class="regular-check" /><label for="rdEnabledYes"></label>
    	<i><label runat="server" id="rdCreditYes" style=" min-width:30px;"></label></i>
       
     </li>
     <li class="lb fl  tdCredit">
    	<i class="name">授信额度</i>
    	 <span id="spanCreditAmount" runat="server" style=" display:inline">企业授信额度</span>
         <input type="text" class="box2" id="lblCreditAmount" readonly="readonly" runat="server" />
     </li>
     <li class="lb clear">
    	<%--<i class="name fl">上传三证</i>
    	   <asp:Panel runat="server" id="DFile" style=" margin-left:5px;"></asp:Panel>--%>
     </li>
</ul>
         
<!--参数设置 end-->



            <div>

                 <div class="div_title" >
                            代理商管理员:
                  </div>


                <table class="tablelist">
                    <thead>
                        <tr>
                            <th class="t3">登录帐号</th>
                            <th class="t3">姓名</th>
                            <%--<th>性别</th>--%>
                            <th class="t3">是否审核</th>
                            <th class="t3">用户类型</th>
                            <th class="t3">手机号码</th>
                            <%-- <th>身份证</th>--%>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="Rpt_User" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <div class="tc"><%# Eval("UserName")%></div>
                                    </td>
                                    <td>
                                        <div class="tc"><%# Eval("TrueName")%></div>
                                    </td>
                                    <%-- <td><%# Eval("Sex") %></td>--%>
                                    <td>
                                        <div class="tc"><%# Eval("AuditState").ToString() == "2" ? "是" : "<span style='color:red'>否</span>"%></div>
                                    </td>
                                    <td>
                                        <div class="tc"><%# Eval("Type").ToString() == "5" ? "管理员" : "用户"%></div>
                                    </td>
                                    <td>
                                        <div class="tc"><%# Eval("Phone")%></div>
                                    </td>
                                    <%--   <td><%# Eval("Identitys")%></td>--%>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>

                <div class="pagin" style="height: 30px;">
                    <webdiyer:AspNetPager ID="Pager" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                        NextPageText=">" PageSize="5" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
                        ShowPageIndexBox="Always" TextAfterPageIndexBox="<span class='pagiPage'>页</span>"
                        TextBeforePageIndexBox="<span class='jump'>跳转到:</span>"
                        CustomInfoSectionWidth="20%" CustomInfoStyle="padding:5px 0 0 20px;cursor: default;color: #737373;" CustomInfoClass="message" ShowCustomInfoSection="Left"
                        CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                        CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5"
                        OnPageChanged="Pager_PageChanged">
                    </webdiyer:AspNetPager>
                </div>
            </div>


            <div class="blank20"></div>
            <div class="c-n-title">合同信息</div>
            <div class="blank20"></div>
            <table class="tablelist">
                <thead>
                    <tr>
                        <th class="t3">合同编码</th>
                        <th class="t3">生效日期</th>
                        <th class="t3">失效日期</th>
                        <th class="t3">附件(查看)</th>
                    </tr>
                </thead>
                <tbody runat="server" id="UpFileText">
                    <asp:Repeater ID="ContractRep" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <div class="tc"><%# Eval("contractNO")%></div>
                                </td>
                                <td>
                                    <div class="tc"><%# Convert.ToDateTime( Eval("ForceDate")).ToString("yyyy-MM-dd")%></div>
                                </td>
                                <td>
                                    <div class="tc"><%#Convert.ToDateTime(  Eval("InvalidDate")).ToString("yyyy-MM-dd")%></div>
                                </td>
                                <td>
                                    <div class="tc"><input  type="hidden" class="ContractID" value="<%# Eval("ID")%>"/>
                                        <a class="selectContractFile"  href="javascript:;">查看</a></div>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
              <div class="pagin" style="height: 30px;">
                    <webdiyer:AspNetPager ID="Page1" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                        NextPageText=">" PageSize="5" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
                        ShowPageIndexBox="Always" TextAfterPageIndexBox="<span class='pagiPage'>页</span>"
                        TextBeforePageIndexBox="<span class='jump'>跳转到:</span>"
                        CustomInfoSectionWidth="20%" CustomInfoStyle="padding:5px 0 0 20px;cursor: default;color: #737373;" CustomInfoClass="message" ShowCustomInfoSection="Left"
                        CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                        CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5"
                        OnPageChanged="Pager_PageChanged">
                    </webdiyer:AspNetPager>
                </div>


            <div class="AddWindow tip" id="SaleWindow" style="display: none;">
                <div style="position: absolute; top: 0; left: 0; right: 0; bottom: 0; z-index: 999; background: #fff;">
                    <div class="tiptop">
                        <span>绑定销售人员</span>
                        <i class="addBtn" style="float: right; margin-right: 35px; cursor: pointer;"><i class="addIcon" style="font-weight: bold;">+</i>新增销售人员</i>
                        <a></a>
                    </div>
                    <div class="tipinfo">
                        <%--    <div class="lb">
                    <span><i class="required">*</i>销售人员类型：</span>
                    <asp:DropDownList runat="server" CssClass="downBox" style=" width:auto;" ID="ddlSMType">
                    <asp:ListItem Selected="True" Text="业务员" Value="1"></asp:ListItem>
                    <asp:ListItem Selected="True" Text="业务经理" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                </div>--%>
                        <div class="lb">
                            <span><i class="required">*</i>选择销售人员：</span>
                            <input name="txtunit" type="text" id="txtSaleMan" autocomplete="off" maxlength="50"
                                runat="server" class="textBox txtSaleMan" />
                            <%--<div class="pullDown2" style="display: none;">
                                <ul class="list">
                                    <asp:Repeater ID="rptUnit" runat="server">
                                        <ItemTemplate>
                                            <li><a href="javascript:;">
                                                <%# Common.BindManName(Convert.ToInt32(Eval("ID").ToString()),CompID)%><input type="hidden" value='<%# Eval("ID").ToString()%>' /></a></li></ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                                <a href="javascript:;" class="addBtn"><i class="addIcon"></i>新增</a>
                            </div>--%>
                        </div>
                        <div class="tipbtn">
                            <asp:Button ID="btnAdd" CssClass="orangeBtn" runat="server" Text="确定" OnClientClick="return  formCheck();"
                                OnClick="btnSMBind_Click" />&nbsp;
                    <input name="" type="button" class="cancel" value="取消" />
                        </div>
                    </div>
                </div>
                <div id="xubox_border1" class="xubox_border" style="z-index: 11; opacity: 0.3; filter: Alpha(Opacity=30) !important; border-radius: 5px; top: -8px; left: -8px; right: -8px; bottom: -8px; background-color: rgb(0, 0, 0); position: absolute; top"></div>
            </div>
    </form>
</body>
</html>
