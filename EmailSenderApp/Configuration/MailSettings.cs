namespace EmailSenderApp.Configuration
{
    /// <summary>
    /// The class represents configuration used to send email messages using SMTP.
    /// The data fills using configuration file (appsettings.json) in Program.cs.
    /// </summary>
    public class MailSettings
    {
        /// <summary>
        /// The readable name that recipient will see near the email address.
        /// For example, Roman Kokhnovets
        /// </summary>
        public string? DisplayName { get; set; }
        /// <summary>
        /// The email address which will be used to send email messages.
        /// Also used as username in SMTP server.
        /// </summary>
        public string? From { get; set; }
        /// <summary>
        /// Password to the email address in the field "From"
        /// </summary>
        public string? Password { get; set; }
        /// <summary>
        /// The SMTP server (for example, "smtp.ethereal.email")
        /// </summary>
        public string? Host { get; set; }
        /// <summary>
        /// The port used to connect to SMTP server
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// Whether you need to use SSL or not
        /// </summary>
        public bool UseSSL { get; set; }
    }
}
