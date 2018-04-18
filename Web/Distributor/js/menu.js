//分辨率
if(screen.width<=1400){document.write("<link rel='stylesheet' type='text/css' href='/css/size1366.css' />");}
if(screen.width<=1270){document.write("<link rel='stylesheet' type='text/css' href='/css/size1024.css' />");}
if(screen.width<=800){document.write("<link rel='stylesheet' type='text/css' href='/css/size800.css' />");}
			
//鼠标移上去选项卡			
//function setTab2(name,cursel,n){
//for(i=1;i<=n;i++){
//var menu=document.getElementById(name+i);
//var con =document.getElementById("con_"+name+"_"+i);
//menu.className=i==cursel?"hover":"";
//con.style.display=i==cursel?"block":"none";
//}

//}	
//主图轮播
$(document).ready(function () {

    $(".prev,.next").hover(function () {
        $(this).stop(true, false).fadeTo("show", 0.9);
    }, function () {
        $(this).stop(true, false).fadeTo("show", 0.4);
    });

    if ($(".main-slider-ad").slide) {
        $(".main-slider-ad").slide({
            titCell: ".hd ul",
            mainCell: ".bd ul",
            prevCell: ".banner-btn .prev",
            nextCell: ".banner-btn .next",
            effect: "fold",
            interTime: 4000,
            delayTime: 600,
            autoPlay: true,
            autoPage: true,
            trigger: "mouseover"
        });
    }
});



//鼠标移上去图片移动
$(function () {
        $(".comeUp a").on({ "mouseover": function () {
            $(this).children("span").animate({
                bottom: '5px'
            });
        }, "mouseout": function () {
             $(this).children("span").animate({
                bottom: '0'
            });
        }   
        });
    })

//鼠标移上去切换
function setTab2(name,cursel,n){
for(i=1;i<=n;i++){
var menu=document.getElementById(name+i);
var con =document.getElementById("con_"+name+"_"+i);
menu.className=i==cursel?"hover":"";
con.style.display=i==cursel?"block":"none";
}

}


