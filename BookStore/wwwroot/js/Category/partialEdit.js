function editButtonClick() {
    var obj = {};
    obj.CategoryName = $("#getName").val();
    if (obj.CategoryName == "") {
        alert("Name must be filled.");
    }
    else {
        obj.CategoryID = $("#getID").val();
        $.ajax({
            url: "/Categories/Edit",
            type: "POST",
            data: obj
        }).done(function (data) {
            editM.style.display = "none";
            categoryDataTable.ajax.reload();
        });
    }

}