var dataTable;

$(document).ready(function () {
    loadDataTable();
    $('html, body').animate({ scrollTop: $(document).height() }, 'slow');  // Прокрутка страницы вниз
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/manager/group/getall' },
        "columns": [
            { "data": "name", "width": "25%" },
            { "data": "creationDate", "width": "15%" },
            { "data": "startTime", "width": "10%" },
            { "data": "endTime", "width": "10%" },
            { "data": "classroom", "width": "10%" },
            {
                data: 'id',
                "render": function (data, type, row) {
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/manager/group/upsert?id=${data}&subjectId=${row.subjectId}" class="btn btn-primary mx-2"> 
                            <i class="bi bi-pencil-square"></i> Edit </a>
                        <a onClick=Delete('/manager/group/delete/${data}') class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
                        <a href="/manager/group/assignStudents?groupId=${data}" class="btn btn-outline-primary border form-control">
                            Assign Students
                        </a>
                        <button class="btn btn-info mx-2" onClick="GenerateLessons(${data})">
                            <i class="bi bi-calendar-plus"></i> Generate Lessons
                        </button>
                    </div>`;
                }
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

function GenerateLessons(groupId) {
    $.ajax({
        url: `/manager/lessons/GenerateLessonsForMonth`, // URL метода контроллера
        type: 'POST', // Используем POST-запрос
        data: { groupId: groupId },
        success: function (response) {
            toastr.success(response); // Показываем сообщение об успешной генерации
            dataTable.ajax.reload(); // Перезагружаем таблицу
        },
        error: function (xhr) {
            let errorMessage = xhr.responseText || "Произошла ошибка при генерации уроков.";
            toastr.error(errorMessage); // Показываем сообщение об ошибке
        }
    });
}
