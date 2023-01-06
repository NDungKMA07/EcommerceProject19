var builder = WebApplication.CreateBuilder(args);
//---
//khai báo để hệ thống sử dụng mô hình MVC
//builder.Services.AddMvc();
//hoặc
builder.Services.AddControllersWithViews();
//khai bao session
builder.Services.AddSession();
//---
var app = builder.Build();
//su dung session
app.UseSession();

//app.MapGet("/", () => "Hello World!");
//---
//khai báo để project hiểu được và có thể load các file trong thư mục wwwroot
app.UseStaticFiles();
//cấu hình đường dẫn
//khi tạo phân hệ area thì phải cấu hình để website nhận biết được mvc trong area đó
//cấu hình phân hệ area phải nằm trên cấu hình của phân hệ mặc đinh
app.MapControllerRoute(name: "area", pattern: "{area=exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
//---
app.Run();
