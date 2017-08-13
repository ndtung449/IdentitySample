namespace IdentitySample.Services
{
    public class SendingEmailOptions
    {
        public string FromAddress { get; set; }
        public string FromAddressTitle { get; set; }
        public string ToAddressTitle { get; set; }
        public string AuthenticationUserName { get; set; }
        public string AuthenticationPassword { get; set; }
        public string SmptServer { get; set; }
        public int SmtpPort { get; set; }
    }
}
