// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var correctCaptcha = function (response) {    
    document.getElementById("submitButton").disabled = false;
};

//Delete Sweet Alert 2
var obj = {status: false, ele: null };

function DeleteConfirm(btnDelete) {

    if (obj.status) {
        obj.status = false;
        return true;
    };

    Swal.fire({
        title: 'İşleme Devam Edilsin mi?',
        text: "Silme işlemini onaylıyorsunuz",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Evet, devam etmek istiyorum',
        cancelButtonText: "İptal",

    }).then((result) => {
        if (result.value) {
            obj.status = true;
            //do postback on success
            obj.ele.click();

            Swal.fire({
                title: 'Onaylandı!',
                text: ' ',
                type: 'success',
            });
        }
    });
    obj.ele = btnDelete;
    return false;
};

//Datatable Search
const table_instance = mdb.Datatable.getInstance(document.getElementById('datatable-search'));
document.getElementById('datatable-search-input').addEventListener('input', (e) => {
    table_instance.search(e.target.value);
});


function searchFunction() {
    var input, filter, table, tr, td, i, txtValue, j, td2;
    input = document.getElementById("table-search-input");
    filter = input.value.toUpperCase();
    table = document.getElementById("table-search");
    tr = table.getElementsByTagName("tr");
    for (i = 1; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td");
        tr[i].style.display = "none";

        for (j = 0; j < td.length; j++) {
            td2 = td[j];
            if (td2) {
                txtValue = td2.textContent || td2.innerText;
                if (txtValue.toUpperCase().indexOf(filter) > -1) {
                    tr[i].style.display = "";
                }
            }
        }
    }
}

function searchFunction2() {
    var input, filter, table, tr, td, i, txtValue, j, td2;
    input = document.getElementById("table-search-input-2");
    filter = input.value.toUpperCase();
    table = document.getElementById("table-search-2");
    tr = table.getElementsByTagName("tr");
    for (i = 1; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td");
        tr[i].style.display = "none";

        for (j = 0; j < td.length; j++) {
            td2 = td[j];
            if (td2) {
                txtValue = td2.textContent || td2.innerText;
                if (txtValue.toUpperCase().indexOf(filter) > -1) {
                    tr[i].style.display = "";
                }
            }
        }
    }
}