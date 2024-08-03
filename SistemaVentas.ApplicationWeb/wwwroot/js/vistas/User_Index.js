const Model_Base = {
    idUser: 0,userName: "",email: "",
    phone: "", idRol: 0, itsActive: 1, urlPhoto: ""
}

let TableData;
$(document).ready(function () {


    fetch("/UserApplication/ListAllRol")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.length > 0) {
                responseJson.forEach((item) => {
                    $("#cboRol").append(
                        $("<option>").val(item.idRol).text(item.descriptions)
                    )
                })
            }
        })


    TableData = $('#tbdata').DataTable({
        responsive: true,
         "ajax": {
             "url": '/UserApplication/ListAllUsers',
             "type": "GET",
             "datatype": "json"
         },
        "columns": [
            { "data": "idUser", "visible": false, "searchable": false },
            {
                "data": "urlPhoto", render: function (data) {
                    return `<img style="height:60px" src=${data} class="rounded mx-auto d-block"/>`;
                }
            },
            { "data": "userName" },
            { "data": "email" },
            { "data": "phone" },
            { "data": "nameRol" },
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
                filename: 'Reporte Usuarios',
                exportOptions: {
                    columns: [2,3,4,5,6]
                }
            }, 'pageLength'
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });
})


//LO QUE HACE ESTA FUNCION ES MOSTRAR LOS DATOS EN LOS CAMPOS DE LA VISTA NUEVO USUARIO
function MostararModal(modelo = Model_Base) {
    $("#txtId").val(modelo.idUser)
    $("#txtNombre").val(modelo.userName)
    $("#txtCorreo").val(modelo.email)
    $("#txtTelefono").val(modelo.phone)
    $("#cboRol").val(modelo.idRol == 0 ? $("#cboRol option:first").val() : modelo.idRol)
    $("#cboEstado").val(modelo.itsActive)
    $("#txtFoto").val("")
    $("#imgUsuario").attr("src",modelo.urlPhoto)

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
    modelo["idUser"] = parseInt($("#txtId").val())
    modelo["userName"] = $("#txtNombre").val()
    modelo["email"] = $("#txtCorreo").val()
    modelo["phone"] = $("#txtTelefono").val()
    modelo["idRol"] = $("#cboRol").val()
    modelo["itsActive"] = $("#cboEstado").val()

    const inputFoto = document.getElementById("txtFoto")
    const formData = new FormData();

    //LOS NOMBRES DEL PRIMER CAMPO DEBEN SER IGUALES A LO QUE RECIBE EL CONTROLADOR EN ESTE CASO DE USUARIO
    // EN SU METODO CREAR USUARIO
    formData.append("PhotoUser", inputFoto.files[0])
    formData.append("ModelEntity", JSON.stringify(modelo))

    $("#modalData").find("div.modal-content").LoadingOverlay("show");
    if (modelo.idUser == 0) {

        fetch("/UserApplication/CreateUserProfile", {
            method: "POST",
            body: formData
        }).then(response => {

            $("#modalData").find("div.modal-content").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);

        }).then(responseJson => {
            if (responseJson.state) {
                TableData.row.add(responseJson.object).draw(false)
                $("#modalData").modal("hide")
                swal("Listo!", "El usuario fue creado", "success")
            }
            else {
                swal("Lo sentimos!", responseJson.message, "error")
            }
        })

    }
    else {
        fetch("/UserApplication/UpdateUserProfile", {
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
                swal("Listo!", "La Informacion del usuario fue modificada", "success")
            }else {
                swal("Lo sentimos!", responseJson.message, "error")
            }
        })
    }
})


//RECOPILAR LA FILA A EDITAR CON SU INFORMACION DE USUARIO
let filaSeleccionada;
$("#tbdata tbody").on("click", ".btn-editar", function () {
    if ($(this).closest("tr").hasClass("child")) {
        filaSeleccionada = $(this).closest("tr").prev();
    }else {
        filaSeleccionada = $(this).closest("tr");
    }

    const data = TableData.row(filaSeleccionada).data();
    //console.log(data);
    MostararModal(data);
})

//ELIMINAR USUARIO
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
        text: `Eliminar el Usuario: "${data.userName}"`,
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Si, eliminar",
        cancelButtonText: "No, cancelar",
        closeOnConfirm: false,
        closeOnCancel:true
    },
        function (response) {
            if (response) {

                //MOSTRAR EL MODAL DE CARGANDO
                $(".showSweetAlert").find("div.modal-content").LoadingOverlay("show");

                fetch(`/UserApplication/DeleteUserProfile?IdUserProfileParameter=${data.idUser}`, {
                    method: "DELETE"
                }).then(response => {

                    $(".showSweetAlert").find("div.modal-content").LoadingOverlay("hide");
                    return response.ok ? response.json() : Promise.reject(response);

                }).then(responseJson => {

                    if (responseJson.state) {

                        TableData.row(fila).remove().draw()
                       
                        swal("Listo!", "La Informacion del usuario fue 'Eliminada'", "success")
                    } else {
                        swal("Lo sentimos!", responseJson.message, "error")
                    }
                })
            }
        }
      
    )
})

