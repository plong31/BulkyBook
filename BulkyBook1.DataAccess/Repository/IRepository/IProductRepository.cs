using BulkyBook1.Models;

namespace BulkyBook1.DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product obj);
    }
}