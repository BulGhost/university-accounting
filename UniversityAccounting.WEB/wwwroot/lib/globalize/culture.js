$.validator.methods.number = function (value, element) {
    return this.optional(element) || !isNaN(Globalize.parseFloat(value));
}
$(document).ready(function () {
    Globalize.culture('@System.Globalization.CultureInfo.CurrentCulture.Name');
});