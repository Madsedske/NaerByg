namespace bmAPI.DTO
{
    public class CategoryResponse
    {
        public required int CategoryId { get; set; }
        public required string Name { get; set; }
        public required int? ParentId { get; set; }
        public required string IsActive { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required DateTime UpdatedAt { get; set; }
    }
}
