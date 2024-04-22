using Store.Data.Entities;

namespace Store.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEnity,TKey> Repository<TEnity, TKey>() where TEnity : BaseEntity<TKey>;
        Task<int> CompleteAsync();
    }
}
