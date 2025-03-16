using System.Net.Mail;

namespace MDSBackend.Utils;

public class EmailClient
{
    #region Fields

    private readonly SmtpClient _smtpClient;
    private readonly string _emailFrom;

    #endregion
    
    public EmailClient(SmtpClient smtpClient, string emailFrom)
    {
        _smtpClient = smtpClient;
        _emailFrom = emailFrom;
    }
    
    public async Task SendEmail(MailMessage email)
    { 
        await  _smtpClient.SendMailAsync(email);
    }
    
    public MailMessage CreateEmail(string emailTo, string subject, string body)
    {
        var email = new MailMessage(_emailFrom, emailTo)
        {
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        return email;
    }
}