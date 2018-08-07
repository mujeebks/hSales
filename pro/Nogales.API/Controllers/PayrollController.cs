
using Nogales.BusinessModel;
using Nogales.DataProvider;
using Nogales.DataProvider.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using static Nogales.DataProvider.Utilities.Enums;

namespace Nogales.API.Controllers
{
    [RoutePrefix("Payroll")]
    public class PayrollController : ApiController
    {
        
        PayrollDataProvider _payrollDataProvider;


        #region Constructor
        public PayrollController()
        {
          
        }
        #endregion

        #region Import
        [Route("ImportPayrollData")]
        [HttpGet]
        public IHttpActionResult ImportPayRollData(string minDate, string maxDate,string invokedBy)
        {
            _payrollDataProvider = new PayrollDataProvider();
            var status = _payrollDataProvider.ImportPayRollData(minDate, maxDate, invokedBy);

            if (status)
            {
                return Ok("success");
            }
            else
            {
                return Ok("Error occured");
            }
        }

        #endregion

        [Route("GetAllEmployees")]
        [HttpGet]
        public IHttpActionResult GetAllEmployees()
        {
            _payrollDataProvider = new PayrollDataProvider();
            var model = _payrollDataProvider.GetAllEmployees();
            return Ok(model);
        }


        [Route("GetAllEmployeePaymentDetails")]
        [HttpPost]
        public IHttpActionResult GetAllEmployeePaymentDetails(PayrollFilter filter)
        {

        
            _payrollDataProvider = new PayrollDataProvider();
            var model = _payrollDataProvider.GetEmployeePaymentDetails(filter);
            return Ok(model);
        }

    }
}

