namespace Rentbook.DTOs
{


    

    public class HouseDTO
    {
        public int Id { get; set; }
        public string Vicinity { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public decimal Price { get; set; }
        public string PropertyType { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public bool PetsAllowed { get; set; }
        public bool IsFurnished { get; set; }
        public List<string> Amenities { get; set; }
        public string Description { get; set; }

        public decimal SquareFootage { get; set; }
        public bool IsAvailable { get; set; }


    }
}
