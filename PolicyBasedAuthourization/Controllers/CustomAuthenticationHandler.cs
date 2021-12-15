using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Security.Principal;


namespace PolicyBasedAuthourization.Controllers
{
    public class CustomAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>, ICustomAuthenticationHandler
    {
        /// <summary>
        /// <see cref="CustomAuthenticationManager"/>
        /// </summary>
        private ICustomAuthenticationManager customAuthenticationManager;

        /// <summary>
        /// Instance of Authentication Handler.
        /// </summary>
        /// <param name="options"><see cref="OptionsMonitor{BasicAuthenticationOptions}"/></param>
        /// <param name="logger"><see cref="LoggerFactory"/></param>
        /// <param name="encoder"><see cref="UrlEncoder"/></param>
        /// <param name="clock"><see cref="SystemClock"/></param>
        /// <param name="customAuthenticationManage><see cref="CustomAuthenticationMan"/></param>
        public CustomAuthenticationHandler(IOptionsMonitor<BasicAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ICustomAuthenticationManager customAuthenticationManager)
            : base(options, logger, encoder, clock)
        {
            this.customAuthenticationManager = customAuthenticationManager;
        }

        public AuthenticateResult ValidateToken(string token)
        {
            KeyValuePair<string, UserCredential> validatedToken = customAuthenticationManager.Tokens
                .FirstOrDefault<KeyValuePair<string, UserCredential>>(t => t.Key.Equals(token));
            if (validatedToken.Key.Equals(token))
            {
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, validatedToken.Value.UserName)
                };

                foreach (string role in validatedToken.Value.Roles)
                {
                    Claim claim = new Claim(ClaimTypes.Role, role);
                    claims.Add(claim);
                }
                ClaimsIdentity identity = new ClaimsIdentity(claims, Scheme.Name);
                GenericPrincipal principal = new GenericPrincipal(identity, validatedToken.Value.Roles.ToArray<string>());
                AuthenticationTicket ticket = new AuthenticationTicket(principal, Scheme.Name);
                return AuthenticateResult.Success(ticket);
            }
            else
                return AuthenticateResult.Fail("Invalid token!");
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Request.Headers.ContainsKey("Authorization"))
            {
                string authorization = Request.Headers["Authorization"];
                string token = string.Empty;

                if (string.IsNullOrEmpty(authorization))
                    return AuthenticateResult.Fail("Unauthorize");
                if (!authorization.StartsWith("bearer", StringComparison.OrdinalIgnoreCase))
                    return AuthenticateResult.Fail("Unauthorize");
                else
                    token = authorization.Substring("bearer".Length).Trim();
                if (string.IsNullOrEmpty(token))
                    return AuthenticateResult.Fail("Unauthorize");
                else
                    try
                    {
                        return ValidateToken(token);
                    }
                    catch (Exception)
                    {
                        // Log.
                        return AuthenticateResult.Fail("Unauthorize");
                    }
            }
            return AuthenticateResult.Fail("Unauthorized");
        }
    }
}
