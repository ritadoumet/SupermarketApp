using System.ComponentModel.DataAnnotations;
namespace SupermarketApp.Models
{
    public class Order
    {

        [Key]
        public int Id { get; set; }

        public DateTime DateTime { get; set; }

        public Double TotalPrice { get; set; }

        public string UserID { get; set; }

        public List<CartItem> CartItems { get; set; }
    }
}
