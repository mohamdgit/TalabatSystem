using ECommerce.Shared.Dtos.IdentityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Dtos.OrderDtos
{
    public class OrderToReturnDto
    {
        public Guid Id { get; set; }
        public string UserEmail { get; set; } = null!;
        public DateTimeOffset OrderDate { get; set; }
        public AddressDto Address { get; set; } = null!;
        public string DeliveryMthod { get; set; } = null!;
        public string OrderStatus { get; set; } = null!;
        public ICollection<OrderItemDto> Items { get; set; } = [];
        public decimal subTotal { get; set; }
        public decimal total { get; set; }

    }
}
