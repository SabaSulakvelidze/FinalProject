namespace FinalProject.Services
{
    public class PasswordService : IPasswordService
    {
        public string Hash(string password)
            => BCrypt.Net.BCrypt.HashPassword(password);

        public bool Verify(string password, string hashedPassowrd)
           => BCrypt.Net.BCrypt.Verify(password, hashedPassowrd);
    }
}
