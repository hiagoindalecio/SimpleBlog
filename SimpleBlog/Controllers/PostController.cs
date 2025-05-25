using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Application.DTOs;
using SimpleBlog.Application.Interfaces;
using System.Security.Claims;

namespace SimpleBlog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PostController(IPostService postService) : ControllerBase
    {
        private readonly IPostService _postService = postService;

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDto dto)
        {
            await _postService.CreatePostAsync(dto);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePost([FromBody] UpdatePostDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            await _postService.UpdatePostAsync(dto, userId);
            return Ok();
        }

        [HttpDelete]
        [Route("{postId:int}")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            await _postService.DeletePostAsync(postId, userId);
            return Ok();
        }

        [HttpGet]
        [Route("{take:int}/{skip:int}")]
        public IActionResult GetAllPaged(int take, int skip)
        {
            var posts = _postService.GetAllPaged(take, skip);
            return Ok(posts);
        }
    }
}
