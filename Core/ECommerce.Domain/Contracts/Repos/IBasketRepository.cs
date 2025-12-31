using ECommerce.Domain.Models.Buskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Contracts.Repos
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetBasketAsync(string Key);
        Task<CustomerBasket> CreateUpdateBasketAsync(CustomerBasket Basket,TimeSpan? TimeToLive =null);
        Task<bool> DeleteBasketAsync(string key);
    }
}
