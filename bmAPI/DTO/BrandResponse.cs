namespace bmAPI.DTO
{
    public class BrandResponse
    {
        public required int BrandId { get; set; }
        public required string Name { get; set; }
        public required string IsActive { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required DateTime UpdatedAt { get; set; }
    }
}
