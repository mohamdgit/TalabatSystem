using ECommerce.Abstraction.IServices;
using ECommerce.Shared.Common;
using ECommerce.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controller
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ProductController(IServiceManager serviceManager) : ControllerBase
    {
        //Get All Product 
        //Get BaseURl/api/Product
        [HttpGet]
        public async Task<ActionResult<PaginationResult<ProductDto>>> GetAllProducts([FromQuery]ProductQueryParams queryParams)
        {
            var products = await serviceManager.ProductServices.GetAllProductsAsync(queryParams);
            return Ok(products) ;//status code 200
        }
        //Get all Brands
        [HttpGet("Brands")]
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetAllBrandsAsync()
        {
            var Brands = await serviceManager.ProductServices.GetAllBrandsAsync();
            return Ok(Brands) ;
        }
        //Get all Types
        [HttpGet("Types")]
        public async Task<ActionResult<IEnumerable<TypeDto>>> GetAllTypesAsync()
        {
            var Types = await serviceManager.ProductServices.GetAllTypesAsync();
            return Ok(Types);
        }
        //Get Product By Id
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductByIdAsync(int id)
        {
            var product = await serviceManager.ProductServices.GetProductByIdAsync(id);
            return Ok(product);
        }

    }
}
