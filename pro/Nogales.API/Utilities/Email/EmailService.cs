using Nogales.API.Models;
using Nogales.BusinessModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace Nogales.API.Utilities
{
    /// <summary>
    /// Service to send e-mails
    /// </summary>
    public class EmailService : EmailCore
    {
        /// <summary>
        /// Send warehouse shortage reports to the emails specified in the parameter "toList"
        /// 
        /// Exceptions Messages:
        ///             1) To receipt should not be null : When there is no emails in the parameter "toList"
        ///             2) An error occured while sending email : When there is any error upon sending e-mail using SendGrid
        /// </summary>
        /// <param name="attachment"> MemoryStream of the excel file which should be send as attachment </param>
        /// <param name="toList"> List of emails to which the e-mail need to send</param>
        /// <returns></returns>
        public bool SendWarehouseShortageEmail(MemoryStream attachment, List<string> toList, List<WarehouseShortReportBM> data, string date)
        {
            try
            {
                MailMessage mailMsg = new MailMessage();
                if (toList != null && toList.Count > 0)
                {
                    foreach (var item in toList)
                    {
                        // add To
                        mailMsg.To.Add(new MailAddress(item));
                    }

                    // From
                    mailMsg.From = new MailAddress(ConfigurationManager.AppSettings["DefaultEmailFrom"]);
                    mailMsg.IsBodyHtml = true;
                    mailMsg.Body = "<html> <head> </head> <body><p>Nogales - Warehouse Shortage Report for " + date +"</p><br/ ><p>" + GenerateSOShortReportEmailContent(data) + "</p> <br/> </body> </html>";
                    mailMsg.Subject = string.Format("Nogales - Warehouse Shortage Report for {0}" , date);
                    #region Adding attachment
                    System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType("application/vnd.ms-excel");
                    System.Net.Mail.Attachment attach = new System.Net.Mail.Attachment(attachment, contentType);
                    attach.ContentDisposition.FileName = "report.xlsx";
                    mailMsg.Attachments.Add(attach);

                    #endregion                    mailMsg.IsBodyHtml = true;
                    mailMsg.BodyEncoding = Encoding.UTF8;
                    SmtpClient smtpClient = sendGridCredentials();
                    smtpClient.Send(mailMsg);
                    return true;
                }
                throw new Exception("To receipt should not be null");
            }
            catch (Exception e)
            {
                // Handle API exceptions
                throw new Exception("An error occured while sending email - " + e.Message);
            }
        }

        /// <summary>
        /// Generate SO Short Report Email Content
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private StringBuilder GenerateSOShortReportEmailContent(List<WarehouseShortReportBM> data)
        {
            try
            {
                StringBuilder xlsxEmailContent = new StringBuilder();
                if (data != null && data.Count() > 0)
                {
                    xlsxEmailContent.Append(
                                                @"<table style='border-collapse: collapse;  border: 1px solid black;'>
                                                    <tr>
                                                        <th style='border: 1px solid black;'>Route</th> <th style='border: 1px solid black;'>Customer</th> 
                                                        <th style='border: 1px solid black;'>Item</th> <th style='border: 1px solid black;'>Description</th> 
                                                        <th style='border: 1px solid black;'>Buyer</th> 
                                                        <th style='border: 1px solid black;'>UOM</th> 
                                                        <th style='border: 1px solid black;'>Quantity Needed</th> 
                                                        <th style='border: 1px solid black;'>Quantity On Hand</th> 
                                                        <th style='border: 1px solid black;'>Quantity Available </th> 
                                                        <th style='border: 1px solid black;'>Transaction Cost</th> 
                                                        <th style='border: 1px solid black;'>Market Price</th> 
                                                        <th style='border: 1px solid black;'>Sales Order Number</th>
                                                </tr>");
                    foreach (var item in data)
                    {
                        xlsxEmailContent.Append("<tr><td  style='border: 1px solid black;'>" + item.Route + "</td><td  style='border: 1px solid black;'>" + item.Customer + "</td> <td  style='border: 1px solid black;'>" + item.Item + "</td> <td  style='border: 1px solid black;'>" + item.Description + "</td> <td  style='border: 1px solid black;'>" + item.Buyer + "</td> <td  style='border: 1px solid black;'>" + item.UOM + "</td> <td  style='border: 1px solid black;'>" + Math.Round(item.QuantityNeeded, 2) + "</td> <td  style='border: 1px solid black;'>" + Math.Round(item.QuantityOnHand, 2) + "</td> <td  style='border: 1px solid black;'>" + Math.Round(item.QtyAvailable, 2) + "</td> <td  style='border: 1px solid black;'>" + Math.Round(item.TransactionCost, 2) + "</td> <td  style='border: 1px solid black;'>" + Math.Round(item.MarketPrice, 2) + "</td> <td  style='border: 1px solid black;'>" + item.SalesOrderNumber + "</td></tr>");
                    }

                    xlsxEmailContent.Append("</table>");
                }
                return xlsxEmailContent;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while creating email content - " + ex.Message);
            }
        }

        internal bool SendWelcomeEmail(RegisterBindingModel model, string randomPassword)
        {
            try
            {
                MailMessage mailMsg = new System.Net.Mail.MailMessage();

                if (model != null && model.Email != "")
                {

                    var applicationUrl = ConfigurationManager.AppSettings["ApplicationUrl"];
                    mailMsg.To.Add(new MailAddress(model.Email));

                    #region mailBody

                    string mailBody = GetMailBody(
                        "Welcome to Nogales Dashboard"
                        , model.FirstName + " " + model.LastName
                        , "Your Nogales Dashboard account is ready"
                        , "Username"
                        , model.Email
                        , "Password"
                        , randomPassword
                         , "Click here to login - <a href='" + applicationUrl + "'>"+ applicationUrl + "</a>" 
                        // , "Click <a href='" + applicationUrl + "'>here</a> to login to the system "
                        );

                    #endregion

                    // From
                    mailMsg.From = new MailAddress(ConfigurationManager.AppSettings["DefaultEmailFrom"]);
                    mailMsg.IsBodyHtml = true;
                    //mailMsg.Body = string.Format("<html> <head> </head> <body><b>Hello {0} {1},</b> <br/> <p> Welcome to Nogales Portal. Please find the below credentials to login to the system. </p> <p> User Name : {2} <br/> Password : {3} <br/> Click <a href='{4}'> here </a> to login to the system </p> <br/> <p> Thanks, </p> <p> noreply@nogalesproduce.com </p> <body> </html>",
                    //                                model.FirstName, model.LastName, model.Email, randomPassword, applicationUrl);
                    mailMsg.Body = mailBody;
                    mailMsg.Subject = "Welcome to Nogales Dashboard";
                    mailMsg.IsBodyHtml = true;
                    mailMsg.BodyEncoding = Encoding.UTF8;
                    SmtpClient smtpClient = base.sendGridCredentials();
                    smtpClient.Send(mailMsg);
                    return true;
                }
                throw new Exception("To receipt should not be null");
            }
            catch (Exception ex)
            {
                // Handle API exceptions
                return false;
            }
        }

        internal bool SendResetPasswordEmail(ApplicationUser model, string newPassword)
        {
            try
            {
                System.Net.Mail.MailMessage mailMsg = new System.Net.Mail.MailMessage();

                if (model != null && model.Email != "")
                {
                    mailMsg.To.Add(new MailAddress(model.Email));

                    var applicationUrl = ConfigurationManager.AppSettings["ApplicationUrl"];


                    #region mailBody

                    string mailBody = GetMailBody(
                        "Password Changed"
                        , model.FirstName + " " + model.LastName
                        , "Your password was recently changed."
                        , "New Password"
                        , newPassword
                        , string.Empty
                        , string.Empty
                        , "Click here to login - <a href='" + applicationUrl + "'>" + applicationUrl + "</a>"
                        //  , "Click <a href='" + applicationUrl + "'> here </a> to login to the system "
                        );

                    #endregion

                    // From
                    mailMsg.From = new MailAddress(ConfigurationManager.AppSettings["DefaultEmailFrom"]);
                    mailMsg.IsBodyHtml = true;
                    //mailMsg.Body = string.Format("<html> <head> </head> <body><b>Hello {0} {1},</b> <br/> <p> Your password was recently reset. Please find the new password below.</p> <p> New Password : {2} <br/> Click <a href='{3}'> here </a> to login to the system </p> <br/> <p> Thanks, </p> <p> noreply@nogalesproduce.com </p> <body> </html>",
                    //                                model.FirstName, model.LastName, newPassword, applicationUrl);
                    int Pos1 = mailBody.IndexOf("<!--RemoveBegin-->") + "<!--RemoveBegin-->".Length;
                    int Pos2 = mailBody.IndexOf("<!--RemoveEnd-->");
                    string FinalString = mailBody.Substring(Pos1, Pos2 - Pos1);
                    mailBody = mailBody.Replace(FinalString, string.Empty);
                    mailMsg.Body = mailBody;
                    mailMsg.Subject = "Nogales Dashboard: Password Changed";
                    mailMsg.IsBodyHtml = true;
                    mailMsg.BodyEncoding = Encoding.UTF8;
                    SmtpClient smtpClient = base.sendGridCredentials();
                    smtpClient.Send(mailMsg);
                    return true;
                }
                throw new Exception("To receipt should not be null");
            }
            catch (Exception e)
            {
                // Handle API exceptions
                return false;
            }
        }

        internal bool SendCustomerForm(NameValueCollection form, string physicalFilePath)
        {
            try
            {
                string content = $"<html><head> <title>Nogales Produce - Customer Form</title></head><body> <table style='border:1px solid gray;margin-top:10px;width:700px;' align='center' cellpadding=5 cellspacing=0> <tbody> <tr style='border-bottom:1px dotted gray;'> <td align='center' colspan='3'> <img src='http://nogalesproduce.com/images/logo-header1.png' alt='NogalesProduce'/> <br/> <span>8220 Forney Road, Dallas, TX, 75227</span><br/> <span>Local (214) 275-3500 Order Online (214) 421-4140</span><br/> <span>FAX: (214) 275-3539 (Sales)</span><br/> <span>FAX: (214) 275-3575 (Accounting)</span><br/> <br/> </td></tr><tr> <td style='color:#808080;'>BUSINESS NAME</td><td>{form["BusinessName"]}</td><td colspan=3></td></tr><tr> <td style='color:#808080;'>EMAIL</td><td>{form["Email"]}</td><td colspan=3></td></tr><tr> <td style='color:#808080;'>WEBSITE</td><td>{form["Website"]}</td><td colspan=3></td></tr><tr> <td style='color:#808080;'>TYPE OF BUSINESS</td><td>{form["BusinessType"]}</td><td colspan=3></td></tr><tr> <td style='color:#808080;'>RESALE CERT#</td><td>{form["ResaleCert"]}</td><td colspan=3></td></tr><tr> <td colspan=5> <h4 style='padding-top:7px'>BILLING ADDRESS</h4> </td></tr><tr> <td colspan=5>{form["BillToAddress"]}, <br/>{form["bill.City"]},{form["bill.State"]},{form["bill.PostalCode"]}<br/>{form["bill.Country"]}</td></tr><tr> <td colspan=5> <h4 style='padding-top:7px'>SHIPPING ADDRESS</h4> </td></tr><tr> <td colspan=5>{form["ShipToAddress"]}, <br/>{form["ship.City"]},{form["ship.State"]},{form["ship.PostalCode"]}<br/>{form["ship.Country"]}</td></tr><tr> <td colspan=5> <h4 style='padding-top:7px'>CONTACT AT LOCATION</h4> </td></tr><tr> <td style='color:#808080;'>Name</td><td>{form["ContactAtLocation"]}</td><td colspan=3></td></tr><tr> <td style='color:#808080;'>Phone</td><td>{form["Phone"]}</td><td colspan=3></td></tr><tr> <td style='color:#808080;'>Fax</td><td>{form["Fax"]}</td><td colspan=3></td></tr><tr> <td colspan=5> <h4 style='padding-top:7px'>ACCOUNT PAYABLE CONTACT</h4> </td></tr><tr> <td style='color:#808080;'>Name</td><td>{form["AccountPayable"]}</td><td colspan=3></td></tr><tr> <td style='color:#808080;'>Phone</td><td>{form["AccountPayable.Phone"]}</td><td colspan=3></td></tr><tr> <td style='color:#808080;'>Fax</td><td>{form["AccountPayable.Fax"]}</td><td colspan=3></td></tr><tr> <td style='color:#808080;'>Delivery, To</td><td>{form["DeliveryWindow"]},{form["To"]}</td><td colspan=3></td></tr><tr> <td style='color:#808080;'>Alternate Delivery, To</td><td>{form["AlternateDeliveryWindow"]},{form["AlternateTo"]}</td><td colspan=3></td></tr><tr> <td colspan=5> <h4 style='padding-top:7px'>OWNER 1 <i>{form["Owner1.Name"]}</i></h4> </td></tr><tr><td colspan=5>{form["Owner1.Address"]},<br/>{form["Owner1.City"]}</td></tr><tr> <td style='color:#808080;'>DL#, State, Expiration Date</td><td>{form["Owner1.DL"]}</td><td colspan=3></td></tr><tr> <td colspan=5> <h4 style='padding-top:7px'>OWNER 2 <i>{form["Owner2.Name"]}</i></h4> </td></tr><tr><td colspan=5>{form["Owner1.Address"]},<br/>{form["Owner2.City"]}</td></tr><tr> <td style='color:#808080;'>DL#, State, Expiration Date</td><td>{form["Owner2.DL"]}</td><td colspan=3></td></tr><tr> <td colspan=5> <h4 style='padding-top:7px'>OWNER 3 <i>{form["Owner3.Name"]}</i></h4> </td></tr><tr><td colspan=5>{form["Owner1.Address"]},<br/>{form["Owner3.City"]}</td></tr><tr> <td style='color:#808080;'>DL#, State, Expiration Date</td><td>{form["Owner3.DL"]}</td><td colspan=3></td></tr><tr style='padding-top:10px;'> <td colspan='5'> <h4 style='color:red;padding-top:7px;'>ACCOUNTING DEPARTMENT ONLY: (Not to be filled in by Customer)</h4> </td></tr><tr> <td style='color:#808080;'>Customer No.</td><td>{form["Customer"]}</td><td colspan=3></td></tr><tr> <td style='color:#808080;'>Sales Person</td><td>{form["SalesPerson"]}</td><td colspan=3></td></tr><tr><td style='color:#808080;'>Price Level</td><td>{form["PriceLevel"]}</td><td colspan=3></td></tr><tr> <td style='color:#808080;'>Source</td><td>{form["Source"]}</td><td colspan=3></td></tr><tr> <td style='color:#808080;'>CATEGORY</td><td> {(form["WillCall"] != null ? "Will Call, " : "")}{(form["National"] != null ? "National, " : "")}{(form["FoodService"] != null ? "Food Service, " : "")}{(form["HealthCare"] != null ? "Health Care, " : "")}{(form["Schools"] != null ? "Schools, " : "")}{(form["Institute"] != null ? "Institute, " : "")}{(form["Resorts"] != null ? "Resorts, " : "")}{(form["Buyer"] != null ? "Buyer, " : "")}{(form["WholeSaler"] != null ? "WholeSaler, " : "")}{(form["Carniceria"] != null ? "Carniceria, " : "")}{(form["Retail1"] != null ? "Retail I, " : "")}{(form["Retail2"] != null ? "Retail II, " : "")}{(form["Retail3"] != null ? "Retail III" : "")}</td><td colspan=3></td></tr><tr> <td style='color:#808080;'>CAT TYPE</td><td> {(form["Chain"] != null ? "Chain," : "")} { (form["Independent"] != null ? "Independent, " : "")} </td><td colspan=3></td></tr><tr> <td style='color:#808080;'>LOCATION</td><td> {(form["Local"] != null ? "Local, " : "")}{(form["OOT"] != null ? "Out of Town" : "")}</td><td colspan=3></td></tr><tr> <td style='color:#808080;'>Route#</td><td>{form["Route"]}</td><td colspan=3></td></tr><tr><td style='color:#808080;'>Stop#</td><td>{form["Stop"]}</td><td colspan=3></td></tr></tbody> </table></body></html>";
                //$"<html><head> <title>Nogales Produce - Customer Form</title><link href='https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css' rel='stylesheet' type='text/css'/></head><body> <table style='border:1px solid gray;margin-top:10px;width:600px;' align='center' cellpadding=0 cellspacing=0> <tbody><tr style='border-bottom:1px dotted gray;'><td align='left'><img src='http://nogalesproduce.com/images/logo-header1.png' alt='NogalesProduce'/></td><td align='center'><h4>New Customer Form</h4></td></tr><tr> <td style='color:#808080;'>Business Name</td><td>{form["BusinessName"]}</td><td colspan=3></td></tr><tr><td style='color:#808080;'>Email</td><td>{form["EmailAddress"]}</td><td colspan=3></td></tr><tr> <td style='color:#808080;'>Type of Business</td><td>{form["BusinessType"]}</td><td colspan=3></td></tr><tr> <td style='color:#808080;'>Resale Certificate</td><td>{form["ResaleCertificate"]}</td><td colspan=3></td></tr><tr><td style='color:#808080;'>Delivery Time</td><td>{form["DeliveryTime"]}</td><td colspan=3></td></tr><tr> <td colspan=5> <label style='padding-top:7px'>Billing Address</label> </td></tr><tr><td colspan=5>{form["BillToAddress.AddressLine1"]}<br/>{form["BillToAddress.AddressLine2"]},<br/>{form["BillToAddress.City"]},{form["BillToAddress.State"]},{form["BillToAddress.PostalCode"]}<br/>{form["BillToAddress.Country"]}</td></tr><tr> <td colspan=5> <label style='padding-top:7px'>Shipping Address</label> </td></tr><tr><td colspan=5>{form["ShipToAddress.AddressLine1"]}<br/>{form["ShipToAddress.AddressLine2"]},<br/>{form["ShipToAddress.City"]},{form["ShipToAddress.State"]},{form["ShipToAddress.PostalCode"]}<br/>{form["ShipToAddress.Country"]}</td></tr><tr> <td colspan=5> <label style='padding-top:7px'>Contact at location</label> </td></tr><tr> <td style='color:#808080;'>Name</td><td>{form["ContactAtLocation.FirstName"]}{form["ContactAtLocation.LastName"]}</td><td colspan=3></td></tr><tr> <td style='color:#808080;'>Phone</td><td>{form["ContactAtLocation.PhoneAreaCode"]}-{form["ContactAtLocation.Phone"]}</td><td colspan=3></td></tr><tr><td style='color:#808080;'>Fax</td><td>{form["ContactAtLocation.FaxAreaCode"]}-{form["ContactAtLocation.Fax"]}</td><td colspan=3></td></tr><tr> <td colspan=5> <label style='padding-top:7px'>Acct. Payable contact</label> </td></tr><tr> <td style='color:#808080;'>Name</td><td>{form["PayableContact.FirstName"]}{form["PayableContact.LastName"]}</td><td colspan=3></td></tr><tr> <td style='color:#808080;'>Phone</td><td>{form["PayableContact.PhoneAreaCode"]}-{form["PayableContact.Phone"]}</td><td colspan=3></td></tr><tr><td style='color:#808080;'>Fax</td><td>{form["PayableContact.FaxAreaCode"]}-{form["PayableContact.Fax"]}</td><td colspan=3></td></tr><tr> <td colspan=5> <label style='padding-top:7px'>Owner 1 <i>{form["Owner1.FirstName"]}{form["Owner1.LastName"]}<i></label> </td></tr><tr><td colspan=5>{form["Owner1.AddressLine1"]}<br/>{form["Owner1.AddressLine2"]},<br/>{form["Owner1.City"]},{form["Owner1.State"]},{form["Owner1.PostalCode"]}<br/>{form["Owner1.Country"]}</td></tr><tr> <td style='color:#808080;'>Phone</td><td>{form["Owner1.PhoneAreaCode"]}-{form["Owner1.Phone"]}</td><td colspan=3></td></tr><tr> <td style='color:#808080;'>Fax</td><td>{form["Owner1.FaxAreaCode"]}-{form["Owner1.Fax"]}</td><td colspan=3></td></tr><tr><td style='color:#808080;'>DOB</td><td>{form["Owner1.DOB"]}</td><td colspan=3></td></tr><tr> <td style='color:#808080;'>DL No.</td><td>{form["Owner1.DrivingLicense"]}</td><td colspan=3></td></tr><tr><td style='color:#808080;'>DL State</td><td>{form["Owner1.DrivingLicenseState"]}</td><td colspan=3></td></tr><tr> <td colspan=5> <label style='padding-top:7px'>Owner 2 <i>{form["Owner2.FirstName"]}{form["Owner2.LastName"]}<i></label> </td></tr><tr><td colspan=5>{form["Owner2.AddressLine1"]}<br/>{form["Owner2.AddressLine2"]},<br/>{form["Owner2.City"]},{form["Owner2.State"]},{form["Owner2.PostalCode"]}<br/>{form["Owner2.Country"]}</td></tr><tr> <td style='color:#808080;'>Phone</td><td>{form["Owner2.PhoneAreaCode"]}-{form["Owner2.Phone"]}</td><td colspan=3></td></tr><tr> <td style='color:#808080;'>Fax</td><td>{form["Owner2.FaxAreaCode"]}-{form["Owner2.Fax"]}</td><td colspan=3></td></tr><tr><td style='color:#808080;'>DOB</td><td>{form["Owner2.DOB"]}</td><td colspan=3></td></tr><tr> <td style='color:#808080;'>DL No.</td><td>{form["Owner2.DrivingLicense"]}</td><td colspan=3></td></tr><tr><td style='color:#808080;'>DL State</td><td>{form["Owner2.DrivingLicenseState"]}</td><td colspan=3></td></tr><tr> <td colspan=5> <label style='padding-top:7px'>Owner 3 <i>{form["Owner3.FirstName"]}{form["Owner3.LastName"]}<i></label> </td></tr><tr><td colspan=5>{form["Owner3.AddressLine1"]}<br/>{form["Owner3.AddressLine2"]},<br/>{form["Owner3.City"]},{form["Owner3.State"]},{form["Owner3.PostalCode"]}<br/>{form["Owner3.Country"]}</td></tr><tr> <td style='color:#808080;'>Phone</td><td>{form["Owner3.PhoneAreaCode"]}-{form["Owner3.Phone"]}</td><td colspan=3></td></tr><tr> <td style='color:#808080;'>Fax</td><td>{form["Owner3.FaxAreaCode"]}-{form["Owner3.Fax"]}</td><td colspan=3></td></tr><tr><td style='color:#808080;'>DOB</td><td>{form["Owner3.DOB"]}</td><td colspan=3></td></tr><tr> <td style='color:#808080;'>DL No.</td><td>{form["Owner3.DrivingLicense"]}</td><td colspan=3></td></tr><tr><td style='color:#808080;'>DL State</td><td>{form["Owner3.DrivingLicenseState"]}</td><td colspan=3></td></tr><tr style='padding-top:10px;'> <td colspan='5'> <label style='color:red;padding-top:7px;'>Credit Department Only: (Not to be filled in by Customer)</label> </td></tr><tr> <td style='color:#808080;'>Customer No.</td><td>{form["CustomerNumber"]}</td><td colspan=3></td></tr><tr><td style='color:#808080;'>Order Type</td><td>{form["OrderType"]}</td><td colspan=3></td></tr><tr> <td style='color:#808080;'>Sales Person</td><td>{form["SalesPerson"]}</td><td colspan=3></td></tr><tr><td style='color:#808080;'>Route No.</td><td>{form["RouteNumber"]}</td><td colspan=3></td></tr><tr> <td style='color:#808080;'>Terms</td><td>{form["Terms"]}</td><td colspan=3></td></tr><tr> <td style='color:#808080;'>Territory</td><td>{form["Territory"]}</td><td colspan=3></td></tr><tr> <td style='color:#808080;'>Price Level</td><td>{form["PriceLevel"]}</td><td colspan=3></td></tr><tr><td style='color:#808080;'>Stop</td><td>{form["Stop"]}</td><td colspan=3></td></tr></tbody> </table></body></html>";

                MailMessage mailMsg = new MailMessage();
                mailMsg.To.Add(new MailAddress(ConfigurationManager.AppSettings["NogalesProduceEmail"]));
                mailMsg.CC.Add(new MailAddress(ConfigurationManager.AppSettings["DigiclarityEmail"]));
                var applicationUrl = ConfigurationManager.AppSettings["ApplicationUrl"];
                // From
                mailMsg.From = new MailAddress(ConfigurationManager.AppSettings["DefaultEmailFrom"]);
                mailMsg.IsBodyHtml = true;
                mailMsg.Body = content;
                mailMsg.Subject = "Nogales Produce: New Customer Form";
                mailMsg.IsBodyHtml = true;
                mailMsg.BodyEncoding = Encoding.UTF8;
                var fileBytes = System.IO.File.ReadAllBytes(physicalFilePath);
                MemoryStream stream = new MemoryStream(fileBytes);
                Attachment attachment = new Attachment(stream, Path.GetFileName(physicalFilePath));
                mailMsg.Attachments.Add(attachment);
                SmtpClient smtpClient = sendGridCredentials();
                smtpClient.Send(mailMsg);
                stream.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        internal bool SendCreditApplicationForm(NameValueCollection form)
        {
            try
            {
                string content = $"<html><head> <title>Nogales Produce - Credit Application Form</title><link href='https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css' rel='stylesheet' type='text/css'/></head><body> <table style='border:1px solid gray;margin-top:10px;width:600px;' align='center' cellpadding=0 cellspacing=0> <tbody> <tr style='border-bottom:1px dotted gray;'> <td align='left'><img src='http://nogalesproduce.com/images/logo-header1.png' alt='NogalesProduce'/></td><td align='center'> <h3>Credit Application Form</h3></td></tr><tr> <td style='color:#808080;'>Business Name</td><td>{form["BusinessName"]}</td><td style='width:150px;'></td><td style='color:#808080;'>Date</td><td>{form["BusinessDate"]}</td></tr><tr> <td style='color:#808080;'>Anticipated Monthly Purchases</td><td>{form["AnticipatedMonthlyPurchases"]}</td><td style='width:150px;'></td><td style='color:#808080;'>Business Type</td><td>{form["BusinessType"]}</td></tr><tr> <td style='color:#808080;'>Texas Resale Number</td><td>{form["ResaleCertificate"]}</td><td style='width:150px;'></td><td style='color:#808080;'>Other Location</td><td>{form["OtherLocation"]}</td></tr><tr> <td style='color:#808080;'>Years in Buisness</td><td>{form["YearsInBusiness"]}</td><td style='width:150px;'></td><td style='color:#808080;'>Subsidiary or Affiliate of</td><td>{form["Subsidary"]}</td></tr><tr> <td style='color:#808080;'>Address line1</td><td>{form["Business.AddressLine1"]}</td><td style='width:150px;'></td><td style='color:#808080;'>Postal Code</td><td>{form["Business.PostalCode"]}</td></tr><tr> <td style='color:#808080;'>Address line2</td><td>{form["Business.AddressLine2"]}</td><td style='width:150px;'></td><td style='color:#808080;'>State</td><td>{form["Business.State"]}</td></tr><tr> <td style='color:#808080;'>City</td><td>{form["Business.City"]}</td><td style='width:150px;'></td><td style='color:#808080;'>Country</td><td>{form["Business.Country"]}</td></tr><tr> <td style='color:#808080;'>Resale Certiicate</td><td>{form["ResaleCertificate"]}</td><td style='width:150px;'></td><td style='color:#808080;'>Delivery Time</td><td>{form["DeliveryTime"]}</td></tr><tr> <td colspan=5> <h4> List Owners / Principals / Partners / Officers / Associates: </h4> </td></tr><tr> <td style='color:#808080;'>1. Full Name</td><td>{form["Associate1.FirstName"]}{form["Associate1.LastName"]}</td><td style='width:150px;'></td><td style='color:#808080;'>Title</td><td>{form["Associate1.Title"]}</td></tr><tr> <td style='color:#808080;'>Phone</td><td>{form["Associate1.PhoneAreaCode"]}-{form["Associate1.Phone"]}</td><td style='width:150px;'></td><td style='color:#808080;'>Social Security Number</td><td>{form["Associate1.SSN"]}</td></tr><tr> <td style='color:#808080;'>Address line1</td><td>{form["Associate1.AddressLine1"]}</td><td style='width:150px;'></td><td style='color:#808080;'>Postal Code</td><td>{form["Associate1.PostalCode"]}</td></tr><tr> <td style='color:#808080;'>Address line2</td><td>{form["Associate1.AddressLine2"]}</td><td style='width:150px;'></td><td style='color:#808080;'>State</td><td>{form["Associate1.State"]}</td></tr><tr> <td style='color:#808080;'>City</td><td>{form["Associate1.City"]}</td><td style='width:150px;'></td><td style='color:#808080;'>Country</td><td>{form["Associate1.Country"]}</td></tr><tr> <td colspan='5'></td></tr><tr> <td style='color:#808080;'>2. Full Name</td><td>{form["Associate2.FirstName"]}{form["Associate2.LastName"]}</td><td style='width:150px;'></td><td style='color:#808080;'>Title</td><td>{form["Associate2.Title"]}</td></tr><tr> <td style='color:#808080;'>Phone</td><td>{form["Associate2.PhoneAreaCode"]}-{form["Associate2.Phone"]}</td><td style='width:150px;'></td><td style='color:#808080;'>Social Security Number</td><td>{form["Associate2.SSN"]}</td></tr><tr> <td style='color:#808080;'>Address line1</td><td>{form["Associate2.AddressLine1"]}</td><td style='width:150px;'></td><td style='color:#808080;'>Postal Code</td><td>{form["Associate2.PostalCode"]}</td></tr><tr> <td style='color:#808080;'>Address line2</td><td>{form["Associate2.AddressLine2"]}</td><td style='width:150px;'></td><td style='color:#808080;'>State</td><td>{form["Associate2.State"]}</td></tr><tr> <td style='color:#808080;'>City</td><td>{form["Associate2.City"]}</td><td style='width:150px;'></td><td style='color:#808080;'>Country</td><td>{form["Associate2.Country"]}</td></tr><tr> <td colspan='5'><h4>Bank Information</h4></td></tr><tr> <td style='color:#808080;'>Name</td><td>{form["Bank.Name"]}</td><td style='width:150px;'></td><td style='color:#808080;'>Account Number</td><td>{form["Bank.AccountNumber"]}</td></tr><tr> <td style='color:#808080;'>Name</td><td>{form["Bank.Name"]}</td><td style='width:150px;'></td><td style='color:#808080;'>Account Number</td><td>{form["Bank.AccountNumber"]}</td></tr><tr> <td style='color:#808080;'>Account Type</td><td>{form["Bank.AccountType"]}</td><td style='width:150px;'></td><td style='color:#808080;'>Phone</td><td>{form["Bank.PhoneAreaCode"]}-{form["Bank.Phone"]}</td></tr><tr> <td style='color:#808080;'>Addresss line1</td><td>{form["Bank.AddressLine1"]}</td><td style='width:150px;'></td><td style='color:#808080;'>Address line2</td><td>{form["Bank.AddressLine2"]}</td></tr><tr> <td style='color:#808080;'>City</td><td>{form["Bank.City"]}</td><td style='width:150px;'></td><td style='color:#808080;'>State</td><td>{form["Bank.State"]}</td></tr><tr> <td style='color:#808080;'>Postal Code</td><td>{form["Bank.PostalCode"]}</td><td style='width:150px;'></td><td style='color:#808080;'>Country</td><td>{form["Bank.Country"]}</td></tr><tr> <td colspan='5'><h4>Trade References</h4></td></tr><tr> <td style='color:#808080;'>1. Name</td><td>{form["Owner1.Name"]}</td><td style='width:150px;'></td><td style='color:#808080;'></td><td></td></tr><tr> <td style='color:#808080;'>Contact</td><td>{form["Owner1.Contact"]}</td><td style='width:150px;'></td><td style='color:#808080;'>Phone</td><td>{form["Owner1.PhoneAreaCode"]}-{form["Owner1.Phone"]}</td></tr><tr> <td colspan='5'></td></tr><tr> <td style='color:#808080;'>2. Name</td><td>{form["Owner2.Name"]}</td><td style='width:150px;'></td><td style='color:#808080;'></td><td></td></tr><tr> <td style='color:#808080;'>Contact</td><td>{form["Owner2.Contact"]}</td><td style='width:150px;'></td><td style='color:#808080;'>Phone</td><td>{form["Owner2.PhoneAreaCode"]}-{form["Owner2.Phone"]}</td></tr><tr> <td colspan='5'></td></tr><tr> <td style='color:#808080;'>3. Name</td><td>{form["Owner3.Name"]}</td><td style='width:150px;'></td><td style='color:#808080;'></td><td></td></tr><tr> <td style='color:#808080;'>Contact</td><td>{form["Owner3.Contact"]}</td><td style='width:150px;'></td><td style='color:#808080;'>Phone</td><td>{form["Owner3.PhoneAreaCode"]}-{form["Owner3.Phone"]}</td></tr><tr> <td colspan='5'></td></tr><tr> <td style='color:#808080;'>4. Name</td><td>{form["Owner4.Name"]}</td><td style='width:150px;'></td><td style='color:#808080;'></td><td></td></tr><tr> <td style='color:#808080;'>Contact</td><td>{form["Owner4.Contact"]}</td><td style='width:150px;'></td><td style='color:#808080;'>Phone</td><td>{form["Owner4.PhoneAreaCode"]}-{form["Owner4.Phone"]}</td></tr><tr> <td colspan='5'></td></tr><tr> <td style='color:#808080;'>5. Name</td><td>{form["Owner5.Name"]}</td><td style='width:150px;'></td><td style='color:#808080;'></td><td></td></tr><tr> <td style='color:#808080;'>Contact</td><td>{form["Owner5.Contact"]}</td><td style='width:150px;'></td><td style='color:#808080;'>Phone</td><td>{form["Owner5.PhoneAreaCode"]}-{form["Owner5.Phone"]}</td></tr><tr> <td> <h4>Guaranty Agreement ({form["Guarantor.Date"]})</h4> </td></tr><tr> <td style='color:#808080;'>1. First Name</td><td>{form["Guarantor1.FirstName"]}</td><td style='width:150px;'></td><td style='color:#808080;'>Last Name</td><td>{form["Guarantor1.LastName"]}</td></tr><tr> <td style='color:#808080;'>Address line1</td><td>{form["Guarantor1.AddressLine1"]}</td><td style='width:150px;'></td><td style='color:#808080;'>Address line2</td><td>{form["Guarantor1.AddressLine1"]}</td></tr><tr> <td style='color:#808080;'>Postal Code</td><td>{form["Guarantor1.PostalCode"]}</td><td style='width:150px;'></td><td style='color:#808080;'>City</td><td>{form["Guarantor1.City"]}</td></tr><tr> <td style='color:#808080;'>State</td><td>{form["Guarantor1.State"]}</td><td style='width:150px;'></td><td style='color:#808080;'>Country</td><td>{form["Guarantor1.Country"]}</td></tr><tr> <td colspan='5'></td></tr><tr> <td style='color:#808080;'>2. First Name</td><td>{form["Guarantor2.FirstName"]}</td><td style='width:150px;'></td><td style='color:#808080;'>Last Name</td><td>{form["Guarantor2.LastName"]}</td></tr><tr> <td style='color:#808080;'>Address line1</td><td>{form["Guarantor2.AddressLine1"]}</td><td style='width:150px;'></td><td style='color:#808080;'>Address line2</td><td>{form["Guarantor2.AddressLine1"]}</td></tr><tr> <td style='color:#808080;'>Postal Code</td><td>{form["Guarantor2.PostalCode"]}</td><td style='width:150px;'></td><td style='color:#808080;'>City</td><td>{form["Guarantor2.City"]}</td></tr><tr> <td style='color:#808080;'>State</td><td>{form["Guarantor2.State"]}</td><td style='width:150px;'></td><td style='color:#808080;'>Country</td><td>{form["Guarantor2.Country"]}</td></tr><tr> <td colspan='5'> <h4>Sales Agreement</h4> </td></tr><tr> <td style='color:#808080;'>Account Name (firm)</td><td>{form["SalesAgreement.AccountName"]}</td><td style='width:150px;'></td><td style='color:#808080;'>First Name</td><td>{form["SalesAgreement.FirstName"]}</td></tr><tr> <td style='color:#808080;'>Last Name</td><td>{form["SalesAgreement.LastName"]}</td><td style='width:150px;'></td><td style='color:#808080;'>Title</td><td>{form["SalesAgreement.Title"]}</td></tr><tr> <td style='color:#808080;'>Date</td><td>{form["SalesAgreement.Date"]}</td><td style='width:150px;'></td><td style='color:#808080;'></td><td></td></tr></tbody> </table></body></html>";

                MailMessage mailMsg = new MailMessage();
                mailMsg.To.Add(new MailAddress(ConfigurationManager.AppSettings["NogalesProduceEmail"]));
                mailMsg.CC.Add(new MailAddress(ConfigurationManager.AppSettings["DigiclarityEmail"]));
                var applicationUrl = ConfigurationManager.AppSettings["ApplicationUrl"];
                // From
                mailMsg.From = new MailAddress(ConfigurationManager.AppSettings["DefaultEmailFrom"]);
                mailMsg.IsBodyHtml = true;
                mailMsg.Body = content;
                mailMsg.Subject = "Nogales Produce: Credit Application Form";
                mailMsg.IsBodyHtml = true;
                mailMsg.BodyEncoding = Encoding.UTF8;
                SmtpClient smtpClient = sendGridCredentials();
                smtpClient.Send(mailMsg);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        internal bool SendFaxAuthorizationForm(NameValueCollection form)
        {
            try
            {
                string content = $"<html><head> <title>Fax Authorization form</title></head><body> <p><span style='font-family:helvetica;font-size:small;color:#666666;'>Dear Customer,</span></p><p><span style='font-family:helvetica;font-size:small;color:#666666;'>According to the new rules from the United States Federal Communications Commission, effective August 25, 2003, we are unable to legally fax advertisements (price lists, availability lists, solicitations, etc.) to anyone in the United States without signed written permission. This is part of the same rules that created the widely publicized <strong>do not call </strong>registry in the United States.</span></p><p><span style='font-family:helvetica;font-size:small;color:#666666;'>In order for Nogales Produce, Inc. to conduct business, it is necessary for us to fax price list and /or ad- vertisements to our customers. If you would like to continue receiving faxes from us, in accordance with this new law, we ask that you take a moment to complete the information below and fax it back Nogales Produce, Inc. before August 25, 2003 to (214) 275 -3575. If you prefer to receive information via email, please indicate below.</span></p><p><span style='font-family:helvetica;font-size:small;color:#666666;'>Please feel free to contacts us if you have any questions. Sincerely,</span></p><p><span style='font-family:helvetica;font-size:small;color:#666666;'>CEO/ President</span></p><p><span style='font-family:helvetica;font-size:small;color:#666666;'>I/we prefer to receive any information via:</span></p><table> <tbody> <tr> <td style='color:#808080;'>FullName</td><td>{form["FirstName"]}{form["LastName"]}</td><td colspan=3>, gives permission to Nogales Produce, Inc. to fas any pertinent information to me/us.</td></tr><tr> <td style='color:#808080;'>Phone</td><td>{form["PhoneAreaCode"]}{form["Phone"]}</td><td style='width:150px;'></td><td style='color:#808080;'>Email</td><td>{form["EmailAddress"]}</td></tr><tr> <td style='color:#808080;'>Company Name</td><td>{form["CompanyName"]}</td></tr></tbody> </table></body></html>";

                MailMessage mailMsg = new MailMessage();
                mailMsg.To.Add(new MailAddress(ConfigurationManager.AppSettings["NogalesProduceEmail"]));
                mailMsg.CC.Add(new MailAddress(ConfigurationManager.AppSettings["DigiclarityEmail"]));
                var applicationUrl = ConfigurationManager.AppSettings["ApplicationUrl"];
                // From
                mailMsg.From = new MailAddress(ConfigurationManager.AppSettings["DefaultEmailFrom"]);
                mailMsg.IsBodyHtml = true;
                mailMsg.Body = content;
                mailMsg.Subject = "Nogales Produce: Fax Authorization Form";
                mailMsg.IsBodyHtml = true;
                mailMsg.BodyEncoding = Encoding.UTF8;
                SmtpClient smtpClient = sendGridCredentials();
                smtpClient.Send(mailMsg);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        internal bool SendTexasResaleForm(NameValueCollection form)
        {
            try
            {
                string content = $"<html><head> <title>Fax Authorization form</title></head><body> <p><span style='font-family:helvetica;font-size:small;color:#666666;'>Dear Customer,</span></p><p><span style='font-family:helvetica;font-size:small;color:#666666;'>According to the new rules from the United States Federal Communications Commission, effective August 25, 2003, we are unable to legally fax advertisements (price lists, availability lists, solicitations, etc.) to anyone in the United States without signed written permission. This is part of the same rules that created the widely publicized <strong>do not call </strong>registry in the United States.</span></p><p><span style='font-family:helvetica;font-size:small;color:#666666;'>In order for Nogales Produce, Inc. to conduct business, it is necessary for us to fax price list and /or ad- vertisements to our customers. If you would like to continue receiving faxes from us, in accordance with this new law, we ask that you take a moment to complete the information below and fax it back Nogales Produce, Inc. before August 25, 2003 to (214) 275 -3575. If you prefer to receive information via email, please indicate below.</span></p><p><span style='font-family:helvetica;font-size:small;color:#666666;'>Please feel free to contacts us if you have any questions. Sincerely,</span></p><p><span style='font-family:helvetica;font-size:small;color:#666666;'>CEO/ President</span></p><p><span style='font-family:helvetica;font-size:small;color:#666666;'>I/we prefer to receive any information via:</span></p><table> <tbody> <tr> <td style='color:#808080;'>FullName</td><td>{form["FirstName"]}{form["LastName"]}</td><td colspan=3>, gives permission to Nogales Produce, Inc. to fas any pertinent information to me/us.</td></tr><tr> <td style='color:#808080;'>Phone</td><td>{form["PhoneAreaCode"]}{form["Phone"]}</td><td style='width:150px;'></td><td style='color:#808080;'>Email</td><td>{form["EmailAddress"]}</td></tr><tr> <td style='color:#808080;'>Company Name</td><td>{form["CompanyName"]}</td></tr></tbody> </table></body></html>";

                MailMessage mailMsg = new MailMessage();
                mailMsg.To.Add(new MailAddress(ConfigurationManager.AppSettings["NogalesProduceEmail"]));
                mailMsg.CC.Add(new MailAddress(ConfigurationManager.AppSettings["DigiclarityEmail"]));
                var applicationUrl = ConfigurationManager.AppSettings["ApplicationUrl"];
                // From
                mailMsg.From = new MailAddress(ConfigurationManager.AppSettings["DefaultEmailFrom"]);
                mailMsg.IsBodyHtml = true;
                mailMsg.Body = content;
                mailMsg.Subject = "Nogales Produce: Texas Resale Form";
                mailMsg.IsBodyHtml = true;
                mailMsg.BodyEncoding = Encoding.UTF8;
                SmtpClient smtpClient = sendGridCredentials();
                smtpClient.Send(mailMsg);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static string GetMailBody(string Header, string Name, string MainContent, string Title1, string Content1, string Title2, string Content2, string BottomContent)
        {
            string mailBody = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("../Content/EmailTemplate.html")))
            {
                mailBody = reader.ReadToEnd();
            }

            mailBody = mailBody.Replace("{{header}}", Header);
            mailBody = mailBody.Replace("{{name}}", Name);
            mailBody = mailBody.Replace("{{maincontent}}", MainContent);
            mailBody = mailBody.Replace("{{title1}}", Title1);
            mailBody = mailBody.Replace("{{content1}}", Content1);
            mailBody = mailBody.Replace("{{title2}}", Title2);
            mailBody = mailBody.Replace("{{content2}}", Content2);
            mailBody = mailBody.Replace("{{bottomcontent}}", BottomContent);
            mailBody = mailBody.Replace("{{preHeader}}", "");

            return mailBody;
        }
    }
}