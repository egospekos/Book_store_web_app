function authorAddClick() {
    var obj = {};
    obj.authorName = $("#authorName").val();


    if (obj.authorName == "") {

        alert("Name must be filled.");
    }
    else {
        obj.authorBday = $("#authorBday").val();

        $.ajax({
            url: "/Authors/Create",
            type: "POST",
            data: obj
        }).done(function (data) {
            addM.style.display = 'none';
            authorDataTable.ajax.reload();
        });
    }

}