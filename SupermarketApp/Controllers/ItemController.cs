using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupermarketApp.Data;
using SupermarketApp.Models;

namespace SupermarketApp.Controllers
{
    [Authorize]
    public class ItemController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ItemController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(string itemCategory, string searchString, double maxPrice)
        {
            IQueryable<string> categoryQuery = from m in _db.Items orderby m.Category select m.Category;

            var items = from m in _db.Items select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                items = items.Where(i => i.Name.Contains(searchString));

            }
            if (!string.IsNullOrEmpty(itemCategory))
            {
                items = items.Where(i => i.Category == itemCategory);
            }
            if (maxPrice > 0)
            {
                items = items.Where(i => i.Price < maxPrice);
            }
            var ItemCategoryListM = new CategoryListModel
            {
                Categories = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await categoryQuery.Distinct().ToListAsync()),
                Items = await items.ToListAsync()
            };

            return View(ItemCategoryListM);
        }


        [Authorize(Roles ="Admin")]
        //Get
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        //Get
        public IActionResult Edit(int? id)
        {
            if(id==null || id == 0)
            {
                return NotFound();
            }
            var itemFromDb = _db.Items.Find(id);
            if (itemFromDb == null)
            {
                return NotFound();
            }
            return View(itemFromDb);
        }

        [Authorize(Roles = "Admin")]
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Item obj)
        {
            List<string> categories = new List<string>();
            categories.Add("dairy");
            categories.Add("meatfish");
            categories.Add("veggiesfruit");
            categories.Add("bread");
            categories.Add("care");
            if (!categories.Contains(obj.Category.ToString()))
            {
                ModelState.AddModelError("CustomCategoryError", "Invalid Category.");
            }
            if (ModelState.IsValid)
            {
                _db.Items.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Item \""+obj.Name+"\" updated successfully.";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        [Authorize(Roles = "Admin")]
        //Get
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var itemFromDb = _db.Items.Find(id);
            if (itemFromDb == null)
            {
                return NotFound();
            }
            return View(itemFromDb);
        }

        [Authorize(Roles = "Admin")]
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _db.Items.Find(id);  
            if(obj == null)
            {
                return NotFound();
            }
                _db.Items.Remove(obj);
                _db.SaveChanges();
            TempData["success"] = "Item \"" + obj.Name + "\" deleted successfully.";
            return RedirectToAction("Index");
            
            return View(obj);
        }

        public IActionResult Cart(int? itemid)
        {
            return RedirectToAction("Create", "Cart", new { @itemid = itemid });
        }

        [Authorize(Roles = "Admin")]
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Item obj)
        {
            List<string> categories = new List<string>();
            categories.Add("dairy");
            categories.Add("meatfish");
            categories.Add("veggiesfruit");
            categories.Add("bread");
            categories.Add("care");
            if (!categories.Contains(obj.Category))
            {
                ModelState.AddModelError("CustomCategoryError", "Invalid Category.");
            }
            if (ModelState.IsValid)
            {
                _db.Items.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Item \"" + obj.Name + "\" created successfully.";
                return RedirectToAction("Index");
            } 
          return View(obj);
        }
    }
}
