<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ComLogisticsAdd.aspx.cs"
    Inherits="Company_SysManager_ComLogisticsAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>常用物流公司维护</title>
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            $_def.ID = "btnAdd";
            //选中快递公司
            $(".cont-1-expressall ul li dl dd a").click(function () {
                var val = $(this).text();
                if (!$(this).hasClass("allsel")) {
                    //已经选中的快递公司
                    $(this).addClass("allsel");
                    var logli = "<li title=\"" + val + "\" class=\"Comli\">" + val + "</li>";
                    $(".Complog ul").append(logli);
                }
                else {
                    var th = this;
                    $.ajax({
                        type: 'post',
                        url: 'ComLogisticsAdd.aspx?action=ComplogDel',
                        data: { json: val },
                        async: true, //true:同步 false:异步
                        success: function (result) {
                            var data = eval('(' + result + ')');

                            if (data["ds"].toString() == "0" || data["ds"].toString() == "3") {
                                //删除成功
                                $(th).removeClass("allsel");
                                //删除li
                                $(".Complog ul li").remove("li[title=\"" + val + "\"]");
                            }
                            else {
                                layerCommon.msg(data["prompt"].toString(), IconOption.错误);
                            }
                        }
                    });
                }
            });

            //新增保存
            $(".orangeBtn").click(function () {
                var json = "{";
                $(".Complog ul li").each(function (index, obj) {
                    var name = $(this).text();
                    json += ",\"" + index + "\":\"" + name + "\"";
                });

                json += "}";
                json = json.replace("{,\"0\"", "{\"0\"");

                $.ajax({
                    type: 'post',
                    url: 'ComLogisticsAdd.aspx?action=Complog',
                    data: { json: json },
                    async: false, //true:同步 false:异步
                    success: function (result) {
                        var data = eval('(' + result + ')');
                        if (data["ds"].toString() == "0") {
                            //window.parent.ComLogisticsAdd(json);
                            window.opener.ComLogisticsAdd(json);
                            window.close();
                            //window.parent.ComLogisticsAdd(json);
                            //window.parent.Layerclose();
                        } else {
                            layerCommon.msg(data["prompt"].toString(), IconOption.错误);
                        }
                    }
                });
            });

            //删除
            $(document).on("click", ".Comli", function () {
                var val = $(this).text();
                var th = this;

                $.ajax({
                    type: 'post',
                    url: 'ComLogisticsAdd.aspx?action=ComplogDel',
                    data: { json: val },
                    async: true, //true:同步 false:异步
                    success: function (result) {
                        var data = eval('(' + result + ')');

                        if (data["ds"].toString() == "0" || data["ds"].toString() == "3") {
                            //删除成功
                            $(th).remove();
                            $(".cont-1-expressall ul li dl dd a[title=\"" + val + "\"]").removeClass("allsel");
                        }
                        else {
                            layerCommon.msg(data["prompt"].toString(), IconOption.错误);
                        }
                    }
                });
            });

            //取消
            $(".cancel").click(function () {
                //window.parent.Layerclose(); //2016-02-24修改
                window.close();
            });
        });

        //加载绑定
        function bindComLog(json) {
            var obj = eval("(" + json + ")");
            var str = "";
            for (var js2 in obj) {
                str += "<li title=\"" + obj[js2] + "\" class=\"Comli\">" + obj[js2] + "</li>";
                $(".cont-1-expressall ul li dl dd a[title=\"" + obj[js2] + "\"]").addClass("allsel");
            }
            $(".Complog ul").append(str);
        }
    </script>
    <style>
        .cont
        {
            width: 960px;
            background: url(../images/ind-top.gif) top repeat-x;
            overflow: auto;
            width: 960px;
            height: auto;
            margin: 0 auto 0;
            margin-top: 10px;
            padding-top: 2px;
        }
        .cont-1
        {
            width: 330px;
            height: auto;
            overflow: auto;
            background: url(../images/ind-jb.png) right repeat-y;
            padding-right: 6px;
            float: left;
        }
        .cont-1-top
        {
            width: 330px;
            position: relative;
        }
        .cont-2
        {
            float: right;
            width: 620px;
            height: auto;
            overflow: hidden;
        }
        .cont-1-index h2, .cont-1 h1, .cont-2 h1, .cont-1-expresstop ul, .cont-1 h4
        {
            border-bottom: #B7B9BB solid 1px;
        }
        .cont-1-index h2, .cont-1 h1, .cont-2 h1, .cont-1 h4
        {
            display: block;
            height: 36px;
            overflow: hidden;
            line-height: 36px;
        }
        .cont-1-index h2, .cont-1 h1, .cont-2 h1
        {
            /* background: url(../images/ind-cy.png) 16px bottom no-repeat #FAFCFF;  text-indent: 1000px;*/
            background: #FAFCFF;
        }
        .cont-1-expresstop ul
        {
            padding-top: 6px;
            padding-bottom: 10px;
        }
        .cont-1-expresstop ul li
        {
            width: 45px;
            float: left;
            text-align: center;
            display: inline-block;
            margin-left: 16px;
            margin-top: 6px;
        }
        .cont-1-expresstop ul li a, .cont-1-expressall ul li dl dd a
        {
            height: 22px;
            line-height: 22px;
            font-size: 14px;
            display: block;
            color: #333;
        }
        .cont-1-expresstop ul li a:hover, .cont-1-expressall ul li dl dd a:hover
        {
            color: #FFA841;
        }
        .cont-1-expresstop ul li a.allsel
        {
            color: #fff;
            background-color: #6CA8FA;
        }
        .cont-1-expresstop ul
        {
            background-color: #EEFAFF;
            height: auto;
            overflow: auto;
        }
        .cont-1-expressall
        {
            position: relative;
            height: 850px;
           /* overflow:scroll; */
        }
        .cont-1-expressall ul
        {
            position: relative;
        }
        .cont-1-expressall ul li
        {
            line-height: 42px;
            height: 42px;
            width: 100%;
            float: left;
        }
        .cont-1-expressall ul li.all-s
        {
            background: url(../images/ind-fx.png) no-repeat #FAFCFF;
            border-right: 0;
        }
        .cont-1-expressall ul li em
        {
            display: block;
            width: 40px;
            background: url(../images/ind-ind.gif) 9px 0 no-repeat;
            font-family: Arial;
            color: #fff;
            font-size: 14px;
            float: left;
            text-indent: 15px;
        }
        .cont-1-expressall ul li em.all-i-s
        {
            background: url(../images/ind-ind-s.gif) 9px 0 no-repeat;
        }
        .cont-1-expressall ul li em.all-i-w
        {
            background: url(../images/ind-ind-w.gif) 9px 0 no-repeat;
        }
        .cont-1-expressall ul li dl
        {
            margin-top: 6px;
        }
        .cont-1-expressall ul li dl dd
        {
            float: left;
            width: 70px;
            margin-top: 5px;
        }
        .cont-1-expressall ul li dl dd a
        {
            padding: 0 5px;
            float: left;
        }
        .cont-1-expressall ul li dl dd a.allsel
        {
            color: #fff;
            background-color: #6CA8FA;
        }
        .cont-2-info
        {
            /*width: 625px;
            background-color: #fff;
            padding-top: 15px;
            height: auto;
            overflow: hidden;
            background: #fafcff;*/
            left: 0px;
            top: 0px;
            width: 620px;
            position: relative;
        }
        .cont-1-index
        {
            position: relative;
        }
        .cont-1-index h2
        {
            background: url(../images/ind-lf-tit.gif) 10px 9px no-repeat #FAFCFF;
            color: #ccc;
            text-indent: 45px;
            overflow: hidden;
        }
        #zmdw
        {
            background: url(../images/ind-zm-all.png) no-repeat;
            width: 43px;
            height: 15px;
            display: block;
            position: absolute;
            right: 10px;
            overflow: hidden;
            line-height: 100px;
            top: 11px;
        }
        #zmdw:hover
        {
            background-position: 0 -14px;
        }
        .cont-1-index ul
        {
            background-color: #EEFAFF;
            height: 62px;
            border-left: #B7B9BB solid 1px;
            display: none;
        }
        .cont-1-index ul li
        {
            float: left;
            height: 30px;
            line-height: 30px;
            width: 31px;
            border-right: #B7B9BB solid 1px;
            text-align: center;
            border-bottom: #B7B9BB solid 1px;
            cursor: pointer;
        }
        .cont-1-index ul li a
        {
            display: block;
            color: #333;
        }
        .cont-1-index ul li a:hover
        {
            background-color: #fff;
            color: #6CA8FA;
        }
        .cont-1-index ul
        {
            border-bottom: #eee solid 1px;
        }
        .cont-2-cont
        {
            height: auto;
            overflow: auto;
            padding: 5px 0 20px 20px;
            background: url("../images/ditu.gif");
        }
        .cont-2-cont ul
        {
            float: left;
            width: 302px;
            display: inline-block;
            height: 70px;
        }
        ul, menu, dir
        {
            display: block;
            list-style-type: disc;
            -webkit-margin-before: 1em;
            -webkit-margin-after: 1em;
            -webkit-margin-start: 0px;
            -webkit-margin-end: 0px;
            -webkit-padding-start: 40px;
        }
        li
        {
            list-style: none;
        }
        body
        {
            font: normal normal normal 12px/20px Arial,sans-serif;
        }
        html, body, legend
        {
            color: #333;
        }
        dd
        {
            display: block;
            -webkit-margin-start: 40px;
        }
        body, ul, ol, li, dl, dd, p, h1, h2, h3, h4, h5, h6, form, fieldset, table
        {
            margin: 0;
            padding: 0;
        }
        .div_footer
        {
            text-align: center;
            padding-top: 10px;
        }
        .orangeBtn
        {
            background: #ff4e02;
            font-weight: normal;
            border: 1px solid #ea5211;
        }
        .cancel
        {
            background: #efefef;
            color: #000;
            font-weight: normal;
            border: 1px solid #e0e0e0;
            height: 21px;
            width: 42px;
        }
        .Comli
        {
            cursor:pointer;
            display:inline-block;
            margin-left:10px;
            margin-top:10px;
            width:auto;
            background:#fafcff none repeat scroll 0 0;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="cont">
        <div class="cont-1">
            <div class="cont-1-top">
                <h1>
                    快递公司</h1>
                
            </div>
            <div id="cont-1-expressall" class="cont-1-expressall">
                <ul>
                    <li><em class="all-i-s">A</em>
                        <dl>
                            <dd>
                                <a code="aae" href="javascript:void(0)" title="AAE快递" class="">AAE快递</a></dd>
                            <dd>
                                <a code="axd" href="javascript:void(0)" title="安信达" class="">安信达</a></dd>
                            <dd>
                                <a code="aj" href="javascript:void(0)" title="安捷" class="">安捷</a></dd>
                        </dl>
                    </li>
                    <li class="all-s"><em>B</em>
                        <dl>
                            <dd>
                                <a code="bgpyghx" href="javascript:void(0)" title="包裹平邮" class="">包裹平邮</a></dd>
                            <dd>
                                <a code="huitong" href="javascript:void(0)" title="百世汇通" class="">百世汇通</a></dd>
                        </dl>
                    </li>
                    <li><em style="text-indent: 14px;">C</em>
                        <dl>
                            <dd>
                                <a code="cs" href="javascript:void(0)" title="城市100" class="">城市100</a></dd>
                            <dd>
                                <a code="cszx" href="javascript:void(0)" title="城市之星" class="">城市之星</a></dd>
                            <dd>
                                <a code="chuanzhi" href="javascript:void(0)" title="传志快递" class="">传志快递</a></dd>
                        </dl>
                    </li>
                    <li class="all-s"><em>D</em>
                        <dl>
                            <dd>
                                <a code="dhl" href="javascript:void(0)" title="DHL快递">DHL快递</a></dd>
                            <dd>
                                <a code="debang" href="javascript:void(0)" title="德邦物流">德邦物流</a></dd>
                            <dd>
                                <a code="dtwl" href="javascript:void(0)" title="大田物流">大田物流</a></dd>
                            <dd>
                                <a code="dpex" href="javascript:void(0)" title="DPEX">DPEX</a></dd>
                        </dl>
                    </li>
                    <li><em>E</em>
                        <dl>
                            <dd>
                                <a code="ems" href="javascript:void(0)" title="EMS快递">EMS快递</a></dd>
                            <dd>
                                <a code="ems" href="javascript:void(0)" title="E邮宝">E邮宝</a></dd>
                        </dl>
                    </li>
                    <li class="all-s"><em>F</em>
                        <dl>
                            <dd>
                                <a code="rufengda" href="javascript:void(0)" title="凡客诚品">凡客诚品</a></dd>
                            <dd>
                                <a code="fbwl" href="javascript:void(0)" title="飞邦物流">飞邦物流</a></dd>
                            <dd>
                                <a href="javascript:void(0)" title="飞洋快递" code="feiyang">飞洋快递</a></dd>
                            <dd>
                                <a href="javascript:void(0)" title="FEDEX" code="fedex">FEDEX</a></dd>
                        </dl>
                    </li>
                    <li><em style="text-indent: 14px">G</em>
                        <dl>
                            <dd>
                                <a code="guotong" href="javascript:void(0)" title="国通快递">国通快递</a></dd>
                            <dd>
                                <a code="gznd" href="javascript:void(0)" title="港中能达">港中能达</a></dd>
                            <dd>
                                <a code="gjbg" href="javascript:void(0)" title="国际包裹">国际包裹</a></dd>
                            <dd>
                                <a code="gsdwl" href="javascript:void(0)" title="共速达">共速达</a></dd>
                        </dl>
                    </li>
                    <li class="all-s"><em>H</em>
                        <dl>
                            <dd>
                                <a code="huitong" href="javascript:void(0)" title="汇通快递">汇通快递</a></dd>
                            <dd>
                                <a code="huiqiang" href="javascript:void(0)" title="汇强快递">汇强快递</a></dd>
                            <dd>
                                <a code="tdhy" href="javascript:void(0)" title="华宇物流">华宇物流</a></dd>
                            <dd>
                                <a code="hswl" href="javascript:void(0)" title="昊盛">昊盛</a></dd>
                        </dl>
                    </li>
                    <li><em>J</em>
                        <dl>
                            <dd>
                                <a code="jywl" href="javascript:void(0)" title="佳怡物流">佳怡物流</a></dd>
                            <dd>
                                <a code="jingdong" href="javascript:void(0)" title="京东快递">京东快递</a></dd>
                            <dd>
                                <a code="jiaji" href="javascript:void(0)" title="佳吉快运">佳吉快运</a></dd>
                        </dl>
                    </li>
                    <li class="all-s"><em>K</em>
                        <dl>
                            <dd>
                                <a code="kuaijie" href="javascript:void(0)" title="快捷快递">快捷快递</a></dd>
                            <dd>
                                <a code="klwl" href="javascript:void(0)" title="康力物流">康力物流</a></dd>
                        </dl>
                    </li>
                    <li><em style="text-indent: 14px">M</em>
                        <dl>
                            <dd>
                                <a code="minhang" href="javascript:void(0)" title="民航快递">民航快递</a></dd>
                            <dd>
                                <a code="meiguo" href="javascript:void(0)" title="美国快递">美国快递</a></dd>
                            <dd>
                                <a code="minbang" href="javascript:void(0)" title="民邦快递">民邦快递</a></dd>
                        </dl>
                    </li>
                    <li class="all-s"><em>N</em>
                        <dl>
                            <dd>
                                <a code="gznd" href="javascript:void(0)" title="能达快递">能达快递</a></dd>
                        </dl>
                    </li>
                    <li><em style="text-indent: 14px">Q</em>
                        <dl>
                            <dd>
                                <a code="quanfeng" href="javascript:void(0)" title="全峰快递">全峰快递</a></dd>
                            <dd>
                                <a code="quanyi" href="javascript:void(0)" title="全一快递">全一快递</a></dd>
                            <dd>
                                <a code="quanchen" href="javascript:void(0)" title="全晨快递">全晨快递</a></dd>
                            <dd>
                                <a code="quanritong" href="javascript:void(0)" title="全日通">全日通</a></dd>
                        </dl>
                    </li>
                    <li class="all-s"><em>R</em>
                        <dl>
                            <dd>
                                <a code="rufengda" href="javascript:void(0)" title="如风达">如风达</a></dd>
                            <dd>
                                <a code="rrs" href="javascript:void(0)" title="日日顺" class="">日日顺</a></dd>
                        </dl>
                    </li>
                    <li><em>S</em>
                        <dl>
                            <dd>
                                <a code="shentong" href="javascript:void(0)" title="申通快递">申通快递</a></dd>
                            <dd>
                                <a code="shunfeng" href="javascript:void(0)" title="顺丰快递">顺丰快递</a></dd>
                            <dd>
                                <a code="suer" href="javascript:void(0)" title="速尔快递">速尔快递</a></dd>
                            <dd>
                                <a code="haihong" href="javascript:void(0)" title="海红">海红</a></dd>
                        </dl>
                    </li>
                    <li class="all-s"><em>T</em>
                        <dl>
                            <dd>
                                <a code="tnt" href="javascript:void(0)" title="TNT快递">TNT快递</a></dd>
                            <dd>
                                <a code="tiantian" href="javascript:void(0)" title="天天快递">天天快递</a></dd>
                            <dd>
                                <a code="tcwl" href="javascript:void(0)" title="通成物流">通成物流</a></dd>
                        </dl>
                    </li>
                    <li><em>U</em>
                        <dl>
                            <dd>
                                <a code="ups" href="javascript:void(0)" title="UPS国际">UPS国际</a></dd>
                        </dl>
                    </li>
                    <li class="all-s"><em>X</em>
                        <dl>
                            <dd>
                                <a code="xinbang" href="javascript:void(0)" title="新邦物流">新邦物流</a></dd>
                            <dd>
                                <a code="xfwl" href="javascript:void(0)" title="信丰物流">信丰物流</a></dd>
                        </dl>
                    </li>
                    <li><em>Y</em>
                        <dl>
                            <dd>
                                <a code="yuantong" href="javascript:void(0)" title="圆通速递">圆通速递</a>
                            </dd>
                            <dd>
                                <a code="yunda" href="javascript:void(0)" title="韵达快递">韵达快递</a></dd>
                            <dd>
                                <a code="yafeng" href="javascript:void(0)" title="亚风快递">亚风快递</a></dd>
                            <dd>
                                <a code="bgpyghx" href="javascript:void(0)" title="邮政物流">邮政物流</a></dd>
                        </dl>
                    </li>
                    <li class="all-s"><em class="all-i-w">Z</em>
                        <dl>
                            <dd>
                                <a code="zhongtong" href="javascript:void(0)" title="中通快递">中通快递</a></dd>
                            <dd>
                                <a code="zhongyou" href="javascript:void(0)" title="中邮物流">中邮物流</a></dd>
                            <dd>
                                <a code="zjs" href="javascript:void(0)" title="宅急送" class="">宅急送</a></dd>
                        </dl>
                    </li>
                </ul>
            </div>
        </div>
        <div class="cont-2">
            <div class="cont-2-info">
                <h1>
                    已选中物流公司</h1>
            </div>
            <div class="Complog">
                <ul id="ulComLog" runat="server">
                </ul>
                <div class="div_footer">
                    <asp:Button ID="btnAdd" CssClass="orangeBtn" runat="server" Text="确 定"  style=" cursor:pointer;"
                        Height="25px" Width="50px" /> &nbsp; 
                    <input name="" type="button" class="cancel"   value="关 闭" style="height: 25px; width:50px; cursor:pointer; " />
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
