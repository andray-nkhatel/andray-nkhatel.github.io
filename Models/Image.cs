using Rentbook.Services;
using System.Text.Json.Serialization;

namespace Rentbook.Models
{
    public class Image
    {
        public int Id { get; set; }
        public int HouseId { get; set; }
        public string ImageUrl { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime UploadedAt { get; set; }
    }
}
