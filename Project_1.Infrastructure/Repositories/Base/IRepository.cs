namespace Project_1.Repositories.Base
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();

        T? GetById(int id);

        void Add(T category);

        void Update(T category);

        void Delete(T category);

        void Save();
    }
}
