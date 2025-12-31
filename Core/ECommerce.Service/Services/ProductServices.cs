using AutoMapper;
using ECommerce.Abstraction.IServices;
using ECommerce.Domain.Contracts.UOW;
using ECommerce.Domain.Exceptions;
using ECommerce.Domain.Models.Products;
using ECommerce.Service.Specifications;
using ECommerce.Shared.Common;
using ECommerce.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Services
{
    public class ProductServices(IMapper mapper, IUnitOfWork unitOfWork) : IProductServices
    {
        public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync()
        {
           var repo = unitOfWork.GetRepository<ProductBrand, int>();
            var Brands = await repo.GetAllAsync();
            var BrandDto = mapper.Map<IEnumerable<ProductBrand>, IEnumerable<BrandDto>>(Brands);
            return BrandDto;
        }

        //public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        //{
        //    var Products = await unitOfWork.GetRepository<Product, int>().GetAllAsync();
        //    var ProductDto = mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(Products);
        //    return ProductDto;
        //}

        public async Task<PaginationResult<ProductDto>> GetAllProductsAsync(ProductQueryParams queryParams)
        {
            var spec = new ProductSpecifications(queryParams);
            var Products = await unitOfWork.GetRepository<Product, int>().GetAllWithSpecificationAsync(spec);
            var Data = mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(Products);
            var size = Data.Count();
            var countSpec = new CountProductSpecification(queryParams);
            var Count = await unitOfWork.GetRepository<Product, int>().GetCountWithSpecificationAsync(countSpec) ;
            return new PaginationResult<ProductDto>(queryParams.PageIndex, size, Count, Data);
        }
        public async Task<IEnumerable<TypeDto>> GetAllTypesAsync()
        {
            var Types = await unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            var TypeDto = mapper.Map<IEnumerable<ProductType>, IEnumerable<TypeDto>>(Types);
            return TypeDto;
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var spec= new ProductSpecifications(id);
            var Product = await unitOfWork.GetRepository<Product, int>().GetByIdWithSpecificationAsync(spec);
            var ProductDto = mapper.Map<Product, ProductDto>(Product);

            if (ProductDto is null)
                throw new ProductNotFound(id);
            return ProductDto;
        }
    }
}
