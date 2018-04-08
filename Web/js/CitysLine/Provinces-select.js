//绑定省
function Province() {
    $(".Province_Citys_list").html("");
    for (var i = 0, l = provinces.length; i < l; i++) {

        //var $li = "<li><a href='javascript:;' onclick='Citys(\"" + provinces[i]['name'] + "\")'>" + provinces[i]["name"] + "</a></li>";

        var $li = "<li><a href='javascript:;' onclick='Province_Click(this)'>" + provinces[i]["name"] + "</a></li>";

        $(".Province_Citys_list").append($li);
    }
    $(".Citys_btn").prop("class", "Citys_btn");
    $(".Province_btn").prop("class", "Province_btn hover");
}
//绑定市
function Citys(obj) {
    $(".Province_btn").text(obj);
    var Province = $(".Province_btn").text();
    var proCode = "";
    $(".Province_Citys_list").html("");
    for (var i = 0, l = provinces.length; i < l; i++) {
        for (var key in provinces[i]) {
            if (provinces[i]["name"] == Province) {
                proCode = provinces[i]["code"];
            }
        }
    }
    for (var i = 0, l = citys.length; i < l; i++) {
        if (citys[i]["code"].substring(0, 2) == proCode.substring(0, 2)) {
            var $li = '<li><a href="javascript:;" onclick="Citys_Click(this)" >' + citys[i]["name"] + '</a></li>';
            $(".Province_Citys_list").append($li);

        }
    }
    $(".Citys_btn").text("请选择");
    $(".Citys_btn").prop("class", "Citys_btn hover");
    $(".Province_btn").prop("class", "Province_btn");
}
//市 选中事件
function Citys_Click(obj) {
    $(".Citys_btn").text($(obj).html());
    $("#SelectDiv").hide();
    $("#SelectDiv").prop("class", "select")
    var prov = $(".Province_btn").text()
        $(".text_select").text($(obj).html())
        $("#text_select").val($(obj).html());
  
    $("#AddName_").val($("#text_select").val())
    $("#Shengshi")[0].click();
}

//省 选中事件
function Province_Click(obj) {
    $("#SelectDiv").hide();
    $("#SelectDiv").prop("class", "select")
    $(".text_select").text($(obj).html())
    $("#text_select").val($(obj).html());

    $("#AddName_").val($("#text_select").val())
    $("#Shengshi")[0].click();
}

//级联菜单 显示隐藏事件
function textselect() {

    if ($("#SelectDiv").prop("class") == "select") {
        $("#SelectDiv").show();
        $("#SelectDiv").prop("class", "select hover")
    }
    else {
        $("#SelectDiv").hide();
        $("#SelectDiv").prop("class", "select")
    }
}
//选择市
function Citys_() {
    var Province_btn = $(".Province_btn").text();
    Citys("" + Province_btn + "")
    $(".Citys_btn").prop("class", "Citys_btn hover");
    $(".Province_btn").prop("class", "Province_btn");
}
//省市导航栏条件取消
function Province_title() {
    $("#AddName_").val("");
    $("#Titlt_close")[0].click();
}
//省市导航栏条件取消
function GoodName_title() {

    $("#GoodsName_").val("");
    $("#Titlt_close")[0].click();
}
