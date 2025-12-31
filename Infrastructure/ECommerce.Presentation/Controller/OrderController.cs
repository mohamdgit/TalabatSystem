using ECommerce.Abstraction.IServices;
using ECommerce.Shared.Dtos.OrderDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace ECommerce.Presentation.Controller
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class OrderController(IServiceManager serviceManager):ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var Order = await serviceManager.OrderServices.CreateOrderAsync(orderDto, Email);
            return Ok(Order);
        }

        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodDto>>> GetAllDeliveryMehtods()
        {
            var DMs = await serviceManager.OrderServices.GetDeliveryMethodsAsync();
            return Ok(DMs);
        }
        [HttpGet("AllOrders")]
        public async Task<ActionResult<IEnumerable<OrderToReturnDto>>> GetAllOrdersForUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var Orders = await serviceManager.OrderServices.GetAllOrdersAsync(Email);
            return Ok(Orders);
        }
        [HttpGet]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderById(Guid orderId)
        {
            var Order = await serviceManager.OrderServices.GetOrderByIDAsync(orderId);
            return Ok(Order);
        }


    }
}
