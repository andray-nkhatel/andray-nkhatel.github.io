using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Rentbook.DTOs
{

    public class CreateHouseDTO
    {
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Vicinity is required")]
        public string Vicinity { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Price is required")]
       
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Property type is required")]
        public string PropertyType { get; set; }

        [Required(ErrorMessage = "Number of bedrooms is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Number of bedrooms must be a positive number")]
        public int Bedrooms { get; set; }

        [Required(ErrorMessage = "Number of bathrooms is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Number of bathrooms must be a positive number")]
        public int Bathrooms { get; set; }

        
        public decimal SquareFootage { get; set; }
        public bool IsFurnished { get; set; }

        public bool PetsAllowed { get; set; }

        public string Description { get; set; }

        public bool IsAvailable { get; set; }

        public List<string> Amenities { get; set; }
    }

    public class UpdateHouseDTO : CreateHouseDTO
    {
        [Required(ErrorMessage = "Id is required for updates")]
        public int Id { get; set; }
    }

}