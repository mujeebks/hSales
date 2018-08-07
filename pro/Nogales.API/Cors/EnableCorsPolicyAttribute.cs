using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http.Cors;

namespace Nogales.API.Cors
{
    /// Custom CORS Policy Provider to handle request from different origin
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class EnableCorsPolicyAttribute : Attribute, ICorsPolicyProvider
    {
        private CorsPolicy _policy;

        public EnableCorsPolicyAttribute()
        {
            // Create a CORS policy.
            _policy = new CorsPolicy
            {
                AllowAnyMethod = true,
                AllowAnyHeader = true,
                SupportsCredentials = false,
                //AllowAnyOrigin = false,
            };

            //// Add allowed origins.

            _policy.Origins.Add(ConfigurationManager.AppSettings["ClientDomain"]);
            //_policy.Origins.Add("http://localhost:53669");
        }


        public Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            return Task.FromResult(_policy);
        }
    }

}