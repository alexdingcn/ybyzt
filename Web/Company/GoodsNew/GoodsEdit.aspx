<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GoodsEdit.aspx.cs" ValidateRequest="false" Inherits="Company_Goods_GoodsEdit" %>

<%@ Register Src="~/Company/UserControl/TreeDemo.ascx" TagPrefix="uc1" TagName="TreeDemo" %>
<%@ Register Src="~/Company/UserControl/CompRemove.ascx" TagPrefix="uc1" TagName="CompRemove" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%if (KeyID == 0)
            { %>商品新增<%}
                      else
                      {%>商品编辑<%}%></title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1;charset=utf-8;" />
    <link href="../css/global-2.0.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />

    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>

    <script src="../../js/ajaxfileupload.js" type="text/javascript"></script>
    <script src="../../kindeditor/kindeditor.js" type="text/javascript"></script>
    <script src="../../kindeditor/lang/zh_CN.js" type="text/javascript"></script>
    <script src="../../kindeditor/plugins/code/prettify.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../js/OpenJs.js" type="text/javascript"></script>
    <script src="../../js/xss.js"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../js/classifyview.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
        <script src="../../js/UploadJs.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
     <script>
        $(function () {
            if ('<%=Request["nextstep"] %>' == "1") {
                document.getElementById("imgmenu").style.display = "block";
            }
            $("#uploadFile2").AjaxUploadFile({ Src: "UploadFile/", ShowDiv: "UpFileText2", ResultId: "HidFfileName2", AjaxSrc: "../../Controller/Fileup.ashx", maxlength: 5, DownSrc: "../../" });


            removeClass(); //去掉SKU td的odd样式
            //商品分类宽度设置
//            $(".showDiv .ifrClass").css("width", "175px");
//            $(".showDiv").css("width", "170px");
//            $('iframe').load(function () {
//                $('iframe').contents().find('.pullDown').css("width", "170px");
            //            })

            //删除单行规格属性
            $(document).on("click", ".del-i-a", function () {
                $(this).parents(".mulSpecItem").find(".delMulSpec").click();
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
            //绑定商品分类
            $("#txt_product_class").click(function () {
                var dataviews=<%=GoodsType%>
               handleChange(this,dataviews);
            });

            //新增计量单位
            $(document).on("click",".addBtn",function(){
               $(".tip").fadeIn(200);
                $(".txtunits").focus();
                $(".txtunits").val("");
                $(".Layer").fadeIn(200); 
                var $This = $(this),HTML;
                $.ajax({
                    type: 'POST',
                    data: { action: "Get_Doc" },
                    dataType: "json",
                    timeout: 5000,
                    cache: false,
                    success: function (ReturnData) {
                        if (typeof(ReturnData)=="object") {
                            $("tbody","#jldwdel").empty();
                            $.each(ReturnData,function(index,Rdata){
                                HTML ="<tr>";
                                HTML +='<td style="width:198px;height:30px;text-align:center;">'+Rdata.AtVal+'</td>';
                                HTML +='<td style="width:198px;height:30px;text-align:center;"><a class="Rpt_Del" style="color: #2e70d3;" data-text="'+Rdata.AtVal+'" data-key="'+Rdata.ID+'" href="javascript:;">删除</a></td>';
                                HTML +="</tr>";
                                $("tbody","#jldwdel").append(HTML);
                            })
                        }
                    }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                        layerCommon.msg("删除失败", IconOption.错误);
                    }
                });
             
            });

            //取消计量单位
            $(".cancel,.tiptop a").click(function () {
                $(".tip").fadeOut(100);
                $(".Layer").fadeOut(100);
            });
            if ($(".pullDown2 .list li").length > 6) {//超过6个单位计量，则出现滚轴
                $(".pullDown2 .list").css("height", "156px");
            }
            //计量单位
            //$(".txtunit").click(function () {
            //    if ($.trim($(".pullDown2").attr("class")) == "pullDown2 xy") {
            //        $(".pullDown2").hide();
            //        $(".pullDown2").removeClass("xy");
            //        $(".pullDown2").prop("display", "none");
            //    } else {
            //        $(".pullDown2").show();
            //        $(".pullDown2").addClass("xy");
            //        $(".pullDown2").prop("display", "block");
            //    }
            //    var x = $(".txtunit").offset().left;
            //    var y = $(".txtunit").offset().top;
            //    // alert(x + "," + y);
            //    $(".pullDown2").css("position", "absolute");
            //    $(".pullDown2").css("left", x - 5 + "px");
            //    $(".pullDown2").css("top", y + 30 + "px");
            //})
            $(document).on("click", ".pullDown2 .list li", function () {
                $(".txtunit").val($.trim($(this).text()));
                $(".pullDown2").hide();
                $(".pullDown2").removeClass("xy");
            })

            $(document).on("click",".select p",function(){
                var tex=$(this).html();
                $("#txtunittex").val(tex);
                $("#txtunit").val(tex);
            })
            //验证新增计量单位
            $(".btnAddUnit").click(function () {
                var unit = $(".txtunits").val();
                if (unit == "") {
                                    layerCommon.msg("计量单位不能为空", IconOption.错误);
                    return false;
                }
                $.ajax({
                    type: "post",
                    url: "/Handler/GoodsEdit.ashx",
                    data: { ck: Math.random(), action: "addUnit", unit: unit },
                    dataType: "json",
                    success: function (data) {
                        var html = "";
                        $(data).each(function (index, obj) {
                            if (obj.AtVal == "sb") {
                                                   layerCommon.msg("计量单位添加失败", IconOption.错误);
                            } else if (obj.AtVal == "ycz") {
                                                   layerCommon.msg("计量单位已存在", IconOption.错误);
                            } else if (obj.AtVal == "cc") {
                                                   layerCommon.msg("计量单位下拉加载失败", IconOption.错误);
                            } else {
                                html += "<p class=\"\"> "+obj.AtVal+"</p>";
                                if (index == 0) {
                                    $("#txtunit").val(obj.AtVal);
                                    $("#txtunittex").val(obj.AtVal);
                                }
                            }
                        })
                        if (html != "") {
                            $(".select").hide();
                            html+="<span class=\"add-more\">新增单位<a href=\"javascript:;\" class=\"addBtn\"></a> </span>";
                            $(".select").html(html);
                            $(".tip").fadeOut(100);
                            $(".Layer").fadeOut(100);
                        }
                    }, error: function () { }
                })
                return false;
            })
           
           // $(".ui-chk").nextAll().hide(); //默认多规格隐藏
            //控制输入规格值的文本框样式
            $(document).on("click", ".selectize-input", function () {
                $(this).addClass("focus");
                $(this).addClass("input-active");
                $(this).find("input").css("opacity", "1");
                $(this).find("input").css("position", "relative");
                $(this).find("input").css("left", "0px");
                $(this).find("input").focus();
            })
            // 输入规格值的文本框blur事件 清空值和隐可以选择值得div
            $(document).on("blur", ".selectize-input>input", function () {
                setTimeout(function () {
                    $(".selectize-input>input").parent().removeClass("focus");
                    $(".selectize-input>input").parent().removeClass("input-active");
                    $(".selectize-input>input").val("");
                    $(".selectize-input>input").css("width", "4px");
                    $(".selectize-dropdown").hide();
                }, 100);
            })
            // 输入规格值的文本框抬起事件 可以选择值得div赋值
            $(document).on("keyup", ".selectize-input>input", function (e) {
                var index = $(this).parent().parent().parent().parent().index(); //规格索引
                if (e.keyCode == 13) {//回车键事件
                    $(".create").trigger("click"); //选中值
                    $(".mulSpecList .mulSpecItem:eq(" + index + ")").find(".selectize-input>input").trigger("blur"); //清楚层
                    $(".mulSpecList .mulSpecItem:eq(" + index + ")").find(".selectize-input").trigger("click"); //文本框选中
                }
//                if(e.keyCode==8){
//                    $(this).prev().remove();
//                    var str=$(this).parent().parent().prev(".selectized").val();
//                   if($.trim(str)!=""){
//                        str=str.substring(0,str.lastIndexOf("@@"));
//                   }
//                    $(this).parent().parent().prev(".selectized").val(str);
//                  TableSku(); //sku信息
//                }
                var str = $.trim($(this).val());
                if (str != "") {
                    var count = $.trim($(this).val().length);
                    $(this).css("width", parseInt(count) * 20 + "px");
                    $(".mulSpecList .mulSpecItem:eq(" + index + ")").find(".selectize-dropdown").css("display", "block");
                    $(".selectize-dropdown").css("width", "415px");
                   // $(".selectize-dropdown").css("top", "42px");
                    $(".selectize-dropdown").css("left", "0px");
                    $(".selectize-dropdown").css("visibility", "visible");
                    var html = "<div class=\"create\" >新增规格值： <strong>" + str + "</strong>…使用键盘“回车键”确认并添加多个规格值</div>";
                    $(".mulSpecList .mulSpecItem:not(" + index + ")").find(".selectize-dropdown-content").html("");
                    $(".mulSpecList .mulSpecItem:eq(" + index + ")").find(".selectize-dropdown-content").html(html);
                } else {
                    $(".selectize-dropdown").hide();
                }
            })
            //选中属性值div得click事件
            $(document).on("click", ".create", function () {
                var index = $(this).parent().parent().parent().parent().parent().index(); //规格索引
                var bool = false;
                var str = $.trim($(this).find("strong").text());
                var strshow = $.trim($(this).find("strong").text());
                if(str.indexOf(',')!=-1)
                {
                    var reg = new RegExp(",","g");//g,表示全部替换
                    str=str.replace(reg,"@@");
                }
                var strlist = ""; //隐藏的规格值
                $(".mulSpecList .mulSpecItem:eq(" + index + ")").find(".selectize-input>.item").each(function (index, obj) {
                    var value = $(this).attr("data-value");
                    if(value.indexOf(',')!=-1)
                    {
                        var reg = new RegExp(",","g");//g,表示全部替换
                        value=value.replace(reg,"@@");
                    }
                    if (str == value) {
                        bool = true; //重复的属性值则不添加
                        return false;
                    } else {
                        strlist += value + "@@";
                    }
                })
                if (!bool) {
                    var html = "<i class=\"o-t item\" data-value=\"" + strshow + "\">" + strshow + "<i  tabindex=\"-1\" class=\"remove close\"></i></i>";
                    $(".mulSpecList .mulSpecItem:eq(" + index + ")").find(".selectize-input>input").before(html);
                    $(".mulSpecList .mulSpecItem:eq(" + index + ")").find(".selectize-input").addClass("focus");
                    $(".mulSpecList .mulSpecItem:eq(" + index + ")").find(".selectize-input").addClass("has-options");
                    $(".mulSpecList .mulSpecItem:eq(" + index + ")").find(".selectize-input").addClass("has-items");
                    $(".mulSpecList .mulSpecItem:eq(" + index + ")").find(".selectized").val(strlist + str);
                }
                $(".mulSpecList .mulSpecItem:eq(" + index + ")").find(".selectize-input").trigger("click");
                TableSku(); //sku信息
            })
            var bols=false;
            //判断是否可以添加规格
            $.ajax({
            type:"post",
            url:"/Handler/GoodsEdit.ashx",
            data:{ck:Math.random(),action:"isAddattr",goodsid:<%=KeyID %>},
            dataType:"text",
            success:function(data){
            if(data=="ycz"){
            $(".addSpec").attr("title", "已有订单存在该商品，不能添加规格"); //禁用
            $(".addSpec").css("cursor", "not-allowed"); //禁用鼠标样式
            bols=true;
            }else if(data=="bcz"){      
          
            }else{
                layerCommon.alert(data, IconOption.哭脸);
                                return false;
            }
            },error:function(){
            
            }

            })
            //删除该属性值
            $(document).on("click", ".remove", function () {
            var $this=this;
                var str = "";
                var strvalue = $.trim($(this).parent().attr("data-value"));//属性值
                if(<%=KeyID %>==0){//新增
                 var strlist = $(this).parent().parent().parent().prev(".selectized").val();
                        if (strlist != "") {
                            for (var i = 0; i < strlist.split('@@').length; i++) {
                                if (strlist.split('@@')[i] != strvalue)
                                    str += strlist.split('@@')[i] + "@@";
                            }
                        }
                        $(this).parent().parent().parent().prev(".selectized").val(str);
                        $(this).parent().remove();
                        TableSku(); //sku信息
                }else{
                        $.ajax({
                        type:"post",
                        url:"/Handler/GoodsEdit.ashx",
                        data:{ck:Math.random(),action:"isDelAttrInfo",attrinfo:strvalue,goodsid:<%=KeyID %>},
                        dataType:"text",
                        success:function(data){
                            if(data==""){
                                 var strlist = $($this).parent().parent().parent().prev(".selectized").val();
                                if (strlist != "") {
                                    for (var i = 0; i < strlist.split('@@').length; i++) {
                                        if (strlist.split('@@')[i] != strvalue)
                                            str += strlist.split('@@')[i] + "@@";
                                    }
                                }
                                $($this).parent().parent().parent().prev(".selectized").val(str);
                                $($this).parent().remove();
                                TableSku(); //sku信息
                            }else
                            {
                                layerCommon.alert(data, IconOption.哭脸);
                                return false;
                            }
                        },
                        error:function(){
                         layerCommon.alert("出错了", IconOption.哭脸);
                                return false;
                        }
                      })
               }
            })
            //删除属性
            $(document).on("click", ".delMulSpec", function () {
          var   $this=this;
            if(<%=KeyID %>==0){
                var count = $(this).parent().parent().find(".mulSpecItem").length;
                if (count == 1) {
                    // layerCommon.msg("规格启用时,最少保留一个", IconOption.错误);
                   // return false;
                   $(this).parent().remove();
                   $("#chkProduct").removeAttr("checked");
                   $("#divProduct").hide();
                   $("#divSku").hide();
                }
                $(".addSpec").show(); //添加规格按钮显示
                $(".addSpecLi").attr("style","display:block;")
                $(this).parent().remove();
                // DelGuige(count); //删除规格的同时 动态删除sku信息
                TableSku(); //sku信息
                }else{
                var strvalue=$(this).prev().find(".mulSpecName").val();//当前行的规格
                 $.ajax({
                        type:"post",
                        url:"/Handler/GoodsEdit.ashx",
                        data:{ck:Math.random(),action:"isDelAttr",attr:strvalue,goodsid:<%=KeyID %>},
                        dataType:"text",
                        success:function(data){
                            if(data==""){
                             var count = $($this).parent().parent().find(".mulSpecItem").length;
                if (count == 1) {
                    // layerCommon.msg("规格启用时,最少保留一个", IconOption.错误);
                   // return false;
                   $($this).parent().remove();
                   $("#chkProduct").removeAttr("disabled");
                   $("#chkProduct").removeAttr("checked");
                   $("#ddlTemplate").removeAttr("Enabled");
                   $("#divProduct").hide();
                   $("#divSku").hide();
                     }
                $(".addSpec").show(); //添加规格按钮显示
                $(".addSpecLi").attr("style","display:block;")
                                 $($this).parent().remove();
                                    // DelGuige(count); //删除规格的同时 动态删除sku信息
                                    TableSku(); //sku信息
                            }else
                            {
                                layerCommon.alert(data, IconOption.哭脸);
                                return false;
                            }
                        },
                        error:function(){
                         layerCommon.alert("出错了", IconOption.哭脸);
                                return false;
                        }
                      })
                }

            })
        
            //添加规格
            $(".addSpec").click(function (e) {
                if(bols){
              return false;
            }
              
                var count = $(".mulSpecList .mulSpecItem").length;
                if (count >= 3) {
                    $(this).hide();
                    $(".addSpecLi").attr("style","display:none;")
                }else
                {
                    //var html = "<div class=\"mulSpecItem\"><div class=\"mulSpecProperty\"><input type=\"text\" class=\"ui-input-dashed mulSpecName\" style=\"width: 96px;\" maxlength=\"4\"  value=\"\" name=\"mulSpecName\"></div><a class=\"delMulSpec\"></a><div class=\"mulSpecValues\"><input type=\"text\" value=\"\" tabindex=\"-1\" class=\"mulSpecInp selectized\" name=\"selectized\" style=\"display: none;\"><div class=\"selectize-control mulSpecInp multi plugin-remove_button\"><div class=\"selectize-input items not-full\"><input type=\"text\" style=\"width: 4px;\" tabindex=\"\" autocomplete=\"off\"></div><div style=\"display: none;\" class=\"selectize-dropdown multi mulSpecInp plugin-remove_button\"><div class=\"selectize-dropdown-content\"></div></div></div></div><div class=\"cb\"></div></div>";
                    var html = "<div class=\"mulSpecItem\"><div class=\"mulSpecProperty\" style=\"width:150px;height: auto;min-height:35px\">"+
     "<input type=\"text\" value=\"\" maxlength=\"4\"  placeholder=\"规格名称(4字内) \" class=\"ui-input-dashed mulSpecName box2\" name=\"mulSpecName\" style=\"height: auto;min-height:35px\"/>"+
     "</div><a class=\"delMulSpec\" style=\"display:none\"></a> <div class=\"mulSpecValues\" style=\"width:680px;\" >"+
     "<input type=\"text\" style=\"display: none;\" class=\"mulSpecInp selectized\" name=\"selectized\" tabindex=\"-1\" value=\"\" maxlength=\"15\"/>"+
     " <div class=\"selectize-control mulSpecInp multi plugin-remove_button\" >"+
     " <div class=\"selectize-input items not-full box1 fl\" style=\"width:720px;height: auto;min-height:30px\" placeholder=\"使用键盘“回车键”确认并添加多个规格值\"> "+ 
     "<input type=\"text\" autocomplete=\"off\" tabindex=\"\" style=\"width: 4px;\" maxlength=\"15\"/><i class=\"del-i del-i-a\"></i>"+
    " </div><div class=\"selectize-dropdown multi mulSpecInp plugin-remove_button\" style=\"display: none;\">"+
     "<div class=\"selectize-dropdown-content\">\</div> </div> </div></div> <div class=\"cb\"></div> </div>"
                    $(".mulSpecList").append(html);
                }
                count = $(".mulSpecList .mulSpecItem").length;
                if (count >= 3) {
                    $(this).hide();
                    $(".addSpecLi").attr("style","display:none;")
                }
                // addGuige(count); //添加规格的同时 动态添加sku信息
                TableSku(); //sku信息
            })
            //复选框事件
            $("#chkProduct").click(function () {
          var  $this=this;
                $.ajax({
                type:"post",
                url:"/Handler/GoodsEdit.ashx",
                data:{ck:Math.random(),action:"isChk",goodsid:<%=KeyID %>},
                dataType:"text",
                success:function(data){
                    if(data==""){
                        if ($($this).attr("checked") == undefined) {
                            $("#divSku").removeClass("none")
                            $($this).attr("checked", true);
                            //var html = "<div class=\"mulSpecItem\"><div class=\"mulSpecProperty\"><input type=\"text\" class=\"ui-input-dashed mulSpecName\" style=\"width: 96px;\" maxlength=\"4\"  value=\"\" name=\"mulSpecName\"></div><a class=\"delMulSpec\"></a><div class=\"mulSpecValues\"><input type=\"text\" value=\"\" tabindex=\"-1\" class=\"mulSpecInp selectized\" name=\"selectized\" style=\"display: none;\"><div class=\"selectize-control mulSpecInp multi plugin-remove_button\"><div class=\"selectize-input items not-full\"><input type=\"text\" style=\"width: 4px;\" tabindex=\"\" autocomplete=\"off\"></div><div style=\"display: none;\" class=\"selectize-dropdown multi mulSpecInp plugin-remove_button\"><div class=\"selectize-dropdown-content\"></div></div></div></div><div class=\"cb\"></div></div>";
                            var html = "<div class=\"mulSpecItem\"><div class=\"mulSpecProperty\" style=\"width:150px;height: auto;min-height:35px\">"+
             "<input type=\"text\" value=\"\" maxlength=\"4\"  placeholder=\"规格名称(4字内) \" class=\"ui-input-dashed mulSpecName box2\" name=\"mulSpecName\" style=\"height: auto;min-height:35px\"/>"+
             "</div><a class=\"delMulSpec\" style=\"display:none\"></a> <div class=\"mulSpecValues\" style=\"width:680px;\" >"+
             "<input type=\"text\" style=\"display: none;\" class=\"mulSpecInp selectized\" name=\"selectized\" tabindex=\"-1\" value=\"\" maxlength=\"15\"/>"+
             " <div class=\"selectize-control mulSpecInp multi plugin-remove_button\" >"+
             " <div class=\"selectize-input items not-full box1 fl\" style=\"width:720px;height: auto;min-height:30px\" placeholder=\"使用键盘“回车键”确认并添加多个规格值\"> "+ 
             "<input type=\"text\" autocomplete=\"off\" tabindex=\"\" style=\"width: 4px;\" maxlength=\"15\"/><i class=\"del-i del-i-a\"></i>"+
            " </div><div class=\"selectize-dropdown multi mulSpecInp plugin-remove_button\" style=\"display: none;\">"+
             "<div class=\"selectize-dropdown-content\">\</div> </div> </div></div> <div class=\"cb\"></div> </div>"
                            $(".mulSpecList").append(html);
                    } else {
                            $($this).attr("checked", false);
                            $("#divSku").addClass("none")
                    }
                    if ($($this).attr("checked")) {
                        $("#ddlTemplate").val("");
                        //$(".ui-chk").nextAll().show(); //显示多规格
                        $("#divProduct").css("display","inline");
                        $("#divSku").show();
                      // $("#addSpec").click();
                        $(".mulSpecList .mulSpecItem:gt(0)").remove(); //除了第一个规格都删除
                        $(".addSpec").show(); //添加规格按钮显示
                        $(".addSpecLi").attr("style","display:block;")
                        $(".ui-input-dashed").val(""); //清空规格
                        $(".selectize-input .item").remove(); //清空所有属性值
                        $(".selectize-input").removeClass("has-items"); //div高度
                        $(".trCode").hide(); //商品编码隐藏
                    } else {
                        $(".trCode").show(); //商品编码显示
                        $(".ui-chk").nextAll().hide(); //多规格隐藏
                        $(".selectized").val("");
                
                     var style="";
                        if(true){
                        style="style='display:none'";
                        }
                        $('.productListBox').html("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"width:1050px;margin:0 auto;\"><thead><tr><th class=\"key\"></th><th>商品编码</th><th>销售价格(元)</th><th "+style+" class=\"kc\"><i class=\"red\">*</i>库存</th><th "+style+" class=\"t5\">批次号</th><th "+style+" class=\"t5\">有效期</th><th>是否上架</th><th>操作</th></tr></thead><tbody><tr><td class=\"key\">1</td><td class=\"trOp\"><input name=\"txtCode\" style=\"width:120px;\" type=\"text\" id=\"Text2\" class=\"textBox txtCode\" value='<%=GoodsCode() %>' maxlength=\"15\"/></td><td class=\"trOp\"><input name=\"txtPrices\" style=\"width:100px;\" type=\"text\" id=\"Text1\" class=\"textBox txtPrices\" onkeyup=\"KeyInt2(this);\" maxlength=\"10\"/></td><td class=\"trOp\" "+style+"><input name=\"txtInventory\" style=\"width:100px;\" type=\"text\" id=\"Text1\" class=\"textBox txtInventory\" onkeyup=\"KeyInt2(this);\" maxlength=\"11\"/></td><td "+style+" class=\"trOp\"><input name=\"txtBatchNO\" type=\"text\"  style=\"width: 100px;\" class=\"dataBox txtBatchNO\" maxlength=\"11\" /></td><td class=\"trOp\" "+style+"><input name=\"txtvalidDate\" onclick=\"WdatePicker()\" readonly=\"readonly\" type=\"text\" style=\"width: 100px;\" class=\"dataBox txtvalidDate\" maxlength=\"11\" /></td><td class=\"trOp\"><div class=\"tc\"><input type=\"checkbox\"  name=\"isOffline\" value=\"1\" checked=\"checked\" id=\"check-2-1\" class=\"r-check\" /><label for=\"check-2-1\"></label></div> <input type=\"hidden\" value=\"1\" name=\"hidIsOffline\" /> <input value=\"0\" name=\"hidId\" class=\"deleteIDlist\" type=\"hidden\"/></td><td class=\"trOp\"><a class=\"theme-color delete\" href=\"javascript:;\"><i class=\"del-i\"></i></a><a href=\"javascript:;\" class=\"theme-color restore none\"><i class=\"pre-i\"></i></a></td></tr></tbody></table>");}
                    $(".txtPrices").val($.trim($(".txtPrice").val()));//销售价格
                    
                    }else
                        {
                        layerCommon.msg(data, IconOption.错误);
                      $($this).attr("checked", false);
                        }
                    },error:function(){}
                })
            })
            //上下架
            $(document).on("click","input[name='isOffline']",function () {
                if ($(this).attr("checked") == undefined) {
                    $(this).attr("checked", true);
                } else {
                    $(this).attr("checked", false);
                }
                if ($(this).attr("checked")) {
                    $(this).val("1");
                    //$(this).next("input[name='hidIsOffline']").val("1");
                    $(this).parents(".trOp").find("input[name='hidIsOffline']").val("1")
                } else {
                    $(this).val("0");
                    //$(this).next("input[name='hidIsOffline']").val("0");
                    $(this).parents(".trOp").find("input[name='hidIsOffline']").val("0")
                }
            })
            //成本价带出销售价
            $(".txtPrice").blur(function(){
                var price=$(this).val();
                $(".txtPrices").val(price);
            })

            //成本价带出销售价
            $("#txtKC").blur(function(){
                var Inventory=$(this).val();
                $(".txtInventory").val(Inventory);
            })

            //商品标签的选定
            $(document).on("click", ".productLabelItem", function () {
                if ($(this).attr("class") == "t productLabelItem") {
                    $(this).addClass("cur");
                    $(this).find("input").attr("checked", "checked");
                } else {
                    $(this).removeClass("cur");
                    $(this).find("input").removeAttr("checked");
                }
                return false;
            })
            //规格模板change事件
            $("#ddlTemplate").change(function () {
                var id = $(this).val(); //模板id
                if (id != "") {
                    $.ajax({
                        type: "post",
                        url: "/Handler/GoodsEdit.ashx",
                        data: { ck: Math.random(), action: "onchange", id: id },
                        dataType: "text",
                        success: function (data) {
                            $(".mulSpecList").html(data.split('@@@')[0]); //模板信息
                            if ($.trim(data.split('@@@')[1]) == 3) {
                                $(".addSpecLi").attr("style","display:none;")
                                $(".addSpec").hide();
                                
                            } else {
                                $(".addSpecLi").attr("style","display:block;")
                                $(".addSpec").show();
                                
                            }
                            TableSku(); //sku信息
                        }
                    })
                }
            })
            //图片相册删除按钮层显示
            $(document).on("mouseenter", ".imgWrap", function () {
                $(this).parent().next("a").css("display", "block");
            })
            //图片删除
            $(document).on("click", ".delImg", function () {
              //  $(this).prev().remove();
              // $(this).next().remove();
                $(this).parent().remove();
                var url = $(this).attr("tip"); //图片名称
                var type=$(this).attr("newPic")=="1"?"0":"1";
                $.ajax({
                    type: "post",
                    url: "/Handler/GoodsEdit.ashx",
                    data: { ck: Math.random(), action: "delImg", filepath: url, type: type },
                    dataType: "text",
                    async: false,
                    success: function (data) {
                        if (data == "cg") {
                            $(this).prev().remove();
                            $(this).next().remove();
                            $(this).remove();
                            var count = $(".ImgList p").length;
                            if (count < 10) {
                                $(".AddImg").show();
                            } else {
                                $(".AddImg").hide();
                            }
                            var msg = $(".ImgList a:first").attr("tip");
                            $("#hrImgPath").val(msg);
                        }
                    }
                })
            })
            //规格名称变化时
            $(document).on("keyup", ".mulSpecName", function () {
                TableSku(); //sku信息
            })
            //删除sku
            $(document).on("click", ".delete", function () {
                var count = $(".productListBox tbody tr:not('.disabled')").length;
                if (count == 1) {
                      layerCommon.msg("最少保留一条商品数据", IconOption.错误);
                    return false;
                }
                $(this).parent().parent().addClass("disabled")//禁用
                $(this).parent().prevAll().children("label").find("input").attr("disabled", "disabled"); //禁用
                $(this).parent().prevAll().children("input").attr("disabled", "disabled"); //禁用
                $(this).parent().prevAll().children("input").css("cursor", "not-allowed"); //禁用鼠标样式
                $(this).addClass("none"); //删除按钮隐藏
                $(this).next(".restore").removeClass("none"); //恢复按钮显示
                $(".deleteIDlist").removeAttr("disabled"); //解除禁用
            })
            //恢复sku
            $(document).on("click", ".restore", function () {
                $(this).parent().parent().removeClass("disabled")//禁用
                $(this).parent().prevAll().children("label").find("input").removeAttr("disabled"); //禁用
                $(this).parent().prevAll().children("input").removeAttr("disabled"); //解除禁用
                $(this).parent().prevAll().children("input").css("cursor", "text"); //文本鼠标样式
                $(this).addClass("none");
                $(this).prev(".delete").removeClass("none");
            })
            //维护分类
              $(document).on("click", ".showCate", function () {
                var height = document.documentElement.clientHeight; //计算高度
                var layerOffsetY = (height - 340) / 2; //计算宽度
                //var index = showDialog('商品分类维护', 'GoodsCategory.aspx?type=1', '818px', '433px', layerOffsetY); //记录弹出对象
                 var index = layerCommon.openWindow('商品分类维护', 'GoodsCategory.aspx?type=1', '828px', '433px'); 
           
                $("#hid_Alert").val(index); //记录弹出对象
            })
            //是否我的桌面 显示
            $("#chkshow").click(function(){
            if ($(this).attr("checked") == undefined) {
                $(this).attr("checked", true);
                $("#isrecommend").removeClass("none");
                $("#isrecommend").removeAttr("style")
                } else {
                $(this).attr("checked", false);
                $("#isrecommend").addClass("none");
                
                }
            })
            //页面加载是判断是否显示零售价
            if ($("#chkisprice").attr("checked") == undefined) {
                $("#txtlspriceli").addClass("none")
            } else {
                $("#txtlsprice").removeClass("none")
                
            }
            //是否显示零售价
            $("#chkisprice").click(function(){
            if ($(this).attr("checked") == undefined) {
                $(this).attr("checked", true);
                $("#txtlsprice").removeClass("none")
                
                } else {
                $(this).attr("checked", false);
                $("#txtlsprice").addClass("none")
                
                }
            })
      
        });
        //图片上传uploadAvatar
        function uploadAvatar(ele) {
           var ua = navigator.userAgent.toLowerCase(); //浏览器信息
    var info = {
        ie: /msie/.test(ua) && !/opera/.test(ua),        //匹配IE浏览器    
        op: /opera/.test(ua),     //匹配Opera浏览器    
        sa: /version.*safari/.test(ua),     //匹配Safari浏览器    
        ch: /chrome/.test(ua),     //匹配Chrome浏览器    
        ff: /gecko/.test(ua) && !/webkit/.test(ua)     //匹配Firefox浏览器
    };
    if (!info.ie) {
      if (ele.files[0].size > 2 * 1024 * 1024) {
                layerCommon.msg("只能上传2M以下的图片", IconOption.错误);
                return false;
            }
    }
            $.ajaxFileUpload(
        {
            type: "post",
            url: "../../Handler/HandleImg2.ashx",            //需要链接到服务器地址
            secureuri: false,
            fileElementId: "upLoadImg",                        //文件选择框的id属性
            dataType: "text",
            //服务器返回的格式，可以是json
            success: function (msg, status)            //相当于java中try语句块的用法
            {
                if (msg == "0") {
                         layerCommon.msg("图片上传失败", IconOption.错误);
                    return false;
                } else if (msg == "1") {
                         layerCommon.msg("只能上传2M以下的图片", IconOption.错误);
                    return false;
                } else {
                    var temp = '';
                    for (var i = 0; i < 4; i++) {
                        temp += parseInt(Math.random() * 10);
                    }
                    var src = msg + "?temp=" + temp;
                    var count = $(".ImgList p").length;
                    count++;
                    // $("#imgAvatar").attr("src", '<%= Common.GetWebConfigKey("ImgViewPath") %>' + "GoodsImg/" + src);
                    $(".ImgList").append("<div><p  draggable=\"true\" style=\"margin:0 5px 5px 0; float: left; cursor: move;\" class=\"p" + count + "\"><img  src=\"" + '<%= Common.GetWebConfigKey("ImgViewPath") %>' + "GoodsImg/" + src + "\"  id=\"img" + count + "\" width=\"100\" height=\"100\" class=\"imgWrap\"  alt=\"图片\" /></p><a href=\"JavaScript:;\" class=\"delImg\" newPic='1' tip=\"" + msg + "\" style=\"color:red; cursor: pointer; float: left; margin: 105px 0 0 -70px;display:none;\">删除</a><input type=\"hidden\" name=\"hidImg\" value=\"" + msg + "\" id=\"hidImg" + count + "\" /></div>");
                    if (count == 1) {
                        $("#hrImgPath").val(msg);
                    } else if (count >= 10) {
                        $(".AddImg").hide();
                    } else {
                        $(".AddImg").show();
                    }
                 
                    return true;
                }
            },
          
            error: function (msg, status, e)            //相当于java中catch语句块的用法
            {
                 layerCommon.msg(msg + "," + status, IconOption.错误);
                return false;
            }
        })
        }
        //去掉odd样式
        function removeClass() {
            $(".productListBox tr").each(function (index, obj) {
                $(this).find("td:gt(0)").removeClass("odd");
            })
        }
         //禁用Enter键表单自动提交  
        document.onkeydown = function (event) {
            var target, code, tag;
            if (!event) {
                event = window.event; //针对ie浏览器  
                target = event.srcElement;
                code = event.keyCode;
                if (code == 13) {
                    tag = target.tagName;
                    if (tag == "TEXTAREA") { return true; }
                    else { return false; }
                }
            }
            else {
                target = event.target; //针对遵循w3c标准的浏览器，如Firefox  
                code = event.keyCode;
                if (code == 13) {
                    tag = target.tagName;
                    if (tag == "INPUT") { return false; }
                    else { return true; }
                }
            }
        }

        //根据规格值生成sku信息
        function TableSku() {
            var count = $(".mulSpecList .mulSpecItem").length; //规格的个数
            var attr = new Array();
            var attr1 = new Array();
            var attr2 = new Array();
            var guigeName = "";
            for (var i = 0; i < count; i++) {
                $(".mulSpecList .mulSpecItem:eq(" + i + ")").find(".selectize-input .item").each(function (index, obj) {
                    if (i == 2) {
                        attr2[index] = $(this).attr("data-value");
                    }
                    if (i == 1) {
                        attr1[index] = $(this).attr("data-value");
                    }
                    if (i == 0) {
                        attr[index] = $(this).attr("data-value");
                    }
                })
               if($(".mulSpecList .mulSpecItem:eq(" + i + ")").find(".selectize-input .item").length!=0){
                guigeName += $(".mulSpecList .mulSpecItem:eq(" + i + ")").find(".mulSpecName").val() + ",";
                }
            }
            if (guigeName != "") {
                guigeName = guigeName.substring(0, guigeName.length - 1);
            }
            $.ajax({
                type: "post",
                url: "GoodsEdit.aspx",
                data: { ck: Math.random(), action: "Sku", list: attr, list1: attr1, list2: attr2, guigeName: guigeName ,goodsid:<%=KeyID %>},
                dataType: "text",
                success: function (data) {
                    if (data != "") {
                        $(".productListBox").html(data);
                           for (var i = 0; i < $(".txtPrices").length; i++) {
                            if ($.trim($(".txtPrices").eq(i).val()) == "") {
                                 // layerCommon.msg("销售价格不能为空", IconOption.错误);
                               $(".txtPrices").eq(i).val($.trim($(".txtPrice").val()));//销售价格
                            }
                          }
                    }else
                    {
                        var style="";
                        if('1'=='1'){
                            style="style='display:none'";
                        }
                        $('.productListBox').html("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"width:1050px;margin:0 auto;\"><thead><tr><th class=\"key\"></th><th>商品编码</th><th>销售价格(元)</th><th "+style+" class=\"kc\"><i class=\"red\">*</i>库存</th><th "+style+" class=\"t5\">批次号</th><th "+style+" class=\"t5\">有效期</th><th>是否上架</th><th>操作</th></tr></thead><tbody><tr><td class=\"key\">1</td><td class=\"trOp\"><input name=\"txtCode\" style=\"width:120px;\" type=\"text\" id=\"Text2\" class=\"dataBox txtCode\" value='<%=GoodsCode() %>' maxlength=\"15\"/></td><td class=\"trOp\"><input name=\"txtPrices\" style=\"width:100px;\" type=\"text\" id=\"Text1\" class=\"dataBox txtPrices\" onkeyup=\"KeyInt2(this);\" maxlength=\"10\"/></td><td class=\"trOp\" "+style+"><input name=\"txtInventory\" style=\"width:100px;\" type=\"text\" id=\"Text1\" class=\"dataBox txtInventory\" onkeyup=\"KeyInt2(this);\" maxlength=\"11\"/></td><td class=\"trOp\" "+style+"><input name=\"txtBatchNO\" type=\"text\"  style=\"width: 100px;\" class=\"dataBox txtBatchNO\" maxlength=\"11\" /></td><td class=\"trOp\" "+style+"><input name=\"txtvalidDate\" onclick=\"WdatePicker()\" readonly=\"readonly\" type=\"text\" style=\"width: 100px;\" class=\"dataBox txtvalidDate\" maxlength=\"11\" /></td><td class=\"trOp\"><div class=\"tc\"><input type=\"checkbox\"  name=\"isOffline\" value=\"1\" checked=\"checked\" id=\"check-2-1\" class=\"r-check\" /><label for=\"check-2-1\"></label></div> <input type=\"hidden\" value=\"1\" name=\"hidIsOffline\" /> <input value=\"0\" name=\"hidId\" class=\"deleteIDlist\" type=\"hidden\"/></td><td class=\"trOp\"><a class=\"theme-color delete\" href=\"javascript:;\"><i class=\"del-i\"></i></a><a href=\"javascript:;\" class=\"theme-color restore none\"><i class=\"pre-i\"></i></a></td></tr></tbody></table>");
                       
                           
                    }
                }
            })
        }
        //验证
        function formCheck() {
            var goodsName = $.trim($(".txtGoodsName").val()); //商品名称
            var goodsCate = $.trim($(".hid_txt_product_class").val()); //商品分类
            var unit = $.trim($("#txtunit").val()); //计量单位
            var price = $.trim($(".txtPrice").val()); //商品价格
            var inventory=$.trim($(".txtInventory").val());//库存
            var yanstr = "";
            if (goodsName == "") {
                  layerCommon.msg("请填写商品名称", IconOption.错误);
                return false;
            }
            if ($.trim($("#editor1").val()) != "") {
                var html = filterXSS($.trim($("#editor1").val()));
                $("#editor1").val(html);
            }

//            if ($("#HidFfileName2").val() == "" && $("#HidFfileName2").val() == undefined) {
//                layerCommon.msg("请上传商品注册证！", IconOption.哭脸);
//                return false;
//            }
//            if ($("#validDate2").val() == "" || $("#validDate2").val() == undefined) {
//                layerCommon.msg("请选择商品注册证有效期！", IconOption.哭脸);
//                return false;
//            }

//            } else {
//                $.ajax({
//                    type: "post",
//                    url: "/Handler/GoodsEdit.ashx",
//                    data: { ck: Math.random(), action: "yanz", goodsname: goodsName,goodsId:<%=KeyID %> },
//                    dataType: "text",
//                    async: false,
//                    success: function (data) {
//                        if (data == "1") {
//                            yanstr = "ycz";
//                            return false;
//                        }else if(data=="2"){
//                          yanstr = "wdl";
//                            return false;
//                        }
//                    }
//                })
//            }
//            if (yanstr == "ycz") {
//                         layerCommon.msg("商品名称已存在", IconOption.错误);
//                return false;
//            }
//             if (yanstr == "wdl") {
//                         layerCommon.msg("请先登录", IconOption.错误);
//                return false;
//            }
            if (goodsCate == "") {
                         layerCommon.msg("请选择商品分类", IconOption.错误);
                return false;
            }
            if (unit == "") {
                         layerCommon.msg("请选择计量单位", IconOption.错误);
                return false;
            }
//            if (price == "") {
//                         layerCommon.msg("请填写商品价格", IconOption.错误);
//                return false;
//            }else
//            {
            if (isNaN(price)) {
                         layerCommon.msg("请填写正确的商品价格", IconOption.错误);
                return false;
            }
//            }
//            if($("#chkisprice").attr("checked")=="checked"){
//                if($.trim($(".txtisPrice").val())==""){
//                   layerCommon.msg("请填写零售价格", IconOption.错误);
//                    return false;
//                }else
//                {
//                 if (isNaN($.trim($(".txtisPrice").val()))) {
//                         layerCommon.msg("请填写正确的零售价格", IconOption.错误);
//                return false;
//            }
//                }
//            }

            //验证规格名称是否重复
            var chkProduct = $("#chkProduct").attr("checked");
            if (chkProduct) {
                var ary = new Array();
                $(".mulSpecList .mulSpecItem").each(function (index, obj) {
                    ary[index] = $(".mulSpecList .mulSpecItem").eq(index).find(".mulSpecName").val();
                })
                var s = ary.join(",") + ",";
                var str = false;
                for (var i = 0; i < ary.length; i++) {
                    if ($.trim(ary[i]) == "") {
                            layerCommon.msg("规格名称不能为空", IconOption.错误);
                        str = true;
                        break;
                    }
//                    if (s.replace(ary[i] + ",", "").indexOf(ary[i] + ",") > -1) {
//                            layerCommon.msg("规格名称不能重复", IconOption.错误);
//                        str = true;
//                        break;
//                    }
                }
              
                if (str) {
                    return false;
                }else
                {
                  if(ary.length==2){
                if($.trim(ary[0])==$.trim(ary[1])){
                 layerCommon.msg("规格名称不能重复", IconOption.错误);
                      return false;
                }
                }else if(ary.length==3){
                 if($.trim(ary[0])==$.trim(ary[1]) || $.trim(ary[0])==$.trim(ary[2]) || $.trim(ary[2])==$.trim(ary[1])){
                 layerCommon.msg("规格名称不能重复", IconOption.错误);
                     return false;
                }
                }
                }
            }
            var speccount = $(".mulSpecList .mulSpecItem").length; //规格个数
            var specbool = false;
            for (var i = 0; i < speccount; i++) {
                var itemcount = $(".mulSpecList .mulSpecItem:eq(" + i + ")").find(".selectize-input>.item").length;
                if (itemcount != 0) {
                    var specname = $(".mulSpecList .mulSpecItem:eq(" + i + ")").find(".mulSpecProperty>.mulSpecName").val();
                    if ($.trim(specname) == "") {
                           layerCommon.msg("已输入规格值的,规格名称不能为空", IconOption.错误);
                        specbool = true;
                        break;
                    }
                }
            }
            if (specbool) {
                return false;
            }
             //商品编码
//        var str="";
//        var strcode="";
//        var codelength=$(".txtCode").length;
//        for (var i = 0; i < codelength; i++) {
//            strcode=$(".txtCode").eq(i).val()+",";
//            for (var z = 0; z < codelength; z++) {
//             if(i!=z){
//                 if($(".txtCode").eq(i).val()==$(".txtCode").eq(z).val()){
//                            str=$(".txtCode").eq(i).val();
//                         break;
//                    }
//               }
//            }
//        }
//        if(str!=""){
//         layerCommon.msg("商品编号"+str+"重复", IconOption.错误);
//          return false;
//        }else
//        {
//            $.ajax({
//            type:"post",
//            url:"/Handler/GoodsEdit.ashx",
//            data:{ck:Math.random(),action:"isChkCode",strcode:strcode,keyId:<%=KeyID %>},
//            dataType:"text",
//            async: false,
//            success:function(data){
//                if(data!=""){
//                    str=data;
//                       return false;
//                }
//            }
//            })
//        }
//        if(str!=""){
//          layerCommon.msg("商品编号"+str+"已存在", IconOption.错误);
//          return false;
//        }

           // var prices = $.trim($(".txtPrices").val()); //销售价格
//           for (var i = 0; i < $(".txtPrices").length; i++) {
//            if ($.trim($(".txtPrices").eq(i).val()) == ""  && $(".txtPrices").eq(i).attr("disabled")!="disabled") {
//                  layerCommon.msg("销售价格不能为空", IconOption.错误);
//                return false;
//            }
//            else
//            {
//                if(isNaN($.trim($(".txtPrices").eq(i).val()))){
//                      layerCommon.msg("请填写正确的销售价格", IconOption.错误);
//                    return false;
//                }
//            }
//          }
           if($(".kc").attr("style")!="display:none"){
               
                for (var i = 0; i < $(".txtInventory").length; i++) {
                     if ($.trim($(".txtInventory").eq(i).val()) == ""  && $(".txtInventory").eq(i).attr("disabled")!="disabled") {
                       layerCommon.msg("商品库存不能为空", IconOption.错误);
                        return false;
                    }
                }
           }


           var des = [];
           $("#deleteIDlist").val("");
           $(".disabled").each(function (i, obj) {
   
               var id = $(obj).find(".deleteIDlist").val();
               des.push(id);
           });
           var checkid = des.join(",");
           $("#deleteIDlist").val(checkid)

           var des2=[];
           $("#GoodsUL tbody tr[class!='disabled']").each(function (i, obj) {
               var id = $(obj).find(".deleteIDlist").val();
               des2.push(id);
           });
           var checkid2 = des2.join(",");
           $("#OdlIDList").val(checkid2)


            return true;
        }
              //显示效果图
        function showPic(pic) {
            var height = document.documentElement.clientHeight; //计算高度
            var layerOffsetY = (height - 340) / 2; //计算宽度
            var wd = "625px";
            var hi = "435px";
            var index = layerCommon.openWindow('效果图展示', "ShowPic.aspx?pic=../images/" + pic, wd, hi);  //记录弹出对象
        }
        //注册资格证  重复上传判断
        function uploadFile2Click() {
            if ($("#HidFfileName2").val() != "" && $("#HidFfileName2").val() != undefined)
            {
                layerCommon.msg("请勿重复上传！", IconOption.哭脸);
                return false;
            }
            else
                return true;
        }
        
        function AnnexDelete2() {
            $("#UpFileText2").html("");
            $("#HidFfileName2").val("");
        }
    </script>
    <script type="text/javascript">
        var sx = 0; //拖动的div index
        $(document).ready(function () {
            // 正在拖动的图片的父级DIV 
            var $srcImgDiv = null;
            // 开始拖动 
            $(document).on("dragstart", ".ImgList p", function (event) {
                sx = $(this).parent().index(); //拖动的div index
                $srcImgDiv = $(this).parent();
            });
            // 拖动到.ImgList img上方时触发的事件 
            $(document).on("dragover", ".ImgList p", function (event) {
                // 必须通过event.preventDefault()来设置允许拖放 
                event.preventDefault();
                event.stopPropagation();
            });
            // 结束拖动放开鼠标的事件 
            $(document).on("drop", ".ImgList p", function (event) {
                event.preventDefault(); // 必须通过event.preventDefault()来设置允许拖放 
                event.stopPropagation();
                if ($srcImgDiv[0] != $(this).parent()[0]) {
                    if (sx > $(this).parent().index()) {//右移动
                        $(this).parent().before($srcImgDiv);
                    } else if (sx < $(this).parent().index()) {//左移动
                        $(this).parent().after($srcImgDiv);
                    }
                    return false;
                }
            });
            $(document).on("click",".Rpt_Del",function(){
                var $This = $(this);
                $.ajax({
                    type: 'POST',
                    data: { action: "Del_Doc", key: $This.attr("data-key") },
                    dataType: "json",
                    timeout: 5000,
                    cache: false,
                    success: function (ReturnData) {
                        if (ReturnData.result) {
                            $(".txtunit#txtunit").val() == $This.attr("data-text") && $(".txtunit#txtunit").val("");
                            $(".select p").each(function (index,item ) {
                                if($(item).html().trim()==$This.attr("data-text").trim()) 
                                {
                                    $(".select p").eq(index).remove()
                                }
                            })
                            $This.closest("tr").remove();
                        } else {
                            layerCommon.msg(ReturnData.msg, IconOption.错误);
                        }
                    }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                        layerCommon.msg("删除失败", IconOption.错误);
                    }
                });
            });
        })

    </script>

    <style>
        a.theme-color {
            color: #2e70d3;
        }

            a.theme-color:hover {
                color: red;
            }

        .mulSpecItem {
            margin: 0;
            padding: 0px 0px 10px 0px;
        }

        .mulSpecProperty {
            float: left;
            width: 120px;
        }

        .delMulSpec {
            background: url("../images/icon8.png") no-repeat scroll -80px -480px;
            display: block;
            float: right;
            height: 16px;
            margin-right: 200px;
            margin-top: 12px;
            width: 16px;
        }

        .mulSpecValues {
            width: 415px;
            float: left;
        }

        .cb {
            clear: both !important;
        }

        .selectize-control {
            max-height: 60px;
            position: relative;
            border: 1px solid #ddd;
            overflow-y: auto;
            float: left;
            border-radius: 5px;
            overflow-x: hidden;
            padding-top: 5px;
            padding-right: 10px;
        }

            .selectize-control .box1 {
                border: none;
            }

            .selectize-control.single .selectize-input.input-active, .selectize-input {
                background: #fff none repeat scroll 0 0;
                cursor: text;
                display: inline-block;
            }

                .selectize-dropdown, .selectize-input, .selectize-input input {
                    color: #303030;
                    font-family: inherit;
                    font-size: 13px;
                    <%-- line-height: 18px;
                    --%>;
                }

        .selectize-dropdown {
            -moz-border-bottom-colors: none;
            -moz-border-left-colors: none;
            -moz-border-right-colors: none;
            -moz-border-top-colors: none;
            background: #fff none repeat scroll 0 0;
            border-color: -moz-use-text-color #d0d0d0 #d0d0d0;
            border-image: none;
            border-style: none solid solid;
            border-width: 0 1px 1px;
            box-sizing: border-box;
            margin: -1px 0 0;
            position: absolute;
            z-index: 10;
        }

        .selectize-input > input {
            background: rgba(0, 0, 0, 0) none repeat scroll 0 0 !important;
            border: 0 none !important;
            display: inline-block !important;
            line-height: inherit !important;
            margin: 0 2px 0 0 !important;
            max-height: none !important;
            max-width: 100% !important;
            min-height: 0 !important;
            padding: 0 !important;
            text-indent: 0 !important;
        }

        .selectize-input > * {
            display: inline;
            vertical-align: baseline;
        }

        .selectize-dropdown-content {
            max-height: 200px;
            overflow-x: hidden;
        }

        .ml140 {
            margin-left: 10px !important;
        }

        .mt10 {
            margin-top: 10px !important;
        }

        .lite-gray, a.lite-gray:hover {
            color: #999 !important;
        }

        .delMulSpec:hover {
            background: url("../images/icon8.png") no-repeat scroll -120px -480px;
            cursor: pointer;
        }

        .selectize-input::after {
            clear: left;
            content: " ";
            display: block;
        }

        .selectize-input {
            box-sizing: border-box;
            display: inline-block;
            padding: 11px 8px;
            position: relative;
            width: 100%;
            z-index: 1;
        }

            .selectize-input.input-active, .selectize-input {
                background: #fff none repeat scroll 0 0;
                cursor: text;
                display: inline-block;
            }

                .selectize-input > input:focus {
                    outline: 0 none !important;
                }

        .selectize-dropdown .create {
            color: rgba(48, 48, 48, 0.5);
            border-top: 1px solid #999;
            padding: 5px 5px 8px 8px;
        }

        .selectize-dropdown {
            cursor: pointer;
            overflow: hidden;
            margin-top: 40px;
            margin-left: 20px;
        }

        .selectize-control.multi .selectize-input > div {
            background: #003c9d none repeat scroll 0 0;
            border: 0 solid #d0d0d0;
            color: #fff;
            cursor: pointer;
            margin: 0 8px 3px 0;
            padding: 5px;
        }

        .remove {
            /*background: url("../images/icon8.png") no-repeat scroll 0 -480px;
            border-left: 0 none;
            bottom: 0;
            color: inherit;
            display: inline;
            font-size: 12px;
            font-weight: 700;
            height: 0;
            overflow: hidden;
            padding: 18px 0 0;
            position: absolute;
            right: -9px;
            text-align: center;
            text-decoration: none;
            top: -6px;
            vertical-align: middle;
            width: 18px;
            z-index: 1;
            box-sizing: border-box;*/
        }

        .selectize-input.dropdown-active::before {
            background: #f0f0f0 none repeat scroll 0 0;
            bottom: 0;
            content: " ";
            display: block;
            height: 1px;
            left: 0;
            position: absolute;
            right: 0;
        }

        .selectize-control.plugin-remove_button [data-value] {
            padding-right: 24px !important;
            position: relative;
        }
        /*    .selectize-control.multi .selectize-input.has-items {
            padding: 6px 8px 3px;
        }*/

        /*计量单位*/
        .pullDown2 {
            margin: -5px 0px 0px 5px;
            border: 1px solid #e5e5e5;
            width: 172px;
            background: #fff;
            position: relative;
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
        /*商品标签*/
        .div_content .tb td label {
            display: inline-block;
            line-height: 25px;
            margin-left: 0px;
            min-width: 0px;
            padding: 0px;
        }

        .control-input label {
            background-color: #fafafa;
            border: 1px solid #d6dee3;
            cursor: pointer;
            display: block;
            float: left;
            height: 26px;
            margin-right: 10px;
            text-align: center;
            width: 80px;
        }

            .control-input label > input {
                display: none;
            }

        input[type="checkbox"] {
            margin: 3px 3px 3px 4px;
            vertical-align: middle;
        }

        .control-input {
            float: none;
            margin-left: 5px;
            overflow: hidden;
            padding-left: 0;
            line-height: 29px;
        }

        label.checked {
            background-color: #7697cb;
            border: 1px solid #7697cb;
            color: #fff;
        }

        /*商品SKU*/
        .productListBox {
            margin: 5px;
        }
        /*        .table-wrap-lite {
            padding-top: 10px;
        }*/
        .table-wrap-lite table {
            border-collapse: collapse;
            border-spacing: 0;
        }

        .table-wrap-lite th {
            background-color: #f5f6f7;
            border: 1px solid #e5e8ea;
            color: #555;
            font-weight: 400;
            height: 50px;
            padding: 0 5px;
            table-layout: fixed;
            vertical-align: middle;
        }

        .table-wrap-lite tbody tr {
            display: table-row;
        }

        .productListBox .key {
            table-layout: fixed;
            text-align: center;
            width: 15px;
        }

        .table-wrap-lite table td.key {
            background-color: #f5f6f7;
            table-layout: fixed;
            text-align: center;
            width: 20px;
        }
        /*.tc {
            text-align: center !important;
        }*/
        .productListBox .trOp {
            table-layout: fixed;
            text-align: center;
        }

        .showquy {
            color: #aaaaaa;
            display: block;
        }

        #ImgList p:hover {
            border-color: #9a9fa4;
            box-shadow: 0 0 6px 0 rgba(0, 0, 0, 0.85);
        }

        #More {
            cursor: pointer;
        }





        /*操作计量单位用到的样式*/
        .Layer {
            background-color: #000;
            opacity: 0.3;
            filter: alpha(opacity=30);
            z-index: 1000;
            width: 100%;
            height: 100%;
            position: fixed;
            left: 0;
            top: 0;
            display: none;
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

        .orangeBtn {
            background: #537fc4;
            font-weight: normal;
            border: 1px solid #537fc4;
        }

            .orangeBtn:hover {
                background: #3b6dbb;
                border: 1px solid #3b6dbb;
            }

        .orangeBtn {
            padding: 0px 20px;
            height: 28px;
            color: #fff;
            background: #537fc4;
            border: 1px solid #537fc4;
            font-size: 14px;
            border-radius: 3px;
            cursor: pointer;
        }

        .cancel:hover {
            background: #e0e0e0;
        }

        .cancel {
            background: #efefef;
            color: #000;
            font-weight: normal;
            border: 1px solid #e0e0e0;
        }

        .cancel {
            padding: 0px 20px;
            height: 28px;
            color: #000;
            background: #efefef;
            border: 1px solid #efefef;
            font-size: 14px;
            border-radius: 3px;
            cursor: pointer;
        }

        .fl {
            margin: 0px;
        }
        .tabLine td {
            padding: 0 2px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <uc1:Top ID="top1" runat="server" ShowID="nav-3" />
        <uc1:CompRemove runat="server" ID="Remove" ModuleID="3" />
        <div class="rightinfo">
            <!--当前位置 start-->
            <div class="info">
                <a href="../jsc.aspx">我的桌面</a>><a href="../GoodsNew/GoodsList.aspx" runat="server" id="atitle">商品列表</a>>
    <a href="javascript:;" runat="server" id="btitle">
        <%if (KeyID == 0)
            { %>商品新增<%}
                                  else
                                  {%>商品编辑<%}%></a>
            </div>
            <!--当前位置 end-->
            <!--商品信息 start-->
            <div class="c-n-title">商品信息</div>
            <ul class="coreInfo">
                <li class="lb fl"><i class="name"><i class="red">*</i>商品名称</i>
                    <input type="text" class="box1 txtGoodsName" id="txtGoodsName" runat="server" name="txtGoodsName" maxlength="100" />

                </li>
                <li class="lb fl">
                    <i class="name fl"><i class="red">*</i>计量单位</i>
                    <div class="pullDown fl w380" style="width: 380px;">
                        <ul>
                            <li>
                                <input type="hidden" runat="server" id="txtunittex" />
                                <input type="button" class="box1 txtunit" onclick="beginSelect(this);" style="width: 382px; margin-left: 4px;" name="txtunit" id="txtunit"
                                    readonly="readonly" runat="server" maxlength="15" /><span class="arrow"></span></li>
                            <li class="select">
                                <asp:Repeater ID="rptUnit" runat="server">
                                    <ItemTemplate>
                                        <p class=""><%# Eval("AtVal")%></p>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <p class="">0</p>
                                        <span class="add-more">新增单位<a href="javascript:;" class="addBtn"></a> </span>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </li>
                        </ul>

                    </div>
                    <div class="fl" style="width: 24px; height: 30px; line-height: 30px">
                        <%--<a href="javascript:;" class="addBtn" style="color:#2e70d3;"><i class="addIcon"></i>维护</a> --%>
                    </div>
                </li>

              



                <li class="lb fl">
                    <i class="name"><i class="red">*</i>商品分类</i>
                    <input type="hidden" id="hid_txt_product_class" class="hid_txt_product_class" runat="server" />
                    <input type="hidden" id="hid_product_CateGoryName" class="hid_product_CateGoryName" runat="server" />
                    <input type="text" id="txt_product_class" autocomplete="off" width="360" class="box1 txt_product_class" runat="server" maxlength="50" readonly="readonly" /><span class="arrow"></span>
                    <%--<a class="showCate" style="color:#2e70d3;position:absolute; margin-top:-25px; margin-left:490px;width:40px;" href="javascript:;">维护</a>--%>
                    <div class="pop-menu" style="width: 605px; display: none;">
                    </div>
                </li>


                <li class="lb fl">
                    <i class="name">关键词/卖点</i>
                    <input name="txtTitle" type="text" width="360" maxlength="100" class="box1 txtTitle" id="txtTitle" runat="server" placeholder="最多可输入100个字符" />
                </li>

                <li class="lb fl none gd">
                    <i class="name">隐私信息1</i>
                    <input name="txtHideInfo1" type="text" maxlength="60" class="box1 txtHideInfo1" width="360"
                        id="txtHideInfo1" runat="server" placeholder="只能代理商可见信息,最多可输入60个字符" />
                </li>
                <li class="lb fl none gd">
                    <i class="name">隐私信息2</i>
                    <input name="txtHideInfo2" type="text" maxlength="60" class="box1 txtHideInfo2" width="360"
                        id="txtHideInfo2" runat="server" placeholder="只能代理商可见信息,最多可输入60个字符" />
                </li>
                <li class="lb fl none gd">
                    <i class="name">排序值</i>
                    <input name="txtIndex" type="text" maxlength="7" class="box1 txtIndex" id="txtIndex" width="360"
                        runat="server" placeholder="数值越大,排名越靠前" onkeyup="KeyInt(this);" />
                </li>
                <li class="lb fl none gd" id="tb">
                    <i class="name fl">商品标签</i>
                    <div class="control-input label fl" id="DivLabel" runat="server">
                    </div>
                </li>
                <li class="lb fl none gd">
                    <i class="name">商城店铺显示</i>
                    <div class="single">
                        <input type="radio" id="chkshow" class="regular-check" name="isshow" value="1" runat="server" checked="true" />
                        <label for="chkshow"></label>
                        <i class="t">店铺显示</i>
                    </div>
                </li>
                <li class="lb fl">
                    <div id="isrecommend" runat="server">
                        <i class="name">商品推荐</i>
                        <div class="single">
                            <input type="radio" id="chkRecommend" class="regular-check" name="isRecommended" value="2" runat="server" checked="true" />
                            <label for="chkRecommend"></label>
                            <i class="t">推荐</i>
                            <input type="radio" id="chkRecommendno" class="regular-check" name="isRecommended" value="1" runat="server" /><label for="chkRecommendno"></label><i class="t">不推荐</i>
                        </div>
                    </div>
                </li>



                <li class="lb fl none gd">
                    <i class="name">显示店铺零售价</i>
                    <div class="single">
                        <input type="radio" name="isprice" value="1" id="chkisprice" runat="server" class="regular-check" />
                        <label for="chkisprice"></label>
                        <i class="t">显示零售价</i>
                    </div>
                </li>
                <li class="lb fl gd none" id="txtlspriceli">
                    <div id="txtlsprice" runat="server" class="none">
                        <i class="name"><i class="red">*</i>零售价：</i>
                        <input name="txtisPrice" type="text" onkeyup="KeyInt2(this);" maxlength="18" class="box1 txtisPrice" id="txtisPrice" runat="server" placeholder="填写零售价" />
                    </div>
                </li>






                <li class="lb fl"><i class="name"></i><i class="more" id="More">更多功能</i></li>
            </ul>
            <!--商品信息 end-->

            <div class=" clear"></div>
           <div class="c-n-title">商品注册证</div>
            <ul class="coreInfo">

                
                  <li class="lb fl">
                    <i class="name fl"><%--<i class="red">*</i>--%>商品注册证</i>
                    <div class="con upload">
                        <div style="float: left">
                            <a href="javascript:;" class="a-upload bclor le">
                                <input id="uploadFile2" runat="server" type="file"
                                    onclick="return uploadFile2Click()"
                                    name="fileAttachment2" class="AddBanner" />上传附件</a>
                        </div>
                        <div style="float: left">
                            <div id="UpFileText2" style="margin-left: 10px; width: 300px;" runat="server">
                            </div>
                        </div>
                    </div>
                    <asp:Panel runat="server" ID="DFile2" Style="margin: 5px 5px;">
                    </asp:Panel>
                    <input runat="server" id="HidFfileName2" type="hidden" />
                </li>

                <li class="lb fl">
                    <i class="name"><%--<i class="red">*</i>--%>注册证有效期</i>
                    <input name="validDate2" runat="server" onclick="WdatePicker()" style="margin-left:5px;"
                        id="validDate2" readonly="readonly" type="text" class="box1" value="" />

                </li>

            </ul>






            <div class=" clear"></div>
            <!--商品规格 start-->
            <div class="c-n-title">
                <span class="fl">商品规格</span>
                <div class="set fl">
                    <input type="checkbox" class="r-check" name="isProductMultispecOn" value="1" id="chkProduct" runat="server" />
                    <label for="chkProduct"></label>设置多规格商品
                </div>
            </div>
            <div style="margin-top: 5px;" id="divSku" runat="server">
                <ul class="coreInfo">
                    <li class="lb">
                        <div style="display: inline; margin-left: 100px;" id="divProduct" runat="server">
                            <i class="name fl">规格模板</i>
                            <div class="pullDown fl w380">
                                <asp:DropDownList ID="ddlTemplate" runat="server" CssClass="box1 p-box"></asp:DropDownList>
                            </div>
                        </div>
                    </li>
                    <li class="lb g-spec">
                        <div class="mulSpecList">

                            <div class="mulSpecItem">
                                <div class="mulSpecProperty" style="width: 150px; height: auto; min-height: 35px">
                                    <input type="text" value="" maxlength="4" placeholder="规格名称(4字内) " class="ui-input-dashed mulSpecName box2" name="mulSpecName" style="height: auto; min-height: 35px" />
                                </div>
                                <a class="delMulSpec" style="display: none"></a>
                                <div class="mulSpecValues" style="width: 680px;">
                                    <input type="text" style="display: none;" class="mulSpecInp selectized" name="selectized" tabindex="-1" value="" maxlength="15" />
                                    <div class="selectize-control mulSpecInp multi plugin-remove_button">
                                        <div class="selectize-input items not-full box1 fl" style="width: 720px; height: auto; min-height: 30px">
                                            <input type="text" autocomplete="off" tabindex="" style="width: 4px;" maxlength="15" /><i class="del-i del-i-a"></i>
                                        </div>
                                        <div class="selectize-dropdown multi mulSpecInp plugin-remove_button" style="display: none;">
                                            <div class="selectize-dropdown-content">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="cb"></div>
                            </div>


                        </div>
                    </li>


                    <li class="lb g-spec addSpecLi">
                        <a class="theme-color addSpec" id="addSpec" href="javascript:;" style="display: block; width: 80px; color: #555; text-decoration: none;"
                            runat="server"><em style="display: inline-block; font-size: 14px; height: 12px; line-height: 32px; text-decoration: none; vertical-align: middle; width: 12px; background-image: url('../images/icon8.png'); background-position: -32px -1px;"></em>添加规格</a></li>
                </ul>
            </div>
            <div class="blank10"></div>
            <div class="tabLine" id="GoodsUL">
                <ul class="coreInfo">
                    <li class="lb fl">
                        <i class="name fl">商品统一价格</i>
                        <input name="txtPrice" type="text" id="txtPrice" runat="server" class="box1  txtPrice" onkeyup="KeyInt2(this);" maxlength="10" />
                    </li>
                    <li class="lb fl" style="display:none">
                        <i class="name fl">商品统一库存</i>
                        <input name="txtKC" type="text" id="txtKC" runat="server" class="box1" onkeyup="KeyInt2(this);" maxlength="10" />
                    </li>
                </ul>
                <p class="gclor9">
                    * 该价格是建议零售价，代理商的价格请在<a href="/Company/Contract/ContractList.aspx"><b>合同</b></a>中维护
                </p>
                <div class="productListBox table-wrap-lite" style="float: left; margin-bottom: 5px;">
                    <table border="0" cellspacing="0" cellpadding="0" style="width: 1050px; margin: 0 auto;">
                        <thead>
                            <tr>
                                <th class="key"></th>
                                <th class="tle2 t3">商品编码
                                </th>
                                <th class="t3">销售价格(元)
                                </th>
                                <th class="t3 kc" style="display:none">
                                    <i class="red">*</i> 库存
                                </th>
                                <%--<th class="t5">
                                    批次号
                                </th>
                                <th class="t5">
                                    有效期
                                </th>--%>
                                <th class="t5">是否上架
                                </th>
                                <th class="t5">操作</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td class="key">1
                                </td>
                                <td class="trOp">
                                    <input name="txtCode" style="width: 120px;" type="text" id="txtCode" runat="server"
                                        class="dataBox txtCode" maxlength="15" />
                                </td>
                                <td class="trOp">
                                    <input name="txtPrices" type="text" id="Text1" style="width: 100px;" class="dataBox txtPrices"
                                        onkeyup="KeyInt2(this);" maxlength="10" />
                                </td>
                                <td class="trOp" style="display:none">
                                    <input name="txtInventory" type="text" id="Text2" style="width: 100px;" class="dataBox txtInventory"
                                        onkeyup="KeyInt3(this);" maxlength="11" />
                                </td>
                                <td class="trOp" style="display:none">
                                    <input name="txtBatchNO" type="text"  style="width: 100px;" class="dataBox txtBatchNO"
                                        maxlength="11" />
                                </td>
                                <td class="trOp" style="display:none">
                                    <input name="txtvalidDate" onclick="WdatePicker()" readonly="readonly" type="text" style="width: 100px;" class="dataBox txtvalidDate" maxlength="11" />
                                </td>
                                <td class="trOp">
                                    <div class="tc">
                                        <input type="checkbox" name="isOffline" value="1" checked="checked" id="check-2-1" class="r-check" /><label for="check-2-1"></label></div>
                                    <input type="hidden" value="1" name="hidIsOffline" />
                                    <input value="0" name="hidId" class="deleteIDlist" type="hidden" />
                                </td>
                                <td class="trOp">
                                    <a class="theme-color delete" href="javascript:;"><i class="del-i"></i></a>
                                    <a href="javascript:;" class="theme-color restore none"><i class="pre-i"></i></a>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div style="float: left; margin-top: 5px;">
                </div>
            </div>
            <!--商品规格 end-->


            <div class="blank10"></div>
            <!--商品图册 start-->
            <div class="c-n-title"><i class="fl">商品图册</i><div class="set fl gclor9">（尺寸800*800最佳，最多添加10张）</div>
            </div>

            <div style="margin: 6px 0px 0px 6px;">
                <div class="uploadBtnBox" style="cursor: hand;">
                    <div class="ImgList" id="ImgList" runat="server" style="cursor: pointer;">
                    </div>
                    <div class="AddImg" id="AddImg" runat="server" style="cursor: hand;">
                        <p style="float: left; cursor: pointer;">
                            <img src="../images/150x150.gif" id="imgAvatar" width="100" height="100" runat="server"
                                class="imgWrap" alt="图片" style="cursor: hand;" />
                        </p>
                        <input type="file" id="upLoadImg" name="upLoadImg" style="width: 120px; height: 100px; font-size: 100px; cursor: hand; float: left; margin: 0px 5px 0px -150px; opacity: 0; filter: alpha(opacity=0);"
                            onchange="uploadAvatar(this);" />
                    </div>
                </div>
                <asp:HiddenField ID="hrImgPath" runat="server" />
            </div>

            <!--商品图册 end-->
            <div class="blank10"></div>
            <!--商品描述 start-->
            <div class="c-n-title"><i class="fl">商品描述</i></div>
            <asp:TextBox ID="editor1" runat="server" TextMode="MultiLine" Height="470px" Width="1050px"
                class="textBox"></asp:TextBox>
            <script>
                                KindEditor.ready(function (K) {
                                    window.editor = K.create('#editor1', {
                                        uploadJson: '../../Kindeditor/asp.net/upload_json.ashx',
                                        fileManagerJson: '../../Kindeditor/asp.net/file_manager_json.ashx',
                                        allowFileManager: true
                                       
                                    });
                                });
            </script>
            <!--商品描述 end-->

            <div class="blank20"></div>
            <div class="btn-box">
                <div class="btn"><a href="javascript:;" id="btnAdds" class="btn-area">提交</a><a href="javascript:;" onclick="javascript:history.go(-1);" class="gray-btn">取消</a></div>
                <div class="bg"></div>
            </div>

        </div>
        <script type="text/javascript">
function beginSelect(elem){
	if(elem.className == "btn"){
		elem.className = "btn btnhover"
		elem.onmouseup = function(){
			this.className = "btn"
		}
	}
	var ul = elem.parentNode.parentNode;
	var li = ul.getElementsByTagName("li");
	var selectArea = li[li.length-1];
	if(selectArea.style.display == "block"){
		selectArea.style.display = "none";
	}
	else{
		selectArea.style.display = "block";
		mouseoverBg(selectArea);
	}
}
function mouseoverBg(elem1){
	var input = elem1.parentNode.getElementsByTagName("input")[0];
	var p = elem1.getElementsByTagName("p");
	var pLength = p.length;
	for(var i = 0; i < pLength; i++){
		p[i].onmouseover = showBg;
		p[i].onmouseout = showBg;
		p[i].onclick = postText;
	}
	function showBg(){
		this.className == "hover"?this.className = " ":this.className = "hover";
	}
	function postText(){
		var selected = this.innerHTML;
		input.setAttribute("value",selected);
		elem1.style.display = "none";

	}
}</script>



        <div class='Layer'>
        </div>
        <div class="tip" style="display: none; height: 400px; width: 400px;">
            <div style="position: absolute; top: 0; left: 0; right: 0; bottom: 0; z-index: 999; background: #fff;">
                <div class="tiptop">
                    <span>单位维护</span><a></a>
                </div>
                <div class="tipinfo">
                    <div class="lb">
                        <span><i class="required">*</i>单位名称：</span>
                        <input name="txtunits" id="txtunits" type="text" runat="server" class="box2 txtunits"
                            maxlength="15" />
                    </div>
                    <div class="tipbtn">
                        <input name="" type="button" class="orangeBtn btnAddUnit" value="确定" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <input name="" type="button" class="cancel" value="取消" />
                    </div>
                    <br />
                    <div id="jldwdel" style="height: 250px; overflow-x: auto; overflow-y: auto; top: expression_r(this.style.pixelHeight+document.body.scrollTop+273);">
                        <table>
                            <thead>
                                <th style="width: 199px; height: 30px; text-align: center; background-color: #f1f1f1;">计量单位
                                </th>
                                <th style="width: 199px; height: 30px; text-align: center; background-color: #f1f1f1;">操作
                                </th>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div id="xubox_border1" class="xubox_border" style="z-index: 11; opacity: 0.3; filter: Alpha(Opacity=30) !important; border-radius: 5px; top: -8px; left: -8px; right: -8px; bottom: -8px; background-color: rgb(0, 0, 0); position: absolute; top">
            </div>
        </div>
        <!--新增 end-->

        <input type="hidden" id="deleteIDlist" runat="server" name="deleteIDlist" />
        <input type="hidden" id="OdlIDList" runat="server" name="OdlIDList" />

        <div class="div_footer">
            <asp:Button ID="btnAdd" CssClass="orangeBtn" runat="server" Text="确定" OnClientClick="return formCheck()"
                OnClick="btnAdd_Click" />&nbsp;
            
        </div>
    </form>

</body>
</html>
