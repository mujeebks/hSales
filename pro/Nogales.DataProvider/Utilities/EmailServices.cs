using Nogales.DataProvider.EmailTemplates;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Nogales.DataProvider.Utilities
{
    public class EmailServices : EmailCore
    {
        internal bool Payroll(string fromDate, string toDate,string function, string execDate, string error)
        {
            try
            {
                MailMessage mailMsg = new System.Net.Mail.MailMessage();



                var applicationUrl = ConfigurationManager.AppSettings["ApplicationUrl"];
                var errorlogEmail = ConfigurationManager.AppSettings["ErrorLogEmail"];
                mailMsg.To.Add(errorlogEmail);
                

                #region mailBody

                string mailBody = GetMailBody(fromDate,toDate,execDate,function,error);

                #endregion

                // From
                mailMsg.From = new MailAddress(ConfigurationManager.AppSettings["DefaultEmailFrom"]);
                mailMsg.IsBodyHtml = true;
                //mailMsg.Body = string.Format("<html> <head> </head> <body><b>Hello {0} {1},</b> <br/> <p> Welcome to Nogales Portal. Please find the below credentials to login to the system. </p> <p> User Name : {2} <br/> Password : {3} <br/> Click <a href='{4}'> here </a> to login to the system </p> <br/> <p> Thanks, </p> <p> noreply@nogalesproduce.com </p> <body> </html>",
                //                                model.FirstName, model.LastName, model.Email, randomPassword, applicationUrl);
                mailMsg.Body = mailBody;
                mailMsg.Subject = "Nogales Dashboard Payroll Import Failed";
                mailMsg.IsBodyHtml = true;
                mailMsg.BodyEncoding = Encoding.UTF8;
                SmtpClient smtpClient = base.sendGridCredentials();
                smtpClient.Send(mailMsg);
                return true;

                throw new Exception("To receipt should not be null");
            }
            catch (Exception ex)
            {
                // Handle API exceptions
                return false;
            }
        }


        private static string GetMailBody(string fromDate, string toDate, string exeDate, string function, string error)
        {
            string mailBody = string.Empty;

            mailBody = EmailTemplateResources.PayrollErrorLog;
            

            mailBody = mailBody.Replace("{{FROMDATE}}", fromDate);
            mailBody = mailBody.Replace("{{TODATE}}", toDate);
            mailBody = mailBody.Replace("{{EXEDATE}}", exeDate);
            mailBody = mailBody.Replace("{{Function}}", function);
            mailBody = mailBody.Replace("{{ERRORREPORT}}", error);
   
            return mailBody;
        }
    }
}
