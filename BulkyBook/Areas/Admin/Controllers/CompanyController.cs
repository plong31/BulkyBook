using BulkyBook1.DataAccess.Repository.IRepository;
using BulkyBook1.Models;
using BulkyBook1.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        public CompanyController(IUnitOfWork db, IWebHostEnvironment hostEnvironment)
        {
            _UnitOfWork = db;
        }

        public IActionResult Index()
        {
            //IEnumerable<Product> objProductList = _UnitOfWork.Product.GetAll();
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Company company = new();
            if (id == null || id == 0)
            {
                return View(company);
            }
            else
            {
                company = _UnitOfWork.Company.GetFirstOrDefault(u=>u.Id==id);
                return View(company);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj)
        {
            if (ModelState.IsValid)
            {
                if(obj.Id == 0)
                {
                    _UnitOfWork.Company.Add(obj);
                    TempData["success"] = "Company is created successfuly";
                }
                else
                {
                    _UnitOfWork.Company.Update(obj);
                    TempData["success"] = "Company is updated successfuly";
                }
                
                _UnitOfWork.Save();
                
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var companyList = _UnitOfWork.Company.GetAll();
            return Json(new { data = companyList });
        }
        [HttpDelete]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _UnitOfWork.Company.GetFirstOrDefault(x => x.Id==id);
            if (obj == null)
                return Json(new { success = false, message = "Error while deleting" });
            _UnitOfWork.Company.Remove(obj);
            _UnitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion API CALLS
    }
}