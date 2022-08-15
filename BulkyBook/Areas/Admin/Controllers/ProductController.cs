using BulkyBook1.DataAccess.Repository.IRepository;
using BulkyBook1.Models;
using BulkyBook1.Models.ViewModels;
using BulkyBook1.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork db, IWebHostEnvironment hostEnvironment)
        {
            _UnitOfWork = db;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            //IEnumerable<Product> objProductList = _UnitOfWork.Product.GetAll();
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _UnitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                CoverTypeList = _UnitOfWork.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            //Product product = new();
            //IEnumerable<SelectListItem> CategoryList = _UnitOfWork.Category.GetAll().Select(
            //    u => new SelectListItem
            //    {
            //        Text = u.Name,
            //        Value = u.Id.ToString()
            //    });
            //IEnumerable<SelectListItem> CoverTypeList = _UnitOfWork.CoverType.GetAll().Select(
            //    u => new SelectListItem
            //    {
            //        Text = u.Name,
            //        Value = u.Id.ToString()
            //    });
            if (id == null || id == 0)
            {
                //create product
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(productVM);
            }
            else
            {
                //update product
                productVM.Product = _UnitOfWork.Product.GetFirstOrDefault(u=>u.Id==id);
                return View(productVM);
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                //WebRootPath:Nhận hoặc đặt đường dẫn tuyệt đối đến thư mục chứa các tệp nội dung ứng dụng có thể phục vụ web.
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    //Tạo tên file ảnh
                    string fileName = Guid.NewGuid().ToString();
                    //Lấy đường dẫn để lưu file wwwRootPath: đường dẫn gốc + images\products = folder
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    //Láy đuôi file ảnh
                    var extension = Path.GetExtension(file.FileName);
                    if(obj.Product.ImageURL != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageURL.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    //Tạo ảnh mới để lưu
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    //lưu ảnh vào đường dẫn
                    obj.Product.ImageURL = @"\images\products\" + fileName + extension;
                }
                if(obj.Product.Id == 0)
                {
                    _UnitOfWork.Product.Add(obj.Product);
                    TempData["success"] = "Product is created successfuly";
                }
                else
                {
                    _UnitOfWork.Product.Update(obj.Product);
                    TempData["success"] = "Product is updated successfuly";
                }
                
                _UnitOfWork.Save();
                
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        public IActionResult Bc(int? id)
        {
            Product productList = _UnitOfWork.Product.GetAll(includeProperties: "Category,CoverType").Where(x=>x.Id.Equals(id)).SingleOrDefault();
            return View(productList);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var ProductFromDb = _UnitOfWork.Product.GetFirstOrDefault(x => x.Id.Equals(id));
            if (ProductFromDb == null)
                return NotFound();
            return View(ProductFromDb);
        }
        //Delete POST
        

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _UnitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
            return Json(new { data = productList });
        }
        [HttpDelete]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _UnitOfWork.Product.GetFirstOrDefault(x => x.Id==id);
            if (obj == null)
                return Json(new { success = false, message = "Error while deleting" });
            //Xoa image of product
            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageURL.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _UnitOfWork.Product.Remove(obj);
            _UnitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion API CALLS
    }
}