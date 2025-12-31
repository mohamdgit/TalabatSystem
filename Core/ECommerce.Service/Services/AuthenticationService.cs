using AutoMapper;
using ECommerce.Abstraction.IServices;
using ECommerce.Domain.Exceptions;
using ECommerce.Domain.Models.Identity.Models;
using ECommerce.Shared.Dtos.IdentityDtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Services
{
    public class AuthenticationService(UserManager<ApplicationUser> userManger ,IConfiguration configuration,IMapper mapper) : IAuthenticationService
    {
        public async Task<UserDto> LoginAsync(LoginDto dto)
        {
            var user = await userManger.FindByEmailAsync(dto.Email) ?? throw new UserNotFoundException(dto.Email);
            var IsPasswordValid = await userManger.CheckPasswordAsync(user, dto.Password);
            if (IsPasswordValid)
            {
                return new UserDto()
                {
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    Token = await CreateTokenAsync(user)
                };
            }
            else
            {
                throw new UnAuthorizedException();
            }
        }

        public async Task<UserDto> RegisterAsync(RegisterDto dto)
        {
            var user = new ApplicationUser()
            {
                DisplayName = dto.DisplayName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                UserName = dto.UserName,
            };
            var Result = await userManger.CreateAsync(user, dto.Password);
            if (Result.Succeeded)
            {
                return new UserDto()
                {
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    Token = await CreateTokenAsync(user)
                };

            }
            else
            {
                //validation
                var Errors = Result.Errors.Select(E => E.Description).ToList();
                throw new BadRequestException(Errors);
            }
        }

        public async Task<bool> CheckEmailAsync(string Email)
        {
            var user = await userManger.FindByEmailAsync(Email);
            return user is not null;
        }

        public async Task<AddressDto> GetCurrentUserAddressAsync(string Email)
        {
            var user = await userManger.Users.Include(u => u.Address).FirstOrDefaultAsync(e => e.Email == Email)?? throw new UserNotFoundException(Email);
            if (user.Address is not null)
            {
                return mapper.Map<Address, AddressDto>(user.Address);
            }
            else
            {
                throw new AddressNotFoundException(user.DisplayName);
            }
        }

        public async Task<AddressDto> UpdateCurrentUserAddressAsync(string Email,AddressDto addressDto)
        {
            var user = await userManger.Users.Include(u => u.Address).FirstOrDefaultAsync(e => e.Email == Email) ?? throw new UserNotFoundException(Email);
            if (user.Address is not null)
            {
                user.Address.Street = addressDto.Street;
                user.Address.City = addressDto.City;
                user.Address.Country = addressDto.Country;
                user.Address.FirstName = addressDto.FirstName;
                user.Address.LastName = addressDto.LastName;
            }
            else
            {
               user.Address = mapper.Map<AddressDto,Address>(addressDto);
            }
            await userManger.UpdateAsync(user);
            return mapper.Map<Address, AddressDto>(user.Address);
        }

        public async Task<UserDto> GetCurrentUserAsync(string Email)
        {
            var user =await userManger.FindByEmailAsync(Email);
            return new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await CreateTokenAsync(user)
            };
        }
        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            var userClaims = new List<Claim>()
            {
                new(ClaimTypes.Email,user.Email),
                new(ClaimTypes.Name,user.UserName),
                new(ClaimTypes.NameIdentifier,user.Id),
            };
            var Roles = await userManger.GetRolesAsync(user);

            foreach (var Role in Roles)
            {
                userClaims.Add(new(ClaimTypes.Role, Role));
            }
            var SecurityKey = configuration.GetSection("JWTOptions")["SecurityKey"];
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey));
            var creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
            var Token = new JwtSecurityToken
                (
                    issuer: configuration.GetSection("JWTOptions")["Issuer"],
                    audience: configuration.GetSection("JWTOptions")["Audience"],
                    claims: userClaims,
                    expires: DateTime.Now.AddDays(5),
                    signingCredentials: creds

                );
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}
