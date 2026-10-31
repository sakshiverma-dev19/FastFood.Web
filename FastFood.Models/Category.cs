using System.ComponentModel.DataAnnotations;

namespace FastFood.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Category Title Required")]
        [StringLength(50)]
        public string Title { get; set; } = string.Empty;
    }
}