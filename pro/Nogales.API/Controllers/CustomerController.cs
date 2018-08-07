using Nogales.API.Utilities;
using Nogales.DataProvider;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Nogales.API.ControllersGetCustomerReport
{
    [Authorize]
    [RoutePrefix("Customer")]
    public class CustomerController : ApiController
    {
        [HttpGet]
        [Route("GetCustomerReport")]
        [AuthorizeModuleAccess(Modules = "Customers")]
        public IHttpActionResult GenerateReport(string startDate)
        {
            try
            {
                DateTime tempdate = new DateTime();
                if (!(DateTime.TryParseExact(startDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out tempdate)))
                {
                    throw new FormatException("Invalid dates");
                }

                var provider = new CustomerProvider();
                var model = provider.GetCustomerReport(startDate);
                return Ok(model);
            }
            catch (FormatException e)
            {
                return InternalServerError(e);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }
    }
}
