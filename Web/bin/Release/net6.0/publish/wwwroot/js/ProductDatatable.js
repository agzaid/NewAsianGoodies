$(document).ready(function () {
    $("#Products").dataTable({
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/en/Admin/Product/LoadProducts",
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": [0],
            "visible": false,
            "searchable": false
        }],
        "columns": [
            { "data": "id", "name": "ID", "autowidth": true },
            { "data": "name", "name": "Name", "autowidth": true },
            { "data": "price", "name": "Price", "autowidth": true },
            { "data": "image", "name": "Image", "autowidth": true },
            { "data": "description", "name": "Description", "autowidth": true },
            {
                "render": function (data, type, row) { return `<a href="#" class="btn btn-danger" onclick=DeleteProduct("` + row.id + `");>Delete</a>` },
                "orderable": false
            },
        ]
    });

});