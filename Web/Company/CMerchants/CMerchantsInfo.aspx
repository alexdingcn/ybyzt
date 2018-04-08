<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CMerchantsInfo.aspx.cs" Inherits="Company_CMerchants_CMerchantsInfo" %>

<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>招商信息</title>
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

</head>
<body>
    <form id="form1" runat="server">
        <uc1:Top ID="top1" runat="server" ShowID="nav-2" />
        <div class="rightinfo">
            <div class="info">
                <a href="../jsc.aspx">我的桌面</a>><a href="javascript:;">招商信息</a>
            </div>
            <div class="tools">
                <ul class="toolbar left">
                    <li id="libtnShangjia" runat="server"><span><img src="../images/t12.png" /></span>上架</li>
                    <li id="libtnXianjia" runat="server"><span><img src="../images/t11.png" /></span>下架</li>
                    <li id="libtnEdit" runat="server"><span><img src="../images/t02.png" /></span>编辑</li>
                    <li id="lblbtnback" runat="server"><span><img src="../images/tp3.png" /></span>返回</li>
                    <asp:Button ID="btnShangjia" CssClass="" runat="server"  OnClick="btnShangjia_Click" />
                    <asp:Button ID="btnXianjia" CssClass="" runat="server"  OnClick="btnXianjia_Click" />
                </ul>
            </div>

            <ul class="coreInfo">
                <li class="lb fl">
                    <i class="name"><i class="red">*</i>招商编码</i>
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
                    <i class="name">商品大类</i>
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
                <li class="lb fl" id="litype" runat="server" visible="false">
                    <i class="name">指定选择</i>
                    <input type="text" maxlength="50" style="margin-left:2px" class="box1" id="txtFc" runat="server" readonly="readonly"/>
                </li>
                <li class="lb fl">
                    <i class="name">需提供资料</i>
                    <input name="chk" value="1" disabled="disabled" runat="server" id="chk1" type="checkbox"/><label for="chk1">营业执照</label>
                    <input name="chk" value="2" disabled="disabled" runat="server" id="chk2" type="checkbox"/><label for="chk2">医疗器械经营许可证</label>
                    <input name="chk" value="3" disabled="disabled" runat="server" id="chk3" type="checkbox"/><label for="chk3">开户许可证</label>
                    <input name="chk" value="4" disabled="disabled" runat="server" id="chk4" type="checkbox"/><label for="chk4">医疗器械备案</label>
                </li>
                <li class="lb fl">
                    <i class="name">备 注</i>
                    <input type="text" style="margin-left:3px; width:650px; height:80px;" readonly="readonly" class="box1" id="txtRemark" runat="server" />
                </li>
            </ul>
        </div>
    </form>
    <script>
        $(function () {
            $("li#libtnEdit").on("click", function () {
                location.href = 'CMerchantsAdd.aspx?KeyID=<%= Common.DesEncrypt(this.KeyID.ToString(), Common.EncryptKey) %>';
            })

            $("li#lblbtnback").on("click", function () {
                location.href = 'CMerchantsList.aspx';
            })

            $("li#libtnShangjia").on("click", function () {
                $("#btnShangjia").trigger("click");
            })

            $("li#libtnXianjia").on("click", function () {
                $("#btnXianjia").trigger("click");
            })
            
        })
    </script>
</body>
</html>
