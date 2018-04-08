
(function ($) {
    $.extend({ BindRegister: function (BindType, CompId) {
        //企业注册
        CompId == undefined && (CompId = "");
        $(document).on("keydown", function (e) {
            if (e.keyCode == 13) {
                if ($(".mianDiv .RegisterNo1").is(":visible")) {
                    $(".mianDiv .RegisterNo1 #btn_Submit").trigger("click");
                }
                else if ($(".mianDiv .RegisterNo2").is(":visible")) {
                    $(".mianDiv .RegisterNo2 #btnRegister").trigger("click");
                }
            }
        });
        var CompRegister = {
            OnInit: function () {
                CompRegister.No1BindEevent();
            },
            No1BindEevent: function () {
                $(".mianDiv .RegisterNo1 input:text").on("focus", function () {
                    var Label;
                    $("label#lblSubmit").attr("class", "text none");
                    $.each($(".mianDiv .RegisterNo1 input:text"), function (index, InControl) {
                        if ($.trim($(InControl).val()) == "") {
                            Label = $(InControl).parent().attr("class", "regBox").siblings("label").attr("class", "text"); ;
                            $(InControl).siblings("i").remove();
                            $(Label).text($(Label).attr("data-Default"));
                        }
                    });
                    Label = $(this).parent().attr("class", "regBox bgreen").siblings("label").attr("class", "text"); ;
                    $(this).siblings("i").remove();
                    $("label#lblSubmit").attr("class", "text none");
                    $(Label).text($(Label).attr("data-Default"));
                });

                $(".mianDiv .RegisterNo1 input:text").on("blur", function () {
                    if ($.trim($(this).val()) == "") {
                        return $(this).parent().attr("class", "regBox");
                    }
                    switch (this.id) {
                        case "txt_Phone":
                            if (!IsMobile($.trim($(this).val()))) {
                                CompRegister.InputtxtOkError(this, false, "手机格式不正确");
                            }
                            else {
                                CompRegister.AjaxCheck(this, CompRegister.DefaultUrl, { GetAction: "GetPhone", Value: $.trim($(this).val()), RegisterType: BindType, Compid: CompId }, function (ReturnData, Control) {
                                    CompRegister.InputtxtOkError(Control, ReturnData.Result, ReturnData.Msg);
                                    $(Control).data("IsPhone", ReturnData.Result);
                                    if ($(".mianDiv .RegisterNo1 #txt_PhoneCode").data("Code") == "ErrorPhone") {
                                        $(".mianDiv .RegisterNo1 #txt_PhoneCode").trigger("blur");
                                    }
                                })
                            }
                            ; break;
                        case "txt_CheckCode":
                            CompRegister.AjaxCheck(this, CompRegister.DefaultUrl, { GetAction: "ChckCode", Value: $.trim($(this).val()) }, function (ReturnData, Control) {
                                CompRegister.InputtxtOkError(Control, ReturnData.Result, ReturnData.Msg);
                            });
                            ; break;
                        case "txt_PhoneCode":
                            CompRegister.AjaxCheck(this, CompRegister.DefaultUrl, { GetAction: "CheckPhoneCode", Value: $.trim($(this).val()), Phone: $.trim($("#txt_Phone").val()) }, function (ReturnData, Control) {
                                CompRegister.InputtxtOkError(Control, ReturnData.Result, ReturnData.Msg);
                                $(Control).data("Code", ReturnData.Code);
                            });
                            ; break;
                    }

                });

                $(".mianDiv .RegisterNo1 #SendPhoneCode").on("click", function () {
                    if ($(this).data("IsSend") != true) {
                        $(this).data("IsSend", true);
                        var SendControl = this;
                        CompRegister.CheckCodeAndPhone(function () {
                            if ($(".mianDiv .RegisterNo1 #txt_Phone").data("IsPhone") != true) {
                                $(SendControl).data("IsSend", false);
                                return false;
                            }
                            CompRegister.AjaxCheck($(SendControl), "/Controller/AddUpDataSource.ashx", { GetAction: "SendPhoneCode", phone: $.trim($("#txt_Phone").val()) }, function (ReturnData, Control) {
                                if (ReturnData.Result) {
                                    SetEndTime(120, Control);
                                } else {
                                    $(SendControl).data("IsSend", false);
                                    $(Control).next("label").attr("class", "text tRed").text(ReturnData.Msg);
                                }
                            });
                        });
                    }
                });

                $(".mianDiv .RegisterNo1 #btn_Submit").on("click", function () {
                    var Control = this;
                    if (!$("#CK_MYAgment").prop("checked")) {
                        return;
                    }
                    $("label#lblSubmit").attr("class", "text none");
                    CompRegister.CheckNo1(function () {
                        CompRegister.No1Submit(Control);
                    })
                });

                $("#CK_MYAgment").on("click", function () {
                    if ($(this).prop("checked")) {
                        $(".mianDiv .RegisterNo1 #btn_Submit").removeClass("ebled");
                        $("#CK_MYAgment").parent().siblings("label").attr("class", "text none").text("");
                    }
                    else {
                        $(".mianDiv .RegisterNo1 #btn_Submit").addClass("ebled");
                    }
                })

            },
            No2BindEevent: function (UserIsRegister) {
                var BindStr = ".mianDiv .RegisterNo2 input:text,.mianDiv .RegisterNo2 input:password";
                $(BindStr).on("focus", function () {
                    var Label;
                    $("label#lblRegister").attr("class", "text none");
                    $.each($(BindStr), function (index, InControl) {
                        if ($.trim($(InControl).val()) == "") {
                            Label = $(InControl).parent().attr("class", "regBox").siblings("label").attr("class", "text"); ;
                            $(InControl).siblings("i").remove();
                            $(Label).text($(Label).attr("data-Default"));
                        }
                    });
                    Label = $(this).parent().attr("class", "regBox bgreen").siblings("label").attr("class", "text"); ;
                    $(this).siblings("i").remove();
                    $("label#lblRegister").attr("class", "text none");
                    $(Label).text($(Label).attr("data-Default"));
                });
                $(BindStr).on("blur", function () {
                    if ($.trim($(this).val()) == "") {
                        return $(this).parent().attr("class", "regBox");
                    }
                    switch (this.id) {
                        case "txt_CompName":
                            if ($.trim($(this).val()).length > 20 || $.trim($(this).val()).length < 2) {
                                CompRegister.InputtxtOkError(this, false, "厂商名称必须在2-20字符之间。");
                            }
                            else {
                                CompRegister.AjaxCheck(this, CompRegister.DefaultUrl, { GetAction: (BindType == "RegiComp" ? "GetComp" : "GetDis"), Value: $.trim($(this).val()), Compid: CompId }, function (ReturnData, Control) {
                                    CompRegister.InputtxtOkError(Control, ReturnData.Result, ReturnData.Msg);
                                })
                            }
                            ; break;
                        case "txt_Account":
                            if (UserIsRegister) {
                                break;
                            }
                            if ($.trim($(this).val()).length < 2 || $.trim($(this).val()).length > 20) {
                                CompRegister.InputtxtOkError(this, false, "登录帐号必须在2-20字符之间。");
                            }
                            else {
                                CompRegister.AjaxCheck(this, CompRegister.DefaultUrl, { GetAction: "Getuser", Value: $.trim($(this).val()) }, function (ReturnData, Control) {
                                    CompRegister.InputtxtOkError(Control, ReturnData.Result, ReturnData.Msg);
                                })
                            }

                            ; break;
                        case "txt_PassWord":
                            if (UserIsRegister) {
                                break;
                            }
                            if ($.trim($(this).val()).length < 6 || $.trim($(this).val()).length > 20) {
                                CompRegister.InputtxtOkError(this, false, "登陆密码必须在6-20字符之间。");
                            }
                            else {
                                if ($.trim($("#txt_CheckPassWord").val()) != "") {
                                    $("#txt_CheckPassWord").trigger("blur");
                                }
                                CompRegister.InputtxtOkError(this, true, "");
                            }
                            ; break;
                        case "txt_CheckPassWord":
                            if (UserIsRegister) {
                                break;
                            }
                            if ($.trim($(this).val()) != $.trim($("#txt_PassWord").val())) {
                                CompRegister.InputtxtOkError(this, false, "确认密码不一致，请重新输入。");
                            }
                            else {
                                CompRegister.InputtxtOkError(this, true, "");
                            }
                            ; break;

                        case "txt_Leading":
                            if (UserIsRegister) {
                                break;
                            }
                            if ($.trim($(this).val()) == "") {
                                CompRegister.InputtxtOkError(this, false, "法人姓名不能为空。");
                            }
                            else if ($.trim($(this).val()).length > 20 || $.trim($(this).val()).length < 2) {
                                CompRegister.InputtxtOkError(this, false, "法人姓名必须在2-20字符之间。");
                            } else {
                                CompRegister.InputtxtOkError(this, true, "");
                            }
                            ; break;
                        case "txt_Licence":
                            if ($.trim($(this).val()) == "") {
                                CompRegister.InputtxtOkError(this, false, "法人身份证不能为空。");
                               
                            }
                            else {
                                CompRegister.InputtxtOkError(this, true, "");
                            }

                            ; break;
                        case "txt_creditCode":
                            if ($.trim($(this).val()) == "") {
                                CompRegister.InputtxtOkError(this, false, "统一社会信用代码不能为空。");
                                
                            }
                            else {
                                CompRegister.InputtxtOkError(this, true, "");
                            }; break;
                        case "txt_TrueName":
                            if ($.trim($(this).val()) == "") {
                                CompRegister.InputtxtOkError(this, false, "联系人姓名不能为空。");
                            }
                            else {
                                CompRegister.InputtxtOkError(this, true, "");
                            }
                    }

                });
                UserIsRegister && BindType != "RegiComp" && $(".mianDiv .RegisterNo2  #txt_CompName").trigger("blur");
                $(".mianDiv .RegisterNo2 #btnRegister").on("click", function () {
                    var Control = this;
                    if ($(Control).data("enble") == true) {
                        return;
                    }
                    $(Control).addClass("ebled").data("enble", true);
                    $("label#lblRegister").attr("class", "text none");
                    CompRegister.CheckNo2(function () {
                        CompRegister.No2Submit(Control, UserIsRegister);
                    }, UserIsRegister)
                });

            },
            No1Submit: function (TControl) {
                var Data = {};
                Data["Data"] = new Array();
                $.each($(".mianDiv .RegisterNo1 input:text"), function (index, Contorl) {
                    Data["Data"].push({ ContorlId: Contorl.id, Value: $.trim(Contorl.value) });
                });
                CompRegister.AjaxCheck(TControl, CompRegister.DefaultUrl, { GetAction: "SubmitCheckNo1", Value: JSON.stringify(Data), RegisterType: BindType, Compid: CompId }, function (ReturnData, Control) {
                    if (ReturnData.Code) {
                        try {
                            var JsonArry = eval('(' + ReturnData.Code + ')')
                            for (var Item in JsonArry) {
                                var Jdata = JsonArry[Item];
                                CompRegister.InputtxtOkError($(".mianDiv .RegisterNo1 #" + Jdata.Attr1 + "").data("Code", Jdata.Code), Jdata.Result, Jdata.Msg);
                            }
                        }
                        catch (e) {
                            CompRegister.InputtxtOkError(TControl, ReturnData.Result, ReturnData.Msg);
                        }
                    }
                    ReturnData.Result ? function () {
                        if (ReturnData.IsRegis) {
                            $(".mianDiv .RegisterNo2 #txt_CompName").val(ReturnData.CompName);
                            $(".mianDiv .RegisterNo2 #txt_Account").val(ReturnData.Name);
                            $(".mianDiv .RegisterNo2 #txt_PassWord").val("123456");
                            $(".mianDiv .RegisterNo2 #txt_CheckPassWord").val("123456");
                        }
                        $(".mianDiv .RegisterNo1").fadeOut(800, function () {
                            $(".mianDiv .RegisterNo2").fadeIn(500);
                            CompRegister.No2BindEevent(ReturnData.IsRegis);
                        });
                    } () : function () {
                        if (ReturnData.Error) {
                            layerCommon.msg(ReturnData.Msg, IconOption.错误, 2000);
                        }
                    } ();
                });
            },
            No2Submit: function (TControl, UserIsRegister) {

                //验证非空





                var Data = {};
                Data["Data"] = new Array();
                Data["Data"].push({ ContorlId: "txt_Phone", Value: $.trim($("#txt_Phone").val()) });
                //Data["Data"].push({ ContorlId: "txt_Account", Value: $.trim($("#txt_Account").val()) });
                //Data["Data"].push({ ContorlId: "txt_PassWord", Value: $.trim($("#txt_PassWord").val()) });
                //Data["Data"].push({ ContorlId: "txt_creditCode", Value: $.trim($("#txt_creditCode").val()) });
                //Data["Data"].push({ ContorlId: "txt_Licence", Value: $.trim($("#txt_Licence").val()) });
                //Data["Data"].push({ ContorlId: "txt_Leading", Value: $.trim($("#txt_Leading").val()) });
                //Data["Data"].push({ ContorlId: "txt_CheckPassWord", Value: $.trim($("#txt_CheckPassWord").val()) });
                $.each($((".mianDiv .RegisterNo2 input:text,.mianDiv .RegisterNo2 input:password")), function (index, Contorl) {
                    Data["Data"].push({ ContorlId: Contorl.id, Value: $.trim(Contorl.value) });
                });
                CompRegister.AjaxCheck(TControl, CompRegister.DefaultUrl, { GetAction: "SubmitCheckNo2", Value: JSON.stringify(Data), RegisterType: BindType, Compid: CompId, FileValue: $.trim($("#HidFfileName").val()) }, function (ReturnData, Control) {
                    try {
                        var JsonArry = eval('(' + ReturnData.Code + ')')
                        for (var Item in JsonArry) {
                            var Jdata = JsonArry[Item];
                            CompRegister.InputtxtOkError($("#" + Jdata.Attr1 + "", ".RegisterNo2,.RegisterNo1"), Jdata.Result, Jdata.Msg);
                        }
                        if (ReturnData.RegisterOrder == "RegisterNo1") {
                            layerCommon.msg(ReturnData.Msg, IconOption.错误, 2000);
                            $(".mianDiv .RegisterNo2").fadeOut(800, function () {
                                $("#txt_PhoneCode").val("");
                                $(".mianDiv .RegisterNo1").fadeIn(500);
                            });
                            return;
                        }
                    }
                    catch (e) {
                        CompRegister.InputtxtOkError(TControl, ReturnData.Result, ReturnData.Msg);
                    }
                    if (ReturnData.Result) {
                        $(".mianDiv .RegisterNo2").fadeOut(800, function () {
                            (ReturnData.Href == "RegiDis" && $(".w1", ".mianDiv .RegisterNo3").html("加盟申请提交成功，请等待审核")) || (ReturnData.Href == "RegiOK" && $(".w1", ".mianDiv .RegisterNo3").html("注册成功,请登录后操作"));
                            $(".mianDiv .RegisterNo3").fadeIn(500, function () {
                                Redirct(CompId, ReturnData.Href);
                            });
                        });
                    } else {
                        $(TControl).removeClass("ebled").data("enble", false);
                    }
                });
            },
            InputtxtOkError: function (TXTControl, IsCheck, Messge) {
                if (IsCheck) {
                    $(TXTControl).parent().append("<i class=\"okIcon\"></i>");
                    var Label = $(TXTControl).parent().attr("class", "regBox").siblings("label").attr("class", "text");
                    $(Label).html($(Label).attr("data-Default"));
                } else {
                    if ($(TXTControl).parent().siblings("label").length > 0) {
                        $(TXTControl).parent().append("<i class=\"ErIcon\"></i>");
                        var Label = $(TXTControl).parent().attr("class", "regBox bRed").siblings("label").attr("class", "text tRed");
                        $(Label).html(Messge);
                    } else {
                        $(TXTControl).siblings("label").attr("class", "text tRed").html(Messge);
                    }
                }
            },
            DefaultUrl: "/Handler/RegisterCheck.ashx",
            //公用ajax验证
            AjaxCheck: function (Control, Url, Data, CallBack, Async) {
                $.ajax({
                    type: 'POST',
                    url: Url,
                    data: Data,
                    dataType: "json",
                    timeout: 5000,
                    cache: false,
                    success: function (ReturnData) {
                        if (!ReturnData.Error) {
                            if (typeof CallBack == "function") {
                                CallBack(ReturnData, Control);
                            }
                        } else {
                            if (Control != undefined) {
                                if ($(Control).parent().siblings("label").length > 0) {
                                    $(Control).parent().append("<i class=\"ErIcon\"></i>");
                                    $(Control).parent().attr("class", "regBox bRed").siblings("label").attr("class", "text tRed").html(ReturnData.Msg);
                                }
                                else {
                                    $(Control).siblings("label").attr("class", "text tRed").html(ReturnData.Msg);
                                }
                            }
                            Control.id == "SendPhoneCode" && $(Control).data("IsSend", false);
                            Control.id == "btnRegister" && $(Control).removeClass("ebled").data("enble", false);
                        }
                    }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                        Control.id == "SendPhoneCode" && $(Control).data("IsSend", false);
                        Control.id == "btnRegister" && $(Control).removeClass("ebled").data("enble", false);
                        if (XMLHttpRequest.statusText == "timeout") {
                            if (Control != undefined) {
                                if ($(Control).parent().siblings("label").length > 0) {
                                    $(Control).parent().siblings("label").attr("class", "text tRed").text("请求超时,请稍候重试");
                                }
                                else {
                                    $(Control).siblings("label").attr("class", "text tRed").text("请求超时,请稍候重试");
                                }
                            }
                        } else {
                            if (Control != undefined) {
                                if ($(Control).parent().siblings("label").length > 0) {
                                    $(Control).parent().siblings("label").attr("class", "text tRed").text("服务器或网络异常,请稍候重试");
                                }
                                else {
                                    $(Control).siblings("label").attr("class", "text tRed").text("服务器或网络异常,请稍候重试");
                                }
                            }
                        }
                    }
                });
            },
            //校验手机和图文验证码 
            CheckCodeAndPhone: function (CallBack) {
                if (!IsMobile($.trim($("#txt_Phone").val())) || $.trim($("#txt_CheckCode").val()) == "") {
                    $(".mianDiv .RegisterNo1 #SendPhoneCode").data("IsSend", false);
                    if ($.trim($("#txt_Phone").val()) == "") {
                        CompRegister.InputtxtOkError($("#txt_Phone"), false, "手机号码不能为空");
                    } else if (!IsMobile($.trim($("#txt_Phone").val()))) {
                        CompRegister.InputtxtOkError($("#txt_Phone"), false, "手机号码格式错误");
                    }
                    if ($.trim($("#txt_CheckCode").val()) == "") {
                        CompRegister.InputtxtOkError($("#txt_CheckCode"), false, "验证码不能为空");
                        return;
                    }
                }
                else {
                    CallBack();
                    //                    CompRegister.AjaxCheck($("#txt_Phone"), CompRegister.DefaultUrl, { Action: "GetPhone", Value: $.trim($("#txt_Phone").val()) }, function (ReturnData, Control) {
                    //                        CompRegister.InputtxtOkError(Control, ReturnData.Result, ReturnData.Msg);
                    //                        if (ReturnData.Result) {
                    //                            if ($.trim($("#txt_CheckCode").val()) == "") {
                    //                                CompRegister.InputtxtOkError($("#txt_CheckCode"), false, "图文验证码不能为空");
                    //                            }
                    //                            else {
                    //                                CompRegister.AjaxCheck($("#txt_CheckCode"), CompRegister.DefaultUrl, { Action: "ChckCode", Value: $.trim($("#txt_CheckCode").val()) }, function (ReturnData, Control) {
                    //                                    CompRegister.InputtxtOkError(Control, ReturnData.Result, ReturnData.Msg);
                    //                                    if (ReturnData.Result) {
                    //                                        CallBack();
                    //                                    }
                    //                                })
                    //                            }
                    //                        }
                    //                    })
                }
            }
            //校验手机和图文验证码 end
            , CheckNo1: function (CallBack) {
                var validator = true;
                $.each($(".mianDiv .RegisterNo1 input:text"), function (index, Contorl) {
                    switch (Contorl.id) {
                        case "txt_Phone":
                            if (!IsMobile($.trim($(Contorl).val()))) {
                                if ($.trim($(Contorl).val()) != "") {
                                    CompRegister.InputtxtOkError($(Contorl), false, "手机号码格式错误");
                                } else {
                                    CompRegister.InputtxtOkError($(Contorl), false, "手机号码不能为空");
                                }
                                validator = false;
                            }
                            ; break;
                        case "txt_CheckCode":
                            if ($.trim($(Contorl).val()) == "") {
                                CompRegister.InputtxtOkError($(Contorl), false, "图文验证码不能为空");
                                validator = false;
                            }
                            ; break;
                        case "txt_PhoneCode":
                            if ($.trim($(Contorl).val()) == "") {
                                CompRegister.InputtxtOkError($(Contorl), false, "手机验证码不能为空");
                                validator = false;
                            }
                            ; break;
                    }
                });
                if (!$("#CK_MYAgment").prop("checked")) {
                    $("#CK_MYAgment").parent().siblings("label").attr("class", "text tRed").text("请阅读并勾选接受医站通平台用户服务协议");
                    validator = false;
                }
                if (validator) {
                    CallBack();
                };
            }
            //注册验证第二步
            , CheckNo2: function (CallBack, UserIsRegister) {
                var validator = true;
                $.each($((".mianDiv .RegisterNo2 input:text,.mianDiv .RegisterNo2 input:password")), function (index, Contorl) {
                    switch (this.id) {
                        case "txt_CompName":
                            if ($.trim($(this).val()) == "") {
                                CompRegister.InputtxtOkError(this, false, "厂商名称不能为空。");
                                validator = false;
                            }
                            else if ($.trim($(this).val()).length > 20 || $.trim($(this).val()).length < 2) {
                                CompRegister.InputtxtOkError(this, false, "厂商名称必须在2-20字符之间。");
                                validator = false;
                            }
                            ; break;
                        case "txt_Leading":
                            if ($.trim($(this).val()) == "") {
                                CompRegister.InputtxtOkError(this, false, "法人姓名不能为空。");
                                validator = false;
                            }
                            else if ($.trim($(this).val()).length > 20 || $.trim($(this).val()).length < 2) {
                                CompRegister.InputtxtOkError(this, false, "法人姓名必须在2-20字符之间。");
                                validator = false;
                            }
                            ; break;
                        case "txt_Licence":
                            if ($.trim($(this).val()) == "") {
                                CompRegister.InputtxtOkError(this, false, "法人身份证不能为空。");
                                validator = false;
                            }
                            ; break;
                        case "txt_creditCode":
                            if ($.trim($(this).val()) == "") {
                                CompRegister.InputtxtOkError(this, false, "统一社会信用代码不能为空。");
                                validator = false;
                            }
                            ; break;
                        case "txt_TrueName":
                            if ($.trim($(this).val()) == "") {
                                CompRegister.InputtxtOkError(this, false, "联系人姓名不能为空。");
                                validator = false;
                            }
                            ; break;
                        case "txt_Account":
                            if (UserIsRegister) { break; }
                            if ($.trim($(this).val()) == "") {
                                CompRegister.InputtxtOkError(this, false, "登录帐号不能为空。");
                                validator = false;
                            }
                            else if ($.trim($(this).val()).length < 2 || $.trim($(this).val()).length > 20) {
                                CompRegister.InputtxtOkError(this, false, "登录帐号必须在2-20字符之间。");
                                validator = false;
                            }
                            ; break;
                        case "txt_PassWord":
                            if (UserIsRegister) { break; }
                            if ($.trim($(this).val()) == "") {
                                CompRegister.InputtxtOkError(this, false, "登陆密码不能为空。");
                                validator = false;
                            }
                            else if ($.trim($(this).val()).length < 6 || $.trim($(this).val()).length > 20) {
                                CompRegister.InputtxtOkError(this, false, "登陆密码必须在6-20字符之间。");
                                validator = false;
                            }
                            ; break;
                        case "txt_CheckPassWord":
                            if (UserIsRegister) { break; }
                            if ($.trim($(this).val()) == "") {
                                CompRegister.InputtxtOkError(this, false, "确认密码不能为空。");
                                validator = false;
                            }
                            else if ($.trim($(this).val()) != $.trim($("#txt_PassWord").val())) {
                                CompRegister.InputtxtOkError(this, false, "确认密码不一致，请重新输入。");
                                validator = false;
                            }
                            ; break;
                    }
                });
                if (validator) {
                    CallBack();
                } else {
                    $(".mianDiv .RegisterNo2 #btnRegister").removeClass("ebled").data("enble", false);
                }
            }

        }

        //代理商注册
        var DisRegister = {

    }
    var EndTime = 0;
    //发送验证码倒计时
    function SetEndTime(time, Control) {
        if (time != undefined && time != null) {
            $(Control).data("IsSend", true).addClass("btns");
            EndTime = time;
            $(Control).next("label").attr("class", "text").text($(Control).next("label").attr("data-Default"));
        }
        if (EndTime > 0) {
            $(Control).text("" + EndTime + "s后重新获取");
            EndTime--;
            setTimeout(function () { SetEndTime(null, Control) }, 1000);
        } else {
            $(Control).data("IsSend", false).removeClass("btns").text("获取验证码");
        }
    }

    var RedirctSecend = 3;

    function Redirct(CompId, Result) {
        if (CompId == "") {
            $(".mianDiv .RegisterNo3 .regOk .w2").html("页面将在" + RedirctSecend + "秒之后跳转至首页<a href=\"index.aspx\">立即跳转</a>");
        } else {
            (Result == "RegiOK" && $(".mianDiv .RegisterNo3 .regOk .w2").html("页面将在" + RedirctSecend + "秒之后跳转到登录页<a href=\"/login.html\">立即跳转</a>")) || $(".mianDiv .RegisterNo3 .regOk .w2").html("页面将在" + RedirctSecend + "秒之后跳转到e店铺<a href=\"/" + CompId + ".html\">立即跳转</a>");
        }
        if (RedirctSecend > 0) {
            RedirctSecend--;
            setTimeout(function () { Redirct(CompId, Result); }, 1000);
        } else {
            if (CompId == "") {
                window.location.href = "/index.html";
            } else {
                (Result == "RegiOK" && (window.location.href = "/login.html")) || (window.location.href = "/" + CompId + ".html");
            }
        }
    }

    CompRegister.OnInit();

}
    });
})(jQuery);