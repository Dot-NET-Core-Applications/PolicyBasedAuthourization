using System;
using Microsoft.AspNetCore.Authentication;

namespace PolicyBasedAuthourization.Controllers
{
    public interface ICustomAuthenticationHandler
    {
        /// <summary>
        /// Validate Custom token string.
        /// </summary>
        /// <param name="token">Token created.</param>
        /// <returns><see cref="AuthenticateResult"/></returns>
        AuthenticateResult ValidateToken(string token);
    }
}
