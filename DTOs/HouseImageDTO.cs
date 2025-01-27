namespace Rentbook.DTOs
{
    public class HouseImageDTO
    {
        public int Id { get; set; }
        public int HouseId { get; set; }
        public string ImageUrl { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
