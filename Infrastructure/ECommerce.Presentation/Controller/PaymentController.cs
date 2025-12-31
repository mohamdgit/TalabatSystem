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
    [Route("api/[controller]")]
    public class PaymentController(IServiceManager serviceManager):ControllerBase
    {
        [Authorize]
        [HttpPost("{basketId}")]

        public async Task<ActionResult<BasketDto>> CreateOrUpdateBasketIntent(string basketId)
        {
            var Basket = await serviceManager.PaymentServices.CreateOrUpdatePaymentIntentAsync(basketId);
            return Ok(Basket);

        }
    }
}
