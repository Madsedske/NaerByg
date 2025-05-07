namespace bmAPI.DTO
{
    public class PostAreaResponse
    {
        public required int PostAreaId { get; set; }
        public required int Code { get; set; }
        public required string City { get; set; }
        public required string IsActive { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required DateTime UpdatedAt { get; set; }
    }
}
