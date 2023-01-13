using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_aspnet_19_DevPro.Models
{
    [Table("TagProducts")]
    public class ItemTagProducts
    {
        [Key]
        public int Id { get; set; }
        public int TagId { get; set; }
        public int TagProduct { get; set; }
    }
}
