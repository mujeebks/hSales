using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace Nogales.API.Utilities
{
    #region Authorize Module Access
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeModuleAccess : AuthorizationFilterAttribute
    {
        
        public string Modules { get; set; }
        public override void OnAuthorization(HttpActionContext filterContext)
        {
            string authorizedToken = string.Empty;
            string userAgent = string.Empty;        
            try
            {
                var identity = HttpContext.Current.User.Identity;
                var roleClaims = ((ClaimsIdentity)HttpContext.Current.User.Identity).Claims
                .Where(c => c.Type == ClaimTypes.Name)
                .Select(c => c.Value).ToList()[1];

                var userModuleAccess = roleClaims.Split(',').ToList();
                var controllerModuleAccess = Modules.Split(',').ToList();

          
                var isInRole = userModuleAccess.Intersect(controllerModuleAccess);
                if (!isInRole.Any())
                {
                    filterContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception)
            {
                filterContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                return;
            }

            base.OnAuthorization(filterContext);
        }
    }
    #endregion
}