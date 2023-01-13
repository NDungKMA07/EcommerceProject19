using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project_aspnet_19_DevPro.Models
{
    [Table("ImgProducts")]
    public class ItemImgProducts
    {

        [Key]
        public int Id { get; set; }
        public int IdProduct { get; set; }
        public string? Photo { get; set; }
    }
}
