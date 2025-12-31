using AutoMapper;
using ECommerce.Abstraction.IServices;
using ECommerce.Domain.Contracts.Repos;
using ECommerce.Domain.Contracts.UOW;
using ECommerce.Domain.Exceptions;
using ECommerce.Domain.Models.Buskets;
using ECommerce.Domain.Models.Orders;
using ECommerce.Domain.Models.Products;
using ECommerce.Shared.Dtos.BasketDtos;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ECommerce.Service.Services
{
    public class PaymentServices(IConfiguration configuration,IBasketRepository basketRepository,IUnitOfWork unitOfWork,IMapper mapper) : IPaymentServices
    {
        public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string BasketId)
        {
            //Confug Stripe with SecretKey : Install Stripe.net
            StripeConfiguration.ApiKey = configuration["StripeSettings:SecretKey"];
            //GetBasket By BasketId 
            var Basket =await basketRepository.GetBasketAsync(BasketId)?? throw new BasketNotFound(BasketId);
            //Get Amount - GetProduct + DliveryMetod cost
            var ProductRepo = unitOfWork.GetRepository<Domain.Models.Products.Product, int>();
            foreach (var item in Basket.Items)
            {
                var product = await ProductRepo.GetByIdAsync(item.Id) ?? throw new ProductNotFound(item.Id);
                item.Price = product.Price;
            }
            var DeliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(Basket.DeliveryMethodId.Value) ?? throw new DeliveryMethodNotFoundException(Basket.DeliveryMethodId.Value);
            
            Basket.ShippingPrice = DeliveryMethod.Price;
            var BasketAmount = (long) (Basket.Items.Sum(item=>item.Quantity * item.Price)+DeliveryMethod.Price)*100;
            //Create Payment Intent [Create -update]
            var PaymentService = new PaymentIntentService();
            //create intent
            if(Basket.PaymentIntentId is null)
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = BasketAmount,
                    Currency = "USD",
                    PaymentMethodTypes = ["card"]

                };
                var PaymentIntent = await PaymentService.CreateAsync(options);
                Basket.PaymentIntentId = PaymentIntent.Id;
                Basket.clientSecret = PaymentIntent.ClientSecret;
            }//update intent with amount
            else
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = BasketAmount
                };
                await PaymentService.UpdateAsync(Basket.PaymentIntentId, options);
            }
            await basketRepository.CreateUpdateBasketAsync(Basket);
            return mapper.Map<CustomerBasket,BasketDto>(Basket);

        }
    }
}
