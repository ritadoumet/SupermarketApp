using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupermarketApp.Data;
using SupermarketApp.Models;

namespace SupermarketWeb.Controllers
{
    [Authorize]
    public class ItemController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ItemController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Item> objItemList = _db.Items;
            return View(objItemList);
        }

        //Get
        public IActionResult Create()
        {
            return View();
        }
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
