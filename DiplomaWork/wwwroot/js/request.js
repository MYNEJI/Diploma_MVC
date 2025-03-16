var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/manager/request/getall' },
        "columns": [
            {
                data: 'requestDate', "width": "20%",
                render: function (data) {
                    var date = new Date(data); // Преобразование строки в объект Date
                    return date.toLocaleString(); // Форматирование в строку "дата и время"
                }
            },
            { data: 'subject.title', "width": "20%" },
            { data: 'email', "width": "15%" },
            { data: 'phoneNumber', "width": "10%" },
            {
                data: 'status', "width": "10%",
                render: function (data, type, row) {
                    switch (data) {
                        case 0: return 'Pending';
                        case 1: return 'Not Processed';
                        case 2: return 'Rejected';
                        case 3: return 'Accepted';
                        default: return 'Unknown';
                    }
                }
            },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                     <a href="/manager/request/upsert?id=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                     <a onClick=Delete('/manager/request/delete/${data}') class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
                    </div>`
                },
                "width": "25%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
}