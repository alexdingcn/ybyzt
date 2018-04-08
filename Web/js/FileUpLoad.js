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
                var ext = getFileName(this.value);
                ext = $.trim(ext.substring(ext.lastIndexOf(".")));
                if (!(ext.toUpperCase() == ".PDF" || ext.toUpperCase() == ".DOC" || ext.toUpperCase() == ".XLS" || ext.toUpperCase() == ".DOCX" || ext.toUpperCase() == ".XLSX" || ext.toUpperCase() == ".TXT" || ext.toUpperCase() == ".JPG" || ext.toUpperCase() == ".PNG" || ext.toUpperCase() == ".BMP" || ext.toUpperCase() == ".GIF" || ext.toUpperCase() == ".RAR" || ext.toUpperCase() == ".ZIP")) {
                    layerCommon.msg("请确认您的文件上传格式", IconOption.错误);
                    return;
                }
                if (!info.ie) {
                    if (this.files[0].size > (option.maxlength * 1024 * 1024)) {
                        if (layerCommon) {
                            layerCommon.msg("只能上传" + option.maxlength + "M以下的附件", IconOption.错误);
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
                LI: null,
                Aname: null,
                Adel: null,
                Adown: null,
                Span: null
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
            function CreateFileInput(SHowUL) {
                inputBt.LI = $("<li></li>");

                if (FileName.length > 20) {
                    inputBt.Aname = $('<a style=" text-decoration:none;" class="name" ></a>').text(FileName.substring(0, 20) + "....").attr("title", FileName);
                }
                else {
                    inputBt.Aname = $('<a style=" text-decoration:none;" class="name" ></a>').text(FileName);
                }
                inputBt.Span = $('<span style="color: #999; margin-left:10px;margin-right:10px" ></span>').text("正在上传...");
                inputBt.LI.append(inputBt.Aname).append(inputBt.Span);
                SHowUL.append(inputBt.LI);
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
                    HidVlue.val(HidVlue.val() + "&&" + name);
                }
                $(Iframe).remove();
                $(form).remove();
                $(inputBt.Span).text("上传成功").css("color", "#14ab00");
                inputBt.Adel = $('<a href="javascript:;" class="bule attrdel">删除</a>').data("FileName", name);
                inputBt.Adel.bind("click", Dispose);
                inputBt.Adown = $('<a href="/fileDown.aspx?filename=' + name + '" target="_blank" class="bule">下载</a>');
                $(inputBt.LI).append(inputBt.Adel).append(inputBt.Adown);

            }

            function Filelose(code) {
                $(Iframe).remove();
                $(form).remove();
                if (code != undefined && code != "") {
                    $(inputBt.Span).text(code).css("color", "red");
                } else {
                    $(inputBt.Span).text("上传失败").css("color", "red");
                }
                inputBt.Adel = $('<a href="javascript:;" class="bule attrdel">删除</a>').data("FileName", name);
                inputBt.Adel.bind("click", function () {
                    $(this).closest("li").remove();
                });
                $(inputBt.LI).append(inputBt.Adel);
            }

            function Dispose() {
                var fileNames = $(this).data("FileName");
                $(this).closest("li").remove();
                $(Iframe).remove();
                $(form).remove();
                if (fileNames) {
                    var HidVlue = $("#" + option.ResultId + "");
                    if (HidVlue.val() == fileNames) {
                        HidVlue.val("");
                    } else if (HidVlue.val().indexOf(fileNames) == 0) {
                        HidVlue.val(HidVlue.val().replace(fileNames + "&&", ""));
                    }
                    else {
                        HidVlue.val(HidVlue.val().replace("&&" + fileNames, ""));
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