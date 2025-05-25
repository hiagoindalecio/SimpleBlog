namespace SimpleBlog.Application.DTOs
{
    public class AuthResponseDto(string token, string name, string email)
    {
        public string Token { get; set; } = token;
        public string Name { get; set; } = name;
        public string Email { get; set; } = email;
    }
}
