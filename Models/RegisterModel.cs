using System.ComponentModel.DataAnnotations;

namespace Rentbook.Models
{
    public class RegisterModel
    {
        [Required]
        public string Role { get; set; }

        [Required]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "The phone number must be in the form 0977123456.")]
        public string PhoneNumber { get; set; }

        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}