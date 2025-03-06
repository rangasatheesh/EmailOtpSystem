using System;
using System.Net;
using System.Net.Mail;
using System.Threading;
using MailKit.Net.Smtp;
using MimeKit;
using SmtpClient = System.Net.Mail.SmtpClient;

class Program
{
    // Email server details
    private static string _smtpServer = "smtp.gmail.com";            // Example for Gmail SMTP server
    private static string _smtpUsername = "your-email@gmail.com";    // Use your email
    private static string _smtpPassword = "your-email-password";     // Use your email app password
    private static string _fromEmail = "your-email@gmail.com";       // Use your email

    // Method to generate a random OTP
    public static string GenerateOtp(int length = 6)
    {
        var rand = new Random();
        var otp = "";
        for (int i = 0; i < length; i++)
        {
            otp += rand.Next(0, 10);  // Generate random number between 0 and 9
        }
        return otp;
    }

    // Method to send the OTP to a given email address
    public static bool SendOtpEmail(string toEmail, string otp)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("OTP Service", _fromEmail));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = "Your OTP Code";
            message.Body = new TextPart("plain") { Text = $"Your OTP code is: {otp}" };
            return true;
            //using (var client = new SmtpClient())
            //{
            //    client.Connect(_smtpServer, 587, false);  // Connect to the SMTP server (587 is for TLS)
            //    client.Authenticate(_smtpUsername, _smtpPassword);  // Authenticate with email credentials
            //    client.Send(message);  // Send the OTP email
            //    client.Disconnect(true);  // Disconnect after sending
            //    return true;
            //}
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
            return false;
        }
    }

    // Method to verify OTP entered by user
    public static bool VerifyOtp(string correctOtp)
    {
        Console.WriteLine("Enter the OTP sent to your email: ");
        string userOtp = Console.ReadLine();

        return userOtp == correctOtp;
    }

    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the OTP System!");

        // Step 1: Get the email address from the user
        Console.Write("Please enter your email address: ");
        string userEmail = Console.ReadLine();

        // Step 2: Generate a random OTP
        string otp = GenerateOtp();

        // Step 3: Send OTP to the user's email
        if (SendOtpEmail(userEmail, otp))
        {
            Console.WriteLine("OTP has been sent to your email. Please check your inbox.");
        }
        else
        {
            Console.WriteLine("Failed to send OTP. Please try again later.");
            return;
        }

        // Step 4: Give user time to check email (optional)
        Thread.Sleep(5000);  // Optional: Wait for 5 seconds

        // Step 5: Ask user to enter OTP
        if (VerifyOtp(otp))
        {
            Console.WriteLine("OTP verified successfully!");
        }
        else
        {
            Console.WriteLine("Invalid OTP. Please try again.");
        }
    }
}
