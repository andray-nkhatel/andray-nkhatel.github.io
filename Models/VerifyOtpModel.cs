using System.ComponentModel.DataAnnotations;

namespace Rentbook.Models
{
    public class VerifyOtpModel
    {
       
        
            [Required]
        
            public string Email { get; set; }

            [Required]
            [StringLength(6, MinimumLength = 6, ErrorMessage = "OTP must be 6 digits long.")]
            public string Otp { get; set; }
        

    }
}
