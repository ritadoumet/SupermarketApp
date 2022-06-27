using Microsoft.AspNetCore.Mvc.Rendering;

namespace SupermarketApp.Models
{
    public class CategoryListModel
    {
        public List<Item> Items { get; set; }
        public SelectList Categories { get; set; }
        public string ItemCategory { get; set; }   
        public string SearchString { get; set; }
    }
}
