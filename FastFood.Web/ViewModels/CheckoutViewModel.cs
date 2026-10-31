using System.ComponentModel.DataAnnotations;

namespace FastFood.Web.ViewModels
{
    public class CheckoutViewModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Phone { get; set; } = string.Empty;

        public string HouseNo { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string Pincode { get; set; }

        public string Landmark { get; set; }
        
    }
}