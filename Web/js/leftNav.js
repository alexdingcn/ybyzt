$(document).ready(function () {
    /* 滑动/展开 */
    $(".expmenu li > div.head").click(function () {

        var DownObj = $(".expmenu li > div.head").find("span.down").parent("div");
        UlDown(DownObj);
        if (DownObj[0] != this) {
            var arrow = $(this).find("span.arrow");

            if (arrow.hasClass("up")) {
                arrow.removeClass("up");
                arrow.addClass("down");
            } else if (arrow.hasClass("down")) {
                arrow.removeClass("down");
                arrow.addClass("up");
            }
            $(this).parent().find("ul.menu").slideToggle();
        }
    });

    function UlDown(Obj) {
        var arrow = $(Obj).find("span.arrow");

        if (arrow.hasClass("up")) {
            arrow.removeClass("up");
            arrow.addClass("down");
        } else if (arrow.hasClass("down")) {
            arrow.removeClass("down");
            arrow.addClass("up");
        }

        $(Obj).parent().find("ul.menu").slideToggle();
    }

});

function ShowMenu(obj,n){
    var Nav = obj.parentNode.parentNode;
 if(!Nav.id){
     var BName = Nav.getElementsByTagName("ol");
     if ($(obj.parentNode).next("ol").find("a").length == 0) {
         return;
     }
  var HName = Nav.getElementsByTagName("h2");
  var t = 2;
 }else{
  var BName = document.getElementById(Nav.id).getElementsByTagName("span");
  var HName = document.getElementById(Nav.id).getElementsByTagName(".head");
  var t = 1;
 }
 for(var i=0; i<HName.length;i++){
  HName[i].innerHTML = HName[i].innerHTML.replace("-","+");
  HName[i].className = "";
 }
obj.parentNode.className = "h" + t;
 for(var i=0; i<BName.length; i++){if(i!=n){BName[i].className = "no";}}
 if(BName[n].className == "no"){
     BName[n].className = "";
     $(obj).html("-");
//  obj.innerHTML = obj.innerHTML.replace("+","-");
 }else{
     BName[n].className = "no";
     $(obj).html("+");
     obj.parentNode.className = "";
//  obj.innerHTML = obj.innerHTML.replace("-","+");
 }
}
