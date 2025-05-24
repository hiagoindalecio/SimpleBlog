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
    }
}
