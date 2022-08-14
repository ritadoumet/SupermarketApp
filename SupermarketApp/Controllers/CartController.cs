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
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        public CartController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var cartitems = _db.CartItems.Include("item").Include("Order");
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            cartitems = cartitems.Where(i => (i.UserID == userId && i.Order==null));

            List<CartItem> Items = await cartitems.ToListAsync();
            return View(Items);
        }
        
        public IActionResult Create(int? itemid)
        {
            if (itemid == null || itemid == 0)
            {
                return NotFound();
            }
            var itemFromDb = _db.Items.Find(itemid);
            var cartItems = _db.CartItems.Include("item").Include("Order");
            if (itemFromDb == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            cartItems = cartItems.Where(c => (c.UserID == userId && c.item.Id == itemid && c.Order == null));
            if (cartItems.Any())
            {
                TempData["success"] = "This item already exists in your cart. You can edit it from here:";
                return RedirectToAction("Edit", "Cart", new {itemid = cartItems.First().Id});
            }
            CartItem cartItem = new CartItem();
            cartItem.item = itemFromDb;
            cartItem.UserID = userId;
            return View(cartItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CartItem obj)
        {
            if (ModelState.IsValid)
            {
                var itemFromDb = _db.Items.Find(obj.item.Id);
                obj.item = itemFromDb;
                _db.CartItems.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Item \"" + obj.item.Name + "\" successfully added to cart.";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Delete(int? itemid)
        {
            if (itemid == null || itemid == 0)
            {
                return NotFound();
            }
            var cartitems = _db.CartItems.Include("item").Include("Order");
            var itemFromDb = cartitems.Where(c => c.Id == itemid).FirstOrDefault();
            if (itemFromDb == null)
            {
                return NotFound();
            }
            return View(itemFromDb);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(CartItem obj)
        {
            if (obj == null)
            {
                return NotFound();
            }
            _db.CartItems.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Item \"" + obj.item.Name + "\" removed successfully from cart.";
            return RedirectToAction("Index");

        }
        public IActionResult Edit(int? itemid)
        {
            if (itemid == null || itemid == 0)
            {
                return NotFound();
            }
            
            var cartitems = _db.CartItems.Include("item").Include("Order");
            var itemFromDb = cartitems.Where(c=> c.Id == itemid).FirstOrDefault();
            if (itemFromDb == null)
            {
                return NotFound();
            }

            return View(itemFromDb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CartItem obj)
        {
            if (ModelState.IsValid)
            {
                _db.CartItems.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Cart Item \"" + obj.item.Name + "\" updated successfully.";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Item()
        {
            return RedirectToAction("Index", "Item");
        }
        public IActionResult Order()
        {
            var Order = new Order();
            var items = _db.CartItems.Include("item").Include("Order");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            items = items.Where(i => (i.UserID == userId && i.Order == null));

            if (!items.Any())
            {
                TempData["success"] = "Your cart is empty. Nothing to order.";
                return RedirectToAction("Index");
            }
            else
            {
                var price = 0.0;
                foreach(CartItem c in items)
                {
                    c.Order = Order;
                    price += c.item.Price * c.Quantity;
                }
                Order.DateTime= DateTime.Now;
                Order.UserID = userId;
                Order.TotalPrice = price;
                _db.Orders.Add(Order);
                _db.SaveChanges();
                TempData["success"] = "Your order for $"+price+" was placed.";
                RedirectToAction("Index", "Order");
            }
            return View();
        }
    }
}
