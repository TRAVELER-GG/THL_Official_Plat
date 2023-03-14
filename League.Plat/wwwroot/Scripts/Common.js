/**
* Common.js 1.0.0
* Copyright (c) 2012-2013 Richstar. All rights reserved.
* Author:Bill Shi
*/

$(function () {
    $.ajaxSetup({ cache: false });
    FixRightDivHeight();
});

$(window).resize(function () {
    FixRightDivHeight();
});

function ReloadPage() {
    location.reload();
}

function JumpPage(URL) {
    window.location.href = URL;
}

function Hide_Textarea_Success() {
    $(".Textarea_Success").hide(200);
}

function ActLeftMenu(IdName) {
    $("#LeftMenu_" + IdName).css("color", "#CE0000");
    $("#LeftMenu_" + IdName).css("font-size", "16px");
}


function TopMenuToAction(TopMenuID) {
    $("#MTB_" + TopMenuID).addClass("TopMenuAction");
}

function TopMenuSubToAction(IdName) {
    $("#TopSub_" + IdName).addClass("TopMenuNavSub_Sim-act");
}

function FixHeightValue() {
    //获取浏览器当前高度
    var ALLHeight = $("#RightDiv").outerHeight(true);
    //获取页面扣除DOM高度 - 重新获得列表框自适应高度
    var DeductionValue = 0;

    //扣除顶部菜单高度
    $('.Sub_Top_Menu').each(function (i) {
        DeductionValue += $(this).outerHeight(true);
    });

    //扣除搜索表格高度
    $('.Search_Table').each(function (i) {
        DeductionValue += $(this).outerHeight(true);
    });

    //扣除底部分页组件高度
    $('.MyPage_DIV').each(function (i) {
        DeductionValue += $(this).outerHeight(true);
    });

    //扣除自定义高度
    $('.DeductionDiv').each(function (i) {
        DeductionValue += $(this).outerHeight(true);
    });

    var Fix_Height = 90;
    return ALLHeight - DeductionValue - Fix_Height;
}

function FixRightDivHeight() {
    //获取浏览器当前高度
    var WinHeight = $(window).height();
    //估算值
    var NewHeight = WinHeight;
    $('#RightDiv').css('height', NewHeight);
    var RightDivContext_Height = FixHeightValue() + 20;
    $('#RightDiv_Context').css('height', RightDivContext_Height);
}

//表单验证按Class验证
function validateFormByClass(ClsName) {
    var AllSet = 0;

    $('.' + ClsName).each(function (i) {
        if ($(this).val() == "") {
            $(this).css("backgroundColor", "#D2E9FF");
            AllSet = AllSet + 1;
            obj = $(this);
        } else {
            $(this).css("backgroundColor", "#FFF");
        }
    });

    if (AllSet > 0) {
        $(obj).focus();
        return false;
    } else {
        return true;
    }
}

function validateFormByRequired(Form_ID) {
    var AllSet = 0;
    $('#' + Form_ID + ' [required]').each(function (i) {
        if ($(this).val() === "") {
            $(this).css("backgroundColor", "#D2E9FF");
            AllSet = AllSet + 1;
            obj = $(this);
        } else {
            $(this).css("backgroundColor", "#FFF");
        }
    });


    if (AllSet > 0) {
        $(obj).focus();
        return false;
    } else {
        return true;
    }
}


//获取复选框当前所选数
function checkBoxCheckLength(classStr) {
    var Loop = 0;
    Loop = $("." + classStr + ":checked").length;
    return Loop;
}

//获取全选操作
function checkBoxCheckAll(checkID, classStr) {
    var isCheck = document.getElementById(checkID).checked;
    if (isCheck === true) {
        $('.' + classStr).each(function (i) {
            $(this).prop({ checked: true });
        });
    } else {
        $('.' + classStr).each(function (i) {
            $(this).prop({ checked: false });
        });
    }
}

function DisAndEnabledByID(ID, Val) {
    if (Val == 1) {
        $('#' + ID).attr("disabled", true);
    } else {
        $('#' + ID).attr("disabled", false);
    }
}

function DisAndEnabledByClass(CLS, Val) {
    if (Val == 1) {
        $('.' + CLS).attr("disabled", true);
    } else {
        $('.' + CLS).attr("disabled", false);
    }
}

function DisAndEnabledBtn(Val) {
    if (Val == 1) {
        $(':button').attr("disabled", true);
    } else {
        $(':button').attr("disabled", false);
    }
}

function Ajax_Data_Post_Callback(Url, Post_Data, Btn_Obj, Success_Fun) {
    $.ajax({
        url: Url,
        type: 'POST',
        data: Post_Data,
        beforeSend: function () {
            $(Btn_Obj).attr("disabled", true);
        },
        error: function (XMLHttpRequest) {
            alert(XMLHttpRequest.responseText + "\n------------------------------------\n" + "Code：" + XMLHttpRequest.status + "\n" + Url);
            $(Btn_Obj).attr("disabled", false);
        },
        success: function (result) {
            $(Btn_Obj).attr("disabled", false);
            Success_Fun(result);
        }
    });
}

function Ajax_Form_Reload(Form_ID, Btn_Obj) {
    Ajax_Form_Post(Form_ID, Btn_Obj, "Reload");
}

function Ajax_Form_Callback(Form_ID, Btn_Obj, Success_Fun) {
    Ajax_Form_Post(Form_ID, Btn_Obj, "Callback", Success_Fun);
}

function Ajax_Form_Post(Form_ID, Btn_Obj, Back_Type, Success_Fun) {
    var Form_Obj = $("#" + Form_ID);
    var Post_Data = $(Form_Obj).serialize();
    var Url = $(Form_Obj).prop("action");

    var Req_List = $(Form_Obj).find("[required]");
    var Req_List_Check = 0;
    $(Req_List).each(function (i) {
        if ($(this).val() === "") {
            $(this).css("backgroundColor", "#D2E9FF");
            $(this).focus();
            Req_List_Check = Req_List_Check + 1;
        } else {
            $(this).css("backgroundColor", "#FFF");
        }
    });
    if (Req_List_Check > 0) { return false; }


    $.ajax({
        url: Url,
        type: 'POST',
        data: Post_Data,
        beforeSend: function () {
            $(Btn_Obj).attr("disabled", true);
        },
        error: function (XMLHttpRequest) {
            alert(XMLHttpRequest.responseText + "\n------------------------------------\n" + "Code：" + XMLHttpRequest.status + "\n" + Url);
            $(Btn_Obj).attr("disabled", false);
        },
        success: function (result) {
            if (Back_Type === "Reload") {
                location.reload();
            } else if (Back_Type === "Callback") {
                Success_Fun(Btn_Obj, result);
            } else {
                $(Btn_Obj).attr("disabled", false);
            }
        }
    });
}

//文件上传
function Ajax_Form_Upload(Form_ID, Btn_Obj, Success_Fun) {
    var Form_Obj = $("#" + Form_ID);
    var Post_Data = new FormData($(Form_Obj)[0]);
    var Url = $(Form_Obj).prop("action");

    $.ajax({
        url: Url,
        type: 'POST',
        cache: false,
        data: Post_Data,
        processData: false,
        contentType: false,
        beforeSend: function () {
            $(Btn_Obj).attr("disabled", true);
        },
        error: function (XMLHttpRequest) {
            $(Btn_Obj).attr("disabled", false);
            alert(XMLHttpRequest.responseText + "\n------------------------------------\n" + "Code：" + XMLHttpRequest.status + "\n" + Url);
        },
        success: function (data) {
            $(Btn_Obj).attr("disabled", false);
            Success_Fun();
        }
    });
}

function Change_Top_Menu(Top_Name) {
    $("#Sub_Top_" + Top_Name).css("background-color", "#FFFFFF");
    $("#Sub_Top_" + Top_Name).css("border-top", "2px solid #ff7d00");
}

function Show_Help_Info(Cont, Act)
{
    $(".TopMenu_Help_DIV").toggle(200);
    $("#TopMenu_Help_DIV_Context").text("Loading...");
    $("#TopMenu_Help_DIV_Context").load("/Help/Context?Cont=" + Cont + "&Act=" + Act);
}

