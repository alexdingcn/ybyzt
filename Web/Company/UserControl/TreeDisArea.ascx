<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TreeDisArea.ascx.cs" Inherits="Company_UserControl_TreeDisArea" %>
<style>
    .showDiv2
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
        $(".txt_txtAreaname").click(function () {
            // $(".showDiv").show();
            if ($.trim($(".showDiv2").attr("class")) == "showDiv2 xy") {
                $(".showDiv2").hide();
                $(".showDiv2").removeClass("xy");
            } else {
                $("#IfameTreeArea").attr("src", $("#IfameTreeArea").attr("src"));
                $('#IfameTreeArea').load(function () {
                    $(".showDiv2").show();
                    $(".showDiv2").addClass("xy");
                    $(".showDiv2").css("display", "block");
                });

            }
            var x = $(".txt_txtAreaname").offset().left;
            var y = $(".txt_txtAreaname").offset().top;
            // alert(x + "," + y);
            var inde = window.location.pathname.lastIndexOf("/");
            var str = window.location.pathname.substring(parseInt(inde) + 1);
            $(".showDiv2").css("position", "absolute");
            if (str == "GoodsAreaEdit.aspx") {
                $(".showDiv2").css({ "left": (x - 143 + "px"), "top": "27px" });
            } else {
                $(".showDiv2").css("left", x - 5 + "px");
                $(".showDiv2").css("top", y + 25 + "px");
            }
        })
        $(document.body).click(function (e) {
            var xx = e.pageX; //鼠标X轴
            var yy = e.pageY; //鼠标Y轴
            var xxx = parseInt($(".txt_txtAreaname").offset().left) - 5; //文本框坐标
            var yyy = parseInt($(".txt_txtAreaname").offset().top) + 25; //文本框坐标
            var max = parseInt($(".txt_txtAreaname").css("width").substring(0, $(".txt_txtAreaname").css("width").length - 2)); //300;
            if (xx > xxx && xx < xxx + max && yy + 25 > yyy && yy < yyy) {
                //  alert(x + "," + xx + ";;" + y + "," + yy)
            } else {
                if ($.trim($(".showDiv2").attr("class")) == "showDiv2 xy") {
                    $(".showDiv2").hide();
                    $(".showDiv2").removeClass("xy");
                }
            }
        });
    })
</script>
<input type="hidden" id="hid_AreaId" class="hid_AreaId" runat="server" />
<input type="text" id="txt_txtAreaname" readonly="readonly" class="box1 txt_txtAreaname"
    style="width: 130px;" runat="server" />
<div style="display: none; width: 130px;" class="showDiv2">
    <iframe src="../TreeDemo.aspx?type=2" class="ifrClass" id="IfameTreeArea" scrolling="no"
        style="width: 130px;" height="220px" frameborder="0"></iframe>
</div>
