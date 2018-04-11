using System;
using System.Configuration;
using System.Net.Mail;
using System.Net;

namespace Helper
{
    public class EmailSetting : ConfigurationSection
    {
        [ConfigurationProperty("host", DefaultValue = "smtp.gmail.com", IsRequired = true)]
        public string host
        {
            get
            {
                return (string)this["host"];
            }
            set
            {
                this["host"] = value;
            }
        }

        [ConfigurationProperty("port", DefaultValue = "587", IsRequired = true)]
        public int port
        {
            get
            {
                return (int)this["port"];
            }
            set
            {
                this["port"] = value;
            }
        }

        [ConfigurationProperty("username", DefaultValue = "username@email.com", IsRequired = true)]
        public string username
        {
            get
            {
                return (string)this["username"];
            }
            set
            {
                this["username"] = value;
            }
        }

        [ConfigurationProperty("password", DefaultValue = "urpassword", IsRequired = true)]
        public string password
        {
            get
            {
                return (string)this["password"];
            }
            set
            {
                this["password"] = value;
            }
        }

        [ConfigurationProperty("enableSsl", DefaultValue = "true", IsRequired = false)]
        public bool enableSsl
        {
            get
            {
                return (bool)this["enableSsl"];
            }
            set
            {
                this["enableSsl"] = value;
            }
        }

        [ConfigurationProperty("isBodyHtml", DefaultValue = "false", IsRequired = false)]
        public bool isBodyHtml
        {
            get
            {
                return (bool)this["isBodyHtml"];
            }
            set
            {
                this["isBodyHtml"] = value;
            }
        }

    }

    public class EmailResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
    }

    class EmailHelper
    {
        public static EmailResponse Send(string from, string to, string subject = "No Subject", string body = "No Body")
        {
            EmailSetting config = (EmailSetting)ConfigurationManager.GetSection("emailSetting");

            EmailResponse result = new EmailResponse();
            string errorMessage = null;

            var client = new SmtpClient(config.host, config.port)
            {
                Credentials = new NetworkCredential(config.username, config.password),
                EnableSsl = config.enableSsl
            };

            MailMessage msg = null;

            try
            {
                msg = new MailMessage(from, to, subject, body);
                msg.IsBodyHtml = config.isBodyHtml;

                client.Send(msg);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;

                result.Status = "ERROR";
                result.Message = errorMessage;
            }
            finally
            {
                if (msg != null)
                {
                    msg.Dispose();
                }

                if (errorMessage == null)
                {
                    result.Status = "SENT";
                }
            }

            return result;
        }


    }
}
