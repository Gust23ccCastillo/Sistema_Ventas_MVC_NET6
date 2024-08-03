//FUNCION DONDE SI SELECCIONO BUSCAR POR FECHA ESCONDA EL CAMPO DE NUMERO DE 
//VENTA Y EN CASO DE SELECIONAR POR NUMERO DE VENTA ESCONDA LOS CAMPOS DE FEHCAS
const view_Search = {
    search_Datetime: () => {
        $("#txtFechaInicio").val("")
        $("#txtFechaFin").val("")
        $("#txtNumeroVenta").val("")

        $(".busqueda-fecha").show()
        $(".busqueda-venta").hide()
    },

     search_Sale: () => {
        $("#txtFechaInicio").val("")
        $("#txtFechaFin").val("")
        $("#txtNumeroVenta").val("")

         $(".busqueda-fecha").hide()
         $(".busqueda-venta").show()
    }
}

//CUANDO CARGA TODO EL DOCUMENTO POR DEFECTO 
$(document).ready(function () {
    view_Search["search_Datetime"]()
    $.datepicker.setDefaults($.datepicker.regional["es"])


    $("#txtFechaInicio").datepicker({dateFormat:"dd/MM/yy"})
    $("#txtFechaFin").datepicker({ dateFormat: "dd/MM/yy" })
})

//CAMBIO AL SELECCIONAR SI BUSCAR POR FECHAS O NUMERO DE VENTA
$("#cboBuscarPor").change(function () {
    if ($("#cboBuscarPor").val() == "fecha") {
        view_Search["search_Datetime"]()
    }
    else {
        view_Search["search_Sale"]()
    }
})

//FUNCION PARA BUSCAR Y MOSTRAR LOS REGISTROS DE VENTAS YA SEA POR NUMERO DE VENTA
// O POR FECHAS SEGUN SU SELECCION
$("#btnBuscar").click(function () {
    if ($("#cboBuscarPor").val() == "fecha") {
        if ($("#txtFechaInicio").val().trim() == "" ||
            $("#txtFechaFin").val().trim() == "") {
            toastr.warning("", "Debe Ingresar una Fecha de Inicio y Fecha de Fin!!")
            return
        }
    }
    else {
        if ($("#txtNumeroVenta").val().trim() == ""){
            toastr.warning("", "Debe Ingresar un Numero de Venta!!")
            return
        }
    }
    let _saleNumber = $("#txtNumeroVenta").val()
    let _startDate = $("#txtFechaInicio").val()
    let _endDate = $("#txtFechaFin").val()

    $(".card-body").find("div.row").LoadingOverlay("show");
    fetch(`/SalesApplication/SalesHistoryInformation?saleNumber=${_saleNumber}&startDate=${_startDate}&endDate=${_endDate}`)
        .then(response => {
            $(".card-body").find("div.row").LoadingOverlay("hide");
            if (response.ok == false) {
                swal("Lo sentimos!", "No se encuentran registros de ventas para esas fechas!!", "error")
               
            }
            return response.json();
        })
        .then(responseJson => {

            $("#tbventa tbody").html("");
            if (responseJson.length > 0) {
                responseJson.forEach((sale) => {
                    $("#tbventa tbody").append(

                        $("<tr>").append(
                            $("<td>").text(sale.registrationDate),
                            $("<td>").text(sale.salesNumber),
                            $("<td>").text(sale.typeOfoDocumentSale),
                            $("<td>").text(sale.clientDocument),
                            $("<td>").text(sale.clientName),
                            $("<td>").text(sale.total),
                            $("<td>").append(
                                $("<button>").addClass("btn btn-info btn-sm").append(
                                    $("<i>").addClass("fas fa-eye")
                                ).data("sale", sale)
                            )
                        )
                    )
                })
            }
            else {
                swal("Lo sentimos!", "No se encuentran registros de ventas para esas fechas!!", "error")
                
            }
        })
})

//FUNCION EL CUAL A DARLE EL CLICK AL ICONO DE OJO PARA VER MAS A DETALLE
// LA VENTA REGISTRADA Y MOSTRAR LA INFORMACION AMPLIA EN UN MODAL
$("#tbventa tbody").on("click", ".btn-info", function () {
    let information = $(this).data("sale")

    $("#txtFechaRegistro").val(information.registrationDate)
    $("#txtNumVenta").val(information.salesNumber)
    $("#txtUsuarioRegistro").val(information.userProfile)
    $("#txtTipoDocumento").val(information.typeOfoDocumentSale)
    $("#txtDocumentoCliente").val(information.clientDocument)
    $("#txtNombreCliente").val(information.clientName)
    $("#txtSubTotal").val(information.subTotal)
    $("#txtIGV").val(information.totalTax)
    $("#txtTotal").val(information.total)


    //LIMPIAR LA TABLA DE PRODUCTOS AL SELECCIONAR LA VENTA DETALLADA
    // Y MOSTRAR EL PRODUCTOS O LOS PRODUCTOS DE LA VENTA SEGUN CORRESPONDA
    $("#tbProductos tbody").html("");

    information.saleDetails.forEach((item) => {
        $("#tbProductos tbody").append(

            $("<tr>").append(
                $("<td>").text(item.productDescription),
                $("<td>").text(item.quantity),
                $("<td>").text(item.price),
                $("<td>").text(item.total),
              
            )
        )
    })
    //CONFIGURACION PARA QUE SE REDIRIJA A IMPRIMIR VENTA POR PDF
    $("#linkImprimir").attr("href", `PrintPDFSale?numberSale=${information.salesNumber}`)
    $("#modalData").modal("show");
})