using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_aspnet_19_DevPro.Models
{
    [Table("Adv")]
    public class ItemAdv
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Photo { get; set; }
        public int Position { get; set; }
    }
}
