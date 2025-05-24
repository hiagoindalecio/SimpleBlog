using SimpleBlog.Application.DTOs;
using SimpleBlog.Application.Exceptions;
using SimpleBlog.Application.Interfaces;
using SimpleBlog.Domain.Entities;
using SimpleBlog.Domain.Exceptions;
using SimpleBlog.Domain.Interfaces;

namespace SimpleBlog.Application.Services
{
    public class PostService(
        IPostRepository repo,
        IEntityValidator<Post> validator,
        IWebSocketNotifierService notifier) : IPostService
    {
        private readonly IPostRepository _postRepository = repo;
        private readonly IEntityValidator<Post> _validator = validator;
        private readonly IWebSocketNotifierService _notifier = notifier;

        public async Task CreatePostAsync(CreatePostDto dto)
        {
            var post = new Post(dto.Title, dto.Content, dto.AuthorId);

            if (!_validator.IsValid(post, out List<string> errors))
                throw new BusinessRuleException($"Invalid post! {errors.Aggregate((a, b) => a + "\n " + b)}");

            await _postRepository.AddAsync(post);
            await _notifier.NotifyAsync($"New post: {post.Title}");
        }

        public async Task DeletePostAsync(int postId)
        {
            var post = await _postRepository.GetByIdAsync(postId) ?? throw new NotFoundException($"Post with ID {postId} not found.");
            await _postRepository.DeleteAsync(post);
        }

        public async Task UpdatePostAsync(UpdatePostDto dto)
        {
            var post = await _postRepository.GetByIdAsync(dto.Id) ?? throw new NotFoundException($"Post with ID {dto.Id} not found.");
            post.Update(dto.Title, dto.Content);
            if (!_validator.IsValid(post, out List<string> errors))
                throw new BusinessRuleException($"Invalid post! {errors.Aggregate((a, b) => a + "\n " + b)}");
            await _postRepository.UpdateAsync(post);
        }

        public async Task<List<PagedPostsDto>> GetAllPagedAsync(int take, int skip)
        {
            var posts = await _postRepository.GetAllPagedAsync(take, skip);
            return posts;
        }
    }
}
