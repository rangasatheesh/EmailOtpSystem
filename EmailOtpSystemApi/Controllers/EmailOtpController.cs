using EmailOtpSystemApi.Common;
using EmailOtpSystemApi.DTO;
using EmailOtpSystemApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmailOtpSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailOtpController : ControllerBase
    {
        private readonly IEmailOtpService _emailOtpService;

        public EmailOtpController(IEmailOtpService emailOtpService)
        {
            _emailOtpService = emailOtpService;
        }

        // Endpoint to generate OTP and send it via email
        [HttpPost("generate")]
        public IActionResult GenerateOtp([FromBody] EmailRequest emailRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return the validation errors
            }
            {
                var result = _emailOtpService.SendOtpByEmail(emailRequest.Email!);
                switch (result)
                {
                    case StatusConstants.STATUS_EMAIL_OK:
                        return Ok($"OTP {result} sent successfully!");
                    case StatusConstants.STATUS_EMAIL_FAIL:
                        return StatusCode(500, "Failed to send OTP email.");
                    case StatusConstants.STATUS_EMAIL_INVALID:
                        return BadRequest("Invalid email address.");
                    default:
                        return StatusCode(500, "An unexpected error occurred.");
                }
            }
        }

        // Endpoint to verify OTP entered by the user
        [HttpPost("verify")]
        public IActionResult VerifyOtp([FromBody] OtpVerificationRequest otpRequest)
        {
            var result = _emailOtpService.ValidateOtp(otpRequest.Email!, otpRequest.Otp!);
            switch (result)
            {
                case StatusConstants.STATUS_OTP_OK:
                    return Ok("OTP verified successfully!");
                case StatusConstants.STATUS_OTP_FAIL:
                    return BadRequest("Incorrect OTP or failed after 10 attempts.");
                case StatusConstants.STATUS_OTP_TIMEOUT:
                    return BadRequest("OTP expired.");
                default:
                    return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }   
}
