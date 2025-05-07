namespace bmAPI.DTO
{
    public class MtmShopProductResponse
    {
        public required int ShopProductId { get; set; }
        public required int ShopId { get; set; }
        public required int ProductId { get; set; }
        public required int Quantity { get; set; }
        public required double Price { get; set; }
        public required string IsActive { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required DateTime UpdatedAt { get; set; }
    }
}
