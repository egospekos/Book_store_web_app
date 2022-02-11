var edit_select_author;
var E_AuthorSelectize;
//
var edit_select_category;
var E_CategorySelectize;
var oldCategories;
$(function () {
    //author
    E_CreateAuthorSelectize();
    E_AuthorSelectize = edit_select_author[0].selectize;
    E_LoadAuthorSelectize();
    //category
    E_CreateCategorySelectize();
    E_CategorySelectize = edit_select_category[0].selectize;
    E_LoadCategorySelectize();
});

//author
function E_CreateAuthorSelectize() {
    edit_select_author = $("#E_authorSelect").selectize({
        create: false,
        valueField: 'authorID',
        sortField: 'authorName',
        labelField: "authorName",
        searchField: "authorName",

    });
}

function E_LoadAuthorSelectize() {
    obj = {};
    $.ajax({
        url: "/Books/getAuthors",
        type: "GET",
        data: obj
    }).done(function (data) {

        for (var i = 0; i < data.length; i++) {
            E_AuthorSelectize.addOption(data[i]);
        }
        E_AuthorSelectize.refreshOptions();
    });
}

// category

function E_CreateCategorySelectize() {
    edit_select_category = $("#E_categorySelect").selectize({
        create: false,
        valueField: 'categoryID',
        sortField: 'categoryName',
        labelField: "categoryName",
        searchField: "categoryName",
        maxItems: 3,
    });
}

function E_LoadCategorySelectize() {
    obj = {};
    $.ajax({
        url: "/Books/getCategories",
        type: "GET",
        data: obj
    }).done(function (data) {

        for (var i = 0; i < data.length; i++) {
            E_CategorySelectize.addOption(data[i]);
        }
        E_CategorySelectize.refreshOptions();
    });
}

function partialEditButton() {
    obj = {};
    obj.bookID = $("#edit_getID").val();
    obj.bookName = $("#edit_getName").val();
    obj.bookPages = $("#edit_getPages").val();
    obj.bookPrice = $("#edit_getPrice").val();
    obj.authorID = $("#E_authorSelect").val();

    if (obj.bookName == "" || obj.bookPages == "" || obj.bookPrice == "" || obj.authorID == "") {
        alert("Name, Pages, Price, Author can't be empty");
    }
    else {
        obj.categoryIDs = $("#E_categorySelect").val();

        if (obj.categoryIDs == "") obj.categoryIDs = oldCategories;

        $.ajax({
            url: "/Books/Edit",
            type: "POST",
            data: obj
        }).done(function (data) {
            editM.style.display = "none";
            bookDataTable.ajax.reload();
        });
    }

}