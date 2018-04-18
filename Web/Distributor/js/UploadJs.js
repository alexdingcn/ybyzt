(function ($) {

    var $IFrameName = "IFrameUploadFileS";
    $.fn.AjaxUploadFile = function (options) {

        var ua = navigator.userAgent.toLowerCase(); //浏览器信息
        var info = {
            ie: /msie/.test(ua) && !/opera/.test(ua),        //匹配IE浏览器    
            op: /opera/.test(ua),     //匹配Opera浏览器    
            sa: /version.*safari/.test(ua),     //匹配Safari浏览器    
            ch: /chrome/.test(ua),     //匹配Chrome浏览器    
            ff: /gecko/.test(ua) && !/webkit/.test(ua)     //匹配Firefox浏览器
        };
        var option = $.extend({}, $.fn.config, options)
        return this.each(function () {
            $(this).on("change", function () {
                if (!info.ie) {
                    if (this.files[0].size > (option.maxlength * 1024 * 1024)) {
                        if (layerCommon) {
                            layerCommon.msg("附件已超过指定大小请重新选择", IconOption.错误);
                        }
                        else {
                            alert("附件已超过指定大小请重新选择");
                        }
                        $(this).val("");
                    }
                    else {
                        SetIframeInput(this);
                    }
                }
                else {
                    SetIframeInput(this);
                }
            })
        });

        function getFileName(o) {
            var pos = o.lastIndexOf("\\");
            return o.substring(pos + 1);
        }

        function SetIframeInput(input) {

            var inputBt =
            {
                DL: null,
                DD: null,
                I: null,
                Span: null,
                A: null
            }
            if ($("form#UpFileFrom").length != 0) {
                if (layerCommon) {
                    layerCommon.msg("当前附件正在上传中", IconOption.错误);
                }
                else {
                    alert("当前附件正在上传中");
                }
                $(input).val("");
                return;
            }
            var body = document.body;
            var FileName = getFileName(input.value);
            var Iframe;

            try { // for I.E.
                Iframe = document.createElement("<iframe name=\"" + $IFrameName + "\" >");
            } catch (ex) {
                Iframe = document.createElement('iframe');
                Iframe.name = $IFrameName;
            }
            Iframe.style.display = "none";
            $(body).before(Iframe);
            var form = document.createElement("form");
            form.target = $IFrameName;
            form.method = "post";
            form.encoding = "multipart/form-data";
            form.action = "" + option.AjaxSrc + "?UploadFiles=" + option.Src + "&maxLenth=" + (option.maxlength * 1024 * 1024) + "";
            form.style.display = "none";
            $(form).attr("id", "UpFileFrom");
            CreateFileInput($("#" + option.ShowDiv + ""));
            $(body).before(form);
            var FilePrent = $(input).parent();
            var File = $(input).clone();
            File.appendTo(FilePrent);
            BindFIleUP(File, option);
            $(form).append(input);

            if (info.ie)//IE 需要注册onload事件
            {
                Iframe.attachEvent("onload", CallBack);
            }
            else {
                Iframe.onload = CallBack;
            }
            form.submit();
            function CreateFileInput(div) {
                inputBt.DL = $(div).find("dl.teamList");
                if (inputBt.DL.length == 0) {
                    inputBt.DL = $("<dl class='teamList'></dl>");
                    $(div).append(inputBt.DL);
                }

                inputBt.DD = $("<dd></dd>");
                if (FileName.length > 15) {
                    inputBt.I = $("<a style='cursor:default;text-decoration:none;'></a>").attr("class", "bt").text(FileName.substring(0, 15) + "....").attr("title", FileName);
                }
                else {
                    inputBt.I = $("<a style='cursor:default;text-decoration:none;'></a>").attr("class", "bt").text(FileName);
                }
                inputBt.Span = $("<span></span>").attr("class", "speed").text("正在上传");
                inputBt.A = $("<a></a>").attr("class", "red").text("停止");
                inputBt.A.bind("click", Dispose);
                inputBt.DD.append(inputBt.I).append(inputBt.Span).append(inputBt.A);
                inputBt.DL.append(inputBt.DD);
            }

            function CallBack() {
                try {
                    var value = Iframe.contentWindow.document.body.innerHTML;
                    value = value.substring(value.indexOf("@returnstart@") + 13, value.indexOf("@returnend@"));
                    var obj = eval("(" + value + ")");
                    if (obj.result) {
                        FileFinsh(obj.name);
                    } else {
                        if (obj.Code) {
                            Filelose(obj.Code);
                        }
                        else {
                            Filelose("");
                        }
                    }
                }
                catch (e) {
                    Filelose("");
                }
            }


            function FileFinsh(name) {
                var HidVlue = $("#" + option.ResultId + "");
                if (HidVlue.val() == "") {
                    HidVlue.val(name);
                }
                else {
                    HidVlue.val(HidVlue.val() + "," + name);
                }
                $(inputBt.I).css({ "cursor": "pointer", "text-decoration": "" }).attr("href", option.DownSrc + "UploadFile/" + name + "").attr("target", "_blank");
                $(inputBt.A).data("FileName", name);
                $(Iframe).remove();
                $(form).remove();
                $(inputBt.Span).text("上传成功").attr("class", "speed green");
                $(inputBt.A).text("删除");
            }

            function Filelose(code) {
                $(Iframe).remove();
                $(form).remove();
                if (code != undefined && code != "") {
                    $(inputBt.Span).text(code).css("color", "red");
                } else {
                    $(inputBt.Span).text("上传失败").css("color", "red");
                }
                $(inputBt.A).text("删除");
            }

            function Dispose() {
                var fileNames = $(this).data("FileName");
                $(inputBt.DD).remove();
                $(Iframe).remove();
                $(form).remove();
                if (fileNames) {
                    var HidVlue = $("#" + option.ResultId + "");
                    if (HidVlue.val() == fileNames) {
                        HidVlue.val("");
                    } else if (HidVlue.val().indexOf(fileNames) == 0) {
                        HidVlue.val(HidVlue.val().replace(fileNames + ",", ""));
                    }
                    else {
                        HidVlue.val(HidVlue.val().replace("," + fileNames, ""));
                    }
                }
            }


        }
    }

    $.fn.config =
    {
        Src: "upfile",
        ShowDiv: "",
        ResultId: "",
        maxlength: 2,
        AjaxSrc: "",
        DownSrc: ""
    }

})(jQuery);


function BindFIleUP(input, option) {
    $(input).AjaxUploadFile(option);
}