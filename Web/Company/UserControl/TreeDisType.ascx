<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TreeDisType.ascx.cs" Inherits="Company_UserControl_TreeDisType" %>
<style>
    .showDiv3
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
        $(".txt_txtTypename").click(function () {
            // $(".showDiv").show();
            if ($.trim($(".showDiv3").attr("class")) == "showDiv3 xy") {
                $(".showDiv3").hide();
                $(".showDiv3").removeClass("xy");
            } else {
                $("#IfameTreeType").attr("src", $("#IfameTreeType").attr("src"));
                $('#IfameTreeType').load(function () {
                    $(".showDiv3").show();
                    $(".showDiv3").addClass("xy");
                    $(".showDiv3").css("display", "block");
                });
            }
            var x = $(".txt_txtTypename").offset().left;
            var y = $(".txt_txtTypename").offset().top;
            // alert(x + "," + y);
            $(".showDiv3").css("position", "absolute");
            var inde = window.location.pathname.lastIndexOf("/");
            var str = window.location.pathname.substring(parseInt(inde) + 1);
            if (str == "DisPriceList.aspx") {
                $(".showDiv3").css("left", "438.65px");
            } else {
                $(".showDiv3").css("left", x - 5 + "px");
                $(".showDiv3").css("top", y + 25 + "px");
            }
        })
        $(document.body).click(function (e) {
            var xx = e.pageX; //鼠标X轴
            var yy = e.pageY; //鼠标Y轴
            var xxx = parseInt($(".txt_txtTypename").offset().left) - 5; //文本框坐标
            var yyy = parseInt($(".txt_txtTypename").offset().top) + 25; //文本框坐标
            var max = parseInt($(".txt_txtTypename").css("width").substring(0, $(".txt_txtTypename").css("width").length - 2)); //300;
            if (xx > xxx && xx < xxx + max && yy + 25 > yyy && yy < yyy) {
                //  alert(x + "," + xx + ";;" + y + "," + yy)
            } else {
                if ($.trim($(".showDiv3").attr("class")) == "showDiv3 xy") {
                    $(".showDiv3").hide();
                    $(".showDiv3").removeClass("xy");
                }
            }
        });
    })
</script>
<input type="hidden" id="hid_TypeId" class="hid_TypeId" runat="server" />
<input type="text" id="txt_txtTypename" readonly="readonly" class="box1 txt_txtTypename" style=" width:130px;"
    runat="server" />
<div style="display: none;width:130px;" class="showDiv3">
    <iframe src="../TreeDemo.aspx?type=3" class="ifrClass" id="IfameTreeType" scrolling="no"  style=" width:130px;"
        height="220px" frameborder="0"></iframe>
</div>
