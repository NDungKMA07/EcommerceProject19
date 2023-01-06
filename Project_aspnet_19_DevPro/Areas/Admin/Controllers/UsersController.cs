using Microsoft.AspNetCore.Mvc;
//thao tac voi IFormCollection
using Microsoft.AspNetCore.Http;
//doi tuong ma hoa password -> co the gan vao mot bien de su dung bien nay
using BC = BCrypt.Net.BCrypt;
//de nhin thay cac class trong folder Models thi phai using dong duoi

//doi tuong phan trang
using X.PagedList;
//su dung kieu List
using System.Collections.Generic;
//de nhin thay file CheckLogin.cs trong thu muc Attributes
using Project_aspnet_19_DevPro.Areas.Admin.Attributes;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Project_aspnet_19_DevPro.Models;

namespace Project_aspnet_19_DevPro.Areas.Admin.Controllers
{
    [Area("Admin")]
    //Gọi Attribute CheckLogin để kiểm tra đăng nhập
    [CheckLogin]
    public class UsersController : Controller
    {
        //đối tượng thao tác csdl
        public MyDbConnect db = new MyDbConnect();
        //các biến khai báo bên ngoài hàm (của class) gọi là biến toàn cục, có thể sử dụng được ở tất cả các hàm trong class
        //int? page -> nếu có giá trị truyền vào hàm thì page = giatri, nếu không có giatri truyền vào hàm thì page = null
        public IActionResult Index(int? page)
        {
            //lấy trang hiện tại
            int current_page = page ?? 1;
            //định nghĩa số bản ghi trên một trang
            int record_per_page = 4;
            //lấy tất cả các bản ghi trong table Users
            List<ItemUser> list_record = db.Users.OrderByDescending(item => item.Id).ToList();
            //truyền giá trị ra view có phân trang
            return View("Index", list_record.ToPagedList(current_page, record_per_page));
        }
        public IActionResult InfoUser() {
			
			string StrUserRecord = HttpContext.Session.GetString("userInfo");
			ItemUser UserItem = JsonConvert.DeserializeObject<ItemUser>(StrUserRecord);
			return View("InfoUser", UserItem);

			

		}
		public IActionResult UpdateProfile()
		{
			string StrUserRecord = HttpContext.Session.GetString("userInfo");
			ItemUser UserItem = JsonConvert.DeserializeObject<ItemUser>(StrUserRecord);
            int _id = UserItem.Id;
			//lay mot ban ghi
			ItemUser record = db.Users.Where(item => item.Id == _id).FirstOrDefault();
			//tạo biến action để đưa vào thuộc tính action của thẻ form
			ViewBag.action = "/Admin/Users/UpdatePost/" + _id;
			//gọi view, truyền dữ liệu ra view
			return View("CreateUpdate", record);
		}

		//mặc định trạng thái của trang là GET
		public IActionResult Update(int? id)
        {
            int _id = id ?? 0;
            //lay mot ban ghi
            ItemUser record = db.Users.Where(item => item.Id == _id).FirstOrDefault();
            //tạo biến action để đưa vào thuộc tính action của thẻ form
            ViewBag.action = "/Admin/Users/UpdatePost/" + _id;
            //gọi view, truyền dữ liệu ra view
            return View("CreateUpdate", record);
        }
        //khi ấn nút submit thì trạng thái của trang sẽ là POST -> khai báo Attribute [HttpPost]
        [HttpPost]
        public IActionResult UpdatePost(IFormCollection fc, int? id)
        {
            //lấy dữ liệu của thẻ form thông qua đối tượng fc
            string _name = fc["name"].ToString().Trim();
            //cũng có thể sử dụng đối tượng Request.Form["ten the form"]
            string _password = Request.Form["password"].ToString().Trim();
            //lấy một bản ghi
            ItemUser record = db.Users.Where(item => item.Id == id).FirstOrDefault();
            if (record != null)
            {
                record.Name = _name;
                //nếu password không rỗng thì update password
                if (!String.IsNullOrEmpty(_password))
                {
                    //mã hóa password
                    _password = BC.HashPassword(_password);
                    record.Password = _password;
                }
            }
            //cập nhật lại table
            db.SaveChanges();
            //di chuyển đến action có tên là Index
            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            //tạo biến action để đưa vào thuộc tính action của thẻ form
            ViewBag.action = "/Admin/Users/CreatePost";
            return View("CreateUpdate");
        }
        [HttpPost]
        public IActionResult CreatePost(IFormCollection fc)
        {
            //lấy dữ liệu của thẻ form thông qua đối tượng fc
            string _name = fc["name"].ToString().Trim();
            string _email = fc["email"].ToString().Trim();
            //cũng có thể sử dụng đối tượng Request.Form["ten the form"]
            string _password = Request.Form["password"].ToString().Trim();
            //mã hóa password
            _password = BC.HashPassword(_password);
            //lay mot ban ghi
            ItemUser record = new ItemUser();
            record.Name = _name;
            record.Email = _email;
            record.Password = _password;
            //them ban ghi vao table Users
            db.Users.Add(record);
            db.SaveChanges();
            //di chuyển đến action có tên là Index
            return RedirectToAction("Index");
        }
        //xoa ban ghi
        public IActionResult Delete(int? id)
        {
            int _id = id ?? 0;
            //lay mot ban ghi
            ItemUser record = db.Users.Where(item => item.Id == _id).FirstOrDefault();
            //xoa ban ghi khoi csdl
            db.Users.Remove(record);
            //cap nhat lai table Users
            db.SaveChanges();
            //di chuyển đến action có tên là Index
            return RedirectToAction("Index");
        }
    }
}
