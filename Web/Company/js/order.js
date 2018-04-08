/*订单*/
$(document).ready(function () {
    //table 行样式
    $('.tablelist tbody tr:odd').addClass('odd');

     $(".liSenior").unbind().on("click", function () {
        $("div.hidden").slideToggle(100);
    })

    //商品促销提示
    $(document).on({
        "mouseover":function(e){
            var proID=$(this).attr("tip");
            var pro_type=$(this).attr("tip_type");
            var th=this;
            $.ajax({
                type: 'post',
                url: '../../Handler/GoodsInfoPrice.ashx?action=pro',
                data: { proID: proID,pro_type:pro_type },
                async: false, //true:同步 false:异步
                success: function (result) {
                    var data = eval('(' + result + ')');
                    var falg=data["falg"];
                    if(falg=="True"){
                        var lb=data["TheLabel"];
                        $(th).append(lb);
                        var X = e.pageX;
                        if(X<500){
                           $(th).find("i").css({"left":"0px"});
                        }else{
                           $(th).find("i").css({"right":"0px"});}
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    layerCommon.msg("服务器异常，请稍后再试",IconOption.哭脸);
                }
            });
        },
        "mouseout":function(e){
            $(this).find("i").remove();
        } 
    },".ProIcon,.sale");
});

//订单生成详细页面
function goInfo(Id) {
    window.location.href = 'OrderCreateInfo.aspx?KeyID=' + Id+"&go=1";
}

//账单生成详细页面
function goInfo_ZD(Id) {
    window.location.href = 'OrderZdtsInfo.aspx?KeyID=' + Id+"&go=1";
}

//订单审核详细页面
function goAuditInfo(Id) {
    window.location.href = 'OrderAuditInfo.aspx?KeyID=' + Id+"&go=1";
}

//返回
function cancel() {
    //window.location.href = 'OrderCreateList.aspx';
    history.go(-1);
}

//查看返利
function ReBateLog() {
    var height = document.body.clientHeight; //计算高度
    var layerOffsetY = (height - 450) / 2; //计算宽度

    var index = layerCommon.openWindow('查看返利', '../../Distributor/DisReBate.aspx', '750px', '380px'); //记录弹出对象
    $("#hid_Alert").val(index); //记录弹出对象
}

//修改发票信息
function upBill(Id){
    var height = document.body.clientHeight; //计算高度
    var layerOffsetY = (height - 450) / 2; //计算宽度

    var ID=$(Id).attr("tip");
    var index = layerCommon.openWindow('发票信息', 'updateBill.aspx?KeyID=' + ID , '520px', '280px'); //记录弹出对象
    $("#hid_Alert").val(index); //记录弹出对象
}

//日志
function Log(KeyId, CompId) {
    var height = document.body.clientHeight; //计算高度
    var layerOffsetY = (height - 450) / 2; //计算宽度

    var index = layerCommon.openWindow('订单日志', '../../BusinessLog.aspx?LogClass=Order&ApplicationId=' + KeyId + '&CompId=' + CompId, '750px', '380px'); //记录弹出对象
    $("#hid_Alert").val(index); //记录弹出对象
}
//日志
function Zd_Log(KeyId, CompId) {
    var height = document.body.clientHeight; //计算高度
    var layerOffsetY = (height - 450) / 2; //计算宽度

    var index = layerCommon.openWindow('账单日志', '../../BusinessLog.aspx?LogClass=Order&ApplicationId=' + KeyId + '&CompId=' + CompId, '750px', '380px'); //记录弹出对象
    $("#hid_Alert").val(index); //记录弹出对象
}

//批量删除
function Del() {
    $("#btnVolumeDel").trigger("click");
}

//物流信息
function Express() {
    CloseDialog();
    //alert(window.location.href)
    $("#btnExpress").trigger("click");
}

//js 判断是否选中checkbox
function fromDel(title, msg, conOK) {
    var delId = { Id: "" };
    var chck = $(".tablelist > tbody > tr > td > input[type=checkbox]:checked");

    $.each(chck, function (index, chebox) {

        var $Id = $(chebox).siblings("input[type=hidden]:eq(0)");

        if ($Id.length > 0) {
            delId.Id += $Id.val() + ",";
        }
    });

    if (delId.Id.length <= 0) {
        errMsg(title, "请选择要删除的选项", "", "");
        return false;
    } else {
        confirm(msg, conOK, title);
    }
}

/*订单生成 start */
$(document).ready(function () {
    //选择商品
    $("#btnGoods").click(function () {
        //$(".tip2").fadeIn(200);
        //$(".opacity").fadeIn(200);
        var DisID = $(".hid_DisId").val();
        var CompId = $("#hidCompId").val();

        if (DisID.toString() == "") {
            layerCommon.msg("请选择代理商", IconOption.感叹);
            return false;
        }

        //转向网页的地址; 
        var url='/Company/Order/GoodsAdd.aspx?DisId=' + DisID + "&CompId=" + CompId;                             
        var name='选购商品';                     //网页名称，可为空; 
        var iWidth=920;                          //弹出窗口的宽度; 
        var iHeight=600;                         //弹出窗口的高度; 
        //获得窗口的垂直位置 
        var iTop = (window.screen.availHeight - 30 - iHeight) / 2; 
        //获得窗口的水平位置 
        var iLeft = (window.screen.availWidth - 10 - iWidth) / 2; 

        //2016-02-24修改
        var height = document.body.clientHeight; //计算高度
        var layerOffsetY = (height - 450) / 2; //计算宽度
        var index = layerCommon.openWindow('订单选购商品', '/Company/Order/GoodsAdd.aspx?DisId=' + DisID + "&CompId=" + CompId, '1000px', '550px'); //记录弹出对象

        $("#hid_Alert").val(index); //记录弹出对象

    });

    //选择加盟商
    $("#txtDisID").click(function () {
        ///获取光标位置
        var Id = $("#hidCompId").val();

        var height = document.body.clientHeight; //计算高度
        var layerOffsetY = (height - 450) / 2; //计算宽度
        var index = layerCommon.openWindow('选择代理商', '/Company/UserControl/SelectDisList.aspx?compid=' + Id , '880px', '460px'); //记录弹出对象
        $("#hid_Alert").val(index); //记录弹出对象
    });

    //选择收货地址
    $("#txtAddrID").click(function () {
        ///获取光标位置
        var x = $("#txtAddrID").offset().left;
        var y = $("#txtAddrID").offset().top;

        var disId = $(".hid_DisId").val();

        if (disId == "") {
            layerCommon.msg("请选择代理商",IconOption.错误);
            return false;
        }
       
        ChoseProductClass('/Company/Order/Tree_Addr.aspx?Id=' + disId, x, y);
    });
});
//选择区域
function ChoseProductClass(url, x, y) {
    if ($("#hid_Alert1").val() != null && $("#hid_Alert1").val() != "") {
       layerCommon.layerClose("hid_Alert1");
    }
    //获取滚动条高度
    var yy = $(document).scrollTop();
    y = parseInt(y) - parseInt(yy);

    //获取光标位置
    x = parseInt(x) + "px";
    y = parseInt(y + 30) + "px";

    var index = ChooseTreeDialog(url, '450px', '250px', y, x);
    $("#hid_Alert1").val(index);
}
//关闭选择区域
function CloseProductClass(type, txtId, hidId, id, name) {

    $("#" + txtId).focus(); //解决 IE11 弹出层后文本框不能输入
     

    if (type == "Dis") {
        //代理商默认地址
        CloseDialog();
        DefaultAddr(id, 0);
        $("#" + txtId).val(name); //区域名称
        $("#" + hidId).val(id); //区域id
    } else if (type == "Addr") {
        //选择地址
        CloseDialogAddr()
        DefaultAddr(0, id);
    }else if(type == "addAddr"){
        //新增地址
        CloseDialogAddAddr()
        DefaultAddr(0, id);
    }
}
//关闭选择代理商区域
function selectDis(id, name){
    if(id.toString()!=""){
        $(".txt_txtDisname").focus(); //解决 IE11 弹出层后文本框不能输入
        layerCommon.layerClose("hid_Alert");
        //代理商默认地址
        DefaultAddr(id, 0);
        $(".txt_txtDisname").val(name); //区域名称
        $(".hid_DisId").val(id); //区域id
    }else{
       $(".txt_txtDisname").focus(); //解决 IE11 弹出层后文本框不能输入
       layerCommon.layerClose("hid_Alert");
       $(".txt_txtDisname").val(name); //区域名称
       $(".hid_DisId").val(id); //区域id
       $("#txtAddrID_txt_product_class").val("");
       $("#txtAddrID_hid_product_class").val("");
    }
}


//代理商默认地址
function DefaultAddr(DisId, AddrId) {
    
    $.ajax({
        type: 'post',
        url: '/Company/Order/OrderCreateAdd.aspx?action=Addr',
        data: { DisId: DisId, AddrId: AddrId },
        async: false,
        success: function (data, status) {

            var result = eval('(' + data + ')');

            if (parseInt(AddrId) == 0) {
                var ds = result["ds"];
                if (ds == "True") {
                    //$("#ddlOtype").val(-1);
                    //$("#optionOtype").css("display", "block");
                    //$("#optionOtype").show();
                    //$("#optionOtype").add();
                    //document.getElementById("optionOtype").style.display='block';

                    $("#ddlOtype").empty();
                    var obj = document.getElementById("ddlOtype"); 
                    obj.add(new Option("请选择","-1"));
                    obj.add(new Option("销售订单","0"));
                    obj.add(new Option("赊销订单","1"));
                    obj.add(new Option("特价订单","2"));

                } else {
                    //$("#ddlOtype").val(-1);
                    //$("#optionOtype").css("display", "none");
                    //document.getElementById("optionOtype").style.display='none';
                    //$("#optionOtype").remove();  
                    
                    $("#ddlOtype").empty();
                    var obj = document.getElementById("ddlOtype"); 
                    obj.add(new Option("请选择","-1"));
                    obj.add(new Option("销售订单","0"));
                    //obj.add(new Option("赊销订单","1"));
                    obj.add(new Option("特价订单","2"));
                }

                $("#ddlOtype").val("0");
                $("#lblTotalAmount").text("0.00");
                $("#hidTotalAmount").val();

                $(".tablelist table tr").each(function(index,obj)
                {
                 if(index>0){
                   $(obj).remove();
                 }
                })
            }
            $("#txtAddrID_txt_product_class").val(result["Address"]);
            $("#txtAddrID_hid_product_class").val(result["AddrID"]);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            layerCommon.msg("服务器异常，请稍后再试",IconOption.错误);
        }
    });
}

//显示查询代理商是否可赊销
function Otype(ds,otype) {
    if (ds == "True") {
        $("#ddlOtype").val(-1);
        //$("#optionOtype").css("display", "block");

        //var obj = document.getElementById("ddlOtype"); 
        //obj.add(new Option("赊销订单","1"));
        //obj.options[obj.options.length].Id="optionOtype";

    } else {
        $("#ddlOtype").val(-1);
        //$("#optionOtype").css("display", "none");

        $("#optionOtype").remove();   
    }
    $("#ddlOtype").val(otype);
}

//订单生成的订单类型
function Otypechange() {
    var Otype = $("#ddlOtype").val();
    if (parseInt(Otype) == 2) {
        $(".lblPrice").css("display", "none");
        $(".spanPrice").css("display", "block");
        $(".txtPrice").css("display", "block");
    } else {
        $(".lblPrice").css("display", "block");
        $(".spanPrice").css("display", "none");
        $(".txtPrice").css("display", "none");
    }
}

function CloseDialog() {
    //$("#btn_del").focus(); //解决 IE11 弹出层后文本框不能输入
    var showedDialog = $("#hid_Alert").val(); //获取弹出对象
    layerCommon.layerClose(showedDialog); //关闭弹出对象
    //$(".txt_count").focus();
}

function CloseDialogAddr() {
    //$("#btn_del").focus(); //解决 IE11 弹出层后文本框不能输入
    var showedDialog = $("#hid_Alert1").val(); //获取弹出对象
    layerCommon.layerClose(showedDialog); //关闭弹出对象
    //$(".txt_count").focus();
}

function CloseDialogAddAddr() {
    //$("#btn_del").focus(); //解决 IE11 弹出层后文本框不能输入
    var showedDialog = $("#hid_Alert2").val(); //获取弹出对象
    layerCommon.layerClose(showedDialog); //关闭弹出对象
    //$(".txt_count").focus();
}

function CloseDialogAddAddr1(keyid) {
    //$("#btn_del").focus(); //解决 IE11 弹出层后文本框不能输入
    var showedDialog = $("#hid_Alert2").val(); //获取弹出对象
    layerCommon.layerClose(showedDialog); //关闭弹出对象
    window.parent.location.href = "returnorderinfo.aspx?KeyID="+keyid;
    //$(".txt_count").focus();
}

//生成订单时 选择商品
function selectGoods(Id) {
    
    layerCommon.layerClose("hid_Alert");//2016-02-24修改
    if(Id.toString()==""){
        //没有选中商品，直接返回
        return;
    }

    //alert(Id);
    //window.location.href = 'OrderCreateAdd.aspx?action=GoodsInfo&';
    //$("#hidgoodsInfo").val(Id);
    //关闭选择商品窗口
   
    $("#btnGoodsInfo").trigger("click");
}
/*订单生成 End*/


/*订单审核 start*/

//审核
function Audit(Id) {
    //关闭选择商品窗口
    CloseDialog();
    //window.location.herf = '../Order/OrderAuditInfo.aspx?KeyID=' + Id;
    $("#btnAudit").trigger("click");
 }

 //审核 --退回
function RAudit(Id) {
    //关闭选择商品窗口
    CloseDialog();
    //window.location.herf = '../Order/OrderAuditInfo.aspx?KeyID=' + Id;
    $("#btnRAudit").trigger("click");
 }

 /*订单审核 End*/


 /*订单退货审核 start*/
 function AuditReturn(Id) {
     //关闭选择商品窗口
     CloseDialog();
     //window.location.herf = '../Order/OrderAuditInfo.aspx?KeyID=' + Id;
     $("#btnAudit").trigger("click");
 }
 /*订单退货审核 end*/



function ShowMenu(obj, n) {
    var Nav = obj.parentNode;
    if (!Nav.id) {
        var BName = Nav.getElementsByTagName("ol");
        var HName = Nav.getElementsByTagName("h2");
        var t = 2;
    } else {
        var BName = document.getElementById(Nav.id).getElementsByTagName("span");
        var HName = document.getElementById(Nav.id).getElementsByTagName(".header");
        var t = 1;
    }
    for (var i = 0; i < HName.length; i++) {
        HName[i].innerHTML = HName[i].innerHTML.replace("-", "+");
        HName[i].className = "";
    }
    obj.className = "h" + t;
    for (var i = 0; i < BName.length; i++) { if (i != n) { BName[i].className = "no"; } }
    if (BName[n].className == "no") {
        BName[n].className = "";
        obj.innerHTML = obj.innerHTML.replace("+", "-");
    } else {
        BName[n].className = "no";
        obj.className = "";
        obj.innerHTML = obj.innerHTML.replace("-", "+");
    }
}
//-->

function getAddress(address,id){
    $("#hidaddr").val(id);
    $("#ddladdr").append("<option selected = 'selected' value="+id+">"+address+"</option>");
    parent.layerCommon.layerClose("hid_Alert2");
}