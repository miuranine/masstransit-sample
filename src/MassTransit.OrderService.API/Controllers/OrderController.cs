using Microsoft.AspNetCore.Mvc;

namespace MassTransit.OrderService.API.Controllers
{
    public class OrderController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}