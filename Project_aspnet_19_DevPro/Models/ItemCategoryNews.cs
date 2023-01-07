using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project_aspnet_19_DevPro.Models
{
    [Table("CategoryNews")]
    public class ItemCategoryNews
    {

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
    }
}
