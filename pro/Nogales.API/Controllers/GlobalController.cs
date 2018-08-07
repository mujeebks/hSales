using Nogales.BusinessModel;
using Nogales.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Nogales.API.Controllers
{

    [RoutePrefix("Global")]
    public class GlobalController : ApiController
    {
        #region Get Site Filters

        [Route("GetFilters")]
        [HttpGet]
        public List<GlobalFilter> GetFilters()
        {

            return GlobaldataProvider.GetFilterWithPeriods();
        }
        #endregion
    }
}
