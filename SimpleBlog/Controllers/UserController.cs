using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Application.DTOs;
using SimpleBlog.Application.Interfaces;

namespace SimpleBlog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController(
        IUserService userService,
        IJwtTokenGenerator jwtTokenGenerator) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;

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

        [AllowAnonymous]
        [HttpGet]
        [Route("authenticate/{email}/{password}")]
        public async Task<IActionResult> Authenticate(string email, string password)
        {
            var authenticatedUser = await _userService.AuthenticateAsync(email, password);
            var tokenDto = _jwtTokenGenerator.GenerateToken(authenticatedUser);
            return Ok(tokenDto);
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
