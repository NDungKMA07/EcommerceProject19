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
{ //khai báo Route Admin để nhận diện trên url đây là Area Admin
    [Area("Admin")]
    //Gọi Attribute CheckLogin để kiểm tra đăng nhập
    [CheckLogin]
    public class CategoriesController : Controller
    {
        //khởi tạo đối tượng thao tác csdl
        public MyDbConnect db = new MyDbConnect();
        //Tạo biến toàn cục để đọc các thông số từ file appsettings.json

        public IActionResult Index(int? page)
        {//lấy trang hiện tại
            int current_page = page ?? 1;
            //định nghĩa số bản ghi trên một trang
            int record_per_page = 5;
            //lấy tất cả các bản ghi trong table Users
            List<ItemCategory> list_record = db.Categories.Where(item => item.ParentId == 0).OrderByDescending(item => item.Id).ToList();
            //truyền giá trị ra view có phân trang
            return View("Index", list_record.ToPagedList(current_page, record_per_page));
        }
        //url: /Admin/Categories/Update/id
        public IActionResult Update(int? id)
        {
            int _id = id ?? 0;
            ItemCategory record = db.Categories.Where(anhxa => anhxa.Id == _id).FirstOrDefault();
            ViewBag.action = "/Admin/Categories/UpdatePost/" + _id;
            return View("CreateUpdate", record);
          
        }
        //khi ấn nút submit thì trang sẽ ở trạng thái POST
        //url: /Admin/Categories/UpdatePost/Id
        //ở trạng thái POST thì phải khai báo tag sau
        [HttpPost]
        public IActionResult UpdatePost(int? id, IFormCollection fc)
        {
            int _id = id ?? 0;
            string _name = fc["name"].ToString().Trim();
            string _parent_id = fc["parent_id"].ToString().Trim();
            ItemCategory record = db.Categories.Where(item => item.Id == _id).FirstOrDefault();
            string _fileName = "";
           if (record != null)
            {
                record.Name = _name;
                record.ParentId = Int32.Parse(_parent_id);
                try
                {
                    _fileName = Request.Form.Files[0].FileName;
                }
                catch
                {
                    ;
                }
                if (!string.IsNullOrEmpty(_fileName))
                {
                    //upload anh moi
                    var timestamp = DateTime.Now.ToFileTime();
                    _fileName = timestamp + "_" + _fileName;
                    //lay duong dan cua file
                    string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Categories", _fileName);
                    //upload file
                    using (var stream = new FileStream(_Path, FileMode.Create))
                    {
                        Request.Form.Files[0].CopyTo(stream);
                    }
                    //update gia tri vao cot Photo trong csdl
                    record.Photo = _fileName;
                    //cập nhật lại table
                    db.SaveChanges();
                }
            }
            db.SaveChanges();   
            return Redirect("/Admin/Categories");
        }
        //url: /Admin/Categories/Create
        public IActionResult Create()
        {
            //tạo biến action để đưa vào thuộc tính action của thẻ form
            ViewBag.action = "/Admin/Categories/CreatePost";
            return View("CreateUpdate");
        }
        //khi ấn nút submit thì trang sẽ ở trạng thái POST
        //url: /Admin/Categories/UpdatePost/Id
        //ở trạng thái POST thì phải khai báo tag sau
        [HttpPost]
        public IActionResult CreatePost(IFormCollection fc)
        {
            string _name = fc["name"].ToString().Trim();
            string _parent_id = fc["parent_id"].ToString().Trim();
           ItemCategory record = new ItemCategory();
            record.Name = _name;    
            record.ParentId = Int32.Parse(_parent_id);
            string _fileName = "";
            try
            {
                _fileName = Request.Form.Files[0].FileName;
            }
            catch
            {
                ;
            }
            if (!string.IsNullOrEmpty(_fileName))
            {
                //upload anh moi
                var timestamp = DateTime.Now.ToFileTime();
                _fileName = timestamp + "_" + _fileName;
                //lay duong dan cua file
                string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Categories", _fileName);
                //upload file
                using (var stream = new FileStream(_Path, FileMode.Create))
                {
                    Request.Form.Files[0].CopyTo(stream);
                }
                //update gia tri vao cot Photo trong csdl
                record.Photo = _fileName;
                //cập nhật lại table
                db.SaveChanges();
            }
            db.Categories.Add(record);  
            db.SaveChanges();   
            //di chuyển đến một url khác
            return Redirect("/Admin/Categories");
        }
        public IActionResult Delete(int? id)
        {
            int _id = id ?? 0;
            ItemCategory record = db.Categories.Where(item => item.Id == _id).FirstOrDefault();
            if (record != null) {
                db.Categories.Remove(record);

            }
            db.SaveChanges();   
            return Redirect("/Admin/Categories");
        }
    }
}
