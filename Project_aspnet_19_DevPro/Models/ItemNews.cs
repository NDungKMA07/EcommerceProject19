using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project_aspnet_19_DevPro.Models
{
    [Table("News")]
    public class ItemNews
    {

        [Key]
        public int Id { get; set; }
        public int Hot { get; set; }
        public int CategoryNewId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string? Photo { get; set; }
    }
}
