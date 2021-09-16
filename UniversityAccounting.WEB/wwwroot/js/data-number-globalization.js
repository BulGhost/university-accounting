$.when(
    $.getJSON("/lib/cldr-data/cldr-core/supplemental/likelySubtags.json"),
    $.getJSON("/lib/cldr-data/cldr-core/supplemental/numberingSystems.json"),
    $.getJSON("/lib/cldr-data/cldr-core/supplemental/plurals.json"),
    $.getJSON("/lib/cldr-data/cldr-core/supplemental/ordinals.json"),
    $.getJSON("/lib/cldr-data/cldr-core/supplemental/currencyData.json"),
    $.getJSON("/lib/cldr-data/cldr-core/supplemental/timeData.json"),
    $.getJSON("/lib/cldr-data/cldr-core/supplemental/weekData.json"),
    $.getJSON(`/lib/cldr-data/cldr-dates-modern/main/${locale}/ca-gregorian.json`),
    $.getJSON(`/lib/cldr-data/cldr-dates-modern/main/${locale}/timeZoneNames.json`),
    $.getJSON(`/lib/cldr-data/cldr-numbers-modern/main/${locale}/numbers.json`),
    $.getJSON(`/lib/cldr-data/cldr-numbers-modern/main/${locale}/currencies.json`),
).then(function () {
    return [].slice.apply(arguments, [0]).map(function (result) {
        return result[0];
    });
}).then(Globalize.load).then(function () {
    Globalize.locale(locale);
});