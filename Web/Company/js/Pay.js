

//关闭选择区域
function CloseProductClass(type, txtId, hidId, id, name) {
    $("#" + txtId).focus(); //解决 IE11 弹出层后文本框不能输入
    CloseDialog();
    $("#" + txtId).val(name); //区域名称
    $("#" + hidId).val(id); //区域id

}


function CloseDialog() {
    //$("#btn_del").focus(); //解决 IE11 弹出层后文本框不能输入
    var showedDialog = $("#hid_Alert").val(); //获取弹出对象
    closeDialog(showedDialog); //关闭弹出对象
    //$(".txt_count").focus();
}
//关闭选择银行窗口
function CloseBankDialog() {
    //$("#btn_del").focus(); //解决 IE11 弹出层后文本框不能输入
    var showedDialog = $("#hid_alerts").val(); //获取弹出对象
    closeDialog(showedDialog); //关闭弹出对象
    //$(".txt_count").focus();
}
//日志
function Log(KeyId, CompId) {
    var height = document.body.clientHeight; //计算高度
    var layerOffsetY = (height - 450) / 2; //计算宽度

    var index = showDialog('订单日志', '../../BusinessLog.aspx?LogClass=PrePayment&ApplicationId=' + KeyId + '&CompId=' + CompId, '750px', '450px', layerOffsetY); //记录弹出对象
    $("#hid_Alert").val(index); //记录弹出对象
}

//---------------------------------收款账户管理页面js方法-------begin--------------------
