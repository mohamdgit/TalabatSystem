using ECommerce.Shared.Dtos.OrderDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Abstraction.IServices
{
    public interface IOrderServices
    {
        //Create Order
        //*take OrderDto [Address - DeliveryMethodId - BasketId] , UserEmail
        //*return OrderToReturn [orderId - Email - OrderDate - Items - Address - DelvermethodName -OrderState - Subtotal - total]
        Task<OrderToReturnDto> CreateOrderAsync(OrderDto orderDto, string Email);

        //Get All Delivery Methods
        Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync();
        //Get All Orders for CurrentUser
        Task<IEnumerable<OrderToReturnDto>> GetAllOrdersAsync(string Email);

        //Get Spacific Order For CurrentUser
        Task<OrderToReturnDto> GetOrderByIDAsync(Guid orderId);


    }
}
