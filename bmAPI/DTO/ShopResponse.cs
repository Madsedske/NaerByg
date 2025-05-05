namespace bmAPI.DTO
{
    public class ShopResponse
    {
        public required int ShopId { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required int PostAreaId { get; set; }
        public required string PhoneNo { get; set; }
        public required string OpeningHours { get; set; }
        public required string IsActive { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required DateTime UpdatedAt { get; set; }
    }
}
