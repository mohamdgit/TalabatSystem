using AutoMapper;
using ECommerce.Abstraction.IServices;
using ECommerce.Domain.Contracts.Repos;
using ECommerce.Domain.Contracts.UOW;
using ECommerce.Domain.Models.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Services
{
    public class ServiceManager(IMapper mapper, IUnitOfWork unitofWork, IBasketRepository basketrepository , UserManager<ApplicationUser> userManger, IConfiguration configuration) : IServiceManager
    {
        #region Products
        private readonly Lazy<IProductServices> LazyProductServices = new Lazy<IProductServices>(() => new ProductServices(mapper, unitofWork));
        public IProductServices ProductServices => LazyProductServices.Value;
        #endregion

        #region Basket
        private readonly Lazy<IBasketService> LazyBasketService = new Lazy<IBasketService>(() => new BasketService(basketrepository, mapper));
        public IBasketService BasketService => LazyBasketService.Value;
        #endregion

        #region Authentication
        private readonly Lazy<IAuthenticationService> LazyAuthenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(userManger, configuration, mapper));
        public IAuthenticationService AuthenticationService => LazyAuthenticationService.Value;
        #endregion

        #region Order
        private readonly Lazy<IOrderServices> LazyOrderServices = new Lazy<IOrderServices>(() => new OrderServices(mapper, basketrepository, unitofWork));
        public IOrderServices OrderServices => LazyOrderServices.Value;
        #endregion

        #region Payment
        private readonly Lazy<IPaymentServices> LazyPaymentServices = new Lazy<IPaymentServices>(() => new PaymentServices(configuration, basketrepository, unitofWork, mapper));
        public IPaymentServices PaymentServices => LazyPaymentServices.Value; 
        #endregion

    }
}
