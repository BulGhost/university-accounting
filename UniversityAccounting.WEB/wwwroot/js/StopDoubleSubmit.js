function DisableButton() {
    $('input[type="submit"]').prop('disabled', true);
}
window.onbeforeunload = DisableButton;