using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_aspnet_19_DevPro.Models
{
    [Table("Tags")]
    public class ItemTags
    {
        [Key]
        public int Id { get; set; }
       public string Name { get; set; } 
    }
}
