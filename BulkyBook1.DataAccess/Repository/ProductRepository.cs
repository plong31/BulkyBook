using BulkyBook1.DataAccess.Repository.IRepository;
using BulkyBook1.Models;

namespace BulkyBook1.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product obj)
        {
            var objFromDb = _db.Products.FirstOrDefault(p => p.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.Description = obj.Description;
                objFromDb.Price = obj.Price;
                objFromDb.ListPrice = obj.ListPrice;
                objFromDb.ListPrice50 = obj.ListPrice50;
                objFromDb.ListPrice100 = obj.ListPrice100;
                objFromDb.Author = obj.Author;
                objFromDb.ISBN = obj.ISBN;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.CoverTypeId = obj.CoverTypeId;
                if (obj.ImageURL != null)
                    objFromDb.ImageURL = obj.ImageURL;
            }
        }
    }
}