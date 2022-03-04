namespace Library.Repositories.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task<IEnumerable<T>> FindAll();

        Task<T> FindById(long id);

        Task<T> Create(T vo);
        Task<T> Update(T vo);
        Task<bool> Delete(long id);
    }
}