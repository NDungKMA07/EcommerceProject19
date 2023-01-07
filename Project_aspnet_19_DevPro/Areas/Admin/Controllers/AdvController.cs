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
    public class AdvController : Controller
    {
        public MyDbConnect db = new MyDbConnect();
        public IActionResult Index(int? page )
        {//lấy trang hiện tại
            int current_page = page ?? 1;
            //định nghĩa số bản ghi trên một trang
            int record_per_page = 5;
            //lấy tất cả các bản ghi trong table Users
            List<ItemAdv> list_record = db.Advs.OrderByDescending(item => item.Id).ToList();
            //truyền giá trị ra view có phân trang
            return View("Index", list_record.ToPagedList(current_page, record_per_page));
        }

        public IActionResult Update(int? id)
        {
            int _id = id ?? 0;  
            ItemAdv record = db.Advs.Where(anhxa => anhxa.Id == _id).FirstOrDefault();
            ViewBag.action = "/Admin/Adv/UpdatePost/" + _id;
            return View("CreateUpdate", record);
        }
        [HttpPost]
        public IActionResult UpdatePost(IFormCollection fc, int? id) {
            int _id = id ?? 0;  
            string _name = fc["name"].ToString().Trim();
            int _position = Convert.ToInt32(fc["position"].ToString().Trim());
            string _fileName = "";
            ItemAdv record = db.Advs.Where(anhxa => anhxa.Id == _id).FirstOrDefault();
            if(record != null)
            {
                record.Name = _name;
                record.Position = _position;
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
                    string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Advs", _fileName);
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
            return RedirectToAction("Index");   
        }

        public IActionResult Create()
        {
          
            ViewBag.action = "/Admin/Adv/CreatePost";
            return View("CreateUpdate");
        }
        [HttpPost]
        public IActionResult CreatePost(IFormCollection fc)
        {
            
            string _name = fc["name"].ToString().Trim();
            int _position = Convert.ToInt32(fc["position"].ToString().Trim());
            string _fileName = "";
            ItemAdv record = new ItemAdv();
            record.Name = _name;
            record.Position = _position;
            try
            {
                //lay ten file
                _fileName = Request.Form.Files[0].FileName;
            }
            catch {; }
            if (!string.IsNullOrEmpty(_fileName))
            {
                //upload anh moi
                var timestamp = DateTime.Now.ToFileTime();
                _fileName = timestamp + "_" + _fileName;
                //lay duong dan cua file
                string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Advs", _fileName);
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


            db.Advs.Add(record);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int? id) {
            int _id = id ?? 0;
            ItemAdv record = db.Advs.Where(anhxa => anhxa.Id == _id).FirstOrDefault();
            db.Advs.Remove(record); 
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
    