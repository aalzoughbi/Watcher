using System.Net;
using System.Net.Mail;

public class EmailService
{
    private string _fromEmail;
    private string _toEmail;
    private string _smtpServer;
    private int _smtpPort;
    private string _smtpUser;
    private string _smtpPass;
    private bool _useSsl;

    public EmailService(string fromEmail, string toEmail, string smtpServer, int smtpPort, string smtpUser, string smtpPass, bool useSsl)
    {
        _fromEmail = fromEmail;
        _toEmail = toEmail;
        _smtpServer = smtpServer;
        _smtpPort = smtpPort;
        _smtpUser = smtpUser;
        _smtpPass = smtpPass;
        _useSsl = useSsl;
    }

    public void SendEmail(string subject, string body, string attachmentPath)
    {
        using (MailMessage mail = new MailMessage())
        {
            mail.From = new MailAddress(_fromEmail);
            mail.To.Add(_toEmail);
            mail.Subject = subject;
            mail.Body = body;
            mail.Attachments.Add(new Attachment(attachmentPath));

            using (SmtpClient smtp = new SmtpClient(_smtpServer, _smtpPort))
            {
                smtp.Credentials = new NetworkCredential(_smtpUser, _smtpPass);
                smtp.EnableSsl = _useSsl; // Use the _useSsl field to set the EnableSsl property
                smtp.Send(mail);
            }
        }
    }
}
