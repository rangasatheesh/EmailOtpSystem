namespace EmailOtpSystemApi.Common
{
    public static class StatusConstants
    {
        public const int STATUS_EMAIL_OK      = 1;
        public const int STATUS_EMAIL_FAIL    = 2;
        public const int STATUS_EMAIL_INVALID = 3;
        public const int STATUS_OTP_OK        = 4;
        public const int STATUS_OTP_FAIL      = 5;
        public const int STATUS_OTP_TIMEOUT   = 6;
    }
}
