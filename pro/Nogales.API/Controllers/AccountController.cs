using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Nogales.API.Models;
using Nogales.API.Providers;
using Nogales.API.Results;
using Nogales.API.Utilities;
using System.Web.Http.Cors;
using System.Linq;
using Nogales.DataProvider;

namespace Nogales.API.Controllers
{
    [EnableCors("*", "*", "*")]
    [Authorize(Roles = "Tool Admin")]
    [RoutePrefix("Account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        [Route("InitAspIdentity")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult InitIdentity()
        {
            Action init = () =>
            {
                var email = "abhilashpa@vofoxsolutions.com";
                var roleManager = new RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
                ApplicationUser user = UserManager.FindByEmail(email);
                if (user == null)
                {
                    var userN = new ApplicationUser() { Email = "admin@dashboardnogales.com", UserName = "admin@dashboardnogales.com", FirstName = "Admin", LastName = "Nogales", ForceReset = false };
                    UserManager.Create(userN, "adminNogales123*");
                    user = UserManager.FindByEmail(email);
                }


                if (!roleManager.RoleExists("Tool Admin"))
                {
                    var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                    role.Name = "Tool Admin";
                    roleManager.Create(role);
                    role.Name = "User";
                    roleManager.Create(role);
                    if (user != null && user.Roles.Count <= 0)
                    {
                        UserManager.AddToRole(user.Id, "Tool Admin");
                    }
                }

                if (!roleManager.RoleExists("User"))
                {
                    var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                    role.Name = "User";
                    roleManager.Create(role);
                }

                if (user != null && user.Roles.Count <= 0)
                {
                    UserManager.AddToRole(user.Id, "Tool Admin");
                }
            };
            try
            {
                init();
            }
            catch (Exception e)
            {
                //System.Data.Entity.Database.SetInitializer<Models.ApplicationDbContext>(null);
                //init();

                return InternalServerError(e);
            }
            return Ok();
        }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
        
            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (IdentityUserLogin linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

       
        // POST api/Account/SetPassword

        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        [OverrideAuthorization]
        [Authorize]
        [Route("ChangePassword")]
        [HttpPost]
        public async Task<IHttpActionResult> ChangePassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.CurrentPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/ResetPassword
        /// <summary>
        /// Reset the password for the logged in user.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [OverrideAuthorization]
        [Authorize]
        [Route("ResetPassword")]
        public async Task<IHttpActionResult> ResetPassword(SetPasswordBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var token = UserManager.GeneratePasswordResetToken(User.Identity.GetUserId());
                IdentityResult result = await UserManager.ResetPasswordAsync(User.Identity.GetUserId(), token, model.NewPassword);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }
                var user = UserManager.FindById(User.Identity.GetUserId());
                user.ForceReset = false;
                UserManager.Update(user);
                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [Route("ResetPasswordByAdmin")]
        public async Task<IHttpActionResult> ResetPasswordByAdmin(SetPasswordBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var token = UserManager.GeneratePasswordResetToken(model.UserId);
                IdentityResult result = await UserManager.ResetPasswordAsync(model.UserId, token, model.NewPassword);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }
                var user = UserManager.FindById(model.UserId);
                var emailService = new EmailService();
                var status = emailService.SendResetPasswordEmail(user, model.NewPassword);
                if (!status)
                    return Ok("Mail_Not_Sent");
                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }




        [Route("ForgotPassword")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ForgotPassword(ApplicationUser model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = UserManager.FindByEmail(model.Email);
            if (user == null)
                throw new Exception("Sorry,user does not exist");
            var randomPassword = Membership.GeneratePassword(8, 0);
            var token = UserManager.GeneratePasswordResetToken(user.Id);
            IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, token, randomPassword);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            user.ForceReset = true;
            UserManager.Update(user);

            model.FirstName = user.FirstName;
            model.LastName = user.LastName;


            var emailService = new EmailService();
            var status = emailService.SendResetPasswordEmail(model, randomPassword);
            if (!status)
                return Ok(new { User = user, Error = "Mail Not sent" });
            return Ok(new { User = user });
        }

        // POST api/Account/AddExternalLogin
        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("The external login is already associated with an account.");
            }

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                   OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims();
                ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                Authentication.SignIn(identity);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }

        // POST api/Account/Register
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, ForceReset = true };

            user.IsRestrictedCategoryAccess = !model.IsRestrictedCategoryAccess;
            user.IsRestrictedModuleAccess = !model.IsRestrictedModuleAccess;

            if (user.IsRestrictedCategoryAccess && !model.Categories.Any(x => x.IsAccess == true))
            {
                return Content(System.Net.HttpStatusCode.BadRequest, "select atleast one category");
            }

            if (user.IsRestrictedModuleAccess && !model.Modules.Any(x => x.IsAccess == true))
            {
                return Content(System.Net.HttpStatusCode.BadRequest, "select atleast one module");
            }

            var randomPassword = Membership.GeneratePassword(8, 0);
            IdentityResult result = await UserManager.CreateAsync(user, randomPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }


            var updatedUser = UserManager.FindByEmail(model.Email);

            var adminManagementDataProvider = new AdminManagementDataProvider();

          

        

            adminManagementDataProvider.AddUserCategoryAccess(model.Categories, updatedUser.Id);
            adminManagementDataProvider.AddUserModuleAccess(model.Modules, updatedUser.Id);
            adminManagementDataProvider.AddUserDisplayModuleAccess(model.DisplayModules, updatedUser.Id);

            if (updatedUser != null)
                UserManager.AddToRole(updatedUser.Id, "User");
            var emailService = new EmailService();
            var status = emailService.SendWelcomeEmail(model, randomPassword);
            if (!status)
                return Ok(new { User = updatedUser, Error = "Mail Not sent" });
            return Ok(new { User = updatedUser });
        }

        [Route("UpdateUser")]
        [HttpPost]
        public IHttpActionResult UpdateUser(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var duplicateUser = UserManager.FindByEmail(model.Email);

            if (duplicateUser != null && duplicateUser.Id == model.Id)
            {
                // Email is not changed
                duplicateUser.FirstName = model.FirstName;
                duplicateUser.LastName = model.LastName;
                var adminManagementDataProvider = new AdminManagementDataProvider();
                duplicateUser.IsRestrictedCategoryAccess = !model.IsRestrictedCategoryAccess;
                duplicateUser.IsRestrictedModuleAccess = !model.IsRestrictedModuleAccess;
                adminManagementDataProvider.AddUserCategoryAccess(model.Categories, model.Id);
                adminManagementDataProvider.AddUserModuleAccess(model.Modules, model.Id);
                adminManagementDataProvider.AddUserDisplayModuleAccess(model.DisplayModules, model.Id);
                UserManager.Update(duplicateUser);
            }
            else
            {
                // Email id is changed
                if (duplicateUser != null)
                    return BadRequest("This email is already exist");
                var user = UserManager.FindById(model.Id);
                user.UserName = model.Email;
                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.IsRestrictedCategoryAccess = !model.IsRestrictedCategoryAccess;
                user.IsRestrictedModuleAccess = !model.IsRestrictedModuleAccess;

                var adminManagementDataProvider = new AdminManagementDataProvider();
                adminManagementDataProvider.AddUserCategoryAccess(model.Categories, model.Id);
                adminManagementDataProvider.AddUserModuleAccess(model.Modules, model.Id);
                adminManagementDataProvider.AddUserDisplayModuleAccess(model.DisplayModules, model.Id);

                UserManager.Update(user);
            }

            return Ok();
        }

        [Route("GetAllUsers")]
        [HttpGet]
        public IHttpActionResult GetAllUsers()
        {
            var a = UserManager.Users.ToList();
            return Ok(UserManager.Users.Where(p=>p.Email!= "admin@dashboardnogales.com").OrderBy(x=>x.FirstName).ToList());
        }

        [Route("DeleteUser/{userId}")]
        [HttpPost]
        public IHttpActionResult DeleteUser(string userId)
        {
            try
            {
                //string userId = "";
                var user = UserManager.FindById(userId);
                if (user != null && user.Id != User.Identity.GetUserId())
                {
                    UserManager.Delete(user);
                }
                else
                    throw new Exception("Not a valid user");
                return Ok();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return InternalServerError();
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            result = await UserManager.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
