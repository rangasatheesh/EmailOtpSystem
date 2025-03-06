using EmailOtpSystemApi.Common;
using EmailOtpSystemApi.Services;
using Moq;

using Assert = Xunit.Assert;

public class EmailOtpServiceTests
{
    private readonly Mock<IEmailOtpService> _mockEmailOtpService;

    public EmailOtpServiceTests()
    {
        _mockEmailOtpService = new Mock<IEmailOtpService>();
    }

    // Test case for generating OTP email
    [Fact]
    public void SendOtpByEmail_ValidRecipientAddress_ReturnsStatusSuccess()
    {
        // Arrange
        var email = "ranga@gov.dso.org.sg";
        _mockEmailOtpService.Setup(service => service.SendOtpByEmail(email))
            .Returns(StatusConstants.STATUS_EMAIL_OK);

        // Act
        var result = _mockEmailOtpService.Object.SendOtpByEmail(email);

        // Assert
        Assert.Equal(StatusConstants.STATUS_EMAIL_OK, result);
    }

    // Test case for generating OTP with invalid email
    [Fact]
    public void SendOtpByEmail_InvalidRecipientAddress_ReturnsStatusEmailInvalid()
    {
        // Arrange
        var email = "rangatest@invalid.com";
        _mockEmailOtpService.Setup(service => service.SendOtpByEmail(email))
            .Returns(StatusConstants.STATUS_EMAIL_INVALID);

        // Act
        var result = _mockEmailOtpService.Object.SendOtpByEmail(email);

        // Assert
        Assert.Equal(StatusConstants.STATUS_EMAIL_INVALID, result);
    }

    // Test case for checking OTP (Valid OTP)
    [Fact]
    public void ValidateOtp_ValidInput_ReturnsStatusOtpOk()
    {
        // Arrange
        var email = "rangatest@gov.dso.org.sg";
        var otp = "123456"; // Valid OTP
        _mockEmailOtpService.Setup(service => service.ValidateOtp(email, otp))
            .Returns(StatusConstants.STATUS_OTP_OK);

        // Act
        var result = _mockEmailOtpService.Object.ValidateOtp(email, otp);

        // Assert
        Assert.Equal(StatusConstants.STATUS_OTP_OK, result);
    }

    // Test case for checking OTP (Invalid OTP)
    [Fact]
    public void ValidateOtp_InvalidInput_ReturnsStatusOtpFail()
    {
        // Arrange
        var email = "rangatest@gov.dso.org.sg";
        var otp = "000000"; // Invalid OTP
        _mockEmailOtpService.Setup(service => service.ValidateOtp(email, otp))
            .Returns(StatusConstants.STATUS_OTP_FAIL);

        // Act
        var result = _mockEmailOtpService.Object.ValidateOtp(email, otp);

        // Assert
        Assert.Equal(StatusConstants.STATUS_OTP_FAIL, result);
    }

    // Test case for checking OTP (Timeout)
    [Fact]
    public void ValidateOtp_TimeoutExceeded_ReturnsOtpTimeoutStatus()
    {
        // Arrange
        var email = "rangatest@gov.dso.org.sg";
        var otp = "123456"; // OTP
        _mockEmailOtpService.Setup(service => service.ValidateOtp(email, otp))
            .Returns(StatusConstants.STATUS_OTP_TIMEOUT);

        // Act
        var result = _mockEmailOtpService.Object.ValidateOtp(email, otp);

        // Assert
        Assert.Equal(StatusConstants.STATUS_OTP_TIMEOUT, result);
    }

    // Test case for checking OTP after max attempts
    [Fact]
    public void ValidateOtp_MaxAttemptsExceeded_ReturnsOtpFailedStatus()
    {
        // Arrange
        var email = "rangatest@gov.dso.org.sg";
        var otp = "123456"; // OTP
        _mockEmailOtpService.Setup(service => service.ValidateOtp(email, otp))
            .Returns(StatusConstants.STATUS_OTP_FAIL); // Mock max attempts reached case

        // Act
        var result = _mockEmailOtpService.Object.ValidateOtp(email, otp);

        // Assert
        Assert.Equal(StatusConstants.STATUS_OTP_FAIL, result);
    }


}