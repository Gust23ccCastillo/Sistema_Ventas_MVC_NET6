$(document).ready(function () {

    $(".card-body").LoadingOverlay("show");
    fetch("/BusinessApplication/GetBusinessInformation")
        .then(response => {
            $(".card-body").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            
            if (responseJson.state) {
                const data = responseJson.object
                $("#txtNumeroDocumento").val(data.documentNumber)
                $("#txtRazonSocial").val(data.nameBusiness)
                $("#txtCorreo").val(data.email)
                $("#txtDireccion").val(data.addressBusiness)
                $("#txTelefono").val(data.phone)
                $("#txtImpuesto").val(data.percentageTax)
                $("#txtSimboloMoneda").val(data.coin)
                $("#imgLogo").attr("src",data.urlLogo)

            }
            else {
                swal("Lo sentimos!", responseJson.message, "error")
            }
        })

})

$("#btnGuardarCambios").click(function () {
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
    else {
        const model = {
            documentNumber: $("#txtNumeroDocumento").val(),
            nameBusiness: $("#txtRazonSocial").val(),
            email: $("#txtCorreo").val(),
            addressBusiness: $("#txtDireccion").val(),
            phone: $("#txTelefono").val(),
            percentageTax: $("#txtImpuesto").val(),
            coin: $("#txtSimboloMoneda").val()
            
        }

        const inputLogo = document.getElementById("txtLogo")
        const formData = new FormData()
        formData.append("logo", inputLogo.files[0])
        formData.append("model", JSON.stringify(model))

        swal({
            title: "Esta seguro ?",
            text: `Actualizar Informacion: "${model.nameBusiness}"`,
            type: "warning",
            showCancelButton: true,
            confirmButtonClass: "btn-danger",
            confirmButtonText: "Si, Guardar",
            cancelButtonText: "No, cancelar",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (response) {
                if (response) {
                    $(".card-body").LoadingOverlay("show");
                    fetch("/BusinessApplication/SaveChangesBusinessInformation", {
                        method: "POST",
                        body: formData
                    })
                        .then(response => {
                            $(".card-body").LoadingOverlay("hide");
                            return response.ok ? response.json() : Promise.reject(response);
                        })
                        .then(responseJson => {

                            if (responseJson.state) {
                                const data = responseJson.object
                                $("#imgLogo").attr("src", data.urlLogo)
                                swal("Listo!", "La Informacion fue Actualizada!!", "success")

                            }
                            else {
                                swal("Lo sentimos!", responseJson.message, "error")
                            }
                        })
                }
            }
        )
    }
})