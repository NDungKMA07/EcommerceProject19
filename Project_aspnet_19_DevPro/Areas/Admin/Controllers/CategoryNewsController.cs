using Microsoft.AspNetCore.Mvc;
//thao tac voi IFormCollection
using Microsoft.AspNetCore.Http;
//doi tuong ma hoa password -> co the gan vao mot bien de su dung bien nay
using BC = BCrypt.Net.BCrypt;
//de nhin thay cac class trong folder Models thi phai using dong duoi
using Project_aspnet_19_DevPro.Models;
//doi tuong phan trang
using X.PagedList;
//su dung kieu List
using System.Collections.Generic;
//de nhin thay file CheckLogin.cs trong thu muc Attributes
using Project_aspnet_19_DevPro.Areas.Admin.Attributes;
using System.Security.Cryptography;
using System.Data;//sử dụng cho các đối tượng: DataTable, SqlConnection, DataAdapter, DataCommand...
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
namespace Project_aspnet_19_DevPro.Areas.Admin.Controllers
{
    [Area("Admin")]
    //Gọi Attribute CheckLogin để kiểm tra đăng nhập
    [CheckLogin]
    public class CategoryNewsController : Controller
    {
        public MyDbConnect db = new MyDbConnect();
        public IActionResult Index(int? page)
        {
            //lấy trang hiện tại
            int current_page = page ?? 1;
            //định nghĩa số bản ghi trên một trang
            int record_per_page = 5;
            //lấy tất cả các bản ghi trong table Users
            List<ItemCategoryNews> list_record = db.CategoryNewsRecord.OrderByDescending(item => item.Id).ToList();
            //truyền giá trị ra view có phân trang
            return View("Index", list_record.ToPagedList(current_page, record_per_page));
        }
        public IActionResult Update(int? id)
        {
            int _id = id ?? 0;
            //lay mot ban ghi
            ItemCategoryNews record = db.CategoryNewsRecord.Where(item => item.Id == _id).FirstOrDefault();
            //tạo biến action để đưa vào thuộc tính action của thẻ form
            ViewBag.action = "/Admin/CategoryNews/UpdatePost/" + _id;
            //gọi view, truyền dữ liệu ra view
            return View("CreateUpdate", record);
        }
        //khi ấn nút submit thì trạng thái của trang sẽ là POST -> khai báo Attribute [HttpPost]
        [HttpPost]
        public IActionResult UpdatePost(IFormCollection fc, int? id)
        {
            //lấy dữ liệu của thẻ form thông qua đối tượng fc
            string _name = fc["name"].ToString().Trim();
           
            //lấy một bản ghi
            ItemUser record = db.Users.Where(item => item.Id == id).FirstOrDefault();
            if (record != null)
            {
                record.Name = _name;
                
            }
            //cập nhật lại table
            db.SaveChanges();
            //di chuyển đến action có tên là Index
            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            //tạo biến action để đưa vào thuộc tính action của thẻ form
            ViewBag.action = "/Admin/CategoryNews/CreatePost";
            return View("CreateUpdate");
        }
        [HttpPost]
        public IActionResult CreatePost(IFormCollection fc)
        {
            //lấy dữ liệu của thẻ form thông qua đối tượng fc
            string _name = fc["name"].ToString().Trim();
           
            ItemCategoryNews record = new ItemCategoryNews();
            record.Name = _name;
           
            //them ban ghi vao table Users
            db.CategoryNewsRecord.Add(record);
            db.SaveChanges();
            //di chuyển đến action có tên là Index
            return RedirectToAction("Index");
        }
        //xoa ban ghi
        public IActionResult Delete(int? id)
        {
            int _id = id ?? 0;
            //lay mot ban ghi
            ItemCategoryNews record = db.CategoryNewsRecord.Where(item => item.Id == _id).FirstOrDefault();
            //xoa ban ghi khoi csdl
            db.CategoryNewsRecord.Remove(record);
            //cap nhat lai table Users
            db.SaveChanges();
            //di chuyển đến action có tên là Index
            return RedirectToAction("Index");
        }
    }
}
