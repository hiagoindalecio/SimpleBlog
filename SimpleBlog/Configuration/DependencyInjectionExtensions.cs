using Microsoft.AspNetCore.Identity;
using SimpleBlog.Application.Interfaces;
using SimpleBlog.Application.Services;
using SimpleBlog.Domain.Entities;
using SimpleBlog.Domain.Interfaces;
using SimpleBlog.Domain.Services;
using SimpleBlog.Infrastructure.Authentication;
using SimpleBlog.Infrastructure.Repositories;
using SimpleBlog.Infrastructure.WebSockets;

namespace SimpleBlog.API.Configuration
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddProjectDependencies(this IServiceCollection services)
        {
            // Domain
            services.AddScoped<IEntityValidator<Post>, PostValidator>();
            services.AddScoped<IEntityValidator<User>, UserValidator>();
            services.AddScoped<IUserPasswordValidator, UserPasswordValidator>();

            // Application
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IUserService, UserService>();

            // Infrastructure
            services.AddScoped<IPasswordHasher<string>, PasswordHasher<string>>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddSingleton<INotificationWebSocketHandler, NotificationWebSocketHandler>();
            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

            return services;
        }
    }
}
