using System.ComponentModel.DataAnnotations;

namespace FastFood.Models
{
   public class OrderHeader
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set;}
        public ApplicationUser ApplicationUser{ get; set; }
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }
        public DateTime TimeofPick{ get; set; }
        public DateTime DateofPick{ get; set; }
        public double SubTotal{ get; set; }
        public double OrderTotal{ get; set; }
        public string CouponCode{ get; set; }
        public double CouponDis{ get; set; }
        public string TransId{ get; set; }
        public string OrderStatus{ get; set; }
        public string Paymentstatus{ get; set; }
        public string PaymentMethod { get; set; }
        public string HouseNo { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string Pincode { get; set; }

        public string Landmark { get; set; }


        public string Name{ get; set; }
        public string Phone{ get; set; }
    }
}
