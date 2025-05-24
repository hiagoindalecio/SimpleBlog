namespace SimpleBlog.Application.DTOs
{
    public class UpdateUserDto
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
    }
}
