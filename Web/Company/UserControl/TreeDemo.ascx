<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TreeDemo.ascx.cs" Inherits="Company_UserControl_TreeDemo" %>
<style>
    .showDiv
    {
        margin-left: 5px;
        z-index: 994;
    }
    .xy
    {
        position: absolute;
        z-index: 994;
    }
</style>
<script>
    var IsLock = true;
    $(function () {
        $(".txt_product_class").on("click", function () {
            IsLock = true;
            var inde = window.location.pathname.lastIndexOf("/");
            var str = window.location.pathname.substring(parseInt(inde) + 1);
            // $(".showDiv").show();
            if ($.trim($(".showDiv").attr("class")) == "showDiv xy") {
                $(".showDiv").hide();
                $(".showDiv").removeClass("xy");
            } else {
                var IframeSrc = $(".showDiv .ifrClass").attr("default-src");
                //                if ($.trim($(this).val()) != "") {
                //                    IframeSrc += ("&val=" + encodeURI($.trim($(this).val())));
                //                }
                $(".showDiv .ifrClass").attr("src", IframeSrc);

                $('.showDiv .ifrClass').load(function () {
                    $(".showDiv").show();
                    $(".showDiv").addClass("xy");
                });
            }
            var x = $(".txt_product_class").offset().left;
            var y = $(".txt_product_class").offset().top;
            if (str == "CompGoodsList.aspx") {
                $(".showDiv").css({ "left": (x - 5 + "px"), "top": "128px" });
            } else if (str == "GoodsAreaEdit.aspx") {
                $(".showDiv").css({ "left": (x - 548 + "px"), "top": "27px" });
            }else {
                $(".showDiv").css({ "left": (x - 5 + "px"), "top": (y + 25 + "px") });
            }
        })
        $(document.body).click(function (e) {
            if ($(".showDiv").is(":visible")) {
                IsLock = false;
                $(".txt_product_class").val($(".hid_product_CateGoryName").val());
            }
            var inde = window.location.pathname.lastIndexOf("/");
            var str = window.location.pathname.substring(parseInt(inde) + 1);
            var xx = e.pageX; //鼠标X轴
            var yy = e.pageY; //鼠标Y轴
            var xxx = parseInt($(".txt_product_class").offset().left) - 5; //文本框坐标
            var yyy = parseInt($(".txt_product_class").offset().top) + 25; //文本框坐标
            var max = parseInt($(".txt_product_class").css("width").substring(0, $(".txt_product_class").css("width").length - 2)); //300;
            if (str == "GoodsEdit.aspx") {
                var x = parseInt($(".txtunit").offset().left) - 5; //文本框坐标
                var y = parseInt($(".txtunit").offset().top) + 25; //文本框坐标
                if (xx > x && xx < x + 172 && yy + 25 > y && yy < y) {
                    //  alert(x + "," + xx + ";;" + y + "," + yy)
                } else {
                    if ($.trim($(".pullDown2").attr("class")) == "pullDown2 xy") {
                        $(".pullDown2").hide();
                        $(".pullDown2").removeClass("xy");
                    }
                }

            }
            if (xx > xxx && xx < xxx + max && yy + 25 > yyy && yy < yyy) {

            } else {
                if ($.trim($(".showDiv").attr("class")) == "showDiv xy") {
                    $(".showDiv").hide().removeClass("xy");
                }
            }

        });

        //按钮被松开发生
        $(".txt_product_class").on("input propertychange", function (e, data) {
            if (e.originalEvent.propertyName == "onFocus") {
                return;
            }
            //删除pullDown下所有div
            if (!IsLock) return;
            $("div .pullDown").html("");
            $.trim($(this).val()) == "" && $(".hid_product_CateGoryName").val("") && $(".hid_product_class").val("");
            var url = '/Company/TreeDemo.aspx?type=1&val=' + $(this).val();
            $(".showDiv .ifrClass").attr("src", url);
        });

    })
</script>
<input type="hidden" id="hid_product_class" class="hid_product_class" runat="server" />
<input type="hidden" id="hid_product_CateGoryName" class="hid_product_CateGoryName" runat="server" />
<input type="text" id="txt_product_class" autocomplete="off" class="textBox txt_product_class" style="width:220px !important;"
    runat="server" maxlength="50" />
<div style="display: none; width:220px !important; " class="showDiv">
    <iframe  default-src="/Company/TreeDemo.aspx?type=1" class="ifrClass" scrolling="no" style="width:225px !important; " height="220px"
        frameborder="0"></iframe>
</div>
