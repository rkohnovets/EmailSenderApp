using Microsoft.AspNetCore.Mvc;
using EmailSenderApp.Models;
using EmailSenderApp.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmailSenderApp.Controllers
{
    [ApiController]
    [Route("api/mails")]
    public class MailsController : ControllerBase
    {
        private IMailsRepository MailsRepository { get; set; }
        
        public MailsController(IMailsRepository mailsRepository) => MailsRepository = mailsRepository;

        /// <summary>
        /// Get info about all the email messages sendings attempts (succeded or failed)
        /// Method: GET; Path: api/mails
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "Get all mails sent (or failed)")]
        public async Task<IActionResult> Get()
        {
            var mailsWithIds = await MailsRepository.GetAllMailsAsync();

            // "id" field remains
            //return Ok(mailsWithIds.Cast<Mail>());

            // "id" field not remains, this is what we need
            return Ok(mailsWithIds.Select(m => (Mail)m));
        }

        /// <summary>
        /// Send an email message
        /// Method: POST; Path: api/mails
        /// </summary>
        /// <param name="new_mail">The email message request form</param>
        /// <param name="mailSender">The service to send an email messages (injects using DI).</param>
        /// <returns></returns>
        [HttpPost(Name = "Try to send an email message")]
        public async Task<IActionResult> Post([FromBody] MailRequest new_mail, IMailSender mailSender)
        {
            Mail mail = new Mail()
            {
                body = new_mail.body,
                subject = new_mail.subject,
                recipients = new_mail.recipients
            };

            try {
                await mailSender.SendAsync(new_mail.recipients, new_mail.subject, new_mail.body);
                mail.Result = "OK";
                mail.FailedMessage = "";
            } catch(Exception ex) {
                mail.Result = "Failed";
                mail.FailedMessage = ex.Message;
            }
            await MailsRepository.CreateAsync(mail);
            return (mail.Result == "OK") ? Ok() : BadRequest();
        }
    }
}
