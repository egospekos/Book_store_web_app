let authorDataTable;
let editM = document.getElementById("editModal");
let addM = document.getElementById("addModal");
let booksM = document.getElementById("booksModal");
let partial_id;
let bookDataTable;
$(function () {
    authorDataTable = $('#authorTable').DataTable({

        "columns": [

            { "data": "authorName", "sType": "string", orderable: true },
            {
                "data": "authorBday", "sType": "date", orderable: true,
                "render": function (data, type, full, meta) {
                    if (data != null && (type == 'display' || type == 'filter'))
                        var _date = new Date(data);
                    return (("0" + _date.getDate()).slice(-2)) + "." + (("0" + (_date.getMonth() + 1)).slice(-2)) + "." + _date.getFullYear();
                    return "None";
                }
            },
            {
                "data": "countBook", "sType": "num", orderable: true,
                "render": function (data, type, full, meta) {
                    if (data != null && (type == 'display' || type == 'filter'))
                        return "<button type='button' class='btn btn-info' onclick='LoadAuthorBooks(" + full.authorID + ",\"" + full.authorName + "\")'>" + data + "</button>";
                    return data;
                }
            },
            {
                "data": "authorID", "sType": "num", orderable: false,
                "render": function (data, type, full, meta) {
                    if (type == 'display' || type == 'filter')
                        return "<button type='button' class='btn btn-warning' onclick='editID(" + data + ",\"" + full.authorName + "\",\"" + full.authorBday + "\")'>Edit</button>" +
                            "<button  class='btn btn-danger' onclick='deleteID(" + data + "," + full.countBook + ")'>Delete</button>";
                    return data;
                }
            },
        ],
        "ajax": function (data, callback, settings) {
            obj = {};
            rows = [];
            $.ajax({
                url: "/Authors/GetAllAuthors",
                type: "GET",
                data: obj
            }).done(function (data) {
                console.log(data);


                callback({ "data": data });
            }) //done
        }


    }); // datatable
});


function editID(ID, Name, Birthday) {
    $("#getID").val(ID);
    $("#getName").val(Name);


    console.log(Birthday);
    var _date = new Date(Birthday);

    var day = ("0" + _date.getDate()).slice(-2);
    var month = ("0" + (_date.getMonth() + 1)).slice(-2);

    var bDay = _date.getFullYear() + "-" + (month) + "-" + (day);

    $("#oldBday").val(bDay);
    $("#getBday").val(bDay);
    editM.style.display = "block";
}

//
bookDataTable = $('#bookTable').DataTable({



    "columns": [
        { "data": "bookName", "sType": "num", orderable: true },
        { "data": "bookPages", "sType": "string", orderable: true },
        { "data": "bookPrice", "sType": "date", orderable: true },

    ],
    "ajax": function (data, callback, settings) {
        obj = {};
        $.ajax({
            url: "/Authors/GetBooks/" + partial_id,
            type: "GET",
            data: obj
        }).done(function (data) {
            console.log(data);


            callback({ "data": data });
        }) //done
    }

}); // datatables

//
function addButtonClick() {
    $("#authorName").val(null);
    $("#authorBday").val(null);
    $("#oldBday").val(null);
    addM.style.display = "block";
}
//
function deleteID(ID, books) {
    if (books > 0) {
        alert("Can't delete Author with books.");
    }
    else {
        obj = {};
        $.ajax({
            url: "/Authors/Delete/" + ID,
            type: "POST",
            data: obj
        }).done(function (data) {

            authorDataTable.ajax.reload();
        });
    }

}
//

window.onclick = function (event) {

    if (event.target == editM) {
        editM.style.display = "none";
    }
    else if (event.target == addM) {
        addM.style.display = "none";
    }
    else if (event.target == booksM) {
        booksM.style.display = "none";
    }
}


//

function LoadAuthorBooks(ID, authorName) {

    partial_id = ID;
    document.getElementById("name4Partial").innerHTML = authorName + " Kitapları";
    bookDataTable.ajax.reload();
    booksM.style.display = "block";

}

    //