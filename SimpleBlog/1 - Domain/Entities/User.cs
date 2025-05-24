namespace SimpleBlog.Domain.Entities
{
    public class User(string name, string email, string hashedPassword)
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = name;
        public string Email { get; private set; } = email;
        public string HashedPassword { get; private set; } = hashedPassword;

        public User() : this(string.Empty, string.Empty, string.Empty)
        { }

        internal void Update(string name, string email)
        {
            Name = name;
            Email = email;
        }

        internal void UpdatePassword(string hashedPassword)
        {
            HashedPassword = hashedPassword;
        }
    }
}
