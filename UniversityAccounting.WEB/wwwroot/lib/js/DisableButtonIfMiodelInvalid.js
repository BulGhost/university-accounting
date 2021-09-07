$('input[type="submit"]').prop('disabled', true);
$('form input').on('blur keyup', function () {
    if ($('form').valid()) {
        $('input[type="submit"]').prop('disabled', false);
    } else {
        $('input[type="submit"]').prop('disabled', true);
    }
});