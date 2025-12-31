using ECommerce.Domain.Contracts.Specification;
using ECommerce.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity ,TKey>(IQueryable<TEntity> BaseQuery ,ISpecifications<TEntity,TKey> specifications) where TEntity : BaseEntity<TKey>
        {
            var Query = BaseQuery;
            if(specifications.Criteria is not null)
            {
                Query = Query.Where(specifications.Criteria);
            }
            if(specifications.OrderBy is not null)
            {
                Query = Query.OrderBy(specifications.OrderBy);
            }
            if (specifications.OrderByDesc is not null)
            {
                Query = Query.OrderByDescending(specifications.OrderByDesc);
            }
            if (specifications.IsPaginated)
            {
                Query = Query.Skip(specifications.Skip).Take(specifications.Take);
            }
            if (specifications.Includes is not null &&  specifications.Includes.Any())
            {
                Query = specifications.Includes.Aggregate(Query, (CurrentQuery, Expression) => CurrentQuery.Include(Expression));
            }
            return Query;
        }
    }
}
