using Microsoft.EntityFrameworkCore;
using Project_1.Infrastructure.Data;
using Project_1.Domain.Models;
namespace Project_1.Infrastructure.Repositories.Base
{
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly AppDbContext _db;

        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();


        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T? GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);

        }
    }
}
