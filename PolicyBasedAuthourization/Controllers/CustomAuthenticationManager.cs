using System;
using System.Linq;
using System.Collections.Generic;

namespace PolicyBasedAuthourization.Controllers
{
    public class CustomAuthenticationManager : ICustomAuthenticationManager
    {
        // <summary>
        /// List of user credentials.
        /// </summary>
        private readonly IList<UserCredential> users;

        /// <summary>
        /// Dictionary mapping of tokens with usernames.
        /// </summary>
        private readonly IDictionary<string, UserCredential> tokens;

        public IDictionary<string, UserCredential> Tokens => tokens;

        public CustomAuthenticationManager(IList<UserCredential> users, IDictionary<string, UserCredential> tokens)
        {
            this.users = users;
            this.tokens = tokens;
        }

        public string Authenticate(string username, string password)
        {
            if (users.Any<UserCredential>(user => user.UserName.Equals(username) && user.Password.Equals(password)))
            {
                string token = Guid.NewGuid().ToString();
                UserCredential user = users.FirstOrDefault<UserCredential>(user =>
                    user.UserName.Equals(username) && user.Password.Equals(password));
                tokens.Add(token, user);
                return token;
            }
            return default;
        }
    }
}
