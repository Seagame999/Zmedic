$(document).ready(function () {
    $("#btnSubmit").click(function () {
        // add spinner to button
        $(this).html(
            `<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> กรุณารอสักครู่...`
        );
    });
});