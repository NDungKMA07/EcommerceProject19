using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_aspnet_19_DevPro.Models
{
    [Table("Products")]
    public class ItemProduct
    {
        [Key]
        public int Id { get; set; }
        public int Hot { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string? Photo { get; set; }
    }
}
