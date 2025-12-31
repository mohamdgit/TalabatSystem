using ECommerce.Shared.Dtos.IdentityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Abstraction.IServices
{
    public interface IAuthenticationService
    {
        //login
        //*Take Email ,password
        //*return Token ,Email,DisplayName
        Task<UserDto> LoginAsync(LoginDto dto);

        //Register
        //*take Email,userName,password ,phone ,DisplayName
        //*return Token ,Email,DisplayName
        Task<UserDto> RegisterAsync(RegisterDto dto);

        //check Email
        //*Take Email   //*Return Bool

        public Task<bool> CheckEmailAsync(string Email);

        //Get Current User Address
        //*Take Email   //*AddressDto

        public Task<AddressDto> GetCurrentUserAddressAsync(string Email);

        //Update Current User Adrress
        //*Take Email ,AddressDto   //*Return updated Address
        public Task<AddressDto> UpdateCurrentUserAddressAsync(string Email, AddressDto addressDto);

        //Get Current User
        //*Take Email   //*Return Token,Email,displayName
        public Task<UserDto> GetCurrentUserAsync(string Email);
    }
}
