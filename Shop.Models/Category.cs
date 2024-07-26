using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
		[DisplayName("CategoryName")]
		public string Name { get; set; }
		[DisplayName("DisplayOrder")]
        [Range(1,100,ErrorMessage ="Dispaly Order must be between 1-100")]
		public int DisplayOrder { get; set; }

    }
}
