var select_author;
var create_select_category;
$(function () {
    CreateAuthorSelectize();
    _createAuthorSelectize = select_author[0].selectize;
    LoadAuthorSelectize();
    Part_Create_Cat_Select();
    _createCategorySelectize = create_select_category[0].selectize;
    Part_Load_Cat_Select();
});
function CreateAuthorSelectize() {
    select_author = $("#authorSelect").selectize({
        create: false,
        valueField: 'authorID',
        sortField: 'authorID',
        labelField: "authorName",
        searchField: "authorName",

    });
}

function LoadAuthorSelectize() {
    obj = {};
    $.ajax({
        url: "/Books/getAuthors",
        type: "GET",
        data: obj
    }).done(function (data) {

        for (var i = 0; i < data.length; i++) {
            _createAuthorSelectize.addOption(data[i]);
        }
        _createAuthorSelectize.refreshOptions();
    });
}

function Part_Create_Cat_Select() {
    create_select_category = $("#partialCategorySelect").selectize({
        create: false,
        valueField: 'categoryID',
        sortField: 'categoryID',
        labelField: "categoryName",
        searchField: "categoryName",
        maxItems: 3,
    });
}

function Part_Load_Cat_Select() {
    obj = {};
    $.ajax({
        url: "/Books/getCategories",
        type: "GET",
        data: obj
    }).done(function (data) {

        for (var i = 0; i < data.length; i++) {
            _createCategorySelectize.addOption(data[i]);
        }
        _createCategorySelectize.refreshOptions();
    });
}

function AddBook() {
    obj = {};
    obj.bookName = $("#getName").val();
    obj.bookPages = $("#getPages").val();
    obj.bookPrice = $("#getPrice").val();
    obj.authorID = $("#authorSelect").val();

    if (obj.bookName == "" || obj.bookPages == "" || obj.bookPrice == "" || obj.authorID == "") {
        alert("Name, Pages, Price, Author can't be empty");
    }
    else {
        obj.categoryIDs = $("#partialCategorySelect").val();
        $.ajax({
            url: "/Books/Create",
            type: "POST",
            data: obj
        }).done(function (data) {
            addM.style.display = "none";
            bookDataTable.ajax.reload();
        });
    }

}