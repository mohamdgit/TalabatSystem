using ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Contracts.Specification
{
    public interface ISpecifications<TEntity,Tkey> where TEntity : BaseEntity<Tkey>
    {
        Expression<Func<TEntity,bool>> Criteria { get; }
        Expression<Func<TEntity, object>> OrderBy { get; }
        Expression<Func<TEntity, object>> OrderByDesc { get; }

       List<Expression<Func<TEntity,object>>> Includes { get; }

        int Take {  get; }
        int Skip { get; }
        bool IsPaginated { get; set; }


    }
}
