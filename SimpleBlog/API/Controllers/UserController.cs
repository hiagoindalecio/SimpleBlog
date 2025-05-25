using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Application.DTOs;
using SimpleBlog.Application.Interfaces;

namespace SimpleBlog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            await _userService.CreateUserAsync(dto);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto dto)
        {
            await _userService.UpdateUserAsync(dto);
            return Ok();
        }

        [HttpDelete]
        [Route("{userId:int}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            await _userService.DeleteUserAsync(userId);
            return Ok();
        }

        [HttpGet]
        [Route("{email}/{password}")]
        public async Task<IActionResult> Authenticate(string email, string password)
        {
            var isAuthenticated = await _userService.AuthenticateAsync(email, password);
            return Ok(isAuthenticated);
        }

        [HttpPut]
        [Route("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto dto)
        {
            await _userService.UpdatePasswordAsync(dto);
            return Ok();
        }
    }
}
