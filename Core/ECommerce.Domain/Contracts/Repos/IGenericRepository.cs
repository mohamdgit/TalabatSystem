using ECommerce.Domain.Contracts.Specification;
using ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Contracts.Repos
{
    public interface IGenericRepository<TEntity,TKey> where TEntity : BaseEntity<TKey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(TKey id);

        Task<IEnumerable<TEntity>> GetAllWithSpecificationAsync(ISpecifications<TEntity, TKey> specifications);
        Task<TEntity> GetByIdWithSpecificationAsync(ISpecifications<TEntity, TKey> specifications);

        Task<int> GetCountWithSpecificationAsync(ISpecifications<TEntity, TKey> specifications);

        void Add(TEntity entity);
        void Update (TEntity entity);
        void Delete(TEntity entity);

    }
}
