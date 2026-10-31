using System.ComponentModel.DataAnnotations;

namespace FastFood.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public double Price { get;set;}

        public string? ImageUrl { get; set; }

        public int CategoryId { get; set; }

        public int SubCategoryId { get; set; }

        public Category Category {get;set;}

        public SubCategory SubCategory {get;set;}
        
    }
}