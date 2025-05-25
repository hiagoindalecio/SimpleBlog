using Microsoft.AspNetCore.Identity;
using SimpleBlog.Application.Interfaces;
using SimpleBlog.Application.Services;
using SimpleBlog.Domain.Entities;
using SimpleBlog.Domain.Interfaces;
using SimpleBlog.Domain.Services;
using SimpleBlog.Infrastructure.Repositories;
using SimpleBlog.Infrastructure.WebSockets;

namespace SimpleBlog.API.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProjectDependencies(this IServiceCollection services)
        {
            // Domain
            services.AddScoped<IEntityValidator<Post>, PostValidator>();
            services.AddScoped<IEntityValidator<User>, UserValidator>();

            // Application
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IUserService, UserService>();

            // Infrastructure
            services.AddScoped<IPasswordHasher<string>, PasswordHasher<string>>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddSingleton<INotificationWebSocketHandler, NotificationWebSocketHandler>();

            // Other services

            return services;
        }
    }
}
