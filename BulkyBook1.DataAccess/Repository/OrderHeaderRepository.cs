using BulkyBook1.DataAccess.Repository.IRepository;
using BulkyBook1.Models;

namespace BulkyBook1.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private ApplicationDbContext _db;

        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderHeader orderHeader)
        {
            _db.OrderHeaders.Update(orderHeader);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var orderFromDb = _db.OrderHeaders.FirstOrDefault(x => x.Id == id);
            if(orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;
                if(paymentStatus != null)
                    orderFromDb.PaymentStatus = paymentStatus;
            }
        }
        public void UpdateStripePaymentID(int id, string sessionId, string paymentIntentId)
        {
            var orderFromDb = _db.OrderHeaders.FirstOrDefault(x => x.Id == id);
            if( orderFromDb != null)
			{
                orderFromDb.PaymentDate = DateTime.Now;
                orderFromDb.SessionId = sessionId;
                orderFromDb.PaymentIntenId = paymentIntentId;
            }                
        }
    }
}