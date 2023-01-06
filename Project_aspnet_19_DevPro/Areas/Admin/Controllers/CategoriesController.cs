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
        public IConfiguration configuration;
        //hàm tạo là hàm mặc định được triệu gọi khi class được khởi tạo
        public CategoriesController(IConfiguration _configuration)
        {
            //sử dụng dòng sau để gán giá trị vào biến toàn cục
            configuration = _configuration;//từ đây có thể đọc được các tham số của file appsettings.json từ đây
        }
        public IActionResult Index(int? page)
        {
            //tạo đối tượng DataTable
            DataTable dtCategories = new DataTable();
            //lấy chuỗi kết nối -> chuỗi này nằm trong file appsettings.json
            string strDbConnectString = configuration.GetConnectionString("DbConnectString").ToString();
            //từ khóa using để thực hiện lệnh bên trong khối using đó, khi kết thúc thì khối lệnh bên trong sẽ bị hủy đi
            using (SqlConnection conn = new SqlConnection(strDbConnectString))
            {
                //thực hiện truy vấn, trả kết quả về biến object
                SqlDataAdapter da = new SqlDataAdapter("select * from categories where ParentId = 0 order by Id desc", conn);
                //đổ dữ liệu vào DataTable có tên là dtCategories
                da.Fill(dtCategories);
            }
            //---
            int current_page = page ?? 1;
            //định nghĩa số bản ghi trên một trang
            int record_per_page = 4;
            //đổ dữ liệu từ biến DataTable có tên là dtCategories vào List để có thể phân trang
            //tạo List có tên listCategories
            List<ItemCategory> listCategories = new List<ItemCategory>();
            foreach (DataRow item in dtCategories.Rows)
            {
                listCategories.Add(new ItemCategory() { Id = Convert.ToInt32(item["Id"].ToString()), Name = item["Name"].ToString(), ParentId = Convert.ToInt32(item["ParentId"].ToString()) });
            }
            //---
            //truyền giá trị ra view có phân trang
            return View("Index", listCategories.ToPagedList(current_page, record_per_page));
        }
        //url: /Admin/Categories/Update/id
        public IActionResult Update(int? id)
        {
            int _id = id ?? 0;
            //tao doi tuong DataTable
            DataTable dtCategories = new DataTable();
            //lay chuoi ket noi -> chuoi nay nam trong file appsettings.json
            string strDbConnectString = configuration.GetConnectionString("DbConnectString").ToString();
            using (SqlConnection conn = new SqlConnection(strDbConnectString))
            {
                //thực hiện truy vấn, trả kết quả về biến object
                SqlDataAdapter da = new SqlDataAdapter("select * from categories where Id = " + _id, conn);
                //đổ dữ liệu vào DataTable có tên là dtCategories
                da.Fill(dtCategories);
            }
            //khởi tạo một item (tương ứng với 1 row trang table) của DataTable dtCategories
            DataRow itemCategory = dtCategories.NewRow();
            if (dtCategories.Rows.Count > 0)
                itemCategory = dtCategories.Rows[0];
            //---
            //liệt kê danh mục để truyền ra view
            int _CurrentId = 0;
            if (itemCategory != null)
                _CurrentId = Convert.ToInt32(itemCategory["Id"]);
            ViewBag.listCategory = db.Categories.Where(item => item.ParentId == 0 && item.Id != _CurrentId).OrderByDescending(item => item.Id).ToList();
            //---
            //tạo biến action để đưa vào thuộc tính action của thẻ form
            ViewBag.action = "/Admin/Categories/UpdatePost/" + _id;
            //truyền 1 DataRow <=> 1 hàng trong DataTable ra view
            return View("CreateUpdate", itemCategory);
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
            //lay chuoi ket noi -> chuoi nay nam trong file appsettings.json
            string strDbConnectString = configuration.GetConnectionString("DbConnectString").ToString();
            using (SqlConnection conn = new SqlConnection(strDbConnectString))
            {
                //update, delete, insert thì phải open đối tượng kết nối
                conn.Open();
                SqlCommand cmd = new SqlCommand("update Categories set Name=@var_name,ParentId=@var_parent_id where Id=@var_id", conn);
                cmd.Parameters.AddWithValue("@var_name", _name);
                cmd.Parameters.AddWithValue("@var_parent_id", _parent_id);
                cmd.Parameters.AddWithValue("@var_id", _id);
                //thực thi truy cấn
                cmd.ExecuteNonQuery();
            }
            //di chuyển đến một url khác
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
            //lay chuoi ket noi -> chuoi nay nam trong file appsettings.json
            string strDbConnectString = configuration.GetConnectionString("DbConnectString").ToString();
            using (SqlConnection conn = new SqlConnection(strDbConnectString))
            {
                //update, delete, insert thì phải open đối tượng kết nối
                conn.Open();
                SqlCommand cmd = new SqlCommand("insert into Categories(Name, ParentId) values (@var_name,@var_parent_id)", conn);
                cmd.Parameters.AddWithValue("@var_name", _name);
                cmd.Parameters.AddWithValue("@var_parent_id", _parent_id);
                //thực thi truy cấn
                cmd.ExecuteNonQuery();
            }
            //di chuyển đến một url khác
            return Redirect("/Admin/Categories");
        }
        public IActionResult Delete(int? id)
        {
            int _id = id ?? 0;
            //lay chuoi ket noi -> chuoi nay nam trong file appsettings.json
            string strDbConnectString = configuration.GetConnectionString("DbConnectString").ToString();
            using (SqlConnection conn = new SqlConnection(strDbConnectString))
            {
                //update, delete, insert thì phải open đối tượng kết nối
                conn.Open();
                SqlCommand cmd = new SqlCommand("delete from Categories where Id=@var_id or ParentId=@var_id", conn);
                cmd.Parameters.AddWithValue("@var_id", _id);
                //thực thi truy cấn
                cmd.ExecuteNonQuery();
            }
            //di chuyển đến một url khác
            return Redirect("/Admin/Categories");
        }
    }
}
