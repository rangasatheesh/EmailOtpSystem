namespace EmailOtpSystemApi.Services
{
    public interface IEmailOtpService
    {
        int SendOtpByEmail(string userEmail);
        int ValidateOtp(string userEmail, string otp);        
    }
}
