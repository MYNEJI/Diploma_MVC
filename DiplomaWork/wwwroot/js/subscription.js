var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/student/subscription/getall' },
        "columns": [
            { data: 'title', "width": "25%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                        <button onclick="prolongSubscription(${data})" class="btn btn-primary mx-2">
                            <i class="bi bi-pencil-square"></i>Prolong course
                        </button>
                    </div>`;
                },

                "width": "25%"
            }
        ]
    });
}
function prolongSubscription(subjectId) {
    console.log("Prolonging subscription for subjectId:", subjectId);
    $.ajax({
        url: '/student/subscription/upsert',
        type: 'POST',
        data: { subjectId: subjectId },
        success: function (response) {
            alert('Subscription prolonged successfully');
            dataTable.ajax.reload();
        },
        error: function (xhr, status, error) {
            console.error("Error:", error);
            console.log("XHR:", xhr);
            alert('Error while prolonging subscription: ' + subjectId);
        }
    });
}