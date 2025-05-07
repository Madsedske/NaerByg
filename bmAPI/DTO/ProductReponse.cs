namespace bmAPI.DTO
{
    public class ProductResponse
    {
        public required int ProductId { get; set; }
        public required string Sku { get; set; }
        public required string Name { get; set; }
        public required int CategoryId { get; set; }
        public required int BrandId { get; set; }
        public required string ImageURL { get; set; }
        public required string IsActive { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required DateTime UpdatedAt { get; set; }
    }
}
