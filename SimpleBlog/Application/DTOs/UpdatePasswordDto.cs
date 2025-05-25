namespace SimpleBlog.Application.DTOs
{
    public class UpdatePasswordDto
    {
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
