using System.ComponentModel.DataAnnotations;

namespace Rentbook.DTOs
{
    public class UpdatedHouseDTO: CreatedHouseDTO
    {
      
        public DateTime LastUpdated { get; set; }
        public List<string> Amenities { get; set; } = new List<string>();

    }
}
