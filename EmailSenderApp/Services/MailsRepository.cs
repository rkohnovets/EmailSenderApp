using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using EmailSenderApp.Models;

namespace EmailSenderApp.Services
{
    /// <summary>
    /// The service providing storage of attempts of sending email messages 
    /// and their results (using Dapper and MS SQL Server).
    /// The service not sends the messages, just stores info about attempts to send them.
    /// </summary>
    public interface IMailsRepository
    {
        /// <summary>
        /// Add info about an email message to storage (DB)
        /// </summary>
        /// <param name="mail">Info about the email message attempt</param>
        /// <returns></returns>
        Task CreateAsync(Mail mail);
        /// <summary>
        /// Get info about all messages sent or failed.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<MailWithId>> GetAllMailsAsync();
    }
    public class MailsRepository : IMailsRepository
    {
        private string connectionString;
        public MailsRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public async Task<IEnumerable<MailWithId>> GetAllMailsAsync()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                // get mails without recipients
                var mails = await db.QueryAsync<MailWithId>("SELECT * FROM Mails");
                var dict = mails.ToDictionary(m => m.id);
                // set recipients
                var recipientsTable = await db.QueryAsync<MailRecipient>("SELECT * FROM MailRecipients");
                foreach (var item in recipientsTable)
                    dict[item.mail_id].recipients.Add(item.recipient);
                return mails;
            }
        }

        public async Task CreateAsync(Mail mail)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "INSERT INTO Mails (subject, body, Result, FailedMessage, CreatedDate) VALUES(@subject, @body, @Result, @FailedMessage, GETDATE()); SELECT CAST(SCOPE_IDENTITY() as int)";
                int mail_id = await db.QueryFirstAsync<int>(sqlQuery, mail);

                foreach (var recipient in mail.recipients)
                {
                    await db.ExecuteAsync(
                        "INSERT INTO MailRecipients (mail_id, recipient) VALUES(@mail_id, @recipient)", 
                        new { mail_id, recipient }
                    );
                }
            }
        }
    }
}
