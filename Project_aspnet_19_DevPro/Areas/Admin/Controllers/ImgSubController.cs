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
    public class ImgSubController : Controller
    {
        public MyDbConnect db = new MyDbConnect();
        public IActionResult Index(int? page,int? id)
        {//lấy trang hiện tại
            int current_page = page ?? 1;
            //định nghĩa số bản ghi trên một trang
            int record_per_page = 5;
            //lấy tất cả các bản ghi trong table Users
            int _idProduct = id ?? 0;
            List<ItemImgProducts> list_record = db.ImgProducts.Where(anhxa => anhxa.IdProduct == _idProduct).OrderByDescending(item => item.Id).ToList();
            //truyền giá trị ra view có phân trang
            ViewBag.IdProduct = _idProduct;
            TempData["IdProduct"] = _idProduct;
            return View("Index", list_record.ToPagedList(current_page, record_per_page));
        }


        public IActionResult Update(int? id)
        {
            int _id = id ?? 0;
            ItemImgProducts record = db.ImgProducts.Where(anhxa => anhxa.Id == _id).FirstOrDefault();
            ViewBag.action = "/Admin/ImgSub/UpdatePost/" + _id;
            return View("CreateUpdate", record);
        }
        [HttpPost]
        public IActionResult UpdatePost(IFormCollection fc, int? id)
        {
            int _id = id ?? 0;
            
            string _fileName = "";
            ItemImgProducts record = db.ImgProducts.Where(anhxa => anhxa.Id == _id).FirstOrDefault();
            if (record != null)
            {
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
                    string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/ImgProducts", _fileName);
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
            return Redirect("/Admin/ImgSub/Index/" + Convert.ToString(TempData["IdProduct"]));
        }

        public IActionResult Create(int? id)
        {
            string _id = Convert.ToString(id ?? 0);
            ViewBag.action = "/Admin/ImgSub/CreatePost/" + _id ;
            return View("CreateUpdate", _id);
        }
        [HttpPost]
        public IActionResult CreatePost(IFormCollection fc)
        {
            //lay mot ban ghi
            int _idProducts = Convert.ToInt32(fc["idProduct"].ToString().Trim());
            ItemImgProducts record = new ItemImgProducts();
            record.IdProduct = _idProducts;
            
            string _file_name = "";
            try
            {
                //lay ten file
                _file_name = Request.Form.Files[0].FileName;
            }
            catch {; }
            if (!string.IsNullOrEmpty(_file_name))
            {
                //upload anh moi
                var timestamp = DateTime.Now.ToFileTime();
                _file_name = timestamp + "_" + _file_name;
                //lay duong dan cua file
                string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/ImgProducts", _file_name);
                //upload file
                using (var stream = new FileStream(_Path, FileMode.Create))
                {
                    Request.Form.Files[0].CopyTo(stream);
                }
                //update gia tri vao cot Photo trong csdl
                record.Photo = _file_name;
                //cập nhật lại table
                db.SaveChanges();
            }
            //them ban ghi vao table Products
            db.ImgProducts.Add(record);
            db.SaveChanges();
            //di chuyển đến action có tên là Index
            return Redirect("/Admin/ImgSub/Index/" + Convert.ToString(_idProducts) );
        }
        public IActionResult Delete(int? id)
        {
            int _id = id ?? 0;
            ItemImgProducts record = db.ImgProducts.Where(anhxa => anhxa.Id == _id).FirstOrDefault();
            db.ImgProducts.Remove(record);
            db.SaveChanges();
            return Redirect("/Admin/ImgSub/Index/" + Convert.ToString(TempData["IdProduct"]));
          
        }

    }
}
