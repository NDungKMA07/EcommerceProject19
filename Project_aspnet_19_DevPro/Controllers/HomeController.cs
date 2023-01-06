using Microsoft.AspNetCore.Mvc;

namespace Project_aspnet_19_DevPro.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("/Admin");
        }
    }
}
