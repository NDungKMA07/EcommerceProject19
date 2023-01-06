using Microsoft.AspNetCore.Mvc;
using Project_aspnet_19_DevPro.Areas.Admin.Attributes;

namespace Project_aspnet_19_DevPro.Areas.Admin.Controllers
{
    [Area("Admin")]
    //goi Attribute CheckLogin (nam trong folder Areas/Admin/Attributes)
    [CheckLogin]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
