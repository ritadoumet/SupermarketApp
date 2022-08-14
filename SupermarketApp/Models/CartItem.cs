using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SupermarketApp.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        public string UserID { get; set; }

        public Item item { get; set; }

        [Range(1,20)]
        public int Quantity { get; set; } = 1;
    }
}
