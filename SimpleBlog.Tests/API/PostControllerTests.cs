using Microsoft.AspNetCore.Mvc;
using Moq;
using SimpleBlog.API.Controllers;
using SimpleBlog.Application.DTOs;
using SimpleBlog.Application.Interfaces;

namespace SimpleBlog.Tests.API
{
    public class PostControllerTests
    {
        private readonly Mock<IPostService> _mockPostService;
        private readonly PostController _controller;

        public PostControllerTests()
        {
            _mockPostService = new Mock<IPostService>();
            _controller = new PostController(_mockPostService.Object);
        }

        [Fact]
        public async Task CreatePost_ReturnsOkResult_WhenPostIsCreated()
        {
            var dto = new CreatePostDto { Title = "Title", Content = "Content", AuthorId = 1 };

            var result = await _controller.CreatePost(dto);

            var actionResult = Assert.IsType<OkResult>(result);
            _mockPostService.Verify(x => x.CreatePostAsync(dto), Times.Once);
        }

        [Fact]
        public async Task UpdatePost_ReturnsOkResult_WhenUpdateIsSuccessful()
        {
            var dto = new UpdatePostDto { Id = 1, Title = "Updated", Content = "Updated" };

            var result = await _controller.UpdatePost(dto);

            var actionResult = Assert.IsType<OkResult>(result);
            _mockPostService.Verify(x => x.UpdatePostAsync(dto), Times.Once);
        }

        [Fact]
        public async Task DeletePost_ReturnsOkResult_WhenDeleteIsSuccessful()
        {
            int postId = 1;

            var result = await _controller.DeletePost(postId);

            var actionResult = Assert.IsType<OkResult>(result);
            _mockPostService.Verify(x => x.DeletePostAsync(postId), Times.Once);
        }

        [Fact]
        public void GetAllPaged_ReturnsListOfPagedPosts()
        {
            var samplePosts = new List<PagedPostsDto>
            {
                new(1, "Title1", "Content1", "Author1", DateTime.UtcNow),
                new(2, "Title2", "Content2", "Author2", DateTime.UtcNow)
            };

            _mockPostService.Setup(x => x.GetAllPaged(10, 0)).Returns(samplePosts);

            var result = _controller.GetAllPaged(10, 0);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<PagedPostsDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }
    }
}
