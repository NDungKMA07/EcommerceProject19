//su dung entity framework
using Microsoft.EntityFrameworkCore;
//doc noi dung file appsettings.json
using Microsoft.Extensions.Configuration;
//thao tac voi file, thu muc
using System.IO;
namespace Project_aspnet_19_DevPro.Models
{
    public class MyDbConnect : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //tao doi tuong de doc thong tin cua file appsettings.json
            var builder = new ConfigurationBuilder();
            //set duong dan cua file appsettings.json
            builder.SetBasePath(Directory.GetCurrentDirectory());
            //add file appsettings.json vao doi tuong builder
            builder.AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            //doc chuoi ket noi o trong file appsettings.json
            string strDbConnectString = configuration.GetConnectionString("DbConnectString").ToString();
            //ket noi voi csdl thong qua chuoi ket noi
            optionsBuilder.UseSqlServer(strDbConnectString);
        }
        public DbSet<ItemUser> Users { get; set; } //<=> table Users trong csdl
        public DbSet<ItemCategory> Categories { get; set; } //<=> table Categories trong csdl
    }
}
