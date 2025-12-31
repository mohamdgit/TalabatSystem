using ECommerce.Domain.Models.Products;
using ECommerce.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Specifications
{
    public class ProductSpecifications : BaseSpecifications<Product ,int>
    {
        public ProductSpecifications(ProductQueryParams queryParams) :
            base(p=>(!queryParams.BrandId.HasValue || p.BrandId == queryParams.BrandId) && (!queryParams.TypeId.HasValue || p.TypeId == queryParams.TypeId)
            && (string.IsNullOrEmpty(queryParams.SearchValue) || p.Name.ToLower().Contains( queryParams.SearchValue)))
        {
            AddInclude(p => p.Brand);
            AddInclude(p => p.Type);

            switch (queryParams.sortingOption)
            {
                case ProductSortingOptions.NameAsc:
                    AddOrderBy(p => p.Name);
                    break;
                case ProductSortingOptions.NameDesc:
                    AddOrderByDesc(p => p.Name);
                    break;
                case ProductSortingOptions.PriceAsc:
                    AddOrderBy(p => p.Price);
                    break;
                case ProductSortingOptions.PriceDesc:
                    AddOrderByDesc(p => p.Price);
                    break;
                default:
                    break;
            }
            ApplyPagination(queryParams.PageIndex , queryParams.PageSize);
        }
        public ProductSpecifications(int id) : base(p=>p.Id ==id)
        {
            AddInclude(p => p.Brand);
            AddInclude(p => p.Type);
        }
    }
}
