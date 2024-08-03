using Microsoft.AspNetCore.Mvc;

namespace SistemaVentas.ApplicationWeb.Controllers
{
    public class TemplatesForServicesController : Controller
    {
        
        public IActionResult SendUserPassword(string EmailParameter, string PasswordParameter)
        {
            ViewData["email"]= EmailParameter;
            ViewData["password"]= PasswordParameter;
            ViewData["url"] = $"{this.Request.Scheme}://{this.Request.Host}";
            return View();
        }

        public IActionResult ResetUserPassword(string PasswordParameter)
        {
            ViewData["password"] = PasswordParameter;
            return View();
        }
    }
}
