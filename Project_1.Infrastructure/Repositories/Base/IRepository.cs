namespace Project_1.Infrastructure.Repositories.Base
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();

        Task<List<T>> GetAllAsync();

        Task<T?> GetByIdAsync(int id);

        T? GetById(int id);

        void Add(T category);

        void Update(T category);

        void Delete(T category);

        void Save();
    }
}
