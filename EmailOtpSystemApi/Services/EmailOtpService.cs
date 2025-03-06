using System.Collections.Concurrent;
using System.Text.RegularExpressions;

using MimeKit;
using EmailOtpSystemApi.Common;

namespace EmailOtpSystemApi.Services
{
    public class EmailOtpService : IEmailOtpService
    {
        // Here will store the User Email and TimeStamp,OTP and Number attempts triggered by the user ranga.
        private readonly ConcurrentDictionary<string, (string otp, DateTime timestamp, int attempts)> _userOtpData
            = new ConcurrentDictionary<string, (string, DateTime, int)>();

        //This details will securely fetch from the Azure Key valut of DB with encryption mode. for local POC purpose I kept open. 
        private readonly string _smtpServer    = "smtp.example.com";
        private readonly string _smtpUsername  = "origin@tst.dso.gov.sg";
        private readonly string _smtpPassword  = "*********";
        private readonly string _fromEmail     = "noreply@tst.dso.gov.sg";

        /// <summary>
        /// Responsible to Validate the User Email and Trigger the Email to the User.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        public int SendOtpByEmail(string userEmail)
        {
            // Assemetions to validate the User Email 
            //"rangatest@gov.dso.org.sg"; -True
            //"invalid@example.com"; -False
            if (!Regex.IsMatch(userEmail, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.(dso\.org\.sg)$"))
            {
                return StatusConstants.STATUS_EMAIL_INVALID;
            }

            // Generate OTP (6 digits)
            var otp = new Random().Next(100000, 1000000).ToString();
            var timestamp = DateTime.Now.AddMinutes(1);

            // Store OTP and timestamp for the user
            _userOtpData[userEmail] = (otp, timestamp, 0);

            // Email body ,the Generated OTP will be attached to the mail body evey new Request triggered. 
            var emailBody = $"Your OTP Code is {otp}. The code is valid for 1 minute.";

            if (SendEmail(userEmail, emailBody))
            {
                // Consider as Email Trigger sucessfull.
                return StatusConstants.STATUS_EMAIL_OK;
            }
            else
            {
                // This will not be called ,because in the Send Email method we are returning as true always.
                // In the real envirement if the mail SMTP cred was wrong then this will be going to call and retrun the status as fail
                return StatusConstants.STATUS_EMAIL_FAIL;
            }
        }

        /// <summary>
        /// Validate the User Enter Email is Exists in the Sent Email List
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="otp"></param>
        /// <returns></returns>
        public int ValidateOtp(string userEmail, string otp)
        {
            // Checking is Email is Existed in the Sent Emails List
            if (!_userOtpData.ContainsKey(userEmail))
            {
                return StatusConstants.STATUS_OTP_FAIL; // OTP not generated yet
            }
            // Destruct the user email properties from the store based on the email
            var (storedOtp, timestamp, attempts) = _userOtpData[userEmail];

            // Check if OTP has expired (1 minute timeout)
            if (DateTime.Now.Subtract(timestamp).TotalSeconds > 60)
            {
                _userOtpData.TryRemove(userEmail, out _);
                return StatusConstants.STATUS_OTP_TIMEOUT;
            }

            // Allow only 10 attempts to enter OTP
            if (attempts >= 10)
            {
                return StatusConstants.STATUS_OTP_FAIL;
            }

            // Check if OTP is correct
            if (otp == storedOtp)
            {
                _userOtpData.TryRemove(userEmail, out _); // Remove OTP after successful verification
                return StatusConstants.STATUS_OTP_OK;
            }
            else
            {
                // Increment attempt count
                _userOtpData[userEmail] = (storedOtp, timestamp, attempts + 1);
                return StatusConstants.STATUS_OTP_FAIL;
            }
        }

        private bool SendEmail(string toEmail, string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("OTP Service", _fromEmail));
                var toEmaillst = new List<MailboxAddress>() {
                new MailboxAddress("",toEmail)
                };
                message.To.AddRange(toEmaillst);
                message.Subject = "OTP Code";
                message.Body = new TextPart("plain")
                {
                    Text = body
                };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {                    
                    return true;
                    //Consider we have the correct SMTP server details and port info.
                    // connect the SMTP server 
                    //await client.ConnectAsync(_smtpServer, 587, false);
                    // Authenticate with SMTP
                    //await client.AuthenticateAsync(_smtpUsername, _smtpPassword);
                    //Trigger the Email
                    //await client.SendAsync(message);
                    //AFter Email Trigger disconnect the connection
                    //await client.DisconnectAsync(true);
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
