

using Rentbook.Services;
using System.Text.Json.Serialization;

namespace Rentbook.Models
{
    public class House
    {
        public int Id { get; set; }
        public string Vicinity { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string OwnerId { get; set; }
        public string OwnerFirstName { get; set; }
        public string OwnerLastName { get; set; }
        public string OwnerContact { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime CreatedAt { get; set; }
        public List<Image> Images { get; set; }
        public decimal Price { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public decimal SquareFootage { get; set; }
        public string PropertyType { get; set; }
        public bool IsFurnished { get; set; }
        public bool PetsAllowed { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime AvailableFrom { get; set; }
        public string Description { get; set; }
        public List<string> Amenities { get; set; }
        public bool IsAvailable { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? LastUpdated { get; set; }
    }

   

}
