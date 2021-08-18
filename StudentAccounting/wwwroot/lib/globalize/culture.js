$.validator.methods.number = function (value, element) {
    return this.optional(element) || !isNaN(Globalize.parseFloat(value));
}
$(document).ready(function () {
    Globalize.culture('@System.Globalization.CultureInfo.CurrentCulture.Name');
});

jQuery.extend(jQuery.validator.methods, {
    range: function (value, element, param) {
        var val = $.global.parseFloat(value);
        return this.optional(element) || (
            val >= param[0] && val <= param[1]);
    }
});