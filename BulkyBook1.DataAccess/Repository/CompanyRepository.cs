using BulkyBook1.DataAccess.Repository.IRepository;
using BulkyBook1.Models;

namespace BulkyBook1.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private ApplicationDbContext _db;

        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Company company)
        {
            _db.Companys.Update(company);
        }
    }
}