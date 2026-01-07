using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace Uniceps.app.Services
{
    public class SmtpSettings
    {
        public string? Server { get; set; }
        public int Port { get; set; }
        public string? SenderEmail { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
    public class EmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SendEmailAsync(string email, int otp)
        {
            var smtpClient = new SmtpClient(_smtpSettings.Server)
            {
                Port = _smtpSettings.Port,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                EnableSsl = true,
            };
            string htmlBody = $@"
    <div style='background-color: #111111; font-family: sans-serif; text-align: center; color: #ffffff;'>
        <div style='max-width: 400px; margin: 0 auto; background-color: #111111; border: 1px solid #22d3ee33; border-radius: 24px; padding: 40px; box-shadow: 0 10px 30px rgba(0,0,0,0.5);'>
            
            <h1 style='color: #22d3ee; margin-bottom: 10px; font-size: 28px; letter-spacing: -1px;'>UNICEPS</h1>
            <p style='color: #666; font-size: 12px; text-transform: uppercase; letter-spacing: 2px; margin-bottom: 30px;'>Fitness Ecosystem</p>
            
            <div style='height: 1px; background: linear-gradient(to right, transparent, #22d3ee, transparent); margin-bottom: 30px;'></div>
            
            <h2 style='color: #ffffff; font-size: 20px; margin-bottom: 10px;'>Verify Your Account</h2>
            <p style='color: #aaaaaa; font-size: 14px; margin-bottom: 30px;'>Use the code below to complete your secure login.</p>
            
            <div style='background-color: #000000; border: 1px dashed #22d3ee; border-radius: 12px; padding: 20px; margin-bottom: 30px;'>
                <span style='font-family: monospace; font-size: 25px; font-weight: bold; color: #22d3ee; letter-spacing: 10px; display: block;'>{otp}</span>
            </div>
            
            <p style='color: #555; font-size: 12px; margin-top: 20px;'>
                If you didn't request this, please ignore this email.<br>
                Code expires in 10 minutes.
            </p>
            
            <div style='margin-top: 40px; border-top: 1px solid #222; pt: 20px;'>
                <p style='color: #444; font-size: 10px;'>© 2025 TRIOVERSE - UNICEPS</p>
            </div>
        </div>
    </div>";
            var mailMessage = new MailMessage
            {
                From = new MailAddress("Unicepse@gmail.com"),
                Subject = "Your OTP Code",
                Body = htmlBody,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(email);
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
