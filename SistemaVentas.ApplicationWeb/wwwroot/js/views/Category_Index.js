const Model_Base = {
    idCategoria: 0,
    descriptions: "",
    itsActive: 1
}

let TableData;
$(document).ready(function () {


    TableData = $('#tbdata').DataTable({
        responsive: true,
        "ajax": {
            "url": '/CategorysApplication/GetAllCategorys',
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "idCategoria", "visible": false, "searchable": false },
            { "data": "descriptions" },
            {
                "data": "itsActive", render: function (data) {

                    if (data == 1)
                        return `<span class="badge badge-info">Activo</span>`;
                    else
                        return `<span class="badge badge-danger">No Activo</span>`;
                }
            },
            {
                "defaultContent": '<button class="btn btn-primary btn-editar btn-sm mr-2"><i class="fas fa-pencil-alt"></i></button>' +
                    '<button class="btn btn-danger btn-eliminar btn-sm"><i class="fas fa-trash-alt"></i></button>',
                "orderable": false,
                "searchable": false,
                "width": "80px"
            }
        ],
        order: [[0, "desc"]],
        dom: "Bfrtip",
        buttons: [
            {
                text: 'Exportar Excel',
                extend: 'excelHtml5',
                title: '',
                filename: 'Reporte Categorias',
                exportOptions: {
                    columns: [1,2]
                }
            }, 'pageLength'
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });
})

function MostararModal(modelo = Model_Base) {
    $("#txtId").val(modelo.idCategoria)
    $("#txtDescripcion").val(modelo.descriptions)
    $("#cboEstado").val(modelo.itsActive)
  
    $("#modalData").modal("show")
}

$("#btnNuevo").click(function () {
    MostararModal()
})

$("#btnGuardar").click(function () {
   
    if ($("#txtDescripcion").val().trim() == "") {
        toastr.warning("", "Debe completar el campo: Descripcion");
        $("#txtDescripcion").focus()
        return;
    }
    const modelo = structuredClone(Model_Base);
    modelo["idCategoria"] = parseInt($("#txtId").val())
    modelo["descriptions"] = $("#txtDescripcion").val()
    modelo["itsActive"] = $("#cboEstado").val()

    $("#modalData").find("div.modal-content").LoadingOverlay("show");
    if (modelo.idUser == 0) {

        fetch("/CategorysApplication/CreateCategory", {
            method: "POST",
            headers: { "Content-Type": "application/json; charset=utf-8" },
            body: JSON.stringify(modelo)
        }).then(response => {

            $("#modalData").find("div.modal-content").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);

        }).then(responseJson => {
            if (responseJson.state) {
                TableData.row.add(responseJson.object).draw(false)
                $("#modalData").modal("hide")
                swal("Listo!", "La categoria fue creado", "success")
            }
            else {
                swal("Lo sentimos!", responseJson.message, "error")
            }
        })

    }
    else {
        fetch("/CategorysApplication/UpdateCategory", {
            method: "PUT",
            headers: { "Content-Type": "application/json; charset=utf-8" },
            body: JSON.stringify(modelo)
        }).then(response => {

            $("#modalData").find("div.modal-content").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);

        }).then(responseJson => {

            if (responseJson.state) {

                TableData.row(filaSeleccionada).data(responseJson.object).draw(false);
                filaSeleccionada = null;

                $("#modalData").modal("hide")
                swal("Listo!", "Informacion de la categoria fue modificada", "success")
            } else {
                swal("Lo sentimos!", responseJson.message, "error")
            }
        })
    }
})

let filaSeleccionada;
$("#tbdata tbody").on("click", ".btn-editar", function () {
    if ($(this).closest("tr").hasClass("child")) {
        filaSeleccionada = $(this).closest("tr").prev();
    } else {
        filaSeleccionada = $(this).closest("tr");
    }

    const data = TableData.row(filaSeleccionada).data();
    //console.log(data);
    MostararModal(data);
})

$("#tbdata tbody").on("click", ".btn-eliminar", function () {
    let fila;
    if ($(this).closest("tr").hasClass("child")) {
        fila = $(this).closest("tr").prev();
    } else {
        fila = $(this).closest("tr");
    }

    const data = TableData.row(fila).data();
    swal({
        title: "Esta seguro ?",
        text: `Eliminar la Categoria: "${data.descriptions}"`,
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Si, eliminar",
        cancelButtonText: "No, cancelar",
        closeOnConfirm: false,
        closeOnCancel: true
    },
        function (response) {
            if (response) {

               
                $(".showSweetAlert").find("div.modal-content").LoadingOverlay("show");

                fetch(`/CategorysApplication/RemoveCategory?idCategory=${data.idCategoria}`, {
                    method: "DELETE"
                }).then(response => {

                    $(".showSweetAlert").find("div.modal-content").LoadingOverlay("hide");
                    return response.ok ? response.json() : Promise.reject(response);

                }).then(responseJson => {

                    if (responseJson.state) {

                        TableData.row(fila).remove().draw()

                        swal("Listo!", "Informacion de la categoria fue 'Eliminada'", "success")
                    } else {
                        swal("Lo sentimos!", responseJson.message, "error")
                    }
                })
            }
        }

    )
})

