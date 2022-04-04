namespace Core.Aspects.Performance
{
    public class SendMailDto
    {   
        public string Email { get; set; }
        public string Password { get; set; }
        public string SMTP { get; set; }
        public int Port { get; set; }
        public bool SSL { get; set; }
        public string email { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
    }
}