using BulkyBook1.DataAccess.Repository.IRepository;
using BulkyBook1.Models;
using BulkyBook1.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        public CoverTypeController(IUnitOfWork db)
        {
            _UnitOfWork = db;
        }

        public IActionResult Index()
        {
            IEnumerable<CoverType> objCoverTypeList = _UnitOfWork.CoverType.GetAll();
            return View(objCoverTypeList);
        }

        //Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType obj)
        {
            if (ModelState.IsValid)
            {
                _UnitOfWork.CoverType.Add(obj);
                _UnitOfWork.Save();
                TempData["success"] = "CoverType is created successfuly";
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
            var CoverTypeFromDb = _UnitOfWork.CoverType.GetFirstOrDefault(x => x.Id == id);
            if (CoverTypeFromDb == null)
                return NotFound();
            return View(CoverTypeFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType obj)
        {
            if (ModelState.IsValid)
            {
                _UnitOfWork.CoverType.Update(obj);
                _UnitOfWork.Save();
                TempData["success"] = "CoverType is updated successfuly";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var CoverTypeFromDb = _UnitOfWork.CoverType.GetFirstOrDefault(x => x.Id.Equals(id));
            if (CoverTypeFromDb == null)
                return NotFound();
            return View(CoverTypeFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _UnitOfWork.CoverType.GetFirstOrDefault(x => x.Id.Equals(id));
            if (obj == null)
                return NotFound();

            _UnitOfWork.CoverType.Remove(obj);
            _UnitOfWork.Save();
            TempData["success"] = "CoverType is deleted successfuly";
            return RedirectToAction("Index");
        }
    }
}