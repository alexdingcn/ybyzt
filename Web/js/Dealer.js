function update(id) {
    $(".li1").slideDown(500);
    $.ajax({
        url: "../Controller/DisDelivery.ashx?type=update",
        data: { updateid: id },
        success: function (result) {
            updatecon(result);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("服务器异常，请稍后再试");
        }
    });
}

function updatecon(result) {
    var update = eval('(' + result + ')');
    $("#txtdlename").val($.trim(update["Name"]));
    $("#txtusername").val($.trim(update["Principal"]));
    $("#txtuserphone").val($.trim(update["Phone"]));
    $("#txtaddress").val($.trim(update["Address"]));
    $("#hidID").val(update["ID"]);
}