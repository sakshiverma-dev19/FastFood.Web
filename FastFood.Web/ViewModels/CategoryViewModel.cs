using System.ComponentModel.DataAnnotations;

namespace FastFood.Web.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category Title is Required")]
        [StringLength(50, ErrorMessage = "Maximum 50 Characters Allowed")]
        public string? Title { get; set; }
    }
}