using Moq;
using Microsoft.AspNetCore.Mvc;
using SimpleBlog.API.Controllers;
using SimpleBlog.Application.DTOs;
using SimpleBlog.Application.Interfaces;
using SimpleBlog.Domain.Entities;

public class UserControllerTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<IJwtTokenGenerator> _mockJwtGenerator;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _mockUserService = new Mock<IUserService>();
        _mockJwtGenerator = new Mock<IJwtTokenGenerator>();
        _controller = new UserController(_mockUserService.Object, _mockJwtGenerator.Object);
    }

    [Fact]
    public async Task CreateUser_ReturnsOk_WhenSuccessful()
    {
        var dto = new CreateUserDto { Name = "Hiago", Email = "hiago@test.com", PlainPassword = "123456" };

        var result = await _controller.CreateUser(dto);

        Assert.IsType<OkResult>(result);
        _mockUserService.Verify(x => x.CreateUserAsync(dto), Times.Once);
    }

    [Fact]
    public async Task UpdateUser_ReturnsOk_WhenSuccessful()
    {
        var dto = new UpdateUserDto { Id = 1, Name = "Updated", Email = "updated@test.com" };

        var result = await _controller.UpdateUser(dto);

        Assert.IsType<OkResult>(result);
        _mockUserService.Verify(x => x.UpdateUserAsync(dto), Times.Once);
    }

    [Fact]
    public async Task DeleteUser_ReturnsOk_WhenSuccessful()
    {
        int userId = 1;

        var result = await _controller.DeleteUser(userId);

        Assert.IsType<OkResult>(result);
        _mockUserService.Verify(x => x.DeleteUserAsync(userId), Times.Once);
    }

    [Fact]
    public async Task Authenticate_ReturnsToken_WhenCredentialsAreValid()
    {
        string email = "hiago@test.com";
        string password = "123456";
        var user = new User("Hiago", email, password);

        _mockUserService.Setup(x => x.AuthenticateAsync(email, password)).ReturnsAsync(user);
        _mockJwtGenerator.Setup(x => x.GenerateToken(user)).Returns(new AuthResponseDto("fake-token", user.Name, email));

        var result = await _controller.Authenticate(email, password);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var token = Assert.IsType<AuthResponseDto>(okResult.Value);
        Assert.Equal("fake-token", token.Token);
    }

    [Fact]
    public async Task Authenticate_ReturnsUnauthorized_WhenUserIsInvalid()
    {
        string email = "invalid@test.com";
        string password = "wrong";
        var user = new User("Hiago", email, password);

        _mockUserService.Setup(x => x.AuthenticateAsync(email, password)).ReturnsAsync(user);

        var result = await _controller.Authenticate(email, password);

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async Task UpdatePassword_ReturnsOk_WhenSuccessful()
    {
        var dto = new UpdatePasswordDto { Email = "123", OldPassword = "123", NewPassword = "456" };

        var result = await _controller.UpdatePassword(dto);

        Assert.IsType<OkResult>(result);
        _mockUserService.Verify(x => x.UpdatePasswordAsync(dto), Times.Once);
    }
}
