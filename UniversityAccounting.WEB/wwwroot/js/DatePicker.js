$(function () {
    $("#datepicker").datepicker({
        format: "dd.mm.yyyy",
        weekStart: 1,
        maxViewMode: 2,
        todayBtn: true,
        orientation: "bottom auto",
        autoclose: true,
        todayHighlight: true
    });
});