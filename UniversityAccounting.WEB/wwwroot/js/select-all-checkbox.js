$(function () {
    $("#selectAll").click(function () {
        $("input[type=checkbox]").prop('checked', $(this).prop('checked'));
    });

    $("input[type=checkbox]").click(function () {
        if (!$(this).prop("checked")) {
            $("#selectAll").prop("checked", false);
        }
    });
})