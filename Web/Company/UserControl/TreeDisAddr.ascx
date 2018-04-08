<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TreeDisAddr.ascx.cs" Inherits="Company_UserControl_TreeDisAddr" %>
<style>
    .showDiv4
    {
        margin-left: 5px;
        z-index: 994;
    }
    .xy
    {
        z-index: 994;
        position: absolute;
    }
</style>
<script>
    $(function () {
        $(".txt_product_class").click(function () {
            if ($.trim($("#txtDisID1_hid_DisId").val()) == "") {
                alert("请选择代理商");
                return;
            }
            $(".showDiv4 .ifrClass").attr("src", "../TreeDemo.aspx?type=4&disId=" + $.trim($("#txtDisID1_hid_DisId").val()));
            // $(".showDiv").show();
            if ($.trim($(".showDiv4").attr("class")) == "showDiv4 xy") {
                $(".showDiv4").hide();
                $(".showDiv4").removeClass("xy");
            } else {
                $(".showDiv4").show();
                $(".showDiv4").addClass("xy");
                $(".showDiv4").css("display", "block");
            }
            var x = $(".txt_product_class").offset().left;
            var y = $(".txt_product_class").offset().top;
            // alert(x + "," + y);
            $(".showDiv4").css("position", "absolute");
            $(".showDiv4").css("left", x - 5 + "px");
            $(".showDiv4").css("top", y + 25 + "px");
        })
        $(document.body).click(function (e) {
            var xx = e.pageX; //鼠标X轴
            var yy = e.pageY; //鼠标Y轴
            var xxx = parseInt($(".txt_product_class").offset().left) - 5; //文本框坐标
            var yyy = parseInt($(".txt_product_class").offset().top) + 25; //文本框坐标
            var max = parseInt($(".txt_product_class").css("width").substring(0, $(".txt_product_class").css("width").length - 2)); //300;
            if (xx > xxx && xx < xxx + max && yy + 25 > yyy && yy < yyy) {
                //  alert(x + "," + xx + ";;" + y + "," + yy)
            } else {
                if ($.trim($(".showDiv4").attr("class")) == "showDiv4 xy") {
                    $(".showDiv4").hide();
                    $(".showDiv4").removeClass("xy");

                }
            }
        });
    })
</script>
<input type="hidden" id="hid_product_class" class="hid_product_class" runat="server" />
<input type="text" id="txt_product_class" readonly="readonly" class="textBox txt_product_class"
    style="margin-left: 5px;" runat="server" />
<div style="display: none;" class="showDiv4">
    <iframe src="" class="ifrClass" scrolling="no" height="220px" frameborder="0"></iframe>
</div>
