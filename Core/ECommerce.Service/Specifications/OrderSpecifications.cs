using ECommerce.Domain.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Specifications
{
    public class OrderSpecifications : BaseSpecifications<Order, Guid>
    {
        public OrderSpecifications(string Email):base(o=>o.UserEmail==Email)
        {
            AddInclude(o => o.Items);
            AddInclude(p => p.DeliveryMethod);

            AddOrderByDesc(o=>o.OrderDate);
        }
        public OrderSpecifications(Guid id) : base(o => o.Id == id)
        {
            AddInclude(o => o.Items);
            AddInclude(p => p.DeliveryMethod);

        }
    }
}
