using Store.Data.Context;
using Store.Data.Entities;
using Store.Repository.Interfaces;
using System.Collections;

namespace Store.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _context;
        private Hashtable _repositories;

        public UnitOfWork(StoreDbContext context)
        {
            _context = context;
        }
        public async Task<int> CompleteAsync()
            => await _context.SaveChangesAsync();

        public IGenericRepository<TEnity, TKey> Repository<TEnity, TKey>() where TEnity : BaseEntity<TKey>
        {
            if(_repositories is null)
                _repositories = new Hashtable();

            var entityKey = typeof(TEnity).Name;

            if (!_repositories.ContainsKey(entityKey))
            {
                var repositoryType = typeof(GenericRepository<, >);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEnity),typeof(TKey)),_context);
                _repositories.Add(entityKey, repositoryInstance);
            }
            return (IGenericRepository<TEnity, TKey>)_repositories[entityKey];
        }
    }
}
