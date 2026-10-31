using System.ComponentModel.DataAnnotations;

namespace FastFood.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        public int ItemId { get; set; }

        public int Quantity { get; set; }

        public Item Item { get; set; }
    }
}