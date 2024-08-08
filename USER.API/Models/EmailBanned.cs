namespace USER.API.Models
{
    public class EmailBanned
    {
        public string ToMail { get; set; }
        public string Subject { get; set; }
        public string HtmlContent { get; set; }
    }
}
