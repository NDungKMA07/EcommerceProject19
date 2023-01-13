using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_aspnet_19_DevPro.Models
{
    [Table("CategoriesProducts")]
    public class ItemCategoriesProducts
    {
        [Key]
        public int Id { get; set; } 
        public int CategoryId { get; set; } 
        public int ProductId { get; set; }  
    }
}
