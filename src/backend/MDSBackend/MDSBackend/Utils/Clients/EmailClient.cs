using System.Net.Mail;
using MDSBackend.Exceptions.UtilServices.Email;

namespace MDSBackend.Utils;

public class EmailClient(SmtpClient smtpClient, string emailFrom, ILogger<EmailClient> logger)
{
    #region Fields

    private readonly string _emailFrom = emailFrom;
    private readonly ILogger<EmailClient> _logger = logger;
    
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
            await smtpClient.SendMailAsync(email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw new SendEmailException("Failed to send email", ex);
        }
    }
    
    #endregion
}