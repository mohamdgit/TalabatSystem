using ECommerce.Domain.Contracts.Repos;
using ECommerce.Domain.Contracts.Specification;
using ECommerce.Domain.Models;
using ECommerce.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Repos
{
    public class GenericRepository<TEntity, TKey>(StoreDbContext context) : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public async Task<IEnumerable<TEntity>> GetAllAsync()
            => await  context.Set<TEntity>().ToListAsync();


        public async Task<TEntity> GetByIdAsync(TKey id)
            => await context.Set<TEntity>().FindAsync(id);

        public void Add(TEntity entity)
            => context.Set<TEntity>().Add(entity);  
        public void Update(TEntity entity)
            => context.Set<TEntity>().Update(entity);

        public void Delete(TEntity entity)
            => context.Set<TEntity>().Remove(entity);

        public async Task<IEnumerable<TEntity>> GetAllWithSpecificationAsync(ISpecifications<TEntity ,TKey> specifications)
        {
            
            var Query =await SpecificationEvaluator.CreateQuery(context.Set<TEntity>(), specifications).ToListAsync();
            return Query;

        }

        public async Task<TEntity> GetByIdWithSpecificationAsync(ISpecifications<TEntity, TKey> specifications)
        {
            var Query = await SpecificationEvaluator.CreateQuery(context.Set<TEntity>(), specifications).FirstOrDefaultAsync();
            return Query;
        }

        public async Task<int> GetCountWithSpecificationAsync(ISpecifications<TEntity, TKey> specifications)
        {
            var Query =  SpecificationEvaluator.CreateQuery(context.Set<TEntity>(), specifications);
            return await Query.CountAsync();
        }
    }
}
