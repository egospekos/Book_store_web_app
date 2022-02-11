function editButtonClick() {
    var obj = {};
    obj.authorName = $("#getName").val();

    if (obj.authorName == "") {
        alert("Name must be filled.");
    }
    else {
        obj.authorID = $("#getID").val();
        obj.authorBday = $("#getBday").val();
        if (obj.authorBday == "") obj.authorBday = $("#oldBday").val();
        $.ajax({
            url: "/Authors/Edit",
            type: "POST",
            data: obj
        }).done(function (data) {
            editM.style.display = 'none';
            authorDataTable.ajax.reload();
        });
    }

}