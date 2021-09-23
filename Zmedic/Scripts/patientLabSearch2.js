$(function () {
    $("#sixIdInput").keyup(function () {
        var btnSubmit = $("#btnSubmit");
        if ($(this).val().trim() != "") {
            btnSubmit.removeAttr("disabled");
        } else {
            btnSubmit.attr("disabled", "disabled");
        }
    });
});