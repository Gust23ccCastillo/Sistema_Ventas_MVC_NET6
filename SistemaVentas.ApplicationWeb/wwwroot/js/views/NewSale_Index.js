
const _viewModelSaleDto = {
    idSale: 0,
    salesNumber: "",
    idTypeOfDocumentSale: 0,
    idUser: 0,
    clientDocument: "",
    clientName: "",
    subTotal: "",
    totalTax: "",
    total: "",
    SaleDetails: []
}


let TaxValue = 0;
$(document).ready(function () {
  

    fetch("/SalesApplication/TypeSaleDocumentList")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            console.log(responseJson);
            if (responseJson.length > 0) {
                responseJson.forEach((data) => {

                    $("#cboTipoDocumentoVenta").append(
                        $("<option>").val(data.idTypeOfDocumentSale).text(data.descriptions)
                    )
                })
            }
        })

    fetch("/BusinessApplication/GetBusinessInformation")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.state) {
                const informationByBusiness = responseJson.object;
                //console.log(informationByBusiness);
                $("#inputGroupSubTotal").text(`Sub Total - ${informationByBusiness.coin}`)
                $("#inputGroupIGV").text(`Impuesto(${informationByBusiness.percentageTax})% - ${informationByBusiness.coin}`)
                $("#inputGroupTotal").text(`Total - ${informationByBusiness.coin}`)
       
                TaxValue = parseFloat(informationByBusiness.percentageTax);
                //console.log(TaxValue);
            }
        })

    $("#cboBuscarProducto").select2({
        ajax: {
            url: "/SalesApplication/ObtainProducts",
            dataType: 'json',
            contentType: "application/json;charset=utf-8",
            delay: 250,
            data: function (params) {
                return {
                    searchProducts: params.term
                };
            },
            processResults: function (data) {
                //FORMA EN COMO SE RETORNA LA INFORMACION DEL PRODUCTO
                return {
                    results: data.map((item) => (
                        {
                            //POR DEFECTO EL SELECT2 LLEVA UN id y text, despues agregamos a nuestro gusto
                            id: item.idProduct,
                            text: item.descriptions,

                            mark: item.mark,
                            category: item.idCategoria,
                            urlImage: item.urlImage,
                            price: parseFloat(item.price)

                        }

                    ))

                };
            }

        },
        language: "es",
        placeholder: 'Buscar Producto',
        minimumInputLength: 1,
        templateResult: DisplayProductInformation,

    });

    //FUNCION PARA MOSTRAR COMO QUEREMOS VER LOS DATOS
    function DisplayProductInformation(data) {
        //esto es por defecto para que muestre el "buscando"
        if (data.loading)
            return data.text;

        var contenedor = $(
            `<table width="100%">
           <tr>
             <td style="width:60px">
             <img style="helght:60px;width:60px;margin-right:10px" src="${data.urlImage}"/>
             </td>
             <td>
               <p style="font-weight:bolder;margin:2px">${data.mark}</p>
              <p style="margin:2px">${data.text}</p>
                
             </td>
           
           </tr>
         
         
         </table>`
        );

        return contenedor;
    }

    //FUNCION PARA MOSTRAER EL MODAL A LA HORA DE SELECCIONAR UN PRODUCTO Y SELECIONAR CUANTOS QUEREMOS
    //DEL MISMO
    $(document).on("select2:open", function () {
        document.querySelector(".select2-search__field").focus();
    })
    let ProductsForSale = [];
    $("#cboBuscarProducto").on("select2:select", function (e) {
        const data = e.params.data;
        console.log(ProductsForSale);

        //VALIDAR SI UN MISMO PRODUCTO YA FUE AGREGADO A LA VENTA
        let productsFound = ProductsForSale.filter(search => search.idProduct == data.id);
        if (productsFound.length > 0) {
            $("#cboBuscarProducto").val("").trigger("change")
            toastr.warning("", "El producto ya fue 'AGREGADO'")
            return false;
        }
        else {
            swal({
                title: data.mark,
                text: data.text,
                ImageUrl: data.urlImage,
                type: "input",
                showCancelButton: true,
                closeOnConfirm: false,
                inputPlaceholder: "Ingrese la Cantidad"

            },
                function (valor) {
                    if (valor === false) return false;

                    if (valor === "") {
                        toastr.warning("", "Necesita Ingresar la Cantidad")
                        return false;
                    }
                    if (isNaN(parseInt(valor))) {
                        toastr.warning("", "Necesita Ingresar un Valor Numerico")
                        return false;
                    }

                    let SaleDetails = {
                        idProduct: data.id,
                        productMark: data.mark,
                        productDescription: data.text,
                        productCategory: data.category,
                        quantity: parseInt(valor),
                        price: data.price.toString(),
                        total: (parseFloat(valor) * data.price).toString()

                    }
                    //AGREGAMOS EL PRODUCTO AL ARRAY Y LUEGO LIMPIAMOS NUESTRO DESPLEGABLE DE PRODUCTOS
                    ProductsForSale.push(SaleDetails)

                    ShowProductsToBuy();

                    $("#cboBuscarProducto").val("").trigger("change")
                    swal.close()
                }



            )
        }



    })


    //MOSTRAR TODOS LOS PRODUCTOS A COMPRAR YA SELECCIONADOS Y EL TOTOAL A PAGAR
    function ShowProductsToBuy() {
        let total = 0;
        let igv = 0;
        let subTotal = 0;
        let percetangeTax = TaxValue / 100;


        $("#tbProducto tbody").html("")

        ProductsForSale.forEach((item) => {
            total = total + parseFloat(item.total);
            $("#tbProducto tbody").append(
                $("<tr>").append(
                    $("<td>").append(
                        $("<button>").addClass("btn btn-danger btn-eliminar btn-sm").append(
                            $("<i>").addClass("fas fa-trash-alt")
                        ).data("idProduct", item.idProduct)
                    ),
                    $("<td>").text(item.productDescription),
                    $("<td>").text(item.productMark),
                    $("<td>").text(item.quantity),
                    $("<td>").text(item.price),
                    $("<td>").text(item.total)



                )
            )
        })

        subTotal = total / (1 + percetangeTax);
        igv = total - subTotal;

        $("#txtSubTotal").val(subTotal.toFixed(2));
        $("#txtIGV").val(igv.toFixed(2));
        $("#txtTotal").val(total.toFixed(2));

    }

    //FUNCIO PARA ELIMINAR LOS PRODUCTOS YA SELECIONADOS 
    $(document).on("click", "button.btn-eliminar", function () {
        const _idProductDelete = $(this).data("idProduct")
        ProductsForSale = ProductsForSale.filter(search => search.idProduct != _idProductDelete);
        ShowProductsToBuy();
    })

    //FUNCION PARA REALIZAR LA VENTA DE COMPRA
    $("#btnTerminarVenta").click(function () {
        if (ProductsForSale.length < 1) {
            toastr.warning("", "Para realizar la compra debe ingresar algun producto!!")
            return;
        }

        //const saleDetails = ProductsForSale;
        const data = structuredClone(_viewModelSaleDto);
        data["idTypeOfDocumentSale"] = parseInt($("#cboTipoDocumentoVenta").val())
        data["clientDocument"] = $("#txtDocumentoCliente").val()
        data["clientName"] = $("#txtNombreCliente").val()
        data["subTotal"] = $("#txtSubTotal").val()
        data["totalTax"] = $("#txtIGV").val()
        data["total"] = $("#txtTotal").val()
        data["SaleDetails"] = ProductsForSale
        //data = ProductsForSale[]


      
        console.log(JSON.stringify(data, null, 2));

        $("#btnTerminarVenta").LoadingOverlay("show");
        fetch("/SalesApplication/SaleRegister", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        }).then(response => {
                $("#btnTerminarVenta").LoadingOverlay("hide");
                return response.ok ? response.json() : Promise.reject(response);

        }).then(responseJson => {
                if (responseJson.state) {
                    ProductsForSale = [];
                    ShowProductsToBuy();

                    $("#cboTipoDocumentoVenta").val($("#cboTipoDocumentoVenta option:first").val())
                    $("#txtDocumentoCliente").val("")
                    $("#txtNombreCliente").val("")

                    swal("Registrado!", `Numero de Venta: ${responseJson.object.salesNumber}`, "success")
                } else {
                    swal("Lo sentimos!", "No se Pudo Registrar la Venta", "error")
                }
            })
           
           

    })

})
