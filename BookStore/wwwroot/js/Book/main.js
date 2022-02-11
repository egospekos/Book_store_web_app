let bookDataTable;
let select_category;
let categorySelectize;
var addM = document.getElementById("addModal");
var editM = document.getElementById("editModal");

$(function () {
    CreateCategorySelectize();
    categorySelectize = select_category[0].selectize;
    LoadCategorySelectize();
    bookDataTable = $('#bookTable').DataTable({
        "dom": 'lfrtip',
        "columns": [

            { "data": "bookName", "sType": "string", orderable: true },
            { "data": "bookPages", "sType": "num", orderable: true },
            { "data": "bookPrice", "sType": "num", orderable: true },
            { "data": "authorName", "sType": "string", orderable: true },
            {
                "data": "categoryNames", "sType": "string", orderable: true,
                "render": function (data, type, full, meta) {

                    if (data == null)
                        return "Kategorisiz";
                    return data;
                }
            },
            {
                "data": "bookID", "sType": "num", orderable: false,
                "render": function (data, type, full, meta) {

                    if (type == 'display' || type == 'filter')
                        return "<button onclick='editModalClick( " + data + " , \"" + full.bookName + "\" ," + full.bookPages + "," + full.bookPrice + "," + full.authorID + ")' class='btn btn-warning'>Edit</button>" +
                            "<button onclick='deleteBook(" + data + ")' class='btn btn-danger'>Delete</button>" +
                            "<i class='btn bi bi-clipboard' onclick='copyText(\"" + full.bookName + "\",\"" + full.bookPages + "\",\"" + full.bookPrice + "\",\"" + full.authorName + "\",\"" + full.categoryName + "\")'></i>";
                    return data;
                }
            },
        ],




        "ajax": function (data, callback, settings) {
            obj = {};

            obj.categoryIDs = $("#categorySelect").val();
            obj.minPrice = $("#getMinPrice").val();
            obj.maxPrice = $("#getMaxPrice").val();
            console.log("min: " + $("#getMinPrice").val() + " /max: " + $("#getMaxPrice").val());
            $.ajax({
                url: "/Books/GetAllBooks",
                type: "POST",
                data: obj
            }).done(function (data) {
                console.log(data);


                callback({ "data": data });
            })
        }







    });

});




function CreateCategorySelectize() {
    select_category = $("#categorySelect").selectize({
        create: false,
        valueField: 'categoryID',
        sortField: 'categoryName',
        labelField: "categoryName",
        searchField: "categoryName",
        maxItems: 3,
    });
}

function LoadCategorySelectize() {
    obj = {};
    $.ajax({
        url: "/Books/getCategories",
        type: "GET",
        data: obj
    }).done(function (data) {

        for (var i = 0; i < data.length; i++) {
            categorySelectize.addOption(data[i]);
        }
        categorySelectize.refreshOptions();
    });
}


function FilterClick() {
    var max = $("#getMaxPrice").val();
    var min = $("#getMinPrice").val();
    if (max != "" && min > max) {

        alert("Max value must be greater.");
    }
    else {
        bookDataTable.ajax.reload();
    }

}

function FilterRemove() {
    categorySelectize.setValue(null);
    $("#getMinPrice").val(null);
    $("#getMaxPrice").val(null);
    bookDataTable.ajax.reload();


}

function AddModalClick() {
    $("#getName").val(null);
    $("#getPages").val(null);
    $("#getPrice").val(null);
    _createAuthorSelectize.setValue(null);
    _createCategorySelectize.setValue(null);


    addM.style.display = "block";
}
function deleteBook(ID) {
    obj = {};
    $.ajax({
        url: "/Books/Delete/" + ID,
        type: "POST",
        data: obj
    }).done(function (data) {
        bookDataTable.ajax.reload();
    })
}
window.onclick = function (event) {
    if (event.target == addM) {
        addM.style.display = "none";
    }
    else if (event.target == editM) {
        editM.style.display = "none";
    }
}


function editModalClick(ID, Name, Page, Price, Author) {
    $("#edit_getID").val(ID);
    $("#edit_getName").val(Name);
    $("#edit_getPages").val(Page);
    $("#edit_getPrice").val(Price);
    E_AuthorSelectize.setValue(Author, false);

    var Categories;
    obj = {};
    $.ajax({
        url: "/Books/getBookCategories/" + ID,
        type: "GET",
        data: obj
    }).done(function (data) {

        Categories = data;

        E_CategorySelectize.setValue(Categories, false);
    })




    editM.style.display = "block";
}

function copyText(bookName, Pages, Price, Author, Category) {
    var text = "Kitap: " + bookName + ", " + Pages + " sayfa, Fiyat: " + Price + "$, Yazar: " + Author + ", Kategorileri: " + Category;
    navigator.clipboard.writeText(text);
}