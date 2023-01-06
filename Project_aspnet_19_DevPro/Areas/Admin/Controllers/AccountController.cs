using Microsoft.AspNetCore.Mvc;
//thao tac voi IFormCollection
using Microsoft.AspNetCore.Http;
//doi tuong ma hoa password -> co the gan vao mot bien de su dung bien nay
using BC = BCrypt.Net.BCrypt;
using Project_aspnet_19_DevPro;
using Microsoft.EntityFrameworkCore;
using Project_aspnet_19_DevPro.Models;
using Newtonsoft.Json;

namespace Project_aspnet_19_DevPro.Areas.Admin.Controllers
{
    [Area("Admin")]//Attribute
    public class AccountController : Controller
    {
        //đối tượng thao tác csdl
        public MyDbConnect db = new MyDbConnect();
        public IActionResult Login()
        {
            return View("Login");
        }
        //sau khi an nut submit
        [HttpPost]
        public IActionResult LoginPost(IFormCollection fc)
        {
            //ham Trim() de loai bo khoang trang ben trai va ben phai cua chuoi
            string _email = fc["email"].ToString().Trim();
            string _password = fc["password"].ToString().Trim();
            //lấy một bản ghi tương ứng với email truyền vào
            ItemUser record = db.Users.Where(item => item.Email == _email).FirstOrDefault();
            if (record != null)
            {
                if (BC.Verify(_password, record.Password) == true)
                {
                    //khoi tao bien session
                    HttpContext.Session.SetString("admin_email", _email);
					//lay id cua record nay
					string UserJson = JsonConvert.SerializeObject(record);
					HttpContext.Session.SetString("userInfo", UserJson);
					HttpContext.Session.SetString("admin_id", record.Id.ToString());
                    //di chuyen den url: /Admin/Home
                    return Redirect("/Admin/Home");
                }
            }
            return Redirect("/Admin/Account/Login?notify=invalid");
        }
        //dang xuat
        public IActionResult Logout()
        {
            //huy bien session admin_email
            HttpContext.Session.Remove("admin_email");
            return Redirect("/Admin/Account/Login");
        }
    }
}
