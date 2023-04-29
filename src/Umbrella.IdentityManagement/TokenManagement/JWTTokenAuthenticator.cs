using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Umbrella.IdentityManagement.Roles;
using Umbrella.IdentityManagement.TokenManagement.Configuration;
using Umbrella.UserManagement;
using IUserRepository = Umbrella.UserManagement.IUserRepository;

namespace Umbrella.IdentityManagement.TokenManagement
{
    /// <summary>
    /// Authentication to implement JWT absic logic
    /// </summary>
    public class JWTTokenAuthenticator : IJWTTokenAuthenticator
    {
        readonly ILogger _Logger;
        readonly JwtSettings _Settings;
        readonly IUserRepository _Repository;
        readonly IRoleRepository _RoleRepository;

        /// <summary>
        /// Default Constr
        /// </summary>
        /// <param name="key"></param>
        public JWTTokenAuthenticator(ILogger logger, IUserRepository repo, IRoleRepository roleRepo, JwtSettings settings)
        {
            this._Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._Repository = repo ?? throw new ArgumentNullException(nameof(repo));
            this._RoleRepository = roleRepo ?? throw new ArgumentNullException(nameof(roleRepo));
            this._Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            if(string.IsNullOrEmpty(settings.Secret))
                throw new ArgumentNullException(nameof(settings), $"Wrong Configuration: {nameof(settings.Secret)} cannot be null");
        }

        #region Private methods

        Microsoft.IdentityModel.Tokens.SymmetricSecurityKey GenerateSecurityKey()
        {
            // Build the key
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._Settings.Secret));
            return securityKey;
        }

        IEnumerable<Claim> GetUserClaims(string username, IEnumerable<string> userRole)
        {
            var role = userRole.FirstOrDefault() ?? "";
            if(String.IsNullOrEmpty(role))
            {
                return new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, username)
                };
            }

            // gets claim by role
            var roleDefinition = this._RoleRepository.GetByKey(role);
            if(roleDefinition == null)
                return new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, username)
                };
            return roleDefinition.Claims.Select(x => new Claim(x.Type,x.Value));
        }

        UserDto? GetUserByUsernameAndPassword(string username, string password)
        {
            if (String.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));

            return this._Repository.GetByKey(username);
        }

        #endregion

        /// <summary>
        /// Autnethicates the client by basic auth, generating back the token
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>JWT Token</returns>
        public string? Authenticate(string username, string password)
        {
            if (String.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));

            // find user by credentials. if not found, return null token
            var user = GetUserByUsernameAndPassword(username, password);
            if(user == null)
                return null;

            // build the secret key
            var securityKey = GenerateSecurityKey();

            // build the credentials with claims
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // gets the user claims
            var claims = GetUserClaims(user.Name, user.Roles);

            // create a token and returin it
            var token = new JwtSecurityToken(this._Settings.ValidIssuer,
                this._Settings.ValidAudience,
                claims,
                expires: DateTime.Now.AddMinutes(this._Settings.TokenValidityInMinutes),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        /// <summary>
        /// VAlidates the token, extracting the user id. if token is not valid, the returned id is null
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public string? ValidateToken(string token)
        {
            if (String.IsNullOrEmpty(token))
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            // Build the key
            var securityKey = GenerateSecurityKey();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    ValidateIssuer = this._Settings.ValidateIssuer,
                    ValidateAudience = this._Settings.ValidateAudience,
                    ValidAudiences = this._Settings.ValidAudience.Split(';'),
                    ValidIssuers = this._Settings.ValidIssuer.Split(';'),
                    // set clockskew to zero so token expires exactly at token expiration time
                    ClockSkew = TimeSpan.Zero 
                }, out Microsoft.IdentityModel.Tokens.SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

                // return the user id from token
                return userId;
            }
            catch
            {
                return null;
            }
        }
    }
}