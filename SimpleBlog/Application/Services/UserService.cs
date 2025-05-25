using Microsoft.AspNetCore.Identity;
using SimpleBlog.Application.DTOs;
using SimpleBlog.Application.Exceptions;
using SimpleBlog.Application.Interfaces;
using SimpleBlog.Domain.Entities;
using SimpleBlog.Domain.Exceptions;
using SimpleBlog.Domain.Interfaces;

namespace SimpleBlog.Application.Services
{
    public class UserService(
        IUserRepository repo,
        IEntityValidator<User> validator,
        IPasswordHasher<string> passwordHasher) : IUserService
    {
        private readonly IUserRepository _userRepository = repo;
        private readonly IEntityValidator<User> _validator = validator;
        private readonly IPasswordHasher<string> _passwordHasher = passwordHasher;

        public async Task CreateUserAsync(CreateUserDto dto)
        {
            string hashedPassword = _passwordHasher.HashPassword(dto.Email, dto.PlainPassword);
            var user = new User(dto.Name, dto.Email, hashedPassword);
            if (!_validator.IsValid(user, out List<string> errors))
                throw new BusinessRuleException($"Invalid user: {errors.Aggregate((a, b) => a + "\n " + b)}");
            await _userRepository.AddAsync(user);
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId) ?? throw new NotFoundException($"User with ID {userId} not found.");
            await _userRepository.DeleteAsync(user);
        }

        public async Task UpdateUserAsync(UpdateUserDto dto)
        {
            var existingUser = await _userRepository.GetByIdAsync(dto.Id) ?? throw new NotFoundException($"User with ID {dto.Id} not found.");
            existingUser.Update(dto.Name, dto.Email);
            if (!_validator.IsValid(existingUser, out List<string> errors))
                throw new BusinessRuleException($"Invalid user: {errors.Aggregate((a, b) => a + "\n " + b)}");
            await _userRepository.UpdateAsync(existingUser);
        }

        public async Task<string> GetUserNameByIdAsync(int userId)
        {
            var userName = await _userRepository.GetNameByIdAsync(userId) ?? throw new NotFoundException($"User with ID {userId} not found.");
            return userName;
        }

        public async Task<bool> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            var result = _passwordHasher.VerifyHashedPassword(email, user.HashedPassword, password);
            return result == PasswordVerificationResult.Success;
        }

        public async Task UpdatePasswordAsync(UpdatePasswordDto dto)
        {
            var user = await _userRepository.GetByIdAsync(dto.UserId) ?? throw new NotFoundException($"User with ID {dto.UserId} not found.");
            string hashedPassword = _passwordHasher.HashPassword(dto.Email, dto.NewPassword);
            user.UpdatePassword(hashedPassword);
            await _userRepository.UpdateAsync(user);
        }
    }
}
