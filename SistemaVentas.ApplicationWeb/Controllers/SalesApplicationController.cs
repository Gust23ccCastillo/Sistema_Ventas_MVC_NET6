using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using SistemaVentas.ApplicationWeb.Models.ViewModelDtos.BussineDtos;
using SistemaVentas.ApplicationWeb.Models.ViewModelDtos.ProductDtos;
using SistemaVentas.ApplicationWeb.Models.ViewModelDtos.SalesDtos;
using SistemaVentas.ApplicationWeb.Profits.Response;
using SistemaVentas.BusinessLogic.Interface;
using SistemaVentas.Entities;
using SkiaSharp;

namespace SistemaVentas.ApplicationWeb.Controllers
{
   
    public class SalesApplicationController : Controller
    {
        private readonly IServicesSales servicesSalesInject;
        private readonly IServicesBusiness _servicesBusiness;
        private readonly IMapper _autoMapperInject;
        private readonly IServicesSaleType _servicesSaleTypeInject;
        

        public SalesApplicationController(IServicesSales servicesSalesInject, 
            IServicesBusiness servicesBusiness, IMapper autoMapperInject,
            IServicesSaleType servicesSaleTypeInject
           )
        {
            this.servicesSalesInject = servicesSalesInject;
            _servicesBusiness = servicesBusiness;
            _autoMapperInject = autoMapperInject;
            _servicesSaleTypeInject = servicesSaleTypeInject;
           
        }

        public IActionResult NewSale()
        {
            return View();
        }


        public IActionResult SalesHistory()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> TypeSaleDocumentList()
        {
            List<ViewModelTypeOfDocumentSaleDto> modelTypeDocumentSale = this._autoMapperInject.Map<List<ViewModelTypeOfDocumentSaleDto>>(
                await this._servicesSaleTypeInject.ListOfTypeSales());
            return StatusCode(StatusCodes.Status200OK, modelTypeDocumentSale);
        }

        [HttpGet]
        public async Task<IActionResult> ObtainProducts(string searchProducts)
        {
            List<ViewModelProductDto> ListProductsSearch = this._autoMapperInject.Map<List<ViewModelProductDto>>(
                await this.servicesSalesInject.ObtainListProducts(searchProducts));
            return StatusCode(StatusCodes.Status200OK, ListProductsSearch);
        }

        [HttpPost]
        public async Task<IActionResult> SaleRegister()
        {
           using var reader = new StreamReader(Request.Body);
           var body = await reader.ReadToEndAsync();
           ViewModelSaleDto sale = JsonConvert.DeserializeObject<ViewModelSaleDto>(body);

            GenericResponse<ViewModelSaleDto> responseToUser = new ();
            if(sale == null)
            {
                responseToUser.State = false;
                responseToUser.Message = "Problemas al crear la venta!!,Profavor Intentalo mas tarde";
            }
            else
            {
                try
                {
                    //TEMPORAL
                    sale.IdUser = 10;
                    sale.TypeOfoDocumentSale = "";
                    sale.SalesNumber = "";
                    sale.UserProfile = "";
                    sale.RegistrationDate = DateTime.Now.ToString("dd/MM/yyyy");

                    Sale saleMapperModel = this._autoMapperInject.Map<Sale>(sale);
                    var ResponseToCreateSale = await this.servicesSalesInject.RegisterSaleApplication(saleMapperModel);
                    if (ResponseToCreateSale != null)
                    {
                        responseToUser.State = true;
                        responseToUser.Object = this._autoMapperInject.Map<ViewModelSaleDto>(ResponseToCreateSale);
                    }
                    else
                    {
                        responseToUser.State = false;
                        responseToUser.Message = "Problemas al crear la venta!!,Profavor Intentalo mas tarde";
                    }
                }
                catch(Exception ex)
                {
                    
                    responseToUser.State = false;
                    responseToUser.Message = "Problemas al crear la venta!!,Profavor Intentalo mas tarde  " + ex.Message;
                }
            }

           
            return StatusCode(StatusCodes.Status200OK, responseToUser);
        }

        [HttpGet]
        public async Task<IActionResult> SalesHistoryInformation(string saleNumber,string startDate, string endDate)
        {
            List<ViewModelSaleDto> salesHistory = this._autoMapperInject.Map<List<ViewModelSaleDto>>(
                await this.servicesSalesInject.SalesHistory(saleNumber, startDate, endDate));


            return StatusCode(StatusCodes.Status200OK, salesHistory);
        }

     

        [HttpGet]
        public async Task<IActionResult> PrintPDFSale(string numberSale)
        {
            ViewModelSaleDto modelSaleDto = this._autoMapperInject.Map<ViewModelSaleDto>(
                await this.servicesSalesInject.GetDetail_Of_A_SpecificSale(numberSale));
            

            ViewModelBusinessDto modelBusinessDto = this._autoMapperInject.Map<ViewModelBusinessDto>(
                await this._servicesBusiness.GetApplicationBusiness());

            //ESTO ES PARA DECARGAR LA IMAGEN DE EL LOGO DE LA EMPRESA Y PODER IMPRIMIRLO EN EL PDF
            var imageLogBusinessData = await DownloadImageLogoBusinessAsync(modelBusinessDto.UrlLogo);
           

            var data = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                   
                    page.Header().ShowOnce().Row(row =>
                    {
                        //IMAGEN DE LA EMPRESA
                      
                        row.ConstantItem(150).Image(imageLogBusinessData);

                        //INFORMACION DE LA EMPRESA
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignCenter().Text(modelBusinessDto.NameBusiness).Bold().FontSize(14);
                            col.Item().AlignCenter().Text(modelBusinessDto.AddressBusiness).FontSize(9);
                            col.Item().AlignCenter().Text(modelBusinessDto.Phone).FontSize(9);
                            col.Item().AlignCenter().Text(modelBusinessDto.Email).FontSize(9);
                           
                        });

                        //INFORMACION DE VENTA COMO NUMERO DE VENTA,REGISTRO Y TIPO DE DOCUMENTO
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text(modelSaleDto.RegistrationDate);
                            col.Item().Background("#257272").Border(1).BorderColor("#257272").AlignCenter().Text(modelSaleDto.SalesNumber).FontColor("#fff");
                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text(modelSaleDto.TypeOfoDocumentSale);


                        });

                    });

                    page.Content().PaddingVertical(10).Column(columInfoCliente =>
                    {
                        columInfoCliente.Item().Column(col =>
                        {
                            col.Item().Text("Datos del Cliente").Underline().Bold();
                            col.Item().Text(text =>
                            {
                                text.Span("Nombre: ").SemiBold().FontSize(10);
                                text.Span(modelSaleDto.ClientName).FontSize(10);
                            });

                            col.Item().Text(text =>
                            {
                                text.Span("Cedula: ").SemiBold().FontSize(10);
                                text.Span(modelSaleDto.ClientDocument).FontSize(10);
                            });

                            col.Item().Text(text =>
                            {
                                text.Span("Factura Efectuada por: ").SemiBold().FontSize(10);
                                text.Span(modelSaleDto.UserProfile).FontSize(10);
                            });
                        });
                      

                        //LINEA HORIZONTAL
                        columInfoCliente.Item().LineHorizontal(0.5f);


                        //SECCION DE LA TABLA PARA LA INFORMACION DE LOS PRODUCTOS COMPRADOS
                        columInfoCliente.Item().Table(tableInfoForSale =>
                        {
                            tableInfoForSale.ColumnsDefinition(colums =>
                            {
                                colums.RelativeColumn(3);
                                colums.RelativeColumn();
                                colums.RelativeColumn();
                                colums.RelativeColumn();
                              
                            });

                            tableInfoForSale.Header(headerInfo =>
                            {
                                headerInfo.Cell().Background("#257272")
                                .Padding(2).Text("Producto").FontColor("#fff");

                                headerInfo.Cell().Background("#257272")
                                .Padding(2).Text("Descripcion").FontColor("#fff");

                                headerInfo.Cell().Background("#257272")
                                .Padding(2).Text("Cantidad").FontColor("#fff");

                                headerInfo.Cell().Background("#257272")
                                .Padding(2).AlignRight().Text("Precio c/u").FontColor("#fff");

                            });

                            foreach (var item in modelSaleDto.SaleDetails)
                            {
                                tableInfoForSale.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(item.ProductMark).FontSize(10);

                                tableInfoForSale.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                              .Padding(2).Text(item.ProductDescription).FontSize(10);

                                tableInfoForSale.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                              .Padding(2).Text(item.Quantity).FontSize(10);

                                tableInfoForSale.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                              .Padding(2).AlignRight().Text($"{modelBusinessDto.Coin} {item.Price}").FontSize(10);
                            }
                        });

                        columInfoCliente.Item().AlignRight().Text($"Sub Total: {modelBusinessDto.Coin} {modelSaleDto.SubTotal}").FontSize(12);
                        columInfoCliente.Item().AlignRight().Text($"Impuesto: {modelBusinessDto.Coin} {modelSaleDto.TotalTax}").FontSize(12);
                        columInfoCliente.Item().AlignRight().Text($"Total: {modelBusinessDto.Coin} {modelSaleDto.Total}").FontSize(12);


                        //SECCION DE COMENTARIO DEL PDF
                        columInfoCliente.Item().Background(Colors.Grey.Lighten3).Padding(3)
                        .Column(columComentary =>
                        {
                            columComentary.Item().Text("Comentario").FontSize(14);
                            columComentary.Item().Text($"Gracias por Comprar en Almacenes: {modelBusinessDto.NameBusiness}, Te deseamos un excelente dia  y que disfrutes nuestras compras!!");
                            columComentary.Spacing(5);
                        });

                        columInfoCliente.Spacing(10);
                    });

                    //SECCION DE EL FOOTER 
                    page.Footer()
                    .AlignRight()
                    .Text(text =>
                    {
                        text.Span("Pagina ").FontSize(10);
                        text.CurrentPageNumber().FontSize(10);
                        text.Span("de ").FontSize(10);
                        text.TotalPages().FontSize(10);
                    });
                });
            }).GeneratePdf();

            Stream stream = new MemoryStream(data);
            return File(stream, "application/pdf", "DetalleVenta.pdf");
        }

        private async Task<byte[]> DownloadImageLogoBusinessAsync(string imageUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                return await client.GetByteArrayAsync(imageUrl);
            }
        }


    }
}
