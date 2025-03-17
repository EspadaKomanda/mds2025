using System.Net.Mail;
using MDSBackend.Exceptions.UtilServices.Email;

namespace MDSBackend.Utils;

public class EmailClient
{
    #region Fields

    private readonly SmtpClient _smtpClient;
    private readonly string _emailFrom;

    #endregion
    
    #region Constructor
    
    public EmailClient(SmtpClient smtpClient, string emailFrom)
    {
        _smtpClient = smtpClient;
        _emailFrom = emailFrom;
    }
    
    #endregion
    
    #region Methods
    
    /// <summary>
    /// Sends the email using the SmtpClient instance.
    /// </summary>
    /// <param name="email">Email to send.</param>
    /// <exception cref="SendEmailException">If the SmtpClient instance fails to send the email.</exception>
    /// <returns>Task that represents the asynchronous operation.</returns>
    public async Task SendEmail(MailMessage email, string emailTo)
    {
        try
        {
            email.To.Add(new MailAddress(emailTo));
            await _smtpClient.SendMailAsync(email);
        }
        catch (Exception ex)
        {
            throw new SendEmailException("Failed to send email", ex);
        }
    }
    
    #endregion
}