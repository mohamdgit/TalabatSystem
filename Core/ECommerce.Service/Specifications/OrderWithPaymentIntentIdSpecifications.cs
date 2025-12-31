using ECommerce.Domain.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Specifications
{
    public class OrderWithPaymentIntentIdSpecifications : BaseSpecifications<Order, Guid>
    {

        public OrderWithPaymentIntentIdSpecifications(string intentId) : base(o=>o.PaymentIntentId ==intentId)
        {
        }
    }
}
