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
            var cartitems = _db.CartItems.Include("item");
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            cartitems = cartitems.Where(i => i.UserID == userId);

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
            if (itemFromDb == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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
            var cartitems = _db.CartItems.Include("item");
            var itemFromDb = cartitems.Where(c => c.Id == itemid).First();
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
            
            var cartitems = _db.CartItems.Include("item");
            var itemFromDb = cartitems.Where(c=> c.Id == itemid).First();
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

    }
}
