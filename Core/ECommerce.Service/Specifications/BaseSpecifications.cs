using ECommerce.Domain.Contracts.Specification;
using ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Specifications
{
    public abstract class BaseSpecifications<TEntity, Tkey> : ISpecifications<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        #region Where
        public Expression<Func<TEntity, bool>> Criteria { get; private set; }

        protected BaseSpecifications(Expression<Func<TEntity, bool>> _Criteria)
        {
            Criteria = _Criteria;
        }
        #endregion


        #region OrderBy
        public Expression<Func<TEntity, object>> OrderBy { get; private set; }

        protected void AddOrderBy(Expression<Func<TEntity, object>> _OrderBy)
        {
            OrderBy = _OrderBy;
        }
        public Expression<Func<TEntity, object>> OrderByDesc { get; private set; }
        protected void AddOrderByDesc(Expression<Func<TEntity, object>> _OrderByDesc)

        {
            OrderByDesc = _OrderByDesc;
        }
        #endregion


        #region Includes
        public List<Expression<Func<TEntity, object>>> Includes { get; } = [];

        protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }
        #endregion



        #region Pagination
        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPaginated { get; set; }

        public void ApplyPagination(int PageIndex ,int PageSize)
        {
            IsPaginated = true;
            Take = PageSize;
            Skip = (PageIndex - 1) * PageSize;

        }
        #endregion

    }
}
