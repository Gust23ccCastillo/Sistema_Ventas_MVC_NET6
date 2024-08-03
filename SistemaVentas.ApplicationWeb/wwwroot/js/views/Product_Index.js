const Model_Base = {
    idProduct: 0, barcode: "", mark: "",
    descriptions: "", idCategoria: 0, stock: "", urlImage:"",
    price:"",itsActive: 1
}

$(document).ready(function () {


    fetch("/CategorysApplication/GetAllCategorys")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            console.log(responseJson);
            if (responseJson.data.length > 0) {
                responseJson.data.forEach((data) => {
                   
                    $("#cboCategoria").append(
                        $("<option>").val(data.idCategoria).text(data.descriptions)
                    )
                })
            }
        })


    TableData = $('#tbdata').DataTable({
        responsive: true,
        "ajax": {
            "url": '/ProductsApplication/GetListProducts',
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "idProduct", "visible": false, "searchable": false },
            {
                "data": "urlImage", render: function (data) {
                    return `<img style="height:60px" src=${data} class="rounded mx-auto d-block"/>`;
                }
            },
            { "data": "barcode" },
            { "data": "mark" },
            { "data": "descriptions" },
            { "data": "nameCategory" },
            { "data": "stock" },
            { "data": "price" },
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
                filename: 'Reporte Productos',
                exportOptions: {
                    columns: [2, 3, 4, 5, 6,7,8]
                }
            }, 'pageLength'
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });
})

function MostararModal(modelo = Model_Base) {
    $("#txtId").val(modelo.idProduct)
    $("#txtCodigoBarra").val(modelo.barcode)
    $("#txtMarca").val(modelo.mark)
    $("#txtDescripcion").val(modelo.descriptions)
    $("#cboCategoria").val(modelo.idCategoria == 0 ? $("#cboCategoria option:first").val() : modelo.idCategoria)
    $("#txtStock").val(modelo.stock)
    $("#txtPrecio").val(modelo.price)
    $("#cboEstado").val(modelo.itsActive)
    $("#txtImagen").val("")
    $("#imgProducto").attr("src", modelo.urlImage)

    $("#modalData").modal("show")
}

$("#btnNuevo").click(function () {
    MostararModal()
})

$("#btnGuardar").click(function () {
    //debugger;
    //validar los campos vacios y serializarlos
    const inputs = $("input.input-validar").serializeArray();

    //buscar dentro del array los campo con vacios o con espacios
    const inputs_sin_valor = inputs.filter((item) => item.value.trim() == "")
    if (inputs_sin_valor.length > 0) {

        //mostrar el mensaje con la libreria toastr y seleccionar el campo vacio en la plantilla
        const mensaje = `Debe completar el campo: "${inputs_sin_valor[0].name}"`;
        toastr.warning("", mensaje);

        $(`input[name="${inputs_sin_valor[0].name}"]`).focus()
        return;
    }

    const modelo = structuredClone(Model_Base);
    modelo["idProduct"] = parseInt($("#txtId").val())
    modelo["barcode"] = $("#txtCodigoBarra").val()
    modelo["mark"] = $("#txtMarca").val()
    modelo["descriptions"] = $("#txtDescripcion").val()
    modelo["idCategoria"] = $("#cboCategoria").val()
    modelo["stock"] = $("#txtStock").val()
    modelo["price"] = $("#txtPrecio").val()
    modelo["itsActive"] = $("#cboEstado").val()

    const inputFoto = document.getElementById("txtImagen")
    const formData = new FormData();

    //LOS NOMBRES DEL PRIMER CAMPO DEBEN SER IGUALES A LO QUE RECIBE EL CONTROLADOR EN ESTE CASO DE USUARIO
    // EN SU METODO CREAR USUARIO
    formData.append("imageProduct", inputFoto.files[0])
    formData.append("model", JSON.stringify(modelo))

    $("#modalData").find("div.modal-content").LoadingOverlay("show");
    if (modelo.idProduct == 0) {

        fetch("/ProductsApplication/CreateProduct", {
            method: "POST",
            body: formData
        }).then(response => {

            $("#modalData").find("div.modal-content").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);

        }).then(responseJson => {
            if (responseJson.state) {
                TableData.row.add(responseJson.object).draw(false)
                $("#modalData").modal("hide")
                swal("Listo!", "El Producto fue creado", "success")
            }
            else {
                swal("Lo sentimos!", responseJson.message, "error")
            }
        })

    }
    else {
        fetch("/ProductsApplication/EditProduct", {
            method: "PUT",
            body: formData
        }).then(response => {

            //MOSTRAR EL MODAL DE CARGANDO
            $("#modalData").find("div.modal-content").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);

        }).then(responseJson => {

            if (responseJson.state) {

                TableData.row(filaSeleccionada).data(responseJson.object).draw(false);
                filaSeleccionada = null;

                $("#modalData").modal("hide")
                swal("Listo!", "La Informacion del Producto fue modificada", "success")
            } else {
                swal("Lo sentimos!", responseJson.message, "error")
            }
        })
    }
})

//FUNCIO PARA OBTENER LA DATA AL EDITAR UN PRODUCTO
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
        title: "Esta seguro de Eliminar ?",
        text: `Eliminar el Producto: "${data.descriptions}"`,
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

                //MOSTRAR EL MODAL DE CARGANDO
                $(".showSweetAlert").find("div.modal-content").LoadingOverlay("show");

                fetch(`/ProductsApplication/DeleteProduct?idProduct=${data.idProduct}`, {
                    method: "DELETE"
                }).then(response => {

                    $(".showSweetAlert").find("div.modal-content").LoadingOverlay("hide");
                    return response.ok ? response.json() : Promise.reject(response);

                }).then(responseJson => {

                    if (responseJson.state) {

                        TableData.row(fila).remove().draw()

                        swal("Listo!", "La Informacion del Producto fue 'Eliminada'", "success")
                    } else {
                        swal("Lo sentimos!", responseJson.message, "error")
                    }
                })
            }
        }

    )
})

