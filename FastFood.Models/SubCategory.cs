using System.ComponentModel.DataAnnotations;

namespace FastFood.Models
{
    public class SubCategory
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}