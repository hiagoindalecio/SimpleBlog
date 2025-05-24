namespace SimpleBlog.Application.DTOs
{
    public class PagedPostsDto(int id, string title, string content, string authorName, DateTime updatedAt)
    {
        public int Id { get; private set; } = id;
        public string Title { get; private set; } = title;
        public string Content { get; private set; } = content;
        public string AuthorName { get; set; } = authorName;
        public DateTime UpdatedAt { get; private set; } = updatedAt;
    }
}
