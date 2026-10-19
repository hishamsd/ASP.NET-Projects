using Microsoft.EntityFrameworkCore;
using Project_1.Domain.Models;
using Project_1.Infrastructure.Data;
using Project_1.Infrastructure.Repositories.Base;

namespace Project_1.Infrastructure.Repositories
{
    public class CartRepository : Repository<CartItem>, ICartRepository
    {
        private readonly AppDbContext _db;

        public CartRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<CartItem?> GetByUserIdAsync(int userId)
        {
            var item = await _db.CartItems
                .Include(item => item.Plan)
                    .ThenInclude(plan => plan.Category)
                .Include(item => item.Employee)
                    .ThenInclude(employee => employee.Jobs)
                .SingleOrDefaultAsync(item => item.UserId == userId);

            return item;
        }
    }
}
