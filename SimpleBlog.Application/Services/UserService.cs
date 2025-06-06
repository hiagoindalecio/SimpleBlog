﻿using Microsoft.AspNetCore.Identity;
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
        IPasswordHasher<string> passwordHasher,
        IUserPasswordValidator userPasswordValidator) : IUserService
    {
        private readonly IUserRepository _userRepository = repo;
        private readonly IEntityValidator<User> _validator = validator;
        private readonly IPasswordHasher<string> _passwordHasher = passwordHasher;
        private readonly IUserPasswordValidator _userPasswordValidator = userPasswordValidator;

        public async Task CreateUserAsync(CreateUserDto dto)
        {
            var hashedPassword = _passwordHasher.HashPassword(dto.Email, dto.PlainPassword);
            var user = new User(dto.Name, dto.Email, hashedPassword);
            if (!_validator.IsValid(user, out List<string> errors))
                throw new BusinessRuleException($"Invalid user: {errors.Aggregate((a, b) => a + "\n " + b)}");
            if (!_userPasswordValidator.IsValid(dto.PlainPassword, out List<string> passwordErrors))
                throw new BusinessRuleException($"Invalid password: {passwordErrors.Aggregate((a, b) => a + "\n " + b)}");
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

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email) ?? throw new NotFoundException($"User with email {email} not found.");
            var result = _passwordHasher.VerifyHashedPassword(email, user.HashedPassword, password);
            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Invalid password.");

            return user;
        }

        public async Task UpdatePasswordAsync(UpdatePasswordDto dto)
        {
            var user = await AuthenticateAsync(dto.Email, dto.OldPassword);
            if (!_userPasswordValidator.IsValid(dto.NewPassword, out List<string> passwordErrors))
                throw new BusinessRuleException($"Invalid password: {passwordErrors.Aggregate((a, b) => a + "\n " + b)}");
            var hashedPassword = _passwordHasher.HashPassword(dto.Email, dto.NewPassword);
            user.UpdatePassword(hashedPassword);
            await _userRepository.UpdateAsync(user);
        }
    }
}
