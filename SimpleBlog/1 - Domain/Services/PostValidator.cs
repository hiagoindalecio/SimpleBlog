using SimpleBlog.Domain.Entities;
using SimpleBlog.Domain.Interfaces;

namespace SimpleBlog.Domain.Services
{
    public class PostValidator : IEntityValidator<Post>
    {
        public bool IsValid(Post post, out List<string> errors)
        {
            errors = [];

            if (string.IsNullOrWhiteSpace(post.Title))
                errors.Add("The post title can't be empty.");
            if (string.IsNullOrWhiteSpace(post.Content))
                errors.Add("The post content can't be empty.");

            return errors.Count == 0;
        }
    }
}
