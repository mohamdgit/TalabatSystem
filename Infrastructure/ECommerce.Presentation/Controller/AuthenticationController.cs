using ECommerce.Abstraction.IServices;
using ECommerce.Shared.Dtos.IdentityDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controller
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AuthenticationController(IServiceManager serviceManager):ControllerBase
    {
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto dto)
        {
            var User = await serviceManager.AuthenticationService.LoginAsync(dto);
            return Ok(User);
        }
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto dto)
        {
            var User = await serviceManager.AuthenticationService.RegisterAsync(dto);
            return Ok(User);
        }
        [Authorize]
        [HttpGet("CheckEmail")]
        public async Task<ActionResult<bool>> CheckEmail(string Email)
        {
            var Result = await serviceManager.AuthenticationService.CheckEmailAsync(Email);
            return Ok(Result);
        }
        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await serviceManager.AuthenticationService.GetCurrentUserAsync(Email);
            return Ok(user);
        }
        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>> GetCurrentAddress()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var Address = await serviceManager.AuthenticationService.GetCurrentUserAddressAsync(Email);
            return Ok(Address);
        }
        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateCurrentUserAddress(AddressDto dto)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var UpdatedAddress = await serviceManager.AuthenticationService.UpdateCurrentUserAddressAsync(Email, dto);
            return Ok(UpdatedAddress);
        }
    }
}
