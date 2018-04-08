<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TreeDisName.ascx.cs" Inherits="Company_UserControl_TreeDisName" %>
<style>
    .showDiv6
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
            $(".showDiv6 .ifrClass").attr("src", "../TreeDemo.aspx?type=6");
            if ($.trim($(".showDiv6").attr("class")) == "showDiv6 xy") {
                $(".showDiv6").hide();
                $(".showDiv6").removeClass("xy");
            } else {
                $(".showDiv6").show();
                $(".showDiv6").addClass("xy");
                $(".showDiv6").css("display", "block");

            }
            var x = $(".txt_txtDisname").offset().left;
            var y = $(".txt_txtDisname").offset().top;
            // alert(x + "," + y);
            $(".showDiv6").css("position", "absolute");
            $(".showDiv6").css("left", x - 5 + "px");
            $(".showDiv6").css("top", y + 25 + "px");
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
                if ($.trim($(".showDiv6").attr("class")) == "showDiv6 xy") {
                    $(".showDiv6").hide();
                    $(".showDiv6").removeClass("xy");
                }
            }
        });
    })
</script>
<input type="hidden" id="hid_DisId" class="hid_DisId" runat="server" />
<input type="text" id="txt_txtDisName" readonly="readonly" style=" cursor:pointer;" class="textBox txt_txtDisname"
    runat="server" />
<div style="display: none;" class="showDiv6">
    <iframe src="../TreeDemo.aspx?type=6" class="ifrClass" id="IfameTreeDisName" scrolling="no"
        height="220px" frameborder="0"></iframe>
</div>
