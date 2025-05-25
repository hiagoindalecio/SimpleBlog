using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Application.DTOs;
using SimpleBlog.Application.Interfaces;

namespace SimpleBlog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            await _postService.UpdatePostAsync(dto);
            return Ok();
        }

        [HttpDelete]
        [Route("{postId:int}")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            await _postService.DeletePostAsync(postId);
            return Ok();
        }

        [HttpGet]
        [Route("{take:int}/{skip:int}")]
        public async Task<IActionResult> GetAllPaged(int take, int skip)
        {
            var posts = await _postService.GetAllPagedAsync(take, skip);
            return Ok(posts);
        }
    }
}
