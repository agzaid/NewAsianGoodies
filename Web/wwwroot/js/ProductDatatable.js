
$(document).ready(function () {
    debugger;
    var AGelem = $(".AG-data-table");
    var AGelemID = AGelem[0].id;
    var AGelemColumns = $("#" + AGelemID).attr("data-AG-columns");
    var url = $("#" + AGelemID).attr("data-AG-load-url");

    $("#" + AGelemID).dataTable({
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": url,
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": [0],
            "visible": false,
            "searchable": false
        }, {
            targets: 1,
            data: name,
            orderable: true,
            className: 'text-start',
            render: function (data, type, row) {
                if (row.image) {
                    return `<div class="d-flex align-items-center"><a class="symbol symbol-50px"><span class="symbol-label" style=""background-image:url(../../../Uploads/Products/` + row.image +`);"></span></a><div class="ms-5"><a href="` + t.getAttribute("data-kaj-edit-url") + `/` + row.id + `" class="text-gray-800 text-hover-primary fs-5 fw-bolder" data-kt-ecommerce-productfilter="product_name">` + row.name + `</a></div></div > `
                } else {
                    return '<a href="' + '/' + row.id + '" class="text-gray-800 text-hover-primary fs-5 fw-bolder mb-1" data-kaj-filter="item_name">' + row.productName + '</a><input type="hidden" data-kaj-filter="item_id" value="' + row.id + '">';;
                }
            },
            }

        ],
        "columns": [
            { "data": "id", "name": "ID", "autowidth": true },
            { "data": "name", "name": "Name", "autowidth": true },
            { "data": "price", "name": "Price", "autowidth": true },
            { "data": "displayOrder", "name": "DisplayOrder", "autowidth": true },
            { "data": "quantity", "name": "Quantity", "autowidth": true },
            { "data": "status", "name": "Status", "autowidth": true },
            { "data": "thumbnailImage", "name": "ThumbnailImage", "autowidth": true },
            { "data": "image", "name": "Image", "autowidth": true },
            { "data": "shortDescription", "name": "ShortDescription", "autowidth": true },
            {
                "render": function (data, type, row) { return `<a href="#" class="btn btn-danger" onclick=DeleteProduct("` + row.id + `");>Delete</a>` },
                "orderable": false
            },
        ]
    });

});