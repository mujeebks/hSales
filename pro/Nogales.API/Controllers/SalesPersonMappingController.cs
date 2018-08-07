using Nogales.API.Utilities;
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
    [RoutePrefix("SalesPersonMapping")]
    [AuthorizeModuleAccess(Modules = "Sales,Cases Sold")]
    public class SalesPersonMappingController : ApiController
    {
        /// <summary>
        /// Get Mapped Sales Person List
        /// </summary>
        /// <returns></returns>
        [Route("GetMappedSalesPersons")]
        [HttpGet]
        public List<SalesPersonsBO> GetMappedSalesPersonList()
        {
            var salesPersonMappingProvider = new SalesPersonMappingDataProvider();
            var result = salesPersonMappingProvider.GetMappedSalesPersonList();
            return result;

        }

        /// <summary>
        /// Get Mapped Sales Person List
        /// </summary>
        /// <returns></returns>
        [Route("GetAllSalesPersonsForFiltering")]
        [HttpGet]
        public List<SalesPersonMappingBM> GetAllSalesPersonsForFiltering()
        {
            var salesPersonMappingProvider = new SalesPersonMappingDataProvider();
            var result = salesPersonMappingProvider.GetAllSalesPersonForFiltering();
            return result;

        }

        /// <summary>
        /// Get Filters For Sales Mapping
        /// </summary>
        /// <returns></returns>
        [Route("GetFiltersForSalesMapping")]
        [HttpGet]
        public SalesPersonMappingFilterBO GetFiltersForSalesMapping()
        {
            var salesPersonMappingProvider = new SalesPersonMappingDataProvider();
            var result = salesPersonMappingProvider.GetFiltersForSalesPersonMapping();
            return result;

        }


        /// <summary>
        /// Submit Mapped Sales Persons
        /// </summary>
        /// <param name="salesPersonMappingBM"></param>
        /// <returns></returns>
        [Route("AddMappedSalesPersons")]
        [HttpPost]
        public List<SalesPersonsBO> PostMappedSalesPersons(SalesPersonMappingBM salesPersonMappingBM)
        {
            List<SalesPersonsBO> SalesPersonMappingBMList = new List<SalesPersonsBO>();
            var salesPersonMappingProvider = new SalesPersonMappingDataProvider();
            int result = Convert.ToInt16(salesPersonMappingProvider.AssignSalesPersonCode(salesPersonMappingBM));
            if (result == 1)
                SalesPersonMappingBMList = salesPersonMappingProvider.GetMappedSalesPersonList();
            return SalesPersonMappingBMList;
        }

        /// <summary>
        /// UnAssign Sales Persons
        /// </summary>
        /// <param name="salesPersonCode"></param>
        /// <param name="assignSalesPersonCode"></param>
        /// <returns></returns>
        [Route("UnAssignSalesPersons")]
        [HttpPost]
        public List<SalesPersonsBO> UnAssignSalesPersons(string salesPersonCode, string assignSalesPersonCode, string endDate)
        {
            List<SalesPersonsBO> SalesPersonMappingBMList = new List<SalesPersonsBO>();
            var salesPersonMappingProvider = new SalesPersonMappingDataProvider();
            int result = Convert.ToInt16(salesPersonMappingProvider.UnAssignSalesPersonCode(salesPersonCode, assignSalesPersonCode, endDate));
            if (result == 1)
                SalesPersonMappingBMList = salesPersonMappingProvider.GetMappedSalesPersonList();
            return SalesPersonMappingBMList;
        }

        /// <summary>
        /// Get Archived Sales Persons List
        /// </summary>
        /// <param name="salesPersonCodes"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [Route("ArchivedSalesPersons")]
        [HttpPost]
        public List<ArchivedSalesPersonMappingBM> GetArchivedSalesPersonsList(ArchivedSalesPersonRequestBM archivedSalesPersonRequestBM)
        {
            List<ArchivedSalesPersonMappingBM> archivedSalesPersonList = new List<ArchivedSalesPersonMappingBM>();
            var salesPersonMappingProvider = new SalesPersonMappingDataProvider();
            archivedSalesPersonList = salesPersonMappingProvider.GetArchivedSalesPersonList(archivedSalesPersonRequestBM);
            return archivedSalesPersonList;
        }

        /// <summary>
        /// Submit mapped Sales Persons
        /// </summary>
        /// <param name="salesPersonMappingBM"></param>
        /// <returns></returns>
        [Route("AssignSalesPersons")]
        [HttpPost]
        public IHttpActionResult AssignSalesPersons(AssignSalesPersonsBO salesPersonMappingBM)
        {
            List<SalesPersonsBO> SalesPersonMappingBMList = new List<SalesPersonsBO>();
            var salesPersonMappingProvider = new SalesPersonMappingDataProvider();
            int result = salesPersonMappingProvider.AssignSalesPerson(salesPersonMappingBM);
            if (result > 0)
            {
                SalesPersonMappingBMList = salesPersonMappingProvider.GetMappedSalesPersonList();
                return Ok(SalesPersonMappingBMList);
            }
            else
            {
                return BadRequest("Sales person code and Master sales person code shouldn't be same..");
            }

        }

        /// <summary>
        /// Update Sales Person Description
        /// </summary>
        /// <param name="salesPerson"></param>
        /// <returns></returns>
        [Route("UpdateSalesPersonDescription")]
        [HttpPost]
        public IHttpActionResult UpdateSalesPersonDescription(SalesPersonsBO salesPerson)
        {
            List<SalesPersonsBO> SalesPersonMappingBMList = new List<SalesPersonsBO>();
            var salesPersonMappingProvider = new SalesPersonMappingDataProvider();
            int result = salesPersonMappingProvider.UpdateSalesPersonDescription(salesPerson);
            if (result > 0)
            {
                SalesPersonMappingBMList = salesPersonMappingProvider.GetMappedSalesPersonList();
                return Ok(SalesPersonMappingBMList);
            }
            else
            {
                return BadRequest("Sales person code and Master sales person code shouldn't be same..");
            }

        }


    }
}