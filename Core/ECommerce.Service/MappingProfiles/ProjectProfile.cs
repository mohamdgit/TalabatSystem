using AutoMapper;
using ECommerce.Domain.Models.Buskets;
using ECommerce.Domain.Models.Identity.Models;
using ECommerce.Domain.Models.Orders;
using ECommerce.Domain.Models.Products;
using ECommerce.Shared.Dtos;
using ECommerce.Shared.Dtos.BasketDtos;
using ECommerce.Shared.Dtos.IdentityDtos;
using ECommerce.Shared.Dtos.OrderDtos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.MappingProfiles
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile(IConfiguration configuration)
        {
            #region Product
            CreateMap<Product, ProductDto>()
        .ForMember(dist => dist.BrandName, options => options.MapFrom(src => src.Brand.Name))
        .ForMember(dist => dist.TypeName, options => options.MapFrom(src => src.Type.Name))
        .ForMember(dist => dist.PictureUrl, options => options.MapFrom(new PictureUrlResolver(configuration)));


            CreateMap<ProductBrand, BrandDto>();
            CreateMap<ProductType, TypeDto>();

            #endregion

            #region Basket
            CreateMap<CustomerBasket, BasketDto>().ReverseMap();
            CreateMap<BasketItem, BasketItemDto>().ReverseMap();
            #endregion

            #region User
            CreateMap<Address, AddressDto>().ReverseMap();
            #endregion

            #region Order
            CreateMap<AddressDto, OrderAddress>().ReverseMap();
            CreateMap<Order, OrderToReturnDto>()
                    .ForMember(D => D.DeliveryMthod, options => options.MapFrom(src => src.DeliveryMethod.ShortName)); 

            CreateMap<OrderItem,OrderItemDto>()
                    .ForMember(D => D.ProductName, options => options.MapFrom(src => src.product.ProductName))
                    .ForMember(D => D.ProductUrl, options => options.MapFrom(new OrderPictureUrlResolver(configuration)));
            CreateMap<DeliveryMethod, DeliveryMethodDto>().ReverseMap();

            #endregion
        }

    }
}
