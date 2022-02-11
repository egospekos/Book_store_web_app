function addButtonClick() {
    var obj = {};
    obj.categoryName = $("#categoryName").val();
    if (obj.categoryName == "") {
        alert("Name must be filled.");
    }
    else {
        $.ajax({
            url: "/Categories/Create",
            type: "POST",
            data: obj
        }).done(function (data) {

            addM.style.display = "none";
            categoryDataTable.ajax.reload();
        });
    }


}