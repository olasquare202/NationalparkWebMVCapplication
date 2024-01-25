$(function () {
    $.ajax({
        "type": "GET",
        url: "https://localhost:7186/api/v1/trails",
       // url: "/Trail/GetAllTrail",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnSuccess,
        failure: function (response) {
            alert(response.d);
        },
        error: function (response) {
            alert(response.d);
        }
    });

});
function OnSuccess(response) {
    console.log(response)
    $("#nationalParks").DataTable(
        {
            bLengthChange: true,
            lengthMenu: [[5, 10, -1], [5, 10, "All"]],
            bFilter: true,
            bSort: true,
            bPaginate: true,
            data: response,
            "columns": [
                { "data": "nationalPark.name", "width": "25%" },
                { "data": "name", "width": "20%" },
                { "data": "distance", "width": "15%" },
                { "data": "elevation", "width": "15%" },
               
                {
                    "data": "id",//To Update or Delete by Id
                    "render": function (data) {
                        //https://localhost:7186/api/v1/nationalParks/8
                        console.log(data)    //onclick=Delete("/nationalParks/Delete/${data}")
                        return `<div class="text-center">
                              <a href="/Trail/Upsert/${data}" class='btn btn-success text-white'
                             style='cursor:pointer;'> <i class='far fa-edit'></i></a> 
                             &nbsp; 
                           <a onclick=Delete("/Trail/Delete/${data}") class='btn btn-danger text-white'
                             style='cursor:pointer;'> <i class='far fa-trash-alt'></i></a> 
                            </div>
                                 `;
                                 //=Delete("https://localhost:7186/api/v1/nationalParks/${data}")  this work for delete throught d API call
                                 //"https://localhost:7186/api/v1/trails/${data}
                        //"/Trail/Delete/${data}"
                    },
                }

            ]

        });
};

function Delete(url) {
    swal({
        title: "Are you sure you want to Delete?",
        text: "You will not be able to restore the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: 'DELETE',
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        DataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}

                              