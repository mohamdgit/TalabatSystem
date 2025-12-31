using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Exceptions
{
    public class DeliveryMethodNotFoundException(int id) : NotFoundException($"DeliveryMethod with Id {id} Is Not Found")
    {
    }
}
