$(document).ready(function () {

    $("div.container-fluid").LoadingOverlay("show");
    fetch("/DashBoardApplication/GeneralDataForTheDashBoard")
        .then(response => {
            $("div.container-fluid").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.state) {

                //MOSTRAR DATOS PARA LAS TARJETAS
                let data = responseJson.object

                $("#totalVenta").text(data.totalSales)
                $("#totalIngresos").text(data.totalRevenues)
                $("#totalProductos").text(data.totalProducts)
                $("#totalCategorias").text(data.totalCategorys)

                //OBTENER LOS TEXTOS Y VALORES PARA EL GRAFICO DE BARRAS
                let barchart_lables;
                let barchart_data;

                if (data.weekendSales.length > 0) {
                    barchart_lables = data.weekendSales.map((item) => { return item.date })
                    barchart_data = data.weekendSales.map((item) => { return item.total })
                }
                else {
                    barchart_lables = ["sin resultados"]
                    barchart_data = [0]
                }

                //OBTENER LOS TEXTOS Y VALORES PARA EL GRAFICO DE PYE

                let pieChart_lables;
                let pieChart_data;

                if (data.productsByWeek.length > 0) {
                    pieChart_lables = data.productsByWeek.map((item) => { return item.product })
                    pieChart_data = data.productsByWeek.map((item) => { return item.quantity })
                }
                else {
                    pieChart_lables = ["sin resultados"]
                    pieChart_data = [0]
                }


                //CONFIGURAR PARA PINTAR LOS DATOS OBTENIDOS EN EL GRAFICO DE BARRAS
                let controlVenta = document.getElementById("chartVentas");
                let myBarChart = new Chart(controlVenta, {
                    type: 'bar',
                    data: {
                        labels: barchart_lables,
                        datasets: [{
                            label: "Cantidad",
                            backgroundColor: "#4e73df",
                            hoverBackgroundColor: "#2e59d9",
                            borderColor: "#4e73df",
                            data: barchart_data,
                        }],
                    },
                    options: {
                        maintainAspectRatio: false,
                        legend: {
                            display: false
                        },
                        scales: {
                            xAxes: [{
                                gridLines: {
                                    display: false,
                                    drawBorder: false
                                },
                                maxBarThickness: 50,
                            }],
                            yAxes: [{
                                ticks: {
                                    min: 0,
                                    maxTicksLimit: 5
                                }
                            }],
                        },
                    }
                });


                 //CONFIGURAR PARA PINTAR LOS DATOS OBTENIDOS EN EL GRAFICO DE PIE
                let controlProducto = document.getElementById("charProductos");
                let myPieChart = new Chart(controlProducto, {
                    type: 'doughnut',
                    data: {
                        labels: pieChart_lables,
                        datasets: [{
                            data: pieChart_data,
                            backgroundColor: ['#4e73df', '#1cc88a', '#36b9cc', "#FF785B"],
                            hoverBackgroundColor: ['#2e59d9', '#17a673', '#2c9faf', "#FF5733"],
                            hoverBorderColor: "rgba(234, 236, 244, 1)",
                        }],
                    },
                    options: {
                        maintainAspectRatio: false,
                        tooltips: {
                            backgroundColor: "rgb(255,255,255)",
                            bodyFontColor: "#858796",
                            borderColor: '#dddfeb',
                            borderWidth: 1,
                            xPadding: 15,
                            yPadding: 15,
                            displayColors: false,
                            caretPadding: 10,
                        },
                        legend: {
                            display: true
                        },
                        cutoutPercentage: 80,
                    },
                });
            }
          
        })

})