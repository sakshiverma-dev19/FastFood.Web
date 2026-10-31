using System.ComponentModel.DataAnnotations;

namespace FastFood.Web.ViewModels
{
    public class SubCategoryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Sub Category Title Required")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please Select Category")]
        public int CategoryId { get; set; }
    }
}