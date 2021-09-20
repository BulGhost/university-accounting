$("form :input").change(function () {
    $(this).closest('form').data('changed', true);
});
$('#backButton').click(function () {
    if ($(this).closest('form').data('changed')) {
        $('#backDialog').modal('toggle');
    } else {
        window.history.back();
    }
});