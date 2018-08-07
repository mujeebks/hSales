using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Nogales.API.Utilities
{
    /// <summary>
    /// Core functionalities of the Email Service
    /// </summary>
    public class EmailCore
    {
        /// <summary>
        /// Get SmtpClient of SendGrid
        /// </summary>
        /// <returns></returns>
        public SmtpClient sendGridCredentials()
        {
            SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SendGridUsername"], ConfigurationManager.AppSettings["SendGridPassword"]);
            smtpClient.Credentials = credentials;
            return smtpClient;
        }
    }
}