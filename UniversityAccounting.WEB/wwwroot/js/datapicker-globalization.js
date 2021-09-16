$(function () {
    $("#datepicker").datepicker({
        maxViewMode: 2,
        todayBtn: true,
        orientation: "bottom auto",
        autoclose: true,
        todayHighlight: true,
        weekStart: weekStart,
        language: lang
    });
});