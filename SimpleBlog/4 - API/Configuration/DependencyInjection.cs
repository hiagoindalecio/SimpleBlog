using Microsoft.AspNetCore.Identity;
using SimpleBlog.Domain.Entities;
using SimpleBlog.Domain.Interfaces;
using SimpleBlog.Domain.Services;
using SimpleBlog.Application.Interfaces;
using SimpleBlog.Application.Services;
using SimpleBlog.Infrastructure.Repositories;

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
            services.AddScoped<IWebSocketNotifierService, WebSocketNotifierService>();

            // Infrastructure
            services.AddScoped<IPasswordHasher<string>, PasswordHasher<string>>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPostRepository, PostRepository>();

            // Other services

            return services;
        }
    }
}
