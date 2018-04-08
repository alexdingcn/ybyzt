<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FirstCampInfo.aspx.cs" Inherits="Company_CMerchants_FirstCampInfo" %>

<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>首营详情</title>

    <link href="../css/style.css" rel="stylesheet" type="text/css" />
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
    
    <script type="text/javascript">
        $(function () {
            $("#uploadFile").AjaxUploadFile({ Src: "UploadFile/", ShowDiv: "UpFileText", ResultId: "HidFfileName", AjaxSrc: "../../Controller/Fileup.ashx", maxlength: 5, DownSrc: "../../" });


            //订单生成 搜索
            $("#liSearch").on("click", function () {
                $("#btnSearch").trigger("click");
            });

        });

        function uploadFileClick() {
            if ($("#HidFfileName").val() != "" && $("#HidFfileName").val() != undefined) {
                window.parent.layerCommon.msg("请勿重复上传！", window.parent.IconOption.哭脸);
                return false;
            }
            else
                return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <uc1:Top ID="top1" runat="server" ShowID="nav-2" />

        <asp:Button ID="btnSearch" Text="搜索" runat="server" OnClick="btnSearch_Click" Style="display: none" />

        <input type="hidden" id="hid_Alert" />
        <div class="rightinfo">
            <div class="info">
                <a href="../jsc.aspx">我的桌面</a>><a href="javascript:;">首营详情</a>
            </div>

            <ul class="coreInfo">
                <li class="lb fl">
                    <i class="name">招商编码</i>
                    <input type="text" class="box1" placeholder="" readonly="readonly" runat="server" maxlength="50" id="txtCMCode"/>
                </li>
                <li class="lb fl">
                    <i class="name">招商名称</i>
                    <input type="text" maxlength="50" style="margin-left:2px" readonly="readonly" class="box1" id="txtCMName" name="txtCMName" runat="server" />
                </li>
                <li class="lb fl">
                    <i class="name">商品编码</i>
                    <input type="text" class="box1" placeholder="" runat="server" readonly="readonly" maxlength="50" id="txtGoodsCode"/>
                </li>
                <li class="lb fl">
                    <i class="name">商品分类</i>
                    <input type="text" maxlength="50" style="margin-left:2px" readonly="readonly" class="box1" id="txtCategoryID" name="txtCategoryID" runat="server" />
                </li>
                <li class="lb fl">
                    <i class="name">商品名称</i>
                    <input type="text" class="box1" placeholder="" runat="server" readonly="readonly" maxlength="50" id="txtGoodsName"/>
                </li>
                <li class="lb fl">
                    <i class="name">规格型号</i>
                    <input type="text" maxlength="50" style="margin-left:2px" readonly="readonly" class="box1" id="txtValueInfo" name="txtValueInfo" runat="server" />
                </li>
                <li class="lb fl">
                    <i class="name">生效日期</i>
                    <input type="text" class="box1" placeholder="" runat="server"  maxlength="50" id="txtForceDate" readonly="readonly"/>
                </li>
                <li class="lb fl">
                    <i class="name">失效日期</i>
                    <input type="text" maxlength="50" style="margin-left:2px" class="box1" id="txtInvalidDate" runat="server" readonly="readonly"/>
                </li>
                <li class="lb fl">
                    <i class="name">类型</i>
                    <select id="ddrtype"  style=" width:380px;" class="box1 p-box" disabled="disabled" runat="server">
                        <option value="1">公开</option>
                        <option value="2">指定区域</option>
                        <option value="3">指定代理商</option>
                    </select>
                </li>
                <li class="lb fl">
                    <i class="name">需提供资料</i>
                    <label id="lblchk1" runat="server" visible="false">
                    <input name="chk" value="1" disabled="disabled" runat="server" id="chk1" type="checkbox"/><label for="chk1">营业执照</label></label>
                    <label id="lblchk2" runat="server" visible="false">
                    <input name="chk" value="2" disabled="disabled" runat="server" id="chk2" type="checkbox"/><label for="chk2">医疗器械经营许可证</label></label>
                     <label id="lblchk3" runat="server" visible="false">
                    <input name="chk" value="3" disabled="disabled" runat="server" id="chk3" type="checkbox"/><label for="chk3">开户许可证</label></label>
                    <label id="lblchk4" runat="server" visible="false">
                    <input name="chk" value="4" disabled="disabled" runat="server" id="chk4" type="checkbox"/><label for="chk4">医疗器械备案</label></label>
                </li>
                <li class="lb fl">
                    <i class="name">备 注</i>
                    <input type="text" style="margin-left:3px" readonly="readonly" class="box1" id="txtRemark" maxlength="20" runat="server" />
                </li>
            </ul>
            <div class="blank10"></div>
            <div class="c-n-title">申请供应商
				<ul class="toolbar right">
                        <li id="liSearch"><span>
                            <img src="../../Company/images/t04.png"></span>搜索</li> 
                 </ul>
				<ul class="toolbar3">
                        <li>代理商:<input id="txtComName" type="text" runat="server" class="textBox"/>&nbsp;&nbsp;</li>
                        <li>医院:<input id="txtContractNO" type="text" runat="server"  class="textBox"/>&nbsp;&nbsp;</li>
                </ul>
			</div>
            <div class="tabLine" id="GoodsUL">
                <div class="productListBox table-wrap-lite" style="float: left; margin-bottom: 5px;">
                    <table border="0" cellspacing="0" cellpadding="0" style="width:1050px;margin:0 auto;">
                        <thead>
                            <tr>
                                <th class="t3">代理商名称</th>
                                <th class="t3">联系人</th>
                                <th class="t3">联系电话</th>
                                <th class="t3">申请日期</th>
                                <th class="t3">医院</th>
                                <th class="t3">提供资料</th>
                                <th class="t3">授权书</th>
                                <th class="t3">代理商区域</th>
                                <th class="t3">申请备注</th>
                                <th class="t3">处理备注</th>
                                <th class="t5">处理</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptFc" runat="server">
                                <ItemTemplate>
                                    <tr>
                                         <td class="trOp"><div class="tc"><%# Eval("DisName")%></div></td>
                                         <td class="trOp"><div class="tc"><%# Eval("Principal")%></div></td>
                                         <td class="trOp"><div class="tc"><%# Eval("Phone")%></div></td>
                                         <td class="trOp"><div class="tc"><%# Eval("CreateDate","{0:yyyy-MM-dd}")%></div></td>
                                         <td class="trOp"><div class="tc"><%# Eval("HospitalName")%></div></td>
                                         <td class="trOp"><div class="tc"><a style=" cursor:pointer;" tip="<%# Eval("id") %>" class="bclor fcamp" state="<%# Eval("State") %>">查看</a></div></td>
                                         <td class="trOp">
                                            <div class="tc">
                                                <%# Queryannex(Eval("ID").ToString()) %>
                                                <a  class="bclor addauth" style=" cursor:pointer;" tip="<%# Eval("ID") %>">上传</a>
                                            </div>
                                         </td>
                                         <td class="trOp">
                                             <div class="tc">
                                                <%# Common.GetDisAreaNameById(Eval("AreaID").ToString().ToInt(0)) %>
                                             </div>
                                         </td>
                                         <td class="trOp"><div class="tc"><%# Eval("ApplyRemark")%></div></td>
                                         <td class="trOp"><div class="tc"><%# Eval("Remark")%></div></td>
                                         <td class="trOp"><div class="tc">  
                                            <%# Eval("State").ToString() == "0" ? "<a htid='" + Eval("HtID") + "' cmtip='" + Eval("CMID") + "' tip='" + Eval("ID") + "' class='bclor fcampAudit' style='cursor:pointer;'>处理</a>" : Eval("State").ToString() == "1" ? "<i style=\"color:#ff0000;\">驳回</i>" : "<i style=\"color:#00DB00;\">通过</i>"%>
                                         </div></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </div>
            <div>
                <ul class="coreInfo">
                    <li class="lb fl">
                        <i> 通过：上传授权书后点击通过按钮</i><br />
                        <i> 驳回：点击驳回按钮并填写理由</i>
                    </li>
                </ul>
            </div>
        </div>

        <script>
            $(function () {
                //查看代理商资料
                $(document).on("click", ".fcamp", function () {
                    var id = $.trim($(this).attr("tip"));
                    var state=$.trim($(this).attr("state"));
//                    if(state==="1"){
//                        layerCommon.msg("代理商没有提交不能查看",IconOption.哭脸);
//                        return false;
//                    }else{
                        var url = 'FirstCampMvg.aspx?KeyID=' + id;
                        var index = layerCommon.openWindow("代理商资料", url, '575px', '615px'); //记录弹出对象
                        $("#hid_Alert").val(index); //记录弹出对象
//                    }
                });


                //首营信息处理
                $(document).on("click", ".fcampAudit", function () {
                    var id = $.trim($(this).attr("tip"));
                    var cmID = $.trim($(this).attr("cmtip"));
                    var htid=$.trim($(this).attr("htid"));

                    $.ajax({
                        type: "Post",
                        url: "FirstCampInfo.aspx/QrFC",
                        data: "{ 'cmID':'" + cmID + "', 'compID':'" + <%= this.CompID %>+ "', 'htid':'" + htid + "'}",
                        dataType: "json",
                        async: false,
                        timeout: 5000,
                        contentType: "application/json; charset=utf-8",
                        success: function (ReturnData) {
                            var Json = eval('(' + ReturnData.d + ')');
                            var url= 'FirstCampAudit.aspx?KeyID='+id+"&htid="+htid;
                            if (Json.result) {
                                var index = layerCommon.openWindow("首营信息处理", url, '750px', '415px'); //记录弹出对象
                                $("#hid_Alert").val(index); //记录弹出对象
                            } else {
                                layerCommon.confirm(Json.code,function(){
                                     var index = layerCommon.openWindow("首营信息处理", url, '750px', '415px'); //记录弹出对象
                                     $("#hid_Alert").val(index); //记录弹出对象
                                });
                            }
                        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                            var a;
                        }
                    });
                });

                //授权书
                $(document).on("click", ".addauth", function () {
                    var id = $.trim($(this).attr("tip"));

                    var url = 'authMvg.aspx?KeyID=' + id;
                    var index = layerCommon.openWindow("授权书上传", url, '575px', '415px'); //记录弹出对象
                    $("#hid_Alert").val(index); //记录弹出对象
                });
            })

            function Refresh() {
                window.location.reload();
             }
        </script>
    </form>
</body>
</html>
