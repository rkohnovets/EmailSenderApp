namespace EmailSenderApp.Models
{
    /// <summary>
    /// The request to send an email message (from client)
    /// </summary>
    public class MailRequest
    {
        /// <summary>
        /// Subject of the email message
        /// </summary>
        public string subject { get; set; }
        /// <summary>
        /// Body of the email message
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// Enumeration of email addresses to whom you send the email message.
        /// Note that in DB recipiens are storaging in the table "MailRecipients", not in "Mails"
        /// </summary>
        public List<string> recipients { get; set; } = new List<string>();
    }
    /// <summary>
    /// The email message with the results of sending.
    /// </summary>
    public class Mail : MailRequest
    {
        /// <summary>
        /// Result of sending the email message - "OK" or "Failed"
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// If the result of sending is "Failed" then here you can see 
        /// the message of exception that failed the sending of the message
        /// </summary>
        public string FailedMessage { get; set; }
        /// <summary>
        /// Date and time when the sending attempt happened
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }

    public class MailWithId : Mail
    {
        /// <summary>
        /// id of the email message attempt in the DB table "Mails"
        /// </summary>
        public int id { get; set; }
    }

    /// <summary>
    /// Represents how the email message recipients storaged in the DB (table "MailRecipients")
    /// </summary>
    public struct MailRecipient
    {
        /// <summary>
        /// id of the email message in the DB table "Mails"
        /// </summary>
        public int mail_id { get; set; }
        /// <summary>
        /// The recipient email address (for example, rkohnovets@mail.ru)
        /// </summary>
        public string recipient { get; set; }
    }
}