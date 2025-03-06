using System.ComponentModel.DataAnnotations;

namespace EmailOtpSystemApi.DTO
{
    public class OtpVerificationRequest
    {
        // Making this as Mandatory 
        [Required]
        public string? Email { get; set; }

        // Making this as Mandatory 
        [Required]
        // The User Eneterd Input has minimum and Maximu length of 6 charecters. other wise attribute will throw error as OTP min and max length was not matched.
        [MinLength(6)]
        [MaxLength(6)]
        public string? Otp { get; set; }
    }
}
