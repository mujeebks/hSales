using iTextSharp.text.pdf;
using Nogales.API.Utilities;
using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using static System.Net.WebRequestMethods;

namespace Nogales.API.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
        }

        public ActionResult Customer()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Request.Form["g-recaptcha-response"]))
                {
                    return Redirect("http://www.nogalesproduce.com/customerform.html");
                }

                var emailService = new EmailService();
                var physicalFilePath = GenerateCustomerFormpdf(Request.Form);
                emailService.SendCustomerForm(Request.Form,physicalFilePath);
                System.IO.File.Delete(physicalFilePath);
                return Redirect("http://www.nogalesproduce.com/newcustomer.html");
            }
            catch (Exception ex)
            {
                return Redirect("http://www.nogalesproduce.com/ .html");
            }
        }

        public ActionResult CreditApplication()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Request.Form["g-recaptcha-response"]))
                {
                    return Redirect("http://www.nogalesproduce.com/creditapplicationform.html");
                }

                var emailService = new EmailService();
                emailService.SendCreditApplicationForm(Request.Form);
                return Redirect("http://www.nogalesproduce.com/newcustomer.html");
            }
            catch (Exception ex)
            {
                return Redirect("http://www.nogalesproduce.com/creditapplicationform.html");
            }
        }

        public ActionResult FaxAuthorization()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Request.Form["g-recaptcha-response"]))
                {
                    return Redirect("http://www.nogalesproduce.com/faxauthorizationform.html");
                }

                var emailService = new EmailService();
                emailService.SendFaxAuthorizationForm(Request.Form);
                return Redirect("http://www.nogalesproduce.com/newcustomer.html");
            }
            catch (Exception ex)
            {
                return Redirect("http://www.nogalesproduce.com/faxauthorizationform.html");
            }
        }

        public ActionResult TexasResale()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Request.Form["g-recaptcha-response"]))
                {
                    return Redirect("http://www.nogalesproduce.com/texasresaleform.html");
                }

                var emailService = new EmailService();
                emailService.SendTexasResaleForm(Request.Form);
                return Redirect("http://www.nogalesproduce.com/newcustomer.html");
            }
            catch (Exception ex)
            {
                return Redirect("http://www.nogalesproduce.com/texasresaleform.html");
            }
        }

        public async Task<JsonResult> ValidateToken(string token)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage message = await client.GetAsync("https://www.google.com/recaptcha/api/siteverify?secret=6LdVMxwUAAAAAEihvkqJJh00FnQQkHcV8lIMOeEq&response=" + token);
            message.EnsureSuccessStatusCode();
            string responseBody = await message.Content.ReadAsStringAsync();
            return Json(responseBody, JsonRequestBehavior.AllowGet);
        }


        public string GenerateCustomerFormpdf(NameValueCollection form)
        {
            string fieldvalue = "";
            PdfReader rdr = new PdfReader("http://www.nogalesproduce.com/forms/Fill%20New%20Cust2.pdf");
            string fileName = Guid.NewGuid() + ".pdf";
            string physicalFilePath = System.IO.Path.Combine(Server.MapPath("/"), fileName);
            PdfStamper stamper = new PdfStamper(rdr, new System.IO.FileStream(physicalFilePath, System.IO.FileMode.Create));

            foreach (var item in form.Keys)
            {
                foreach (var pdfitem in rdr.AcroFields.Fields)
                {
                    if (item.ToString() == pdfitem.Key.ToString())
                    {
                        fieldvalue = form[item.ToString()];
                        if (item.ToString() == "BusinessType")
                        {
                            if (form["BusinessType"].ToString() == "Corporation")
                            {
                                fieldvalue = "1";
                            }
                            else if (form["BusinessType"].ToString() == "Partnership")
                            {
                                fieldvalue = "2";
                            }
                            else if (form["BusinessType"].ToString() == "Sole Proprietor")
                            {
                                fieldvalue = "3";
                            }
                        }
                        else if (item.ToString() == "BillToAddress.AddressLine1")
                        {
                            fieldvalue = form["BillToAddress.AddressLine1"] + " " + form["BillToAddress.AddressLine2"];
                        }
                        else if (item.ToString() == "ShipToAddress.AddressLine1")
                        {
                            fieldvalue = form["ShipToAddress.AddressLine1"] + " " + form["ShipToAddress.AddressLine2"];
                        }
                        else if (item.ToString() == "ContactAtLocation.FirstName")
                        {
                            fieldvalue = form["ContactAtLocation.FirstName"].ToString() + " " + form["ContactAtLocation.LastName"].ToString();
                        }
                        else if (item.ToString() == "ContactAtLocation.Phone")
                        {
                            fieldvalue = form["ContactAtLocation.PhoneAreaCode"] + " " + form["ContactAtLocation.Phone"];
                        }
                        else if (item.ToString() == "ContactAtLocation.Fax")
                        {
                            fieldvalue = form["ContactAtLocation.FaxAreaCode"] + " " + form["ContactAtLocation.Fax"];
                        }
                        else if (item.ToString() == "PayableContact.FirstName")
                        {
                            fieldvalue = form["PayableContact.FirstName"] + " " + form["PayableContact.LastName"];
                        }
                        else if (item.ToString() == "PayableContact.FirstName")
                        {
                            fieldvalue = form["PayableContact.FirstName"] + " " + form["PayableContact.LastName"];
                        }
                        else if (item.ToString() == "PayableContact.FirstName")
                        {
                            fieldvalue = form["PayableContact.FirstName"] + " " + form["PayableContact.LastName"];
                        }
                        else if (item.ToString() == "PayableContact.Phone")
                        {
                            fieldvalue = form["PayableContact.PhoneAreaCode"] + " " + form["PayableContact.Phone"];
                        }
                        else if (item.ToString() == "PayableContact.Fax")
                        {
                            fieldvalue = form["PayableContact.FaxAreaCode"] + " " + form["PayableContact.Fax"];
                        }
                        else if (item.ToString() == "Owner1.FirstName")
                        {
                            fieldvalue = form["Owner1.FirstName"] + " " + form["Owner1.LastName"];
                        }
                        else if (item.ToString() == "Owner1.AddressLine1")
                        {
                            fieldvalue = form["Owner1.AddressLine1"] + " " + form["Owner1.AddressLine2"];
                        }
                        else if (item.ToString() == "Owner2.FirstName")
                        {
                            fieldvalue = form["Owner2.FirstName"] + " " + form["Owner2.LastName"];
                        }
                        else if (item.ToString() == "Owner2.AddressLine1")
                        {
                            fieldvalue = form["Owner2.AddressLine1"] + " " + form["Owner2.AddressLine2"];
                        }
                        else if (item.ToString() == "Owner3.FirstName")
                        {
                            fieldvalue = form["Owner3.FirstName"] + " " + form["Owner3.LastName"];
                        }
                        else if (item.ToString() == "Owner3.AddressLine1")
                        {
                            fieldvalue = form["Owner3.AddressLine1"] + " " + form["Owner3.AddressLine2"];
                        }
                        else if (item.ToString() == "Owner1.Phone")
                        {
                            if (form[item.ToString()].ToString() != "")
                            {
                                fieldvalue = "P:" + form["Owner1.PhoneAreaCode"] + " " + form["Owner1.Phone"] + " F:" + form["Owner1.FaxAreaCode"] + " " + form["Owner1.Fax"];
                            }
                        }
                        else if (item.ToString() == "Owner2.Phone")
                        {
                            if (form[item.ToString()].ToString() != "")
                            {
                                fieldvalue = "P:" + form["Owner2.PhoneAreaCode"] + " " + form["Owner2.Phone"] + " F:" + form["Owner2.FaxAreaCode"] + " " + form["Owner2.Fax"];
                            }
                        }
                        else if (item.ToString() == "Owner3.Phone")
                        {
                            if (form[item.ToString()].ToString() != "")
                            {
                                fieldvalue = "P:" + form["Owner3.PhoneAreaCode"] + " " + form["Owner3.Phone"] + " F:" + form["Owner3.FaxAreaCode"] + " " + form["Owner3.Fax"];
                            }
                        }
                        stamper.AcroFields.SetField(pdfitem.Key.ToString(), fieldvalue);
                        break;
                    }
                }
            }
            stamper.Close();
            rdr.Close();
            rdr.Dispose();
            stamper.Dispose();
            return physicalFilePath;
        }
       
    }
}