using Nogales.BusinessModel;
using Nogales.DataProvider;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace Nogales.API.Controllers
{
    [Authorize]
    [RoutePrefix("AdminUserManagement")]
    public class AdminUserManagementController : ApiController
    {

        AdminManagementDataProvider _adminManagementDataProvider;

        #region Get Modules & Categories
        [Route("GetAllSalesPersonCategories")]
        [HttpGet]
        public IHttpActionResult GetAllSalesPersonCategories()
        {
            _adminManagementDataProvider = new AdminManagementDataProvider();
            var result = _adminManagementDataProvider.GetAllCategories();
            if (result.Count > 0)
            {
                return Ok(result);
            }
            else
            {
                return Ok("no data found");
            }
        }

        [Route("GetAllModules")]
        [HttpGet]
        public IHttpActionResult GetAllModules()
        {
            _adminManagementDataProvider = new AdminManagementDataProvider();
            var result = _adminManagementDataProvider.GetAllModules();
            if (result.Count > 0)
            {
                return Ok(result);
            }
            else
            {
                return Ok("no data found");
            }
        }

        [Route("GetAllModules")]
        [HttpGet]
        public IHttpActionResult GetAllModules(string email)
        {
            _adminManagementDataProvider = new AdminManagementDataProvider();
            var result = _adminManagementDataProvider.GetAllModules();
            if (result.Count > 0)
            {
                return Ok(result);
            }
            else
            {
                return Ok("no data found");
            }
        }
        #endregion

        #region Get User Details
        [Route("GetUserDetails")]
        [HttpGet]
        public IHttpActionResult GetUserDetails(string userId)
        { 
            _adminManagementDataProvider = new AdminManagementDataProvider();
            var result = _adminManagementDataProvider.GetUserDetails(userId);
            if (result!=null)
            {
                return Ok(result);
            }
            else
            {
                return Ok("no data found");
            }
        }
        #endregion

        #region Get User Access of Modules and Categories
        [Route("GetUserAccess")]
        [HttpGet]
        public IHttpActionResult GetUserAccess(string userId)
        {
            _adminManagementDataProvider = new AdminManagementDataProvider();
            var result = _adminManagementDataProvider.GetUserAccess(userId);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return Ok("no data found");
            }
        }

        #endregion

    }
}