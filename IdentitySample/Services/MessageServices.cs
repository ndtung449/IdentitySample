namespace IdentitySample.Services
{
    using MailKit.Net.Smtp;
    using Microsoft.Extensions.Options;
    using MimeKit;
    using System;
    using System.Threading.Tasks;

    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link https://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        private readonly SendingEmailOptions _sendingEmailOptions;

        public AuthMessageSender(IOptions<SendingEmailOptions> container)
        {
            _sendingEmailOptions = container.Value;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            // https://social.technet.microsoft.com/wiki/contents/articles/37534.send-email-using-asp-net-core-1-1-with-mailkit-in-visual-studio-2017.aspx
            string ToAdressTitle = "Microsoft ASP.NET Core";
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(_sendingEmailOptions.FromAddressTitle, _sendingEmailOptions.FromAddress));
            mimeMessage.To.Add(new MailboxAddress(ToAdressTitle, email));
            mimeMessage.Subject = subject;
            mimeMessage.Body = new TextPart("plain")
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                client.Connect(_sendingEmailOptions.SmptServer, _sendingEmailOptions.SmtpPort, false);
                // Note: only needed if the SMTP server requires authentication 
                // Error 5.5.1 Authentication  
                client.Authenticate(_sendingEmailOptions.AuthenticationUserName, _sendingEmailOptions.AuthenticationPassword);
                client.Send(mimeMessage);
                Console.WriteLine("The mail has been sent successfully !!");
                Console.ReadLine();
                client.Disconnect(true);
            }

            return Task.FromResult(0);
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
