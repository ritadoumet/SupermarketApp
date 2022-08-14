using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupermarketApp.Data;
using SupermarketApp.Models;
using System.Security.Claims;
namespace SupermarketApp.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public OrderController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            IQueryable<Order> orders = _db.Orders;

            var items = _db.CartItems.Include("item");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            orders = orders.Where(o => (o.UserID == userId)).Include("CartItems");
            
            List<Order> Orders = await orders.ToListAsync();
            foreach(Order o in Orders)
            {
                foreach(CartItem c in o.CartItems)
                {
                    c.item = items.Where(i => i.Id == c.Id).First().item;
                }
            }
            Orders = Orders.OrderBy(o => o.DateTime).ToList();
            return View(Orders);
        }

        public IActionResult Cart()
        {
            return RedirectToAction("Index", "Cart");
        }
    }
}
