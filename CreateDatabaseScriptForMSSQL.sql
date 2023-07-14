CREATE DATABASE MailsForMonkDL;

GO

use MailsForMonkDL;

CREATE TABLE Mails
(
    id INT PRIMARY KEY IDENTITY,
    subject VARCHAR(MAX),
	body VARCHAR(MAX),
	Result VARCHAR(6),
	FailedMessage VARCHAR(MAX),
	CreatedDate DATETIME
)

CREATE TABLE MailRecipients
(
	mail_id INT FOREIGN KEY REFERENCES Mails(id),
	recipient VARCHAR(MAX)
)