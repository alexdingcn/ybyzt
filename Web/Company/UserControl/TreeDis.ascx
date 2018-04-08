<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TreeDis.ascx.cs" Inherits="Company_UserControl_TreeDis" %>
<style>
    .showDiv5
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
        $(".txt_txtDisname").click(function () {
            // $(".showDiv").show();
            if ($.trim($(".showDiv5").attr("class")) == "showDiv5 xy") {
                $(".showDiv5").hide();
                $(".showDiv5").removeClass("xy");
            } else {
                $(".showDiv5").show();
                $(".showDiv5").addClass("xy");
                $(".showDiv5").css("display", "block");
            }
            var x = $(".txt_txtDisname").offset().left;
            var y = $(".txt_txtDisname").offset().top;
            // alert(x + "," + y);
            $(".showDiv5").css("position", "absolute");
            $(".showDiv5").css("left", x - 5 + "px");
            $(".showDiv5").css("top", y + 25 + "px");
        })
        $(document.body).click(function (e) {
            var xx = e.pageX; //鼠标X轴
            var yy = e.pageY; //鼠标Y轴
            var xxx = parseInt($(".txt_txtDisname").offset().left) - 5; //文本框坐标
            var yyy = parseInt($(".txt_txtDisname").offset().top) + 25; //文本框坐标
            var max = parseInt($(".txt_txtDisname").css("width").substring(0, $(".txt_txtDisname").css("width").length - 2)); //300;

            if (xx > xxx && xx < xxx + max && yy + 25 > yyy && yy < yyy) {
                //  alert(x + "," + xx + ";;" + y + "," + yy)
            } else {
                if ($.trim($(".showDiv5").attr("class")) == "showDiv5 xy") {
                    $(".showDiv5").hide();
                    $(".showDiv5").removeClass("xy");
                }
            }
        });
    })
</script>
<input type="hidden" id="hid_DisId" class="hid_DisId" runat="server" />
<input type="text" id="txt_txtDisName" readonly="readonly" class="textBox txt_txtDisname"
     runat="server" />
<div style="display: none;" class="showDiv5">
    <iframe src="../TreeDemo.aspx?type=5" class="ifrClass" scrolling="no"
        height="220px" frameborder="0"></iframe>
</div>