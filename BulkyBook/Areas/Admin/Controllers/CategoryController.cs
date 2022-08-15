using BulkyBook1.DataAccess.Repository.IRepository;
using BulkyBook1.Models;
using BulkyBook1.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        public CategoryController(IUnitOfWork db)
        {
            _UnitOfWork = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _UnitOfWork.Category.GetAll();
            return View(objCategoryList);
        }

        //Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name");
            }
            if (ModelState.IsValid)
            {
                _UnitOfWork.Category.Add(obj);
                _UnitOfWork.Save();
                TempData["success"] = "Category is created successfuly";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var CategoryFromDb = _UnitOfWork.Category.GetFirstOrDefault(x => x.Id == id);
            if (CategoryFromDb == null)
                return NotFound();
            return View(CategoryFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name");
            }
            if (ModelState.IsValid)
            {
                _UnitOfWork.Category.Update(obj);
                _UnitOfWork.Save();
                TempData["success"] = "Category is" +
                    "updated successfuly";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        public IActionResult Ab(int? id)
        {
            Category cate = _UnitOfWork.Category.GetAll().Where(x=>x.Id.Equals(id)).SingleOrDefault();

            return View(cate);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var CategoryFromDb = _UnitOfWork.Category.GetFirstOrDefault(x => x.Id.Equals(id));
            if (CategoryFromDb == null)
                return NotFound();
            return View(CategoryFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _UnitOfWork.Category.GetFirstOrDefault(x => x.Id.Equals(id));
            if (obj == null)
                return NotFound();

            _UnitOfWork.Category.Remove(obj);
            _UnitOfWork.Save();
            TempData["success"] = "Category is deleted successfuly";
            return RedirectToAction("Index");
        }
    }
}