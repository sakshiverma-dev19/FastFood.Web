using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FastFood.Web.ViewModels
{
    public class ProfileViewModel
    {
        public string? Name { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public string? City { get; set; }

        public string? Address { get; set; }

        public string? PostalCode { get; set; }

        public string? ProfileImage { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}
