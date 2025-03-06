namespace EmailOtpSystemApi.Services
{
    public interface IEmailOtpService
    {
        int GenerateOtpEmail(string userEmail);
        int CheckOtp(string userEmail, string otp);        
    }
}
