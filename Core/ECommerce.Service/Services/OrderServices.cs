using AutoMapper;
using ECommerce.Abstraction.IServices;
using ECommerce.Domain.Contracts.Repos;
using ECommerce.Domain.Contracts.UOW;
using ECommerce.Domain.Exceptions;
using ECommerce.Domain.Models.Orders;
using ECommerce.Domain.Models.Products;
using ECommerce.Service.Specifications;
using ECommerce.Shared.Dtos.IdentityDtos;
using ECommerce.Shared.Dtos.OrderDtos;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Services
{
    public class OrderServices (IMapper mapper,IBasketRepository basketRepository,IUnitOfWork unitOfWork): IOrderServices
    {
        public async Task<OrderToReturnDto> CreateOrderAsync(OrderDto orderDto, string Email)
        {
            //Map Address to Order Address
            var OrderAddress = mapper.Map<AddressDto, OrderAddress>(orderDto.Address);
            //Get Basket
            var basket =await basketRepository.GetBasketAsync(orderDto.BasketId) ?? throw new BasketNotFound(orderDto.BasketId);

            //check PaymentIntent if Exist Order
            ArgumentException.ThrowIfNullOrEmpty(basket.PaymentIntentId);
            var OrderRepo = unitOfWork.GetRepository<Order, Guid>();
            var spec = new OrderWithPaymentIntentIdSpecifications(basket.PaymentIntentId);
            var ExsisOrder =await OrderRepo.GetByIdWithSpecificationAsync(spec);
            if (ExsisOrder is not null) OrderRepo.Delete(ExsisOrder);

            //Create OrderItems List
            List<OrderItem> orderItems = [];

            var productRepo = unitOfWork.GetRepository<Product, int>();
            foreach (var item in basket.Items)
            {
                var Product = await productRepo.GetByIdAsync(item.Id)?? throw new ProductNotFound(item.Id);
                var orderItem = new OrderItem()
                {
                    product = new ProductItemOrdered()
                    {
                        ProductId = Product.Id,
                        ProductName = Product.Name,
                        PictureUrl = Product.PictureUrl,
                    },
                    Quantity = item.Quantity,
                    Price = Product.Price,
                };
                orderItems.Add(orderItem);
            }

            //Get Delivery Method
            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderDto.DeliveryMethodId) ?? throw new DeliveryMethodNotFoundException(orderDto.DeliveryMethodId);

            //sub Total
            var subTotal = orderItems.Sum(I => I.Price * I.Quantity);

            var Order = new Order(Email, OrderAddress, deliveryMethod, orderItems, subTotal,basket.PaymentIntentId);
            unitOfWork.GetRepository<Order,Guid>().Add(Order);
            await unitOfWork.SaveChangesAsync();
            return mapper.Map<Order, OrderToReturnDto>(Order);
        }

        public async Task<IEnumerable<OrderToReturnDto>> GetAllOrdersAsync(string Email)
        {
            var spec = new OrderSpecifications(Email);
            var Orders = await unitOfWork.GetRepository<Order, Guid>().GetAllWithSpecificationAsync(spec);
            return mapper.Map<IEnumerable<Order>, IEnumerable<OrderToReturnDto>>(Orders);
        }

        public async Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await unitOfWork.GetRepository<DeliveryMethod,int>().GetAllAsync();
            return mapper.Map<IEnumerable< DeliveryMethod>,IEnumerable<DeliveryMethodDto>>(deliveryMethods);
        }

        public async Task<OrderToReturnDto> GetOrderByIDAsync(Guid orderId)
        {
            var spec = new OrderSpecifications(orderId);
            var Order = await unitOfWork.GetRepository<Order,Guid>().GetByIdWithSpecificationAsync(spec);
            return mapper.Map<Order, OrderToReturnDto>(Order);

        }
    }
}
