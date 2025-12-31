using AutoMapper;
using ECommerce.Abstraction.IServices;
using ECommerce.Domain.Contracts.Repos;
using ECommerce.Domain.Exceptions;
using ECommerce.Domain.Models.Buskets;
using ECommerce.Shared.Dtos.BasketDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Services
{
    public class BasketService(IBasketRepository repository,IMapper mapper) : IBasketService
    {
        public async Task<BasketDto> CreateOrUpdateBasketAsync(BasketDto basket)
        {
            var CustomerBasket = mapper.Map<BasketDto, CustomerBasket>(basket);
            var CreatedBasket = repository.CreateUpdateBasketAsync(CustomerBasket);
            
            if (CreatedBasket is not null)
                return await GetBasketAsync(basket.Id);
            else
                throw new Exception("Some Thing went Wrong");
        }

        public async Task<bool> DeleteBasketAsync(string Key)
        {
            return await repository.DeleteBasketAsync(Key);
        }

        public async Task<BasketDto> GetBasketAsync(string Key)
        {
            var Basket = await repository.GetBasketAsync(Key);
            if (Basket is not null)
                return mapper.Map<BasketDto>(Basket);
            else
                throw new BasketNotFound(Key);
        }
    }
}
