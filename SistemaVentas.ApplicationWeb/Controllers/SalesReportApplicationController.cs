using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SistemaVentas.ApplicationWeb.Models.ViewModelDtos.SalesDtos;
using SistemaVentas.Dal.Interface;

namespace SistemaVentas.ApplicationWeb.Controllers
{
    public class SalesReportApplicationController : Controller
    {
        private readonly IMapper _automapperInject;
        private readonly ISalesRepository _salesRepositoryInject;

        public SalesReportApplicationController(IMapper automapperInject, ISalesRepository salesRepositoryInject)
        {
            _automapperInject = automapperInject;
            _salesRepositoryInject = salesRepositoryInject;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SaleReport(string startDate, string endDate)
        {
            List<ViewModelReportSaleDto> modelSaleReport = this._automapperInject.Map<List<ViewModelReportSaleDto>>(
                await this._salesRepositoryInject.ReportSale(Convert.ToDateTime(startDate), Convert.ToDateTime(endDate)));

            return StatusCode(StatusCodes.Status200OK, new { data = modelSaleReport });
        }
    }
}
