using Nogales.API.Results;
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
    [Authorize]
    //[AllowAnonymous]
    [RoutePrefix("Collector")]
    public class CollectorController : ApiController
    {
        protected CollectorManagementDataProvider _collectorManagementDataProvider;
        public CollectorController()
        {
            _collectorManagementDataProvider = new CollectorManagementDataProvider();
        }


        [Route("GetAllCollectors")]
        [HttpGet]
        public IHttpActionResult GetAllCollectors()
        {
            var result = _collectorManagementDataProvider.GetAllCollectors();
            return Ok(result);
        }

        [Route("GetAllUnAssignedCustomerPrefixes")]
        [HttpGet]
        public IHttpActionResult GetAllUnAssignedCustomerPrefixes()
        {
            var result = _collectorManagementDataProvider.GetAllUnAssignedCustomerPrefixes();
            return Ok(result);
        }

        [Route("AddCollector")]
        [HttpPost]
        public IHttpActionResult AddCollector(string collectorName)
        {
            if (string.IsNullOrWhiteSpace(collectorName))
                return BadRequest();

            var isExist = _collectorManagementDataProvider.IsExistCollector(0, collectorName);
            if (isExist)
            {
                return new ConflictActionResult("Collector Name already exists", Request);
            }
            else
            {
                var result = _collectorManagementDataProvider.InsertCollector(collectorName);
                if (result)
                    return Ok("Collector has been added successfully");
                else
                    return BadRequest();
            }
        }

        [Route("UpdateCollector")]
        [HttpPut]
        public IHttpActionResult UpdateCollector(CollectorBM collector)
        {
            if (ModelState.IsValid)
            {
                var isExist = _collectorManagementDataProvider.IsExistCollector(collector.CollectorId, collector.CollectorName);
                if (isExist)
                {
                    return new ConflictActionResult("Collector Name already exists", Request);
                }
                else
                {
                    var result = _collectorManagementDataProvider.UpdateCollector(collector);
                    if (result)
                        return Ok("Collector has been updated successfully");
                }
            }
            return BadRequest();
        }

        [Route("DeleteCollector")]
        [HttpDelete]
        public IHttpActionResult DeleteCollector(int collectorId)
        {
            if (collectorId < 1)
                return BadRequest();

            var result = _collectorManagementDataProvider.DeleteCollector(collectorId);
            return Ok(result);
        }

        [Route("AssignUnAssignCustomerPrefix")]
        [HttpPut]
        public IHttpActionResult AssignUnAssignCustomerPrefix(UnAssignedCustomerPrefix collectorAssignment)
        {
            if (ModelState.IsValid)
            {
                var result = _collectorManagementDataProvider.AssignUnAssignCustomerPrefix(collectorAssignment);
                if (result)
                    return Ok("Customer prefix has been updated successfully");
                else
                    return NotFound();
            }
            return BadRequest();
        }
    }
}
