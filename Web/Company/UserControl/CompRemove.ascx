<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CompRemove.ascx.cs" Inherits="Company_UserControl_CompRemove" %>
<script type="text/javascript">
    function cancel() {
        document.getElementById("imgmenu").style.display = "none";
    }
    function todkxd() {
        layerCommon.alert("恭喜您成功入驻医站通，您的商城您做主，尽情享受您的电商之旅吧！", IconOption.笑脸, undefined, function () { onlinkOrder('../newOrder/orderBuy.aspx', 'dkxd') });
    }
</script>
<div id="imgmenu" class="stepImg" style="display: none; position: absolute; right: 8px;
    top: 15%; position: fixed; z-index: 19999;">
    <div class="title">
        <i>开通步骤</i><a href="javascript:void(0);" onclick="cancel()" class="close"></a></div>
    <ul class="list">
        <li class="atop" id="szgwqx"><a href="javascript:void(0);" onclick="onlinkOrder('../SysManager/RoleList.aspx?nextstep=1','ktszgwqx')"
            class="alink icon1"></a><i><a href="javascript:void(0);" onclick="onlinkOrder('../SysManager/RoleList.aspx?nextstep=1','ktszgwqx')">
                设置岗位权限</a></i></li>
        <li id="xzsp"><a href="javascript:void(0);" onclick="onlinkOrder('../GoodsNew/GoodsEdit.aspx?nextstep=1','ktxzsp')"
            class="alink icon3"></a><i><a href="javascript:void(0);" onclick="onlinkOrder('../GoodsNew/GoodsEdit.aspx?nextstep=1','ktxzsp')">
                新增商品</a></i><em class="two"></em></li>
        <li id="xzjxs"><a href="javascript:void(0);" onclick="onlinkOrder('../SysManager/DisEdit.aspx?nextstep=1','ktxzjxs')"
            class="alink icon2"></a><i><a href="javascript:void(0);" onclick="onlinkOrder('../SysManager/DisEdit.aspx?nextstep=1','ktxzjxs')">
                新增代理商</a></i><em class="one"></em></li>
        <li id="bdskzh"><a href="javascript:void(0);" onclick="onlinkOrder('../Pay/PayAccountList.aspx?nextstep=1','ktbdskzh')"
            class="alink icon4"></a><i><a href="javascript:void(0);" onclick="onlinkOrder('../Pay/PayAccountList.aspx?nextstep=1','ktbdskzh')">
                绑定收款帐号</a></i><em class="three"></em></li>
        <li id="zxsc"><a href="javascript:void(0);" onclick="onlinkOrder('../SysManager/CompEdit.aspx?nextstep=1','ktzxsc')"
            class="alink icon5"></a><i><a href="javascript:void(0);" onclick="onlinkOrder('../SysManager/CompEdit.aspx?nextstep=1','ktzxsc')">
                装修店铺</a></i><em class="four"></em></li>
        <li><a href="javascript:void(0);" onclick="todkxd()" class="alink icon6"></a><i><a
            href="javascript:void(0);" onclick="todkxd()">开启电商之旅</a></i></li>
    </ul>
</div>
