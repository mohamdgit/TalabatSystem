using ECommerce.Abstraction.IServices;
using ECommerce.Shared.Dtos.BasketDtos;
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
    public class BasketController(IServiceManager serviceManager):ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<BasketDto>> GetBasketAsync(string Key)
        {
            var Basket = await serviceManager.BasketService.GetBasketAsync(Key);
            return Ok(Basket);
        }
        [HttpPost]
        public async Task<ActionResult<BasketDto>> CreateUpdateBasketAsync(BasketDto Basket)
        {
            var CreatedBasket = await serviceManager.BasketService.CreateOrUpdateBasketAsync(Basket);
            return Ok(CreatedBasket);
        }
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasketAsync(string Key)
        {
            var Result = await serviceManager.BasketService.DeleteBasketAsync(Key);
            return Ok(Result);}
    }
}
