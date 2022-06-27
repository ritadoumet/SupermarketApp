using System.ComponentModel.DataAnnotations;

namespace SupermarketApp.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength =2)]
        public string Name { get; set; } 
        [Required]
        public double Price { get; set; }
        
        public int Weight { get; set; }

        [Required]
        public string Category { get; set; }
    }
}
