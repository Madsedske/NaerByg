namespace bmAPI.DTO
{
    public class AuthResponse
    {
        public required string Token { get; set; }

        public required DateTime ExpiryTime { get; set; }
    }
}
