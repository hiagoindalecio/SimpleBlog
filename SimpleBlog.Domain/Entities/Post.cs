namespace SimpleBlog.Domain.Entities
{
    public class Post(string title, string content, int authorId)
    {
        public int Id { get; private set; }
        public string Title { get; private set; } = title;
        public string Content { get; private set; } = content;
        public int AuthorId { get; private set; } = authorId;
        public User? Author { get; private set; }
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

        public void Update(string title, string content)
        {
            Title = title;
            Content = content;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
