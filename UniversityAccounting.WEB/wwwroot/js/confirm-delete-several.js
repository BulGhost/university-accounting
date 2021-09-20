document.getElementById("deleteButton").onclick = function () {
    var checkboxes = $('input[name="ids"]:checked').length;
    document.getElementById("message").innerHTML = message + checkboxes;
    $('#deleteDialog').modal('toggle');
};