let partial_id;
let categoryDataTable;
let bookDataTable;
let categoryM = document.getElementById("categoryBooksModal");
let addM = document.getElementById("addModal");
let editM = document.getElementById("editModal");

$(function () {

    categoryDataTable = $('#categoryTable').DataTable({
        "columns": [

            { "data": "categoryName", "sType": "string", orderable: true },
            {
                "data": "categoryID", "sType": "num", orderable: false,
                "render": function (data, type, full, meta) {
                    if (data != null && (type == 'display' || type == 'filter')) {

                        return "<button type='button' class='btn btn-info' onclick='LoadCategoryBooks(" + data + ",\"" + full.categoryName + "\")'>Books</button>";
                    }
                    return data;
                }
            },
            {
                "data": "categoryID", "sType": "num", orderable: false,
                "render": function (data, type, full, meta) {
                    if (data != null && (type == 'display' || type == 'filter')) {
                        return "<button onclick='editModalClick(" + data + ",\"" + full.categoryName + "\")' class='btn btn-warning'>Edit</button>" +
                            "<button onclick='deleteCategory(" + data + ")' class='btn btn-danger'>Delete</button>";
                    }
                    return data;
                }
            },
        ],




        "ajax": function (data, callback, settings) {
            obj = {};
            $.ajax({
                url: "/Categories/GetAllCategories",
                type: "GET",
                data: obj
            }).done(function (data) {
                console.log(data);


                callback({ "data": data });
            }) // a_done
        } //a_funct


    }); // dt



    bookDataTable = $('#categoryBookTable').DataTable({



        "columns": [
            { "data": "bookName", "sType": "string", orderable: true },
            { "data": "bookPages", "sType": "num", orderable: true },
            { "data": "bookPrice", "sType": "num", orderable: true },
            { "data": "authorName", "sType": "string", orderable: true },

        ],
        "ajax": function (data, callback, settings) {
            obj = {};
            $.ajax({
                url: "/Categories/GetCategoryBooks/" + partial_id,
                type: "GET",
                data: obj
            }).done(function (data) {
                console.log(data);


                callback({ "data": data });
            }) //done
        }

    }); // datatables



}); // f






function LoadCategoryBooks(ID, catName) {
    console.log("içerde")
    partial_id = ID;
    document.getElementById("name4Partial").innerHTML = catName + " Kitapları";
    bookDataTable.ajax.reload();
    categoryM.style.display = "block";

}

function addModalClick() {
    $("#categoryName").val(null);
    addM.style.display = "block";
}
function editModalClick(ID, Name) {
    $("#getName").val(Name);
    $("#getID").val(ID);
    editM.style.display = "block";

}
function deleteCategory(ID) {
    obj = {};
    $.ajax({
        url: "/Categories/Delete/" + ID,
        type: "POST",
        data: obj
    }).done(function (data) {
        categoryDataTable.ajax.reload();
    })
}


//
window.onclick = function (event) {
    if (event.target == categoryM) {
        categoryM.style.display = "none";
    }
    if (event.target == addM) {
        addM.style.display = "none";
    }
    if (event.target == editM) {
        editM.style.display = "none";
    }

}


    //