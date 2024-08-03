//FECHA A SELECCIONAR CUANDO CARGA EL DOCUMENTO QUE TENGA EL FORMATO dd/MM/yyyy
// Y ADEMAS SE EN ESPANOL

let TableData;
$(document).ready(function () {
    $.datepicker.setDefaults($.datepicker.regional["es"])

    $("#txtFechaInicio").datepicker({ dateFormat: "dd/MM/yy" })
    $("#txtFechaFin").datepicker({ dateFormat: "dd/MM/yy" })

    TableData = $('#tbdata').DataTable({
        responsive: true,
        "ajax": {
            "url": '/SalesReportApplication/SaleReport?startDate=01/01/1991&endDate=01/01/1991',
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "registrationDate" },
            { "data": "numberSale" },
            { "data": "typeOfDocument" },
            { "data": "clientDocument" },
            { "data": "clientName" },
            { "data": "subTotal" },
            { "data": "totalTax" },
            { "data": "totalSale" },
            { "data": "product" },
            { "data": "description" },
            { "data": "quantity" },
            { "data": "price" },
            { "data": "total" },
            
           
        ],
        order: [[0, "desc"]],
        dom: "Bfrtip",
        buttons: [
            {
                text: 'Exportar Excel',
                extend: 'excelHtml5',
                title: '',
                filename: 'Reporte Ventas',
                exportOptions: {
                    columns: [0,1,2,3,4,5,6,7,8,9,10,11,12]
                }
            }, 'pageLength'
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });
})

$("#btnBuscar").click(function () {
   
    if ($("#txtFechaInicio").val().trim() == "" ||
        $("#txtFechaFin").val().trim() == "") {
        toastr.warning("", "Debe Ingresar una Fecha de Inicio y Fecha de Fin!!")
        return
    }
    else {
        let _startDate = $("#txtFechaInicio").val().trim();
        let _endDate = $("#txtFechaFin").val().trim();

        let GetSpecificInfoSaleDetails = `/SalesReportApplication/SaleReport?startDate=${_startDate}&endDate=${_endDate}`;
        TableData.ajax.url(GetSpecificInfoSaleDetails).load();
    }
    
})
