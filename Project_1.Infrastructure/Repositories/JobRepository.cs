using Project_1.Data;
using Project_1.Models;
using Project_1.Repositories.Base;

namespace Project_1.Repositories
{
        public class JobRepository : Repository<Job>, IJobRepository
        {
            private readonly AppDbContext _db;
            public JobRepository(AppDbContext db) : base(db)
            {
                _db = db;

            }
        }
    }

